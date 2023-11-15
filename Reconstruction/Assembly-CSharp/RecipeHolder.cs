using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000276 RID: 630
public class RecipeHolder : MonoBehaviour
{
	// Token: 0x06000FA0 RID: 4000 RVA: 0x00029C1C File Offset: 0x00027E1C
	public void Initialize()
	{
		this.m_RecipeParent = base.transform.Find("RecipeParent");
		this.toggleGroup = base.GetComponent<ToggleGroup>();
		this.rareTxt.text = GameMultiLang.GetTraduction("TURRETLEVEL") + this.rareLevel.ToString() + GameMultiLang.GetTraduction("RECIPE");
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x00029C7C File Offset: 0x00027E7C
	public void AddRecipe(TurretAttribute att)
	{
		RecipeSlot recipeSlot = Object.Instantiate<RecipeSlot>(this.recipeSlotPrefab, this.m_RecipeParent);
		this.m_RecipeSlots.Add(recipeSlot);
		recipeSlot.Initialize(att, this.toggleGroup);
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x00029CB4 File Offset: 0x00027EB4
	public void UnselectAll()
	{
		foreach (RecipeSlot recipeSlot in this.m_RecipeSlots)
		{
			recipeSlot.OnSelect(false);
		}
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00029D08 File Offset: 0x00027F08
	public void SetSlotSelect(TurretAttribute att)
	{
		foreach (RecipeSlot recipeSlot in this.m_RecipeSlots)
		{
			if (recipeSlot.m_Att == att)
			{
				recipeSlot.OnSelect(true);
				break;
			}
		}
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00029D6C File Offset: 0x00027F6C
	public List<TurretAttribute> GetSelectRecipe()
	{
		List<TurretAttribute> list = new List<TurretAttribute>();
		foreach (RecipeSlot recipeSlot in this.m_RecipeSlots)
		{
			if (recipeSlot.IsSelected)
			{
				list.Add(recipeSlot.m_Att);
			}
		}
		return list;
	}

	// Token: 0x04000801 RID: 2049
	[SerializeField]
	private TextMeshProUGUI rareTxt;

	// Token: 0x04000802 RID: 2050
	[SerializeField]
	private int rareLevel;

	// Token: 0x04000803 RID: 2051
	[SerializeField]
	private RecipeSlot recipeSlotPrefab;

	// Token: 0x04000804 RID: 2052
	private ToggleGroup toggleGroup;

	// Token: 0x04000805 RID: 2053
	private Transform m_RecipeParent;

	// Token: 0x04000806 RID: 2054
	private List<RecipeSlot> m_RecipeSlots = new List<RecipeSlot>();
}
