using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Inventory Status")]
	public class CmpeInventoryStatus : IBqlTable
	{
		#region Keys
		public class PK : PrimaryKeyOf<CmpeInventoryStatus>.By<partid, warehouseid, locationid>
		{
			public static CmpeInventoryStatus Find(PXGraph graph, int? partId, int? warehouseid, int? locationId) => FindBy(graph, partId, warehouseid, locationId);
		}
		#endregion

		#region InventoryStatusID
		[PXDBIdentity()]
		public virtual int? Inventorystatusid { get; set; }
		public abstract class inventorystatusid : PX.Data.BQL.BqlInt.Field<inventorystatusid> { }
		#endregion

		#region PartID
		[PXDBInt(IsKey = true)]
		[PXSelector(typeof(Search<CmpePart.partid>),
		typeof(CmpePart.partid),
		typeof(CmpePart.partcd),
		SubstituteKey = typeof(CmpePart.partcd))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region WarehouseID
		[PXDBInt(IsKey = true)]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseid>),
		typeof(CmpeWarehouse.warehousecd),
		typeof(CmpeWarehouse.warehousedescription),
		SubstituteKey = typeof(CmpeWarehouse.warehousecd))]
		[PXUIField(DisplayName = "Warehouse")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseid : PX.Data.BQL.BqlInt.Field<warehouseid> { }
		#endregion

		#region LocationID
		[PXDBInt(IsKey = true)]
		[PXSelector(typeof(Search<CmpeLocation.locationID,
							Where<CmpeLocation.warehouseid,
							Equal<Current<warehouseid>>>>),
			typeof(CmpeLocation.locationID),
			typeof(CmpeLocation.locationCD),
			SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location")]
		public virtual int? LocationID { get; set; }
		public abstract class locationid : PX.Data.BQL.BqlInt.Field<locationid> { }
		#endregion

		#region Quantity
		[PXDBDecimal]
		[PXUIField(DisplayName = "Quantity")]
		public virtual decimal? Quantity { get; set; }
		public abstract class quantity : PX.Data.BQL.BqlDecimal.Field<quantity> { }
		#endregion

		#region Price
		[PXDBInt]
		[PXUIField(DisplayName = "Price")]
		public virtual int? Price { get; set; }
		public abstract class price : PX.Data.BQL.BqlString.Field<price> { }
		#endregion

		#region IsTotal
		[PXBool]
		[PXUIField(DisplayName = "Price")]
		public virtual bool? IsTotal { get; set; }
		public abstract class isTotal : PX.Data.BQL.BqlBool.Field<isTotal> { }
		#endregion

		#region CreatedDateTime
		[PXDBCreatedDateTime()]
		public virtual DateTime? CreatedDateTime { get; set; }
		public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
		#endregion

		#region CreatedByID
		[PXDBCreatedByID()]
		public virtual Guid? CreatedByID { get; set; }
		public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
		#endregion

		#region CreatedByScreenID
		[PXDBCreatedByScreenID()]
		public virtual string CreatedByScreenID { get; set; }
		public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
		#endregion

		#region LastModifiedDateTime
		[PXDBLastModifiedDateTime()]
		public virtual DateTime? LastModifiedDateTime { get; set; }
		public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
		#endregion

		#region LastModifiedByID
		[PXDBLastModifiedByID()]
		public virtual Guid? LastModifiedByID { get; set; }
		public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
		#endregion

		#region LastModifiedByScreenID
		[PXDBLastModifiedByScreenID()]
		public virtual string LastModifiedByScreenID { get; set; }
		public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
		#endregion

		#region Tstamp
		[PXDBTimestamp()]
		public virtual byte[] Tstamp { get; set; }
		public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
		#endregion

		#region NoteID
		[PXNote()]
		public virtual Guid? NoteID { get; set; }
		public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
		#endregion
	}
}
