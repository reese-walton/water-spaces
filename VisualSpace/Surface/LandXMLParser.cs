using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Xml;

namespace VisualSpace;

static class LandXMLParser
{

    public static MeshGeometry3D ParseSurface(System.IO.Stream xmlStream)
    {

        XmlDocument xmlDoc = new();
        xmlDoc.Load(xmlStream);
        XmlNode landXml = xmlDoc.DocumentElement;

        XmlNamespaceManager ns = new(xmlDoc.NameTable);
        ns.AddNamespace("l", landXml.NamespaceURI);

        if (landXml.SelectSingleNode("//l:Surface", ns) is XmlNode surfDef)
        {
            MeshGeometry3D mesh = new();
            Dictionary<uint, int> pointIdMap = new();

            XmlNode points = surfDef.SelectSingleNode("//l:Pnts", ns);
            foreach (XmlNode p in points.SelectNodes("l:P", ns))
            {
                uint pointId = uint.Parse(p.Attributes["id"]?.Value);
                var pointCoords = p.InnerText.Split(' ').Select(double.Parse).ToList();
                if (pointCoords.Count != 3)
                {
                    throw new Exception($"Invalid LandXML - Point has {pointCoords.Count} coordinates");
                }

                // LandXML coordinate are represented as "northing easting elevation"
                var point = new Point3D(pointCoords[1], pointCoords[0], pointCoords[2]);
                pointIdMap.Add(pointId, pointIdMap.Count);
                mesh.Positions.Add(point);
            }

            XmlNode faces = surfDef.SelectSingleNode("//l:Faces", ns);
            foreach (XmlNode f in faces.SelectNodes("l:F", ns))
            {
                List<uint> pointIds = f.InnerText.Split(' ').Select(uint.Parse).ToList();
                if (pointIds.Count != 3)
                {
                    throw new Exception($"Invalid LandXML - Face has {pointIds.Count} points");
                }
                // let WPF calculate vertex normals as average of connected planes (will result in smooth edges instead of visible lines)
                mesh.TriangleIndices.Add(pointIdMap[pointIds[0]]);
                mesh.TriangleIndices.Add(pointIdMap[pointIds[1]]);
                mesh.TriangleIndices.Add(pointIdMap[pointIds[2]]);
            }
            return mesh;
        }
        else
        {
            throw new Exception($"No surface definition found");
        }
    }
}
