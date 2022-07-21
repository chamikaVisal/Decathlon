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
		public class PK : PrimaryKeyOf<CmpeInventoryAllocation>.By<partID>
		{
			public static CmpeInventoryAllocation Find(PXGraph graph, int? partId) => FindBy(graph, partId);
		}
		#endregion

		#region InventoryAllocationID
		[PXDBIdentity]
		public virtual int? InventoryAllocationID { get; set; }
		public abstract class inventoryAllocationID : PX.Data.BQL.BqlInt.Field<inventoryAllocationID> { }
		#endregion

		#region PartID
		[PXDBInt(IsKey=true)]
		[PXSelector(typeof(Search<CmpePart.partID>),
		typeof(CmpePart.partID),
		typeof(CmpePart.partCD),
		SubstituteKey = typeof(CmpePart.partCD))]
		[PXUIField(DisplayName = "Part")]
		public virtual int? PartID { get; set; }
		public abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

		#region AvailableForSale
		[PXDBInt]
		[PXFormula(typeof(Sub<quantityInHand, reservedQuantity>))]
		[PXUIField(DisplayName = "Available for Sale")]
		public virtual int? AvailableForSale { get; set; }
		public abstract class availableForSale : PX.Data.BQL.BqlInt.Field<availableForSale> { }
		#endregion

		#region ReservedQuantity
		[PXDBInt]
		[PXUIField(DisplayName = "Reserved Quantity")]
		public virtual int? ReservedQuantity { get; set; }
		public abstract class reservedQuantity : PX.Data.BQL.BqlInt.Field<reservedQuantity> { }
		#endregion

		#region QuantityInHand
		[PXDBInt]
		[PXUIField(DisplayName = "Quantity in Hand")]
		public virtual int? QuantityInHand { get; set; }
		public abstract class quantityInHand : PX.Data.BQL.BqlInt.Field<quantityInHand> { }
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
