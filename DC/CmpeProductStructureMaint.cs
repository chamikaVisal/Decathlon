using System;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	public class CmpeProductStructureMaint : PXGraph<CmpeProductStructureMaint,CmpeProductStructure>
	{
		public SelectFrom<CmpePart>.View ProductDetails;

		public SelectFrom<CmpeProductStructure>.
			Where<CmpeProductStructure.productID.
				IsEqual<CmpePart.partID.FromCurrent>>.View PartDetails;
	}
}
