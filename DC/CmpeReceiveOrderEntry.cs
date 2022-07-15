using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using PX.Objects.DC.Descriptor;
using System.Collections;
using System.Linq;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC
{
	public class CmpeReceiveOrderEntry : PXGraph<CmpeReceiveOrderEntry, CmpeProductionOrderAllocation>
	{
		//POP UP
		#region Views
		public SelectFrom<CmpeProductionOrderAllocation>.View Document;
		public SelectFrom<CmpeProductionOrder>.View OrderDetails;
		public SelectFrom<CmpeInventoryStatus>.View Inventory;
		#endregion

		#region Event
		protected virtual void _(Events.RowPersisting<CmpeProductionOrderAllocation> e)
		{
			CmpeProductionOrderAllocation row = e.Row;

			CmpeInventoryStatus inventory = CmpeInventoryStatus.PK.Find(this, row.ProductID, row.WarehouseID, row.LocationID);

			CmpeInventoryStatus newinventory = new CmpeInventoryStatus();
			newinventory.PartID = row.ProductID;
			newinventory.LocationID = row.LocationID;
			newinventory.WarehouseID = row.WarehouseID;
			newinventory.Quantity = row.Quantity;

			if (inventory == null)
			{
				Inventory.Insert(newinventory);
				Inventory.Cache.Persist(PXDBOperation.Insert);
			}
			else
			{
				newinventory.Quantity += row.Quantity;
				Inventory.Update(newinventory);
				Inventory.Cache.Persist(PXDBOperation.Update);
			}

			OrderDetails.UpdateCurrent();
		}

		protected virtual void _(Events.RowPersisted<CmpeProductionOrderAllocation> e)
		{
			OrderDetails.Current.ProductionOrderStatus = ProductionOrderStatuses.Closed;
			OrderDetails.UpdateCurrent();

			// Acuminator disable once PX1045 PXGraphCreateInstanceInEventHandlers [Justification]
			CmpeProductionOrderMaint grp = PXGraph.CreateInstance<CmpeProductionOrderMaint>();
			grp.OrderDetails.Cache.Clear();
			grp.OrderDetails.View.RequestRefresh();

			CmpeProductionOrderAllocation.Events.Select(ev => ev.SaveDocument).FireOn(this, e.Row);
		}
		#endregion

	}
}
