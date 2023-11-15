using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C55 RID: 3157
public class TemporalTearSideScreen : SideScreenContent
{
	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x0600640F RID: 25615 RVA: 0x0024F988 File Offset: 0x0024DB88
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x06006410 RID: 25616 RVA: 0x0024F995 File Offset: 0x0024DB95
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06006411 RID: 25617 RVA: 0x0024F9A5 File Offset: 0x0024DBA5
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x06006412 RID: 25618 RVA: 0x0024F9AC File Offset: 0x0024DBAC
	public override bool IsValidForTarget(GameObject target)
	{
		Clustercraft component = target.GetComponent<Clustercraft>();
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		return component != null && temporalTear != null && temporalTear.Location == component.Location;
	}

	// Token: 0x06006413 RID: 25619 RVA: 0x0024F9F8 File Offset: 0x0024DBF8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		KButton reference = base.GetComponent<HierarchyReferences>().GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			target.GetComponent<Clustercraft>();
			ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear().ConsumeCraft(this.targetCraft);
		};
		this.RefreshPanel(null);
	}

	// Token: 0x06006414 RID: 25620 RVA: 0x0024FA64 File Offset: 0x0024DC64
	private void RefreshPanel(object data = null)
	{
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		bool flag = temporalTear.IsOpen();
		component.GetReference<LocText>("label").SetText(flag ? UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_OPEN : UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_CLOSED);
		component.GetReference<KButton>("button").isInteractable = flag;
	}

	// Token: 0x04004449 RID: 17481
	private Clustercraft targetCraft;
}
