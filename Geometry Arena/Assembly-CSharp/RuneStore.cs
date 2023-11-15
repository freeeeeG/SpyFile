using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000032 RID: 50
[Serializable]
public class RuneStore
{
	// Token: 0x06000240 RID: 576 RVA: 0x0000D868 File Offset: 0x0000BA68
	public void TryInit()
	{
		if (this.refreshTimes < 0)
		{
			Debug.LogError("Error_RefreshTimes<0!");
		}
		if (this.refreshTimes <= 0)
		{
			this.OnlyRefresh(false);
			return;
		}
		if (this.runeGoods == null || this.runeGoods.Length == 0)
		{
			Debug.LogError("Error_初始化过了咋还是0呢");
			this.OnlyRefresh(false);
			return;
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0000D8BC File Offset: 0x0000BABC
	public void RefreshTime_TryCountDownInUpdate()
	{
		if (this.freeRefreshTimeLeft <= 0.0)
		{
			this.freeRefreshTimeLeft = 0.0;
			return;
		}
		this.freeRefreshTimeLeft -= (double)Time.unscaledDeltaTime;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000D8F2 File Offset: 0x0000BAF2
	public void RefreshTime_Reset()
	{
		this.freeRefreshTimeLeft = 180.0;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000D903 File Offset: 0x0000BB03
	public int Get_RefreshPrice()
	{
		if (this.freeRefreshTimeLeft <= 0.0)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000D91C File Offset: 0x0000BB1C
	public void TryPayAndRefresh()
	{
		int refreshPrice = this.Get_RefreshPrice();
		if (refreshPrice == 0)
		{
			this.RefreshTime_Reset();
			this.OnlyRefresh(true);
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_Refresh);
		}
		else
		{
			if (GameData.inst.GeometryCoin < (long)refreshPrice)
			{
				UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.lackOfGeometryCoin);
				return;
			}
			GameData.inst.GeometryCoin_Use((long)refreshPrice);
			this.OnlyRefresh(true);
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_Refresh);
			UI_Panel_Rune_RuneDetail.inst.ClearRuneGood();
			UI_Panel_Rune_RuneDetail.inst.UpdatePreview();
		}
		MySteamAchievement.TryUnlockAchievementWithName("RuneRefresh");
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
	private void OnlyRefresh(bool ifUpdatePanel)
	{
		this.refreshTimes++;
		this.runeGoods = new RuneGood[5];
		for (int i = 0; i < this.runeGoods.Length; i++)
		{
			this.runeGoods[i] = new RuneGood();
			this.runeGoods[i].InitRandom();
			this.runeGoods[i].indexInGood = i;
		}
		if (ifUpdatePanel && UI_Panel_Main_RunePanel.inst != null)
		{
			UI_Panel_Main_RunePanel.inst.Open();
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000DA4C File Offset: 0x0000BC4C
	public void Debug_AllGoods()
	{
		for (int i = 0; i < this.runeGoods.Length; i++)
		{
			if (this.runeGoods[i].theRune != null)
			{
				Debug.Log(string.Concat(new object[]
				{
					i,
					" ",
					this.runeGoods[i].thePrice,
					" \n",
					this.runeGoods[i].theRune.GetInfo_Total()
				}));
			}
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
	public static int Get_10GeometryCoinPrice(int times)
	{
		float num = Mathf.Round((Mathf.Pow(Mathf.Log((float)(times + 2)), 2.4f) + 1f) / 1.41f * 100f) / 100f;
		num = Mathf.Min(90f, num);
		return (num * 100000f).RoundToInt();
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000DB28 File Offset: 0x0000BD28
	public bool TryBuy10GeometryCoins()
	{
		int current10GeometryCoinPrice = this.Get_Current10GeometryCoinPrice();
		if (GameData.inst.Star < (long)current10GeometryCoinPrice)
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.talent_LackOfStar);
			return false;
		}
		GameData.inst.UseStar(current10GeometryCoinPrice);
		GameData.inst.GeometryCoin_Get(10L);
		this.buyGCoinTimes++;
		UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_BuyGeometryCoin);
		UI_Panel_Main_RunePanel.inst.Open();
		return true;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000DBAF File Offset: 0x0000BDAF
	public IEnumerator TryBuy10GeometryCoins_Repeat10Times()
	{
		int i = 0;
		while (i < 10 && this.TryBuy10GeometryCoins())
		{
			yield return null;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0000DBBE File Offset: 0x0000BDBE
	public IEnumerator TryBuy10GeometryCoins_Repeat100Times()
	{
		int i = 0;
		while (i < 100 && this.TryBuy10GeometryCoins())
		{
			yield return null;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000DBCD File Offset: 0x0000BDCD
	public int Get_Current10GeometryCoinPrice()
	{
		return RuneStore.Get_10GeometryCoinPrice(this.buyGCoinTimes);
	}

	// Token: 0x040001FA RID: 506
	public int refreshTimes;

	// Token: 0x040001FB RID: 507
	public int buyGCoinTimes;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	public RuneGood[] runeGoods;

	// Token: 0x040001FD RID: 509
	public double freeRefreshTimeLeft = 180.0;
}
