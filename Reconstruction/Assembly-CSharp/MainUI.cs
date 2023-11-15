using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000149 RID: 329
public class MainUI : IUserInterface
{
	// Token: 0x1700032E RID: 814
	// (set) Token: 0x060008E0 RID: 2272 RVA: 0x000184CC File Offset: 0x000166CC
	public int CurrentWave
	{
		set
		{
			string @string = PlayerPrefs.GetString("_language");
			if (@string == "ch")
			{
				this.waveTxt.text = GameMultiLang.GetTraduction("NUM") + value.ToString() + ((Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Endless) ? ("/" + Singleton<LevelManager>.Instance.CurrentLevel.Wave.ToString()) : "") + GameMultiLang.GetTraduction("WAVE");
				return;
			}
			if (!(@string == "en"))
			{
				return;
			}
			this.waveTxt.text = GameMultiLang.GetTraduction("WAVE") + value.ToString() + ((Singleton<LevelManager>.Instance.CurrentLevel.ModeType != ModeType.Endless) ? ("/" + Singleton<LevelManager>.Instance.CurrentLevel.Wave.ToString()) : "");
		}
	}

	// Token: 0x1700032F RID: 815
	// (set) Token: 0x060008E1 RID: 2273 RVA: 0x000185BA File Offset: 0x000167BA
	public int Coin
	{
		set
		{
			this.coinTxt.text = value.ToString();
		}
	}

	// Token: 0x17000330 RID: 816
	// (set) Token: 0x060008E2 RID: 2274 RVA: 0x000185CE File Offset: 0x000167CE
	public int Life
	{
		set
		{
			this.PlayerLifeTxt.text = value.ToString() + "/" + Singleton<LevelManager>.Instance.CurrentLevel.PlayerHealth.ToString();
		}
	}

	// Token: 0x17000331 RID: 817
	// (set) Token: 0x060008E3 RID: 2275 RVA: 0x00018600 File Offset: 0x00016800
	public int GameSpeed
	{
		set
		{
			this.GameSpeedImg.sprite = this.GameSpeedSprites[value - 1];
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00018618 File Offset: 0x00016818
	public override void Initialize()
	{
		base.Initialize();
		this.GameSpeed = 1;
		MainUI.CoinAnim = this.m_RootUI.transform.Find("Coin").GetComponent<Animator>();
		MainUI.LifeAnim = this.m_RootUI.transform.Find("Life").GetComponent<Animator>();
		MainUI.WaveAnim = this.m_RootUI.transform.Find("Wave").GetComponent<Animator>();
		this.m_TechPanel.Initialize();
		this.SetRules();
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000186A0 File Offset: 0x000168A0
	public override void Release()
	{
		base.Release();
		this.GameSpeed = 1;
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000186AF File Offset: 0x000168AF
	public static void PlayMainUIAnim(int part, string key, bool value)
	{
		switch (part)
		{
		case 0:
			MainUI.CoinAnim.SetBool(key, value);
			return;
		case 1:
			MainUI.LifeAnim.SetBool(key, value);
			return;
		case 2:
			MainUI.WaveAnim.SetBool(key, value);
			return;
		default:
			return;
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x000186EA File Offset: 0x000168EA
	public override void Show()
	{
		MainUI.PlayMainUIAnim(0, "Show", true);
		MainUI.PlayMainUIAnim(1, "Show", true);
		MainUI.PlayMainUIAnim(2, "Show", true);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00018710 File Offset: 0x00016910
	public override void Hide()
	{
		MainUI.PlayMainUIAnim(0, "Show", false);
		MainUI.PlayMainUIAnim(1, "Show", false);
		MainUI.PlayMainUIAnim(2, "Show", false);
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00018736 File Offset: 0x00016936
	public void PrepareNextWave(List<EnemySequence> sequences, EnemyType nextBoss, int nextBossWave)
	{
		this.m_WaveInfoSetter.SetWaveInfo(sequences, nextBoss, nextBossWave);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00018746 File Offset: 0x00016946
	public bool ConsumeMoney(int cost)
	{
		if (GameRes.Coin >= cost)
		{
			GameRes.Coin -= cost;
			return true;
		}
		Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMONEY"));
		return false;
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00018773 File Offset: 0x00016973
	public void GuideBookBtnClick()
	{
		Singleton<GuideGirlSystem>.Instance.ShowGuideBook(0);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00018780 File Offset: 0x00016980
	public void GameSpeedBtnClick()
	{
		GameRes.GameSpeed++;
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x00018790 File Offset: 0x00016990
	public void SetRules()
	{
		if (RuleFactory.BattleRules.Count > 0)
		{
			string text = "";
			foreach (Rule rule in RuleFactory.BattleRules)
			{
				text = text + rule.Description + "\n";
			}
			this.m_RuleBtn.SetContent(text);
			this.m_RuleBtn.gameObject.SetActive(true);
			return;
		}
		this.m_RuleBtn.gameObject.SetActive(false);
	}

	// Token: 0x0400049E RID: 1182
	private static Animator CoinAnim;

	// Token: 0x0400049F RID: 1183
	private static Animator WaveAnim;

	// Token: 0x040004A0 RID: 1184
	private static Animator LifeAnim;

	// Token: 0x040004A1 RID: 1185
	[SerializeField]
	private Image GameSpeedImg;

	// Token: 0x040004A2 RID: 1186
	[SerializeField]
	private Text PlayerLifeTxt;

	// Token: 0x040004A3 RID: 1187
	[SerializeField]
	private Text coinTxt;

	// Token: 0x040004A4 RID: 1188
	[SerializeField]
	private WaveInfoSetter m_WaveInfoSetter;

	// Token: 0x040004A5 RID: 1189
	[SerializeField]
	private Text waveTxt;

	// Token: 0x040004A6 RID: 1190
	[SerializeField]
	private TechListPanel m_TechPanel;

	// Token: 0x040004A7 RID: 1191
	[SerializeField]
	private InfoBtn m_RuleBtn;

	// Token: 0x040004A8 RID: 1192
	[SerializeField]
	private Sprite[] GameSpeedSprites;
}
