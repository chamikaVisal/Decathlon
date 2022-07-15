using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects.Common;
using PX.Objects.DC.DAC;
using static PX.Objects.DC.DAC.CmpeProductionOrder;
using static PX.Objects.DC.Descriptor.Constants;

namespace PX.Objects.DC.Workflows
{
	public class IBProductionOrderWorkflow : PX.Data.PXGraphExtension<CmpeProductionOrderMaint>
	{
		public static bool IsActive() => false;

		#region Constants
		public static class States
		{
			public const string NotSet = ProductionOrderStatuses.Not_Set;
			public const string Released = ProductionOrderStatuses.Released;
			public const string Reserved = ProductionOrderStatuses.Reserved;
			public const string Closed = ProductionOrderStatuses.Closed;

			public class notSet : PX.Data.BQL.BqlString.Constant<notSet>
			{
				public notSet() : base(NotSet) { }
			}
			public class released : PX.Data.BQL.BqlString.Constant<released>
			{
				public released() : base(Released) { }
			}
			public class reserved : PX.Data.BQL.BqlString.Constant<reserved>
			{
				public reserved() : base(Reserved) { }
			}
			public class closed : PX.Data.BQL.BqlString.Constant<closed>
			{
				public closed() : base(Closed) { }
			}
		}
		#endregion

		public override void Configure(PXScreenConfiguration config)
		{
			var context = config.GetScreenConfigurationContext<CmpeProductionOrderMaint, CmpeProductionOrder>();

			//#region Categories
			//var commonCategories = CommonActionCategories.Get(context);
			//var processingCategory = commonCategories.Processing;
			//#endregion

			context.AddScreenConfigurationFor(screen =>
			{
				return screen
					.StateIdentifierIs<productionOrderStatus>()
					.AddDefaultFlow(flow =>
						flow.WithFlowStates(flowStates =>
						{
							flowStates.Add<States.notSet>(flowState =>
							{
								return flowState
								.IsInitial()
								.WithActions(actions =>
								{
									actions.Add(g => g.Release, a => a.IsDuplicatedInToolbar());
								});
							});
							flowStates.Add<States.released>(flowState =>
							{
								return flowState
									.WithActions(actions =>
									{
										actions.Add(g => g.IssueMaterial, a => a.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
									})
									.WithFieldStates(states =>
									{
										states.AddField<CmpeProductionOrder.orderID>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.productionOrderDate>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.requestedDate>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.productNumber>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.lotSize>(state => state.IsDisabled());
									});
							});
							flowStates.Add<States.reserved>(flowState =>
							{
								return flowState
									.WithActions(actions =>
									{
										actions.Add(g => g.ReceiveShopOrder, a => a.IsDuplicatedInToolbar().WithConnotation(ActionConnotation.Success));
									})
									.WithEventHandlers(handlers =>
									{
										handlers.Add(g => g.OnSaveReceiveStock);
									})
									.WithFieldStates(states =>
									{
										states.AddField<CmpeProductionOrder.orderID>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.productionOrderDate>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.requestedDate>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.productNumber>(state => state.IsDisabled());
										states.AddField<CmpeProductionOrder.lotSize>(state => state.IsDisabled());
									});
							});
							flowStates.Add<States.closed>(flowState =>
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
								transitions.Add(t => t.From<States.notSet>().To<States.released>().IsTriggeredOn(g => g.Release));
								transitions.Add(t => t.From<States.released>().To<States.reserved>().IsTriggeredOn(g => g.IssueMaterial));
								transitions.Add(t => t.From<States.reserved>().To<States.closed>().IsTriggeredOn(g => g.OnSaveReceiveStock));
							})
					)
					.WithHandlers(handlers =>
					{
						handlers.Add(handler => handler
						.WithTargetOf<CmpeProductionOrderAllocation>()
						.OfEntityEvent<CmpeProductionOrderAllocation.Events>(e => e.SaveDocument)
						.Is(g => g.OnSaveReceiveStock)
						.UsesPrimaryEntityGetter<SelectFrom<CmpeProductionOrder>.Where<productNumber.IsEqual<CmpeProductionOrderAllocation.productID.FromCurrent>>>());
					})
					.WithActions(actions =>
					{
						actions.Add(g => g.Release);
						actions.Add(g => g.IssueMaterial);
						actions.Add(g => g.ReceiveShopOrder);
					});
			});
		}
	}
}

