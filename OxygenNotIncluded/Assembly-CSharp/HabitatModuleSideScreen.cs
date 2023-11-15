using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C20 RID: 3104
public class HabitatModuleSideScreen : SideScreenContent
{
	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06006236 RID: 25142 RVA: 0x0024456F File Offset: 0x0024276F
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006237 RID: 25143 RVA: 0x0024457C File Offset: 0x0024277C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006238 RID: 25144 RVA: 0x0024458C File Offset: 0x0024278C
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006239 RID: 25145 RVA: 0x00244593 File Offset: 0x00242793
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetPassengerModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x0600623A RID: 25146 RVA: 0x002445B8 File Offset: 0x002427B8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		PassengerRocketModule passengerModule = this.GetPassengerModule(this.targetCraft);
		this.RefreshModulePanel(passengerModule);
	}

	// Token: 0x0600623B RID: 25147 RVA: 0x002445EC File Offset: 0x002427EC
	private PassengerRocketModule GetPassengerModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600623C RID: 25148 RVA: 0x00244654 File Offset: 0x00242854
	private void RefreshModulePanel(PassengerRocketModule module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		KButton reference = component.GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			AudioMixer.instance.Start(module.interiorReverbSnapshot);
			ClusterManager.Instance.SetActiveWorld(module.GetComponent<ClustercraftExteriorDoor>().GetTargetWorld().id);
			ManagementMenu.Instance.CloseAll();
		};
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x040042F4 RID: 17140
	private Clustercraft targetCraft;

	// Token: 0x040042F5 RID: 17141
	public GameObject moduleContentContainer;

	// Token: 0x040042F6 RID: 17142
	public GameObject modulePanelPrefab;
}
