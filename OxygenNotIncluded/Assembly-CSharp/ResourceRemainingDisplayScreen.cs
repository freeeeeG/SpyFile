using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BD8 RID: 3032
public class ResourceRemainingDisplayScreen : KScreen
{
	// Token: 0x06005F97 RID: 24471 RVA: 0x00233796 File Offset: 0x00231996
	public static void DestroyInstance()
	{
		ResourceRemainingDisplayScreen.instance = null;
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x0023379E File Offset: 0x0023199E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Activate();
		ResourceRemainingDisplayScreen.instance = this;
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x002337BE File Offset: 0x002319BE
	public void ActivateDisplay(GameObject target)
	{
		this.numberOfPendingConstructions = 0;
		this.dispayPrefab.SetActive(true);
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x002337D3 File Offset: 0x002319D3
	public void DeactivateDisplay()
	{
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x002337E4 File Offset: 0x002319E4
	public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
	{
		this.selected_elements.Clear();
		foreach (Tag item in _selected_elements)
		{
			this.selected_elements.Add(item);
		}
		this.currentRecipe = recipe;
		global::Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, string.Format("{0} Mismatch number of selected elements {1} and recipe requirements {2}", recipe.Name, this.selected_elements.Count, recipe.Ingredients.Count));
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x00233890 File Offset: 0x00231A90
	public void SetNumberOfPendingConstructions(int number)
	{
		this.numberOfPendingConstructions = number;
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x0023389C File Offset: 0x00231A9C
	public void Update()
	{
		if (!this.dispayPrefab.activeSelf)
		{
			return;
		}
		if (base.canvas != null)
		{
			if (this.rect == null)
			{
				this.rect = base.GetComponent<RectTransform>();
			}
			this.rect.anchoredPosition = base.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		}
		if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
		{
			this.label.text = "";
			return;
		}
		this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x0023392C File Offset: 0x00231B2C
	public string GetString()
	{
		string text = "";
		if (this.selected_elements != null && this.currentRecipe != null)
		{
			for (int i = 0; i < this.currentRecipe.Ingredients.Count; i++)
			{
				Tag tag = this.selected_elements[i];
				float num = this.currentRecipe.Ingredients[i].amount * (float)this.numberOfPendingConstructions;
				float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, true);
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				text = string.Concat(new string[]
				{
					text,
					tag.ProperName(),
					": ",
					GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
					" / ",
					GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
				if (i < this.selected_elements.Count - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	// Token: 0x040040F8 RID: 16632
	public static ResourceRemainingDisplayScreen instance;

	// Token: 0x040040F9 RID: 16633
	public GameObject dispayPrefab;

	// Token: 0x040040FA RID: 16634
	public LocText label;

	// Token: 0x040040FB RID: 16635
	private Recipe currentRecipe;

	// Token: 0x040040FC RID: 16636
	private List<Tag> selected_elements = new List<Tag>();

	// Token: 0x040040FD RID: 16637
	private int numberOfPendingConstructions;

	// Token: 0x040040FE RID: 16638
	private int displayedConstructionCostMultiplier;

	// Token: 0x040040FF RID: 16639
	private RectTransform rect;
}
