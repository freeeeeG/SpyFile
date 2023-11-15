using System;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200020B RID: 523
	public class LeaderboardUI : MonoBehaviour
	{
		// Token: 0x06000BCE RID: 3022 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		public async void SubmitAndShowsync(int score)
		{
			this.retrievingDataTMP.enabled = true;
			Leaderboard leaderboard2 = await SteamIntegration.Instance.GetLeaderboardAsync("Endless Mode");
			Leaderboard leaderboard = leaderboard2;
			try
			{
				await leaderboard.SubmitScoreAsync(score, null);
				LeaderboardEntry[] array = await leaderboard.GetScoresFromFriendsAsync();
				if (array != null)
				{
					this.SetLeaderboards(array);
				}
				this.retrievingDataTMP.enabled = false;
			}
			catch (Exception message)
			{
				Debug.Log(message);
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002C03C File Offset: 0x0002A23C
		private void SetLeaderboards(LeaderboardEntry[] lbEntries)
		{
			for (int i = 0; i < Mathf.Min(this.maxEntries, lbEntries.Length); i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.entryPrefab);
				gameObject.transform.SetParent(this.entriesLayout.transform);
				gameObject.transform.localScale = Vector3.one;
				LeaderboardUIEntry component = gameObject.GetComponent<LeaderboardUIEntry>();
				if (component != null)
				{
					component.rankTMP.text = lbEntries[i].GlobalRank.ToString();
					component.nameTMP.text = lbEntries[i].User.Name.Truncate(12);
					component.scoreTMP.text = lbEntries[i].Score.ToString();
				}
			}
		}

		// Token: 0x0400083E RID: 2110
		[SerializeField]
		private GameObject entryPrefab;

		// Token: 0x0400083F RID: 2111
		[SerializeField]
		private LayoutGroup entriesLayout;

		// Token: 0x04000840 RID: 2112
		[SerializeField]
		private TMP_Text retrievingDataTMP;

		// Token: 0x04000841 RID: 2113
		[SerializeField]
		private int maxEntries;
	}
}
