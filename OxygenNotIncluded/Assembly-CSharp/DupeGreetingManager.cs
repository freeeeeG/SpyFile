using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000777 RID: 1911
[AddComponentMenu("KMonoBehaviour/scripts/DupeGreetingManager")]
public class DupeGreetingManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x060034DB RID: 13531 RVA: 0x0011BC97 File Offset: 0x00119E97
	protected override void OnPrefabInit()
	{
		this.candidateCells = new Dictionary<int, MinionIdentity>();
		this.activeSetups = new List<DupeGreetingManager.GreetingSetup>();
		this.cooldowns = new Dictionary<MinionIdentity, float>();
	}

	// Token: 0x060034DC RID: 13532 RVA: 0x0011BCBC File Offset: 0x00119EBC
	public void Sim200ms(float dt)
	{
		if (GameClock.Instance.GetTime() / 600f < TuningData<DupeGreetingManager.Tuning>.Get().cyclesBeforeFirstGreeting)
		{
			return;
		}
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			DupeGreetingManager.GreetingSetup greetingSetup = this.activeSetups[i];
			if (!this.ValidNavigatingMinion(greetingSetup.A.minion) || !this.ValidOppositionalMinion(greetingSetup.A.minion, greetingSetup.B.minion))
			{
				greetingSetup.A.reactable.Cleanup();
				greetingSetup.B.reactable.Cleanup();
				this.activeSetups.RemoveAt(i);
			}
		}
		this.candidateCells.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if ((!this.cooldowns.ContainsKey(minionIdentity) || GameClock.Instance.GetTime() - this.cooldowns[minionIdentity] >= 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier) && this.ValidNavigatingMinion(minionIdentity))
			{
				for (int j = 0; j <= 2; j++)
				{
					int offsetCell = this.GetOffsetCell(minionIdentity, j);
					if (this.candidateCells.ContainsKey(offsetCell) && this.ValidOppositionalMinion(minionIdentity, this.candidateCells[offsetCell]))
					{
						this.BeginNewGreeting(minionIdentity, this.candidateCells[offsetCell], offsetCell);
						break;
					}
					this.candidateCells[offsetCell] = minionIdentity;
				}
			}
		}
	}

	// Token: 0x060034DD RID: 13533 RVA: 0x0011BE64 File Offset: 0x0011A064
	private int GetOffsetCell(MinionIdentity minion, int offset)
	{
		if (!minion.GetComponent<Facing>().GetFacing())
		{
			return Grid.OffsetCell(Grid.PosToCell(minion), offset, 0);
		}
		return Grid.OffsetCell(Grid.PosToCell(minion), -offset, 0);
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x0011BE90 File Offset: 0x0011A090
	private bool ValidNavigatingMinion(MinionIdentity minion)
	{
		if (minion == null)
		{
			return false;
		}
		Navigator component = minion.GetComponent<Navigator>();
		return component != null && component.IsMoving() && component.CurrentNavType == NavType.Floor;
	}

	// Token: 0x060034DF RID: 13535 RVA: 0x0011BECC File Offset: 0x0011A0CC
	private bool ValidOppositionalMinion(MinionIdentity reference_minion, MinionIdentity minion)
	{
		if (reference_minion == null)
		{
			return false;
		}
		if (minion == null)
		{
			return false;
		}
		Facing component = minion.GetComponent<Facing>();
		Facing component2 = reference_minion.GetComponent<Facing>();
		return this.ValidNavigatingMinion(minion) && component != null && component2 != null && component.GetFacing() != component2.GetFacing();
	}

	// Token: 0x060034E0 RID: 13536 RVA: 0x0011BF2C File Offset: 0x0011A12C
	private void BeginNewGreeting(MinionIdentity minion_a, MinionIdentity minion_b, int cell)
	{
		DupeGreetingManager.GreetingSetup greetingSetup = new DupeGreetingManager.GreetingSetup();
		greetingSetup.cell = cell;
		greetingSetup.A = new DupeGreetingManager.GreetingUnit(minion_a, this.GetReactable(minion_a));
		greetingSetup.B = new DupeGreetingManager.GreetingUnit(minion_b, this.GetReactable(minion_b));
		this.activeSetups.Add(greetingSetup);
	}

	// Token: 0x060034E1 RID: 13537 RVA: 0x0011BF78 File Offset: 0x0011A178
	private Reactable GetReactable(MinionIdentity minion)
	{
		if (DupeGreetingManager.emotes == null)
		{
			DupeGreetingManager.emotes = new List<Emote>
			{
				Db.Get().Emotes.Minion.Wave,
				Db.Get().Emotes.Minion.Wave_Shy,
				Db.Get().Emotes.Minion.FingerGuns
			};
		}
		Emote emote = DupeGreetingManager.emotes[UnityEngine.Random.Range(0, DupeGreetingManager.emotes.Count)];
		SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(minion.gameObject, "NavigatorPassingGreeting", Db.Get().ChoreTypes.Emote, 1000f, 20f, float.PositiveInfinity, 0f);
		selfEmoteReactable.SetEmote(emote).SetThought(Db.Get().Thoughts.Chatty);
		selfEmoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.BeginReacting), null);
		return selfEmoteReactable;
	}

	// Token: 0x060034E2 RID: 13538 RVA: 0x0011C074 File Offset: 0x0011A274
	private void BeginReacting(GameObject minionGO)
	{
		if (minionGO == null)
		{
			return;
		}
		MinionIdentity component = minionGO.GetComponent<MinionIdentity>();
		Vector3 vector = Vector3.zero;
		foreach (DupeGreetingManager.GreetingSetup greetingSetup in this.activeSetups)
		{
			if (greetingSetup.A.minion == component)
			{
				if (greetingSetup.B.minion != null)
				{
					vector = greetingSetup.B.minion.transform.GetPosition();
					greetingSetup.A.minion.Trigger(-594200555, greetingSetup.B.minion);
					greetingSetup.B.minion.Trigger(-594200555, greetingSetup.A.minion);
					break;
				}
				break;
			}
			else if (greetingSetup.B.minion == component)
			{
				if (greetingSetup.A.minion != null)
				{
					vector = greetingSetup.A.minion.transform.GetPosition();
					break;
				}
				break;
			}
		}
		minionGO.GetComponent<Facing>().SetFacing(vector.x < minionGO.transform.GetPosition().x);
		minionGO.GetComponent<Effects>().Add("Greeting", true);
		this.cooldowns[component] = GameClock.Instance.GetTime();
	}

	// Token: 0x04001FF1 RID: 8177
	private const float COOLDOWN_TIME = 720f;

	// Token: 0x04001FF2 RID: 8178
	private Dictionary<int, MinionIdentity> candidateCells;

	// Token: 0x04001FF3 RID: 8179
	private List<DupeGreetingManager.GreetingSetup> activeSetups;

	// Token: 0x04001FF4 RID: 8180
	private Dictionary<MinionIdentity, float> cooldowns;

	// Token: 0x04001FF5 RID: 8181
	private static List<Emote> emotes;

	// Token: 0x020014F3 RID: 5363
	public class Tuning : TuningData<DupeGreetingManager.Tuning>
	{
		// Token: 0x040066DE RID: 26334
		public float cyclesBeforeFirstGreeting;

		// Token: 0x040066DF RID: 26335
		public float greetingDelayMultiplier;
	}

	// Token: 0x020014F4 RID: 5364
	private class GreetingUnit
	{
		// Token: 0x0600865A RID: 34394 RVA: 0x003088D6 File Offset: 0x00306AD6
		public GreetingUnit(MinionIdentity minion, Reactable reactable)
		{
			this.minion = minion;
			this.reactable = reactable;
		}

		// Token: 0x040066E0 RID: 26336
		public MinionIdentity minion;

		// Token: 0x040066E1 RID: 26337
		public Reactable reactable;
	}

	// Token: 0x020014F5 RID: 5365
	private class GreetingSetup
	{
		// Token: 0x040066E2 RID: 26338
		public int cell;

		// Token: 0x040066E3 RID: 26339
		public DupeGreetingManager.GreetingUnit A;

		// Token: 0x040066E4 RID: 26340
		public DupeGreetingManager.GreetingUnit B;
	}
}
