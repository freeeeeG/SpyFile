using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F7 RID: 503
	public class PlayerDeadState : GameState
	{
		// Token: 0x06000B54 RID: 2900 RVA: 0x0002A834 File Offset: 0x00028A34
		private void OnClickRetry()
		{
			this.owner.ChangeState<TransitionToRetryState>();
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0002A841 File Offset: 0x00028A41
		private void OnClickQuit()
		{
			this.owner.ChangeState<TransitionToTitleState>();
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002A850 File Offset: 0x00028A50
		public override void Enter()
		{
			GameTimer.SharedInstance.Stop();
			AIController.SharedInstance.playerRepel = true;
			base.playerCameraRig.enabled = false;
			base.StartCoroutine(this.LoseVisionCR());
			base.retryRunButton.onClick.AddListener(new UnityAction(this.OnClickRetry));
			base.quitToTitleButton.onClick.AddListener(new UnityAction(this.OnClickQuit));
			AudioManager.Instance.SetLowPassFilter(true);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0002A8D0 File Offset: 0x00028AD0
		public override void Exit()
		{
			base.retryRunButton.onClick.RemoveListener(new UnityAction(this.OnClickRetry));
			base.quitToTitleButton.onClick.RemoveListener(new UnityAction(this.OnClickQuit));
			base.powerupListUI.Hide();
			base.loadoutUI.Hide();
			AudioManager.Instance.SetLowPassFilter(false);
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002A936 File Offset: 0x00028B36
		private IEnumerator LoseVisionCR()
		{
			LeanTween.scale(base.playerFogRevealer, new Vector3(0.7f, 0.7f, 1f), 1f).setEase(LeanTweenType.easeOutBack);
			yield return new WaitForSeconds(1.5f);
			LeanTween.scale(base.playerFogRevealer, new Vector3(0f, 0f, 1f), 1f).setEase(LeanTweenType.easeInCubic);
			AudioManager.Instance.FadeOutMusic(0.5f);
			yield return new WaitForSeconds(0.5f);
			base.hud.Hide();
			base.youDiedSFX.Play(null);
			Score score = ScoreCalculator.SharedInstance.GetScore();
			base.endScreenUIC.SetScores(score);
			base.endScreenUIC.Show(false);
			base.powerupListUI.Show();
			base.loadoutUI.Show();
			PauseController.SharedInstance.Pause();
			if (SelectedMap.MapData != null && SelectedMap.MapData.endless)
			{
				base.leaderBoardPanel.Show();
				base.leaderboardUI.SubmitAndShowsync(score.totalScore);
			}
			yield break;
		}
	}
}
