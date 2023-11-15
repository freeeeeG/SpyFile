using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000D15 RID: 3349
	public class Quests : ResourceSet<Quest>
	{
		// Token: 0x060069EA RID: 27114 RVA: 0x00291C3C File Offset: 0x0028FE3C
		public Quests(ResourceSet parent) : base("Quests", parent)
		{
			this.LonelyMinionGreetingQuest = base.Add(new Quest("KnockQuest", new QuestCriteria[]
			{
				new QuestCriteria("Neighbor", null, 1, null, QuestCriteria.BehaviorFlags.None)
			}));
			this.LonelyMinionFoodQuest = base.Add(new Quest("FoodQuest", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("FoodQuality", new float[]
				{
					4f
				}, 3, new HashSet<Tag>
				{
					GameTags.Edible
				}, QuestCriteria.BehaviorFlags.UniqueItems)
			}));
			this.LonelyMinionPowerQuest = base.Add(new Quest("PluggedIn", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[]
				{
					3000f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
			this.LonelyMinionDecorQuest = base.Add(new Quest("HighDecor", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("Decor", new float[]
				{
					120f
				}, 1, null, (QuestCriteria.BehaviorFlags)6)
			}));
			this.FossilHuntQuest = base.Add(new Quest("FossilHuntQuest", new QuestCriteria[]
			{
				new QuestCriteria_Equals("LostSpecimen", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostIceFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostResinFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostRockFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
		}

		// Token: 0x04004CAB RID: 19627
		public Quest LonelyMinionGreetingQuest;

		// Token: 0x04004CAC RID: 19628
		public Quest LonelyMinionFoodQuest;

		// Token: 0x04004CAD RID: 19629
		public Quest LonelyMinionPowerQuest;

		// Token: 0x04004CAE RID: 19630
		public Quest LonelyMinionDecorQuest;

		// Token: 0x04004CAF RID: 19631
		public Quest FossilHuntQuest;
	}
}
