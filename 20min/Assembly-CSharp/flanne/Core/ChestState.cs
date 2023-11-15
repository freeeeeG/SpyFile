using System;
using System.Collections;
using System.Collections.Generic;
using flanne.UI;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x020001EE RID: 494
	public class ChestState : GameState
	{
		// Token: 0x06000B1F RID: 2847 RVA: 0x00029E85 File Offset: 0x00028085
		private void OnTakeClick(object sender, EventArgs e)
		{
			PlayerController.Instance.playerPerks.Equip(this._currPowerup);
			base.powerupGenerator.RemoveFromCharacterPool(this._currPowerup);
			base.StartCoroutine(this.WaitToLeaveMenu());
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00029EBA File Offset: 0x000280BA
		private void OnLeaveClick(object sender, EventArgs e)
		{
			base.StartCoroutine(this.WaitToLeaveMenu());
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00029ECC File Offset: 0x000280CC
		public override void Enter()
		{
			List<Powerup> randomCharacterProfile = base.powerupGenerator.GetRandomCharacterProfile(1);
			this._currPowerup = randomCharacterProfile[0];
			base.chestUIController.SetToPowerup(this._currPowerup);
			base.chestUIController.Show();
			ChestUIController chestUIController = base.chestUIController;
			chestUIController.TakeClickEvent = (EventHandler)Delegate.Combine(chestUIController.TakeClickEvent, new EventHandler(this.OnTakeClick));
			ChestUIController chestUIController2 = base.chestUIController;
			chestUIController2.LeaveClickEvent = (EventHandler)Delegate.Combine(chestUIController2.LeaveClickEvent, new EventHandler(this.OnLeaveClick));
			base.pauseController.Pause();
			AudioManager.Instance.SetLowPassFilter(true);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00029F73 File Offset: 0x00028173
		public override void Exit()
		{
			base.pauseController.UnPause();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00029F8B File Offset: 0x0002818B
		private IEnumerator WaitToLeaveMenu()
		{
			ChestUIController chestUIController = base.chestUIController;
			chestUIController.TakeClickEvent = (EventHandler)Delegate.Remove(chestUIController.TakeClickEvent, new EventHandler(this.OnTakeClick));
			ChestUIController chestUIController2 = base.chestUIController;
			chestUIController2.LeaveClickEvent = (EventHandler)Delegate.Remove(chestUIController2.LeaveClickEvent, new EventHandler(this.OnLeaveClick));
			base.chestUIController.Hide();
			yield return new WaitForSecondsRealtime(0.5f);
			if (base.playerHealth.hp != 0)
			{
				this.owner.ChangeState<CombatState>();
			}
			else
			{
				this.owner.ChangeState<PlayerDeadState>();
			}
			yield break;
		}

		// Token: 0x040007E2 RID: 2018
		private Powerup _currPowerup;
	}
}
