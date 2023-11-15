using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000299 RID: 665
public class TurretTips : TileTips
{
	// Token: 0x0600104B RID: 4171 RVA: 0x0002C064 File Offset: 0x0002A264
	public override void Initialize()
	{
		base.Initialize();
		this.CriticalInfo.SetContent(GameMultiLang.GetTraduction("CRITICALINFO"));
		this.SplashInfo.SetContent(GameMultiLang.GetTraduction("SPLASHINFO"));
		this.SlowInfo.SetContent(GameMultiLang.GetTraduction("SLOWINFO"));
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0002C0B6 File Offset: 0x0002A2B6
	public override void Show()
	{
		base.Show();
		this.TileInfo_Anim.SetTrigger("Show");
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0002C0CE File Offset: 0x0002A2CE
	public override void Hide()
	{
		base.Hide();
		this.m_Strategy = null;
		this.showingTurret = false;
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
	public void CloseBtnClick()
	{
		this.CloseTips();
		if (BluePrintGrid.SelectingGrid != null)
		{
			BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0002C104 File Offset: 0x0002A304
	private void AreaSetControl(TurretAttribute att, int id, int quality)
	{
		string text = GameMultiLang.GetTraduction(att.Name);
		this.TurretQualitySetter.gameObject.SetActive(id == 0);
		this.TurretQualitySetter.SetSwitchCost(GameRes.SwitchTurretCost);
		switch (att.StrategyType)
		{
		case StrategyType.Element:
			text = StaticData.FormElementName(att.element, quality) + text;
			this.AttributeArea.SetActive(true);
			this.DesArea.SetActive(true);
			this.TurretQualitySetter.UpgradeBtn.SetActive(false);
			this.ElementsHolder.gameObject.SetActive(false);
			this.AnalysisArea.SetActive(id == 0);
			this.RareSetter.gameObject.SetActive(false);
			this.MainFuncArea.SetActive(false);
			this.Description.text = GameMultiLang.GetTraduction(att.Name + "SKILL");
			this.Icon.sprite = att.TurretLevels[quality - 1].TurretIcon;
			break;
		case StrategyType.Composite:
			this.AttributeArea.SetActive(this.isFold);
			this.DesArea.SetActive(this.isFold);
			this.ElementsHolder.gameObject.SetActive(id != 1 && att.Rare > 0);
			this.AnalysisArea.SetActive(id == 0);
			this.MainFuncArea.SetActive(id == 2);
			this.RareSetter.SetRare(att.Rare);
			this.TurretQualitySetter.SetLevel(this.m_Strategy);
			this.SetElementSkill();
			this.Description.text = StaticData.GetTurretDes(this.m_Strategy.TurretSkills[0]);
			this.Icon.sprite = att.TurretLevels[quality - 1].TurretIcon;
			break;
		case StrategyType.Building:
			this.AttributeArea.SetActive(false);
			this.DesArea.SetActive(true);
			this.RareSetter.gameObject.SetActive(false);
			this.AnalysisArea.SetActive(false);
			this.MainFuncArea.SetActive(id == 2);
			this.ElementsHolder.gameObject.SetActive(false);
			this.SetElementSkill();
			this.TurretQualitySetter.UpgradeBtn.SetActive(false);
			this.Icon.sprite = att.Icon;
			this.Description.text = StaticData.GetTurretDes(this.m_Strategy.TurretSkills[0]);
			break;
		}
		this.Name.text = text;
		string text2 = "";
		switch ((id == 1) ? att.RangeType : this.m_Strategy.RangeType)
		{
		case RangeType.Circle:
			text2 = GameMultiLang.GetTraduction("RANGETYPE1");
			break;
		case RangeType.HalfCircle:
			text2 = GameMultiLang.GetTraduction("RANGETYPE2");
			break;
		case RangeType.Line:
			text2 = GameMultiLang.GetTraduction("RANGETYPE3");
			break;
		}
		this.RangeTypeValue.text = text2;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0002C3F4 File Offset: 0x0002A5F4
	public void ReadTurret(StrategyBase Strategy, int showID)
	{
		this.m_Strategy = Strategy;
		this.AreaSetControl(this.m_Strategy.Attribute, showID, Strategy.Quality);
		if (showID == 2)
		{
			this.UpdateBluePrintInfo();
			return;
		}
		this.UpdateRealTimeValue();
		this.showingTurret = true;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0002C430 File Offset: 0x0002A630
	public void ReadAttribute(TurretAttribute att)
	{
		int num = (att.StrategyType == StrategyType.Element) ? 5 : 3;
		this.AreaSetControl(att, 0, num);
		this.UpdatePreviewValue(att, num - 1);
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0002C460 File Offset: 0x0002A660
	public void SetElementSkill()
	{
		foreach (TipsElementConstruct tipsElementConstruct in this.elementConstructs)
		{
			tipsElementConstruct.gameObject.SetActive(false);
			tipsElementConstruct.SetStrategy(this.m_Strategy, this);
		}
		for (int i = 0; i < 5; i++)
		{
			this.elementConstructs[i].gameObject.SetActive(true);
			if (i < this.m_Strategy.TurretSkills.Count - 1)
			{
				this.elementConstructs[i].SetElements((ElementSkill)this.m_Strategy.TurretSkills[i + 1]);
			}
			else if (i < this.m_Strategy.ElementSKillSlot)
			{
				this.elementConstructs[i].SetEmpty();
			}
			else
			{
				this.elementConstructs[i].SetUnlock(i, i == this.m_Strategy.ElementSKillSlot);
			}
		}
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x0002C570 File Offset: 0x0002A770
	private void UpdateSkillValues()
	{
		for (int i = 0; i < this.m_Strategy.ElementSKillSlot; i++)
		{
			this.elementConstructs[i].UpdateDes();
		}
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0002C5A4 File Offset: 0x0002A7A4
	private void UpdatePreviewValue(TurretAttribute att, int quality)
	{
		this.AttackValue.text = att.TurretLevels[quality].AttackDamage.ToString();
		this.AttackChangeTxt.text = "";
		this.SpeedValue.text = att.TurretLevels[quality].AttackSpeed.ToString();
		this.SpeedChangeTxt.text = "";
		this.RangeValue.text = att.TurretLevels[quality].AttackRange.ToString();
		this.RangeChangeTxt.text = "";
		this.CriticalValue.text = (att.TurretLevels[quality].CriticalRate * 100f).ToString() + "%";
		this.CriticalChangeTxt.text = "";
		this.CritDmgValue.text = (StaticData.DefaultCritDmg * 100f).ToString() + "%";
		this.CritDmgChangeTxt.text = "";
		this.SplashRangeValue.text = att.TurretLevels[quality].SplashRange.ToString();
		this.SplashChangeTxt.text = "";
		this.SlowRateValue.text = att.TurretLevels[quality].SlowRate.ToString();
		this.SlowRateChangeTxt.text = "";
		this.SplashDmgValue.text = (StaticData.DefaultSplashDmg * 100f).ToString() + "%";
		this.SplashDmgChangeTxt.text = "";
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0002C75C File Offset: 0x0002A95C
	private void UpdateRealTimeValue()
	{
		this.AttackValue.text = this.m_Strategy.FinalAttack.ToString();
		this.AttackChangeTxt.text = "";
		this.SpeedValue.text = this.m_Strategy.FinalFireRate.ToString();
		this.SpeedChangeTxt.text = "";
		this.RangeValue.text = this.m_Strategy.FinalRange.ToString();
		this.RangeChangeTxt.text = "";
		this.CriticalValue.text = Mathf.RoundToInt(this.m_Strategy.FinalCriticalRate * 100f).ToString() + "%";
		this.CriticalChangeTxt.text = "";
		this.CritDmgValue.text = Mathf.RoundToInt(this.m_Strategy.FinalCriticalPercentage * 100f).ToString() + "%";
		this.CritDmgChangeTxt.text = "";
		this.SplashRangeValue.text = this.m_Strategy.FinalSplashRange.ToString();
		this.SplashChangeTxt.text = "";
		this.SlowRateValue.text = this.m_Strategy.FinalSlowRate.ToString();
		this.SlowRateChangeTxt.text = "";
		this.SplashDmgValue.text = Mathf.RoundToInt(this.m_Strategy.FinalSplashPercentage * 100f).ToString() + "%";
		this.SplashDmgChangeTxt.text = "";
		this.AnalysisValue.text = this.m_Strategy.TurnDamage.ToString();
		this.ElementsHolder.SetElementCount(this.m_Strategy);
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0002C948 File Offset: 0x0002AB48
	private void UpdateBluePrintInfo()
	{
		this.AttackValue.text = this.m_Strategy.FinalAttack.ToString();
		this.AttackChangeTxt.text = ((this.m_Strategy.FinalAttack > this.m_Strategy.InitAttack) ? ("+" + (this.m_Strategy.FinalAttack - this.m_Strategy.InitAttack).ToString()) : "");
		this.SpeedValue.text = this.m_Strategy.FinalFireRate.ToString();
		this.SpeedChangeTxt.text = ((this.m_Strategy.FinalFireRate > this.m_Strategy.InitFireRate) ? ("+" + (this.m_Strategy.FinalFireRate - this.m_Strategy.InitFireRate).ToString()) : "");
		this.RangeValue.text = this.m_Strategy.FinalRange.ToString();
		this.RangeChangeTxt.text = ((this.m_Strategy.FinalRange > this.m_Strategy.InitRange) ? ("+" + (this.m_Strategy.FinalRange - this.m_Strategy.InitRange).ToString()) : "");
		this.CriticalValue.text = (this.m_Strategy.FinalCriticalRate * 100f).ToString() + "%";
		this.CriticalChangeTxt.text = ((this.m_Strategy.FinalCriticalRate > this.m_Strategy.InitCriticalRate) ? ("+" + ((this.m_Strategy.FinalCriticalRate - this.m_Strategy.InitCriticalRate) * 100f).ToString() + "%") : "");
		this.CritDmgValue.text = (this.m_Strategy.FinalCriticalPercentage * 100f).ToString() + "%";
		this.CritDmgChangeTxt.text = ((this.m_Strategy.FinalCriticalPercentage > StaticData.DefaultCritDmg) ? ("+" + Mathf.RoundToInt((this.m_Strategy.FinalCriticalPercentage - StaticData.DefaultCritDmg) * 100f).ToString() + "%") : "");
		this.SplashRangeValue.text = this.m_Strategy.FinalSplashRange.ToString();
		this.SplashChangeTxt.text = ((this.m_Strategy.FinalSplashRange > this.m_Strategy.InitSplashRange) ? ("+" + (this.m_Strategy.FinalSplashRange - this.m_Strategy.InitSplashRange).ToString()) : "");
		this.SplashDmgValue.text = (this.m_Strategy.FinalSplashPercentage * 100f).ToString() + "%";
		this.SplashDmgChangeTxt.text = ((this.m_Strategy.FinalSplashPercentage > StaticData.DefaultSplashDmg) ? ("+" + Mathf.RoundToInt((this.m_Strategy.FinalSplashPercentage - StaticData.DefaultSplashDmg) * 100f).ToString() + "%") : "");
		this.SlowRateValue.text = this.m_Strategy.FinalSlowRate.ToString();
		this.SlowRateChangeTxt.text = ((this.m_Strategy.FinalSlowRate > this.m_Strategy.InitSlowRate) ? ("+" + (this.m_Strategy.FinalSlowRate - this.m_Strategy.InitSlowRate).ToString()) : "");
		this.ElementsHolder.SetElementCount(this.m_Strategy);
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x0002CD34 File Offset: 0x0002AF34
	public void UpdateLevelUpInfo()
	{
		if (this.m_Strategy.Quality >= 3)
		{
			return;
		}
		float num = this.m_Strategy.NextAttack - this.m_Strategy.FinalAttack;
		this.AttackValue.text = this.m_Strategy.FinalAttack.ToString();
		this.AttackChangeTxt.text = ((num > 0f) ? ("+" + num.ToString()) : "");
		float num2 = this.m_Strategy.NextFirarate - this.m_Strategy.FinalFireRate;
		this.SpeedValue.text = this.m_Strategy.FinalFireRate.ToString();
		this.SpeedChangeTxt.text = ((num2 > 0f) ? ("+" + num2.ToString()) : "");
		float num3 = this.m_Strategy.NextCriticalRate - this.m_Strategy.BaseCriticalRate;
		this.CriticalValue.text = (this.m_Strategy.FinalCriticalRate * 100f).ToString() + "%";
		this.CriticalChangeTxt.text = ((num3 > 0f) ? ("+" + (num3 * 100f).ToString()) : "");
		float num4 = this.m_Strategy.NextSplashRange - this.m_Strategy.BaseSplashRange;
		this.SplashRangeValue.text = this.m_Strategy.FinalSplashRange.ToString();
		this.SplashChangeTxt.text = ((num4 > 0f) ? ("+" + num4.ToString()) : "");
		float num5 = this.m_Strategy.NextSlowRate - this.m_Strategy.BaseSlowRate;
		this.SlowRateValue.text = this.m_Strategy.FinalSlowRate.ToString();
		this.SlowRateChangeTxt.text = ((num5 > 0f) ? ("+" + num5.ToString()) : "");
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0002CF58 File Offset: 0x0002B158
	public void UpgradeBtnClick(int cost)
	{
		if (this.m_Strategy.Quality < 3 && Singleton<GameManager>.Instance.ConsumeMoney(cost))
		{
			StrategyBase strategy = this.m_Strategy;
			int quality = strategy.Quality;
			strategy.Quality = quality + 1;
			this.m_Strategy.SetQualityValue();
			this.m_Strategy.Concrete.SetGraphic();
			this.m_Strategy.Concrete.m_ContentStruct.Quality = this.m_Strategy.Quality;
			this.AreaSetControl(this.m_Strategy.Attribute, 0, this.m_Strategy.Quality);
			this.UpdateRealTimeValue();
			((RefactorTurret)this.m_Strategy.Concrete).ShowLandedEffect();
		}
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0002D00E File Offset: 0x0002B20E
	public void FoldElementArea()
	{
		if (this.isFolding)
		{
			return;
		}
		this.isFold = !this.isFold;
		base.StartCoroutine(this.FoldCor());
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0002D035 File Offset: 0x0002B235
	private IEnumerator FoldCor()
	{
		this.isFolding = true;
		this.foldArrowImg.rectTransform.localScale = (this.isFold ? new Vector2(1f, -1f) : new Vector2(1f, 1f));
		float x = this.ElementsHolder.GetComponent<RectTransform>().sizeDelta.x;
		this.ElementsHolder.GetComponent<RectTransform>().DOSizeDelta(new Vector2(x, (float)(this.isFold ? 390 : 790)), 0.3f, false);
		this.AttributeArea.SetActive(this.isFold);
		this.DesArea.SetActive(this.isFold);
		yield return new WaitForSeconds(0.3f);
		this.isFolding = false;
		yield break;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0002D044 File Offset: 0x0002B244
	private void FixedUpdate()
	{
		if (this.showingTurret)
		{
			this.UpdateRealTimeValue();
			this.UpdateSkillValues();
		}
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0002D05A File Offset: 0x0002B25A
	public void CompositeBtnClick()
	{
		Singleton<GameManager>.Instance.CompositeShape(BluePrintGrid.SelectingGrid);
	}

	// Token: 0x04000893 RID: 2195
	[Header("Area")]
	[SerializeField]
	private GameObject AttributeArea;

	// Token: 0x04000894 RID: 2196
	[SerializeField]
	private GameObject AnalysisArea;

	// Token: 0x04000895 RID: 2197
	[SerializeField]
	private GameObject MainFuncArea;

	// Token: 0x04000896 RID: 2198
	[SerializeField]
	private GameObject DesArea;

	// Token: 0x04000897 RID: 2199
	[SerializeField]
	private TurretQualitySetter TurretQualitySetter;

	// Token: 0x04000898 RID: 2200
	[SerializeField]
	private RareInfoSetter RareSetter;

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private ElementHolder ElementsHolder;

	// Token: 0x0400089A RID: 2202
	[SerializeField]
	private Text RangeTypeValue;

	// Token: 0x0400089B RID: 2203
	[SerializeField]
	private Text AttackValue;

	// Token: 0x0400089C RID: 2204
	[SerializeField]
	private Text SpeedValue;

	// Token: 0x0400089D RID: 2205
	[SerializeField]
	private Text RangeValue;

	// Token: 0x0400089E RID: 2206
	[SerializeField]
	private Text CriticalValue;

	// Token: 0x0400089F RID: 2207
	[SerializeField]
	private Text CritDmgValue;

	// Token: 0x040008A0 RID: 2208
	[SerializeField]
	private Text SplashRangeValue;

	// Token: 0x040008A1 RID: 2209
	[SerializeField]
	private Text SplashDmgValue;

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private Text SlowRateValue;

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private Text IntensifyValue;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private Text AnalysisValue;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private List<TipsElementConstruct> elementConstructs;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private Text AttackChangeTxt;

	// Token: 0x040008A7 RID: 2215
	[SerializeField]
	private Text SpeedChangeTxt;

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private Text RangeChangeTxt;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private Text CriticalChangeTxt;

	// Token: 0x040008AA RID: 2218
	[SerializeField]
	private Text CritDmgChangeTxt;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private Text SplashChangeTxt;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private Text SplashDmgChangeTxt;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private Text SlowRateChangeTxt;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private Text IntentsifyChangeTxt;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private InfoBtn CriticalInfo;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private InfoBtn SplashInfo;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private InfoBtn SlowInfo;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private Image foldArrowImg;

	// Token: 0x040008B3 RID: 2227
	public StrategyBase m_Strategy;

	// Token: 0x040008B4 RID: 2228
	public bool showingTurret;

	// Token: 0x040008B5 RID: 2229
	private bool isFold = true;

	// Token: 0x040008B6 RID: 2230
	private bool isFolding;

	// Token: 0x040008B7 RID: 2231
	[SerializeField]
	private Animator TileInfo_Anim;
}
