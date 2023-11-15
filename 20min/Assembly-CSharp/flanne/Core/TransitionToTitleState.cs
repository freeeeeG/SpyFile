using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace flanne.Core
{
	// Token: 0x020001FE RID: 510
	public class TransitionToTitleState : GameState
	{
		// Token: 0x06000B7A RID: 2938 RVA: 0x0002B123 File Offset: 0x00029323
		public override void Enter()
		{
			base.StartCoroutine(this.WaitToExitToTitleScreen());
			this.Save();
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0002B138 File Offset: 0x00029338
		private void Save()
		{
			PointsTracker.pts += ScoreCalculator.SharedInstance.GetScore().totalScore;
			if (SaveSystem.data != null)
			{
				SaveSystem.data.points = PointsTracker.pts;
				SaveSystem.data.playedGame = true;
				SaveSystem.Save();
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0002B185 File Offset: 0x00029385
		private IEnumerator WaitToExitToTitleScreen()
		{
			base.endScreenUIC.Hide();
			yield return new WaitForSecondsRealtime(1.5f);
			PauseController.SharedInstance.UnPause();
			SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
			yield break;
		}
	}
}
