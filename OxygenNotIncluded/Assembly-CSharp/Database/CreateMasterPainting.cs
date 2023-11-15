using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D4F RID: 3407
	public class CreateMasterPainting : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006ADC RID: 27356 RVA: 0x0029A4C8 File Offset: 0x002986C8
		public override bool Success()
		{
			foreach (Painting painting in Components.Paintings.Items)
			{
				if (painting != null)
				{
					ArtableStage artableStage = Db.GetArtableStages().TryGet(painting.CurrentStage);
					if (artableStage != null && artableStage.statusItem == Db.Get().ArtableStatuses.LookingGreat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006ADD RID: 27357 RVA: 0x0029A554 File Offset: 0x00298754
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x0029A556 File Offset: 0x00298756
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
		}
	}
}
