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
		[PXSelector(typeof(Search<CmpePart.partID>),
		typeof(CmpePart.partID),
		typeof(CmpePart.partCD),
		SubstituteKey = typeof(CmpePart.partCD))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region WarehouseID
		[PXInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseID>),
		typeof(CmpeWarehouse.warehouseCD),
		typeof(CmpeWarehouse.warehouseDescription),
		SubstituteKey = typeof(CmpeWarehouse.warehouseCD))]
		[PXUIField(DisplayName = "Warehouse")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseid : PX.Data.BQL.BqlInt.Field<warehouseid> { }
		#endregion

		#region LocationID
		[PXInt]
		[PXSelector(typeof(Search<CmpeLocation.locationID, Where<CmpeLocation.warehouseID, Equal<Current<warehouseid>>>>),
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
			var cmd = new SelectFrom<CmpeInventoryStatus>.View(this);

			if (Filter.Current.PartID != null)
			{
				cmd.WhereAnd<Where<CmpeInventoryStatus.partID.IsEqual<InventoryStockfilter.partid.FromCurrent>>>();
			}

			if (Filter.Current.WarehouseID != null)
			{
				cmd.WhereAnd<Where<CmpeInventoryStatus.warehouseID.IsEqual<InventoryStockfilter.warehouseid.FromCurrent>>>();

				if (Filter.Current.LocationID != null)
				{
					cmd.WhereAnd<Where<CmpeInventoryStatus.locationID.IsEqual<InventoryStockfilter.locationid.FromCurrent>>>();
				}
			}

			List<Type> fieldsScope = new List<Type>(new Type[]{
				typeof(CmpeInventoryStatus.inventoryStatusID),
				typeof(CmpeInventoryStatus.quantity),
				typeof(CmpeInventoryStatus.partID),
				typeof(CmpeInventoryStatus.warehouseID),
				typeof(CmpeInventoryStatus.locationID)
			});


			var resultSet = new List<CmpeInventoryStatus>();

			using (new PXFieldScope(cmd.View, fieldsScope.ToArray()))
			{
				foreach (CmpeInventoryStatus result in cmd.Select())
				{
					resultSet.Add(result);
				}
			}

			return resultSet
				.OrderBy(x => x.PartID)
				.ThenBy(x => x.WarehouseID)
				.ThenBy(x => x.LocationID)
				.Select(x => x);
		}


		private CmpeInventoryStatus CalculateSummaryTotal(IEnumerable<CmpeInventoryStatus> resultsset)
		{
			CmpeInventoryStatus total = resultsset.CalculateSumTotal(INStatusRecords.Cache);

			total.InventoryStatusID = null;
			total.PartID = null;
			total.WarehouseID = null;
			total.LocationID = -1;
			total.Price = null;
			total.IsTotal = true;

			return total;
		}

		protected virtual void _(Events.FieldSelecting<CmpeInventoryStatus.locationID> e)
		{
			switch (e.ReturnValue)
			{
				case -1:
					e.ReturnState = PXFieldState.CreateInstance(PXMessages.LocalizeNoPrefix(Messages.Total), typeof(string), false, null, null, null, null, null,nameof(CmpeInventoryStatus.LocationID), null, GetLocationDisplayName(), null, PXErrorLevel.Undefined, null, null, null, PXUIVisibility.Undefined, null, null, null);
					e.Cancel = true;
					{ }
					break;
			}

		}

		private string GetLocationDisplayName()
		{
			var displayName = PXUIFieldAttribute.GetDisplayName<CmpeInventoryStatus.locationID>(INStatusRecords.Cache);
			if (displayName != null) displayName = PXMessages.LocalizeNoPrefix(displayName);

			return displayName;
		}

	}
}
