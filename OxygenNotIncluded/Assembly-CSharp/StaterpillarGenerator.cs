using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000695 RID: 1685
public class StaterpillarGenerator : Generator
{
	// Token: 0x06002D2F RID: 11567 RVA: 0x000EFC98 File Offset: 0x000EDE98
	protected override void OnSpawn()
	{
		Staterpillar staterpillar = this.parent.Get();
		if (staterpillar == null || staterpillar.GetGenerator() != this)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.smi = new StaterpillarGenerator.StatesInstance(this);
		this.smi.StartSM();
		base.OnSpawn();
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x000EFCF4 File Offset: 0x000EDEF4
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = base.GetComponent<Generator>().WattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04001AA8 RID: 6824
	private StaterpillarGenerator.StatesInstance smi;

	// Token: 0x04001AA9 RID: 6825
	[Serialize]
	public Ref<Staterpillar> parent = new Ref<Staterpillar>();

	// Token: 0x020013B7 RID: 5047
	public class StatesInstance : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance
	{
		// Token: 0x060081FA RID: 33274 RVA: 0x002F746C File Offset: 0x002F566C
		public StatesInstance(StaterpillarGenerator master) : base(master)
		{
		}

		// Token: 0x0400632E RID: 25390
		private Attributes attributes;
	}

	// Token: 0x020013B8 RID: 5048
	public class States : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
	{
		// Token: 0x060081FB RID: 33275 RVA: 0x002F7478 File Offset: 0x002F5678
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StaterpillarGenerator.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StaterpillarGenerator.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(StaterpillarGenerator.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
		}

		// Token: 0x0400632F RID: 25391
		public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;
	}
}
