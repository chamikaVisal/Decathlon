using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using PX.Objects.DC.Descriptor;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC
{
	public class CmpeSalesOrderEntry : PXGraph<CmpeSalesOrderEntry, CmpeSalesOrder>
	{
		#region Views
		public SelectFrom<CmpeSalesOrder>.View CustomerOrderDetails;

		public SelectFrom<CmpeCustomerOrderPartDetails>
			.Where<CmpeCustomerOrderPartDetails.customerOrder.IsEqual<CmpeSalesOrder.customerOrder.FromCurrent>>
			.View CustomerOrderPartDetails;

		public SelectFrom<CmpeCustomerOrderNoPartDetails>
			.Where<CmpeCustomerOrderNoPartDetails.customerOrder.IsEqual<CmpeSalesOrder.customerOrder.FromCurrent>>
			.View CustomerOrderNoPartDetails;

		public SelectFrom<CmpeInventoryAllocation>.View InventoryAllocation;

		public SelectFrom<CmpeInventoryStatus>.View Inventory;

		public PXSetup<CmpeSetup> NumberingSeqSetup;
		#endregion

		#region Graph constructor
		public CmpeSalesOrderEntry()
		{
			CmpeSetup setup = NumberingSeqSetup.Current;
		}
		#endregion

		#region Actions

		public PXAction<CmpeSalesOrder> ReleaseOrder;
		[PXButton]
		[PXUIField(DisplayName = "Release", Enabled = true)]
		protected virtual void releaseOrder()
		{
			CmpeSalesOrder currentorder = CustomerOrderDetails.Current;
			currentorder.Status = CustomerOrderStatus.COReleased;
			CustomerOrderDetails.UpdateCurrent();
			Actions.PressSave();
		}

		public PXAction<CmpeSalesOrder> CancelOrder;
		[PXButton]
		[PXUIField(DisplayName = "Cancel", Enabled = true)]
		protected virtual void cancelOrder()
		{
			if (IsAnyItemNotDelivered() && CustomerOrderDetails.Current.Status.Trim() == CustomerOrderStatus.COReleased || CustomerOrderDetails.Current.Status.Trim() == CustomerOrderStatus.COPlanned)
			{
				CmpeSalesOrder currentorder = CustomerOrderDetails.Current;
				currentorder.Status = CustomerOrderStatus.COCancelled;
				CustomerOrderDetails.UpdateCurrent();
				Actions.PressSave();
			}
			ChangeItemStatusToCancelled();

			Actions.PressSave();
		}

		public PXAction<CmpeSalesOrder> DeliverOrder;
		[PXButton]
		[PXUIField(DisplayName = "Deliver", Enabled = true)]
		protected virtual void deliverOrder()
		{
			if (CustomerOrderDetails.Current.Status.Trim() == CustomerOrderStatus.COReleased)
			{
				ChangeStatusDelivered();
			}
		}

		#endregion


		#region Events
		protected void _(Events.RowSelected<CmpeSalesOrder> e)
		{
			CmpeSalesOrder row = e.Row;

			if (row != null)
			{
				if (row.Status.Trim() == CustomerOrderStatus.COPlanned)
				{
					ReleaseOrder.SetEnabled(true);
					CancelOrder.SetEnabled(true);
					DeliverOrder.SetEnabled(false);
				}
				if (row.Status.Trim() == CustomerOrderStatus.Not_Set)
				{
					ReleaseOrder.SetEnabled(false);
					CancelOrder.SetEnabled(false);
					DeliverOrder.SetEnabled(false);
				}
				if (row.Status.Trim() == CustomerOrderStatus.COReleased)
				{
					ReleaseOrder.SetEnabled(false);
					DeliverOrder.SetEnabled(true);
				}
				if (row.Status.Trim() == CustomerOrderStatus.COCancelled || row.Status.Trim() == CustomerOrderStatus.COClosed)
				{
					ReleaseOrder.SetEnabled(false);
					CancelOrder.SetEnabled(false);
					DeliverOrder.SetEnabled(false);
				}
			}
		}

		protected void _(Events.RowSelected<CmpeCustomerOrderPartDetails> e)
		{
			CmpeCustomerOrderPartDetails row = e.Row;

			if (row != null)
			{
				if (row.Status.Trim() == CustomerOrderItemDetailsStatus.COItemDelivered)
				{
					DeliverOrder.SetEnabled(false);
					CancelOrder.SetEnabled(false);
				}
				if (!IsAnyItemNotDelivered() && row.Status.Trim() != CustomerOrderItemDetailsStatus.COItemRequired)
				{
					CustomerOrderDetails.Current.Status = CustomerOrderStatus.COClosed;
					CustomerOrderDetails.UpdateCurrent();
					// Acuminator disable once PX1043 SavingChangesInEventHandlers [Justification]
					CustomerOrderDetails.Cache.Persist(PXDBOperation.Update);
				}
			}
		}
		protected virtual void _(Events.RowPersisting<CmpeSalesOrder> e)
		{
			CmpeSalesOrder row = e.Row;

			if (row.Status == CustomerOrderStatus.Not_Set)
			{
				row.Status = CustomerOrderStatus.COPlanned;
				CustomerOrderDetails.Update(row);
			}
		}

		protected void _(Events.FieldUpdated<CmpeCustomerOrderPartDetails, CmpeCustomerOrderPartDetails.qty> e)
		{
			CmpeCustomerOrderPartDetails row = e.Row;

			if (row != null)
			{
				CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
				Where<CmpeInventoryAllocation.partID, Equal<Required<CmpeCustomerOrderPartDetails.partID
					>>>>.Select(this, row.PartID);

				if (row.Qty > inventoryitem.AvailableForSale)
				{
					e.Cache.RaiseExceptionHandling<CmpeCustomerOrderPartDetails.qty>(row, row.Qty, new PXException(Messages.NoSufficientQtyMessage));
				}
			}
		}
		#endregion

		public void ChangeStatusDelivered()
		{
			foreach (CmpeCustomerOrderPartDetails item in CustomerOrderPartDetails.Select())
			{
				CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
				Where<CmpeInventoryAllocation.partID, Equal<Required<CmpeCustomerOrderPartDetails.partID
					>>>>.Select(this, item.PartID);

				PXResultset<CmpeInventoryStatus> inventory = PXSelect<CmpeInventoryStatus,
				Where<CmpeInventoryStatus.partID, Equal<Required<CmpeCustomerOrderPartDetails.partID>>>, OrderBy<Desc<CmpeInventoryStatus.quantity>>
				>.Select(this, item.PartID);
				UpdateQuantities(item, inventoryitem, inventory);

				item.Status = CustomerOrderItemDetailsStatus.COItemDelivered;
				CustomerOrderPartDetails.Update(item);
			}
			Actions.PressSave();
		}

		private void UpdateQuantities(CmpeCustomerOrderPartDetails item, CmpeInventoryAllocation inventoryitem, PXResultset<CmpeInventoryStatus> inventory)
		{
			foreach (CmpeInventoryStatus inventoryRes in inventory)
			{
				if ((int)inventoryRes.Quantity >= item.Qty)
				{
					inventoryitem.AvailableForSale -= (int)item.Qty;
					inventoryitem.QuantityInHand -= (int)item.Qty;
					inventoryRes.Quantity -= item.Qty;

					InventoryAllocation.Update(inventoryitem);
					Inventory.Update(inventoryRes);

					Actions.PressSave();

					break;
				}
				else if ((int)inventoryRes.Quantity < item.Qty)
				{
					inventoryRes.Quantity -= inventoryRes.Quantity;
					inventoryitem.QuantityInHand -= (int)item.Qty;
					inventoryitem.AvailableForSale -= (int)item.Qty;

					item.Qty = (int)(item.Qty - inventoryRes.Quantity);

					InventoryAllocation.Update(inventoryitem);
					Inventory.Update(inventoryRes);
					Actions.PressSave();

					if (item.Qty != 0) {continue;}
					else
						break;
				}
			}
		}

		public bool IsAnyItemNotDelivered()
		{
			bool isNotDelivered = false;

			foreach (CmpeCustomerOrderPartDetails item in CustomerOrderPartDetails.Select())
			{
				if (item.Status != CustomerOrderItemDetailsStatus.COItemDelivered)
				{
					return isNotDelivered = true;
				}
			}

			return isNotDelivered;
		}
		public void ChangeItemStatusToCancelled()
		{
			foreach (CmpeCustomerOrderPartDetails item in CustomerOrderPartDetails.Select())
			{
				item.Status = CustomerOrderItemDetailsStatus.COItemCancelled;
				CustomerOrderPartDetails.Update(item);
				Actions.PressSave();
			}
		}
	}
}

