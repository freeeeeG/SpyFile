using System;
using System.Collections;
using EndingCredit;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200033E RID: 830
	public sealed class ShowHardmodeEndingCredit : CRunnable
	{
		// Token: 0x06000FBF RID: 4031 RVA: 0x0002F80C File Offset: 0x0002DA0C
		public override IEnumerator CRun()
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			levelManager.player.playerComponents.inventory.item.RemoveAll();
			levelManager.player.playerComponents.inventory.quintessence.RemoveAll();
			this._creditRoll = Scene<GameBase>.instance.uiManager.endingCredit;
			this._creditRoll.Show();
			yield return Chronometer.global.WaitForSeconds(this._delay);
			base.StartCoroutine(this._creditRoll.CRun(false));
			while (this._creditRoll.active)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0002F81B File Offset: 0x0002DA1B
		private void OnDisable()
		{
			if (this._creditRoll != null)
			{
				this._creditRoll.Hide();
			}
		}

		// Token: 0x04000CEF RID: 3311
		[SerializeField]
		private float _delay;

		// Token: 0x04000CF0 RID: 3312
		private CreditRoll _creditRoll;
	}
}
