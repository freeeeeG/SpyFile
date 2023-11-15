using System;
using flanne.Core;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000083 RID: 131
	[CreateAssetMenu(fileName = "BlankDifficultyModifier", menuName = "DifficultyMods/BlankDifficultyModifier")]
	public class DifficultyModifier : ScriptableObject
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00019513 File Offset: 0x00017713
		public string description
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.descriptionStringID.key);
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void ModifyHordeSpawner(HordeSpawner hordeSpawner)
		{
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void ModifyBossSpawner(BossSpawner bossSpawner)
		{
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00002F51 File Offset: 0x00001151
		public virtual void ModifyGame(GameController gameController)
		{
		}

		// Token: 0x04000312 RID: 786
		public LocalizedString descriptionStringID;
	}
}
