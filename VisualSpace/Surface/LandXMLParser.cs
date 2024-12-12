using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml;
using HelixToolkit.Wpf;

namespace VisualSpace;

static class LandXMLParser
{
    private class LandXMLPointCollection : KeyedCollection<int, LandXMLPoint>
    {
        protected override int GetKeyForItem(LandXMLPoint item) => item.Id;
    }

    private record struct LandXMLPoint(int Id, Point3D Coordinates);

    public static MeshGeometry3D ParseSurface(string xmlPath)
    {
        LandXMLPointCollection pointList = new();

        XmlDocument xmlDoc = new();
        xmlDoc.Load(xmlPath);
        XmlNode landXml = xmlDoc.DocumentElement;

        XmlNamespaceManager ns = new(xmlDoc.NameTable);
        ns.AddNamespace("l", landXml.NamespaceURI);

        if (landXml.SelectSingleNode("//l:Surface", ns) is XmlNode surfDef)
        {
            MeshBuilder builder = new();
            XmlNode points = surfDef.SelectSingleNode("//l:Pnts", ns);
            foreach (XmlNode p in points.SelectNodes("l:P", ns))
            {
                var pointId = int.Parse(p.Attributes["id"]?.Value);
                var pointCoords = p.InnerText.Split(' ').Select(double.Parse).ToList();
                if (pointCoords.Count != 3)
                {
                    throw new Exception($"Invalid LandXML - Point has {pointCoords.Count} coordinates");
                }
                // LandXML coordinate are represented as "northing easting elevation"
                pointList.Add(new LandXMLPoint(pointId, new Point3D(pointCoords[1], pointCoords[0], pointCoords[2])));
            }

            XmlNode faces = surfDef.SelectSingleNode("//l:Faces", ns);
            foreach (XmlNode f in faces.SelectNodes("l:F", ns))
            {
                List<int> pointIds = f.InnerText.Split(' ').Select(int.Parse).ToList();
                if (pointIds.Count != 3)
                {
                    throw new Exception($"Invalid LandXML - Face has {pointIds.Count} points");
                }

                builder.AddTriangle(pointList[pointIds[0]].Coordinates, pointList[pointIds[1]].Coordinates, pointList[pointIds[2]].Coordinates);
            }
            return builder.ToMesh(true);
        }
        else
        {
            throw new Exception($"No surface definition in file {xmlPath}");
        }
    }
}
