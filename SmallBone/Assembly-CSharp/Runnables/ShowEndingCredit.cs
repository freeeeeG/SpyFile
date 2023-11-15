using System;
using System.Collections;
using EndingCredit;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200033C RID: 828
	public class ShowEndingCredit : CRunnable
	{
		// Token: 0x06000FB6 RID: 4022 RVA: 0x0002F736 File Offset: 0x0002D936
		public override IEnumerator CRun()
		{
			Singleton<Service>.Instance.levelManager.DestroyPlayer();
			this._creditRoll = Scene<GameBase>.instance.uiManager.endingCredit;
			this._creditRoll.Show();
			yield return Chronometer.global.WaitForSeconds(this._delay);
			base.StartCoroutine(this._creditRoll.CRun(true));
			yield break;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x0002F745 File Offset: 0x0002D945
		private void OnDisable()
		{
			if (this._creditRoll != null)
			{
				this._creditRoll.Hide();
			}
		}

		// Token: 0x04000CEA RID: 3306
		[SerializeField]
		private float _delay;

		// Token: 0x04000CEB RID: 3307
		private CreditRoll _creditRoll;
	}
}
