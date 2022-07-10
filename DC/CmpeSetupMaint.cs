using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	public class CmpeSetupMaint : PXGraph<CmpeSetupMaint>
	{
		public PXSave<CmpeSetup> Save;
		public PXCancel<CmpeSetup> Cancel;

		public SelectFrom<CmpeSetup>.View Setup;

	}
}
