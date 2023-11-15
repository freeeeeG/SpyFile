using System;
using UnityEngine;

// Token: 0x020007DD RID: 2013
public class ElementSpout : StateMachineComponent<ElementSpout.StatesInstance>
{
	// Token: 0x060038D1 RID: 14545 RVA: 0x0013B8B8 File Offset: 0x00139AB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Grid.Objects[cell, 2] = base.gameObject;
		base.smi.StartSM();
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x0013B8F9 File Offset: 0x00139AF9
	public void SetEmitter(ElementEmitter emitter)
	{
		this.emitter = emitter;
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x0013B902 File Offset: 0x00139B02
	public void ConfigureEmissionSettings(float emissionPollFrequency = 3f, float emissionIrregularity = 1.5f, float maxPressure = 1.5f, float perEmitAmount = 0.5f)
	{
		this.maxPressure = maxPressure;
		this.emissionPollFrequency = emissionPollFrequency;
		this.emissionIrregularity = emissionIrregularity;
		this.perEmitAmount = perEmitAmount;
	}

	// Token: 0x040025AA RID: 9642
	[SerializeField]
	private ElementEmitter emitter;

	// Token: 0x040025AB RID: 9643
	[MyCmpAdd]
	private KBatchedAnimController anim;

	// Token: 0x040025AC RID: 9644
	public float maxPressure = 1.5f;

	// Token: 0x040025AD RID: 9645
	public float emissionPollFrequency = 3f;

	// Token: 0x040025AE RID: 9646
	public float emissionIrregularity = 1.5f;

	// Token: 0x040025AF RID: 9647
	public float perEmitAmount = 0.5f;

	// Token: 0x0200158C RID: 5516
	public class StatesInstance : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.GameInstance
	{
		// Token: 0x060087F8 RID: 34808 RVA: 0x0030D047 File Offset: 0x0030B247
		public StatesInstance(ElementSpout smi) : base(smi)
		{
		}

		// Token: 0x060087F9 RID: 34809 RVA: 0x0030D050 File Offset: 0x0030B250
		private bool CanEmitOnCell(int cell, float max_pressure, Element.State expected_state)
		{
			return Grid.Mass[cell] < max_pressure && (Grid.Element[cell].IsState(expected_state) || Grid.Element[cell].IsVacuum);
		}

		// Token: 0x060087FA RID: 34810 RVA: 0x0030D080 File Offset: 0x0030B280
		public bool CanEmitAnywhere()
		{
			int cell = Grid.PosToCell(base.smi.transform.GetPosition());
			int cell2 = Grid.CellLeft(cell);
			int cell3 = Grid.CellRight(cell);
			int cell4 = Grid.CellAbove(cell);
			Element.State state = ElementLoader.FindElementByHash(base.smi.master.emitter.outputElement.elementHash).state;
			return false || this.CanEmitOnCell(cell, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell2, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell3, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell4, base.smi.master.maxPressure, state);
		}
	}

	// Token: 0x0200158D RID: 5517
	public class States : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout>
	{
		// Token: 0x060087FB RID: 34811 RVA: 0x0030D158 File Offset: 0x0030B358
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DefaultState(this.idle.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
			}).ScheduleGoTo((ElementSpout.StatesInstance smi) => smi.master.emissionPollFrequency, this.emit);
			this.idle.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.idle.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.DefaultState(this.emit.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				float num = 1f + UnityEngine.Random.Range(0f, smi.master.emissionIrregularity);
				float massGenerationRate = smi.master.perEmitAmount / num;
				smi.master.emitter.SetEmitting(true);
				smi.master.emitter.emissionFrequency = 1f;
				smi.master.emitter.outputElement.massGenerationRate = massGenerationRate;
				smi.ScheduleGoTo(num, this.idle);
			});
			this.emit.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutEmitting, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("emit", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(true);
			}).Transition(this.emit.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(false);
			}).Transition(this.emit.unblocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
		}

		// Token: 0x040068E9 RID: 26857
		public ElementSpout.States.Idle idle;

		// Token: 0x040068EA RID: 26858
		public ElementSpout.States.Emitting emit;

		// Token: 0x02002186 RID: 8582
		public class Idle : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x04009623 RID: 38435
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x04009624 RID: 38436
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}

		// Token: 0x02002187 RID: 8583
		public class Emitting : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x04009625 RID: 38437
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x04009626 RID: 38438
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}
	}
}
