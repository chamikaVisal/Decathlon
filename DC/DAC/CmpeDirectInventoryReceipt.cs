using PX.Data;
using System;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Direct Inventory Receipt")]
	public class CmpeDirectInventoryReceipt : IBqlTable
	{
		#region DIReceiptID
		[PXDBIdentity(IsKey = true)]
		[PXUIField(DisplayName = "Receipt CD")]
		public virtual int? DIReceiptid { get; set; }
		public abstract class direceiptid : PX.Data.BQL.BqlInt.Field<direceiptid> { }
		#endregion

		#region DIReceiptCD
		[PXDBString(50, IsUnicode = true, InputMask = ">aaaaaaaaaaaaaaa")]
		[PXUIField(DisplayName = "Receipt Name")]
		//[PXDBDefault]
		public virtual string DIReceiptcd { get; set; }
		public abstract class direceiptcd : PX.Data.BQL.BqlString.Field<direceiptcd> { }
		#endregion

		#region PartID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpePart.partid>),
			typeof(CmpePart.partid),
			typeof(CmpePart.partcd),
			SubstituteKey = typeof(CmpePart.partcd))]
		[PXUIField(DisplayName = "Part Name")]
		public virtual int? Partid { get; set; }
		public abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region WarehouseID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseid>),
			typeof(CmpeWarehouse.warehousecd),
			typeof(CmpeWarehouse.warehousedescription),
			SubstituteKey = typeof(CmpeWarehouse.warehousecd))]
		[PXUIField(DisplayName = "Warehouse Name")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
		#endregion

		#region LocationID
		[PXDBInt()]
		[PXSelector(typeof(Search<CmpeLocation.locationID,
							Where<CmpeLocation.warehouseid,
							Equal<Current<warehouseID>>>>),
			typeof(CmpeLocation.locationID),
			typeof(CmpeLocation.locationCD),
			SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location Name")]
		public virtual int? Locationid { get; set; }
		public abstract class locationid : PX.Data.BQL.BqlInt.Field<locationid> { }
		#endregion

		#region Quantity
		[PXDBInt]
		[PXDefault]
		[PXUIField(DisplayName = "Quantity")]
		public virtual int? Quantity { get; set; }
		public abstract class quantity : PX.Data.BQL.BqlString.Field<quantity> { }
		#endregion

		#region ReleasedStatus
		[PXDBBool]
		[PXDefault(false)]
		public virtual bool? ReleasedStatus { get; set; }
		public abstract class releasedStatus : PX.Data.BQL.BqlBool.Field<releasedStatus> { }
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

	public class CmpeDirectInventory : CmpeDirectInventoryReceipt
	{
		#region PartID
		[PXDBInt]
		[PXUIField(DisplayName = "Part Name")]
		public new virtual int? Partid { get; set; }
		public new abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region WarehouseID
		[PXDBInt]
		[PXUIField(DisplayName = "Warehouse Name")]
		public new virtual int? WarehouseID { get; set; }
		public new abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
		#endregion
	}
}
