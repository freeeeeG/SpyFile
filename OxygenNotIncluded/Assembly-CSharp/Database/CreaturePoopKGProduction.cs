using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D55 RID: 3413
	public class CreaturePoopKGProduction : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AF4 RID: 27380 RVA: 0x0029A999 File Offset: 0x00298B99
		public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
		{
			this.poopElement = poopElement;
			this.amountToPoop = amountToPoop;
		}

		// Token: 0x06006AF5 RID: 27381 RVA: 0x0029A9B0 File Offset: 0x00298BB0
		public override bool Success()
		{
			return Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement) && Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= this.amountToPoop;
		}

		// Token: 0x06006AF6 RID: 27382 RVA: 0x0029AA00 File Offset: 0x00298C00
		public void Deserialize(IReader reader)
		{
			this.amountToPoop = reader.ReadSingle();
			string name = reader.ReadKleiString();
			this.poopElement = new Tag(name);
		}

		// Token: 0x06006AF7 RID: 27383 RVA: 0x0029AA2C File Offset: 0x00298C2C
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, GameUtil.GetFormattedMass(complete ? this.amountToPoop : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
		}

		// Token: 0x04004DBE RID: 19902
		private Tag poopElement;

		// Token: 0x04004DBF RID: 19903
		private float amountToPoop;
	}
}
