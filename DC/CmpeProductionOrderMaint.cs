using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using System.Collections;
using PX.Common;
using System.Linq;
using static PX.Objects.DC.Descriptor.Constants;
using Messages = PX.Objects.DC.Descriptor.Messages;
using PX.Data.WorkflowAPI;

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

		public PXAction<CmpeProductionOrder> Release;
		[PXButton]
		[PXUIField(DisplayName = "Release")]
		protected virtual IEnumerable release(PXAdapter adapter)
		{
			return adapter.Get();
		}

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

			return adapter.Get();
		}
		

		public PXAction<CmpeProductionOrder> ReceiveShopOrder;
		[PXButton]
		[PXUIField(DisplayName = "Receive Shop Order", Enabled = true)]
		protected virtual IEnumerable receiveShopOrder(PXAdapter adapter)
		{
			try
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

					dialogBoxGraph.OrderDetails.Current = OrderDetails.Current;

					throw new PXPopupRedirectException(dialogBoxGraph, "Receive Shop Order");
				}
			}
			finally
			{
				CmpeProductionOrderMaint grp = PXGraph.CreateInstance<CmpeProductionOrderMaint>();
				grp.OrderDetails.Cache.Clear();
				grp.OrderDetails.View.RequestRefresh();
			}
		
			return adapter.Get();
		}
		#endregion

		#region Events


		protected virtual void _(Events.FieldUpdated<CmpeProductionOrder,CmpeProductionOrder.lotSize> e)
		{
			foreach (CmpeProductStructure item in BOMDetails.Select())
			{
				CmpeProductStructure copy = (CmpeProductStructure)BOMDetails.Cache.CreateCopy(item);
				object totalQuantity = e.Row.LotSize * copy.Quantity;

				BOMDetails.Cache.RaiseFieldUpdating<CmpeProductStructure.totalQuantity>(copy, ref totalQuantity);

				copy.TotalQuantity = (int)totalQuantity;
				BOMDetails.Update(copy);
			}

			SetQuantityAvailability();
		}

		protected virtual void _(Events.FieldUpdated<CmpeProductStructure, CmpeProductStructure.totalQuantity> e)
		{

			CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
				Where<CmpeInventoryAllocation.partID, Equal<Required<CmpeProductStructure.partID
					>>>>.Select(this, e.Row.PartID);

			if (e.Row.TotalQuantity > inventoryitem.QuantityInHand)
			{
				BOMDetails.Cache.RaiseExceptionHandling("TotalQuantity", e.Row, e.Row.TotalQuantity, new PXException(Messages.NoQuantity, typeof(CmpeProductStructure.quantity)));
			}
			else if (inventoryitem.QuantityInHand == null)
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
			if (row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Reserved || row.ProductionOrderStatus.Trim() == ProductionOrderStatuses.Closed)
			{
				Delete.SetEnabled(false);
			}
		}
		#endregion

		#region WorlflowEventHandler
		public PXWorkflowEventHandler<CmpeProductionOrder, CmpeProductionOrderAllocation> OnSaveReceiveStock;
		#endregion

		public void ReduceStock(PXResultset<CmpeProductStructure> productStructure)
		{
			try
			{
				foreach (CmpeProductStructure bomitem in productStructure)
				{
					CmpeInventoryAllocation inventoryitem = PXSelect<CmpeInventoryAllocation,
					Where<CmpeInventoryAllocation.partID, Equal<Required<CmpeProductStructure.partID
						>>>>.Select(this, bomitem.PartID);

					PXResultset<CmpeInventoryStatus> locations = PXSelect<CmpeInventoryStatus,
					Where<CmpeInventoryStatus.partID, Equal<Required<CmpeProductStructure.partID>>>, OrderBy<Desc<CmpeInventoryStatus.quantity>>
					>.Select(this, bomitem.PartID);


					foreach (CmpeInventoryStatus location in locations)
					{
						if ((int)location.Quantity >= bomitem.TotalQuantity)
						{
							inventoryitem.QuantityInHand -= bomitem.TotalQuantity;
							inventoryitem.ReservedQuantity += bomitem.TotalQuantity;
							location.Quantity -= bomitem.TotalQuantity;

							InventoryAllocation.Update(inventoryitem);
							Inventory.Update(location);

							Actions.PressSave();

							break;
						}
						else if ((int)location.Quantity < bomitem.TotalQuantity)
						{
							location.Quantity -= location.Quantity;
							inventoryitem.QuantityInHand -= (int)location.Quantity;
							inventoryitem.ReservedQuantity += (int)location.Quantity;

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
								.InnerJoin<CmpeInventoryStatus>.On<CmpeInventoryStatus.partID.IsEqual<CmpeProductStructure.partID>>
								.Where<CmpeProductStructure.productID.IsEqual<CmpeProductionOrder.productNumber.FromCurrent>>
								.AggregateTo<GroupBy<CmpeInventoryStatus.partID>, Sum<CmpeInventoryStatus.quantity>>
								.View.ReadOnly(this).Select()
								.ToDictionary(e => e.GetItem<CmpeInventoryStatus>().PartID, e => e.GetItem<CmpeInventoryStatus>().Quantity);

			foreach (CmpeProductStructure item in BOMDetails.Select())
			{
				item.Available = item.TotalQuantity <= result[item.PartID];
			}
		}

	}

}
