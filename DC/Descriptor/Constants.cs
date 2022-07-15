using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PX.Objects.DC.Descriptor
{
	internal class Constants
	{
		public static class InventoryParts
		{
			public const string Manufactured = "M";
			public const string Purchased = "P";
			public const string Service = "Service";
		}
		public class Manufactured : BqlString.Constant<Manufactured> { public Manufactured() : base(InventoryParts.Manufactured) { } }
		public class Purchased : BqlString.Constant<Purchased> { public Purchased() : base(InventoryParts.Purchased) { } }
		public class Service : BqlString.Constant<Purchased> { public Service() : base(InventoryParts.Service) { } }

		public static class ItemTypes
		{
			public const string Stock = "S";
			public const string NonStock = "NS";
		}
		public static class ProductionOrderStatuses
		{
			public const string Released = "RE";
			public const string Reserved = "RS";
			public const string Closed = "CL";
			public const string Cancelled = "CN";
			public const string Not_Set = "NS";
		}
		public class released : PX.Data.BQL.BqlString.Constant<released> { public released() : base(ProductionOrderStatuses.Released) { } }
		public class reserved : PX.Data.BQL.BqlString.Constant<reserved> { public reserved() : base(ProductionOrderStatuses.Reserved) { } }
		public class closed : PX.Data.BQL.BqlString.Constant<closed> { public closed() : base(ProductionOrderStatuses.Closed) { } }
		public class cancelled : PX.Data.BQL.BqlString.Constant<cancelled> { public cancelled() : base(ProductionOrderStatuses.Cancelled) { } }
		public class notSet : PX.Data.BQL.BqlString.Constant<notSet> { public notSet() : base(ProductionOrderStatuses.Not_Set) { } }

		public static class CustomerOrderStatus
		{
			public const string Not_Set = "Not Set";
			public const string COPlanned = "Planned";
			public const string COReleased = "Released";
			public const string COClosed = "Closed";
			public const string COCancelled = "Cancelled";
		}

		public static class CustomerOrderItemDetailsStatus
		{
			public const string COItemRequired = "Required";
			public const string COItemDelivered = "Delivered";
			public const string COItemCancelled = "Cancelled";
		}

		public class Stock : BqlString.Constant<Stock> { public Stock() : base(ItemTypes.Stock) { } }
		public class NonStock : BqlString.Constant<NonStock> { public NonStock() : base(ItemTypes.NonStock) { } }
	}
}
