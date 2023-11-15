using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2F RID: 3119
public class ModuleFlightUtilitySideScreen : SideScreenContent
{
	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x060062B0 RID: 25264 RVA: 0x00246FD0 File Offset: 0x002451D0
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x060062B1 RID: 25265 RVA: 0x00246FDD File Offset: 0x002451DD
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060062B2 RID: 25266 RVA: 0x00246FED File Offset: 0x002451ED
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x060062B3 RID: 25267 RVA: 0x00246FF4 File Offset: 0x002451F4
	public override bool IsValidForTarget(GameObject target)
	{
		if (target.GetComponent<Clustercraft>() != null && this.HasFlightUtilityModule(target.GetComponent<CraftModuleInterface>()))
		{
			return true;
		}
		RocketControlStation component = target.GetComponent<RocketControlStation>();
		return component != null && this.HasFlightUtilityModule(component.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface);
	}

	// Token: 0x060062B4 RID: 25268 RVA: 0x00247048 File Offset: 0x00245248
	private bool HasFlightUtilityModule(CraftModuleInterface craftModuleInterface)
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = craftModuleInterface.ClusterModules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetSMI<IEmptyableCargo>() != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060062B5 RID: 25269 RVA: 0x002470A0 File Offset: 0x002452A0
	public override void SetTarget(GameObject target)
	{
		if (target != null)
		{
			foreach (int id in this.refreshHandle)
			{
				target.Unsubscribe(id);
			}
			this.refreshHandle.Clear();
		}
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		if (this.targetCraft == null && target.GetComponent<RocketControlStation>() != null)
		{
			this.targetCraft = target.GetMyWorld().GetComponent<Clustercraft>();
		}
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(-1298331547, new Action<object>(this.RefreshAll)));
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(1792516731, new Action<object>(this.RefreshAll)));
		this.BuildModules();
	}

	// Token: 0x060062B6 RID: 25270 RVA: 0x002471A8 File Offset: 0x002453A8
	private void ClearModules()
	{
		foreach (KeyValuePair<IEmptyableCargo, HierarchyReferences> keyValuePair in this.modulePanels)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.modulePanels.Clear();
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x00247210 File Offset: 0x00245410
	private void BuildModules()
	{
		this.ClearModules();
		foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
		{
			IEmptyableCargo smi = @ref.Get().GetSMI<IEmptyableCargo>();
			if (smi != null)
			{
				HierarchyReferences value = Util.KInstantiateUI<HierarchyReferences>(this.modulePanelPrefab, this.moduleContentContainer, true);
				this.modulePanels.Add(smi, value);
				this.RefreshModulePanel(smi);
			}
		}
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x00247298 File Offset: 0x00245498
	private void RefreshAll(object data = null)
	{
		this.BuildModules();
	}

	// Token: 0x060062B9 RID: 25273 RVA: 0x002472A0 File Offset: 0x002454A0
	private void RefreshModulePanel(IEmptyableCargo module)
	{
		HierarchyReferences hierarchyReferences = this.modulePanels[module];
		hierarchyReferences.GetReference<Image>("icon").sprite = Def.GetUISprite(module.master.gameObject, "ui", false).first;
		KButton reference = hierarchyReferences.GetReference<KButton>("button");
		reference.isInteractable = module.CanEmptyCargo();
		reference.ClearOnClick();
		reference.onClick += module.EmptyCargo;
		KButton reference2 = hierarchyReferences.GetReference<KButton>("repeatButton");
		if (module.CanAutoDeploy)
		{
			this.StyleRepeatButton(module);
			reference2.ClearOnClick();
			reference2.onClick += delegate()
			{
				this.OnRepeatClicked(module);
			};
			reference2.gameObject.SetActive(true);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		DropDown reference3 = hierarchyReferences.GetReference<DropDown>("dropDown");
		reference3.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		reference3.Close();
		CrewPortrait reference4 = hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait");
		WorldContainer component = (module as StateMachine.Instance).GetMaster().GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<WorldContainer>();
		if (component != null && module.ChooseDuplicant)
		{
			int id = component.id;
			reference3.gameObject.SetActive(true);
			reference3.Initialize(Components.LiveMinionIdentities.GetWorldItems(id, false), new Action<IListableOption, object>(this.OnDuplicantEntryClick), null, new Action<DropDownEntry, object>(this.DropDownEntryRefreshAction), true, module);
			reference3.selectedLabel.text = ((module.ChosenDuplicant != null) ? this.GetDuplicantRowName(module.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
			reference4.gameObject.SetActive(true);
			reference4.SetIdentityObject(module.ChosenDuplicant, false);
			reference3.openButton.isInteractable = !module.ModuleDeployed;
		}
		else
		{
			reference3.gameObject.SetActive(false);
			reference4.gameObject.SetActive(false);
		}
		hierarchyReferences.GetReference<LocText>("label").SetText(module.master.gameObject.GetProperName());
	}

	// Token: 0x060062BA RID: 25274 RVA: 0x002474F8 File Offset: 0x002456F8
	private string GetDuplicantRowName(MinionIdentity minion)
	{
		MinionResume component = minion.GetComponent<MinionResume>();
		if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
		{
			return string.Format(UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.PILOT_FMT, minion.GetProperName());
		}
		return minion.GetProperName();
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x00247548 File Offset: 0x00245748
	private void OnRepeatClicked(IEmptyableCargo module)
	{
		module.AutoDeploy = !module.AutoDeploy;
		this.StyleRepeatButton(module);
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x00247560 File Offset: 0x00245760
	private void OnDuplicantEntryClick(IListableOption option, object data)
	{
		MinionIdentity chosenDuplicant = (MinionIdentity)option;
		IEmptyableCargo emptyableCargo = (IEmptyableCargo)data;
		emptyableCargo.ChosenDuplicant = chosenDuplicant;
		HierarchyReferences hierarchyReferences = this.modulePanels[emptyableCargo];
		hierarchyReferences.GetReference<DropDown>("dropDown").selectedLabel.text = ((emptyableCargo.ChosenDuplicant != null) ? this.GetDuplicantRowName(emptyableCargo.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
		hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait").SetIdentityObject(emptyableCargo.ChosenDuplicant, false);
		this.RefreshAll(null);
	}

	// Token: 0x060062BD RID: 25277 RVA: 0x002475E8 File Offset: 0x002457E8
	private void DropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		entry.label.text = this.GetDuplicantRowName(minionIdentity);
		entry.portrait.SetIdentityObject(minionIdentity, false);
		bool flag = false;
		foreach (Ref<RocketModuleCluster> @ref in this.targetCraft.ModuleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (!(rocketModuleCluster == null))
			{
				IEmptyableCargo smi = rocketModuleCluster.GetSMI<IEmptyableCargo>();
				if (smi != null && !(((IEmptyableCargo)targetData).ChosenDuplicant == minionIdentity))
				{
					flag = (flag || smi.ChosenDuplicant == minionIdentity);
				}
			}
		}
		entry.button.isInteractable = !flag;
	}

	// Token: 0x060062BE RID: 25278 RVA: 0x002476B8 File Offset: 0x002458B8
	private void StyleRepeatButton(IEmptyableCargo module)
	{
		KButton reference = this.modulePanels[module].GetReference<KButton>("repeatButton");
		reference.bgImage.colorStyleSetting = (module.AutoDeploy ? this.repeatOn : this.repeatOff);
		reference.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x04004349 RID: 17225
	private Clustercraft targetCraft;

	// Token: 0x0400434A RID: 17226
	public GameObject moduleContentContainer;

	// Token: 0x0400434B RID: 17227
	public GameObject modulePanelPrefab;

	// Token: 0x0400434C RID: 17228
	public ColorStyleSetting repeatOff;

	// Token: 0x0400434D RID: 17229
	public ColorStyleSetting repeatOn;

	// Token: 0x0400434E RID: 17230
	private Dictionary<IEmptyableCargo, HierarchyReferences> modulePanels = new Dictionary<IEmptyableCargo, HierarchyReferences>();

	// Token: 0x0400434F RID: 17231
	private List<int> refreshHandle = new List<int>();
}
