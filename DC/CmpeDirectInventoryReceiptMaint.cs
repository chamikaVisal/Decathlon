using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using System;
using Messages = PX.Objects.DC.Descriptor.Messages;

namespace PX.Objects.DC
{
	public class CmpeDirectInventoryReceiptMaint : PXGraph<CmpeDirectInventoryReceiptMaint, CmpeDirectInventoryReceipt>
	{
		//pop up

		public SelectFrom<CmpeDirectInventoryReceipt>.View Document;
		public SelectFrom<CmpeInventoryStatus>.View InventoryStatus;
		public SelectFrom<CmpeInventoryAllocation>.View InventoryAllocationDetails;

		#region Actions
		public PXAction<CmpeDirectInventoryReceipt> ReleaseReceipt;
		[PXButton(CommitChanges = true)]
		[PXUIField(DisplayName = "Release Receipt", Enabled = true)]

		public virtual void releaseReceipt()
		{
			CmpeInventoryStatus newinventorystatus = new CmpeInventoryStatus();;

			newinventorystatus.PartID = Document.Current.PartID;
			newinventorystatus.WarehouseID = Document.Current.WarehouseID;
			newinventorystatus.LocationID = Document.Current.LocationID;

			CmpeInventoryStatus check = CheckExistingInventory();

			CmpeInventoryAllocation newallocation = new CmpeInventoryAllocation();

			newallocation.PartID = Document.Current.PartID;

			CmpeInventoryAllocation checkallocation = CheckExistingallocation();

			CmpeInventoryStatus NewqtyInHand = PXSelectGroupBy<CmpeInventoryStatus,
			Where<CmpeInventoryStatus.partID, Equal<Required<CmpeDirectInventoryReceipt.partID>>>,
			Aggregate<GroupBy<CmpeInventoryStatus.partID, Sum<CmpeInventoryStatus.quantity>>>>.Select(this, Document.Current.PartID);

			if (check == null)
			{
				newinventorystatus.Quantity = Document.Current.Quantity;
				InventoryStatus.Insert(newinventorystatus);

			}
			else
			{
				newinventorystatus.Quantity = check.Quantity + Document.Current.Quantity;
				InventoryStatus.Update(newinventorystatus);
			}

			if (checkallocation == null)
			{
				newallocation.QuantityInHand = Document.Current.Quantity;
				InventoryAllocationDetails.Insert(newallocation);
			}
			else
			{
				newallocation.QuantityInHand = (int?)(Document.Current.Quantity + NewqtyInHand.Quantity);
				newallocation.AvailableForSale = 0;
				newallocation.ReservedQuantity = 0;
				InventoryAllocationDetails.Update(newallocation);
			}
			
			Document.Current.ReleasedStatus = true;
			Document.UpdateCurrent();
			Actions.PressSave();
		}
		#endregion

		#region Event Handlers
		protected virtual void _(Events.RowSelected<CmpeDirectInventoryReceipt> e)
		{
			CmpeDirectInventoryReceipt row = e.Row; 

			if (row.ReleasedStatus == true)
			{
				ReleaseReceipt.SetEnabled(false);
			}

		}
		#endregion

		#region Methods
		private CmpeInventoryStatus CheckExistingInventory()
		{
			CmpeInventoryStatus newstatus = CmpeInventoryStatus.PK.Find(this, Document.Current.PartID, Document.Current.WarehouseID, Document.Current.LocationID);

			return newstatus;
		}

		private CmpeInventoryAllocation CheckExistingallocation()
		{
			CmpeInventoryAllocation newallocation = CmpeInventoryAllocation.PK.Find(this, Document.Current.PartID);

			return newallocation;
		}
		#endregion

	}

}
