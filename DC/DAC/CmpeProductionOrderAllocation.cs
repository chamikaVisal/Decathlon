using System;
using PX.Data;
using PX.Data.WorkflowAPI;

namespace PX.Objects.DC.DAC
{
	[PXCacheName("Production Order Allocation")]
	public class CmpeProductionOrderAllocation : IBqlTable
	{
		#region Events
		public class Events : PXEntityEvent<CmpeProductionOrderAllocation>.Container<Events>
		{
			public PXEntityEvent<CmpeProductionOrderAllocation> SaveDocument;
		}
		#endregion

		#region CustomerOrderAllocationID
		[PXDBIdentity(IsKey =true)]
		public virtual int? CustomerOrderAllocationID { get; set; }
		public abstract class customerOrderAllocationID : PX.Data.BQL.BqlInt.Field<customerOrderAllocationID> { }
		#endregion

		#region ProductID
		[PXDBInt]
		[PXUIField(DisplayName = "Product")]
		[PXSelector(typeof(Search<CmpePart.partID, Where<CmpePart.partID.IsEqual<productID.FromCurrent>>>),
		typeof(CmpePart.partCD),
		typeof(CmpePart.description), SubstituteKey = typeof(CmpePart.partCD))]
		public virtual int? ProductID { get; set; }
		public abstract class productID : PX.Data.BQL.BqlInt.Field<productID> { }
		#endregion

		#region WarehouseID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeWarehouse.warehouseID>),
			typeof(CmpeWarehouse.warehouseCD),
			typeof(CmpeWarehouse.warehouseDescription),
			SubstituteKey = typeof(CmpeWarehouse.warehouseCD))]
		[PXUIField(DisplayName = "Warehouse")]
		public virtual int? WarehouseID { get; set; }
		public abstract class warehouseID : PX.Data.BQL.BqlInt.Field<warehouseID> { }//Not consistent
		#endregion

		#region LocationID
		[PXDBInt]
		[PXSelector(typeof(Search<CmpeLocation.locationID, Where<CmpeLocation.warehouseID, Equal<Current<warehouseID>>>>),
			typeof(CmpeLocation.locationID),
			typeof(CmpeLocation.locationCD),
			SubstituteKey = typeof(CmpeLocation.locationCD))]
		[PXUIField(DisplayName = "Location")]
		public virtual int? LocationID { get; set; }
		public abstract class locationID : PX.Data.BQL.BqlInt.Field<locationID> { }//Not consistent
		#endregion

		#region Quantity
		[PXDBInt]
		[PXUIField(DisplayName = "Quantity")]
		public virtual int? Quantity { get; set; }
		public abstract class quantity : PX.Data.BQL.BqlInt.Field<quantity> { }
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
