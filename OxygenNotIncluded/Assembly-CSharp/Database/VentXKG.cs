using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D4B RID: 3403
	public class VentXKG : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006ACB RID: 27339 RVA: 0x00299DF3 File Offset: 0x00297FF3
		public VentXKG(SimHashes element, float kilogramsToVent)
		{
			this.element = element;
			this.kilogramsToVent = kilogramsToVent;
		}

		// Token: 0x06006ACC RID: 27340 RVA: 0x00299E0C File Offset: 0x0029800C
		public override bool Success()
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return num >= this.kilogramsToVent;
		}

		// Token: 0x06006ACD RID: 27341 RVA: 0x00299ED4 File Offset: 0x002980D4
		public void Deserialize(IReader reader)
		{
			this.element = (SimHashes)reader.ReadInt32();
			this.kilogramsToVent = reader.ReadSingle();
		}

		// Token: 0x06006ACE RID: 27342 RVA: 0x00299EF0 File Offset: 0x002980F0
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.VENTED_MASS, GameUtil.GetFormattedMass(complete ? this.kilogramsToVent : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.kilogramsToVent, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
		}

		// Token: 0x04004DB4 RID: 19892
		private SimHashes element;

		// Token: 0x04004DB5 RID: 19893
		private float kilogramsToVent;
	}
}
