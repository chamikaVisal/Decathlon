using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using PX.Objects.DC.DAC;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC.Workflows
{
	public class CmpeProductionOrderWorkflow : PX.Data.PXGraphExtension<CmpeProductionOrderMaint>
	{
		public static bool IsActive() => false;

		public override void Configure(PXScreenConfiguration config)
		{
			var context = config.GetScreenConfigurationContext<CmpeProductionOrderMaint, CmpeProductionOrder>();

			#region Categories
			var commonCategories = CommonActionCategories.Get(context);
			var processingCategory = commonCategories.Processing;
			#endregion

			context.AddScreenConfigurationFor(screen => screen
			.StateIdentifierIs<CmpeProductionOrder.productionOrderStatus>()
			.AddDefaultFlow(flow => flow
			.WithFlowStates(fss =>
			{
				fss.Add<notSet>(flowState =>
				{
					return flowState
					.IsInitial();
				});
				fss.Add<released>(flowState =>
				{
					return flowState
					.WithActions(actions =>
					{
						actions.Add(g => g.IssueMaterial, a => a
						.IsDuplicatedInToolbar()
						.WithConnotation(ActionConnotation.Success));
					});
				});
				fss.Add<reserved>(flowState =>
				{
					return flowState
					.WithActions(actions =>
					{
						actions.Add(g => g.ReceiveShopOrder, a => a
						.IsDuplicatedInToolbar()
						.WithConnotation(ActionConnotation.Success));
					});
				});
				fss.Add<closed>(flowState =>
				{
					return flowState
					.WithFieldStates(states =>
					{
						states.AddField<CmpeProductionOrder.orderID>(state => state.IsDisabled());
						states.AddField<CmpeProductionOrder.productionOrderDate>(state => state.IsDisabled());
						states.AddField<CmpeProductionOrder.requestedDate>(state => state.IsDisabled());
						states.AddField<CmpeProductionOrder.productNumber>(state => state.IsDisabled());
						states.AddField<CmpeProductionOrder.lotSize>(state => state.IsDisabled());
					});
				});
			})
			.WithTransitions(transitions =>
			{
				transitions.Add(t => t
				  .From<released>()
				  .To<reserved>()
				  .IsTriggeredOn(g => g.IssueMaterial));
				transitions.Add(t => t
				  .From<reserved>()
				  .To<closed>()
				  .IsTriggeredOn(g => g.ReceiveShopOrder));
			}))
			.WithCategories(categories =>
			{
				categories.Add(processingCategory);
			})
			.WithActions(actions =>
			{
				actions.Add(g => g.IssueMaterial, c => c
						.WithCategory(processingCategory));
				actions.Add(g => g.ReceiveShopOrder, c => c
						.WithCategory(processingCategory));
			})
			);
		}
	}
}

