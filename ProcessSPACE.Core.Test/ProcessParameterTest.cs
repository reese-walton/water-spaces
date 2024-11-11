using System.Collections;
using ProcessSPACE.Core.Models;

using static ProcessSPACE.Core.Models.BaseParameter;
using static ProcessSPACE.Core.Models.ProcessParameter;

namespace ProcessSPACE.Core.Test;

public class ProcessParameterTest
{
    [TestCaseSource(typeof(ProcessParameterTest), nameof(BaseParametersTestData))]
    public BaseParameter[] BaseParameters(ProcessParameter p) => p.ToBaseParameters();

    public static IEnumerable BaseParametersTestData {
        get {
            yield return new TestCaseData(SSVol).Returns(new []{InpSSVol});
            yield return new TestCaseData(SSInt).Returns(new []{InpSSInt});
            yield return new TestCaseData(SSTot).Returns(new []{InpSSVol, InpSSInt});
            yield return new TestCaseData(BODSol).Returns(new []{InpBODSol});
            yield return new TestCaseData(BODPrt).Returns(new []{InpBODPrt});
            yield return new TestCaseData(BODTot).Returns(new []{InpBODSol, InpBODPrt});
        }
    }
}