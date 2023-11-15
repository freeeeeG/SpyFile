using System;
using Data;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech
{
	// Token: 0x02000154 RID: 340
	public sealed class DarktechActivator : MonoBehaviour
	{
		// Token: 0x060006BE RID: 1726 RVA: 0x000137A8 File Offset: 0x000119A8
		private void Awake()
		{
			if (!GameData.HardmodeProgress.hardmode)
			{
				this._target.SetActive(false);
				return;
			}
			if (Singleton<DarktechManager>.Instance.IsUnlocked(this._info))
			{
				this._target.SetActive(true);
				return;
			}
			this._target.SetActive(false);
		}

		// Token: 0x040004F0 RID: 1264
		[SerializeField]
		private DarktechData _info;

		// Token: 0x040004F1 RID: 1265
		[SerializeField]
		private GameObject _target;
	}
}
