using System;
using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BD7 RID: 3031
[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x06005F85 RID: 24453 RVA: 0x0023310C File Offset: 0x0023130C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.QuantityLabel.color = this.AvailableColor;
		this.NameLabel.color = this.AvailableColor;
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06005F86 RID: 24454 RVA: 0x0023315D File Offset: 0x0023135D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
		this.RefreshChart();
	}

	// Token: 0x06005F87 RID: 24455 RVA: 0x00233184 File Offset: 0x00231384
	private void OnClick()
	{
		this.lastClickTime = Time.unscaledTime;
		if (this.cachedPickupables == null)
		{
			this.cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(this.Resource);
			base.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
		}
		if (this.cachedPickupables == null)
		{
			return;
		}
		Pickupable pickupable = null;
		for (int i = 0; i < this.cachedPickupables.Count; i++)
		{
			this.selectionIdx++;
			int index = this.selectionIdx % this.cachedPickupables.Count;
			pickupable = this.cachedPickupables[index];
			if (pickupable != null && !pickupable.HasTag(GameTags.StoredPrivate))
			{
				break;
			}
		}
		if (pickupable != null)
		{
			Transform transform = pickupable.transform;
			if (pickupable.storage != null)
			{
				transform = pickupable.storage.transform;
			}
			SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
			for (int j = 0; j < this.cachedPickupables.Count; j++)
			{
				Pickupable pickupable2 = this.cachedPickupables[j];
				if (pickupable2 != null)
				{
					KAnimControllerBase component = pickupable2.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						component.HighlightColour = this.HighlightColor;
					}
				}
			}
		}
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x002332DB File Offset: 0x002314DB
	private IEnumerator ClearCachedPickupablesAfterThreshold()
	{
		while (this.cachedPickupables != null && this.lastClickTime != 0f && Time.unscaledTime - this.lastClickTime < 10f)
		{
			yield return SequenceUtil.WaitForSeconds(1f);
		}
		this.cachedPickupables = null;
		yield break;
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x002332EC File Offset: 0x002314EC
	public void GetAmounts(EdiblesManager.FoodInfo food_info, bool doExtras, out float available, out float total, out float reserved)
	{
		available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.Resource, false);
		total = (doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(this.Resource, false) : 0f);
		reserved = (doExtras ? MaterialNeeds.GetAmount(this.Resource, ClusterManager.Instance.activeWorldId, false) : 0f);
		if (food_info != null)
		{
			available *= food_info.CaloriesPerUnit;
			total *= food_info.CaloriesPerUnit;
			reserved *= food_info.CaloriesPerUnit;
		}
	}

	// Token: 0x06005F8A RID: 24458 RVA: 0x0023338C File Offset: 0x0023158C
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		EdiblesManager.FoodInfo food_info = (this.Measure == GameUtil.MeasureUnit.kcal) ? EdiblesManager.GetFoodInfo(this.Resource.Name) : null;
		this.GetAmounts(food_info, doExtras, out available, out total, out reserved);
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x002333C4 File Offset: 0x002315C4
	public void UpdateValue()
	{
		this.SetName(this.Resource.ProperName());
		bool allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
		float num;
		float num2;
		float num3;
		this.GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
		if (this.currentQuantity != num)
		{
			this.currentQuantity = num;
			this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, this.Measure);
		}
		Color color = this.AvailableColor;
		if (num3 > num2)
		{
			color = this.OverdrawnColor;
		}
		else if (num == 0f)
		{
			color = this.UnavailableColor;
		}
		if (this.QuantityLabel.color != color)
		{
			this.QuantityLabel.color = color;
		}
		if (this.NameLabel.color != color)
		{
			this.NameLabel.color = color;
		}
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x0023348C File Offset: 0x0023168C
	private string OnToolTip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.NameLabel.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource).GetDelta(150f);
		if (delta != 0f)
		{
			text = text + "\n\n" + string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP, (delta > 0f) ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR, GameUtil.GetFormattedMass(Mathf.Abs(delta), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;
		}
		return text;
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x00233582 File Offset: 0x00231782
	public void SetName(string name)
	{
		this.NameLabel.text = name;
	}

	// Token: 0x06005F8E RID: 24462 RVA: 0x00233590 File Offset: 0x00231790
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.Resource = t;
		this.Measure = measure;
		this.cachedPickupables = null;
	}

	// Token: 0x06005F8F RID: 24463 RVA: 0x002335A8 File Offset: 0x002317A8
	private void Hover(bool is_hovering)
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (is_hovering)
		{
			this.Background.color = this.BackgroundHoverColor;
		}
		else
		{
			this.Background.color = new Color(0f, 0f, 0f, 0f);
		}
		ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.Resource, false);
		if (pickupables == null)
		{
			return;
		}
		foreach (Pickupable pickupable in pickupables)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					if (is_hovering)
					{
						component.HighlightColour = this.HighlightColor;
					}
					else
					{
						component.HighlightColour = Color.black;
					}
				}
			}
		}
	}

	// Token: 0x06005F90 RID: 24464 RVA: 0x0023369C File Offset: 0x0023189C
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x002336A5 File Offset: 0x002318A5
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x002336B0 File Offset: 0x002318B0
	public void SetSprite(Tag t)
	{
		Element element = ElementLoader.FindElementByName(this.Resource.Name);
		if (element != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, "");
			if (uispriteFromMultiObjectAnim != null)
			{
				this.image.sprite = uispriteFromMultiObjectAnim;
			}
		}
	}

	// Token: 0x06005F93 RID: 24467 RVA: 0x00233702 File Offset: 0x00231902
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x06005F94 RID: 24468 RVA: 0x00233710 File Offset: 0x00231910
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x00233718 File Offset: 0x00231918
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x040040E5 RID: 16613
	public Tag Resource;

	// Token: 0x040040E6 RID: 16614
	public GameUtil.MeasureUnit Measure;

	// Token: 0x040040E7 RID: 16615
	public LocText NameLabel;

	// Token: 0x040040E8 RID: 16616
	public LocText QuantityLabel;

	// Token: 0x040040E9 RID: 16617
	public Image image;

	// Token: 0x040040EA RID: 16618
	[SerializeField]
	private Color AvailableColor;

	// Token: 0x040040EB RID: 16619
	[SerializeField]
	private Color UnavailableColor;

	// Token: 0x040040EC RID: 16620
	[SerializeField]
	private Color OverdrawnColor;

	// Token: 0x040040ED RID: 16621
	[SerializeField]
	private Color HighlightColor;

	// Token: 0x040040EE RID: 16622
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x040040EF RID: 16623
	[SerializeField]
	private Image Background;

	// Token: 0x040040F0 RID: 16624
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x040040F1 RID: 16625
	[MyCmpReq]
	private Button button;

	// Token: 0x040040F2 RID: 16626
	public GameObject sparkChart;

	// Token: 0x040040F3 RID: 16627
	private const float CLICK_RESET_TIME_THRESHOLD = 10f;

	// Token: 0x040040F4 RID: 16628
	private int selectionIdx;

	// Token: 0x040040F5 RID: 16629
	private float lastClickTime;

	// Token: 0x040040F6 RID: 16630
	private List<Pickupable> cachedPickupables;

	// Token: 0x040040F7 RID: 16631
	private float currentQuantity = float.MinValue;
}
