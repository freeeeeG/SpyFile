using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001FB RID: 507
	public class ShanaHaloState : GameState
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x0002AFE5 File Offset: 0x000291E5
		private void OnTakeClick()
		{
			PlayerController.Instance.playerPerks.Equip(this._currPowerup);
			base.StartCoroutine(this.WaitToLeaveMenu());
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002B00C File Offset: 0x0002920C
		public override void Enter()
		{
			this._currPowerup = base.haloUI.data;
			base.haloUIPanel.Show();
			base.takeHaloButton.onClick.AddListener(new UnityAction(this.OnTakeClick));
			base.pauseController.Pause();
			AudioManager.Instance.SetLowPassFilter(true);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00029F73 File Offset: 0x00028173
		public override void Exit()
		{
			base.pauseController.UnPause();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002B067 File Offset: 0x00029267
		private IEnumerator WaitToLeaveMenu()
		{
			base.takeHaloButton.onClick.RemoveListener(new UnityAction(this.OnTakeClick));
			base.haloUIPanel.Hide();
			yield return new WaitForSecondsRealtime(0.5f);
			this.owner.ChangeState<CombatState>();
			yield break;
		}

		// Token: 0x040007E7 RID: 2023
		private Powerup _currPowerup;
	}
}
