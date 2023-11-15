using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000894 RID: 2196
public class SneezeMonitor : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance>
{
	// Token: 0x06003FD0 RID: 16336 RVA: 0x00164E80 File Offset: 0x00163080
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.ParamTransition<bool>(this.isSneezy, this.sneezy, (SneezeMonitor.Instance smi, bool p) => p);
		this.sneezy.ParamTransition<bool>(this.isSneezy, this.idle, (SneezeMonitor.Instance smi, bool p) => !p).ToggleReactable((SneezeMonitor.Instance smi) => smi.GetReactable());
	}

	// Token: 0x04002980 RID: 10624
	public StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter isSneezy = new StateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x04002981 RID: 10625
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04002982 RID: 10626
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State taking_medicine;

	// Token: 0x04002983 RID: 10627
	public GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.State sneezy;

	// Token: 0x04002984 RID: 10628
	public const float SINGLE_SNEEZE_TIME_MINOR = 140f;

	// Token: 0x04002985 RID: 10629
	public const float SINGLE_SNEEZE_TIME_MAJOR = 70f;

	// Token: 0x04002986 RID: 10630
	public const float SNEEZE_TIME_VARIANCE = 0.3f;

	// Token: 0x04002987 RID: 10631
	public const float SHORT_SNEEZE_THRESHOLD = 5f;

	// Token: 0x020016B1 RID: 5809
	public new class Instance : GameStateMachine<SneezeMonitor, SneezeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008BF1 RID: 35825 RVA: 0x00317680 File Offset: 0x00315880
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sneezyness = Db.Get().Attributes.Sneezyness.Lookup(master.gameObject);
			this.OnSneezyChange();
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
		}

		// Token: 0x06008BF2 RID: 35826 RVA: 0x003176E1 File Offset: 0x003158E1
		public override void StopSM(string reason)
		{
			AttributeInstance attributeInstance = this.sneezyness;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, new System.Action(this.OnSneezyChange));
			base.StopSM(reason);
		}

		// Token: 0x06008BF3 RID: 35827 RVA: 0x00317714 File Offset: 0x00315914
		public float NextSneezeInterval()
		{
			if (this.sneezyness.GetTotalValue() <= 0f)
			{
				return 70f;
			}
			float num = (this.IsMinorSneeze() ? 140f : 70f) / this.sneezyness.GetTotalValue();
			return UnityEngine.Random.Range(num * 0.7f, num * 1.3f);
		}

		// Token: 0x06008BF4 RID: 35828 RVA: 0x0031776D File Offset: 0x0031596D
		public bool IsMinorSneeze()
		{
			return this.sneezyness.GetTotalValue() <= 5f;
		}

		// Token: 0x06008BF5 RID: 35829 RVA: 0x00317784 File Offset: 0x00315984
		private void OnSneezyChange()
		{
			base.smi.sm.isSneezy.Set(this.sneezyness.GetTotalValue() > 0f, base.smi, false);
		}

		// Token: 0x06008BF6 RID: 35830 RVA: 0x003177B8 File Offset: 0x003159B8
		public Reactable GetReactable()
		{
			float localCooldown = this.NextSneezeInterval();
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Sneeze", Db.Get().ChoreTypes.Cough, 0f, localCooldown, float.PositiveInfinity, 0f);
			string s = "sneeze";
			string s2 = "sneeze_pst";
			Emote emote = Db.Get().Emotes.Minion.Sneeze;
			if (this.IsMinorSneeze())
			{
				s = "sneeze_short";
				s2 = "sneeze_short_pst";
				emote = Db.Get().Emotes.Minion.Sneeze_Short;
			}
			selfEmoteReactable.SetEmote(emote);
			return selfEmoteReactable.RegisterEmoteStepCallbacks(s, new Action<GameObject>(this.TriggerDisurbance), null).RegisterEmoteStepCallbacks(s2, null, new Action<GameObject>(this.ResetSneeze));
		}

		// Token: 0x06008BF7 RID: 35831 RVA: 0x00317887 File Offset: 0x00315A87
		private void TriggerDisurbance(GameObject go)
		{
			if (this.IsMinorSneeze())
			{
				AcousticDisturbance.Emit(go, 2);
				return;
			}
			AcousticDisturbance.Emit(go, 3);
		}

		// Token: 0x06008BF8 RID: 35832 RVA: 0x003178A0 File Offset: 0x00315AA0
		private void ResetSneeze(GameObject go)
		{
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x04006C85 RID: 27781
		private AttributeInstance sneezyness;

		// Token: 0x04006C86 RID: 27782
		private StatusItem statusItem;
	}
}
