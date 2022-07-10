using System;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.DC.Descriptor;
using static PX.Objects.DC.Descriptor.Constants;
using Messages = PX.Objects.DC.Descriptor.Messages;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Sales Order")]
	public class CmpeSalesOrder : IBqlTable
	{
		#region CustomerOrderID
		[PXDBIdentity]
		public virtual int? CustomerOrderID { get; set; }
		public abstract class customerOrderID : PX.Data.BQL.BqlInt.Field<customerOrderID> { }
		#endregion

		#region CustomerOrder
		[PXDBString(200,IsUnicode = true,IsKey =true)]
		[PXDefault]
		[PXUIField(DisplayName ="CustomerOrderNo")]
		[AutoNumber(typeof(CmpeSetup.salesOrderNumberingID), typeof(CmpeSalesOrder.orderDate))]
		public virtual string CustomerOrder { get; set; }
		public abstract class customerOrder : PX.Data.BQL.BqlString.Field<customerOrder> { }
		#endregion

		#region OrderDate
		[PXDBCreatedDateTime()]
		[PXDefault]
		[PXUIField(DisplayName = "Order Date")]
		public virtual DateTime? OrderDate { get; set; }
		public abstract class orderDate : PX.Data.BQL.BqlDateTime.Field<orderDate> { }
		#endregion

		#region CustomerID
		[PXDBInt(IsKey = true)]
		[PXDefault(typeof(CmpeCustomer.customername))]
		[PXUIField(DisplayName = "Customer Name")]
		[PXSelector(typeof(Search<CmpeCustomer.customerid>),
			typeof(CmpeCustomer.customerid),
			typeof(CmpeCustomer.customername),
			SubstituteKey = typeof(CmpeCustomer.customername))]
		public virtual int? CustomerID { get; set; }
		public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
		#endregion

		#region CustomerAddress
		[PXDBString(50)]
		[PXDefault]
		[PXUIField(DisplayName = "Customer Address")]
		public virtual string CustomerAddress { get; set; }
		public abstract class customerAddress : PX.Data.BQL.BqlString.Field<customerAddress> { }
		#endregion

		#region Status
		[PXDBString(50, IsUnicode = true, InputMask = "")]
		[PXDefault(Messages.CONot_Set)]
		[PXUIField(DisplayName = "Status", Enabled = false)]
		public virtual string Status { get; set; }
		public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
		#endregion

		#region PartItemTotalPrice
		[PXDecimal()]
		[PXUnboundDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? PartItemTotalPrice { get; set; }
		public abstract class partItemTotalPrice : PX.Data.BQL.BqlDecimal.Field<partItemTotalPrice> { }
		#endregion

		#region NoPartItemTotalPrice
		[PXDecimal()]
		[PXUnboundDefault(TypeCode.Decimal, "0.0")]
		public virtual Decimal? NoPartItemTotalPrice { get; set; }
		public abstract class noPartItemTotalPrice : PX.Data.BQL.BqlDecimal.Field<noPartItemTotalPrice> { }
		#endregion


		#region TotalPrice
		[PXDBDecimal()]
		[PXDefault(TypeCode.Decimal, "0.0")]
		[PXFormula(typeof(Add<partItemTotalPrice, noPartItemTotalPrice>))]
		[PXUIField(DisplayName = "Total Price", Required = true)]
		public virtual Decimal? TotalPrice { get; set; }
		public abstract class totalPrice : PX.Data.BQL.BqlDecimal.Field<totalPrice> { }
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
