using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000810 RID: 2064
public class ClusterMapSelectTool : InterfaceTool
{
	// Token: 0x06003B58 RID: 15192 RVA: 0x00149DE6 File Offset: 0x00147FE6
	public static void DestroyInstance()
	{
		ClusterMapSelectTool.Instance = null;
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x00149DEE File Offset: 0x00147FEE
	protected override void OnPrefabInit()
	{
		ClusterMapSelectTool.Instance = this;
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x00149DF6 File Offset: 0x00147FF6
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x00149E1A File Offset: 0x0014801A
	public KSelectable GetSelected()
	{
		return this.m_selected;
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x00149E22 File Offset: 0x00148022
	public override bool ShowHoverUI()
	{
		return ClusterMapScreen.Instance.HasCurrentHover();
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x00149E2E File Offset: 0x0014802E
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x00149E48 File Offset: 0x00148048
	private void UpdateHoveredSelectables()
	{
		this.m_hoveredSelectables.Clear();
		if (ClusterMapScreen.Instance.HasCurrentHover())
		{
			AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
			List<KSelectable> collection = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation)
			select entity.GetComponent<KSelectable>() into selectable
			where selectable != null && selectable.IsSelectable
			select selectable).ToList<KSelectable>();
			this.m_hoveredSelectables.AddRange(collection);
		}
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x00149EDC File Offset: 0x001480DC
	public override void LateUpdate()
	{
		this.UpdateHoveredSelectables();
		KSelectable kselectable = (this.m_hoveredSelectables.Count > 0) ? this.m_hoveredSelectables[0] : null;
		base.UpdateHoverElements(this.m_hoveredSelectables);
		if (!this.hasFocus)
		{
			base.ClearHover();
		}
		else if (kselectable != this.hover)
		{
			base.ClearHover();
			this.hover = kselectable;
			if (kselectable != null)
			{
				Game.Instance.Trigger(2095258329, kselectable.gameObject);
				kselectable.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x00149F7F File Offset: 0x0014817F
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x00149FAD File Offset: 0x001481AD
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x00149FC8 File Offset: 0x001481C8
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.m_selected)
		{
			return;
		}
		if (this.m_selected != null)
		{
			this.m_selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == -1)
		{
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
		}
		this.m_selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x0400272F RID: 10031
	private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();

	// Token: 0x04002730 RID: 10032
	private KSelectable m_selected;

	// Token: 0x04002731 RID: 10033
	public static ClusterMapSelectTool Instance;

	// Token: 0x04002732 RID: 10034
	private KSelectable delayedNextSelection;

	// Token: 0x04002733 RID: 10035
	private bool delayedSkipSound;
}
