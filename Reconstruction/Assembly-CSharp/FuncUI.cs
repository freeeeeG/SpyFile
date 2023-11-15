using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023F RID: 575
public class FuncUI : IUserInterface
{
	// Token: 0x17000532 RID: 1330
	// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x00026A14 File Offset: 0x00024C14
	public float DiscountRate
	{
		set
		{
			this.DiscountTxt.text = (GameRes.BuildDiscount * 100f).ToString() + "%";
			this.m_DrawInfo.SetContent(GameMultiLang.GetTraduction("DRAWINFO") + (GameRes.BuildDiscount * 100f).ToString() + "%");
		}
	}

	// Token: 0x17000533 RID: 1331
	// (set) Token: 0x06000EC3 RID: 3779 RVA: 0x00026A7C File Offset: 0x00024C7C
	public int BuyShapeCost
	{
		set
		{
			this.DrawBtnTxt.text = "<sprite=7>" + GameRes.BuildCost.ToString();
		}
	}

	// Token: 0x17000534 RID: 1332
	// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00026AAB File Offset: 0x00024CAB
	public int SystemLevel
	{
		set
		{
			this.SystemLevelTxt.text = value.ToString();
		}
	}

	// Token: 0x17000535 RID: 1333
	// (set) Token: 0x06000EC5 RID: 3781 RVA: 0x00026ABF File Offset: 0x00024CBF
	public int SystemUpgradeCost
	{
		set
		{
			if (GameRes.SystemLevel < Singleton<StaticData>.Instance.SystemMaxLevel)
			{
				this.SystemUpgradeCostTxt.text = "<sprite=7>" + value.ToString();
				return;
			}
			this.SystemUpgradeCostTxt.text = "MAX";
		}
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00026B00 File Offset: 0x00024D00
	public override void Initialize()
	{
		base.Initialize();
		FuncUI.BuildBtnAnim = this.m_RootUI.transform.Find("Build").GetComponent<Animator>();
		FuncUI.NextWaveBtnAnim = this.m_RootUI.transform.Find("NextWave").GetComponent<Animator>();
		FuncUI.SystemBtnAnim = this.m_RootUI.transform.Find("System").GetComponent<Animator>();
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x00026B70 File Offset: 0x00024D70
	public override void Show()
	{
		this.m_Active = true;
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge)
		{
			FuncUI.BuildBtnAnim.SetBool("Show", false);
			FuncUI.SystemBtnAnim.SetBool("Show", false);
		}
		else
		{
			FuncUI.BuildBtnAnim.SetBool("Show", true);
			FuncUI.SystemBtnAnim.SetBool("Show", true);
		}
		FuncUI.NextWaveBtnAnim.SetBool("Show", true);
		this.isDrawingAnim = false;
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x00026BEF File Offset: 0x00024DEF
	public override void Hide()
	{
		this.m_Active = false;
		FuncUI.BuildBtnAnim.SetBool("Show", false);
		FuncUI.NextWaveBtnAnim.SetBool("Show", false);
		FuncUI.SystemBtnAnim.SetBool("Show", false);
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x00026C28 File Offset: 0x00024E28
	public static void PlayFuncUIAnim(int partID, string key, bool value)
	{
		switch (partID)
		{
		case 0:
			FuncUI.BuildBtnAnim.SetBool(key, value);
			return;
		case 1:
			FuncUI.NextWaveBtnAnim.SetBool(key, value);
			return;
		case 2:
			FuncUI.SystemBtnAnim.SetBool(key, value);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00026C64 File Offset: 0x00024E64
	public void DrawBtnClick()
	{
		if (this.isDrawingAnim)
		{
			return;
		}
		if (Singleton<GameManager>.Instance.OperationState.StateName == StateName.BuildingState && Singleton<GameManager>.Instance.ConsumeMoney(GameRes.BuildCost))
		{
			GameRes.BuildCost += Singleton<StaticData>.Instance.MultipleShapeCost;
			this.isDrawingAnim = true;
			GameRes.DrawThisTurn = true;
			Singleton<GameManager>.Instance.DrawShapes();
			return;
		}
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00026CCB File Offset: 0x00024ECB
	public void NextWaveBtnClick()
	{
		if (base.IsVisible() && Singleton<GameManager>.Instance.OperationState.StateName == StateName.BuildingState)
		{
			Singleton<GameManager>.Instance.StartNewWave();
		}
	}

	// Token: 0x04000728 RID: 1832
	private static Animator BuildBtnAnim;

	// Token: 0x04000729 RID: 1833
	private static Animator SystemBtnAnim;

	// Token: 0x0400072A RID: 1834
	private static Animator NextWaveBtnAnim;

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private TextMeshProUGUI DrawBtnTxt;

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private TextMeshProUGUI SystemUpgradeCostTxt;

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private Text SystemLevelTxt;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private Text DiscountTxt;

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private InfoBtn m_DrawInfo;

	// Token: 0x04000730 RID: 1840
	private bool isDrawingAnim;
}
