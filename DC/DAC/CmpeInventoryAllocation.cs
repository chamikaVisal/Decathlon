using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Inventory Allocation")]
	public class CmpeInventoryAllocation : IBqlTable
	{
		#region Keys
		public class PK : PrimaryKeyOf<CmpeInventoryAllocation>.By<partid>
		{
			public static CmpeInventoryAllocation Find(PXGraph graph, int? partId) => FindBy(graph, partId);
		}
		#endregion

		#region InventoryAllocationID
		[PXDBIdentity]
		public virtual int? Inventoryallocationid { get; set; }
		public abstract class inventoryallocationid : PX.Data.BQL.BqlInt.Field<inventoryallocationid> { }
		#endregion

		#region PartID
		[PXDBInt(IsKey=true)]
		[PXSelector(typeof(Search<CmpePart.partid>),
		typeof(CmpePart.partid),
		typeof(CmpePart.partcd),
		SubstituteKey = typeof(CmpePart.partcd))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partid : PX.Data.BQL.BqlInt.Field<partid> { }
		#endregion

		#region AvailableForSale
		[PXDBInt]
		[PXFormula(typeof(Sub<quantityinhand, reservedquantity>))]
		[PXUIField(DisplayName = "Available for Sale")]
		public virtual int? Availableforsale { get; set; }
		public abstract class availableforsale : PX.Data.BQL.BqlInt.Field<availableforsale> { }
		#endregion

		#region ReservedQuantity
		[PXDBInt]
		[PXUIField(DisplayName = "Reserved Quantity")]
		public virtual int? Reservedquantity { get; set; }
		public abstract class reservedquantity : PX.Data.BQL.BqlInt.Field<reservedquantity> { }
		#endregion

		#region QuantityInHand
		[PXDBInt]
		[PXUIField(DisplayName = "Quantity in Hand")]
		public virtual int? Quantityinhand { get; set; }
		public abstract class quantityinhand : PX.Data.BQL.BqlInt.Field<quantityinhand> { }
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
