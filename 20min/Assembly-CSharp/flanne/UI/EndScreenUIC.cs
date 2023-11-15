using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000209 RID: 521
	public class EndScreenUIC : MonoBehaviour
	{
		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002BBD5 File Offset: 0x00029DD5
		public void Show(bool survived)
		{
			if (survived)
			{
				this.youSurvivedPanel.Show();
			}
			else
			{
				this.youDiedPanel.Show();
			}
			base.StartCoroutine(this.ShowPanelsCR());
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002BC00 File Offset: 0x00029E00
		public void Hide()
		{
			this.youSurvivedPanel.Hide();
			this.youDiedPanel.Hide();
			Panel[] array = this.scorePanels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Hide();
			}
			this.quitButtonPanel.Hide();
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002BC4C File Offset: 0x00029E4C
		public void SetScores(Score score)
		{
			this.timeSurvivedTMP.text = LocalizationSystem.GetLocalizedValue(this.timeSurvivedLabel.key) + " <color=#F5D6C1>(" + score.timeSurvivedString + ")</color>";
			this.timeSurvivedScoreTMP.text = score.timeSurvivedScore.ToString();
			this.enemiesKilledTMP.text = string.Concat(new object[]
			{
				LocalizationSystem.GetLocalizedValue(this.enemiesKilledLabel.key),
				" <color=#F5D6C1>(",
				score.enemiesKilled,
				")</color>"
			});
			this.enemiesKilledScoreTMP.text = score.enemiesKilledScore.ToString();
			this.levelsEarnedTMP.text = string.Concat(new object[]
			{
				LocalizationSystem.GetLocalizedValue(this.levelsEarnedLabel.key),
				" <color=#F5D6C1>(",
				score.levelsEarned,
				")</color>"
			});
			this.levelsEarnedScoreTMP.text = score.levelsEarnedScore.ToString();
			this.totalScoreTMP.text = score.totalScore.ToString();
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002BD72 File Offset: 0x00029F72
		private IEnumerator ShowPanelsCR()
		{
			yield return new WaitForSecondsRealtime(1.5f);
			Panel[] array = this.scorePanels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Show();
				yield return new WaitForSecondsRealtime(0.1f);
			}
			array = null;
			yield return new WaitForSecondsRealtime(1f);
			this.screenCoverPanel.Show();
			this.quitButtonPanel.Show();
			yield break;
		}

		// Token: 0x04000827 RID: 2087
		[SerializeField]
		private Panel youDiedPanel;

		// Token: 0x04000828 RID: 2088
		[SerializeField]
		private Panel youSurvivedPanel;

		// Token: 0x04000829 RID: 2089
		[SerializeField]
		private Panel screenCoverPanel;

		// Token: 0x0400082A RID: 2090
		[SerializeField]
		private Panel[] scorePanels;

		// Token: 0x0400082B RID: 2091
		[SerializeField]
		private Panel quitButtonPanel;

		// Token: 0x0400082C RID: 2092
		[SerializeField]
		private TMP_Text timeSurvivedTMP;

		// Token: 0x0400082D RID: 2093
		[SerializeField]
		private TMP_Text timeSurvivedScoreTMP;

		// Token: 0x0400082E RID: 2094
		[SerializeField]
		private TMP_Text enemiesKilledTMP;

		// Token: 0x0400082F RID: 2095
		[SerializeField]
		private TMP_Text enemiesKilledScoreTMP;

		// Token: 0x04000830 RID: 2096
		[SerializeField]
		private TMP_Text levelsEarnedTMP;

		// Token: 0x04000831 RID: 2097
		[SerializeField]
		private TMP_Text levelsEarnedScoreTMP;

		// Token: 0x04000832 RID: 2098
		[SerializeField]
		private TMP_Text totalScoreTMP;

		// Token: 0x04000833 RID: 2099
		[SerializeField]
		private LocalizedString timeSurvivedLabel;

		// Token: 0x04000834 RID: 2100
		[SerializeField]
		private LocalizedString enemiesKilledLabel;

		// Token: 0x04000835 RID: 2101
		[SerializeField]
		private LocalizedString levelsEarnedLabel;
	}
}
