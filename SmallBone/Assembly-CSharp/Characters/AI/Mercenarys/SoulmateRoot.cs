using System;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Mercenarys
{
	// Token: 0x020011DF RID: 4575
	public class SoulmateRoot : MonoBehaviour
	{
		// Token: 0x060059D1 RID: 22993 RVA: 0x0010B088 File Offset: 0x00109288
		private void Start()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.OnMapChanged;
			Singleton<Service>.Instance.levelManager.onMapLoaded += this._soulmate.Hide;
			UnityEngine.Object.DontDestroyOnLoad(this._soulmate.gameObject);
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x0010B0E0 File Offset: 0x001092E0
		private void OnMapChanged(Map old, Map @new)
		{
			if (WitchBonus.instance.soul.fatalMind.level == 0)
			{
				return;
			}
			if (Singleton<Service>.Instance.levelManager.currentChapter.type == Chapter.Type.Castle)
			{
				this._soulmate.Hide();
				return;
			}
			base.StartCoroutine(this._soulmate.CAppearance());
		}

		// Token: 0x0400488B RID: 18571
		[SerializeField]
		private Soulmate _soulmate;
	}
}
