using System;
using STRINGS;

// Token: 0x020005E8 RID: 1512
public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	// Token: 0x060025B3 RID: 9651 RVA: 0x000CCCB8 File Offset: 0x000CAEB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DetectorNetwork.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (DetectorNetwork.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x0400158D RID: 5517
	public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;

	// Token: 0x0400158E RID: 5518
	public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;

	// Token: 0x0400158F RID: 5519
	public DetectorNetwork.NetworkStates operational;

	// Token: 0x02001281 RID: 4737
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001282 RID: 4738
	public class NetworkStates : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
	{
		// Token: 0x06007DA8 RID: 32168 RVA: 0x002E534C File Offset: 0x002E354C
		public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
		{
			base.DefaultState(this.poor);
			this.poor.ToggleStatusItem(BUILDING.STATUSITEMS.NETWORKQUALITY.NAME, BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, default(HashedString), 129022, new Func<string, DetectorNetwork.Instance, string>(this.StringCallback), null, null).ParamTransition<float>(parent.networkQuality, this.good, (DetectorNetwork.Instance smi, float p) => (double)p >= 0.8);
			this.good.ToggleStatusItem(BUILDING.STATUSITEMS.NETWORKQUALITY.NAME, BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, new Func<string, DetectorNetwork.Instance, string>(this.StringCallback), null, null).ParamTransition<float>(parent.networkQuality, this.poor, (DetectorNetwork.Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		// Token: 0x06007DA9 RID: 32169 RVA: 0x002E5450 File Offset: 0x002E3650
		private string StringCallback(string str, DetectorNetwork.Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			num = num.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(0f, 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f, GameUtil.TimeSlice.None)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min, "F0")).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max, "F0")).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04005FDE RID: 24542
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;

		// Token: 0x04005FDF RID: 24543
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;
	}

	// Token: 0x02001283 RID: 4739
	public new class Instance : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance
	{
		// Token: 0x06007DAB RID: 32171 RVA: 0x002E5520 File Offset: 0x002E3720
		public Instance(IStateMachineTarget master, DetectorNetwork.Def def) : base(master, def)
		{
		}

		// Token: 0x06007DAC RID: 32172 RVA: 0x002E552A File Offset: 0x002E372A
		public override void StartSM()
		{
			this.worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(this.worldId, this);
			base.StartSM();
		}

		// Token: 0x06007DAD RID: 32173 RVA: 0x002E5559 File Offset: 0x002E3759
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(this.worldId, this);
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x002E5573 File Offset: 0x002E3773
		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi, false);
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x002E558E File Offset: 0x002E378E
		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}

		// Token: 0x04005FE0 RID: 24544
		[NonSerialized]
		private int worldId;
	}
}
