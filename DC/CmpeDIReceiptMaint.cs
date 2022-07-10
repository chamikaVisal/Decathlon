﻿using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.DC.DAC;
using System.Linq;

namespace PX.Objects.DC
{
	public class CmpeDIReceiptMaint : PXGraph<CmpeDIReceiptMaint>
	{
		//main view
		public SelectFrom<CmpeInventoryStatus>.View StatusDetails;

		#region Actions
		public PXAction<CmpeInventoryStatus> DirectInventoryReceipts;
		[PXButton(OnClosingPopup = PXSpecialButtonType.Refresh)]
		[PXUIField(DisplayName = "Direct Inventory Receipts", Enabled = true)]

		public virtual void directInventoryReceipts()
		{
			var dialogBoxGraph = CreateInstance<CmpeDirectInventoryReceiptMaint>();
			throw new PXPopupRedirectException(dialogBoxGraph, "Direct Inventory Receipts");
		}
		#endregion

	}
}
