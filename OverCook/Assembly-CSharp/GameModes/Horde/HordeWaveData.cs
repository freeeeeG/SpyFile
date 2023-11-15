using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B8 RID: 1976
	[Serializable]
	public struct HordeWaveData
	{
		// Token: 0x04001DC1 RID: 7617
		[SerializeField]
		public RecipeList m_recipes;

		// Token: 0x04001DC2 RID: 7618
		[SerializeField]
		public int m_intervalSeconds;

		// Token: 0x04001DC3 RID: 7619
		[SerializeField]
		public List<HordeSpawnData> m_spawns;
	}
}
