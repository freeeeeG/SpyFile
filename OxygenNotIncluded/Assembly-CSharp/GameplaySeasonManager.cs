using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;

// Token: 0x020007D6 RID: 2006
public class GameplaySeasonManager : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
	// Token: 0x0600389D RID: 14493 RVA: 0x0013AC08 File Offset: 0x00138E08
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(GameplaySeasonManager.Instance smi)
		{
			smi.Initialize();
		}).Update(delegate(GameplaySeasonManager.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_4000ms, false);
	}

	// Token: 0x02001581 RID: 5505
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001582 RID: 5506
	public new class Instance : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
	{
		// Token: 0x060087C9 RID: 34761 RVA: 0x0030C755 File Offset: 0x0030A955
		public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def) : base(master, def)
		{
			this.activeSeasons = new List<GameplaySeasonInstance>();
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x0030C76C File Offset: 0x0030A96C
		public void Initialize()
		{
			this.activeSeasons.RemoveAll((GameplaySeasonInstance item) => item.Season == null);
			List<GameplaySeason> list = new List<GameplaySeason>();
			if (this.m_worldContainer != null)
			{
				ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
				using (List<string>.Enumerator enumerator = this.m_worldContainer.GetSeasonIds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(text);
						if (gameplaySeason == null)
						{
							Debug.LogWarning("world " + component.name + " has invalid season " + text);
						}
						else
						{
							if (gameplaySeason.type != GameplaySeason.Type.World)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"world ",
									component.name,
									" has specified season ",
									text,
									", which is not a world type season"
								}));
							}
							list.Add(gameplaySeason);
						}
					}
					goto IL_146;
				}
			}
			Debug.Assert(base.GetComponent<SaveGame>() != null);
			list = (from season in Db.Get().GameplaySeasons.resources
			where season.type == GameplaySeason.Type.Cluster
			select season).ToList<GameplaySeason>();
			IL_146:
			foreach (GameplaySeason gameplaySeason2 in list)
			{
				if (DlcManager.IsContentActive(gameplaySeason2.dlcId) && gameplaySeason2.startActive && !this.SeasonExists(gameplaySeason2) && gameplaySeason2.events.Count > 0)
				{
					this.activeSeasons.Add(gameplaySeason2.Instantiate(this.GetWorldId()));
				}
			}
			foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>(this.activeSeasons))
			{
				if (!list.Contains(gameplaySeasonInstance.Season) || !DlcManager.IsContentActive(gameplaySeasonInstance.Season.dlcId))
				{
					this.activeSeasons.Remove(gameplaySeasonInstance);
				}
			}
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x0030C9C4 File Offset: 0x0030ABC4
		private int GetWorldId()
		{
			if (this.m_worldContainer != null)
			{
				return this.m_worldContainer.id;
			}
			return -1;
		}

		// Token: 0x060087CC RID: 34764 RVA: 0x0030C9E4 File Offset: 0x0030ABE4
		public void Update(float dt)
		{
			foreach (GameplaySeasonInstance gameplaySeasonInstance in this.activeSeasons)
			{
				if (gameplaySeasonInstance.ShouldGenerateEvents() && GameUtil.GetCurrentTimeInCycles() > gameplaySeasonInstance.NextEventTime)
				{
					int num = 0;
					while (num < gameplaySeasonInstance.Season.numEventsToStartEachPeriod && gameplaySeasonInstance.StartEvent(false))
					{
						num++;
					}
				}
			}
		}

		// Token: 0x060087CD RID: 34765 RVA: 0x0030CA64 File Offset: 0x0030AC64
		public void StartNewSeason(GameplaySeason seasonType)
		{
			if (DlcManager.IsContentActive(seasonType.dlcId))
			{
				this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
			}
		}

		// Token: 0x060087CE RID: 34766 RVA: 0x0030CA8C File Offset: 0x0030AC8C
		public bool SeasonExists(GameplaySeason seasonType)
		{
			return this.activeSeasons.Find((GameplaySeasonInstance e) => e.Season.IdHash == seasonType.IdHash) != null;
		}

		// Token: 0x040068C4 RID: 26820
		[Serialize]
		public List<GameplaySeasonInstance> activeSeasons;

		// Token: 0x040068C5 RID: 26821
		[MyCmpGet]
		private WorldContainer m_worldContainer;
	}
}
