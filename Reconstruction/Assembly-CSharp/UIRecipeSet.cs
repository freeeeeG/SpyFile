using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class UIRecipeSet : IUserInterface
{
	// Token: 0x06000FAB RID: 4011 RVA: 0x00029EF4 File Offset: 0x000280F4
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
		foreach (RecipeHolder recipeHolder in this.m_RecipeHolders)
		{
			recipeHolder.Initialize();
		}
		foreach (TurretAttribute turretAttribute in Singleton<StaticData>.Instance.ContentFactory.RefactorDIC.Values)
		{
			if (turretAttribute.Rare > 0)
			{
				this.m_RecipeHolders[turretAttribute.Rare - 1].AddRecipe(turretAttribute);
			}
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x00029FC4 File Offset: 0x000281C4
	public void ShowRecipes(List<TurretAttribute> atts)
	{
		foreach (RecipeHolder recipeHolder in this.m_RecipeHolders)
		{
			recipeHolder.UnselectAll();
		}
		foreach (TurretAttribute turretAttribute in atts)
		{
			this.m_RecipeHolders[turretAttribute.Rare - 1].SetSlotSelect(turretAttribute);
		}
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0002A064 File Offset: 0x00028264
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetBool("isOpen", true);
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x0002A07D File Offset: 0x0002827D
	public override void ClosePanel()
	{
		if (this.SaveSetting())
		{
			this.m_Anim.SetBool("isOpen", false);
			return;
		}
		Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("SELECTNOTENOUGH"));
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x0002A0B0 File Offset: 0x000282B0
	private bool SaveSetting()
	{
		this.attList.Clear();
		foreach (RecipeHolder recipeHolder in this.m_RecipeHolders)
		{
			List<TurretAttribute> selectRecipe = recipeHolder.GetSelectRecipe();
			if (selectRecipe.Count <= 0)
			{
				return false;
			}
			foreach (TurretAttribute item in selectRecipe)
			{
				this.attList.Add(item);
			}
		}
		this.m_BattleRecipe.SetRecipes(this.attList);
		return true;
	}

	// Token: 0x0400080E RID: 2062
	private Animator m_Anim;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private List<RecipeHolder> m_RecipeHolders;

	// Token: 0x04000810 RID: 2064
	public BattleRecipe m_BattleRecipe;

	// Token: 0x04000811 RID: 2065
	private List<TurretAttribute> attList = new List<TurretAttribute>();
}
