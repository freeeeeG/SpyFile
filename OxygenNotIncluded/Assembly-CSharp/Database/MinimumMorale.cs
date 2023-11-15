using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D35 RID: 3381
	public class MinimumMorale : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A64 RID: 27236 RVA: 0x0029860B File Offset: 0x0029680B
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, this.minimumMorale);
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x00298627 File Offset: 0x00296827
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, this.minimumMorale);
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x00298643 File Offset: 0x00296843
		public MinimumMorale(int minimumMorale = 16)
		{
			this.minimumMorale = minimumMorale;
		}

		// Token: 0x06006A67 RID: 27239 RVA: 0x00298654 File Offset: 0x00296854
		public override bool Success()
		{
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
				if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
				{
					AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
					flag = (attributeInstance != null && attributeInstance.GetTotalValue() >= (float)this.minimumMorale && flag);
				}
			}
			return flag;
		}

		// Token: 0x06006A68 RID: 27240 RVA: 0x002986FC File Offset: 0x002968FC
		public void Deserialize(IReader reader)
		{
			this.minimumMorale = reader.ReadInt32();
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x0029870A File Offset: 0x0029690A
		public override string GetProgress(bool complete)
		{
			return this.Description();
		}

		// Token: 0x04004D98 RID: 19864
		public int minimumMorale;
	}
}
