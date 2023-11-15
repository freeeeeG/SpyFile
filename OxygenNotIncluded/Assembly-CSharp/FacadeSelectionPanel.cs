using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AFE RID: 2814
public class FacadeSelectionPanel : KMonoBehaviour
{
	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x060056D3 RID: 22227 RVA: 0x001FBD1E File Offset: 0x001F9F1E
	public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x060056D4 RID: 22228 RVA: 0x001FBD26 File Offset: 0x001F9F26
	// (set) Token: 0x060056D5 RID: 22229 RVA: 0x001FBD2E File Offset: 0x001F9F2E
	public string SelectedFacade
	{
		get
		{
			return this._selectedFacade;
		}
		set
		{
			if (this._selectedFacade != value)
			{
				this._selectedFacade = value;
				this.RefreshToggles();
				if (this.OnFacadeSelectionChanged != null)
				{
					this.OnFacadeSelectionChanged();
				}
			}
		}
	}

	// Token: 0x060056D6 RID: 22230 RVA: 0x001FBD5E File Offset: 0x001F9F5E
	public void SetBuildingDef(string defID)
	{
		this.ClearToggles();
		this.selectedBuildingDefID = defID;
		this.SelectedFacade = "DEFAULT_FACADE";
		this.RefreshToggles();
		base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
	}

	// Token: 0x060056D7 RID: 22231 RVA: 0x001FBD9C File Offset: 0x001F9F9C
	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

	// Token: 0x060056D8 RID: 22232 RVA: 0x001FBE24 File Offset: 0x001FA024
	private void RefreshToggles()
	{
		this.AddDefaultFacadeToggle();
		foreach (string text in Assets.GetBuildingDef(this.selectedBuildingDefID).AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(text);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				this.AddNewToggle(text);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			keyValuePair.Value.multiToggle.ChangeState((this.SelectedFacade == keyValuePair.Key) ? 1 : 0);
		}
		this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		this.storeButton.gameObject.transform.SetAsLastSibling();
		LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
		component.minHeight = (float)(58 * ((this.activeFacadeToggles.Count <= 5) ? 1 : 2));
		component.preferredHeight = component.minHeight;
	}

	// Token: 0x060056D9 RID: 22233 RVA: 0x001FBF78 File Offset: 0x001FA178
	private void AddDefaultFacadeToggle()
	{
		this.AddNewToggle("DEFAULT_FACADE");
	}

	// Token: 0x060056DA RID: 22234 RVA: 0x001FBF88 File Offset: 0x001FA188
	private void AddNewToggle(string facadeID)
	{
		if (this.activeFacadeToggles.ContainsKey(facadeID))
		{
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectedFacade = newToggle.id;
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
	}

	// Token: 0x04003A81 RID: 14977
	[SerializeField]
	private GameObject togglePrefab;

	// Token: 0x04003A82 RID: 14978
	[SerializeField]
	private RectTransform toggleContainer;

	// Token: 0x04003A83 RID: 14979
	[SerializeField]
	private LayoutElement scrollRect;

	// Token: 0x04003A84 RID: 14980
	private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

	// Token: 0x04003A85 RID: 14981
	private List<GameObject> pooledFacadeToggles = new List<GameObject>();

	// Token: 0x04003A86 RID: 14982
	[SerializeField]
	private KButton storeButton;

	// Token: 0x04003A87 RID: 14983
	public System.Action OnFacadeSelectionChanged;

	// Token: 0x04003A88 RID: 14984
	private string selectedBuildingDefID;

	// Token: 0x04003A89 RID: 14985
	private string _selectedFacade;

	// Token: 0x04003A8A RID: 14986
	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	// Token: 0x02001A10 RID: 6672
	private struct FacadeToggle
	{
		// Token: 0x06009600 RID: 38400 RVA: 0x0033BA14 File Offset: 0x00339C14
		public FacadeToggle(string facadeID, string buildingPrefabID, GameObject gameObject)
		{
			this.id = facadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			if (facadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(facadeID);
				gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage").sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
				this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ApplyBoldString(buildingFacadeResource.Name) + "\n\n" + buildingFacadeResource.Description);
				return;
			}
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("FGImage").sprite = Def.GetUISprite(buildingPrefabID, "ui", false).first;
			StringEntry stringEntry;
			Strings.TryGet(string.Concat(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.",
				buildingPrefabID.ToUpperInvariant(),
				".FACADES.DEFAULT_",
				buildingPrefabID.ToUpperInvariant(),
				".NAME"
			}), out stringEntry);
			StringEntry stringEntry2;
			Strings.TryGet(string.Concat(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.",
				buildingPrefabID.ToUpperInvariant(),
				".FACADES.DEFAULT_",
				buildingPrefabID.ToUpperInvariant(),
				".DESC"
			}), out stringEntry2);
			GameObject prefab = Assets.GetPrefab(buildingPrefabID);
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ApplyBoldString((stringEntry != null) ? stringEntry.String : prefab.GetProperName()) + "\n\n" + ((stringEntry2 != null) ? stringEntry2.String : ""));
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06009601 RID: 38401 RVA: 0x0033BBB4 File Offset: 0x00339DB4
		// (set) Token: 0x06009602 RID: 38402 RVA: 0x0033BBBC File Offset: 0x00339DBC
		public string id { readonly get; set; }

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06009603 RID: 38403 RVA: 0x0033BBC5 File Offset: 0x00339DC5
		// (set) Token: 0x06009604 RID: 38404 RVA: 0x0033BBCD File Offset: 0x00339DCD
		public GameObject gameObject { readonly get; set; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06009605 RID: 38405 RVA: 0x0033BBD6 File Offset: 0x00339DD6
		// (set) Token: 0x06009606 RID: 38406 RVA: 0x0033BBDE File Offset: 0x00339DDE
		public MultiToggle multiToggle { readonly get; set; }
	}
}
