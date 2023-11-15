using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C3D RID: 3133
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x06006321 RID: 25377 RVA: 0x00249F5C File Offset: 0x0024815C
	public override string GetTitle()
	{
		if (this.targetReceptacle == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetReceptacle.GetProperName());
	}

	// Token: 0x06006322 RID: 25378 RVA: 0x00249FB8 File Offset: 0x002481B8
	public void Initialize(SingleEntityReceptacle target)
	{
		if (target == null)
		{
			global::Debug.LogError("SingleObjectReceptacle provided was null.");
			return;
		}
		this.targetReceptacle = target;
		base.gameObject.SetActive(true);
		this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
		this.entityToggles.ForEach(delegate(ReceptacleToggle rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.entityToggles.Clear();
		foreach (Tag tag in this.targetReceptacle.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			int num = prefabsWithTag.Count;
			List<IHasSortOrder> list = new List<IHasSortOrder>();
			foreach (GameObject gameObject in prefabsWithTag)
			{
				if (!this.targetReceptacle.IsValidEntity(gameObject))
				{
					num--;
				}
				else
				{
					IHasSortOrder component = gameObject.GetComponent<IHasSortOrder>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			global::Debug.Assert(list.Count == num, "Not all entities in this receptacle implement IHasSortOrder!");
			list.Sort((IHasSortOrder a, IHasSortOrder b) => a.sortOrder - b.sortOrder);
			foreach (IHasSortOrder hasSortOrder in list)
			{
				GameObject gameObject2 = (hasSortOrder as MonoBehaviour).gameObject;
				GameObject gameObject3 = Util.KInstantiateUI(this.entityToggle, this.requestObjectList, false);
				gameObject3.SetActive(true);
				ReceptacleToggle newToggle = gameObject3.GetComponent<ReceptacleToggle>();
				IReceptacleDirection component2 = gameObject2.GetComponent<IReceptacleDirection>();
				string properName = gameObject2.GetProperName();
				newToggle.title.text = properName;
				Sprite entityIcon = this.GetEntityIcon(gameObject2.PrefabID());
				if (entityIcon == null)
				{
					entityIcon = this.elementPlaceholderSpr;
				}
				newToggle.image.sprite = entityIcon;
				newToggle.toggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				newToggle.toggle.onPointerEnter += delegate()
				{
					this.CheckAmountsAndUpdate(null);
				};
				this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity
				{
					tag = gameObject2.PrefabID(),
					direction = ((component2 != null) ? component2.Direction : SingleEntityReceptacle.ReceptacleDirection.Top),
					asset = gameObject2
				});
				this.entityToggles.Add(newToggle);
			}
		}
		this.RestoreSelectionFromOccupant();
		this.selectedEntityToggle = null;
		if (this.entityToggles.Count > 0)
		{
			if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
			{
				int index = this.entityPreviousSelectionMap[this.targetReceptacle];
				this.ToggleClicked(this.entityToggles[index]);
			}
			else
			{
				this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
				this.requestSelectedEntityBtn.isInteractable = false;
				this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
				this.HideAllDescriptorPanels();
			}
		}
		this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
		this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
		this.UpdateState(null);
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06006323 RID: 25379 RVA: 0x0024A3A8 File Offset: 0x002485A8
	protected virtual void UpdateState(object data)
	{
		this.requestSelectedEntityBtn.ClearOnClick();
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.CheckReceptacleOccupied())
		{
			Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
			if (uprootable != null && uprootable.IsMarkedForUproot)
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					uprootable.ForceCancelUproot(null);
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			else
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					this.targetReceptacle.OrderRemoveOccupant();
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			Tag tag = this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID();
			this.ConfigureActiveEntity(tag);
			this.SetResultDescriptions(this.targetReceptacle.Occupant);
		}
		else if (this.targetReceptacle.GetActiveRequest != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
			GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
			if (prefab != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), prefab.GetProperName()));
				this.SetResultDescriptions(prefab);
			}
		}
		else if (this.selectedEntityToggle != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag, false);
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Active : ImageToggleState.State.DisabledActive);
			this.ToggleObjectPicker(true);
			GameObject prefab2 = Assets.GetPrefab(this.selectedDepositObjectTag);
			if (prefab2 != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), prefab2.GetProperName()));
				this.SetResultDescriptions(prefab2);
			}
		}
		else
		{
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = false;
			this.ToggleObjectPicker(true);
		}
		this.UpdateAvailableAmounts(null);
		this.UpdateListeners();
	}

	// Token: 0x06006324 RID: 25380 RVA: 0x0024A728 File Offset: 0x00248928
	private void UpdateListeners()
	{
		if (this.CheckReceptacleOccupied())
		{
			if (this.onObjectDestroyedHandle == -1)
			{
				this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, delegate(object d)
				{
					this.UpdateState(null);
				});
				return;
			}
		}
		else if (this.onObjectDestroyedHandle != -1)
		{
			this.onObjectDestroyedHandle = -1;
		}
	}

	// Token: 0x06006325 RID: 25381 RVA: 0x0024A784 File Offset: 0x00248984
	private void OnOccupantValidChanged(object obj)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (!this.CheckReceptacleOccupied() && this.targetReceptacle.GetActiveRequest != null)
		{
			bool flag = false;
			ReceptacleSideScreen.SelectableEntity entity;
			if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
			{
				flag = this.CanDepositEntity(entity);
			}
			if (!flag)
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateState(null);
				this.UpdateAvailableAmounts(null);
			}
		}
	}

	// Token: 0x06006326 RID: 25382 RVA: 0x0024A7F7 File Offset: 0x002489F7
	private bool CanDepositEntity(ReceptacleSideScreen.SelectableEntity entity)
	{
		return this.ValidRotationForDeposit(entity.direction) && (!this.RequiresAvailableAmountToDeposit() || this.GetAvailableAmount(entity.tag) > 0f) && this.AdditionalCanDepositTest();
	}

	// Token: 0x06006327 RID: 25383 RVA: 0x0024A82A File Offset: 0x00248A2A
	protected virtual bool AdditionalCanDepositTest()
	{
		return true;
	}

	// Token: 0x06006328 RID: 25384 RVA: 0x0024A82D File Offset: 0x00248A2D
	protected virtual bool RequiresAvailableAmountToDeposit()
	{
		return true;
	}

	// Token: 0x06006329 RID: 25385 RVA: 0x0024A830 File Offset: 0x00248A30
	private void ClearSelection()
	{
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			keyValuePair.Key.toggle.Deselect();
		}
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x0024A890 File Offset: 0x00248A90
	private void ToggleObjectPicker(bool Show)
	{
		this.requestObjectListContainer.SetActive(Show);
		if (this.scrollBarContainer != null)
		{
			this.scrollBarContainer.SetActive(Show);
		}
		this.requestObjectList.SetActive(Show);
		this.activeEntityContainer.SetActive(!Show);
	}

	// Token: 0x0600632B RID: 25387 RVA: 0x0024A8E0 File Offset: 0x00248AE0
	private void ConfigureActiveEntity(Tag tag)
	{
		string properName = Assets.GetPrefab(tag).GetProperName();
		this.activeEntityContainer.GetComponentInChildrenOnly<LocText>().text = properName;
		this.activeEntityContainer.transform.GetChild(0).gameObject.GetComponentInChildrenOnly<Image>().sprite = this.GetEntityIcon(tag);
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x0024A931 File Offset: 0x00248B31
	protected virtual Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x0024A94C File Offset: 0x00248B4C
	public override bool IsValidForTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		return component != null && component.enabled && target.GetComponent<PlantablePlot>() == null && target.GetComponent<EggIncubator>() == null && target.GetComponent<SpecialCargoBayClusterReceptacle>() == null;
	}

	// Token: 0x0600632E RID: 25390 RVA: 0x0024A99C File Offset: 0x00248B9C
	public override void SetTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a SingleObjectReceptacle!");
			return;
		}
		this.Initialize(component);
		this.UpdateState(null);
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x0024A9D2 File Offset: 0x00248BD2
	protected virtual void RestoreSelectionFromOccupant()
	{
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x0024A9D4 File Offset: 0x00248BD4
	public override void ClearTarget()
	{
		if (this.targetReceptacle != null)
		{
			if (this.CheckReceptacleOccupied())
			{
				this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
				this.onObjectDestroyedHandle = -1;
			}
			this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
			this.onStorageChangedHandle = -1;
			this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
			this.onOccupantValidChangedHandle = -1;
			if (this.targetReceptacle.GetActiveRequest == null)
			{
				this.targetReceptacle.SetPreview(Tag.Invalid, false);
			}
			SimAndRenderScheduler.instance.Remove(this);
			this.targetReceptacle = null;
		}
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x0024AA7C File Offset: 0x00248C7C
	protected void SetImageToggleState(KToggle toggle, ImageToggleState.State state)
	{
		switch (state)
		{
		case ImageToggleState.State.Disabled:
			toggle.GetComponent<ImageToggleState>().SetDisabled();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		case ImageToggleState.State.Inactive:
			toggle.GetComponent<ImageToggleState>().SetInactive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.Active:
			toggle.GetComponent<ImageToggleState>().SetActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.DisabledActive:
			toggle.GetComponent<ImageToggleState>().SetDisabledActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x06006332 RID: 25394 RVA: 0x0024AB27 File Offset: 0x00248D27
	public void Render1000ms(float dt)
	{
		this.CheckAmountsAndUpdate(null);
	}

	// Token: 0x06006333 RID: 25395 RVA: 0x0024AB30 File Offset: 0x00248D30
	private void CheckAmountsAndUpdate(object data)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.UpdateAvailableAmounts(null))
		{
			this.UpdateState(null);
		}
	}

	// Token: 0x06006334 RID: 25396 RVA: 0x0024AB54 File Offset: 0x00248D54
	private bool UpdateAvailableAmounts(object data)
	{
		bool result = false;
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(keyValuePair.Value.tag))
			{
				keyValuePair.Key.gameObject.SetActive(false);
			}
			else if (!keyValuePair.Key.gameObject.activeSelf)
			{
				keyValuePair.Key.gameObject.SetActive(true);
			}
			float availableAmount = this.GetAvailableAmount(keyValuePair.Value.tag);
			if (keyValuePair.Value.lastAmount != availableAmount)
			{
				result = true;
				keyValuePair.Value.lastAmount = availableAmount;
				keyValuePair.Key.amount.text = availableAmount.ToString();
			}
			if (!this.ValidRotationForDeposit(keyValuePair.Value.direction) || availableAmount <= 0f)
			{
				if (this.selectedEntityToggle != keyValuePair.Key)
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Disabled);
				}
				else
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.DisabledActive);
				}
			}
			else if (this.selectedEntityToggle != keyValuePair.Key)
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Inactive);
			}
			else
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Active);
			}
		}
		return result;
	}

	// Token: 0x06006335 RID: 25397 RVA: 0x0024ACF4 File Offset: 0x00248EF4
	private float GetAvailableAmount(Tag tag)
	{
		if (this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
		{
			IEnumerable<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
			float num = 0f;
			foreach (Pickupable pickupable in pickupables)
			{
				num += (float)Mathf.CeilToInt(pickupable.TotalAmount);
			}
			return num;
		}
		return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
	}

	// Token: 0x06006336 RID: 25398 RVA: 0x0024AD84 File Offset: 0x00248F84
	private bool ValidRotationForDeposit(SingleEntityReceptacle.ReceptacleDirection depositDir)
	{
		return this.targetReceptacle.rotatable == null || depositDir == this.targetReceptacle.Direction;
	}

	// Token: 0x06006337 RID: 25399 RVA: 0x0024ADAC File Offset: 0x00248FAC
	protected virtual void ToggleClicked(ReceptacleToggle toggle)
	{
		if (!this.depositObjectMap.ContainsKey(toggle))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedEntityToggle != null)
		{
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		this.selectedEntityToggle = toggle;
		this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
		this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
		MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
		this.selectedDepositObjectAdditionalTag = (component ? component.SubSpeciesID : Tag.Invalid);
		this.UpdateAvailableAmounts(null);
		this.UpdateState(null);
	}

	// Token: 0x06006338 RID: 25400 RVA: 0x0024AE98 File Offset: 0x00249098
	private void CreateOrder(bool isInfinite)
	{
		this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
	}

	// Token: 0x06006339 RID: 25401 RVA: 0x0024AEB1 File Offset: 0x002490B1
	protected bool CheckReceptacleOccupied()
	{
		return this.targetReceptacle != null && this.targetReceptacle.Occupant != null;
	}

	// Token: 0x0600633A RID: 25402 RVA: 0x0024AED8 File Offset: 0x002490D8
	protected virtual void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text = component.description;
		}
		else
		{
			KPrefabID component2 = go.GetComponent<KPrefabID>();
			if (component2 != null)
			{
				Element element = ElementLoader.GetElement(component2.PrefabID());
				if (element != null)
				{
					text = element.Description();
				}
			}
			else
			{
				text = go.GetProperName();
			}
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600633B RID: 25403 RVA: 0x0024AF40 File Offset: 0x00249140
	protected virtual void HideAllDescriptorPanels()
	{
		for (int i = 0; i < this.descriptorPanels.Count; i++)
		{
			this.descriptorPanels[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x04004395 RID: 17301
	protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;

	// Token: 0x04004396 RID: 17302
	[SerializeField]
	protected KButton requestSelectedEntityBtn;

	// Token: 0x04004397 RID: 17303
	[SerializeField]
	private string requestStringDeposit;

	// Token: 0x04004398 RID: 17304
	[SerializeField]
	private string requestStringCancelDeposit;

	// Token: 0x04004399 RID: 17305
	[SerializeField]
	private string requestStringRemove;

	// Token: 0x0400439A RID: 17306
	[SerializeField]
	private string requestStringCancelRemove;

	// Token: 0x0400439B RID: 17307
	public GameObject activeEntityContainer;

	// Token: 0x0400439C RID: 17308
	public GameObject nothingDiscoveredContainer;

	// Token: 0x0400439D RID: 17309
	[SerializeField]
	protected LocText descriptionLabel;

	// Token: 0x0400439E RID: 17310
	protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();

	// Token: 0x0400439F RID: 17311
	[SerializeField]
	private string subtitleStringSelect;

	// Token: 0x040043A0 RID: 17312
	[SerializeField]
	private string subtitleStringSelectDescription;

	// Token: 0x040043A1 RID: 17313
	[SerializeField]
	private string subtitleStringAwaitingSelection;

	// Token: 0x040043A2 RID: 17314
	[SerializeField]
	private string subtitleStringAwaitingDelivery;

	// Token: 0x040043A3 RID: 17315
	[SerializeField]
	private string subtitleStringEntityDeposited;

	// Token: 0x040043A4 RID: 17316
	[SerializeField]
	private string subtitleStringAwaitingRemoval;

	// Token: 0x040043A5 RID: 17317
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x040043A6 RID: 17318
	[SerializeField]
	private List<DescriptorPanel> descriptorPanels;

	// Token: 0x040043A7 RID: 17319
	public Material defaultMaterial;

	// Token: 0x040043A8 RID: 17320
	public Material desaturatedMaterial;

	// Token: 0x040043A9 RID: 17321
	[SerializeField]
	private GameObject requestObjectList;

	// Token: 0x040043AA RID: 17322
	[SerializeField]
	private GameObject requestObjectListContainer;

	// Token: 0x040043AB RID: 17323
	[SerializeField]
	private GameObject scrollBarContainer;

	// Token: 0x040043AC RID: 17324
	[SerializeField]
	private GameObject entityToggle;

	// Token: 0x040043AD RID: 17325
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x040043AE RID: 17326
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x040043AF RID: 17327
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x040043B0 RID: 17328
	[SerializeField]
	private bool hideUndiscoveredEntities;

	// Token: 0x040043B1 RID: 17329
	protected ReceptacleToggle selectedEntityToggle;

	// Token: 0x040043B2 RID: 17330
	protected SingleEntityReceptacle targetReceptacle;

	// Token: 0x040043B3 RID: 17331
	protected Tag selectedDepositObjectTag;

	// Token: 0x040043B4 RID: 17332
	protected Tag selectedDepositObjectAdditionalTag;

	// Token: 0x040043B5 RID: 17333
	protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;

	// Token: 0x040043B6 RID: 17334
	protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();

	// Token: 0x040043B7 RID: 17335
	private int onObjectDestroyedHandle = -1;

	// Token: 0x040043B8 RID: 17336
	private int onOccupantValidChangedHandle = -1;

	// Token: 0x040043B9 RID: 17337
	private int onStorageChangedHandle = -1;

	// Token: 0x02001B7E RID: 7038
	protected class SelectableEntity
	{
		// Token: 0x04007CE7 RID: 31975
		public Tag tag;

		// Token: 0x04007CE8 RID: 31976
		public SingleEntityReceptacle.ReceptacleDirection direction;

		// Token: 0x04007CE9 RID: 31977
		public GameObject asset;

		// Token: 0x04007CEA RID: 31978
		public float lastAmount = -1f;
	}
}
