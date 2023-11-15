using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200026B RID: 619
public class UIBillBoard : IUserInterface
{
	// Token: 0x06000F61 RID: 3937 RVA: 0x0002900A File Offset: 0x0002720A
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x0002901E File Offset: 0x0002721E
	public void RefreashBtnClick()
	{
		if (!this.refreashingLeaderboard)
		{
			base.StartCoroutine(this.RefreashCor());
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00029035 File Offset: 0x00027235
	private IEnumerator RefreashCor()
	{
		this.refreashBtn.SetActive(false);
		this.refreashingLeaderboard = true;
		Singleton<PlayfabManager>.Instance.UpdateLoacalScore();
		yield return new WaitForSeconds(2f);
		LeaderBoard leaderBoardType = this.LeaderBoardType;
		if (leaderBoardType != LeaderBoard.Endless)
		{
			if (leaderBoardType == LeaderBoard.Challenge)
			{
				Singleton<PlayfabManager>.Instance.GetChallengeVersion();
			}
		}
		else
		{
			Singleton<PlayfabManager>.Instance.GetEndlessVersion();
		}
		yield return new WaitForSeconds(2f);
		this.refreashingLeaderboard = false;
		this.SetLeaderBoard();
		this.refreashBtn.SetActive(true);
		yield break;
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00029044 File Offset: 0x00027244
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("isOpen", true);
		this.refreashBtn.SetActive(true);
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0002906C File Offset: 0x0002726C
	private void SetUIInfo()
	{
		LeaderBoard leaderBoardType = this.LeaderBoardType;
		if (leaderBoardType == LeaderBoard.Endless)
		{
			this.lastTxt.text = GameMultiLang.GetTraduction("YESTERDAY");
			this.newTxt.text = GameMultiLang.GetTraduction("TODAY");
			this.refreshTipsTxt.text = GameMultiLang.GetTraduction("RESETTIME2");
			this.titleTxt.text = GameMultiLang.GetTraduction("ENDLESSBILLBOARD");
			return;
		}
		if (leaderBoardType != LeaderBoard.Challenge)
		{
			return;
		}
		this.lastTxt.text = GameMultiLang.GetTraduction("YESTERDAY");
		this.newTxt.text = GameMultiLang.GetTraduction("TODAY");
		this.refreshTipsTxt.text = GameMultiLang.GetTraduction("RESETTIME2");
		this.titleTxt.text = GameMultiLang.GetTraduction("CHALLENGEBILLBOARD");
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00029131 File Offset: 0x00027331
	public void ShowLeaderBoard(LeaderBoard leaderBoardType)
	{
		this.LeaderBoardType = leaderBoardType;
		this.SetLeaderBoard();
		this.SetUIInfo();
		this.Show();
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0002914C File Offset: 0x0002734C
	public override void ClosePanel()
	{
		this.anim.SetBool("isOpen", false);
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00029160 File Offset: 0x00027360
	public void SetLeaderBoard()
	{
		this.ClearBillBoard();
		LeaderBoard leaderBoardType = this.LeaderBoardType;
		if (leaderBoardType != LeaderBoard.Endless)
		{
			if (leaderBoardType != LeaderBoard.Challenge)
			{
				return;
			}
		}
		else
		{
			if (Singleton<PlayfabManager>.Instance.EndlessResult[0].LeaderBoardResult != null)
			{
				foreach (PlayerLeaderboardEntry playerLeaderboardEntry in Singleton<PlayfabManager>.Instance.EndlessResult[0].LeaderBoardResult.Leaderboard)
				{
					BillboardItem billboardItem = Object.Instantiate<BillboardItem>(this.billboardItemPrefab, this.todayParent);
					billboardItem.SetContent(playerLeaderboardEntry.Position + 1, playerLeaderboardEntry.DisplayName, playerLeaderboardEntry.StatValue, true);
					this.m_Items.Add(billboardItem);
				}
			}
			if (Singleton<PlayfabManager>.Instance.EndlessResult[1].LeaderBoardResult == null)
			{
				return;
			}
			using (List<PlayerLeaderboardEntry>.Enumerator enumerator = Singleton<PlayfabManager>.Instance.EndlessResult[1].LeaderBoardResult.Leaderboard.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PlayerLeaderboardEntry playerLeaderboardEntry2 = enumerator.Current;
					BillboardItem billboardItem2 = Object.Instantiate<BillboardItem>(this.billboardItemPrefab, this.yesterdayParent);
					billboardItem2.SetContent(playerLeaderboardEntry2.Position + 1, playerLeaderboardEntry2.DisplayName, playerLeaderboardEntry2.StatValue, true);
					this.m_Items.Add(billboardItem2);
				}
				return;
			}
		}
		if (Singleton<PlayfabManager>.Instance.ChallengeResults[0].LeaderBoardResult != null)
		{
			foreach (PlayerLeaderboardEntry playerLeaderboardEntry3 in Singleton<PlayfabManager>.Instance.ChallengeResults[0].LeaderBoardResult.Leaderboard)
			{
				BillboardItem billboardItem3 = Object.Instantiate<BillboardItem>(this.billboardItemPrefab, this.todayParent);
				billboardItem3.SetContent(playerLeaderboardEntry3.Position + 1, playerLeaderboardEntry3.DisplayName, playerLeaderboardEntry3.StatValue, false);
				this.m_Items.Add(billboardItem3);
			}
		}
		if (Singleton<PlayfabManager>.Instance.ChallengeResults[1].LeaderBoardResult != null)
		{
			foreach (PlayerLeaderboardEntry playerLeaderboardEntry4 in Singleton<PlayfabManager>.Instance.ChallengeResults[1].LeaderBoardResult.Leaderboard)
			{
				BillboardItem billboardItem4 = Object.Instantiate<BillboardItem>(this.billboardItemPrefab, this.yesterdayParent);
				billboardItem4.SetContent(playerLeaderboardEntry4.Position + 1, playerLeaderboardEntry4.DisplayName, playerLeaderboardEntry4.StatValue, false);
				this.m_Items.Add(billboardItem4);
			}
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00029428 File Offset: 0x00027628
	private void ClearBillBoard()
	{
		foreach (BillboardItem billboardItem in this.m_Items)
		{
			Object.Destroy(billboardItem.gameObject);
		}
		this.m_Items.Clear();
	}

	// Token: 0x040007CD RID: 1997
	private Animator anim;

	// Token: 0x040007CE RID: 1998
	[SerializeField]
	private BillboardItem billboardItemPrefab;

	// Token: 0x040007CF RID: 1999
	[SerializeField]
	private Transform todayParent;

	// Token: 0x040007D0 RID: 2000
	[SerializeField]
	private Transform yesterdayParent;

	// Token: 0x040007D1 RID: 2001
	private List<BillboardItem> m_Items = new List<BillboardItem>();

	// Token: 0x040007D2 RID: 2002
	[SerializeField]
	private GameObject refreashBtn;

	// Token: 0x040007D3 RID: 2003
	[SerializeField]
	private bool refreashingLeaderboard;

	// Token: 0x040007D4 RID: 2004
	[SerializeField]
	private Text lastTxt;

	// Token: 0x040007D5 RID: 2005
	[SerializeField]
	private Text newTxt;

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private TextMeshProUGUI refreshTipsTxt;

	// Token: 0x040007D7 RID: 2007
	[SerializeField]
	private TextMeshProUGUI titleTxt;

	// Token: 0x040007D8 RID: 2008
	private LeaderBoard LeaderBoardType;
}
