using System;
using System.Diagnostics;
using Klei.CustomSettings;

namespace Klei.AI
{
	// Token: 0x02000DFA RID: 3578
	[DebuggerDisplay("{base.Id}")]
	public class MeteorShowerSeason : GameplaySeason
	{
		// Token: 0x06006DE8 RID: 28136 RVA: 0x002B5690 File Offset: 0x002B3890
		public MeteorShowerSeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f) : base(id, type, dlcId, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod)
		{
			this.affectedByDifficultySettings = affectedByDifficultySettings;
			this.clusterTravelDuration = clusterTravelDuration;
		}

		// Token: 0x06006DE9 RID: 28137 RVA: 0x002B56D8 File Offset: 0x002B38D8
		public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
			(generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
		}

		// Token: 0x06006DEA RID: 28138 RVA: 0x002B56EC File Offset: 0x002B38EC
		public override float GetSeasonPeriod()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			float num = base.GetSeasonPeriod();
			if (this.affectedByDifficultySettings && currentQualitySetting != null)
			{
				string id = currentQualitySetting.id;
				if (id != null)
				{
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 2f;
						}
					}
					else
					{
						num *= 2f;
					}
				}
			}
			return num;
		}

		// Token: 0x04005271 RID: 21105
		public bool affectedByDifficultySettings = true;

		// Token: 0x04005272 RID: 21106
		public float clusterTravelDuration = -1f;
	}
}
