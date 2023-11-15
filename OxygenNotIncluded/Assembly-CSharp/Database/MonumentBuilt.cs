using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x02000D30 RID: 3376
	public class MonumentBuilt : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A49 RID: 27209 RVA: 0x002982DA File Offset: 0x002964DA
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
		}

		// Token: 0x06006A4A RID: 27210 RVA: 0x002982E6 File Offset: 0x002964E6
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
		}

		// Token: 0x06006A4B RID: 27211 RVA: 0x002982F4 File Offset: 0x002964F4
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.MonumentParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((MonumentPart)enumerator.Current).IsMonumentCompleted())
					{
						Game.Instance.unlocks.Unlock("thriving", true);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x00298368 File Offset: 0x00296568
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06006A4D RID: 27213 RVA: 0x0029836A File Offset: 0x0029656A
		public override string GetProgress(bool complete)
		{
			return this.Name();
		}
	}
}
