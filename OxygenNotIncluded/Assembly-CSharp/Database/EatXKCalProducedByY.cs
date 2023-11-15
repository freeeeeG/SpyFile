using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;

namespace Database
{
	// Token: 0x02000D47 RID: 3399
	public class EatXKCalProducedByY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006ABB RID: 27323 RVA: 0x0029993B File Offset: 0x00297B3B
		public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
		{
			this.numCalories = numCalories;
			this.foodProducers = foodProducers;
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x00299954 File Offset: 0x00297B54
		public override bool Success()
		{
			List<string> list = new List<string>();
			foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
			{
				foreach (Tag b in this.foodProducers)
				{
					using (List<Tag>.Enumerator enumerator3 = complexRecipe.fabricators.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current == b)
							{
								list.Add(complexRecipe.FirstResult.ToString());
							}
						}
					}
				}
			}
			return RationTracker.Get().GetCaloiresConsumedByFood(list.Distinct<string>().ToList<string>()) / 1000f > (float)this.numCalories;
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x00299A70 File Offset: 0x00297C70
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.foodProducers = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.foodProducers.Add(new Tag(name));
			}
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x00299AC0 File Offset: 0x00297CC0
		public override string GetProgress(bool complete)
		{
			string text = "";
			for (int i = 0; i < this.foodProducers.Count; i++)
			{
				if (i != 0)
				{
					text += COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[i].Name);
				if (buildingDef != null)
				{
					text += buildingDef.Name;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, text);
		}

		// Token: 0x04004DB0 RID: 19888
		private int numCalories;

		// Token: 0x04004DB1 RID: 19889
		private List<Tag> foodProducers;
	}
}
