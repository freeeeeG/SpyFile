using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027A RID: 634
public class BattleRecipe : MonoBehaviour
{
	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0002A25B File Offset: 0x0002845B
	// (set) Token: 0x06000FBF RID: 4031 RVA: 0x0002A263 File Offset: 0x00028463
	public List<TurretAttribute> CurrentRecipes { get; set; }

	// Token: 0x06000FC0 RID: 4032 RVA: 0x0002A26C File Offset: 0x0002846C
	public void Initialize()
	{
		this.m_ToggleGroup = base.GetComponent<ToggleGroup>();
		this.SetRecipes(Singleton<StaticData>.Instance.ContentFactory.BattleRecipes);
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x0002A290 File Offset: 0x00028490
	public void SetRecipes(List<TurretAttribute> recipes)
	{
		foreach (TurretItemSlot turretItemSlot in this.currentSlots)
		{
			Object.Destroy(turretItemSlot.gameObject);
		}
		this.currentSlots.Clear();
		this.CurrentRecipes = recipes;
		for (int i = 0; i < recipes.Count; i++)
		{
			TurretItemSlot turretItemSlot2 = Object.Instantiate<TurretItemSlot>(this.turretSlotPrefab, this.slotParent);
			turretItemSlot2.SetContent(recipes[i], this.m_ToggleGroup);
			this.currentSlots.Add(turretItemSlot2);
		}
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x0002A33C File Offset: 0x0002853C
	public void UpdateRecipes()
	{
		Singleton<StaticData>.Instance.ContentFactory.BattleRecipes = this.CurrentRecipes;
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x0002A353 File Offset: 0x00028553
	public void OpenSetPanel()
	{
		this.m_UIRecipeSet.Show();
		this.m_UIRecipeSet.m_BattleRecipe = this;
		this.m_UIRecipeSet.ShowRecipes(this.CurrentRecipes);
	}

	// Token: 0x04000814 RID: 2068
	[SerializeField]
	private UIRecipeSet m_UIRecipeSet;

	// Token: 0x04000815 RID: 2069
	[SerializeField]
	private TurretItemSlot turretSlotPrefab;

	// Token: 0x04000816 RID: 2070
	[SerializeField]
	private Transform slotParent;

	// Token: 0x04000817 RID: 2071
	private List<TurretItemSlot> currentSlots = new List<TurretItemSlot>();

	// Token: 0x04000818 RID: 2072
	private ToggleGroup m_ToggleGroup;
}
