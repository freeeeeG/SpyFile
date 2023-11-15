using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BD5 RID: 3029
[AddComponentMenu("KMonoBehaviour/scripts/ResourceCategoryHeader")]
public class ResourceCategoryHeader : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x06005F6B RID: 24427 RVA: 0x002324B8 File Offset: 0x002306B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.EntryContainer.SetParent(base.transform.parent);
		this.EntryContainer.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
		this.EntryContainer.localScale = Vector3.one;
		this.mButton = base.GetComponent<Button>();
		this.mButton.onClick.AddListener(delegate()
		{
			this.ToggleOpen(true);
		});
		this.SetInteractable(this.anyDiscovered);
		this.SetActiveColor(false);
	}

	// Token: 0x06005F6C RID: 24428 RVA: 0x00232544 File Offset: 0x00230744
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		this.UpdateContents();
		this.RefreshChart();
	}

	// Token: 0x06005F6D RID: 24429 RVA: 0x0023256F File Offset: 0x0023076F
	private void SetInteractable(bool state)
	{
		if (!state)
		{
			this.SetOpen(false);
			this.expandArrow.SetDisabled();
			return;
		}
		if (!this.IsOpen)
		{
			this.expandArrow.SetInactive();
			return;
		}
		this.expandArrow.SetActive();
	}

	// Token: 0x06005F6E RID: 24430 RVA: 0x002325A8 File Offset: 0x002307A8
	private void SetActiveColor(bool state)
	{
		if (state)
		{
			this.elements.QuantityText.color = this.TextColor_Interactable;
			this.elements.LabelText.color = this.TextColor_Interactable;
			this.expandArrow.ActiveColour = this.TextColor_Interactable;
			this.expandArrow.InactiveColour = this.TextColor_Interactable;
			this.expandArrow.TargetImage.color = this.TextColor_Interactable;
			return;
		}
		this.elements.LabelText.color = this.TextColor_NonInteractable;
		this.elements.QuantityText.color = this.TextColor_NonInteractable;
		this.expandArrow.ActiveColour = this.TextColor_NonInteractable;
		this.expandArrow.InactiveColour = this.TextColor_NonInteractable;
		this.expandArrow.TargetImage.color = this.TextColor_NonInteractable;
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x00232684 File Offset: 0x00230884
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.ResourceCategoryTag = t;
		this.Measure = measure;
		this.elements.LabelText.text = t.ProperName();
		if (SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
		{
			this.anyDiscovered = true;
			this.ToggleOpen(false);
		}
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x002326DC File Offset: 0x002308DC
	private void ToggleOpen(bool play_sound)
	{
		if (!this.anyDiscovered)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			}
			return;
		}
		if (!this.IsOpen)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			}
			this.SetOpen(true);
			this.elements.LabelText.fontSize = (float)this.maximizedFontSize;
			this.elements.QuantityText.fontSize = (float)this.maximizedFontSize;
			return;
		}
		if (play_sound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.SetOpen(false);
		this.elements.LabelText.fontSize = (float)this.minimizedFontSize;
		this.elements.QuantityText.fontSize = (float)this.minimizedFontSize;
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x002327A0 File Offset: 0x002309A0
	private void Hover(bool is_hovering)
	{
		this.Background.color = (is_hovering ? this.BackgroundHoverColor : new Color(0f, 0f, 0f, 0f));
		ICollection<Pickupable> collection = null;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			collection = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.ResourceCategoryTag, false);
		}
		if (collection == null)
		{
			return;
		}
		foreach (Pickupable pickupable in collection)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					component.HighlightColour = (is_hovering ? this.highlightColour : Color.black);
				}
			}
		}
	}

	// Token: 0x06005F72 RID: 24434 RVA: 0x00232880 File Offset: 0x00230A80
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x00232889 File Offset: 0x00230A89
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x06005F74 RID: 24436 RVA: 0x00232894 File Offset: 0x00230A94
	public void SetOpen(bool open)
	{
		this.IsOpen = open;
		if (open)
		{
			this.expandArrow.SetActive();
			if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
			{
				SaveGame.Instance.expandedResourceTags.Add(this.ResourceCategoryTag);
			}
		}
		else
		{
			this.expandArrow.SetInactive();
			SaveGame.Instance.expandedResourceTags.Remove(this.ResourceCategoryTag);
		}
		this.EntryContainer.gameObject.SetActive(this.IsOpen);
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x0023291C File Offset: 0x00230B1C
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		available = 0f;
		total = 0f;
		reserved = 0f;
		HashSet<Tag> hashSet = null;
		if (!DiscoveredResources.Instance.TryGetDiscoveredResourcesFromTag(this.ResourceCategoryTag, out hashSet))
		{
			return;
		}
		ListPool<Tag, ResourceCategoryHeader>.PooledList pooledList = ListPool<Tag, ResourceCategoryHeader>.Allocate();
		foreach (Tag tag in hashSet)
		{
			EdiblesManager.FoodInfo foodInfo = null;
			if (this.Measure == GameUtil.MeasureUnit.kcal)
			{
				foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
				if (foodInfo == null)
				{
					pooledList.Add(tag);
					continue;
				}
			}
			this.anyDiscovered = true;
			ResourceEntry resourceEntry = null;
			if (!this.ResourcesDiscovered.TryGetValue(tag, out resourceEntry))
			{
				resourceEntry = this.NewResourceEntry(tag, this.Measure);
				this.ResourcesDiscovered.Add(tag, resourceEntry);
			}
			float num;
			float num2;
			float num3;
			resourceEntry.GetAmounts(foodInfo, doExtras, out num, out num2, out num3);
			available += num;
			total += num2;
			reserved += num3;
		}
		foreach (Tag item in pooledList)
		{
			hashSet.Remove(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x06005F76 RID: 24438 RVA: 0x00232A68 File Offset: 0x00230C68
	public void UpdateContents()
	{
		float num;
		float num2;
		float num3;
		this.GetAmounts(false, out num, out num2, out num3);
		if (num != this.cachedAvailable || num2 != this.cachedTotal || num3 != this.cachedReserved)
		{
			if (this.quantityString == null || this.currentQuantity != num)
			{
				switch (this.Measure)
				{
				case GameUtil.MeasureUnit.mass:
					this.quantityString = GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
					break;
				case GameUtil.MeasureUnit.kcal:
					this.quantityString = GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true);
					break;
				case GameUtil.MeasureUnit.quantity:
					this.quantityString = num.ToString();
					break;
				}
				this.elements.QuantityText.text = this.quantityString;
				this.currentQuantity = num;
			}
			this.cachedAvailable = num;
			this.cachedTotal = num2;
			this.cachedReserved = num3;
		}
		foreach (KeyValuePair<Tag, ResourceEntry> keyValuePair in this.ResourcesDiscovered)
		{
			keyValuePair.Value.UpdateValue();
		}
		this.SetActiveColor(num > 0f);
		this.SetInteractable(this.anyDiscovered);
	}

	// Token: 0x06005F77 RID: 24439 RVA: 0x00232B98 File Offset: 0x00230D98
	private string OnTooltip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.elements.LabelText.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag).GetDelta(150f);
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

	// Token: 0x06005F78 RID: 24440 RVA: 0x00232C93 File Offset: 0x00230E93
	private ResourceEntry NewResourceEntry(Tag resourceTag, GameUtil.MeasureUnit measure)
	{
		ResourceEntry component = Util.KInstantiateUI(this.Prefab_ResourceEntry, this.EntryContainer.gameObject, true).GetComponent<ResourceEntry>();
		component.SetTag(resourceTag, measure);
		return component;
	}

	// Token: 0x06005F79 RID: 24441 RVA: 0x00232CB9 File Offset: 0x00230EB9
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x00232CC4 File Offset: 0x00230EC4
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x040040C1 RID: 16577
	public GameObject Prefab_ResourceEntry;

	// Token: 0x040040C2 RID: 16578
	public Transform EntryContainer;

	// Token: 0x040040C3 RID: 16579
	public Tag ResourceCategoryTag;

	// Token: 0x040040C4 RID: 16580
	public GameUtil.MeasureUnit Measure;

	// Token: 0x040040C5 RID: 16581
	public bool IsOpen;

	// Token: 0x040040C6 RID: 16582
	public ImageToggleState expandArrow;

	// Token: 0x040040C7 RID: 16583
	private Button mButton;

	// Token: 0x040040C8 RID: 16584
	public Dictionary<Tag, ResourceEntry> ResourcesDiscovered = new Dictionary<Tag, ResourceEntry>();

	// Token: 0x040040C9 RID: 16585
	public ResourceCategoryHeader.ElementReferences elements;

	// Token: 0x040040CA RID: 16586
	public Color TextColor_Interactable;

	// Token: 0x040040CB RID: 16587
	public Color TextColor_NonInteractable;

	// Token: 0x040040CC RID: 16588
	private string quantityString;

	// Token: 0x040040CD RID: 16589
	private float currentQuantity;

	// Token: 0x040040CE RID: 16590
	private bool anyDiscovered;

	// Token: 0x040040CF RID: 16591
	public const float chartHistoryLength = 3000f;

	// Token: 0x040040D0 RID: 16592
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x040040D1 RID: 16593
	[SerializeField]
	private int minimizedFontSize;

	// Token: 0x040040D2 RID: 16594
	[SerializeField]
	private int maximizedFontSize;

	// Token: 0x040040D3 RID: 16595
	[SerializeField]
	private Color highlightColour;

	// Token: 0x040040D4 RID: 16596
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x040040D5 RID: 16597
	[SerializeField]
	private Image Background;

	// Token: 0x040040D6 RID: 16598
	public GameObject sparkChart;

	// Token: 0x040040D7 RID: 16599
	private float cachedAvailable = float.MinValue;

	// Token: 0x040040D8 RID: 16600
	private float cachedTotal = float.MinValue;

	// Token: 0x040040D9 RID: 16601
	private float cachedReserved = float.MinValue;

	// Token: 0x02001B20 RID: 6944
	[Serializable]
	public struct ElementReferences
	{
		// Token: 0x04007BC6 RID: 31686
		public LocText LabelText;

		// Token: 0x04007BC7 RID: 31687
		public LocText QuantityText;
	}
}
