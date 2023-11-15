using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C9 RID: 201
	[CreateAssetMenu(fileName = "MapData", menuName = "MapData")]
	public class MapData : ScriptableObject
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0001D181 File Offset: 0x0001B381
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStringID.key);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x0001D193 File Offset: 0x0001B393
		public string description
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.descriptionStringID.key);
			}
		}

		// Token: 0x04000424 RID: 1060
		public LocalizedString nameStringID;

		// Token: 0x04000425 RID: 1061
		public LocalizedString descriptionStringID;

		// Token: 0x04000426 RID: 1062
		public GameObject mapPrefab;

		// Token: 0x04000427 RID: 1063
		public bool achievementsDisabled;

		// Token: 0x04000428 RID: 1064
		public bool darknessDisabled;

		// Token: 0x04000429 RID: 1065
		public bool runesDisabled;

		// Token: 0x0400042A RID: 1066
		public int numPowerupsRepeat = 1;

		// Token: 0x0400042B RID: 1067
		public float timeLimit;

		// Token: 0x0400042C RID: 1068
		public List<SpawnSession> spawnSessions;

		// Token: 0x0400042D RID: 1069
		public List<BossSpawn> bossSpawns;

		// Token: 0x0400042E RID: 1070
		public bool endless;

		// Token: 0x0400042F RID: 1071
		public float endlessLoopTime;

		// Token: 0x04000430 RID: 1072
		public List<SpawnSession> endlessSpawnSessions;

		// Token: 0x04000431 RID: 1073
		public List<BossSpawn> endlessBossSpawn;
	}
}
