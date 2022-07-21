using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	[Serializable]
	[PXCacheName("Inventory")]
	public class CmpeInventory : IBqlTable
	{
		#region InventoryID
		[PXDBIdentity]
		public virtual int? InventoryID { get; set; }
		public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
		#endregion

		#region PartID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpePart.partID>),
		typeof(CmpePart.partID),
		typeof(CmpePart.partCD),
		SubstituteKey = typeof(CmpePart.partCD))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

		#region WarehouseID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseID>),
		typeof(CmpeWarehouse.warehouseCD),
		typeof(CmpeWarehouse.warehouseDescription),
		SubstituteKey = typeof(CmpeWarehouse.warehouseCD))]
		[PXUIField(DisplayName = "Warehouse")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
		#endregion

		#region LocationID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeLocation.locationID,
							Where<CmpeLocation.warehouseID,
							Equal<Current<warehouseID>>>>),
			typeof(CmpeLocation.locationID),
			typeof(CmpeLocation.locationCD),
			SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location")]
		public virtual int? LocationID { get; set; }
		public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
		#endregion

		#region Price
		[PXDBInt]
        [PXUIField(DisplayName = "Price")]
        public virtual int? Price { get; set; }
        public abstract class price : PX.Data.BQL.BqlString.Field<price> { }
        #endregion

        #region TotalPrice
        [PXDBInt]
        [PXFormula(typeof(Mult<qty, price>))]
        [PXUIField(DisplayName = "Total Price")]
        public virtual int? TotalPrice { get; set; }
        public abstract class totalPrice : PX.Data.BQL.BqlString.Field<totalPrice> { }
        #endregion

        #region Qty
        [PXDBInt]
		[PXUIField(DisplayName = "Quantity")]
		public virtual int? Qty { get; set; }
		public abstract class qty : PX.Data.BQL.BqlString.Field<qty> { }	
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

