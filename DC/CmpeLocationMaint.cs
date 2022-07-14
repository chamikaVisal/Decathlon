using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using Messages = PX.Objects.DC.Descriptor.Messages;

namespace PX.Objects.DC
{
	public class CmpeLocationMaint : PXGraph<CmpeLocationMaint, CmpeWarehouse>
	{
		public SelectFrom<CmpeWarehouse>.View WarehouseDetails;

		public SelectFrom<CmpeLocation>.
			Where<CmpeLocation.warehouseid.
				IsEqual<CmpeWarehouse.warehouseid.FromCurrent>>.View LocationDetails;

		protected virtual void _(Events.RowInserting<CmpeLocation> e)
		{
			CmpeLocation row = e.Row;
			
			if(row == null) return;

			if (String.IsNullOrEmpty(row.LocationCD))
			{	
				throw new PXException(Messages.EmptyLocationCD,
									  typeof(CmpeLocation.locationCD));
			}
				
			var loc = LocationDetails.Locate(row);

			if(loc != null)
			{
				throw new PXException(Messages.DuplicatedLocationCD,
							row.LocationCD);
			}	
		}
	}
}
