using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AB4 RID: 2740
public class ClusterMapHex : MultiToggle, ICanvasRaycastFilter
{
	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060053BE RID: 21438 RVA: 0x001E2C5C File Offset: 0x001E0E5C
	// (set) Token: 0x060053BF RID: 21439 RVA: 0x001E2C64 File Offset: 0x001E0E64
	public AxialI location { get; private set; }

	// Token: 0x060053C0 RID: 21440 RVA: 0x001E2C70 File Offset: 0x001E0E70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rectTransform = base.GetComponent<RectTransform>();
		this.onClick = new System.Action(this.TrySelect);
		this.onDoubleClick = new Func<bool>(this.TryGoTo);
		this.onEnter = new System.Action(this.OnHover);
		this.onExit = new System.Action(this.OnUnhover);
	}

	// Token: 0x060053C1 RID: 21441 RVA: 0x001E2CD7 File Offset: 0x001E0ED7
	public void SetLocation(AxialI location)
	{
		this.location = location;
	}

	// Token: 0x060053C2 RID: 21442 RVA: 0x001E2CE0 File Offset: 0x001E0EE0
	public void SetRevealed(ClusterRevealLevel level)
	{
		this._revealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			this.fogOfWar.gameObject.SetActive(true);
			this.peekedTile.gameObject.SetActive(false);
			return;
		case ClusterRevealLevel.Peeked:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(true);
			return;
		case ClusterRevealLevel.Visible:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x060053C3 RID: 21443 RVA: 0x001E2D6F File Offset: 0x001E0F6F
	public void SetDestinationStatus(string fail_reason)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x001E2DA4 File Offset: 0x001E0FA4
	public void SetDestinationStatus(string fail_reason, int pathLength, int rocketRange, bool repeat)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		if (pathLength > 0)
		{
			string text = repeat ? UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH_RETURN : UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH;
			if (repeat)
			{
				pathLength *= 2;
			}
			text = string.Format(text, pathLength, GameUtil.GetFormattedRocketRange((float)rocketRange, GameUtil.TimeSlice.None, true));
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x001E2E30 File Offset: 0x001E1030
	public void UpdateToggleState(ClusterMapHex.ToggleState state)
	{
		int new_state_index = -1;
		switch (state)
		{
		case ClusterMapHex.ToggleState.Unselected:
			new_state_index = 0;
			break;
		case ClusterMapHex.ToggleState.Selected:
			new_state_index = 1;
			break;
		case ClusterMapHex.ToggleState.OrbitHighlight:
			new_state_index = 2;
			break;
		}
		base.ChangeState(new_state_index);
	}

	// Token: 0x060053C6 RID: 21446 RVA: 0x001E2E64 File Offset: 0x001E1064
	private void TrySelect()
	{
		if (DebugHandler.InstantBuildMode)
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.location, 0);
		}
		ClusterMapScreen.Instance.SelectHex(this);
	}

	// Token: 0x060053C7 RID: 21447 RVA: 0x001E2E90 File Offset: 0x001E1090
	private bool TryGoTo()
	{
		List<WorldContainer> list = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(this.location)
		select entity.GetComponent<WorldContainer>() into x
		where x != null
		select x).ToList<WorldContainer>();
		if (list.Count == 1)
		{
			CameraController.Instance.ActiveWorldStarWipe(list[0].id, null);
			return true;
		}
		return false;
	}

	// Token: 0x060053C8 RID: 21448 RVA: 0x001E2F20 File Offset: 0x001E1120
	private void OnHover()
	{
		this.m_tooltip.ClearMultiStringTooltip();
		string text = "";
		switch (this._revealLevel)
		{
		case ClusterRevealLevel.Hidden:
			text = UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX;
			break;
		case ClusterRevealLevel.Peeked:
		{
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.Asteroid);
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell2 = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.POI);
			text = ((hiddenEntitiesOfLayerAtCell.Count > 0 || hiddenEntitiesOfLayerAtCell2.Count > 0) ? UI.CLUSTERMAP.TOOLTIP_PEEKED_HEX_WITH_OBJECT : UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX);
			break;
		}
		case ClusterRevealLevel.Visible:
			if (ClusterGrid.Instance.GetEntitiesOnCell(this.location).Count == 0)
			{
				text = UI.CLUSTERMAP.TOOLTIP_EMPTY_HEX;
			}
			break;
		}
		if (!text.IsNullOrWhiteSpace())
		{
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(true);
		ClusterMapScreen.Instance.OnHoverHex(this);
	}

	// Token: 0x060053C9 RID: 21449 RVA: 0x001E2FFC File Offset: 0x001E11FC
	private void OnUnhover()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.OnUnhoverHex(this);
		}
	}

	// Token: 0x060053CA RID: 21450 RVA: 0x001E3018 File Offset: 0x001E1218
	private void UpdateHoverColors(bool validDestination)
	{
		Color color_on_hover = validDestination ? this.hoverColorValid : this.hoverColorInvalid;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].color_on_hover = color_on_hover;
			for (int j = 0; j < this.states[i].additional_display_settings.Length; j++)
			{
				this.states[i].additional_display_settings[j].color_on_hover = color_on_hover;
			}
		}
		base.RefreshHoverColor();
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x001E30A0 File Offset: 0x001E12A0
	public bool IsRaycastLocationValid(Vector2 inputPoint, Camera eventCamera)
	{
		Vector2 vector = this.rectTransform.position;
		float num = Mathf.Abs(inputPoint.x - vector.x);
		float num2 = Mathf.Abs(inputPoint.y - vector.y);
		Vector2 vector2 = this.rectTransform.lossyScale;
		return num <= vector2.x && num2 <= vector2.y && vector2.y * vector2.x - vector2.y / 2f * num - vector2.x * num2 >= 0f;
	}

	// Token: 0x040037F9 RID: 14329
	private RectTransform rectTransform;

	// Token: 0x040037FA RID: 14330
	public Color hoverColorValid;

	// Token: 0x040037FB RID: 14331
	public Color hoverColorInvalid;

	// Token: 0x040037FC RID: 14332
	public Image fogOfWar;

	// Token: 0x040037FD RID: 14333
	public Image peekedTile;

	// Token: 0x040037FE RID: 14334
	public TextStyleSetting invalidDestinationTooltipStyle;

	// Token: 0x040037FF RID: 14335
	public TextStyleSetting informationTooltipStyle;

	// Token: 0x04003800 RID: 14336
	[MyCmpGet]
	private ToolTip m_tooltip;

	// Token: 0x04003801 RID: 14337
	private ClusterRevealLevel _revealLevel;

	// Token: 0x020019C8 RID: 6600
	public enum ToggleState
	{
		// Token: 0x04007759 RID: 30553
		Unselected,
		// Token: 0x0400775A RID: 30554
		Selected,
		// Token: 0x0400775B RID: 30555
		OrbitHighlight
	}
}
