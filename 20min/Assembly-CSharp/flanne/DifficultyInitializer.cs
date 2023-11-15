using System;
using flanne.Core;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000081 RID: 129
	public class DifficultyInitializer : MonoBehaviour
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x0001947C File Offset: 0x0001767C
		private void Start()
		{
			MapData mapData = SelectedMap.MapData;
			if (mapData != null && !mapData.darknessDisabled)
			{
				int difficultyLevel = Loadout.difficultyLevel;
				this.ApplyDifficulty(difficultyLevel);
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x000194B0 File Offset: 0x000176B0
		public void ApplyDifficulty(int diffLevel)
		{
			for (int i = 0; i < diffLevel + 1; i++)
			{
				this.modList.mods[i].ModifyHordeSpawner(this.hordeSpawner);
				this.modList.mods[i].ModifyBossSpawner(this.bossSpawner);
				this.modList.mods[i].ModifyGame(this.gameController);
			}
		}

		// Token: 0x0400030D RID: 781
		[SerializeField]
		private HordeSpawner hordeSpawner;

		// Token: 0x0400030E RID: 782
		[SerializeField]
		private BossSpawner bossSpawner;

		// Token: 0x0400030F RID: 783
		[SerializeField]
		private GameController gameController;

		// Token: 0x04000310 RID: 784
		[SerializeField]
		private DifficultyModList modList;
	}
}
