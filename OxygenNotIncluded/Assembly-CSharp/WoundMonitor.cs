using System;

// Token: 0x020008A3 RID: 2211
public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
	// Token: 0x06004013 RID: 16403 RVA: 0x00166BDC File Offset: 0x00164DDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		this.root.ToggleAnims("anim_hits_kanim", 0f, "").EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi, object data)
		{
			smi.OnHealthChanged(data);
		});
		this.healthy.EventTransition(GameHashes.HealthChanged, this.wounded, (WoundMonitor.Instance smi) => smi.health.State > Health.HealthState.Perfect);
		this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter(delegate(WoundMonitor.Instance smi)
		{
			switch (smi.health.State)
			{
			case Health.HealthState.Scuffed:
				smi.GoTo(this.wounded.light);
				return;
			case Health.HealthState.Injured:
				smi.GoTo(this.wounded.medium);
				return;
			case Health.HealthState.Critical:
				smi.GoTo(this.wounded.heavy);
				return;
			default:
				return;
			}
		}).EventHandler(GameHashes.HealthChanged, delegate(WoundMonitor.Instance smi)
		{
			smi.GoToProperHeathState();
		});
		this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f, "");
		this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f, "").Update("LookForAvailableClinic", delegate(WoundMonitor.Instance smi, float dt)
		{
			smi.FindAvailableMedicalBed();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x040029BD RID: 10685
	public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x040029BE RID: 10686
	public WoundMonitor.Wounded wounded;

	// Token: 0x020016D8 RID: 5848
	public class Wounded : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04006D25 RID: 27941
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;

		// Token: 0x04006D26 RID: 27942
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;

		// Token: 0x04006D27 RID: 27943
		public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
	}

	// Token: 0x020016D9 RID: 5849
	public new class Instance : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008CBC RID: 36028 RVA: 0x00319A0C File Offset: 0x00317C0C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.health = master.GetComponent<Health>();
			this.worker = master.GetComponent<Worker>();
		}

		// Token: 0x06008CBD RID: 36029 RVA: 0x00319A30 File Offset: 0x00317C30
		public void OnHealthChanged(object data)
		{
			float num = (float)data;
			if (this.health.hitPoints != 0f && num < 0f)
			{
				this.PlayHitAnimation();
			}
		}

		// Token: 0x06008CBE RID: 36030 RVA: 0x00319A64 File Offset: 0x00317C64
		private void PlayHitAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("hit"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
				if (text.Contains("impact"))
				{
					return;
				}
			}
			string s = "hit";
			AttackChore.StatesInstance smi = base.gameObject.GetSMI<AttackChore.StatesInstance>();
			if (smi != null && smi.GetCurrentState() == smi.sm.attack)
			{
				s = smi.master.GetHitAnim();
			}
			if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				s = "hit_ladder";
			}
			else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
			{
				s = "hit_pole";
			}
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x06008CBF RID: 36031 RVA: 0x00319BE8 File Offset: 0x00317DE8
		public void PlayKnockedOverImpactAnimation()
		{
			string text = null;
			KBatchedAnimController kbatchedAnimController = base.smi.Get<KBatchedAnimController>();
			if (kbatchedAnimController.CurrentAnim != null)
			{
				text = kbatchedAnimController.CurrentAnim.name;
			}
			KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
			if (text != null)
			{
				if (text.Contains("impact"))
				{
					return;
				}
				if (text.Contains("2_0"))
				{
					return;
				}
				if (text.Contains("2_1"))
				{
					return;
				}
				if (text.Contains("2_-1"))
				{
					return;
				}
				if (text.Contains("2_-2"))
				{
					return;
				}
				if (text.Contains("1_-1"))
				{
					return;
				}
				if (text.Contains("1_-2"))
				{
					return;
				}
				if (text.Contains("1_1"))
				{
					return;
				}
				if (text.Contains("1_2"))
				{
					return;
				}
				if (text.Contains("breathe_"))
				{
					return;
				}
				if (text.Contains("death_"))
				{
					return;
				}
			}
			string s = "impact";
			kbatchedAnimController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
			if (text != null)
			{
				kbatchedAnimController.Queue(text, playMode, 1f, 0f);
			}
		}

		// Token: 0x06008CC0 RID: 36032 RVA: 0x00319CF8 File Offset: 0x00317EF8
		public void GoToProperHeathState()
		{
			switch (base.smi.health.State)
			{
			case Health.HealthState.Perfect:
				base.smi.GoTo(base.sm.healthy);
				return;
			case Health.HealthState.Alright:
				break;
			case Health.HealthState.Scuffed:
				base.smi.GoTo(base.sm.wounded.light);
				break;
			case Health.HealthState.Injured:
				base.smi.GoTo(base.sm.wounded.medium);
				return;
			case Health.HealthState.Critical:
				base.smi.GoTo(base.sm.wounded.heavy);
				return;
			default:
				return;
			}
		}

		// Token: 0x06008CC1 RID: 36033 RVA: 0x00319D9B File Offset: 0x00317F9B
		public bool ShouldExitInfirmary()
		{
			return this.health.State == Health.HealthState.Perfect;
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x00319DB0 File Offset: 0x00317FB0
		public void FindAvailableMedicalBed()
		{
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			Ownables soleOwner = base.gameObject.GetComponent<MinionIdentity>().GetSoleOwner();
			if (soleOwner.GetSlot(clinic).assignable == null)
			{
				soleOwner.AutoAssignSlot(clinic);
			}
		}

		// Token: 0x04006D28 RID: 27944
		public Health health;

		// Token: 0x04006D29 RID: 27945
		private Worker worker;
	}
}
