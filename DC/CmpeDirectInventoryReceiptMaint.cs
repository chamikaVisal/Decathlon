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
			CmpeInventoryStatus newinventorystatus = new CmpeInventoryStatus();

			newinventorystatus.PartID = Document.Current.Partid;
			newinventorystatus.WarehouseID = Document.Current.WarehouseID;
			newinventorystatus.LocationID = Document.Current.Locationid;

			CmpeInventoryStatus check = CheckExistingInventory();

			CmpeInventoryAllocation newallocation = new CmpeInventoryAllocation();

			newallocation.PartID = Document.Current.Partid;

			CmpeInventoryAllocation checkallocation = CheckExistingallocation();

			CmpeInventoryStatus NewqtyInHand = PXSelectGroupBy<CmpeInventoryStatus,
			Where<CmpeInventoryStatus.partid, Equal<Required<CmpeDirectInventoryReceipt.partid>>>,
			Aggregate<GroupBy<CmpeInventoryStatus.partid, Sum<CmpeInventoryStatus.quantity>>>>.Select(this, Document.Current.Partid);

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
				newallocation.Quantityinhand = Document.Current.Quantity;
				InventoryAllocationDetails.Insert(newallocation);
			}
			else
			{
				newallocation.Quantityinhand = (int?)(Document.Current.Quantity + NewqtyInHand.Quantity);
				newallocation.Availableforsale = 0;
				newallocation.Reservedquantity = 0;
				InventoryAllocationDetails.Update(newallocation);
			}
			CmpeDirectInventoryReceipt current = new CmpeDirectInventoryReceipt();
			current = Document.Current;

			current.ReleasedStatus = true;
			Document.Update(current);
			Actions.PressSave();
		}
		#endregion

		#region Event Handlers
		protected virtual void _(Events.RowSelected<CmpeDirectInventoryReceipt> e)
		{
			CmpeDirectInventoryReceipt row = e.Row;

			if (Document.Current.ReleasedStatus == true)
			{
				ReleaseReceipt.SetEnabled(false);
			}

		}
		#endregion

		#region Methods
		private CmpeInventoryStatus CheckExistingInventory()
		{
			CmpeInventoryStatus newstatus = CmpeInventoryStatus.PK.Find(this, Document.Current.Partid, Document.Current.WarehouseID, Document.Current.Locationid);

			return newstatus;
		}

		private CmpeInventoryAllocation CheckExistingallocation()
		{
			CmpeInventoryAllocation newallocation = CmpeInventoryAllocation.PK.Find(this, Document.Current.Partid);

			return newallocation;
		}
		#endregion

	}
}
