using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF8 RID: 3576
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GameplaySeasonInstance : ISaveLoadable
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06006DDB RID: 28123 RVA: 0x002B5298 File Offset: 0x002B3498
		public float NextEventTime
		{
			get
			{
				return this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06006DDC RID: 28124 RVA: 0x002B52A7 File Offset: 0x002B34A7
		public GameplaySeason Season
		{
			get
			{
				if (this._season == null)
				{
					this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
				}
				return this._season;
			}
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x002B52D4 File Offset: 0x002B34D4
		public GameplaySeasonInstance(GameplaySeason season, int worldId)
		{
			this.seasonId = season.Id;
			this.worldId = worldId;
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			if (season.synchronizedToPeriod)
			{
				float seasonPeriod = this.Season.GetSeasonPeriod();
				this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
			}
			else
			{
				this.nextPeriodTime = currentTimeInCycles;
			}
			this.CalculateNextEventTime();
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x002B533C File Offset: 0x002B353C
		private void CalculateNextEventTime()
		{
			float seasonPeriod = this.Season.GetSeasonPeriod();
			this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			float num = this.nextPeriodTime + this.randomizedNextTime;
			while (num < currentTimeInCycles || num < this.Season.minCycle)
			{
				this.nextPeriodTime += seasonPeriod;
				num = this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x002B53C4 File Offset: 0x002B35C4
		public bool StartEvent(bool ignorePreconditions = false)
		{
			bool result = false;
			this.CalculateNextEventTime();
			this.numStartEvents++;
			List<GameplayEvent> list;
			if (!ignorePreconditions)
			{
				list = (from x in this.Season.events
				where x.IsAllowed()
				select x).ToList<GameplayEvent>();
			}
			else
			{
				list = this.Season.events;
			}
			List<GameplayEvent> list2 = list;
			if (list2.Count > 0)
			{
				list2.ForEach(delegate(GameplayEvent x)
				{
					x.CalculatePriority();
				});
				list2.Sort();
				int maxExclusive = Mathf.Min(list2.Count, 5);
				GameplayEvent eventType = list2[UnityEngine.Random.Range(0, maxExclusive)];
				GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
				result = true;
			}
			this.allEventWillNotRunAgain = true;
			using (List<GameplayEvent>.Enumerator enumerator = this.Season.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WillNeverRunAgain())
					{
						this.allEventWillNotRunAgain = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x002B5500 File Offset: 0x002B3700
		public bool ShouldGenerateEvents()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
			if (!world.IsDupeVisited && !world.IsRoverVisted)
			{
				return false;
			}
			if ((this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents) || this.allEventWillNotRunAgain)
			{
				return false;
			}
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			return currentTimeInCycles > this.Season.minCycle && currentTimeInCycles < this.Season.maxCycle;
		}

		// Token: 0x04005262 RID: 21090
		public const int LIMIT_SELECTION = 5;

		// Token: 0x04005263 RID: 21091
		[Serialize]
		public int numStartEvents;

		// Token: 0x04005264 RID: 21092
		[Serialize]
		public int worldId;

		// Token: 0x04005265 RID: 21093
		[Serialize]
		private readonly string seasonId;

		// Token: 0x04005266 RID: 21094
		[Serialize]
		private float nextPeriodTime;

		// Token: 0x04005267 RID: 21095
		[Serialize]
		private float randomizedNextTime;

		// Token: 0x04005268 RID: 21096
		private bool allEventWillNotRunAgain;

		// Token: 0x04005269 RID: 21097
		private GameplaySeason _season;
	}
}
