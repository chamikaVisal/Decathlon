using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace PX.Objects.DC.DAC
{
	[Serializable]
	[PXCacheName("Customers")]
	public class CmpeCustomer : IBqlTable
	{
		#region Keys
		public class PK : PrimaryKeyOf<CmpeCustomer>.By<customerID>
		{
			public static CmpeCustomer Find(PXGraph graph, int? customerid) => FindBy(graph, customerid);
		}
		#endregion

		#region CustomerID
		[PXDBIdentity]
		public virtual int? CustomerID { get; set; }
		public abstract class customerID : PX.Data.BQL.BqlInt.Field<customerID> { }
		#endregion

		#region CustomerName
		[PXDBString(50, IsKey = true)]
		[PXDefault]
		[PXUIField(DisplayName = "Customer Name")]
		public virtual string CustomerName { get; set; }
		public abstract class customerName : PX.Data.BQL.BqlString.Field<customerName> { }
		#endregion

		#region Address
		[PXDBString(200)]
		[PXUIField(DisplayName = "Address")]
		public virtual string Address { get; set; }
		public abstract class address : PX.Data.BQL.BqlString.Field<address> { }
		#endregion
	}
}
