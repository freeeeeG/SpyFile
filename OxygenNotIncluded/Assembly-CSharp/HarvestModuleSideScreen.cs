using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C21 RID: 3105
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x0600623E RID: 25150 RVA: 0x002446E8 File Offset: 0x002428E8
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600623F RID: 25151 RVA: 0x002446F5 File Offset: 0x002428F5
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006240 RID: 25152 RVA: 0x00244705 File Offset: 0x00242905
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006241 RID: 25153 RVA: 0x0024470C File Offset: 0x0024290C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x06006242 RID: 25154 RVA: 0x00244730 File Offset: 0x00242930
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		this.RefreshModulePanel(resourceHarvestModule);
	}

	// Token: 0x06006243 RID: 25155 RVA: 0x00244764 File Offset: 0x00242964
	private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
			{
				return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
			}
		}
		return null;
	}

	// Token: 0x06006244 RID: 25156 RVA: 0x002447D0 File Offset: 0x002429D0
	private void RefreshModulePanel(StateMachine.Instance module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x06006245 RID: 25157 RVA: 0x00244824 File Offset: 0x00242A24
	public void SimEveryTick(float dt)
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		if (resourceHarvestModule == null)
		{
			return;
		}
		GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("progressBar");
		float num = 4f;
		float num2 = resourceHarvestModule.timeinstate % num;
		if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
		{
			reference.SetFillPercentage(num2 / num);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
		}
		else
		{
			reference.SetFillPercentage(0f);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
		}
		GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("diamondProgressBar");
		Storage component2 = resourceHarvestModule.GetComponent<Storage>();
		float fillPercentage = component2.MassStored() / component2.Capacity();
		reference2.SetFillPercentage(fillPercentage);
		reference2.label.SetText(ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name + ": " + GameUtil.GetFormattedMass(component2.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x040042F7 RID: 17143
	private Clustercraft targetCraft;

	// Token: 0x040042F8 RID: 17144
	public GameObject moduleContentContainer;

	// Token: 0x040042F9 RID: 17145
	public GameObject modulePanelPrefab;
}
