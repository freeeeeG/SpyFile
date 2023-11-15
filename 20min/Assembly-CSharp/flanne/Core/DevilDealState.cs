using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F0 RID: 496
	public class DevilDealState : GameState
	{
		// Token: 0x06000B2F RID: 2863 RVA: 0x0002A1F0 File Offset: 0x000283F0
		private void OnConfirm(object sender, Powerup e)
		{
			PlayerController.Instance.playerPerks.Equip(e);
			base.powerupGenerator.RemoveFromDevilPool(e);
			if (base.playerHealth.hp != 0)
			{
				this.owner.ChangeState<CombatState>();
				return;
			}
			this.owner.ChangeState<PlayerDeadState>();
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002A23F File Offset: 0x0002843F
		private void OnLeave()
		{
			this.owner.ChangeState<CombatState>();
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002A24C File Offset: 0x0002844C
		public override void Enter()
		{
			base.pauseController.Pause();
			this.GeneratePowerups();
			base.devilDealMenuPanel.Show();
			base.devilDealMenu.ConfirmEvent += this.OnConfirm;
			base.devilDealLeaveButton.onClick.AddListener(new UnityAction(this.OnLeave));
			AudioManager.Instance.SetLowPassFilter(true);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002A2B4 File Offset: 0x000284B4
		public override void Exit()
		{
			base.pauseController.UnPause();
			base.devilDealMenuPanel.Hide();
			base.devilDealMenu.ConfirmEvent -= this.OnConfirm;
			base.devilDealLeaveButton.onClick.RemoveListener(new UnityAction(this.OnLeave));
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0002A318 File Offset: 0x00028518
		private void GeneratePowerups()
		{
			this.powerupChoices = base.powerupGenerator.GetRandomDevilProfile(3);
			for (int i = 0; i < this.powerupChoices.Count; i++)
			{
				base.devilDealMenu.SetData(i, this.powerupChoices[i]);
			}
		}

		// Token: 0x040007E3 RID: 2019
		private List<Powerup> powerupChoices;
	}
}
