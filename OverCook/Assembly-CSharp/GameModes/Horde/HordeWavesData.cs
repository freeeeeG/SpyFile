using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B7 RID: 1975
	[Serializable]
	public struct HordeWavesData
	{
		// Token: 0x170002F5 RID: 757
		public HordeWaveData this[int idx]
		{
			get
			{
				return this.m_waves[idx];
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x000B3333 File Offset: 0x000B1733
		public int Count
		{
			get
			{
				return (this.m_waves == null) ? 0 : this.m_waves.Count;
			}
		}

		// Token: 0x04001DC0 RID: 7616
		[SerializeField]
		private List<HordeWaveData> m_waves;
	}
}
