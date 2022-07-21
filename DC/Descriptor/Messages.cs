using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.DC.Descriptor
{
	[PX.Common.PXLocalizable()]//Explain this
	public class Messages
	{
		public const string Manufactured = "Manufactured";
		public const string Purchased = "Purchased";
		public const string Service = "Service";
		public const string DuplicatedLocationCD = "You can not duplicate the same location CD";
		public const string EmptyLocationCD = "You can not leave the location CD empty";
		public const string NoSufficientQtyMessage = "No sufficient quantity in stock!";

		//Coloumn labels
		public const string Total = "TOTAL";

		//Item Error Messages
		public const string TypeNotSelected = "Part Type should be selected!";
		public const string NoQuantity = "There is no sufficient quantity available!";
		public const string QuantityNotFound = "Quantity details not available!";

		//Common Erros
		public const string UnexpectedError = "Unexpected Error occured!";

		//inventory parts item types
		public const string Stock = "Stock";
		public const string NonStock = "Non-Stock";

		//production order status
		public const string Not_Set = "Not Set";
		public const string Released = "Released";
		public const string Reserved = "Reserved";
		public const string Closed = "Closed";
		public const string Cancelled = "Cancelled";

		//Customer Order status
		public const string CONot_Set = "Not Set";
		public const string COPlanned = "Planned";
		public const string COReleased = "Released";
		public const string COClosed = "Closed";
		public const string COCancelled = "Cancelled";

		//Customer Order - Item details Status
		public const string COItemRequired = "Required";
		public const string COItemDelivered = "Delivered";
		public const string COItemCancelled = "Cancelled";

	}

}
