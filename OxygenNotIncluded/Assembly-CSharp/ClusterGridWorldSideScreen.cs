using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C06 RID: 3078
public class ClusterGridWorldSideScreen : SideScreenContent
{
	// Token: 0x0600617B RID: 24955 RVA: 0x0023F7F0 File Offset: 0x0023D9F0
	protected override void OnSpawn()
	{
		this.viewButton.onClick += this.OnClickView;
	}

	// Token: 0x0600617C RID: 24956 RVA: 0x0023F809 File Offset: 0x0023DA09
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AsteroidGridEntity>() != null;
	}

	// Token: 0x0600617D RID: 24957 RVA: 0x0023F818 File Offset: 0x0023DA18
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetEntity = target.GetComponent<AsteroidGridEntity>();
		this.icon.sprite = Def.GetUISprite(this.targetEntity, "ui", false).first;
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		bool flag = component != null && component.IsDiscovered;
		this.viewButton.isInteractable = flag;
		if (!flag)
		{
			this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
			return;
		}
		this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
	}

	// Token: 0x0600617E RID: 24958 RVA: 0x0023F8BC File Offset: 0x0023DABC
	private void OnClickView()
	{
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		if (!component.IsDupeVisited)
		{
			component.LookAtSurface();
		}
		ClusterManager.Instance.SetActiveWorld(component.id);
		ManagementMenu.Instance.CloseAll();
	}

	// Token: 0x0400425E RID: 16990
	public Image icon;

	// Token: 0x0400425F RID: 16991
	public KButton viewButton;

	// Token: 0x04004260 RID: 16992
	private AsteroidGridEntity targetEntity;
}
