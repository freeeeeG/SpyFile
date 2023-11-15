using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F9 RID: 505
	public class PowerupMenuState : GameState
	{
		// Token: 0x06000B61 RID: 2913 RVA: 0x0002AE58 File Offset: 0x00029058
		private void OnConfirm(object sender, Powerup e)
		{
			base.StartCoroutine(this.EndLevelUpAnimationCR());
			PlayerController.Instance.playerPerks.Equip(e);
			base.powerupGenerator.RemoveFromPool(e);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0002AE90 File Offset: 0x00029090
		private void OnReroll()
		{
			this.GeneratePowerups();
			base.powerupRerollButton.gameObject.SetActive(false);
			base.powerupMenuPanel.SelectDefault();
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002AEB4 File Offset: 0x000290B4
		public override void Enter()
		{
			base.pauseController.Pause();
			base.StartCoroutine(this.PlayLevelUpAnimationCR());
			AudioManager.Instance.SetLowPassFilter(true);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002AED9 File Offset: 0x000290D9
		public override void Exit()
		{
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002AEE8 File Offset: 0x000290E8
		private void GeneratePowerups()
		{
			int num = 5;
			this.powerupChoices = base.powerupGenerator.GetRandom(base.numPowerupChoices);
			for (int i = 0; i < base.numPowerupChoices; i++)
			{
				base.powerupMenu.SetData(i, this.powerupChoices[i]);
				base.powerupMenu.SetActive(i, true);
			}
			for (int j = base.numPowerupChoices; j < num; j++)
			{
				base.powerupMenu.SetActive(j, false);
			}
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002AF62 File Offset: 0x00029162
		private IEnumerator PlayLevelUpAnimationCR()
		{
			base.screenFlash.Flash(1);
			base.levelupAnimator.gameObject.SetActive(true);
			base.levelupAnimator.SetTrigger("Start");
			LeanTween.scale(base.playerFogRevealer, new Vector3(2f, 2f, 1f), 0.7f).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
			yield return new WaitForSecondsRealtime(0.6f);
			base.screenFlash.Flash(1);
			base.powerupMenuSFX.Play(null);
			yield return new WaitForSecondsRealtime(0.1f);
			this.GeneratePowerups();
			base.powerupMenuPanel.Show();
			base.powerupMenu.ConfirmEvent += this.OnConfirm;
			base.powerupRerollButton.onClick.AddListener(new UnityAction(this.OnReroll));
			if (PowerupGenerator.CanReroll)
			{
				base.powerupRerollButton.gameObject.SetActive(true);
			}
			yield break;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002AF71 File Offset: 0x00029171
		private IEnumerator EndLevelUpAnimationCR()
		{
			base.levelupAnimator.SetTrigger("End");
			LeanTween.scale(base.playerFogRevealer, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutCubic).setIgnoreTimeScale(true);
			base.powerupMenu.ConfirmEvent -= this.OnConfirm;
			base.powerupRerollButton.onClick.RemoveListener(new UnityAction(this.OnReroll));
			base.powerupRerollButton.gameObject.SetActive(false);
			base.powerupMenuPanel.Hide();
			yield return new WaitForSecondsRealtime(1f);
			base.pauseController.UnPause();
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

		// Token: 0x040007E6 RID: 2022
		private List<Powerup> powerupChoices;
	}
}
