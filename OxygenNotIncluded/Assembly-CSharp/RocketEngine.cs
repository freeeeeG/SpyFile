using System;
using KSerialization;
using STRINGS;

// Token: 0x020009B3 RID: 2483
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
	// Token: 0x060049EF RID: 18927 RVA: 0x001A0628 File Offset: 0x0019E828
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(FuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
		}
	}

	// Token: 0x0400309A RID: 12442
	public float exhaustEmitRate = 50f;

	// Token: 0x0400309B RID: 12443
	public float exhaustTemperature = 1500f;

	// Token: 0x0400309C RID: 12444
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x0400309D RID: 12445
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x0400309E RID: 12446
	public Tag fuelTag;

	// Token: 0x0400309F RID: 12447
	public float efficiency = 1f;

	// Token: 0x040030A0 RID: 12448
	public bool requireOxidizer = true;

	// Token: 0x040030A1 RID: 12449
	public bool mainEngine = true;

	// Token: 0x02001839 RID: 6201
	public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
	{
		// Token: 0x0600912C RID: 37164 RVA: 0x00328D66 File Offset: 0x00326F66
		public StatesInstance(RocketEngine smi) : base(smi)
		{
		}
	}

	// Token: 0x0200183A RID: 6202
	public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
	{
		// Token: 0x0600912D RID: 37165 RVA: 0x00328D70 File Offset: 0x00326F70
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, null).Update(delegate(RocketEngine.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num))
				{
					SimMessages.EmitMass(num, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				int num2 = 10;
				for (int i = 1; i < num2; i++)
				{
					int num3 = Grid.OffsetCell(num, -1, -i);
					int num4 = Grid.OffsetCell(num, 0, -i);
					int num5 = Grid.OffsetCell(num, 1, -i);
					if (Grid.IsValidCell(num3))
					{
						SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num4))
					{
						SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num5))
					{
						SimMessages.ModifyEnergy(num5, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
		}

		// Token: 0x04007163 RID: 29027
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;

		// Token: 0x04007164 RID: 29028
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;

		// Token: 0x04007165 RID: 29029
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;
	}
}
