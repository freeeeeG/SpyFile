using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Gear.Synergy
{
	// Token: 0x02000860 RID: 2144
	[Serializable]
	public class InscriptionSettings
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x00088906 File Offset: 0x00086B06
		public AssetReference reference
		{
			get
			{
				return this._reference;
			}
		}

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x0008890E File Offset: 0x00086B0E
		public AssetReference omenItem
		{
			get
			{
				return this._omenItem;
			}
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x00088916 File Offset: 0x00086B16
		public int[] steps
		{
			get
			{
				return this._steps;
			}
		}

		// Token: 0x040025A2 RID: 9634
		[SerializeField]
		private AssetReference _reference;

		// Token: 0x040025A3 RID: 9635
		[SerializeField]
		private AssetReference _omenItem;

		// Token: 0x040025A4 RID: 9636
		[SerializeField]
		private int[] _steps;
	}
}
