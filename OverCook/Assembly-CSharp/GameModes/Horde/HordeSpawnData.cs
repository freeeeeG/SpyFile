using System;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B9 RID: 1977
	[Serializable]
	public struct HordeSpawnData
	{
		// Token: 0x060025E0 RID: 9696 RVA: 0x000B3351 File Offset: 0x000B1751
		public bool CanSpawn(double time)
		{
			return (double)this.m_spawnTimeSeconds <= time;
		}

		// Token: 0x04001DC4 RID: 7620
		[SerializeField]
		public int m_spawnTimeSeconds;

		// Token: 0x04001DC5 RID: 7621
		[SerializeField]
		public GameObject m_prefab;
	}
}
