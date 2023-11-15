using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000BD6 RID: 3030
public class ResourceCategoryScreen : KScreen
{
	// Token: 0x06005F7D RID: 24445 RVA: 0x00232D6C File Offset: 0x00230F6C
	public static void DestroyInstance()
	{
		ResourceCategoryScreen.Instance = null;
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x00232D74 File Offset: 0x00230F74
	protected override void OnActivate()
	{
		base.OnActivate();
		ResourceCategoryScreen.Instance = this;
		base.ConsumeMouseScroll = true;
		MultiToggle hiderButton = this.HiderButton;
		hiderButton.onClick = (System.Action)Delegate.Combine(hiderButton.onClick, new System.Action(this.OnHiderClick));
		this.OnHiderClick();
		this.CreateTagSetHeaders(GameTags.MaterialCategories, GameUtil.MeasureUnit.mass);
		this.CreateTagSetHeaders(GameTags.CalorieCategories, GameUtil.MeasureUnit.kcal);
		this.CreateTagSetHeaders(GameTags.UnitCategories, GameUtil.MeasureUnit.quantity);
		if (!this.DisplayedCategories.ContainsKey(GameTags.Miscellaneous))
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(GameTags.Miscellaneous, GameUtil.MeasureUnit.mass);
			this.DisplayedCategories.Add(GameTags.Miscellaneous, value);
		}
		this.DisplayedCategoryKeys = this.DisplayedCategories.Keys.ToArray<Tag>();
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x00232E2C File Offset: 0x0023102C
	private void CreateTagSetHeaders(IEnumerable<Tag> set, GameUtil.MeasureUnit measure)
	{
		foreach (Tag tag in set)
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(tag, measure);
			this.DisplayedCategories.Add(tag, value);
		}
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x00232E84 File Offset: 0x00231084
	private void OnHiderClick()
	{
		this.HiderButton.NextState();
		if (this.HiderButton.CurrentState == 0)
		{
			this.targetContentHideHeight = 0f;
			return;
		}
		this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x00232EFC File Offset: 0x002310FC
	private void Update()
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (this.HideTarget.minHeight != this.targetContentHideHeight)
		{
			float num = this.HideTarget.minHeight;
			float num2 = this.targetContentHideHeight - num;
			num2 = Mathf.Clamp(num2 * this.HideSpeedFactor * Time.unscaledDeltaTime, (num2 > 0f) ? (-num2) : num2, (num2 > 0f) ? num2 : (-num2));
			num += num2;
			this.HideTarget.minHeight = num;
		}
		for (int i = 0; i < 1; i++)
		{
			Tag tag = this.DisplayedCategoryKeys[this.categoryUpdatePacer];
			ResourceCategoryHeader resourceCategoryHeader = this.DisplayedCategories[tag];
			if (DiscoveredResources.Instance.IsDiscovered(tag) && !resourceCategoryHeader.gameObject.activeInHierarchy)
			{
				resourceCategoryHeader.gameObject.SetActive(true);
			}
			resourceCategoryHeader.UpdateContents();
			this.categoryUpdatePacer = (this.categoryUpdatePacer + 1) % this.DisplayedCategoryKeys.Length;
		}
		if (this.HiderButton.CurrentState != 0)
		{
			this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
		}
		if (MeterScreen.Instance != null && !MeterScreen.Instance.StartValuesSet)
		{
			MeterScreen.Instance.InitializeValues();
		}
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x0023306F File Offset: 0x0023126F
	private ResourceCategoryHeader NewCategoryHeader(Tag categoryTag, GameUtil.MeasureUnit measure)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_CategoryBar, this.CategoryContainer.gameObject, false);
		gameObject.name = "CategoryHeader_" + categoryTag.Name;
		ResourceCategoryHeader component = gameObject.GetComponent<ResourceCategoryHeader>();
		component.SetTag(categoryTag, measure);
		return component;
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x002330AC File Offset: 0x002312AC
	public static string QuantityTextForMeasure(float quantity, GameUtil.MeasureUnit measure)
	{
		switch (measure)
		{
		case GameUtil.MeasureUnit.mass:
			return GameUtil.GetFormattedMass(quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.MeasureUnit.kcal:
			return GameUtil.GetFormattedCalories(quantity, GameUtil.TimeSlice.None, true);
		}
		return quantity.ToString();
	}

	// Token: 0x040040DA RID: 16602
	public static ResourceCategoryScreen Instance;

	// Token: 0x040040DB RID: 16603
	public GameObject Prefab_CategoryBar;

	// Token: 0x040040DC RID: 16604
	public Transform CategoryContainer;

	// Token: 0x040040DD RID: 16605
	public MultiToggle HiderButton;

	// Token: 0x040040DE RID: 16606
	public KLayoutElement HideTarget;

	// Token: 0x040040DF RID: 16607
	private float HideSpeedFactor = 12f;

	// Token: 0x040040E0 RID: 16608
	private float maxHeightPadding = 480f;

	// Token: 0x040040E1 RID: 16609
	private float targetContentHideHeight;

	// Token: 0x040040E2 RID: 16610
	public Dictionary<Tag, ResourceCategoryHeader> DisplayedCategories = new Dictionary<Tag, ResourceCategoryHeader>();

	// Token: 0x040040E3 RID: 16611
	private Tag[] DisplayedCategoryKeys;

	// Token: 0x040040E4 RID: 16612
	private int categoryUpdatePacer;
}
