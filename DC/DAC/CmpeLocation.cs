using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using static PX.Objects.DC.DAC.CmpeWarehouse;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Locations")]
	public class CmpeLocation : IBqlTable
	{
		#region LocationID
		[PXDBIdentity]
		public virtual int? LocationID { get; set; }
		public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }
		#endregion

		#region LocationCD
		[PXDBString(50, IsUnicode = true, InputMask = "", IsKey = true)]
		[PXUIField(DisplayName = "Location CD")]
		[PXDefault]
		public virtual string LocationCD { get; set; }
		public abstract class locationCD : PX.Data.BQL.BqlString.Field<locationCD> { }
		#endregion

		#region WarehouseID
		[PXDBInt(IsKey = true)]
		[PXDBDefault(typeof(CmpeWarehouse.warehouseid))]
		[PXParent(typeof(SelectFrom<CmpeWarehouse>.
			Where<CmpeWarehouse.warehouseid.
			IsEqual<warehouseid.FromCurrent>>))]
		public virtual int? WarehouseId { get; set; }
		public abstract class warehouseid : PX.Data.BQL.BqlInt.Field<warehouseid> { }
		#endregion

		#region Description
		[PXDBString(50, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Description")]
		public virtual string Description { get; set; }
		public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
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
