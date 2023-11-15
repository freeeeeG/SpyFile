using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000234 RID: 564
public class BluePrintShopUI : IUserInterface
{
	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x000255A7 File Offset: 0x000237A7
	// (set) Token: 0x06000E87 RID: 3719 RVA: 0x000255AF File Offset: 0x000237AF
	public int CurrentLock
	{
		get
		{
			return this.currentLock;
		}
		set
		{
			this.currentLock = value;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (set) Token: 0x06000E88 RID: 3720 RVA: 0x000255B8 File Offset: 0x000237B8
	public int NextRefreshTrun
	{
		set
		{
			this.NextRefreshTurnsTxt.text = GameMultiLang.GetTraduction("NEXTREFRESH") + ":" + value.ToString() + GameMultiLang.GetTraduction("WAVE");
		}
	}

	// Token: 0x1700052D RID: 1325
	// (set) Token: 0x06000E89 RID: 3721 RVA: 0x000255EA File Offset: 0x000237EA
	public int PerfectElementCount
	{
		set
		{
			this.PerfectElementTxt.text = value.ToString();
		}
	}

	// Token: 0x1700052E RID: 1326
	// (set) Token: 0x06000E8A RID: 3722 RVA: 0x000255FE File Offset: 0x000237FE
	public int RefreshShopCost
	{
		set
		{
			this.refreshCost_Txt.text = "<sprite=7>" + value.ToString();
		}
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x0002561C File Offset: 0x0002381C
	public override void Initialize()
	{
		this.anim = base.GetComponent<Animator>();
		this.CurrentLock = 0;
		this.RefactorTrigger = new TempWord(TempWordType.Refactor, 0);
		this.perfectInfo.SetContent(GameMultiLang.GetTraduction("PERFECTINFO"));
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x00025653 File Offset: 0x00023853
	public override void Release()
	{
		base.Release();
		BluePrintShopUI.ShopBluePrints.Clear();
		BluePrintGrid.SelectingGrid = null;
		BluePrintGrid.RefactorGrid = null;
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00025671 File Offset: 0x00023871
	public void SetShopBtnActive(bool value)
	{
		this.ShopBtnObj.SetActive(value);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x00025680 File Offset: 0x00023880
	public void LoadSaveGame()
	{
		foreach (BlueprintStruct blueprintStruct in Singleton<LevelManager>.Instance.LastGameSave.SaveBluePrints)
		{
			RefactorStrategy specificStrategyByString = ConstructHelper.GetSpecificStrategyByString(blueprintStruct.Name, blueprintStruct.ElementRequirements, blueprintStruct.QualityRequirements, 1);
			specificStrategyByString.AddElementSkill(TurretSkillFactory.GetElementSkill(blueprintStruct.ElementRequirements));
			this.AddBluePrint(specificStrategyByString);
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x00025708 File Offset: 0x00023908
	public void RefreshShop(int cost)
	{
		if (this.isRefreshing)
		{
			return;
		}
		if (!Singleton<GameManager>.Instance.ConsumeMoney(cost))
		{
			return;
		}
		if (BluePrintGrid.SelectingGrid != null)
		{
			BluePrintGrid.SelectingGrid.OnBluePrintDeselect();
			BluePrintGrid.SelectingGrid = null;
		}
		BluePrintGrid.RefactorGrid = null;
		int num = 0;
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints.ToList<BluePrintGrid>())
		{
			if (!bluePrintGrid.IsLock)
			{
				this.RemoveGrid(bluePrintGrid);
			}
			else
			{
				num++;
			}
		}
		base.StartCoroutine(this.RefreshShopCor(num));
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000257B8 File Offset: 0x000239B8
	private IEnumerator RefreshShopCor(int lockNum)
	{
		this.isRefreshing = true;
		int num;
		for (int i = 0; i < GameRes.ShopCapacity - lockNum; i = num + 1)
		{
			RefactorStrategy randomRefactorStrategy = ConstructHelper.GetRandomRefactorStrategy();
			this.AddBluePrint(randomRefactorStrategy);
			yield return new WaitForSeconds(0.02f);
			num = i;
		}
		this.isRefreshing = false;
		yield break;
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x000257D0 File Offset: 0x000239D0
	public void AddBluePrint(RefactorStrategy strategy)
	{
		BluePrintGrid bluePrintGrid = Singleton<ObjectPool>.Instance.Spawn(this.bluePrintGridPrefab) as BluePrintGrid;
		bluePrintGrid.transform.SetParent(this.shopContent);
		bluePrintGrid.transform.localScale = Vector3.one;
		bluePrintGrid.transform.localPosition = Vector3.zero;
		bluePrintGrid.SetElements(this, this.shopContent.GetComponent<ToggleGroup>(), strategy);
		bluePrintGrid.ShowLockBtn(this.CurrentLock < GameRes.LockCount);
		BluePrintShopUI.ShopBluePrints.Add(bluePrintGrid);
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x00025858 File Offset: 0x00023A58
	public void OnLockGrid(bool isLock)
	{
		int num;
		if (isLock)
		{
			num = this.CurrentLock;
			this.CurrentLock = num + 1;
			if (this.CurrentLock < GameRes.LockCount)
			{
				return;
			}
			using (List<BluePrintGrid>.Enumerator enumerator = BluePrintShopUI.ShopBluePrints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BluePrintGrid bluePrintGrid = enumerator.Current;
					if (!bluePrintGrid.IsLock)
					{
						bluePrintGrid.ShowLockBtn(false);
					}
				}
				return;
			}
		}
		num = this.CurrentLock;
		this.CurrentLock = num - 1;
		if (this.CurrentLock < GameRes.LockCount)
		{
			foreach (BluePrintGrid bluePrintGrid2 in BluePrintShopUI.ShopBluePrints)
			{
				bluePrintGrid2.ShowLockBtn(true);
			}
		}
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x00025934 File Offset: 0x00023B34
	public void ShowAllLock(int value)
	{
		if (value > this.CurrentLock)
		{
			using (List<BluePrintGrid>.Enumerator enumerator = BluePrintShopUI.ShopBluePrints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BluePrintGrid bluePrintGrid = enumerator.Current;
					bluePrintGrid.ShowLockBtn(true);
				}
				return;
			}
		}
		if (value == 0)
		{
			using (List<BluePrintGrid>.Enumerator enumerator = BluePrintShopUI.ShopBluePrints.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BluePrintGrid bluePrintGrid2 = enumerator.Current;
					bluePrintGrid2.IsLock = false;
					bluePrintGrid2.ShowLockBtn(false);
				}
				return;
			}
		}
		int num = this.CurrentLock - value;
		foreach (BluePrintGrid bluePrintGrid3 in BluePrintShopUI.ShopBluePrints)
		{
			if (bluePrintGrid3.IsLock)
			{
				if (num > 0)
				{
					bluePrintGrid3.IsLock = false;
					bluePrintGrid3.ShowLockBtn(false);
					num--;
				}
			}
			else
			{
				bluePrintGrid3.ShowLockBtn(false);
			}
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x00025A44 File Offset: 0x00023C44
	public void ShopBtnClick()
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge || (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Standard && Singleton<LevelManager>.Instance.CurrentLevel.Level == 0))
		{
			return;
		}
		Singleton<GameEvents>.Instance.TutorialTrigger(TutorialType.ShopBtnClick);
		this.Showing = !this.Showing;
		if (this.Showing)
		{
			this.Show();
			return;
		}
		this.Hide();
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00025AB6 File Offset: 0x00023CB6
	public override void Hide()
	{
		this.anim.SetBool("Showing", false);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00025AC9 File Offset: 0x00023CC9
	public override void Show()
	{
		this.anim.SetBool("Showing", true);
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00025ADC File Offset: 0x00023CDC
	public void RefreshBtnClick()
	{
		Singleton<GameManager>.Instance.RefreshShop(GameRes.RefreshShopCost);
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00025AED File Offset: 0x00023CED
	public void RefactorBluePrint(BluePrintGrid grid)
	{
		grid.Strategy.RefactorTurret();
		this.RemoveGrid(grid);
		this.CheckAllBluePrint();
		Singleton<TipsManager>.Instance.HideTips();
		BluePrintGrid.RefactorGrid = grid;
		Singleton<GameEvents>.Instance.TempWordTrigger(this.RefactorTrigger);
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x00025B27 File Offset: 0x00023D27
	public void RemoveGrid(BluePrintGrid grid)
	{
		if (BluePrintShopUI.ShopBluePrints.Contains(grid))
		{
			BluePrintShopUI.ShopBluePrints.Remove(grid);
		}
		Singleton<ObjectPool>.Instance.UnSpawn(grid);
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00025B50 File Offset: 0x00023D50
	public void CheckAllBluePrint()
	{
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints)
		{
			bluePrintGrid.CheckElements();
		}
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x00025BA0 File Offset: 0x00023DA0
	public void PreviewComposition(bool value, ElementType element, int quality)
	{
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints)
		{
			bluePrintGrid.PreviewElement(value, element, quality);
		}
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00025BF4 File Offset: 0x00023DF4
	public void RemoveUnlockedRecipes()
	{
		foreach (BluePrintGrid bluePrintGrid in BluePrintShopUI.ShopBluePrints.ToList<BluePrintGrid>())
		{
			if (!bluePrintGrid.IsLock)
			{
				this.RemoveGrid(bluePrintGrid);
			}
		}
	}

	// Token: 0x040006F9 RID: 1785
	private bool isRefreshing;

	// Token: 0x040006FA RID: 1786
	private bool Showing;

	// Token: 0x040006FB RID: 1787
	private Animator anim;

	// Token: 0x040006FC RID: 1788
	[SerializeField]
	private GameObject ShopBtnObj;

	// Token: 0x040006FD RID: 1789
	[SerializeField]
	private BluePrintGrid bluePrintGridPrefab;

	// Token: 0x040006FE RID: 1790
	[SerializeField]
	private Text NextRefreshTurnsTxt;

	// Token: 0x040006FF RID: 1791
	[SerializeField]
	private Transform shopContent;

	// Token: 0x04000700 RID: 1792
	[SerializeField]
	private Text PerfectElementTxt;

	// Token: 0x04000701 RID: 1793
	[SerializeField]
	private TextMeshProUGUI refreshCost_Txt;

	// Token: 0x04000702 RID: 1794
	[SerializeField]
	private InfoBtn perfectInfo;

	// Token: 0x04000703 RID: 1795
	public static List<BluePrintGrid> ShopBluePrints = new List<BluePrintGrid>();

	// Token: 0x04000704 RID: 1796
	private int currentLock;

	// Token: 0x04000705 RID: 1797
	private TempWord RefactorTrigger;
}
