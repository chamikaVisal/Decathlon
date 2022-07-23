
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	public class CmpeProductStructureMaint : PXGraph<CmpeProductStructureMaint, CmpePart>
	{
		public SelectFrom<CmpePart>.View PartDetails;

		public SelectFrom<CmpeProductStructure>.
			Where<CmpeProductStructure.productID.
				IsEqual<CmpePart.partID.FromCurrent>>.View ProductStructureDetails;
	}
}
