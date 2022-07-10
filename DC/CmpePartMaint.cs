using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC
{
	public class CmpePartMaint : PXGraph<CmpePartMaint, CmpePart>
	{
		public SelectFrom<CmpePart>.View Parts;

		#region Events
		protected virtual void _(Events.RowSelected<CmpePart> e)
		{
			CmpePart row = e.Row;

			if (row != null)
			{
				PXUIFieldAttribute.SetVisible<CmpePart.type>(e.Cache, e.Row, row.ItemType.Equals("S"));
			}
		}
		protected virtual void _(Events.RowPersisting<CmpePart> e)
		{
			CmpePart row = e.Row;

			if (row.ItemType == ItemTypes.Stock && row.Type == null)
			{
				// Acuminator disable once PX1050 HardcodedStringInLocalizationMethod [Justification]
				e.Cache.RaiseExceptionHandling<CmpePart.type>(row, row.Type, new PXException("Part Type should be selected!"));
			}
		}

		protected virtual void _(Events.FieldUpdated<CmpePart, CmpePart.itemtype> e)
		{
			CmpePart row = e.Row;

			if (row.ItemType == ItemTypes.NonStock)
			{
				e.Cache.SetValueExt<CmpePart.type>(row, InventoryParts.Service);
			}
			else
			{
				e.Cache.SetValueExt<CmpePart.type>(row, null);
			}
		}



	}


}




	#endregion


