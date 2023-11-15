using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C53 RID: 3155
public class TelescopeSideScreen : SideScreenContent
{
	// Token: 0x060063FD RID: 25597 RVA: 0x0024F5D4 File Offset: 0x0024D7D4
	public TelescopeSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x060063FE RID: 25598 RVA: 0x0024F5F0 File Offset: 0x0024D7F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectStarmapScreen.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
		SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x060063FF RID: 25599 RVA: 0x0024F64A File Offset: 0x0024D84A
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
	}

	// Token: 0x06006400 RID: 25600 RVA: 0x0024F673 File Offset: 0x0024D873
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06006401 RID: 25601 RVA: 0x0024F68F File Offset: 0x0024D88F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x06006402 RID: 25602 RVA: 0x0024F6AB File Offset: 0x0024D8AB
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telescope>() != null;
	}

	// Token: 0x06006403 RID: 25603 RVA: 0x0024F6BC File Offset: 0x0024D8BC
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		if (SelectTool.Instance.selected.GetComponent<Telescope>() == null)
		{
			return;
		}
		if (!SpacecraftManager.instance.HasAnalysisTarget())
		{
			this.DescriptionText.text = "<b><color=#FF0000>" + UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
			return;
		}
		string text = UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
		this.DescriptionText.text = text;
	}

	// Token: 0x0400443D RID: 17469
	public KButton selectStarmapScreen;

	// Token: 0x0400443E RID: 17470
	public Image researchButtonIcon;

	// Token: 0x0400443F RID: 17471
	public GameObject content;

	// Token: 0x04004440 RID: 17472
	private GameObject target;

	// Token: 0x04004441 RID: 17473
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x04004442 RID: 17474
	public LocText DescriptionText;
}
