using System;
using Klei.AI;

// Token: 0x02000887 RID: 2183
public class MournMonitor : GameStateMachine<MournMonitor, MournMonitor.Instance>
{
	// Token: 0x06003F7F RID: 16255 RVA: 0x00162E30 File Offset: 0x00161030
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.EffectAdded, new GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.OnEffectAdded)).Enter(delegate(MournMonitor.Instance smi)
		{
			if (this.ShouldMourn(smi))
			{
				smi.GoTo(this.needsToMourn);
			}
		});
		this.needsToMourn.ToggleChore((MournMonitor.Instance smi) => new MournChore(smi.master), this.idle);
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x00162EA4 File Offset: 0x001610A4
	private bool ShouldMourn(MournMonitor.Instance smi)
	{
		Effect effect = Db.Get().effects.Get("Mourning");
		return smi.master.GetComponent<Effects>().HasEffect(effect);
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x00162ED7 File Offset: 0x001610D7
	private void OnEffectAdded(MournMonitor.Instance smi, object data)
	{
		if (this.ShouldMourn(smi))
		{
			smi.GoTo(this.needsToMourn);
		}
	}

	// Token: 0x0400293E RID: 10558
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x0400293F RID: 10559
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State needsToMourn;

	// Token: 0x02001695 RID: 5781
	public new class Instance : GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008B7B RID: 35707 RVA: 0x00316946 File Offset: 0x00314B46
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
