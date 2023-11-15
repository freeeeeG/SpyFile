using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using ProcGen;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5F RID: 3167
public class SimpleInfoScreen : TargetScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x060064AA RID: 25770 RVA: 0x002529F0 File Offset: 0x00250BF0
	// (set) Token: 0x060064AB RID: 25771 RVA: 0x002529F8 File Offset: 0x00250BF8
	public CollapsibleDetailContentPanel StoragePanel { get; private set; }

	// Token: 0x060064AC RID: 25772 RVA: 0x00252A01 File Offset: 0x00250C01
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x060064AD RID: 25773 RVA: 0x00252A04 File Offset: 0x00250C04
	protected override void OnPrefabInit()
	{
		this.onStorageChangeDelegate = new Action<object>(this.OnStorageChange);
		base.OnPrefabInit();
		this.processConditionContainer = this.CreateCollapsableSection(UI.DETAILTABS.PROCESS_CONDITIONS.NAME);
		this.statusItemPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_STATUS);
		this.statusItemPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
		this.statusItemPanel.scalerMask.hoverLock = true;
		this.statusItemsFolder = this.statusItemPanel.Content.gameObject;
		this.spaceSimpleInfoPOIPanel = new SpacePOISimpleInfoPanel(this);
		this.spacePOIPanel = this.CreateCollapsableSection(null);
		this.rocketSimpleInfoPanel = new RocketSimpleInfoPanel(this);
		this.rocketStatusContainer = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ROCKET);
		this.vitalsPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION);
		this.vitalsContainer = global::Util.KInstantiateUI(this.VitalsPanelTemplate, this.vitalsPanel.Content.gameObject, false).GetComponent<MinionVitalsPanel>();
		this.fertilityPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_FERTILITY);
		this.infoPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_DESCRIPTION);
		this.requirementsPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		this.requirementContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.requirementsPanel.Content.gameObject, false);
		this.effectsPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_EFFECTS);
		this.effectsContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.effectsPanel.Content.gameObject, false);
		this.worldMeteorShowersPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_METEORSHOWERS);
		this.worldElementsPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ELEMENTS);
		this.worldGeysersPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_GEYSERS);
		this.worldTraitsPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_WORLDTRAITS);
		this.worldBiomesPanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_BIOMES);
		this.worldLifePanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_LIFE);
		this.StoragePanel = this.CreateCollapsableSection(null);
		this.stressPanel = this.CreateCollapsableSection(null);
		this.stressDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.stressPanel.Content.gameObject);
		this.movePanel = this.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_MOVABLE);
		base.Subscribe<SimpleInfoScreen>(-1514841199, SimpleInfoScreen.OnRefreshDataDelegate);
	}

	// Token: 0x060064AE RID: 25774 RVA: 0x00252C9C File Offset: 0x00250E9C
	private CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = global::Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x00252CD0 File Offset: 0x00250ED0
	public override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		base.Subscribe(target, -1697596308, this.onStorageChangeDelegate);
		base.Subscribe(target, -1197125120, this.onStorageChangeDelegate);
		this.RefreshStorage();
		base.Subscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
		this.RefreshBreedingChance();
		this.vitalsPanel.SetTitle((target.GetComponent<WiltCondition>() == null) ? UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION : UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Combine(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (StatusItemGroup.Entry entry in statusItemGroup)
				{
					if (entry.category != null && entry.category.Id == "Main")
					{
						this.DoAddStatusItem(entry, entry.category, false);
					}
				}
				foreach (StatusItemGroup.Entry entry2 in statusItemGroup)
				{
					if (entry2.category == null || entry2.category.Id != "Main")
					{
						this.DoAddStatusItem(entry2, entry2.category, false);
					}
				}
			}
		}
		this.statusItemPanel.gameObject.SetActive(true);
		this.statusItemPanel.scalerMask.UpdateSize();
		this.Refresh(true);
		this.RefreshWorld();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
	}

	// Token: 0x060064B0 RID: 25776 RVA: 0x00252EC8 File Offset: 0x002510C8
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		if (target != null)
		{
			base.Unsubscribe(target, -1697596308, this.onStorageChangeDelegate);
			base.Unsubscribe(target, -1197125120, this.onStorageChangeDelegate);
			base.Unsubscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
		}
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Remove(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry in this.statusItems)
				{
					statusItemEntry.Destroy(true);
				}
				this.statusItems.Clear();
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems)
				{
					statusItemEntry2.onDestroy = null;
					statusItemEntry2.Destroy(true);
				}
				this.oldStatusItems.Clear();
			}
		}
	}

	// Token: 0x060064B1 RID: 25777 RVA: 0x00253028 File Offset: 0x00251228
	private void OnStorageChange(object data)
	{
		this.RefreshStorage();
	}

	// Token: 0x060064B2 RID: 25778 RVA: 0x00253030 File Offset: 0x00251230
	private void OnBreedingChanceChanged(object data)
	{
		this.RefreshBreedingChance();
	}

	// Token: 0x060064B3 RID: 25779 RVA: 0x00253038 File Offset: 0x00251238
	private void OnAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category)
	{
		this.DoAddStatusItem(status_item, category, false);
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x00253044 File Offset: 0x00251244
	private void DoAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category, bool show_immediate = false)
	{
		GameObject gameObject = this.statusItemsFolder;
		Color color;
		if (status_item.item.notificationType == NotificationType.BadMinor || status_item.item.notificationType == NotificationType.Bad || status_item.item.notificationType == NotificationType.DuplicantThreatening)
		{
			color = GlobalAssets.Instance.colorSet.statusItemBad;
		}
		else if (status_item.item.notificationType == NotificationType.Event)
		{
			color = GlobalAssets.Instance.colorSet.statusItemEvent;
		}
		else
		{
			color = this.statusItemTextColor_regular;
		}
		TextStyleSetting style = (category == Db.Get().StatusItemCategories.Main) ? this.StatusItemStyle_Main : this.StatusItemStyle_Other;
		SimpleInfoScreen.StatusItemEntry statusItemEntry = new SimpleInfoScreen.StatusItemEntry(status_item, category, this.StatusItemPrefab, gameObject.transform, this.ToolTipStyle_Property, color, style, show_immediate, new Action<SimpleInfoScreen.StatusItemEntry>(this.OnStatusItemDestroy));
		statusItemEntry.SetSprite(status_item.item.sprite);
		if (category != null)
		{
			int num = -1;
			foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems.FindAll((SimpleInfoScreen.StatusItemEntry e) => e.category == category))
			{
				num = statusItemEntry2.GetIndex();
				statusItemEntry2.Destroy(true);
				this.oldStatusItems.Remove(statusItemEntry2);
			}
			if (category == Db.Get().StatusItemCategories.Main)
			{
				num = 0;
			}
			if (num != -1)
			{
				statusItemEntry.SetIndex(num);
			}
		}
		this.statusItems.Add(statusItemEntry);
	}

	// Token: 0x060064B5 RID: 25781 RVA: 0x002531F0 File Offset: 0x002513F0
	private void OnRemoveStatusItem(StatusItemGroup.Entry status_item, bool immediate = false)
	{
		this.DoRemoveStatusItem(status_item, immediate);
	}

	// Token: 0x060064B6 RID: 25782 RVA: 0x002531FC File Offset: 0x002513FC
	private void DoRemoveStatusItem(StatusItemGroup.Entry status_item, bool destroy_immediate = false)
	{
		for (int i = 0; i < this.statusItems.Count; i++)
		{
			if (this.statusItems[i].item.item == status_item.item)
			{
				SimpleInfoScreen.StatusItemEntry statusItemEntry = this.statusItems[i];
				this.statusItems.RemoveAt(i);
				this.oldStatusItems.Add(statusItemEntry);
				statusItemEntry.Destroy(destroy_immediate);
				return;
			}
		}
	}

	// Token: 0x060064B7 RID: 25783 RVA: 0x0025326A File Offset: 0x0025146A
	private void OnStatusItemDestroy(SimpleInfoScreen.StatusItemEntry item)
	{
		this.oldStatusItems.Remove(item);
	}

	// Token: 0x060064B8 RID: 25784 RVA: 0x00253279 File Offset: 0x00251479
	private void Update()
	{
		this.Refresh(false);
	}

	// Token: 0x060064B9 RID: 25785 RVA: 0x00253282 File Offset: 0x00251482
	private void OnRefreshData(object obj)
	{
		this.Refresh(false);
	}

	// Token: 0x060064BA RID: 25786 RVA: 0x0025328C File Offset: 0x0025148C
	public void Refresh(bool force = false)
	{
		if (this.selectedTarget != this.lastTarget || force)
		{
			this.lastTarget = this.selectedTarget;
			if (this.selectedTarget != null)
			{
				this.SetPanels(this.selectedTarget);
			}
		}
		int count = this.statusItems.Count;
		this.statusItemPanel.gameObject.SetActive(count > 0);
		for (int i = 0; i < count; i++)
		{
			this.statusItems[i].Refresh();
		}
		if (this.vitalsContainer.isActiveAndEnabled)
		{
			this.vitalsContainer.Refresh();
		}
		this.RefreshStress();
		this.RefreshStorage();
		this.RefreshMovePanel();
		this.rocketSimpleInfoPanel.Refresh(this.rocketStatusContainer, this.selectedTarget);
	}

	// Token: 0x060064BB RID: 25787 RVA: 0x00253354 File Offset: 0x00251554
	private void SetPanels(GameObject target)
	{
		MinionIdentity component = target.GetComponent<MinionIdentity>();
		Amounts amounts = target.GetAmounts();
		PrimaryElement component2 = target.GetComponent<PrimaryElement>();
		BuildingComplete component3 = target.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component4 = target.GetComponent<BuildingUnderConstruction>();
		CellSelectionObject component5 = target.GetComponent<CellSelectionObject>();
		InfoDescription component6 = target.GetComponent<InfoDescription>();
		Edible component7 = target.GetComponent<Edible>();
		bool component8 = target.GetComponent<IProcessConditionSet>() != null;
		this.attributeLabels.ForEach(delegate(LocText x)
		{
			UnityEngine.Object.Destroy(x.gameObject);
		});
		this.attributeLabels.Clear();
		this.vitalsPanel.gameObject.SetActive(amounts != null);
		string text = "";
		string text2 = "";
		if (amounts != null)
		{
			this.vitalsContainer.selectedEntity = this.selectedTarget;
			Uprootable component9 = this.selectedTarget.gameObject.GetComponent<Uprootable>();
			if (component9 != null)
			{
				this.vitalsPanel.gameObject.SetActive(component9.GetPlanterStorage != null);
			}
			if (this.selectedTarget.gameObject.GetComponent<WiltCondition>() != null)
			{
				this.vitalsPanel.gameObject.SetActive(true);
			}
		}
		if (component8)
		{
			this.processConditionContainer.gameObject.SetActive(true);
			this.RefreshProcessConditions();
		}
		else
		{
			this.processConditionContainer.gameObject.SetActive(false);
		}
		if (component)
		{
			text = "";
		}
		else if (component6)
		{
			text = component6.description;
			text2 = component6.effect;
		}
		else if (component3 != null)
		{
			text = component3.DescFlavour;
			text2 = component3.Desc;
		}
		else if (component4 != null)
		{
			text = component4.Def.Effect;
			text2 = component4.Desc;
		}
		else if (component7 != null)
		{
			EdiblesManager.FoodInfo foodInfo = component7.FoodInfo;
			text += string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true));
		}
		else if (component5 != null)
		{
			text = component5.element.FullDescription(false);
		}
		else if (component2 != null)
		{
			Element element = ElementLoader.FindElementByHash(component2.ElementID);
			text = ((element != null) ? element.FullDescription(false) : "");
		}
		if (this.vitalsPanel.gameObject.activeSelf && amounts.Count == 0)
		{
			this.vitalsPanel.gameObject.SetActive(false);
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(component4 ? component4.Def.BuildingComplete : target, true);
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(component4 ? component4.Def.BuildingComplete : target, true);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors, false);
		bool flag = gameObjectEffects.Count > 0;
		this.effectsContent.gameObject.SetActive(flag);
		if (flag)
		{
			this.effectsContent.SetDescriptors(gameObjectEffects);
		}
		this.effectsPanel.gameObject.SetActive(component == null && flag);
		this.effectsContent.gameObject.SetActive(this.effectsPanel.gameObject.activeSelf);
		bool flag2 = requirementDescriptors.Count > 0 && !this.vitalsPanel.gameObject.activeSelf;
		this.requirementContent.gameObject.SetActive(flag2);
		if (flag2)
		{
			this.requirementContent.SetDescriptors(requirementDescriptors);
		}
		this.requirementsPanel.gameObject.SetActive(component == null && flag2);
		this.requirementContent.gameObject.SetActive(this.requirementsPanel.gameObject.activeSelf);
		this.infoPanel.SetLabel("Description", text, "");
		bool flag3 = !string.IsNullOrEmpty(text2) && text2 != "\n";
		string text3 = "\n" + text2;
		if (flag3)
		{
			this.infoPanel.SetLabel("Flavour", text3, "");
		}
		string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(target);
		bool flag4 = roomClassForObject != null;
		if (flag4)
		{
			string text4 = "\n" + CODEX.HEADERS.BUILDINGTYPE + ":";
			foreach (string str in roomClassForObject)
			{
				text4 = text4 + "\n    • " + str;
			}
			this.infoPanel.SetLabel("RoomClass", text4, "");
		}
		bool flag5 = text.IsNullOrWhiteSpace() && text2.IsNullOrWhiteSpace() && !flag4;
		this.infoPanel.Commit();
		this.infoPanel.gameObject.SetActive(component == null && !flag5);
	}

	// Token: 0x060064BC RID: 25788 RVA: 0x002537FC File Offset: 0x002519FC
	private void RefreshBreedingChance()
	{
		if (this.selectedTarget == null)
		{
			this.fertilityPanel.gameObject.SetActive(false);
			return;
		}
		FertilityMonitor.Instance smi = this.selectedTarget.GetSMI<FertilityMonitor.Instance>();
		if (smi == null)
		{
			this.fertilityPanel.gameObject.SetActive(false);
			return;
		}
		int num = 0;
		foreach (FertilityMonitor.BreedingChance breedingChance in smi.breedingChances)
		{
			List<FertilityModifier> forTag = Db.Get().FertilityModifiers.GetForTag(breedingChance.egg);
			if (forTag.Count > 0)
			{
				string text = "";
				foreach (FertilityModifier fertilityModifier in forTag)
				{
					text += string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_MOD_FORMAT, fertilityModifier.GetTooltip());
				}
				this.fertilityPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None), text));
			}
			else
			{
				this.fertilityPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)));
			}
		}
		this.fertilityPanel.Commit();
	}

	// Token: 0x060064BD RID: 25789 RVA: 0x00253A34 File Offset: 0x00251C34
	private void RefreshStorage()
	{
		if (this.selectedTarget == null)
		{
			this.StoragePanel.gameObject.SetActive(false);
			return;
		}
		IStorage[] array = this.selectedTarget.GetComponentsInChildren<IStorage>();
		if (array == null)
		{
			this.StoragePanel.gameObject.SetActive(false);
			return;
		}
		array = Array.FindAll<IStorage>(array, (IStorage n) => n.ShouldShowInUI());
		if (array.Length == 0)
		{
			this.StoragePanel.gameObject.SetActive(false);
			return;
		}
		string title = (this.selectedTarget.GetComponent<MinionIdentity>() != null) ? UI.DETAILTABS.DETAILS.GROUPNAME_MINION_CONTENTS : UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS;
		this.StoragePanel.gameObject.SetActive(true);
		this.StoragePanel.SetTitle(title);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storageLabels)
		{
			keyValuePair.Value.SetActive(false);
		}
		int num = 0;
		foreach (IStorage storage in array)
		{
			ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.PooledList pooledList = ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.Allocate();
			foreach (GameObject gameObject in storage.GetItems())
			{
				if (!(gameObject == null))
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (!(component != null) || component.Mass != 0f)
					{
						Rottable.Instance smi = gameObject.GetSMI<Rottable.Instance>();
						HighEnergyParticleStorage component2 = gameObject.GetComponent<HighEnergyParticleStorage>();
						string text = "";
						pooledList.Clear();
						if (component != null && component2 == null)
						{
							text = GameUtil.GetUnitFormattedName(gameObject, false);
							text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedMass(component.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, text, GameUtil.GetFormattedTemperature(component.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
						}
						if (component2 != null)
						{
							text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
							text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedHighEnergyParticles(component2.Particles, GameUtil.TimeSlice.None, true));
						}
						if (smi != null)
						{
							string text2 = smi.StateString();
							if (!string.IsNullOrEmpty(text2))
							{
								text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text2);
							}
							pooledList.Add(new global::Tuple<string, TextStyleSetting>(smi.GetToolTip(), PluginAssets.Instance.defaultTextStyleSetting));
						}
						if (component.DiseaseIdx != 255)
						{
							text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, false));
							string formattedDisease = GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true);
							pooledList.Add(new global::Tuple<string, TextStyleSetting>(formattedDisease, PluginAssets.Instance.defaultTextStyleSetting));
						}
						GameObject gameObject2 = this.AddOrGetStorageLabel(this.storageLabels, this.StoragePanel, "storage_" + num.ToString());
						num++;
						gameObject2.GetComponentInChildren<LocText>().text = text;
						gameObject2.GetComponentInChildren<ToolTip>().ClearMultiStringTooltip();
						foreach (global::Tuple<string, TextStyleSetting> tuple in pooledList)
						{
							gameObject2.GetComponentInChildren<ToolTip>().AddMultiStringTooltip(tuple.first, tuple.second);
						}
						KButton component3 = gameObject2.GetComponent<KButton>();
						GameObject select_target = gameObject;
						component3.onClick += delegate()
						{
							SelectTool.Instance.Select(select_target.GetComponent<KSelectable>(), false);
						};
						if (storage.allowUIItemRemoval)
						{
							Transform transform = gameObject2.transform.Find("removeAttributeButton");
							if (transform != null)
							{
								KButton component4 = transform.GetComponent<KButton>();
								component4.enabled = true;
								component4.gameObject.SetActive(true);
								GameObject select_item = gameObject;
								IStorage selected_storage = storage;
								component4.onClick += delegate()
								{
									selected_storage.Drop(select_item, true);
								};
							}
						}
					}
				}
			}
			pooledList.Recycle();
		}
		if (num == 0)
		{
			this.AddOrGetStorageLabel(this.storageLabels, this.StoragePanel, "empty").GetComponentInChildren<LocText>().text = UI.DETAILTABS.DETAILS.STORAGE_EMPTY;
		}
	}

	// Token: 0x060064BE RID: 25790 RVA: 0x00253EE8 File Offset: 0x002520E8
	private void RefreshMovePanel()
	{
		if (this.selectedTarget == null)
		{
			this.movePanel.gameObject.SetActive(false);
			return;
		}
		CancellableMove component = this.selectedTarget.GetComponent<CancellableMove>();
		Movable component2 = this.selectedTarget.GetComponent<Movable>();
		if (component == null && (component2 == null || !component2.IsMarkedForMove))
		{
			this.movePanel.gameObject.SetActive(false);
			return;
		}
		this.movePanel.gameObject.SetActive(true);
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storageLabels)
		{
			keyValuePair.Value.SetActive(false);
		}
		if (component != null)
		{
			List<Ref<Movable>> movingObjects = component.movingObjects;
			int num = 0;
			using (List<Ref<Movable>>.Enumerator enumerator2 = movingObjects.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Ref<Movable> @ref = enumerator2.Current;
					ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.PooledList pooledList = ListPool<global::Tuple<string, TextStyleSetting>, SimpleInfoScreen>.Allocate();
					Movable movable = @ref.Get();
					GameObject gameObject = (movable != null) ? movable.gameObject : null;
					if (!(gameObject == null) && !gameObject.HasTag(GameTags.Stored))
					{
						PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
						if (!(component3 != null) || component3.Mass != 0f)
						{
							Rottable.Instance smi = gameObject.GetSMI<Rottable.Instance>();
							HighEnergyParticleStorage component4 = gameObject.GetComponent<HighEnergyParticleStorage>();
							string text = "";
							pooledList.Clear();
							if (component3 != null && component4 == null)
							{
								text = GameUtil.GetUnitFormattedName(gameObject, false);
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedMass(component3.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, text, GameUtil.GetFormattedTemperature(component3.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							}
							if (component4 != null)
							{
								text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedHighEnergyParticles(component4.Particles, GameUtil.TimeSlice.None, true));
							}
							if (smi != null)
							{
								string text2 = smi.StateString();
								if (!string.IsNullOrEmpty(text2))
								{
									text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text2);
								}
								pooledList.Add(new global::Tuple<string, TextStyleSetting>(smi.GetToolTip(), PluginAssets.Instance.defaultTextStyleSetting));
							}
							if (component3.DiseaseIdx != 255)
							{
								text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, GameUtil.GetFormattedDisease(component3.DiseaseIdx, component3.DiseaseCount, false));
								string formattedDisease = GameUtil.GetFormattedDisease(component3.DiseaseIdx, component3.DiseaseCount, true);
								pooledList.Add(new global::Tuple<string, TextStyleSetting>(formattedDisease, PluginAssets.Instance.defaultTextStyleSetting));
							}
							GameObject gameObject2 = this.AddOrGetStorageLabel(this.storageLabels, this.movePanel, "storage_" + num.ToString());
							num++;
							gameObject2.GetComponentInChildren<LocText>().text = text;
							gameObject2.GetComponentInChildren<ToolTip>().ClearMultiStringTooltip();
							foreach (global::Tuple<string, TextStyleSetting> tuple in pooledList)
							{
								gameObject2.GetComponentInChildren<ToolTip>().AddMultiStringTooltip(tuple.first, tuple.second);
							}
							KButton component5 = gameObject2.GetComponent<KButton>();
							GameObject select_target = gameObject;
							component5.onClick += delegate()
							{
								SelectTool.Instance.SelectAndFocus(select_target.transform.GetPosition(), select_target.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
							};
							pooledList.Recycle();
						}
					}
				}
				return;
			}
		}
		if (component2 != null && component2.IsMarkedForMove)
		{
			GameObject gameObject3 = this.AddOrGetStorageLabel(this.storageLabels, this.movePanel, "moveplacer");
			gameObject3.GetComponentInChildren<LocText>().text = MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS;
			gameObject3.GetComponentInChildren<ToolTip>().ClearMultiStringTooltip();
			gameObject3.GetComponentInChildren<ToolTip>().SetSimpleTooltip(MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS_TOOLTIP);
			KButton component6 = gameObject3.GetComponent<KButton>();
			Storage select_target = component2.StorageProxy;
			component6.onClick += delegate()
			{
				SelectTool.Instance.SelectAndFocus(select_target.transform.GetPosition(), select_target.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
			};
		}
	}

	// Token: 0x060064BF RID: 25791 RVA: 0x00254358 File Offset: 0x00252558
	private void CreateWorldTraitRow()
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		this.worldTraitRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").gameObject.SetActive(false);
		component.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
	}

	// Token: 0x060064C0 RID: 25792 RVA: 0x002543C0 File Offset: 0x002525C0
	private void RefreshWorld()
	{
		WorldContainer worldContainer = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<WorldContainer>();
		AsteroidGridEntity x = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<AsteroidGridEntity>();
		bool flag = ManagementMenu.Instance.IsScreenOpen(ClusterMapScreen.Instance) && worldContainer != null && x != null;
		this.worldBiomesPanel.gameObject.SetActive(flag);
		this.worldGeysersPanel.gameObject.SetActive(flag);
		this.worldMeteorShowersPanel.gameObject.SetActive(flag);
		this.worldTraitsPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.biomeRows)
		{
			keyValuePair.Value.SetActive(false);
		}
		if (worldContainer.Biomes != null)
		{
			using (List<string>.Enumerator enumerator2 = worldContainer.Biomes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string text = enumerator2.Current;
					Sprite biomeSprite = GameUtil.GetBiomeSprite(text);
					if (!this.biomeRows.ContainsKey(text))
					{
						this.biomeRows.Add(text, global::Util.KInstantiateUI(this.bigIconLabelRow, this.worldBiomesPanel.Content.gameObject, true));
						HierarchyReferences component = this.biomeRows[text].GetComponent<HierarchyReferences>();
						component.GetReference<Image>("Icon").sprite = biomeSprite;
						component.GetReference<LocText>("NameLabel").SetText(UI.FormatAsLink(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".NAME"), "BIOME" + text.ToUpper()));
						component.GetReference<LocText>("DescriptionLabel").SetText(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".DESC"));
					}
					this.biomeRows[text].SetActive(true);
				}
				goto IL_23C;
			}
		}
		this.worldBiomesPanel.gameObject.SetActive(false);
		IL_23C:
		List<Tag> list = new List<Tag>();
		foreach (Geyser cmp in Components.Geysers.GetItems(worldContainer.id))
		{
			list.Add(cmp.PrefabID());
		}
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetUnspawnedWithType<Geyser>(worldContainer.id));
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("OilWell", worldContainer.id, true));
		foreach (KeyValuePair<Tag, GameObject> keyValuePair2 in this.geyserRows)
		{
			keyValuePair2.Value.SetActive(false);
		}
		foreach (Tag tag in list)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			if (!this.geyserRows.ContainsKey(tag))
			{
				this.geyserRows.Add(tag, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
				HierarchyReferences component2 = this.geyserRows[tag].GetComponent<HierarchyReferences>();
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(tag).GetProperName());
				component2.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			}
			this.geyserRows[tag].SetActive(true);
		}
		int count = SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("GeyserGeneric", worldContainer.id, false).Count;
		if (count > 0)
		{
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite("GeyserGeneric", "ui", false);
			Tag key = "GeyserGeneric";
			if (!this.geyserRows.ContainsKey(key))
			{
				this.geyserRows.Add(key, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
				HierarchyReferences component3 = this.geyserRows[key].GetComponent<HierarchyReferences>();
				component3.GetReference<Image>("Icon").sprite = uisprite2.first;
				component3.GetReference<Image>("Icon").color = uisprite2.second;
				component3.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.UNKNOWN_GEYSERS.Replace("{num}", count.ToString()));
				component3.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			}
			this.geyserRows[key].SetActive(true);
		}
		Tag key2 = "NoGeysers";
		if (!this.geyserRows.ContainsKey(key2))
		{
			this.geyserRows.Add(key2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
			HierarchyReferences component4 = this.geyserRows[key2].GetComponent<HierarchyReferences>();
			component4.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component4.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_GEYSERS);
			component4.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.geyserRows[key2].gameObject.SetActive(list.Count == 0);
		foreach (KeyValuePair<Tag, GameObject> keyValuePair3 in this.meteorShowerRows)
		{
			keyValuePair3.Value.SetActive(false);
		}
		bool flag2 = false;
		foreach (string id in worldContainer.GetSeasonIds())
		{
			GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(id);
			if (gameplaySeason != null)
			{
				foreach (GameplayEvent gameplayEvent in gameplaySeason.events)
				{
					if (gameplayEvent.tags.Contains(GameTags.SpaceDanger) && gameplayEvent is MeteorShowerEvent)
					{
						flag2 = true;
						MeteorShowerEvent meteorShowerEvent = gameplayEvent as MeteorShowerEvent;
						string id2 = meteorShowerEvent.Id;
						global::Tuple<Sprite, Color> uisprite3 = Def.GetUISprite(meteorShowerEvent.GetClusterMapMeteorShowerID(), "ui", false);
						if (!this.meteorShowerRows.ContainsKey(id2))
						{
							this.meteorShowerRows.Add(id2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
							HierarchyReferences component5 = this.meteorShowerRows[id2].GetComponent<HierarchyReferences>();
							component5.GetReference<Image>("Icon").sprite = uisprite3.first;
							component5.GetReference<Image>("Icon").color = uisprite3.second;
							component5.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(meteorShowerEvent.GetClusterMapMeteorShowerID()).GetProperName());
							component5.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
						}
						this.meteorShowerRows[id2].SetActive(true);
					}
				}
			}
		}
		Tag key3 = "NoMeteorShowers";
		if (!this.meteorShowerRows.ContainsKey(key3))
		{
			this.meteorShowerRows.Add(key3, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
			HierarchyReferences component6 = this.meteorShowerRows[key3].GetComponent<HierarchyReferences>();
			component6.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component6.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_METEORSHOWERS);
			component6.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.meteorShowerRows[key3].gameObject.SetActive(!flag2);
		List<string> worldTraitIds = worldContainer.WorldTraitIds;
		if (worldTraitIds != null)
		{
			for (int i = 0; i < worldTraitIds.Count; i++)
			{
				if (i > this.worldTraitRows.Count - 1)
				{
					this.CreateWorldTraitRow();
				}
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraitIds[i], false);
				Image reference = this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				if (cachedWorldTrait != null)
				{
					Sprite sprite = Assets.GetSprite(cachedWorldTrait.filePath.Substring(cachedWorldTrait.filePath.LastIndexOf("/") + 1));
					reference.gameObject.SetActive(true);
					reference.sprite = ((sprite == null) ? Assets.GetSprite("unknown") : sprite);
					reference.color = global::Util.ColorFromHex(cachedWorldTrait.colorHex);
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(Strings.Get(cachedWorldTrait.name));
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip(Strings.Get(cachedWorldTrait.description));
				}
				else
				{
					Sprite sprite2 = Assets.GetSprite("NoTraits");
					reference.gameObject.SetActive(true);
					reference.sprite = sprite2;
					reference.color = Color.white;
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.MISSING_TRAIT);
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip("");
				}
			}
			for (int j = 0; j < this.worldTraitRows.Count; j++)
			{
				this.worldTraitRows[j].SetActive(j < worldTraitIds.Count);
			}
			if (worldTraitIds.Count == 0)
			{
				if (this.worldTraitRows.Count < 1)
				{
					this.CreateWorldTraitRow();
				}
				Image reference2 = this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				Sprite sprite3 = Assets.GetSprite("NoTraits");
				reference2.gameObject.SetActive(true);
				reference2.sprite = sprite3;
				reference2.color = Color.black;
				this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND);
				this.worldTraitRows[0].AddOrGet<ToolTip>().SetSimpleTooltip(WORLD_TRAITS.NO_TRAITS.DESCRIPTION);
				this.worldTraitRows[0].SetActive(true);
			}
		}
		for (int k = this.surfaceConditionRows.Count - 1; k >= 0; k--)
		{
			global::Util.KDestroyGameObject(this.surfaceConditionRows[k]);
		}
		this.surfaceConditionRows.Clear();
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component7 = gameObject.GetComponent<HierarchyReferences>();
		component7.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_lights");
		component7.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.LIGHT);
		component7.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedLux(worldContainer.SunlightFixedTraits[worldContainer.sunlightFixedTrait]));
		component7.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject);
		GameObject gameObject2 = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component8 = gameObject2.GetComponent<HierarchyReferences>();
		component8.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_radiation");
		component8.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.RADIATION);
		component8.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedRads((float)worldContainer.CosmicRadiationFixedTraits[worldContainer.cosmicRadiationFixedTrait], GameUtil.TimeSlice.None));
		component8.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject2);
	}

	// Token: 0x060064C1 RID: 25793 RVA: 0x00255170 File Offset: 0x00253370
	private void RefreshProcessConditions()
	{
		foreach (GameObject original in this.processConditionRows)
		{
			global::Util.KDestroyGameObject(original);
		}
		this.processConditionRows.Clear();
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			if (this.selectedTarget.GetComponent<LaunchableRocket>() != null)
			{
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.All);
			return;
		}
		else
		{
			if (this.selectedTarget.GetComponent<LaunchPad>() != null || this.selectedTarget.GetComponent<RocketProcessConditionDisplayTarget>() != null)
			{
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketFlight);
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType.All);
			return;
		}
	}

	// Token: 0x060064C2 RID: 25794 RVA: 0x00255250 File Offset: 0x00253450
	private void RefreshProcessConditionsForType(ProcessCondition.ProcessConditionType conditionType)
	{
		List<ProcessCondition> conditionSet = this.selectedTarget.GetComponent<IProcessConditionSet>().GetConditionSet(conditionType);
		if (conditionSet.Count == 0)
		{
			return;
		}
		HierarchyReferences hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.processConditionHeader.gameObject, this.processConditionContainer.Content.gameObject, true);
		hierarchyReferences.GetReference<LocText>("Label").text = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper());
		hierarchyReferences.GetComponent<ToolTip>().toolTip = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper() + "_TOOLTIP");
		this.processConditionRows.Add(hierarchyReferences.gameObject);
		List<ProcessCondition> list = new List<ProcessCondition>();
		using (List<ProcessCondition>.Enumerator enumerator = conditionSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ProcessCondition condition = enumerator.Current;
				if (condition.ShowInUI() && (condition.GetType() == typeof(RequireAttachedComponent) || list.Find((ProcessCondition match) => match.GetType() == condition.GetType()) == null))
				{
					list.Add(condition);
					GameObject gameObject = global::Util.KInstantiateUI(this.processConditionRow, this.processConditionContainer.Content.gameObject, true);
					this.processConditionRows.Add(gameObject);
					ConditionListSideScreen.SetRowState(gameObject, condition);
				}
			}
		}
	}

	// Token: 0x060064C3 RID: 25795 RVA: 0x002553F0 File Offset: 0x002535F0
	public GameObject AddOrGetStorageLabel(Dictionary<string, GameObject> labels, CollapsibleDetailContentPanel panel, string id)
	{
		GameObject gameObject;
		if (labels.ContainsKey(id))
		{
			gameObject = labels[id];
			gameObject.GetComponent<KButton>().ClearOnClick();
			Transform transform = gameObject.transform.Find("removeAttributeButton");
			if (transform != null)
			{
				KButton kbutton = transform.FindComponent<KButton>();
				kbutton.enabled = false;
				kbutton.gameObject.SetActive(false);
				kbutton.ClearOnClick();
			}
		}
		else
		{
			gameObject = global::Util.KInstantiate(this.attributesLabelButtonTemplate, panel.Content.gameObject, null);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			labels[id] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060064C4 RID: 25796 RVA: 0x0025549C File Offset: 0x0025369C
	private void RefreshStress()
	{
		MinionIdentity identity = (this.selectedTarget != null) ? this.selectedTarget.GetComponent<MinionIdentity>() : null;
		if (identity == null)
		{
			this.stressPanel.gameObject.SetActive(false);
			return;
		}
		List<ReportManager.ReportEntry.Note> stressNotes = new List<ReportManager.ReportEntry.Note>();
		this.stressPanel.gameObject.SetActive(true);
		this.stressPanel.SetTitle(UI.DETAILTABS.STATS.GROUPNAME_STRESS);
		ReportManager.ReportEntry reportEntry = ReportManager.Instance.TodaysReport.reportEntries.Find((ReportManager.ReportEntry entry) => entry.reportType == ReportManager.ReportType.StressDelta);
		this.stressDrawer.BeginDrawing();
		float num = 0f;
		stressNotes.Clear();
		int num2 = reportEntry.contextEntries.FindIndex((ReportManager.ReportEntry entry) => entry.context == identity.GetProperName());
		ReportManager.ReportEntry reportEntry2 = (num2 != -1) ? reportEntry.contextEntries[num2] : null;
		if (reportEntry2 != null)
		{
			reportEntry2.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
			{
				stressNotes.Add(note);
			});
			stressNotes.Sort((ReportManager.ReportEntry.Note a, ReportManager.ReportEntry.Note b) => a.value.CompareTo(b.value));
			for (int i = 0; i < stressNotes.Count; i++)
			{
				string text = float.IsNegativeInfinity(stressNotes[i].value) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(stressNotes[i].value);
				this.stressDrawer.NewLabel(string.Concat(new string[]
				{
					(stressNotes[i].value > 0f) ? UIConstants.ColorPrefixRed : "",
					stressNotes[i].note,
					": ",
					text,
					"%",
					(stressNotes[i].value > 0f) ? UIConstants.ColorSuffix : ""
				}));
				num += stressNotes[i].value;
			}
		}
		string arg = float.IsNegativeInfinity(num) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(num);
		this.stressDrawer.NewLabel(((num > 0f) ? UIConstants.ColorPrefixRed : "") + string.Format(UI.DETAILTABS.DETAILS.NET_STRESS, arg) + ((num > 0f) ? UIConstants.ColorSuffix : ""));
		this.stressDrawer.EndDrawing();
	}

	// Token: 0x060064C5 RID: 25797 RVA: 0x0025575A File Offset: 0x0025395A
	public void Sim1000ms(float dt)
	{
		if (this.selectedTarget != null && this.selectedTarget.GetComponent<IProcessConditionSet>() != null)
		{
			this.RefreshProcessConditions();
		}
	}

	// Token: 0x060064C6 RID: 25798 RVA: 0x0025577D File Offset: 0x0025397D
	public void Sim4000ms(float dt)
	{
		this.RefreshWorld();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
	}

	// Token: 0x040044B3 RID: 17587
	public GameObject iconLabelRow;

	// Token: 0x040044B4 RID: 17588
	public GameObject spacerRow;

	// Token: 0x040044B5 RID: 17589
	[SerializeField]
	private GameObject attributesLabelTemplate;

	// Token: 0x040044B6 RID: 17590
	[SerializeField]
	private GameObject attributesLabelButtonTemplate;

	// Token: 0x040044B7 RID: 17591
	[SerializeField]
	private DescriptorPanel DescriptorContentPrefab;

	// Token: 0x040044B8 RID: 17592
	[SerializeField]
	private GameObject VitalsPanelTemplate;

	// Token: 0x040044B9 RID: 17593
	[SerializeField]
	private GameObject StatusItemPrefab;

	// Token: 0x040044BA RID: 17594
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x040044BB RID: 17595
	[SerializeField]
	private HierarchyReferences processConditionHeader;

	// Token: 0x040044BC RID: 17596
	[SerializeField]
	private GameObject processConditionRow;

	// Token: 0x040044BD RID: 17597
	[SerializeField]
	private Text StatusPanelCurrentActionLabel;

	// Token: 0x040044BE RID: 17598
	[SerializeField]
	private GameObject bigIconLabelRow;

	// Token: 0x040044BF RID: 17599
	[SerializeField]
	private TextStyleSetting ToolTipStyle_Property;

	// Token: 0x040044C0 RID: 17600
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Main;

	// Token: 0x040044C1 RID: 17601
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Other;

	// Token: 0x040044C2 RID: 17602
	[SerializeField]
	private Color statusItemTextColor_regular = Color.black;

	// Token: 0x040044C3 RID: 17603
	[SerializeField]
	private Color statusItemTextColor_old = new Color(0.8235294f, 0.8235294f, 0.8235294f);

	// Token: 0x040044C5 RID: 17605
	private CollapsibleDetailContentPanel statusItemPanel;

	// Token: 0x040044C6 RID: 17606
	private CollapsibleDetailContentPanel vitalsPanel;

	// Token: 0x040044C7 RID: 17607
	private CollapsibleDetailContentPanel fertilityPanel;

	// Token: 0x040044C8 RID: 17608
	private CollapsibleDetailContentPanel rocketStatusContainer;

	// Token: 0x040044C9 RID: 17609
	private CollapsibleDetailContentPanel worldLifePanel;

	// Token: 0x040044CA RID: 17610
	private CollapsibleDetailContentPanel worldElementsPanel;

	// Token: 0x040044CB RID: 17611
	private CollapsibleDetailContentPanel worldBiomesPanel;

	// Token: 0x040044CC RID: 17612
	private CollapsibleDetailContentPanel worldGeysersPanel;

	// Token: 0x040044CD RID: 17613
	private CollapsibleDetailContentPanel worldMeteorShowersPanel;

	// Token: 0x040044CE RID: 17614
	private CollapsibleDetailContentPanel spacePOIPanel;

	// Token: 0x040044CF RID: 17615
	private CollapsibleDetailContentPanel worldTraitsPanel;

	// Token: 0x040044D0 RID: 17616
	private CollapsibleDetailContentPanel processConditionContainer;

	// Token: 0x040044D1 RID: 17617
	private CollapsibleDetailContentPanel requirementsPanel;

	// Token: 0x040044D2 RID: 17618
	private CollapsibleDetailContentPanel effectsPanel;

	// Token: 0x040044D3 RID: 17619
	private CollapsibleDetailContentPanel stressPanel;

	// Token: 0x040044D4 RID: 17620
	private CollapsibleDetailContentPanel infoPanel;

	// Token: 0x040044D5 RID: 17621
	private CollapsibleDetailContentPanel movePanel;

	// Token: 0x040044D6 RID: 17622
	private DescriptorPanel effectsContent;

	// Token: 0x040044D7 RID: 17623
	private DescriptorPanel requirementContent;

	// Token: 0x040044D8 RID: 17624
	private RocketSimpleInfoPanel rocketSimpleInfoPanel;

	// Token: 0x040044D9 RID: 17625
	private SpacePOISimpleInfoPanel spaceSimpleInfoPOIPanel;

	// Token: 0x040044DA RID: 17626
	private MinionVitalsPanel vitalsContainer;

	// Token: 0x040044DB RID: 17627
	private DetailsPanelDrawer stressDrawer;

	// Token: 0x040044DC RID: 17628
	private bool TargetIsMinion;

	// Token: 0x040044DD RID: 17629
	private GameObject lastTarget;

	// Token: 0x040044DE RID: 17630
	private GameObject statusItemsFolder;

	// Token: 0x040044DF RID: 17631
	private Dictionary<string, GameObject> storageLabels = new Dictionary<string, GameObject>();

	// Token: 0x040044E0 RID: 17632
	private Dictionary<Tag, GameObject> lifeformRows = new Dictionary<Tag, GameObject>();

	// Token: 0x040044E1 RID: 17633
	private Dictionary<Tag, GameObject> biomeRows = new Dictionary<Tag, GameObject>();

	// Token: 0x040044E2 RID: 17634
	private Dictionary<Tag, GameObject> geyserRows = new Dictionary<Tag, GameObject>();

	// Token: 0x040044E3 RID: 17635
	private Dictionary<Tag, GameObject> meteorShowerRows = new Dictionary<Tag, GameObject>();

	// Token: 0x040044E4 RID: 17636
	private List<GameObject> worldTraitRows = new List<GameObject>();

	// Token: 0x040044E5 RID: 17637
	private List<GameObject> surfaceConditionRows = new List<GameObject>();

	// Token: 0x040044E6 RID: 17638
	private List<SimpleInfoScreen.StatusItemEntry> statusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x040044E7 RID: 17639
	private List<SimpleInfoScreen.StatusItemEntry> oldStatusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x040044E8 RID: 17640
	private List<LocText> attributeLabels = new List<LocText>();

	// Token: 0x040044E9 RID: 17641
	private List<GameObject> processConditionRows = new List<GameObject>();

	// Token: 0x040044EA RID: 17642
	private Action<object> onStorageChangeDelegate;

	// Token: 0x040044EB RID: 17643
	private static readonly EventSystem.IntraObjectHandler<SimpleInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<SimpleInfoScreen>(delegate(SimpleInfoScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x02001B92 RID: 7058
	[DebuggerDisplay("{item.item.Name}")]
	public class StatusItemEntry : IRenderEveryTick
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06009A51 RID: 39505 RVA: 0x00346654 File Offset: 0x00344854
		public Image GetImage
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x06009A52 RID: 39506 RVA: 0x0034665C File Offset: 0x0034485C
		public StatusItemEntry(StatusItemGroup.Entry item, StatusItemCategory category, GameObject status_item_prefab, Transform parent, TextStyleSetting tooltip_style, Color color, TextStyleSetting style, bool skip_fade, Action<SimpleInfoScreen.StatusItemEntry> onDestroy)
		{
			this.item = item;
			this.category = category;
			this.tooltipStyle = tooltip_style;
			this.onDestroy = onDestroy;
			this.color = color;
			this.style = style;
			this.widget = global::Util.KInstantiateUI(status_item_prefab, parent.gameObject, false);
			this.text = this.widget.GetComponentInChildren<LocText>(true);
			SetTextStyleSetting.ApplyStyle(this.text, style);
			this.toolTip = this.widget.GetComponentInChildren<ToolTip>(true);
			this.image = this.widget.GetComponentInChildren<Image>(true);
			item.SetIcon(this.image);
			this.widget.SetActive(true);
			this.toolTip.OnToolTip = new Func<string>(this.OnToolTip);
			this.button = this.widget.GetComponentInChildren<KButton>();
			if (item.item.statusItemClickCallback != null)
			{
				this.button.onClick += this.OnClick;
			}
			else
			{
				this.button.enabled = false;
			}
			this.fadeStage = (skip_fade ? SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT : SimpleInfoScreen.StatusItemEntry.FadeStage.IN);
			SimAndRenderScheduler.instance.Add(this, false);
			this.Refresh();
			this.SetColor(1f);
		}

		// Token: 0x06009A53 RID: 39507 RVA: 0x0034679D File Offset: 0x0034499D
		internal void SetSprite(TintedSprite sprite)
		{
			if (sprite != null)
			{
				this.image.sprite = sprite.sprite;
			}
		}

		// Token: 0x06009A54 RID: 39508 RVA: 0x003467B3 File Offset: 0x003449B3
		public int GetIndex()
		{
			return this.widget.transform.GetSiblingIndex();
		}

		// Token: 0x06009A55 RID: 39509 RVA: 0x003467C5 File Offset: 0x003449C5
		public void SetIndex(int index)
		{
			this.widget.transform.SetSiblingIndex(index);
		}

		// Token: 0x06009A56 RID: 39510 RVA: 0x003467D8 File Offset: 0x003449D8
		public void RenderEveryTick(float dt)
		{
			switch (this.fadeStage)
			{
			case SimpleInfoScreen.StatusItemEntry.FadeStage.IN:
			{
				this.fade = Mathf.Min(this.fade + Time.deltaTime / this.fadeInTime, 1f);
				float num = this.fade;
				this.SetColor(num);
				if (this.fade >= 1f)
				{
					this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT;
					return;
				}
				break;
			}
			case SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT:
				break;
			case SimpleInfoScreen.StatusItemEntry.FadeStage.OUT:
			{
				float num2 = this.fade;
				this.SetColor(num2);
				this.fade = Mathf.Max(this.fade - Time.deltaTime / this.fadeOutTime, 0f);
				if (this.fade <= 0f)
				{
					this.Destroy(true);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06009A57 RID: 39511 RVA: 0x0034688A File Offset: 0x00344A8A
		private string OnToolTip()
		{
			this.item.ShowToolTip(this.toolTip, this.tooltipStyle);
			return "";
		}

		// Token: 0x06009A58 RID: 39512 RVA: 0x003468A8 File Offset: 0x00344AA8
		private void OnClick()
		{
			this.item.OnClick();
		}

		// Token: 0x06009A59 RID: 39513 RVA: 0x003468B8 File Offset: 0x00344AB8
		public void Refresh()
		{
			string name = this.item.GetName();
			if (name != this.text.text)
			{
				this.text.text = name;
				this.SetColor(1f);
			}
		}

		// Token: 0x06009A5A RID: 39514 RVA: 0x003468FC File Offset: 0x00344AFC
		private void SetColor(float alpha = 1f)
		{
			Color color = new Color(this.color.r, this.color.g, this.color.b, alpha);
			this.image.color = color;
			this.text.color = color;
		}

		// Token: 0x06009A5B RID: 39515 RVA: 0x0034694C File Offset: 0x00344B4C
		public void Destroy(bool immediate)
		{
			if (this.toolTip != null)
			{
				this.toolTip.OnToolTip = null;
			}
			if (this.button != null && this.button.enabled)
			{
				this.button.onClick -= this.OnClick;
			}
			if (immediate)
			{
				if (this.onDestroy != null)
				{
					this.onDestroy(this);
				}
				SimAndRenderScheduler.instance.Remove(this);
				UnityEngine.Object.Destroy(this.widget);
				return;
			}
			this.fade = 0.5f;
			this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.OUT;
		}

		// Token: 0x04007D20 RID: 32032
		public StatusItemGroup.Entry item;

		// Token: 0x04007D21 RID: 32033
		public StatusItemCategory category;

		// Token: 0x04007D22 RID: 32034
		public Color color;

		// Token: 0x04007D23 RID: 32035
		public TextStyleSetting style;

		// Token: 0x04007D24 RID: 32036
		public Action<SimpleInfoScreen.StatusItemEntry> onDestroy;

		// Token: 0x04007D25 RID: 32037
		private LayoutElement spacerLayout;

		// Token: 0x04007D26 RID: 32038
		private GameObject widget;

		// Token: 0x04007D27 RID: 32039
		private ToolTip toolTip;

		// Token: 0x04007D28 RID: 32040
		private TextStyleSetting tooltipStyle;

		// Token: 0x04007D29 RID: 32041
		private Image image;

		// Token: 0x04007D2A RID: 32042
		private LocText text;

		// Token: 0x04007D2B RID: 32043
		private KButton button;

		// Token: 0x04007D2C RID: 32044
		private SimpleInfoScreen.StatusItemEntry.FadeStage fadeStage;

		// Token: 0x04007D2D RID: 32045
		private float fade;

		// Token: 0x04007D2E RID: 32046
		private float fadeInTime;

		// Token: 0x04007D2F RID: 32047
		private float fadeOutTime = 1.8f;

		// Token: 0x0200223A RID: 8762
		private enum FadeStage
		{
			// Token: 0x040098F4 RID: 39156
			IN,
			// Token: 0x040098F5 RID: 39157
			WAIT,
			// Token: 0x040098F6 RID: 39158
			OUT
		}
	}
}
