using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C59 RID: 3161
public class TreeFilterableSideScreen : SideScreenContent
{
	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06006445 RID: 25669 RVA: 0x00250F78 File Offset: 0x0024F178
	private bool InputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06006446 RID: 25670 RVA: 0x00250F8F File Offset: 0x0024F18F
	public bool IsStorage
	{
		get
		{
			return this.storage != null;
		}
	}

	// Token: 0x06006447 RID: 25671 RVA: 0x00250F9D File Offset: 0x0024F19D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Initialize();
	}

	// Token: 0x06006448 RID: 25672 RVA: 0x00250FAC File Offset: 0x0024F1AC
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.rowPool = new UIPool<TreeFilterableSideScreenRow>(this.rowPrefab);
		this.elementPool = new UIPool<TreeFilterableSideScreenElement>(this.elementPrefab);
		MultiToggle multiToggle = this.allCheckBox;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			TreeFilterableSideScreenRow.State allCheckboxState = this.GetAllCheckboxState();
			if (allCheckboxState > TreeFilterableSideScreenRow.State.Mixed)
			{
				if (allCheckboxState == TreeFilterableSideScreenRow.State.On)
				{
					this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.Off);
					return;
				}
			}
			else
			{
				this.SetAllCheckboxState(TreeFilterableSideScreenRow.State.On);
			}
		}));
		this.onlyAllowTransportItemsCheckBox.onClick = new System.Action(this.OnlyAllowTransportItemsClicked);
		this.onlyAllowSpicedItemsCheckBox.onClick = new System.Action(this.OnlyAllowSpicedItemsClicked);
		this.initialized = true;
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x00251040 File Offset: 0x0024F240
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allCheckBox.transform.parent.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTONTOOLTIP);
		this.onlyAllowTransportItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP);
		this.onlyAllowSpicedItemsCheckBox.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ONLYALLOWSPICEDITEMSBUTTONTOOLTIP);
		this.inputField.ActivateInputField();
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.InitSearch();
	}

	// Token: 0x0600644A RID: 25674 RVA: 0x002510F4 File Offset: 0x0024F2F4
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x0600644B RID: 25675 RVA: 0x0025110A File Offset: 0x0024F30A
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600644C RID: 25676 RVA: 0x00251124 File Offset: 0x0024F324
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600644D RID: 25677 RVA: 0x0025113E File Offset: 0x0024F33E
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x0600644E RID: 25678 RVA: 0x00251144 File Offset: 0x0024F344
	private void UpdateAllCheckBoxVisualState()
	{
		switch (this.GetAllCheckboxState())
		{
		case TreeFilterableSideScreenRow.State.Off:
			this.allCheckBox.ChangeState(0);
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			this.allCheckBox.ChangeState(1);
			break;
		case TreeFilterableSideScreenRow.State.On:
			this.allCheckBox.ChangeState(2);
			break;
		}
		this.visualDirty = false;
	}

	// Token: 0x0600644F RID: 25679 RVA: 0x0025119C File Offset: 0x0024F39C
	public void Update()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (keyValuePair.Value.visualDirty)
			{
				keyValuePair.Value.UpdateCheckBoxVisualState();
				this.visualDirty = true;
			}
		}
		if (this.visualDirty)
		{
			this.UpdateAllCheckBoxVisualState();
		}
	}

	// Token: 0x06006450 RID: 25680 RVA: 0x00251218 File Offset: 0x0024F418
	private void OnlyAllowTransportItemsClicked()
	{
		this.storage.SetOnlyFetchMarkedItems(!this.storage.GetOnlyFetchMarkedItems());
	}

	// Token: 0x06006451 RID: 25681 RVA: 0x00251233 File Offset: 0x0024F433
	private void OnlyAllowSpicedItemsClicked()
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		component.SpicedFoodOnly = !component.SpicedFoodOnly;
	}

	// Token: 0x06006452 RID: 25682 RVA: 0x00251250 File Offset: 0x0024F450
	private TreeFilterableSideScreenRow.State GetAllCheckboxState()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			switch (keyValuePair.Value.GetState())
			{
			case TreeFilterableSideScreenRow.State.Off:
				flag2 = true;
				break;
			case TreeFilterableSideScreenRow.State.Mixed:
				flag3 = true;
				break;
			case TreeFilterableSideScreenRow.State.On:
				flag = true;
				break;
			}
		}
		if (flag3)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		return TreeFilterableSideScreenRow.State.Off;
	}

	// Token: 0x06006453 RID: 25683 RVA: 0x002512F0 File Offset: 0x0024F4F0
	private void SetAllCheckboxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			using (Dictionary<Tag, TreeFilterableSideScreenRow>.Enumerator enumerator = this.tagRowMap.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair = enumerator.Current;
					keyValuePair.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
				}
				goto IL_8C;
			}
			break;
		case TreeFilterableSideScreenRow.State.Mixed:
			goto IL_8C;
		case TreeFilterableSideScreenRow.State.On:
			break;
		default:
			goto IL_8C;
		}
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair2 in this.tagRowMap)
		{
			keyValuePair2.Value.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
		}
		IL_8C:
		this.visualDirty = true;
	}

	// Token: 0x06006454 RID: 25684 RVA: 0x002513AC File Offset: 0x0024F5AC
	public bool GetElementTagAcceptedState(Tag t)
	{
		return this.targetFilterable.ContainsTag(t);
	}

	// Token: 0x06006455 RID: 25685 RVA: 0x002513BC File Offset: 0x0024F5BC
	public override bool IsValidForTarget(GameObject target)
	{
		TreeFilterable component = target.GetComponent<TreeFilterable>();
		Storage component2 = target.GetComponent<Storage>();
		return component != null && target.GetComponent<FlatTagFilterable>() == null && component.showUserMenu && (component2 == null || component2.showInUI);
	}

	// Token: 0x06006456 RID: 25686 RVA: 0x0025140C File Offset: 0x0024F60C
	public override void SetTarget(GameObject target)
	{
		this.Initialize();
		this.target = target;
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		this.contentMask.GetComponent<LayoutElement>().minHeight = (float)((this.targetFilterable.uiHeight == TreeFilterable.UISideScreenHeight.Tall) ? 380 : 256);
		this.storage = this.targetFilterable.GetComponent<Storage>();
		this.storage.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.storage.Subscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		this.OnOnlyFetchMarkedItemsSettingChanged(null);
		this.OnOnlySpicedItemsSettingChanged(null);
		this.CreateCategories();
		this.titlebar.SetActive(false);
		if (this.storage.showSideScreenTitleBar)
		{
			this.titlebar.SetActive(true);
			this.titlebar.GetComponentInChildren<LocText>().SetText(this.storage.GetProperName());
		}
		if (!this.InputFieldEmpty)
		{
			this.ClearSearch();
		}
		this.ToggleSearchConfiguration(!this.InputFieldEmpty);
	}

	// Token: 0x06006457 RID: 25687 RVA: 0x00251548 File Offset: 0x0024F748
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		this.onlyAllowTransportItemsCheckBox.ChangeState(this.storage.GetOnlyFetchMarkedItems() ? 1 : 0);
		if (this.storage.allowSettingOnlyFetchMarkedItems)
		{
			this.onlyallowTransportItemsRow.SetActive(true);
			return;
		}
		this.onlyallowTransportItemsRow.SetActive(false);
	}

	// Token: 0x06006458 RID: 25688 RVA: 0x00251598 File Offset: 0x0024F798
	private void OnOnlySpicedItemsSettingChanged(object data)
	{
		FoodStorage component = this.storage.GetComponent<FoodStorage>();
		if (component != null)
		{
			this.onlyallowSpicedItemsRow.SetActive(true);
			this.onlyAllowSpicedItemsCheckBox.ChangeState(component.SpicedFoodOnly ? 1 : 0);
			return;
		}
		this.onlyallowSpicedItemsRow.SetActive(false);
	}

	// Token: 0x06006459 RID: 25689 RVA: 0x002515EA File Offset: 0x0024F7EA
	public bool IsTagAllowed(Tag tag)
	{
		return this.targetFilterable.AcceptedTags.Contains(tag);
	}

	// Token: 0x0600645A RID: 25690 RVA: 0x002515FD File Offset: 0x0024F7FD
	public void AddTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.AddTagToFilter(tag);
	}

	// Token: 0x0600645B RID: 25691 RVA: 0x0025161A File Offset: 0x0024F81A
	public void RemoveTag(Tag tag)
	{
		if (this.targetFilterable == null)
		{
			return;
		}
		this.targetFilterable.RemoveTagFromFilter(tag);
	}

	// Token: 0x0600645C RID: 25692 RVA: 0x00251638 File Offset: 0x0024F838
	private List<TreeFilterableSideScreen.TagOrderInfo> GetTagsSortedAlphabetically(ICollection<Tag> tags)
	{
		List<TreeFilterableSideScreen.TagOrderInfo> list = new List<TreeFilterableSideScreen.TagOrderInfo>();
		foreach (Tag tag in tags)
		{
			list.Add(new TreeFilterableSideScreen.TagOrderInfo
			{
				tag = tag,
				strippedName = tag.ProperNameStripLink()
			});
		}
		list.Sort((TreeFilterableSideScreen.TagOrderInfo a, TreeFilterableSideScreen.TagOrderInfo b) => a.strippedName.CompareTo(b.strippedName));
		return list;
	}

	// Token: 0x0600645D RID: 25693 RVA: 0x002516CC File Offset: 0x0024F8CC
	private TreeFilterableSideScreenRow AddRow(Tag rowTag)
	{
		if (this.tagRowMap.ContainsKey(rowTag))
		{
			return this.tagRowMap[rowTag];
		}
		TreeFilterableSideScreenRow freeElement = this.rowPool.GetFreeElement(this.rowGroup, true);
		freeElement.Parent = this;
		this.tagRowMap.Add(rowTag, freeElement);
		Dictionary<Tag, bool> dictionary = new Dictionary<Tag, bool>();
		foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(rowTag)))
		{
			dictionary.Add(tagOrderInfo.tag, this.targetFilterable.ContainsTag(tagOrderInfo.tag) || this.targetFilterable.ContainsTag(rowTag));
		}
		freeElement.SetElement(rowTag, this.targetFilterable.ContainsTag(rowTag), dictionary);
		freeElement.transform.SetAsLastSibling();
		return freeElement;
	}

	// Token: 0x0600645E RID: 25694 RVA: 0x002517BC File Offset: 0x0024F9BC
	public float GetAmountInStorage(Tag tag)
	{
		if (!this.IsStorage)
		{
			return 0f;
		}
		return this.storage.GetMassAvailable(tag);
	}

	// Token: 0x0600645F RID: 25695 RVA: 0x002517D8 File Offset: 0x0024F9D8
	private void CreateCategories()
	{
		if (this.storage.storageFilters != null && this.storage.storageFilters.Count >= 1)
		{
			bool flag = this.target.GetComponent<CreatureDeliveryPoint>() != null;
			foreach (TreeFilterableSideScreen.TagOrderInfo tagOrderInfo in this.GetTagsSortedAlphabetically(this.storage.storageFilters))
			{
				Tag tag = tagOrderInfo.tag;
				if (flag || DiscoveredResources.Instance.IsDiscovered(tag))
				{
					this.AddRow(tag);
				}
			}
			this.visualDirty = true;
			return;
		}
		global::Debug.LogError("If you're filtering, your storage filter should have the filters set on it");
	}

	// Token: 0x06006460 RID: 25696 RVA: 0x00251898 File Offset: 0x0024FA98
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.storage != null)
		{
			this.storage.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
			this.storage.Unsubscribe(1163645216, new Action<object>(this.OnOnlySpicedItemsSettingChanged));
		}
		this.rowPool.ClearAll();
		this.elementPool.ClearAll();
		this.tagRowMap.Clear();
	}

	// Token: 0x06006461 RID: 25697 RVA: 0x00251914 File Offset: 0x0024FB14
	private void RecordRowExpandedStatus()
	{
		this.rowExpandedStatusMemory.Clear();
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			this.rowExpandedStatusMemory.Add(keyValuePair.Key, keyValuePair.Value.ArrowExpanded);
		}
	}

	// Token: 0x06006462 RID: 25698 RVA: 0x0025198C File Offset: 0x0024FB8C
	private void RestoreRowExpandedStatus()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			if (this.rowExpandedStatusMemory.ContainsKey(keyValuePair.Key))
			{
				keyValuePair.Value.SetArrowToggleState(this.rowExpandedStatusMemory[keyValuePair.Key]);
			}
		}
	}

	// Token: 0x06006463 RID: 25699 RVA: 0x00251A0C File Offset: 0x0024FC0C
	private void InitSearch()
	{
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.RecordRowExpandedStatus();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
			KScreenManager.Instance.RefreshStack();
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			if (this.InputFieldEmpty)
			{
				this.RestoreRowExpandedStatus();
			}
			this.ToggleSearchConfiguration(!this.InputFieldEmpty);
			this.UpdateSearchFilter();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER;
		this.clearButton.onClick += delegate()
		{
			if (!this.InputFieldEmpty)
			{
				this.ClearSearch();
			}
		};
	}

	// Token: 0x06006464 RID: 25700 RVA: 0x00251AB0 File Offset: 0x0024FCB0
	private void ToggleSearchConfiguration(bool searching)
	{
		this.configurationRowsContainer.gameObject.SetActive(!searching);
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.ShowToggleBox(!searching);
		}
	}

	// Token: 0x06006465 RID: 25701 RVA: 0x00251B20 File Offset: 0x0024FD20
	private void ClearSearch()
	{
		this.inputField.text = "";
		this.RestoreRowExpandedStatus();
		this.ToggleSearchConfiguration(false);
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06006466 RID: 25702 RVA: 0x00251B3F File Offset: 0x0024FD3F
	public string CurrentSearchValue
	{
		get
		{
			if (this.inputField.text == null)
			{
				return "";
			}
			return this.inputField.text;
		}
	}

	// Token: 0x06006467 RID: 25703 RVA: 0x00251B60 File Offset: 0x0024FD60
	private void UpdateSearchFilter()
	{
		foreach (KeyValuePair<Tag, TreeFilterableSideScreenRow> keyValuePair in this.tagRowMap)
		{
			keyValuePair.Value.FilterAgainstSearch(keyValuePair.Key, this.CurrentSearchValue);
		}
	}

	// Token: 0x0400447A RID: 17530
	[SerializeField]
	private MultiToggle allCheckBox;

	// Token: 0x0400447B RID: 17531
	[SerializeField]
	private MultiToggle onlyAllowTransportItemsCheckBox;

	// Token: 0x0400447C RID: 17532
	[SerializeField]
	private GameObject onlyallowTransportItemsRow;

	// Token: 0x0400447D RID: 17533
	[SerializeField]
	private MultiToggle onlyAllowSpicedItemsCheckBox;

	// Token: 0x0400447E RID: 17534
	[SerializeField]
	private GameObject onlyallowSpicedItemsRow;

	// Token: 0x0400447F RID: 17535
	[SerializeField]
	private TreeFilterableSideScreenRow rowPrefab;

	// Token: 0x04004480 RID: 17536
	[SerializeField]
	private GameObject rowGroup;

	// Token: 0x04004481 RID: 17537
	[SerializeField]
	private TreeFilterableSideScreenElement elementPrefab;

	// Token: 0x04004482 RID: 17538
	[SerializeField]
	private GameObject titlebar;

	// Token: 0x04004483 RID: 17539
	[SerializeField]
	private GameObject contentMask;

	// Token: 0x04004484 RID: 17540
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04004485 RID: 17541
	[SerializeField]
	private KButton clearButton;

	// Token: 0x04004486 RID: 17542
	[SerializeField]
	private GameObject configurationRowsContainer;

	// Token: 0x04004487 RID: 17543
	private GameObject target;

	// Token: 0x04004488 RID: 17544
	private bool visualDirty;

	// Token: 0x04004489 RID: 17545
	private bool initialized;

	// Token: 0x0400448A RID: 17546
	private KImage onlyAllowTransportItemsImg;

	// Token: 0x0400448B RID: 17547
	public UIPool<TreeFilterableSideScreenElement> elementPool;

	// Token: 0x0400448C RID: 17548
	private UIPool<TreeFilterableSideScreenRow> rowPool;

	// Token: 0x0400448D RID: 17549
	private TreeFilterable targetFilterable;

	// Token: 0x0400448E RID: 17550
	private Dictionary<Tag, TreeFilterableSideScreenRow> tagRowMap = new Dictionary<Tag, TreeFilterableSideScreenRow>();

	// Token: 0x0400448F RID: 17551
	private Dictionary<Tag, bool> rowExpandedStatusMemory = new Dictionary<Tag, bool>();

	// Token: 0x04004490 RID: 17552
	private Storage storage;

	// Token: 0x02001B8E RID: 7054
	private struct TagOrderInfo
	{
		// Token: 0x04007D12 RID: 32018
		public Tag tag;

		// Token: 0x04007D13 RID: 32019
		public string strippedName;
	}
}
