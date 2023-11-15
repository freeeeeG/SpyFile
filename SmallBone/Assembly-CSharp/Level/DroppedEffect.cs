using System;
using UnityEngine;

namespace Level
{
	// Token: 0x020004B7 RID: 1207
	public class DroppedEffect : MonoBehaviour
	{
		// Token: 0x06001754 RID: 5972 RVA: 0x000495A8 File Offset: 0x000477A8
		public void SpawnLegendaryEffect()
		{
			for (int i = 0; i < this._legendary.Length; i++)
			{
				this._legendary[i].SetActive(true);
			}
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x000495D8 File Offset: 0x000477D8
		public void SpawnOmenEffect()
		{
			for (int i = 0; i < this._omen.Length; i++)
			{
				this._omen[i].SetActive(true);
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x00049608 File Offset: 0x00047808
		public void Despawn()
		{
			for (int i = 0; i < this._legendary.Length; i++)
			{
				this._legendary[i].SetActive(false);
			}
			for (int j = 0; j < this._omen.Length; j++)
			{
				this._omen[j].SetActive(false);
			}
		}

		// Token: 0x0400146D RID: 5229
		[SerializeField]
		private GameObject[] _legendary;

		// Token: 0x0400146E RID: 5230
		[SerializeField]
		private GameObject[] _omen;
	}
}
