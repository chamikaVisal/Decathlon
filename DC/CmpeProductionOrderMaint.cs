using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using System.Collections;
using PX.Common;
using System.Linq;
using static PX.Objects.DC.Descriptor.Constants;
using Messages = PX.Objects.DC.Descriptor.Messages;

namespace PX.Objects.DC
{
	public class CmpeProductionOrderMaint : PXGraph<CmpeProductionOrderMaint, CmpeProductionOrder>
	{
		#region Views
		public SelectFrom<CmpeProductionOrder>.View OrderDetails;

		public SelectFrom<CmpeProductStructure>.
		Where<CmpeProductStructure.productID.
		IsEqual<CmpeProductionOrder.productNumber.FromCurrent>>.View BOMDetails;

		public SelectFrom<CmpeInventoryAllocation>.View InventoryAllocation;
		public SelectFrom<CmpeInventoryStatus>.View Inventory;

		public PXSetup<CmpeSetup> NumberingSeqSetup;
		#endregion

		#region Graph constructor
		public CmpeProductionOrderMaint()
		{
			CmpeSetup setup = NumberingSeqSetup.Current;
		}
		#endregion

		#region Actions
		public PXAction<CmpeProductionOrder> IssueMaterial;
		[PXButton]
		[PXUIField(DisplayName = "Issue Material", Enabled = true)]
		protected virtual IEnumerable issueMaterial(PXAdapter adapter)
		{
			var bomItems = BOMDetails.Select();

			PXLongOperation.StartOperation(this, delegate ()
			{
				var productionordermaint = PXGraph.CreateInstance<CmpeProductionOrderMaint>();
				productionordermaint.ReduceStock(bomItems);
			});

			//CmpeProductionOrder prod = OrderDetails.Current;
			//prod.ProductionOrderStatus = ProductionOrderStatuses.Reserved;
			//OrderDetails.Update(prod);
			Save.Press();

			return adapter.Get();
		}
		

		public PXAction<CmpeProductionOrder> ReceiveShopOrder;
		[PXButton]
		[PXUIField(DisplayName = "Receive Shop Order", Enabled = true)]
		protected virtual IEnumerable receiveShopOrder(PXAdapter adapter)
		{
			if (OrderDetails.Current != null)
			{
				var dialogBoxGraph = CreateInstance<CmpeReceiveOrderEntry>();

				dialogBoxGraph.Document.Cache.Clear();
				dialogBoxGraph.Document.Current = SelectFrom<CmpeProductionOrderAllocation>.View.Select(dialogBoxGraph);
				dialogBoxGraph.Document.Current.Quantity = OrderDetails.Current.LotSize;
				dialogBoxGraph.Document.Current.ProductID = OrderDetails.Current.ProductNumber;

				dialogBoxGraph.Document.Current.WarehouseID = null;
				dialogBoxGraph.Document.Current.LocationID = null;
				dialogBoxGraph.Document.UpdateCurrent();

				//dialogBoxGraph.OrderDetails.Current = OrderDetails.Current;
				//dialogBoxGraph.OrderDetails.Current.ProductionOrderStatus = ProductionOrderStatuses.Closed;

				throw new PXPopupRedirectException(dialogBoxGraph, "Receive Shop Order");
			}

			return adapter.Get();
		}
		#endregion

		#region Events

		protected virtual void _(Events.RowPersisting<CmpeProductionOrder> e)
		{
			CmpeProductionOrder row = e.Row;

			if (row.ProductionOrderStatus == ProductionOrderStatuses.Not_Set)
			{
				row.ProductionOrderStatus = ProductionOrderStatuses.Released;
				OrderDetails.Update(row);
			}
		}

		protected virtual void _(Events.FieldUpdated<CmpeProductionOrder,CmpeProductionOrder.lotSize> e)
		{
			foreach (CmpeProductStructure item in BOMDetails.Select())
			{
				//item.TotalQuantity = e.Row.LotSize * item.Quantity;
				CmpeProductStructure copy = (CmpeProductStructure)BOMDetails.Cache.CreateCopy(item);
				object TotalQuantity = e.Row.LotSize * copy.Quantity;

				BOMDetails.Cache.RaiseFieldUpdating<CmpeProductStructure.totalQuantity>(copy, ref TotalQuantity);

				copy.TotalQuantity = (int)TotalQuantity;
				BOMDetails.Update(copy);
			}

			SetQuantityAvailability();
		}

		protected virtual void _(Events.FieldUpdated<CmpeProductStructure, CmpeProductStructure.totalQuantity> e)
		{

			CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
				Where<CmpeInventoryAllocation.partid, Equal<Required<CmpeProductStructure.partID
					>>>>.Select(this, e.Row.PartID);

			if (e.Row.TotalQuantity > inventoryitem.Quantityinhand)
			{
				BOMDetails.Cache.RaiseExceptionHandling("TotalQuantity", e.Row, e.Row.TotalQuantity, new PXException(Messages.NoQuantity, typeof(CmpeProductStructure.quantity)));
				// Acuminator disable once PX1070 UiPresentationLogicInEventHandlers [Justification]
				Save.SetEnabled(false);
			}
			else if (inventoryitem.Quantityinhand == null)
			{
				BOMDetails.Cache.RaiseExceptionHandling("TotalQuantity", e.Row, e.Row.TotalQuantity, new PXException(Messages.QuantityNotFound));
			}
		}

		protected virtual void _(Events.RowSelected<CmpeProductStructure> e)
		{
			CmpeProductionOrder copy = OrderDetails.Current;

			if (copy.LotSize != null)
			{
				foreach (CmpeProductStructure item in BOMDetails.Select())
				{
					object TotalQuantity = item.Quantity * copy.LotSize;
					item.TotalQuantity = (int)TotalQuantity;
				}
			}
			else
			{
				foreach (CmpeProductStructure item in BOMDetails.Select())
				{
					item.TotalQuantity = 0;
				}
			}

			SetQuantityAvailability();
		}

		protected virtual void _(Events.RowSelected<CmpeProductionOrder> e)
		{
			CmpeProductionOrder row = e.Row;
			if (row != null)
			{
				PXUIFieldAttribute.SetVisible<CmpeProductionOrder.lotSize>(e.Cache, e.Row, row.ProductNumber.HasValue);
			}

			//if (row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Released)
			//{
			//	IssueMaterial.SetEnabled(true);
			//	ReceiveShopOrder.SetEnabled(false);
			//}
			//if (row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Not_Set || OrderDetails.Current.ProductionOrderStatus == ProductionOrderStatuses.Not_Set)
			//{
			//	IssueMaterial.SetEnabled(false);
			//	ReceiveShopOrder.SetEnabled(false);
			//}
			if (row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Reserved || row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Closed)
			{
				//IssueMaterial.SetEnabled(false);
				//ReceiveShopOrder.SetEnabled(true);
				Delete.SetEnabled(false);
			}
			//if (row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Closed || OrderDetails.Current.ProductionOrderStatus == ProductionOrderStatuses.Closed)
			//{
			//	IssueMaterial.SetEnabled(false);
			//	ReceiveShopOrder.SetEnabled(false);
			//	Delete.SetEnabled(false);

			//	PXUIFieldAttribute.SetEnabled<CmpeProductionOrder.orderID>(e.Cache, row, false);
			//	PXUIFieldAttribute.SetEnabled<CmpeProductionOrder.productionOrderDate>(e.Cache, row, false);
			//	PXUIFieldAttribute.SetEnabled<CmpeProductionOrder.requestedDate>(e.Cache, row, false);
			//	PXUIFieldAttribute.SetEnabled<CmpeProductionOrder.productNumber>(e.Cache, row, false);
			//	PXUIFieldAttribute.SetEnabled<CmpeProductionOrder.lotSize>(e.Cache, row, false);
			//}

		}
		#endregion

		public void ReduceStock(PXResultset<CmpeProductStructure> productStructure)
		{
			try
			{
				foreach (CmpeProductStructure bomitem in productStructure)
				{
					/*
					 * 1. select current bom item (CmpeProductStructure) Ex : PartID = 1
					 * 2. check for the quantity in hand for the bom item (CmpeInventoryAllocation) Ex : ParID = 1, QuantityInhand = 50
					 * 3. check for all the locations corresponding to the part selected in step 1 (CmpeInventoryStatus)
					 */
					CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
					Where<CmpeInventoryAllocation.partid, Equal<Required<CmpeProductStructure.partID
						>>>>.Select(this, bomitem.PartID); // results the row which has the summarized amount for the part in the product structure selected

					PXResultset<CmpeInventoryStatus> locations = PXSelect<CmpeInventoryStatus,
					Where<CmpeInventoryStatus.partid, Equal<Required<CmpeProductStructure.partID>>>, OrderBy<Desc<CmpeInventoryStatus.quantity>>
					>.Select(this, bomitem.PartID);


					foreach (CmpeInventoryStatus location in locations)
					{
						if ((int)location.Quantity >= bomitem.TotalQuantity)
						{
							inventoryitem.Quantityinhand -= bomitem.TotalQuantity;
							inventoryitem.Reservedquantity += bomitem.TotalQuantity;
							location.Quantity -= bomitem.TotalQuantity;

							InventoryAllocation.Update(inventoryitem);
							Inventory.Update(location);

							Actions.PressSave();

							break;
						}
						else if ((int)location.Quantity < bomitem.TotalQuantity)
						{
							location.Quantity -= location.Quantity;
							inventoryitem.Quantityinhand -= (int)location.Quantity;
							inventoryitem.Reservedquantity += (int)location.Quantity;

							bomitem.TotalQuantity = (int)(bomitem.TotalQuantity - location.Quantity);

							InventoryAllocation.Update(inventoryitem);
							Inventory.Update(location);
							Actions.PressSave();

							if (bomitem.TotalQuantity != 0)
							{
								continue;
							}
							else
							{
								break;
							}

						}
					}
				}
			}
			catch
			{
				throw new PXException(Messages.UnexpectedError);
			}
			
		}

		public void SetQuantityAvailability()
		{
			var result = new SelectFrom<CmpeProductStructure>
								.InnerJoin<CmpeInventoryStatus>.On<CmpeInventoryStatus.partid.IsEqual<CmpeProductStructure.partID>>
								.Where<CmpeProductStructure.productID.IsEqual<CmpeProductionOrder.productNumber.FromCurrent>>
								.AggregateTo<GroupBy<CmpeInventoryStatus.partid>, Sum<CmpeInventoryStatus.quantity>>
								.View.ReadOnly(this).Select()
								.ToDictionary(e => e.GetItem<CmpeInventoryStatus>().PartID, e => e.GetItem<CmpeInventoryStatus>().Quantity);
				//.ToDictionary(e => e.GetItem<CmpeInventoryStatus>().PartID, e => e.GetItem<CmpeInventoryStatus>().Quantity - e.GetItem<CmpeInventoryStatus>().reserved);

			foreach (CmpeProductStructure item in BOMDetails.Select())
			{
				item.Available = item.TotalQuantity <= result[item.PartID];
			}
		}

	}

}
