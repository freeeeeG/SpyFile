using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02000DF7 RID: 3575
	[DebuggerDisplay("{base.Id}")]
	public class GameplaySeason : Resource
	{
		// Token: 0x06006DD6 RID: 28118 RVA: 0x002B5178 File Offset: 0x002B3378
		public GameplaySeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1) : base(id, null, null)
		{
			this.type = type;
			this.dlcId = dlcId;
			this.period = period;
			this.synchronizedToPeriod = synchronizedToPeriod;
			global::Debug.Assert(period > 0f, "Season " + id + "'s Period cannot be 0 or negative");
			if (randomizedEventStartTime == -1f)
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(--0f * period, 0f * period);
			}
			else
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
				DebugUtil.DevAssert((this.randomizedEventStartTime.max - this.randomizedEventStartTime.min) * 0.4f < period, string.Format("Season {0} randomizedEventStartTime is greater than {1}% of its period.", id, 0.4f), null);
			}
			this.startActive = startActive;
			this.finishAfterNumEvents = finishAfterNumEvents;
			this.minCycle = minCycle;
			this.maxCycle = maxCycle;
			this.events = new List<GameplayEvent>();
			this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x002B5276 File Offset: 0x002B3476
		public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x002B5278 File Offset: 0x002B3478
		public virtual float GetSeasonPeriod()
		{
			return this.period;
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x002B5280 File Offset: 0x002B3480
		public GameplaySeason AddEvent(GameplayEvent evt)
		{
			this.events.Add(evt);
			return this;
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x002B528F File Offset: 0x002B348F
		public virtual GameplaySeasonInstance Instantiate(int worldId)
		{
			return new GameplaySeasonInstance(this, worldId);
		}

		// Token: 0x04005253 RID: 21075
		public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0f;

		// Token: 0x04005254 RID: 21076
		public const float PERCENTAGE_WARNING = 0.4f;

		// Token: 0x04005255 RID: 21077
		public const float USE_DEFAULT = -1f;

		// Token: 0x04005256 RID: 21078
		public const int INFINITE = -1;

		// Token: 0x04005257 RID: 21079
		public float period;

		// Token: 0x04005258 RID: 21080
		public bool synchronizedToPeriod;

		// Token: 0x04005259 RID: 21081
		public MathUtil.MinMax randomizedEventStartTime;

		// Token: 0x0400525A RID: 21082
		public int finishAfterNumEvents = -1;

		// Token: 0x0400525B RID: 21083
		public bool startActive;

		// Token: 0x0400525C RID: 21084
		public int numEventsToStartEachPeriod;

		// Token: 0x0400525D RID: 21085
		public float minCycle;

		// Token: 0x0400525E RID: 21086
		public float maxCycle;

		// Token: 0x0400525F RID: 21087
		public List<GameplayEvent> events;

		// Token: 0x04005260 RID: 21088
		public GameplaySeason.Type type;

		// Token: 0x04005261 RID: 21089
		public string dlcId;

		// Token: 0x02001F7A RID: 8058
		public enum Type
		{
			// Token: 0x04008E3C RID: 36412
			World,
			// Token: 0x04008E3D RID: 36413
			Cluster
		}
	}
}
