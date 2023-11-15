using System;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F6 RID: 502
	public class PauseState : GameState
	{
		// Token: 0x06000B4D RID: 2893 RVA: 0x0002A688 File Offset: 0x00028888
		private void OnResume()
		{
			this.owner.ChangeState<CombatState>();
			base.pauseController.UnPause();
			base.powerupListUI.Hide();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0002A6B6 File Offset: 0x000288B6
		private void OnOptions()
		{
			this.owner.ChangeState<OptionsState>();
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002A6C3 File Offset: 0x000288C3
		private void OnSynergies()
		{
			this.owner.ChangeState<SynergyUIState>();
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002A6D0 File Offset: 0x000288D0
		private void OnGiveUp()
		{
			this.owner.ChangeState<CombatState>();
			base.playerHealth.AutoKill();
			base.pauseController.UnPause();
			base.powerupListUI.Hide();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002A70C File Offset: 0x0002890C
		public override void Enter()
		{
			AudioManager.Instance.SetLowPassFilter(true);
			base.pauseMenu.Show();
			base.powerupListUI.Show();
			base.pauseResumeButton.onClick.AddListener(new UnityAction(this.OnResume));
			base.optionsButton.onClick.AddListener(new UnityAction(this.OnOptions));
			base.synergiesButton.onClick.AddListener(new UnityAction(this.OnSynergies));
			base.giveupButton.onClick.AddListener(new UnityAction(this.OnGiveUp));
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002A7AC File Offset: 0x000289AC
		public override void Exit()
		{
			base.pauseMenu.Hide();
			base.pauseResumeButton.onClick.RemoveListener(new UnityAction(this.OnResume));
			base.optionsButton.onClick.RemoveListener(new UnityAction(this.OnOptions));
			base.synergiesButton.onClick.RemoveListener(new UnityAction(this.OnSynergies));
			base.giveupButton.onClick.RemoveListener(new UnityAction(this.OnGiveUp));
		}
	}
}
