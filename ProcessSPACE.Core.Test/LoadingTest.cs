using ProcessSPACE.Core.Models;
namespace ProcessSPACE.Core.Test;

public class LoadingTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Basic()
    {
        double vss = 10.0;
        double iss = 20.0;

        double nh4 = 15.0;
        double son = 10.0;
        double pon = 10.0;

        ProcessLoad load = ProcessLoad.Empty() with
        {
            VolSS = vss,
            InertSS = iss,

            AmmN = nh4,
            SolOrgN = son,
            PartOrgN = pon,
        };

        Assert.Multiple(() =>
        {
            Assert.That(load.TotSS, Is.EqualTo(vss + iss));
            Assert.That(load.TotOrgN, Is.EqualTo(son + pon));
            Assert.That(load.TotKN, Is.EqualTo(nh4 + son + pon));
        });
    }
}
