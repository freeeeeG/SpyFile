using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200011F RID: 287
	public class StatsHolder : MonoBehaviour
	{
		// Token: 0x17000083 RID: 131
		public StatMod this[StatType s]
		{
			get
			{
				return this._data[(int)s];
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00021A28 File Offset: 0x0001FC28
		private void Awake()
		{
			for (int i = 0; i < this._data.Length; i++)
			{
				this._data[i] = new StatMod();
			}
		}

		// Token: 0x040005BC RID: 1468
		private StatMod[] _data = new StatMod[21];
	}
}
