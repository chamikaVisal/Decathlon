using PX.Common;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.Common.Extensions;
using PX.Objects.DC.DAC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Messages = PX.Objects.DC.Descriptor.Messages;


namespace PX.Objects.DC
{
	[PXHidden]
	public class InventoryStockfilter : IBqlTable
	{
		#region PartID
		[PXInt]
		[PXSelector(typeof(Search<CmpePart.partid>),
		typeof(CmpePart.partid),
		typeof(CmpePart.partcd),
		SubstituteKey = typeof(CmpePart.partcd))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region WarehouseID
		[PXInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseid>),
		typeof(CmpeWarehouse.warehousecd),
		typeof(CmpeWarehouse.warehousedescription),
		SubstituteKey = typeof(CmpeWarehouse.warehousecd))]
		[PXUIField(DisplayName = "Warehouse")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseid : PX.Data.BQL.BqlInt.Field<warehouseid> { }
		#endregion

		#region LocationID
		[PXInt]
		[PXSelector(typeof(Search<CmpeLocation.locationID, Where<CmpeLocation.warehouseid, Equal<Current<warehouseid>>>>),
		typeof(CmpeLocation.locationID),
		typeof(CmpeLocation.locationCD),
		SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location")]
		public virtual int? LocationID { get; set; }
		public abstract class locationid : PX.Data.BQL.BqlInt.Field<locationid> { }
		#endregion

		#region Qty
		[PXDBInt]
		[PXUIField(DisplayName = "Quantity", Enabled = false)]
		[PXUnboundDefault(0)]
		public virtual int? Qty { get; set; }
		public abstract class qty : PX.Data.BQL.BqlString.Field<qty> { }
		#endregion
	}

	public class CmpeInventoryMaint : PXGraph<CmpeInventoryMaint>
	{
		public PXCancel<InventoryStockfilter> cancel;
		public PXFilter<InventoryStockfilter> Filter;

		[PXFilterable]
		public SelectFrom<CmpeInventoryStatus>.View.ReadOnly INStatusRecords;

		protected virtual IEnumerable iNStatusRecords()
		{

			INStatusRecords.Cache.Clear();

			foreach (var item in FetchINStatusRecord().OfType<CmpeInventoryStatus>())
			{
				INStatusRecords.Cache.Hold(item);
			}

			var resultsset = INStatusRecords.Cache.Cached.RowCast<CmpeInventoryStatus>();
			var totalraw = CalculateSummaryTotal(resultsset);

			var delegateresult = new PXDelegateResult() { IsResultSorted = true };
			var sortedresult = PXView.Sort(resultsset).RowCast<CmpeInventoryStatus>();

			if (!PXView.ReverseOrder)
			{
				delegateresult.AddRange(sortedresult);
				delegateresult.Add(totalraw);
			}
			else
			{
				delegateresult.Add(totalraw);
				delegateresult.AddRange(sortedresult);
			}

			return delegateresult;
		}

		protected virtual IEnumerable FetchINStatusRecord()
		{
			var cmd = new SelectFrom<CmpeInventoryStatus>
						.InnerJoin<CmpePart>.On<CmpePart.partid.IsEqual<CmpeInventoryStatus.partid>>
						.InnerJoin<CmpeWarehouse>.On<CmpeWarehouse.warehouseid.IsEqual<CmpeInventoryStatus.warehouseid>>
						.InnerJoin<CmpeLocation>
							.On<CmpeLocation.warehouseid.IsEqual<CmpeWarehouse.warehouseid>
							.And<CmpeLocation.locationID.IsEqual<CmpeInventoryStatus.locationid>>>.View(this);

			if (Filter.Current.PartID != null)
			{
				cmd.WhereAnd<Where<CmpePart.partid.IsEqual<InventoryStockfilter.partid.FromCurrent>>>();
			}

			if (Filter.Current.WarehouseID != null)
			{
				cmd.WhereAnd<Where<CmpeWarehouse.warehouseid.IsEqual<InventoryStockfilter.warehouseid.FromCurrent>>>();

				if (Filter.Current.LocationID != null)
				{
					cmd.WhereAnd<Where<CmpeLocation.locationID.IsEqual<InventoryStockfilter.locationid.FromCurrent>>>();
				}
			}

			List<Type> fieldsScope = new List<Type>(new Type[]{
				typeof(CmpeInventoryStatus.inventorystatusid),
				typeof(CmpeInventoryStatus.quantity),
				typeof(CmpePart.partcd),
				typeof(CmpeWarehouse.warehousecd),
				typeof(CmpeLocation.locationCD)
			});


			var resultSet = new List<(CmpeInventoryStatus instatus, CmpePart part, CmpeWarehouse warehouse, CmpeLocation location)>();

			using (new PXFieldScope(cmd.View, fieldsScope.ToArray()))
			{
				foreach (PXResult<CmpeInventoryStatus, CmpePart, CmpeWarehouse, CmpeLocation> result in cmd.Select())
				{
					CmpeInventoryStatus inSatus = result;
					CmpePart part = result;
					CmpeWarehouse warehouse = result;
					CmpeLocation location = result;

					resultSet.Add((inSatus, part, warehouse, location));
				}
			}

			return resultSet
				.OrderBy(x => x.part.Partcd)
				.ThenBy(x => x.warehouse.Warehousecd)
				.ThenBy(x => x.location.LocationCD)
				.Select(x => x.instatus);
		}

		private CmpeInventoryStatus CalculateSummaryTotal(IEnumerable<CmpeInventoryStatus> resultsset)
		{
			CmpeInventoryStatus total = resultsset.CalculateSumTotal(INStatusRecords.Cache);

			total.Inventorystatusid = null;
			total.PartID = null;
			total.WarehouseID = null;
			total.LocationID = -1;
			total.Price = null;
			total.IsTotal = true;

			return total;
		}

		protected virtual void _(Events.FieldSelecting<CmpeInventoryStatus.locationid> e)
		{
			switch (e.ReturnValue)
			{
				case -1:
					e.ReturnState = PXFieldState.CreateInstance(PXMessages.LocalizeNoPrefix(Messages.Total), typeof(string), false, null, null, null, null, null,nameof(CmpeInventoryStatus.locationid), null, GetLocationDisplayName(), null, PXErrorLevel.Undefined, null, null, null, PXUIVisibility.Undefined, null, null, null);
					e.Cancel = true;
					{ }
					break;
			}

		}

		private string GetLocationDisplayName()
		{
			var displayName = PXUIFieldAttribute.GetDisplayName<CmpeInventoryStatus.locationid>(INStatusRecords.Cache);
			if (displayName != null) displayName = PXMessages.LocalizeNoPrefix(displayName);

			return displayName;
		}

	}
}
