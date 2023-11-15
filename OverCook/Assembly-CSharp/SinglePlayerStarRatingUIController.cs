using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000B4D RID: 2893
public class SinglePlayerStarRatingUIController : StarRatingUIController
{
	// Token: 0x06003ACB RID: 15051 RVA: 0x001180B4 File Offset: 0x001164B4
	public override void SetScoreData(object _scoreData)
	{
		SinglePlayerStarRatingUIController.ScoreData scoreData = _scoreData as SinglePlayerStarRatingUIController.ScoreData;
		base.SetScoreData(scoreData.StarRating);
		this.m_levelTimer.Value = scoreData.LevelTime;
		GameSession gameSession = GameUtils.GetGameSession();
		GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(SceneManager.GetActiveScene().name);
		if (progress.Completed)
		{
			this.m_bestTime.Value = Mathf.Min(-progress.HighScore, scoreData.LevelTime);
		}
		else
		{
			this.m_bestTime.gameObject.SetActive(false);
			this.m_bestTimeLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x04002FB0 RID: 12208
	[SerializeField]
	private DisplayTimeUIController m_levelTimer;

	// Token: 0x04002FB1 RID: 12209
	[SerializeField]
	private DisplayTimeUIController m_bestTime;

	// Token: 0x04002FB2 RID: 12210
	[SerializeField]
	private Text m_bestTimeLabel;

	// Token: 0x02000B4E RID: 2894
	public class ScoreData
	{
		// Token: 0x04002FB3 RID: 12211
		public int StarRating;

		// Token: 0x04002FB4 RID: 12212
		public int LevelTime;
	}
}
