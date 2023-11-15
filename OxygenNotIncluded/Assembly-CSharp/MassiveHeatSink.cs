using System;
using STRINGS;

// Token: 0x0200064E RID: 1614
public class MassiveHeatSink : StateMachineComponent<MassiveHeatSink.StatesInstance>
{
	// Token: 0x06002A8D RID: 10893 RVA: 0x000E2E40 File Offset: 0x000E1040
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x040018E0 RID: 6368
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040018E1 RID: 6369
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x02001324 RID: 4900
	public class States : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink>
	{
		// Token: 0x06007FD0 RID: 32720 RVA: 0x002EE478 File Offset: 0x002EC678
		private string AwaitingFuelResolveString(string str, object obj)
		{
			ElementConverter elementConverter = ((MassiveHeatSink.StatesInstance)obj).master.elementConverter;
			string arg = elementConverter.consumedElements[0].Tag.ProperName();
			string formattedMass = GameUtil.GetFormattedMass(elementConverter.consumedElements[0].MassConsumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
			str = string.Format(str, arg, formattedMass);
			return str;
		}

		// Token: 0x06007FD1 RID: 32721 RVA: 0x002EE4D8 File Offset: 0x002EC6D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.idle, (MassiveHeatSink.StatesInstance smi) => smi.master.operational.IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational).ToggleStatusItem(BUILDING.STATUSITEMS.AWAITINGFUEL.NAME, BUILDING.STATUSITEMS.AWAITINGFUEL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, default(HashedString), 129022, new Func<string, MassiveHeatSink.StatesInstance, string>(this.AwaitingFuelResolveString), null, null).EventTransition(GameHashes.OnStorageChange, this.active, (MassiveHeatSink.StatesInstance smi) => smi.master.elementConverter.HasEnoughMassToStartConverting(false));
			this.active.EventTransition(GameHashes.OperationalChanged, this.disabled, (MassiveHeatSink.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OnStorageChange, this.idle, (MassiveHeatSink.StatesInstance smi) => !smi.master.elementConverter.HasEnoughMassToStartConverting(false)).Enter(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(MassiveHeatSink.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x04006186 RID: 24966
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State disabled;

		// Token: 0x04006187 RID: 24967
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State idle;

		// Token: 0x04006188 RID: 24968
		public GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.State active;
	}

	// Token: 0x02001325 RID: 4901
	public class StatesInstance : GameStateMachine<MassiveHeatSink.States, MassiveHeatSink.StatesInstance, MassiveHeatSink, object>.GameInstance
	{
		// Token: 0x06007FD3 RID: 32723 RVA: 0x002EE67A File Offset: 0x002EC87A
		public StatesInstance(MassiveHeatSink master) : base(master)
		{
		}
	}
}
