using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200082E RID: 2094
public class SelectTool : InterfaceTool
{
	// Token: 0x06003CC9 RID: 15561 RVA: 0x00150E2B File Offset: 0x0014F02B
	public static void DestroyInstance()
	{
		SelectTool.Instance = null;
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x00150E34 File Offset: 0x0014F034
	protected override void OnPrefabInit()
	{
		this.defaultLayerMask = (1 | LayerMask.GetMask(new string[]
		{
			"World",
			"Pickupable",
			"Place",
			"PlaceWithDepth",
			"BlockSelection",
			"Construction",
			"Selection"
		}));
		this.layerMask = this.defaultLayerMask;
		this.selectMarker = global::Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
		this.selectMarker.gameObject.SetActive(false);
		this.populateHitsList = true;
		SelectTool.Instance = this;
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x00150ED6 File Offset: 0x0014F0D6
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x00150EFA File Offset: 0x0014F0FA
	public void SetLayerMask(int mask)
	{
		this.layerMask = mask;
		base.ClearHover();
		this.LateUpdate();
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x00150F0F File Offset: 0x0014F10F
	public void ClearLayerMask()
	{
		this.layerMask = this.defaultLayerMask;
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x00150F1D File Offset: 0x0014F11D
	public int GetDefaultLayerMask()
	{
		return this.defaultLayerMask;
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x00150F25 File Offset: 0x0014F125
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06003CD0 RID: 15568 RVA: 0x00150F3C File Offset: 0x0014F13C
	public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		if (selectable != null)
		{
			pos = selectable.transform.GetPosition();
		}
		pos.z = -40f;
		pos += offset;
		WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
		if (worldFromPosition != null)
		{
			CameraController.Instance.ActiveWorldStarWipe(worldFromPosition.id, pos, 10f, null);
			return;
		}
		DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x00150FAB File Offset: 0x0014F1AB
	public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		this.Focus(pos, selectable, offset);
		this.Select(selectable, false);
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x00150FBE File Offset: 0x0014F1BE
	public void SelectAndFocus(Vector3 pos, KSelectable selectable)
	{
		this.SelectAndFocus(pos, selectable, Vector3.zero);
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x00150FCD File Offset: 0x0014F1CD
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x00150FFB File Offset: 0x0014F1FB
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x00151018 File Offset: 0x0014F218
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.previousSelection)
		{
			return;
		}
		this.previousSelection = new_selected;
		if (this.selected != null)
		{
			this.selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			SelectToolHoverTextCard component = base.GetComponent<SelectToolHoverTextCard>();
			if (component != null)
			{
				int num = component.currentSelectedSelectableIndex;
				int recentNumberOfDisplayedSelectables = component.recentNumberOfDisplayedSelectables;
				if (recentNumberOfDisplayedSelectables != 0)
				{
					num = (num + 1) % recentNumberOfDisplayedSelectables;
					if (!skipSound)
					{
						if (recentNumberOfDisplayedSelectables == 1)
						{
							KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty", false));
						}
						else
						{
							EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero, 1f);
							instance.setParameterByName("selection", (float)num, false);
							SoundEvent.EndOneShot(instance);
						}
						this.playedSoundThisFrame = true;
					}
				}
			}
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
			this.selectMarker.SetTargetTransform(gameObject.transform);
			this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
		}
		else if (this.selectMarker != null)
		{
			this.selectMarker.gameObject.SetActive(false);
		}
		this.selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x00151184 File Offset: 0x0014F384
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		KSelectable objectUnderCursor = base.GetObjectUnderCursor<KSelectable>(true, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, this.selected);
		this.selectedCell = Grid.PosToCell(cursor_pos);
		this.Select(objectUnderCursor, false);
		if (DevToolSimDebug.Instance != null)
		{
			DevToolSimDebug.Instance.SetCell(this.selectedCell);
		}
		if (DevToolNavGrid.Instance != null)
		{
			DevToolNavGrid.Instance.SetCell(this.selectedCell);
		}
	}

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00151200 File Offset: 0x0014F400
	public int GetSelectedCell()
	{
		return this.selectedCell;
	}

	// Token: 0x040027B8 RID: 10168
	public KSelectable selected;

	// Token: 0x040027B9 RID: 10169
	protected int cell_new;

	// Token: 0x040027BA RID: 10170
	private int selectedCell;

	// Token: 0x040027BB RID: 10171
	protected int defaultLayerMask;

	// Token: 0x040027BC RID: 10172
	public static SelectTool Instance;

	// Token: 0x040027BD RID: 10173
	private KSelectable delayedNextSelection;

	// Token: 0x040027BE RID: 10174
	private bool delayedSkipSound;

	// Token: 0x040027BF RID: 10175
	private KSelectable previousSelection;
}
