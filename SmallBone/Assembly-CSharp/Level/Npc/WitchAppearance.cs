using System;
using Data;
using UnityEngine;

namespace Level.Npc
{
	// Token: 0x020005C9 RID: 1481
	public class WitchAppearance : MonoBehaviour
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x0005A186 File Offset: 0x00058386
		public bool humanForm
		{
			get
			{
				return this._humanForm.activeSelf;
			}
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0005A194 File Offset: 0x00058394
		private void Start()
		{
			if (GameData.Generic.tutorial.isPlayed())
			{
				return;
			}
			if (MMMaths.Chance(0.7))
			{
				this._catForm.SetActive(false);
				this._humanForm.SetActive(true);
				return;
			}
			this._humanForm.SetActive(false);
			this._catForm.SetActive(true);
		}

		// Token: 0x040018FF RID: 6399
		[SerializeField]
		private GameObject _humanForm;

		// Token: 0x04001900 RID: 6400
		[SerializeField]
		private GameObject _catForm;
	}
}
