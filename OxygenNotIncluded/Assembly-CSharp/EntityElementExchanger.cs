using System;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class EntityElementExchanger : StateMachineComponent<EntityElementExchanger.StatesInstance>
{
	// Token: 0x06004025 RID: 16421 RVA: 0x001672C4 File Offset: 0x001654C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x001672CC File Offset: 0x001654CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x001672DF File Offset: 0x001654DF
	public void SetConsumptionRate(float consumptionRate)
	{
		this.consumeRate = consumptionRate;
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x001672E8 File Offset: 0x001654E8
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		EntityElementExchanger entityElementExchanger = (EntityElementExchanger)data;
		if (entityElementExchanger != null)
		{
			entityElementExchanger.OnSimConsume(mass_cb_info);
		}
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x0016730C File Offset: 0x0016550C
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		float num = mass_cb_info.mass * base.smi.master.exchangeRatio;
		if (this.reportExchange && base.smi.master.emittedElement == SimHashes.Oxygen)
		{
			string text = base.gameObject.GetProperName();
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			if (component != null && component.GetReceptacle() != null)
			{
				text = text + " (" + component.GetReceptacle().gameObject.GetProperName() + ")";
			}
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, num, text, null);
		}
		SimMessages.EmitMass(Grid.PosToCell(base.smi.master.transform.GetPosition() + this.outputOffset), ElementLoader.FindElementByHash(base.smi.master.emittedElement).idx, num, ElementLoader.FindElementByHash(base.smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
	}

	// Token: 0x040029DD RID: 10717
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x040029DE RID: 10718
	public bool reportExchange;

	// Token: 0x040029DF RID: 10719
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040029E0 RID: 10720
	public SimHashes consumedElement;

	// Token: 0x040029E1 RID: 10721
	public SimHashes emittedElement;

	// Token: 0x040029E2 RID: 10722
	public float consumeRate;

	// Token: 0x040029E3 RID: 10723
	public float exchangeRatio;

	// Token: 0x020016E0 RID: 5856
	public class StatesInstance : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.GameInstance
	{
		// Token: 0x06008CE2 RID: 36066 RVA: 0x00319FE6 File Offset: 0x003181E6
		public StatesInstance(EntityElementExchanger master) : base(master)
		{
		}
	}

	// Token: 0x020016E1 RID: 5857
	public class States : GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger>
	{
		// Token: 0x06008CE3 RID: 36067 RVA: 0x00319FF0 File Offset: 0x003181F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.exchanging;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.exchanging.Enter(delegate(EntityElementExchanger.StatesInstance smi)
			{
				WiltCondition component = smi.master.gameObject.GetComponent<WiltCondition>();
				if (component != null && component.IsWilting())
				{
					smi.GoTo(smi.sm.paused);
				}
			}).EventTransition(GameHashes.Wilt, this.paused, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementConsume, null).ToggleStatusItem(Db.Get().CreatureStatusItems.ExchangingElementOutput, null).Update("EntityElementExchanger", delegate(EntityElementExchanger.StatesInstance smi, float dt)
			{
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(EntityElementExchanger.OnSimConsumeCallback), smi.master, "EntityElementExchanger");
				SimMessages.ConsumeMass(Grid.PosToCell(smi.master.gameObject), smi.master.consumedElement, smi.master.consumeRate * dt, 3, handle.index);
			}, UpdateRate.SIM_1000ms, false);
			this.paused.EventTransition(GameHashes.WiltRecover, this.exchanging, null);
		}

		// Token: 0x04006D42 RID: 27970
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State exchanging;

		// Token: 0x04006D43 RID: 27971
		public GameStateMachine<EntityElementExchanger.States, EntityElementExchanger.StatesInstance, EntityElementExchanger, object>.State paused;
	}
}
