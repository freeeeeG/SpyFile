using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D54 RID: 3412
	public class RevealAsteriod : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AF0 RID: 27376 RVA: 0x0029A895 File Offset: 0x00298A95
		public RevealAsteriod(float percentToReveal)
		{
			this.percentToReveal = percentToReveal;
		}

		// Token: 0x06006AF1 RID: 27377 RVA: 0x0029A8A4 File Offset: 0x00298AA4
		public override bool Success()
		{
			this.amountRevealed = 0f;
			float num = 0f;
			WorldContainer startWorld = ClusterManager.Instance.GetStartWorld();
			Vector2 minimumBounds = startWorld.minimumBounds;
			Vector2 maximumBounds = startWorld.maximumBounds;
			int num2 = (int)minimumBounds.x;
			while ((float)num2 <= maximumBounds.x)
			{
				int num3 = (int)minimumBounds.y;
				while ((float)num3 <= maximumBounds.y)
				{
					if (Grid.Visible[Grid.PosToCell(new Vector2((float)num2, (float)num3))] > 0)
					{
						num += 1f;
					}
					num3++;
				}
				num2++;
			}
			this.amountRevealed = num / (float)(startWorld.Width * startWorld.Height);
			return this.amountRevealed > this.percentToReveal;
		}

		// Token: 0x06006AF2 RID: 27378 RVA: 0x0029A958 File Offset: 0x00298B58
		public void Deserialize(IReader reader)
		{
			this.percentToReveal = reader.ReadSingle();
		}

		// Token: 0x06006AF3 RID: 27379 RVA: 0x0029A966 File Offset: 0x00298B66
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REVEALED, this.amountRevealed * 100f, this.percentToReveal * 100f);
		}

		// Token: 0x04004DBC RID: 19900
		private float percentToReveal;

		// Token: 0x04004DBD RID: 19901
		private float amountRevealed;
	}
}
