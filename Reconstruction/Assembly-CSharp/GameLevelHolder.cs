using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000272 RID: 626
public class GameLevelHolder : MonoBehaviour
{
	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06000F88 RID: 3976 RVA: 0x000297B8 File Offset: 0x000279B8
	// (set) Token: 0x06000F89 RID: 3977 RVA: 0x000297C0 File Offset: 0x000279C0
	public int CurrentExp
	{
		get
		{
			return this.currentExp;
		}
		set
		{
			this.currentExp = value;
			this.ExpTxt.text = value.ToString() + "/" + this.NextExp.ToString();
			this.ExpProgress.fillAmount = (float)this.currentExp / (float)this.NextExp;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00029818 File Offset: 0x00027A18
	// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00029820 File Offset: 0x00027A20
	public int CurrentLevel
	{
		get
		{
			return this.currentLevel;
		}
		set
		{
			this.currentLevel = value;
			this.LevelTxt.text = this.currentLevel.ToString();
			if (this.currentLevel == Singleton<LevelManager>.Instance.MaxGameLevel)
			{
				this.CurrentExp = this.NextExp;
				return;
			}
			this.NextExp = Singleton<LevelManager>.Instance.GetLevelInfo(this.currentLevel).ExpRequire;
		}
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00029884 File Offset: 0x00027A84
	// (set) Token: 0x06000F8D RID: 3981 RVA: 0x0002988C File Offset: 0x00027A8C
	public int NextExp
	{
		get
		{
			return this.nextExp;
		}
		set
		{
			this.nextExp = value;
			this.ExpTxt.text = this.CurrentExp.ToString() + "/" + this.nextExp.ToString();
			this.ExpProgress.fillAmount = (float)this.currentExp / (float)this.NextExp;
		}
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x000298E8 File Offset: 0x00027AE8
	public void SetData()
	{
		this.CurrentLevel = Singleton<LevelManager>.Instance.GameLevel;
		this.CurrentExp = Singleton<LevelManager>.Instance.GameExp;
		this.NextExp = Singleton<LevelManager>.Instance.GetLevelInfo(this.CurrentLevel).ExpRequire;
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00029925 File Offset: 0x00027B25
	public void AddExp(int Exp)
	{
		this.ExpCor(Exp);
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x0002992E File Offset: 0x00027B2E
	private void ExpCor(int Exp)
	{
		base.StartCoroutine(this.AddExpCor(Exp));
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x0002993E File Offset: 0x00027B3E
	private IEnumerator AddExpCor(int Exp)
	{
		if (this.CurrentLevel >= Singleton<LevelManager>.Instance.MaxGameLevel)
		{
			int final = this.CurrentExp + Exp;
			float delta = (float)Exp / (float)this.changeSpeed;
			int num;
			for (int i = 0; i < this.changeSpeed; i = num + 1)
			{
				this.CurrentExp += (int)delta;
				yield return new WaitForSeconds(0.02f);
				num = i;
			}
			this.CurrentExp = final;
		}
		else
		{
			int final = this.NextExp - this.CurrentExp;
			if (Exp >= final)
			{
				float delta = (float)final / (float)this.changeSpeed;
				int num;
				for (int i = 0; i < this.changeSpeed; i = num + 1)
				{
					this.CurrentExp += (int)delta;
					yield return new WaitForSeconds(0.02f);
					num = i;
				}
				this.CurrentExp = 0;
				num = this.CurrentLevel;
				this.CurrentLevel = num + 1;
				Singleton<TipsManager>.Instance.ShowBonusTips(Singleton<LevelManager>.Instance.GetLevelInfo(this.CurrentLevel));
				Singleton<Sound>.Instance.PlayUISound("Sound_LevelUp");
				this.ExpCor(Exp - final);
			}
			else
			{
				int i = this.CurrentExp + Exp;
				float delta = (float)Exp / (float)this.changeSpeed;
				int num;
				for (int j = 0; j < this.changeSpeed; j = num + 1)
				{
					this.CurrentExp += (int)delta;
					yield return new WaitForSeconds(0.02f);
					num = j;
				}
				this.CurrentExp = i;
			}
		}
		yield break;
	}

	// Token: 0x040007EE RID: 2030
	[SerializeField]
	private Text LevelTxt;

	// Token: 0x040007EF RID: 2031
	[SerializeField]
	private Text ExpTxt;

	// Token: 0x040007F0 RID: 2032
	[SerializeField]
	private Image ExpProgress;

	// Token: 0x040007F1 RID: 2033
	private int changeSpeed = 50;

	// Token: 0x040007F2 RID: 2034
	private int currentExp;

	// Token: 0x040007F3 RID: 2035
	private int nextExp;

	// Token: 0x040007F4 RID: 2036
	private int currentLevel;
}
