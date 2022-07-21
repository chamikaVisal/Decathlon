using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.DC.Descriptor;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Inventory Items")]
	public class CmpePart : IBqlTable
	{
		#region Keys
		public class PK : PrimaryKeyOf<CmpePart>.By<partID>
		{
			public static CmpePart Find(PXGraph graph, int? partNo) => FindBy(graph, partNo);
		}
		#endregion
		#region Partid
		[PXDBIdentity]
		public virtual int? PartID { get; set; }
		public abstract class partID : PX.Data.BQL.BqlInt.Field<partID> { }
		#endregion

		#region Partcd
		[PXDBString(50, IsUnicode = true, InputMask = ">aaaaaaaaaaaaaaa", IsKey = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Item Name")]
		[PXSelector(typeof(Search<partCD, Where<CmpePart.type.IsEqual<Manufactured>>>),
		typeof(partCD),
		typeof(description))]
		public virtual string PartCD { get; set; }
		public abstract class partCD : PX.Data.BQL.BqlString.Field<partCD> { }
		#endregion

		#region Description
		[PXDBString(100, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Item Description")]
		public virtual string Description { get; set; }
		public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
		#endregion

		#region Type
		[PXDBString(50, IsUnicode = true, InputMask = "")]
		[PXUIField(DisplayName = "Item Type",Required = true)]
		[PXDefault(InventoryParts.Service)]
		[PXStringList(
		new string[]{
			InventoryParts.Manufactured,
			InventoryParts.Purchased,
		},
		new string[]
		{
			Messages.Manufactured,
			Messages.Purchased
		})]
		public virtual string Type { get; set; }
		public abstract class type : PX.Data.BQL.BqlString.Field<type> { }
		#endregion

		#region ItemType
		[PXDBString(50, IsUnicode = true, InputMask = "")]
		[PXDefault(ItemTypes.NonStock)]
		[PXUIField(DisplayName = "Stock Type", Required = true)]
		[PXStringList(
		new string[]{
			ItemTypes.Stock,
			ItemTypes.NonStock
		},
		new string[]
		{
			Messages.Stock,
			Messages.NonStock
		})]
		public virtual string ItemType { get; set; }
		public abstract class itemType : PX.Data.BQL.BqlString.Field<itemType> { }
		#endregion

		#region Price
		[PXDBDecimal()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Price", Required = true)]
		public virtual Decimal? Price { get; set; }	
		public abstract class price : PX.Data.BQL.BqlDecimal.Field<price> { }
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
