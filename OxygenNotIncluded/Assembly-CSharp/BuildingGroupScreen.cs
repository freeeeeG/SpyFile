using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000AB1 RID: 2737
public class BuildingGroupScreen : KScreen
{
	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060053AA RID: 21418 RVA: 0x001E27A1 File Offset: 0x001E09A1
	public static bool SearchIsEmpty
	{
		get
		{
			return BuildingGroupScreen.Instance == null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060053AB RID: 21419 RVA: 0x001E27C6 File Offset: 0x001E09C6
	public static bool IsEditing
	{
		get
		{
			return !(BuildingGroupScreen.Instance == null) && BuildingGroupScreen.Instance.isEditing;
		}
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x001E27E1 File Offset: 0x001E09E1
	protected override void OnPrefabInit()
	{
		BuildingGroupScreen.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060053AD RID: 21421 RVA: 0x001E27F8 File Offset: 0x001E09F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.ConfigurePlanScreenForSearch();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			PlanScreen.Instance.RefreshCategoryPanelTitle();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
		this.clearButton.onClick += this.ClearSearch;
	}

	// Token: 0x060053AE RID: 21422 RVA: 0x001E28B3 File Offset: 0x001E0AB3
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x001E28C2 File Offset: 0x001E0AC2
	public void ClearSearch()
	{
		this.inputField.text = "";
	}

	// Token: 0x060053B0 RID: 21424 RVA: 0x001E28D4 File Offset: 0x001E0AD4
	private void ConfigurePlanScreenForSearch()
	{
		PlanScreen.Instance.SoftCloseRecipe();
		PlanScreen.Instance.ClearSelection();
		PlanScreen.Instance.ForceRefreshAllBuildingToggles();
		PlanScreen.Instance.ConfigurePanelSize(null);
	}

	// Token: 0x040037EC RID: 14316
	public static BuildingGroupScreen Instance;

	// Token: 0x040037ED RID: 14317
	public KInputTextField inputField;

	// Token: 0x040037EE RID: 14318
	[SerializeField]
	public KButton clearButton;
}
