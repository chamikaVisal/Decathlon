using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using Messages = PX.Objects.DC.Descriptor.Messages;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Sales Order No Part Details")]
	public class CmpeCustomerOrderNoPartDetails : IBqlTable
	{
		#region CustomerOrder
		[PXDBString(IsKey = true)]
		[PXDBDefault(typeof(CmpeSalesOrder.customerOrder))]
		[PXParent(typeof(SelectFrom<CmpeSalesOrder>.
			Where<CmpeSalesOrder.customerOrder.
			IsEqual<customerOrder.FromCurrent>>))]
		public virtual string CustomerOrder { get; set; }
		public abstract class customerOrder : BqlString.Field<customerOrder> { }
		#endregion

		#region NoPartID
		[PXDBInt(IsKey = true)]
		[PXDefault]
		[PXSelector(typeof(Search<CmpePart.partID, Where<CmpePart.itemType.IsEqual<NonStock>>>),
		typeof(CmpePart.partCD),
		typeof(CmpePart.description),
		SubstituteKey = typeof(CmpePart.partCD))]
		public virtual int? NoPartID { get; set; }
		public abstract class noPartID : BqlInt.Field<noPartID> { }
		#endregion

		#region PartID
		[PXDBInt]
		[PXDefault]
		[PXSelector(typeof(Search<CmpePart.partID, Where<CmpePart.itemType.IsEqual<Stock>>>),
		typeof(CmpePart.partCD),
		typeof(CmpePart.description),
		SubstituteKey = typeof(CmpePart.partCD))]
		public virtual int? PartID { get; set; }
		public abstract class partID : BqlInt.Field<partID> { }
		#endregion

		#region PartDescription
		[PXDBString(100)]
		[PXDefault]
		[PXUIField(DisplayName = "Description")]
		[PXSelector(typeof(SelectFrom<CmpePart>.Where<CmpePart.partID.IsEqual<partID.FromCurrent>>.SearchFor<CmpePart.description>), SelectorMode = PXSelectorMode.MaskAutocomplete)]
		public virtual string PartDescription { get; set; }
		public abstract class partDescription : BqlString.Field<partDescription> { }
		#endregion

		#region Qty
		[PXDBDecimal()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Quantity")]
		public virtual Decimal? Qty { get; set; }
		public abstract class qty : BqlDecimal.Field<qty> { }
		#endregion

		#region Price
		[PXDBDecimal()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXUIField(DisplayName = "Price", Required = true)]
		[PXFormula(typeof(Selector<CmpeCustomerOrderNoPartDetails.noPartID, CmpePart.price>))]
		public virtual Decimal? Price { get; set; }
		public abstract class price : BqlDecimal.Field<price> { }
		#endregion

		#region TotalPrice
		[PXDecimal()]
		[PXUIField(DisplayName = "Total Price")]
		[PXFormula(typeof(Mult<qty, price>),typeof(SumCalc<CmpeSalesOrder.noPartItemTotalPrice>))]
		public virtual Decimal? TotalPrice { get; set; }
		public abstract class totalPrice : BqlDecimal.Field<totalPrice> { }
		#endregion

		#region Status
		[PXDBString(20, IsUnicode = true, InputMask = "")]
		[PXDefault(Messages.COItemRequired)]
		[PXUIField(DisplayName = "Status")]
		public virtual string Status { get; set; }
		public abstract class status : BqlString.Field<status> { }
		#endregion

		#region CreatedDateTime
		[PXDBCreatedDateTime()]
		public virtual DateTime? CreatedDateTime { get; set; }
		public abstract class createdDateTime : BqlDateTime.Field<createdDateTime> { }
		#endregion

		#region CreatedByID
		[PXDBCreatedByID()]
		public virtual Guid? CreatedByID { get; set; }
		public abstract class createdByID : BqlGuid.Field<createdByID> { }
		#endregion

		#region CreatedByScreenID
		[PXDBCreatedByScreenID()]
		public virtual string CreatedByScreenID { get; set; }
		public abstract class createdByScreenID : BqlString.Field<createdByScreenID> { }
		#endregion

		#region LastModifiedDateTime
		[PXDBLastModifiedDateTime()]
		public virtual DateTime? LastModifiedDateTime { get; set; }
		public abstract class lastModifiedDateTime : BqlDateTime.Field<lastModifiedDateTime> { }
		#endregion

		#region LastModifiedByID
		[PXDBLastModifiedByID()]
		public virtual Guid? LastModifiedByID { get; set; }
		public abstract class lastModifiedByID : BqlGuid.Field<lastModifiedByID> { }
		#endregion

		#region LastModifiedByScreenID
		[PXDBLastModifiedByScreenID()]
		public virtual string LastModifiedByScreenID { get; set; }
		public abstract class lastModifiedByScreenID : BqlString.Field<lastModifiedByScreenID> { }
		#endregion

		#region Tstamp
		[PXDBTimestamp()]
		public virtual byte[] Tstamp { get; set; }
		public abstract class tstamp : BqlByteArray.Field<tstamp> { }
		#endregion

		#region NoteID
		[PXNote()]
		public virtual Guid? NoteID { get; set; }
		public abstract class noteID : BqlGuid.Field<noteID> { }
		#endregion
	}
}
