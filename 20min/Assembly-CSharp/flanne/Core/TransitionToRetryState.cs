using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.Core
{
	// Token: 0x020001FD RID: 509
	public class TransitionToRetryState : GameState
	{
		// Token: 0x06000B76 RID: 2934 RVA: 0x0002B0C8 File Offset: 0x000292C8
		public override void Enter()
		{
			base.StartCoroutine(this.WaitToReload());
			this.Save();
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002B0DD File Offset: 0x000292DD
		private void Save()
		{
			PointsTracker.pts += ScoreCalculator.SharedInstance.GetScore().totalScore;
			if (SaveSystem.data != null)
			{
				SaveSystem.data.points = PointsTracker.pts;
				SaveSystem.Save();
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002B114 File Offset: 0x00029314
		private IEnumerator WaitToReload()
		{
			base.endScreenUIC.Hide();
			yield return new WaitForSecondsRealtime(1.5f);
			PauseController.SharedInstance.UnPause();
			SceneManager.LoadScene("Battle", LoadSceneMode.Single);
			yield break;
		}
	}
}
