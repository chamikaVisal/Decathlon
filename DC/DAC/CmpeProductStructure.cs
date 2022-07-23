using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("BOM")]
	public class CmpeProductStructure : IBqlTable
	{
		#region StructureID
		[PXDBIdentity]
		public virtual int? StructureID { get; set; }
		public abstract class structureID : PX.Data.BQL.BqlInt.Field<structureID> { }
		#endregion

		#region ProductID
		[PXDBInt(IsKey = true)]
		[PXDBDefault(typeof(CmpePart.partID))]
		[PXParent(typeof(SelectFrom<CmpePart>.
			Where<CmpePart.partID.
			IsEqual<productID.FromCurrent>>))]
		public virtual int? ProductID { get; set; }
		public abstract class productID : PX.Data.BQL.BqlInt.Field<productID> { }
		#endregion

		#region PartID
		[PXDBInt(IsKey = true)]
		[PXUIField(DisplayName = "Part Name")]
		[PXDBDefault()]
		[PXSelector(typeof(Search<CmpePart.partID, Where<CmpePart.type.IsEqual<Purchased>>>),
		typeof(CmpePart.partCD),
		typeof(CmpePart.description), SubstituteKey = typeof(CmpePart.partCD))] //change this to partcd
		public virtual int? PartID { get; set; }
		public abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

		#region Description
		[PXDBString(50, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Description")]
		public virtual string Description { get; set; }
		public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
		#endregion

		#region Quantity
		[PXDBInt]
		[PXUIField(DisplayName = "Quantity")]
		public virtual int? Quantity { get; set; }
		public abstract class quantity : PX.Data.BQL.BqlInt.Field<quantity> { }
		#endregion

		#region TotalQuantity
		[PXInt]
		[PXUIField(DisplayName = "Total Quantity")]
		[PXUnboundDefault(0)]
		public virtual int? TotalQuantity { get; set; }
		public abstract class totalQuantity : PX.Data.BQL.BqlInt.Field<totalQuantity> { }
		#endregion

		#region Available
		[PXBool]
		[PXUIField(DisplayName = "Availability")]
		public virtual bool? Available { get; set; }
		public abstract class available : PX.Data.BQL.BqlBool.Field<available> { }
		#endregion

		#region QuantityInHand
		[PXInt]
		[PXUIField(Visible = false)]
		[PXUnboundDefault(typeof(Search<CmpeInventoryAllocation.quantityInHand, Where<CmpeInventoryAllocation.partID, Equal<Current<partID>>>>))]
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

	[PXCacheName("CmpePSPart")]
	public class CmpePSPart : CmpeProductStructure
	{
		#region PartID
		[PXDBInt(IsKey = true)]
		public new virtual int? PartID { get; set; }
		public new abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

	}
}
