using System;
using flanne.Player;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200010B RID: 267
	public class ScoreCalculator : MonoBehaviour
	{
		// Token: 0x06000789 RID: 1929 RVA: 0x00020C48 File Offset: 0x0001EE48
		private void OnDeath(object sender, object args)
		{
			if ((sender as Health).gameObject.tag == "Enemy")
			{
				this._enemiesKilled++;
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00020C74 File Offset: 0x0001EE74
		private void Awake()
		{
			ScoreCalculator.SharedInstance = this;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00020C7C File Offset: 0x0001EE7C
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00020C95 File Offset: 0x0001EE95
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00020CB0 File Offset: 0x0001EEB0
		public Score GetScore()
		{
			return new Score
			{
				timeSurvivedString = this.gameTimer.TimeToString(),
				timeSurvivedScore = Mathf.CeilToInt(this.gameTimer.timer),
				enemiesKilled = this._enemiesKilled,
				enemiesKilledScore = this._enemiesKilled,
				levelsEarned = this.playerXP.level - 1,
				levelsEarnedScore = (this.playerXP.level - 1) * 100
			};
		}

		// Token: 0x0400056B RID: 1387
		public static ScoreCalculator SharedInstance;

		// Token: 0x0400056C RID: 1388
		[SerializeField]
		private GameTimer gameTimer;

		// Token: 0x0400056D RID: 1389
		[SerializeField]
		private PlayerXP playerXP;

		// Token: 0x0400056E RID: 1390
		private int _enemiesKilled;
	}
}
