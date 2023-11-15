using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000233 RID: 563
public class BluePrintGrid : ReusableObject
{
	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00025154 File Offset: 0x00023354
	// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0002515C File Offset: 0x0002335C
	public Transform Root { get; set; }

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00025165 File Offset: 0x00023365
	// (set) Token: 0x06000E78 RID: 3704 RVA: 0x0002516D File Offset: 0x0002336D
	public bool IsLock
	{
		get
		{
			return this.isLock;
		}
		set
		{
			this.isLock = value;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00025176 File Offset: 0x00023376
	// (set) Token: 0x06000E7A RID: 3706 RVA: 0x0002517E File Offset: 0x0002337E
	public RefactorStrategy Strategy { get; set; }

	// Token: 0x06000E7B RID: 3707 RVA: 0x00025188 File Offset: 0x00023388
	private void Awake()
	{
		this.Root = base.transform.Find("Root");
		this.BGImage = this.Root.Find("BluePrintGridBG").GetComponent<Image>();
		this.BGImage.material = new Material(this.BGImage.material);
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x000251E4 File Offset: 0x000233E4
	public void SetElements(BluePrintShopUI shop, ToggleGroup group, RefactorStrategy strategy)
	{
		this.Root.transform.position += Vector3.left * 2f;
		this.Root.DOLocalMoveX(2f, 0.2f, false);
		this.m_Shop = shop;
		this.m_Toggle = base.GetComponent<Toggle>();
		this.Strategy = strategy;
		this.Strategy.CheckElement();
		this.m_Toggle.group = group;
		this.compositeIcon.sprite = this.Strategy.Attribute.TurretLevels[0].TurretIcon;
		string text = "";
		foreach (int key in ((ElementSkill)strategy.TurretSkills[1]).InitElements)
		{
			text += StaticData.ElementDIC[(ElementType)key].GetElementName;
		}
		this.skillNameTxt.text = GameMultiLang.GetTraduction(text);
		this.turretBaseImg.sprite = this.turretBases[strategy.Attribute.Rare - 1];
		this.RefreshElementsSprite();
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0002532C File Offset: 0x0002352C
	public void PreviewElement(bool value, ElementType element, int quality)
	{
		ElementGrid[] array = this.elementGrids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetPreview(value, element, quality);
		}
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x0002535C File Offset: 0x0002355C
	private void RefreshElementsSprite()
	{
		for (int i = 0; i < this.elementGrids.Length; i++)
		{
			if (i < this.Strategy.Attribute.elementNumber)
			{
				this.elementGrids[i].gameObject.SetActive(true);
				this.elementGrids[i].SetElement(this.Strategy.Compositions[i]);
			}
			else
			{
				this.elementGrids[i].gameObject.SetActive(false);
			}
		}
		this.buildAble = this.Strategy.CheckBuildable();
		if (this.buildAble)
		{
			this.matTween.Kill(false);
			this.BGImage.material.SetFloat("_ShineLocation", 0f);
			this.matTween = this.BGImage.material.DOFloat(1f, "_ShineLocation", 2f).SetLoops(-1);
		}
		else
		{
			this.matTween.Kill(false);
			this.BGImage.material.SetFloat("_ShineLocation", 0f);
		}
		this.compositeIcon.color = (this.buildAble ? Color.white : this.UnobtainColor);
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00025488 File Offset: 0x00023688
	public void OnBluePrintSelect()
	{
		if (this.m_Toggle.isOn)
		{
			if (BluePrintGrid.SelectingGrid != null)
			{
				BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
			}
			BluePrintGrid.SelectingGrid = this;
			Singleton<TipsManager>.Instance.ShowTurreTips(this.Strategy, StaticData.LeftMidTipsPos, 2);
			if (this.buildAble)
			{
				this.Strategy.PreviewElements(true);
				Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.BlueprintSelect);
			}
			Singleton<Sound>.Instance.PlayUISound("Sound_Click");
			return;
		}
		if (BluePrintGrid.SelectingGrid == this)
		{
			this.OnBluePrintDeselect();
			Singleton<TipsManager>.Instance.HideTips();
			BluePrintGrid.SelectingGrid = null;
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00025527 File Offset: 0x00023727
	public void OnBluePrintDeselect()
	{
		this.m_Toggle.isOn = false;
		if (this.buildAble)
		{
			this.Strategy.PreviewElements(false);
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00025549 File Offset: 0x00023749
	public void OnLock(bool value)
	{
		this.IsLock = value;
		this.m_Shop.OnLockGrid(value);
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0002555E File Offset: 0x0002375E
	public void CheckElements()
	{
		this.Strategy.CheckElement();
		this.RefreshElementsSprite();
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00025571 File Offset: 0x00023771
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.IsLock = false;
		this.m_LockToggle.isOn = false;
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x0002558C File Offset: 0x0002378C
	public void ShowLockBtn(bool value)
	{
		this.m_LockToggle.gameObject.SetActive(value);
	}

	// Token: 0x040006E9 RID: 1769
	public static BluePrintGrid SelectingGrid;

	// Token: 0x040006EA RID: 1770
	public static BluePrintGrid RefactorGrid;

	// Token: 0x040006EB RID: 1771
	[SerializeField]
	private Color UnobtainColor;

	// Token: 0x040006EC RID: 1772
	[SerializeField]
	private TextMeshProUGUI skillNameTxt;

	// Token: 0x040006ED RID: 1773
	private bool isLock;

	// Token: 0x040006EE RID: 1774
	[SerializeField]
	private Image compositeIcon;

	// Token: 0x040006EF RID: 1775
	[SerializeField]
	private Image turretBaseImg;

	// Token: 0x040006F0 RID: 1776
	[SerializeField]
	private Sprite[] turretBases;

	// Token: 0x040006F1 RID: 1777
	[SerializeField]
	private ElementGrid[] elementGrids;

	// Token: 0x040006F2 RID: 1778
	[SerializeField]
	private Toggle m_LockToggle;

	// Token: 0x040006F3 RID: 1779
	private Toggle m_Toggle;

	// Token: 0x040006F4 RID: 1780
	private BluePrintShopUI m_Shop;

	// Token: 0x040006F6 RID: 1782
	private bool buildAble;

	// Token: 0x040006F7 RID: 1783
	private Image BGImage;

	// Token: 0x040006F8 RID: 1784
	private Tween matTween;
}
