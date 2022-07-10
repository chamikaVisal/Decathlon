using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;

namespace PX.Objects.DC
{
	public class CmpeCustomerMaint : PXGraph<CmpeCustomerMaint>
	{
		public SelectFrom<CmpeCustomer>.View Customers;
		public PXSave<CmpeCustomer> Save;
		public PXCancel<CmpeCustomer> Cancel;

	}
}
