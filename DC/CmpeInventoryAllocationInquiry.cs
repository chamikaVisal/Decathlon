using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	public class CmpeInventoryAllocationInquiry : PXGraph<CmpeInventoryAllocationInquiry>
	{
		public SelectFrom<CmpeInventory>.View InventoryDetails;
	}
}
