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
		[PXUIField(DisplayName = "Receipt ID")]
		public virtual int? DIReceiptID { get; set; }
		public abstract class dIReceiptID : PX.Data.BQL.BqlInt.Field<dIReceiptID> { }
		#endregion

		#region DIReceiptCD
		[PXDBString(50, InputMask = ">aaaaaaaaaaaaaaa")]
		[PXUIField(DisplayName = "Receipt Name")]
		public virtual string DIReceiptCD { get; set; }
		public abstract class dIReceiptCD : PX.Data.BQL.BqlString.Field<dIReceiptCD> { }
		#endregion

		#region PartID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpePart.partID>),
			typeof(CmpePart.partID),
			typeof(CmpePart.partCD),
			SubstituteKey = typeof(CmpePart.partCD))]
		[PXUIField(DisplayName = "Part Name")]
		public virtual int? PartID { get; set; }
		public abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

		#region WarehouseID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseID>),
			typeof(CmpeWarehouse.warehouseCD),
			typeof(CmpeWarehouse.warehouseDescription),
			SubstituteKey = typeof(CmpeWarehouse.warehouseCD))]
		[PXUIField(DisplayName = "Warehouse Name")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }
		#endregion

		#region LocationID
		[PXDBInt()]
		[PXSelector(typeof(Search<CmpeLocation.locationID,
							Where<CmpeLocation.warehouseID,
							Equal<Current<warehouseID>>>>),
			typeof(CmpeLocation.locationID),
			typeof(CmpeLocation.locationCD),
			SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location Name")]
		public virtual int? LocationID { get; set; }
		public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
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
}
