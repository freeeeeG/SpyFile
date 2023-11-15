using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000241 RID: 577
public class GameEndUI : IUserInterface
{
	// Token: 0x06000ED1 RID: 3793 RVA: 0x00026DBB File Offset: 0x00024FBB
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x00026DC9 File Offset: 0x00024FC9
	public override void Initialize()
	{
		base.Initialize();
		this.gainExp = 0;
		this.gameLevelPrefab.SetData();
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00026DE4 File Offset: 0x00024FE4
	public void SetGameResult(bool win)
	{
		Singleton<LevelManager>.Instance.LevelEnd = true;
		this.gainExp = Singleton<LevelManager>.Instance.GainExp(GameRes.CurrentWave);
		switch (Singleton<LevelManager>.Instance.CurrentLevel.ModeType)
		{
		case ModeType.Standard:
			this.ChallengeInfo.gameObject.SetActive(false);
			this.title.gameObject.SetActive(true);
			Singleton<LevelManager>.Instance.LevelWin = win;
			if (win)
			{
				this.title.text = GameMultiLang.GetTraduction("WIN") + GameMultiLang.GetTraduction("DIFFICULTY") + Singleton<LevelManager>.Instance.CurrentLevel.Level.ToString();
				Singleton<GameEvents>.Instance.TempWordTrigger(new TempWord(TempWordType.StandardWin, Singleton<LevelManager>.Instance.CurrentLevel.Level));
				if (Singleton<LevelManager>.Instance.PassDiifcutly < Singleton<LevelManager>.Instance.CurrentLevel.Level + 1)
				{
					Singleton<LevelManager>.Instance.PassDiifcutly = Singleton<LevelManager>.Instance.CurrentLevel.Level + 1;
				}
				this.titleBG.sprite = this.TitleBGs[Singleton<LevelManager>.Instance.CurrentLevel.Level + 1];
				if (Singleton<LevelManager>.Instance.CurrentLevel.Level < Singleton<LevelManager>.Instance.StandardLevels.Length)
				{
					this.NextLevelBtn.SetActive(true);
					this.RestartBtn.SetActive(false);
				}
				else
				{
					this.NextLevelBtn.SetActive(false);
					this.RestartBtn.SetActive(false);
				}
			}
			else
			{
				this.title.text = GameMultiLang.GetTraduction("LOSE");
				Singleton<GameEvents>.Instance.TempWordTrigger(new TempWord(TempWordType.StandardLose, Singleton<LevelManager>.Instance.CurrentLevel.Level));
				this.titleBG.sprite = this.TitleBGs[0];
				this.NextLevelBtn.SetActive(false);
				this.RestartBtn.SetActive(true);
			}
			break;
		case ModeType.Endless:
		{
			this.ChallengeInfo.gameObject.SetActive(false);
			this.title.gameObject.SetActive(true);
			this.title.text = GameMultiLang.GetTraduction("PASSLEVEL") + GameRes.CurrentWave.ToString() + GameMultiLang.GetTraduction("WAVE");
			if (Singleton<LevelManager>.Instance.CurrentLevel.Level == 1)
			{
				Singleton<PlayfabManager>.Instance.EndlessWave = GameRes.CurrentWave;
			}
			int num = Mathf.Clamp(GameRes.CurrentWave / 20, 0, 5);
			this.titleBG.sprite = this.TitleBGs[num + 2];
			Singleton<GameEvents>.Instance.TempWordTrigger(new TempWord(TempWordType.EndlessEnd, num));
			this.NextLevelBtn.SetActive(false);
			this.RestartBtn.SetActive(true);
			Singleton<LevelManager>.Instance.LevelWin = (GameRes.CurrentWave > 29);
			break;
		}
		case ModeType.Challenge:
		{
			int num2 = 0;
			num2 += GameRes.CurrentWave * 100;
			num2 += GameRes.Life * 20;
			num2 += GameRes.SkipTimes * 20;
			this.titleBG.sprite = this.TitleBGs[win ? 5 : 0];
			this.title.text = GameMultiLang.GetTraduction("SCORE") + ":" + num2.ToString();
			this.ChallengeInfo.gameObject.SetActive(true);
			this.ChallengeInfo.SetContent(GameMultiLang.GetTraduction("CHALLENGESCOREINFO"));
			this.NextLevelBtn.SetActive(false);
			this.RestartBtn.SetActive(true);
			Singleton<PlayfabManager>.Instance.ChallngeScore = num2;
			break;
		}
		}
		this.m_BillBoard.SetBillBoard();
		base.StartCoroutine(this.SetValueCor());
		this.SetAchievement();
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00027178 File Offset: 0x00025378
	private void SetAchievement()
	{
		if (Singleton<LevelManager>.Instance.LevelWin)
		{
			if (GameRes.MaxPath >= 150)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_LONGPATH");
			}
			if (GameRes.TotalRefactor >= 50)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_BILLIONS");
			}
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Standard && Singleton<LevelManager>.Instance.CurrentLevel.Level == 6 && (DateTime.Now - GameRes.LevelStart).Minutes <= 9)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_FASTLEVEL6");
			}
			if ((float)GameRes.GainGold / (float)GameRes.CurrentWave > 400f)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_MONEY");
			}
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Standard)
			{
				if (Singleton<LevelManager>.Instance.CurrentLevel.Level >= 2 && GameRes.Life == Singleton<LevelManager>.Instance.CurrentLevel.PlayerHealth)
				{
					Singleton<LevelManager>.Instance.SetAchievement("ACH_EASY");
				}
				if (Singleton<LevelManager>.Instance.CurrentLevel.Level >= 1 && GameRes.TotalRefactor == 0)
				{
					Singleton<LevelManager>.Instance.SetAchievement("ACH_DECEIVE");
				}
				if (BoardSystem.shortestPath.Count <= 3)
				{
					Singleton<LevelManager>.Instance.SetAchievement("ACH_EXTREME");
				}
			}
		}
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x000272C3 File Offset: 0x000254C3
	private void SetExp()
	{
		this.expValueTxt.text = GameMultiLang.GetTraduction("EXPVALUE") + ":" + this.gainExp.ToString();
		this.gameLevelPrefab.AddExp(this.gainExp);
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00027300 File Offset: 0x00025500
	private IEnumerator SetValueCor()
	{
		this.passTimeTxt.text = "";
		this.totalCompositeTxt.text = "";
		this.totalDamageTxt.text = "";
		this.maxPathTxt.text = "";
		this.maxMarkTxt.text = "";
		this.gainGoldTxt.text = "";
		this.expValueTxt.text = "";
		yield return new WaitForSeconds(this.waittime);
		this.passTimeTxt.text = (GameRes.LevelStart - DateTime.Now).ToString("hh\\:mm\\:ss");
		float delta = (float)GameRes.TotalRefactor / (float)this.changeSpeed;
		this.result = 0f;
		int num;
		for (int i = 0; i < this.changeSpeed; i = num + 1)
		{
			this.result += delta;
			this.totalCompositeTxt.text = Mathf.RoundToInt(this.result).ToString();
			yield return new WaitForSeconds(this.waittime);
			num = i;
		}
		this.totalCompositeTxt.text = GameRes.TotalRefactor.ToString();
		delta = (float)GameRes.TotalDamage / (float)this.changeSpeed;
		this.result = 0f;
		for (int i = 0; i < this.changeSpeed; i = num + 1)
		{
			this.result += delta;
			this.totalDamageTxt.text = Mathf.RoundToInt(this.result).ToString();
			yield return new WaitForSeconds(this.waittime);
			num = i;
		}
		this.totalDamageTxt.text = GameRes.TotalDamage.ToString();
		delta = (float)GameRes.MaxPath / (float)this.changeSpeed;
		this.result = 0f;
		for (int i = 0; i < this.changeSpeed; i = num + 1)
		{
			this.result += delta;
			this.maxPathTxt.text = Mathf.RoundToInt(this.result).ToString();
			yield return new WaitForSeconds(this.waittime);
			num = i;
		}
		this.maxPathTxt.text = GameRes.MaxPath.ToString();
		delta = (float)GameRes.MaxMark / (float)this.changeSpeed;
		this.result = 0f;
		for (int i = 0; i < this.changeSpeed; i = num + 1)
		{
			this.result += delta;
			this.maxMarkTxt.text = Mathf.RoundToInt(this.result).ToString();
			yield return new WaitForSeconds(this.waittime);
			num = i;
		}
		this.maxMarkTxt.text = GameRes.MaxMark.ToString();
		delta = (float)GameRes.GainGold / (float)this.changeSpeed;
		this.result = 0f;
		for (int i = 0; i < this.changeSpeed; i = num + 1)
		{
			this.result += delta;
			this.gainGoldTxt.text = Mathf.RoundToInt(this.result).ToString();
			yield return new WaitForSeconds(this.waittime);
			num = i;
		}
		this.gainGoldTxt.text = GameRes.GainGold.ToString();
		this.SetExp();
		base.StopCoroutine(this.SetValueCor());
		yield break;
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0002730F File Offset: 0x0002550F
	public void NextLevelBtnClick()
	{
		Singleton<LevelManager>.Instance.StartNewGame(Singleton<LevelManager>.Instance.CurrentLevel.ModeID + 1);
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0002732C File Offset: 0x0002552C
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("Show", true);
	}

	// Token: 0x04000737 RID: 1847
	[SerializeField]
	private TurretBillboard m_BillBoard;

	// Token: 0x04000738 RID: 1848
	[SerializeField]
	private Image titleBG;

	// Token: 0x04000739 RID: 1849
	[SerializeField]
	private TextMeshProUGUI title;

	// Token: 0x0400073A RID: 1850
	[SerializeField]
	private Text passTimeTxt;

	// Token: 0x0400073B RID: 1851
	[SerializeField]
	private Text totalCompositeTxt;

	// Token: 0x0400073C RID: 1852
	[SerializeField]
	private Text totalDamageTxt;

	// Token: 0x0400073D RID: 1853
	[SerializeField]
	private Text maxPathTxt;

	// Token: 0x0400073E RID: 1854
	[SerializeField]
	private Text maxMarkTxt;

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private Text gainGoldTxt;

	// Token: 0x04000740 RID: 1856
	[SerializeField]
	private Text expValueTxt;

	// Token: 0x04000741 RID: 1857
	[SerializeField]
	private GameLevelHolder gameLevelPrefab;

	// Token: 0x04000742 RID: 1858
	[SerializeField]
	private GameObject NextLevelBtn;

	// Token: 0x04000743 RID: 1859
	[SerializeField]
	private GameObject RestartBtn;

	// Token: 0x04000744 RID: 1860
	[SerializeField]
	private InfoBtn ChallengeInfo;

	// Token: 0x04000745 RID: 1861
	[Header("标题底图")]
	[SerializeField]
	private Sprite[] TitleBGs;

	// Token: 0x04000746 RID: 1862
	private int changeSpeed = 10;

	// Token: 0x04000747 RID: 1863
	private float waittime = 0.05f;

	// Token: 0x04000748 RID: 1864
	private float result;

	// Token: 0x04000749 RID: 1865
	private Animator anim;

	// Token: 0x0400074A RID: 1866
	private int gainExp;
}
