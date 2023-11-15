using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200089D RID: 2205
public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	// Token: 0x06003FF8 RID: 16376 RVA: 0x00166180 File Offset: 0x00164380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.safe;
		this.root.EventHandler(GameHashes.SafeFromThreats, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.safe.Enter(delegate(ThreatMonitor.Instance smi)
		{
			smi.revengeThreat.Clear();
			smi.RefreshThreat(null);
		}).Update("safe", delegate(ThreatMonitor.Instance smi, float dt)
		{
			smi.RefreshThreat(null);
		}, UpdateRate.SIM_1000ms, true);
		this.threatened.duplicant.Transition(this.safe, (ThreatMonitor.Instance smi) => !smi.CheckForThreats(), UpdateRate.SIM_200ms);
		this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe).Update("DupeUpdateTarget", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget), UpdateRate.SIM_200ms, false);
		this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
		this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (ThreatMonitor.Instance smi) => !smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (ThreatMonitor.Instance smi) => smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).Update("Threatened", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterUpdateThreats), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x0016639A File Offset: 0x0016459A
	private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.MainThreat == null || !smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			smi.Trigger(2144432245, null);
		}
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x001663C8 File Offset: 0x001645C8
	private static void CritterUpdateThreats(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934, null);
			return;
		}
		if (!smi.CheckForThreats())
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	// Token: 0x06003FFB RID: 16379 RVA: 0x0016642B File Offset: 0x0016462B
	private Chore CreateAttackChore(ThreatMonitor.Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x0016643E File Offset: 0x0016463E
	private Chore CreateFleeChore(ThreatMonitor.Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}

	// Token: 0x040029AB RID: 10667
	private FactionAlignment alignment;

	// Token: 0x040029AC RID: 10668
	private Navigator navigator;

	// Token: 0x040029AD RID: 10669
	public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State safe;

	// Token: 0x040029AE RID: 10670
	public ThreatMonitor.ThreatenedStates threatened;

	// Token: 0x020016C8 RID: 5832
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006CE0 RID: 27872
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		// Token: 0x04006CE1 RID: 27873
		public Tag[] friendlyCreatureTags;
	}

	// Token: 0x020016C9 RID: 5833
	public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04006CE2 RID: 27874
		public ThreatMonitor.ThreatenedDuplicantStates duplicant;

		// Token: 0x04006CE3 RID: 27875
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
	}

	// Token: 0x020016CA RID: 5834
	public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04006CE4 RID: 27876
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;

		// Token: 0x04006CE5 RID: 27877
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
	}

	// Token: 0x020016CB RID: 5835
	public struct Grudge
	{
		// Token: 0x06008C63 RID: 35939 RVA: 0x003187BC File Offset: 0x003169BC
		public void Reset(FactionAlignment revengeTarget)
		{
			this.target = revengeTarget;
			float num = 10f;
			this.grudgeTime = num;
		}

		// Token: 0x06008C64 RID: 35940 RVA: 0x003187E0 File Offset: 0x003169E0
		public bool Calm(float dt, FactionAlignment self)
		{
			if (this.grudgeTime <= 0f)
			{
				return true;
			}
			this.grudgeTime = Mathf.Max(0f, this.grudgeTime - dt);
			if (this.grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
				}
				this.Clear();
				return true;
			}
			return false;
		}

		// Token: 0x06008C65 RID: 35941 RVA: 0x00318873 File Offset: 0x00316A73
		public void Clear()
		{
			this.grudgeTime = 0f;
			this.target = null;
		}

		// Token: 0x06008C66 RID: 35942 RVA: 0x00318888 File Offset: 0x00316A88
		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			return this.target != null && this.target.IsAlignmentActive() && (this.target.health == null || !this.target.health.IsDefeated()) && (!isDuplicant || !this.target.IsPlayerTargeted());
		}

		// Token: 0x04006CE6 RID: 27878
		public FactionAlignment target;

		// Token: 0x04006CE7 RID: 27879
		public float grudgeTime;
	}

	// Token: 0x020016CC RID: 5836
	public new class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
	{
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x06008C67 RID: 35943 RVA: 0x003188EA File Offset: 0x00316AEA
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06008C68 RID: 35944 RVA: 0x003188F2 File Offset: 0x00316AF2
		public bool IAmADuplicant
		{
			get
			{
				return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
			}
		}

		// Token: 0x06008C69 RID: 35945 RVA: 0x00318904 File Offset: 0x00316B04
		public Instance(IStateMachineTarget master, ThreatMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.choreDriver = master.GetComponent<ChoreDriver>();
			this.health = master.GetComponent<Health>();
			this.choreConsumer = master.GetComponent<ChoreConsumer>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06008C6A RID: 35946 RVA: 0x00318972 File Offset: 0x00316B72
		public void ClearMainThreat()
		{
			this.SetMainThreat(null);
		}

		// Token: 0x06008C6B RID: 35947 RVA: 0x0031897C File Offset: 0x00316B7C
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x06008C6C RID: 35948 RVA: 0x00318A64 File Offset: 0x00316C64
		public void OnSafe(object data)
		{
			if (this.revengeThreat.target != null)
			{
				if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					this.revengeThreat.Clear();
				}
				this.ClearMainThreat();
			}
		}

		// Token: 0x06008C6D RID: 35949 RVA: 0x00318AA4 File Offset: 0x00316CA4
		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			this.revengeThreat.Reset(factionAlignment);
			if (this.mainThreat == null)
			{
				this.SetMainThreat(factionAlignment.gameObject);
				this.GoToThreatened();
			}
			else if (!this.WillFight())
			{
				this.GoToThreatened();
			}
			if (factionAlignment.GetComponent<Bee>())
			{
				Chore chore = (this.choreDriver != null) ? this.choreDriver.GetCurrentChore() : null;
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		// Token: 0x06008C6E RID: 35950 RVA: 0x00318B48 File Offset: 0x00316D48
		public bool WillFight()
		{
			if (this.choreConsumer != null)
			{
				if (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return this.health.State < base.smi.def.fleethresholdState;
		}

		// Token: 0x06008C6F RID: 35951 RVA: 0x00318BC4 File Offset: 0x00316DC4
		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
				return;
			}
			if (currentChore != null && currentChore.target != null && currentChore.target != base.master && currentChore.target.GetComponent<Pickupable>() != null)
			{
				return;
			}
			base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
		}

		// Token: 0x06008C70 RID: 35952 RVA: 0x00318C79 File Offset: 0x00316E79
		public void GoToThreatened()
		{
			if (this.IAmADuplicant)
			{
				this.GotoThreatResponse();
				return;
			}
			base.smi.GoTo(base.sm.threatened.creature);
		}

		// Token: 0x06008C71 RID: 35953 RVA: 0x00318CA5 File Offset: 0x00316EA5
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x06008C72 RID: 35954 RVA: 0x00318CE0 File Offset: 0x00316EE0
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (base.smi.GetCurrentState() != base.sm.safe)
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.safe);
			}
		}

		// Token: 0x06008C73 RID: 35955 RVA: 0x00318D44 File Offset: 0x00316F44
		public bool CheckForThreats()
		{
			GameObject x;
			if (this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant))
			{
				x = this.revengeThreat.target.gameObject;
			}
			else
			{
				x = this.FindThreat();
			}
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x06008C74 RID: 35956 RVA: 0x00318D8C File Offset: 0x00316F8C
		public GameObject FindThreat()
		{
			this.threats.Clear();
			if (base.isMasterNull)
			{
				return null;
			}
			bool flag = this.WillFight();
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			int radius = 20;
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), radius);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			for (int i = 0; i < pooledList.Count; i++)
			{
				FactionAlignment factionAlignment = pooledList[i].obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack)
				{
					if (base.def.friendlyCreatureTags != null)
					{
						bool flag2 = false;
						foreach (Tag tag in base.def.friendlyCreatureTags)
						{
							if (factionAlignment.HasTag(tag))
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							goto IL_127;
						}
					}
					if (this.navigator.CanReach(factionAlignment.attackable))
					{
						this.threats.Add(factionAlignment);
					}
				}
				IL_127:;
			}
			pooledList.Recycle();
			if (this.IAmADuplicant && flag)
			{
				for (int k = 0; k < 6; k++)
				{
					if (k != 0)
					{
						foreach (FactionAlignment factionAlignment2 in FactionManager.Instance.GetFaction((FactionManager.FactionID)k).Members)
						{
							if (factionAlignment2.IsPlayerTargeted() && !factionAlignment2.health.IsDefeated() && !this.threats.Contains(factionAlignment2) && this.navigator.CanReach(factionAlignment2.attackable))
							{
								this.threats.Add(factionAlignment2);
							}
						}
					}
				}
			}
			if (this.threats.Count == 0)
			{
				return null;
			}
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x06008C75 RID: 35957 RVA: 0x00318FB8 File Offset: 0x003171B8
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x04006CE8 RID: 27880
		public FactionAlignment alignment;

		// Token: 0x04006CE9 RID: 27881
		private Navigator navigator;

		// Token: 0x04006CEA RID: 27882
		public ChoreDriver choreDriver;

		// Token: 0x04006CEB RID: 27883
		private Health health;

		// Token: 0x04006CEC RID: 27884
		private ChoreConsumer choreConsumer;

		// Token: 0x04006CED RID: 27885
		public ThreatMonitor.Grudge revengeThreat;

		// Token: 0x04006CEE RID: 27886
		private GameObject mainThreat;

		// Token: 0x04006CEF RID: 27887
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x04006CF0 RID: 27888
		private Action<object> refreshThreatDelegate;
	}
}
