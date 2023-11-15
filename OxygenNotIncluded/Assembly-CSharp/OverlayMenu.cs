using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A73 RID: 2675
public class OverlayMenu : KIconToggleMenu
{
	// Token: 0x06005139 RID: 20793 RVA: 0x001CDFFC File Offset: 0x001CC1FC
	public static void DestroyInstance()
	{
		OverlayMenu.Instance = null;
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x001CE004 File Offset: 0x001CC204
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		OverlayMenu.Instance = this;
		this.InitializeToggles();
		base.Setup(this.overlayToggleInfos);
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		KInputManager.InputChange.AddListener(new UnityAction(this.Refresh));
		base.onSelect += this.OnToggleSelect;
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x001CE08F File Offset: 0x001CC28F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshButtons();
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x001CE09D File Offset: 0x001CC29D
	public void Refresh()
	{
		this.RefreshButtons();
	}

	// Token: 0x0600513D RID: 20797 RVA: 0x001CE0A8 File Offset: 0x001CC2A8
	protected override void RefreshButtons()
	{
		base.RefreshButtons();
		if (Research.Instance == null)
		{
			return;
		}
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.overlayToggleInfos)
		{
			OverlayMenu.OverlayToggleInfo overlayToggleInfo = (OverlayMenu.OverlayToggleInfo)toggleInfo;
			toggleInfo.toggle.gameObject.SetActive(overlayToggleInfo.IsUnlocked());
			toggleInfo.tooltip = GameUtil.ReplaceHotkeyString(overlayToggleInfo.originalToolTipText, toggleInfo.hotKey);
		}
	}

	// Token: 0x0600513E RID: 20798 RVA: 0x001CE13C File Offset: 0x001CC33C
	private void OnResearchComplete(object data)
	{
		this.RefreshButtons();
	}

	// Token: 0x0600513F RID: 20799 RVA: 0x001CE144 File Offset: 0x001CC344
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.Refresh));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x001CE162 File Offset: 0x001CC362
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
	}

	// Token: 0x06005141 RID: 20801 RVA: 0x001CE185 File Offset: 0x001CC385
	private void InitializeToggleGroups()
	{
	}

	// Token: 0x06005142 RID: 20802 RVA: 0x001CE188 File Offset: 0x001CC388
	private void InitializeToggles()
	{
		this.overlayToggleInfos = new List<KIconToggleMenu.ToggleInfo>
		{
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.OXYGEN.BUTTON, "overlay_oxygen", OverlayModes.Oxygen.ID, "", global::Action.Overlay1, UI.TOOLTIPS.OXYGENOVERLAYSTRING, UI.OVERLAYS.OXYGEN.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ELECTRICAL.BUTTON, "overlay_power", OverlayModes.Power.ID, "", global::Action.Overlay2, UI.TOOLTIPS.POWEROVERLAYSTRING, UI.OVERLAYS.ELECTRICAL.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TEMPERATURE.BUTTON, "overlay_temperature", OverlayModes.Temperature.ID, "", global::Action.Overlay3, UI.TOOLTIPS.TEMPERATUREOVERLAYSTRING, UI.OVERLAYS.TEMPERATURE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TILEMODE.BUTTON, "overlay_materials", OverlayModes.TileMode.ID, "", global::Action.Overlay4, UI.TOOLTIPS.TILEMODE_OVERLAY_STRING, UI.OVERLAYS.TILEMODE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIGHTING.BUTTON, "overlay_lights", OverlayModes.Light.ID, "", global::Action.Overlay5, UI.TOOLTIPS.LIGHTSOVERLAYSTRING, UI.OVERLAYS.LIGHTING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIQUIDPLUMBING.BUTTON, "overlay_liquidvent", OverlayModes.LiquidConduits.ID, "", global::Action.Overlay6, UI.TOOLTIPS.LIQUIDVENTOVERLAYSTRING, UI.OVERLAYS.LIQUIDPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.GASPLUMBING.BUTTON, "overlay_gasvent", OverlayModes.GasConduits.ID, "", global::Action.Overlay7, UI.TOOLTIPS.GASVENTOVERLAYSTRING, UI.OVERLAYS.GASPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DECOR.BUTTON, "overlay_decor", OverlayModes.Decor.ID, "", global::Action.Overlay8, UI.TOOLTIPS.DECOROVERLAYSTRING, UI.OVERLAYS.DECOR.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DISEASE.BUTTON, "overlay_disease", OverlayModes.Disease.ID, "", global::Action.Overlay9, UI.TOOLTIPS.DISEASEOVERLAYSTRING, UI.OVERLAYS.DISEASE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CROPS.BUTTON, "overlay_farming", OverlayModes.Crop.ID, "", global::Action.Overlay10, UI.TOOLTIPS.CROPS_OVERLAY_STRING, UI.OVERLAYS.CROPS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ROOMS.BUTTON, "overlay_rooms", OverlayModes.Rooms.ID, "", global::Action.Overlay11, UI.TOOLTIPS.ROOMSOVERLAYSTRING, UI.OVERLAYS.ROOMS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.SUIT.BUTTON, "overlay_suit", OverlayModes.Suit.ID, "SuitsOverlay", global::Action.Overlay12, UI.TOOLTIPS.SUITOVERLAYSTRING, UI.OVERLAYS.SUIT.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LOGIC.BUTTON, "overlay_logic", OverlayModes.Logic.ID, "AutomationOverlay", global::Action.Overlay13, UI.TOOLTIPS.LOGICOVERLAYSTRING, UI.OVERLAYS.LOGIC.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CONVEYOR.BUTTON, "overlay_conveyor", OverlayModes.SolidConveyor.ID, "ConveyorOverlay", global::Action.Overlay14, UI.TOOLTIPS.CONVEYOR_OVERLAY_STRING, UI.OVERLAYS.CONVEYOR.BUTTON)
		};
		if (Sim.IsRadiationEnabled())
		{
			this.overlayToggleInfos.Add(new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.RADIATION.BUTTON, "overlay_radiation", OverlayModes.Radiation.ID, "", global::Action.Overlay15, UI.TOOLTIPS.RADIATIONOVERLAYSTRING, UI.OVERLAYS.RADIATION.BUTTON));
		}
	}

	// Token: 0x06005143 RID: 20803 RVA: 0x001CE528 File Offset: 0x001CC728
	private void OnToggleSelect(KIconToggleMenu.ToggleInfo toggle_info)
	{
		if (SimDebugView.Instance.GetMode() == ((OverlayMenu.OverlayToggleInfo)toggle_info).simView)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			return;
		}
		if (((OverlayMenu.OverlayToggleInfo)toggle_info).IsUnlocked())
		{
			OverlayScreen.Instance.ToggleOverlay(((OverlayMenu.OverlayToggleInfo)toggle_info).simView, true);
		}
	}

	// Token: 0x06005144 RID: 20804 RVA: 0x001CE588 File Offset: 0x001CC788
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString y = (HashedString)overlay_data;
		for (int i = 0; i < this.overlayToggleInfos.Count; i++)
		{
			this.overlayToggleInfos[i].toggle.isOn = (((OverlayMenu.OverlayToggleInfo)this.overlayToggleInfos[i]).simView == y);
		}
	}

	// Token: 0x06005145 RID: 20805 RVA: 0x001CE5E4 File Offset: 0x001CC7E4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && e.TryConsume(global::Action.Escape))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005146 RID: 20806 RVA: 0x001CE638 File Offset: 0x001CC838
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x04003530 RID: 13616
	public static OverlayMenu Instance;

	// Token: 0x04003531 RID: 13617
	private List<KIconToggleMenu.ToggleInfo> overlayToggleInfos;

	// Token: 0x04003532 RID: 13618
	private UnityAction inputChangeReceiver;

	// Token: 0x02001913 RID: 6419
	private class OverlayToggleGroup : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x060093A8 RID: 37800 RVA: 0x003300C9 File Offset: 0x0032E2C9
		public OverlayToggleGroup(string text, string icon_name, List<OverlayMenu.OverlayToggleInfo> toggle_group, string required_tech_item = "", global::Action hot_key = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hot_key, tooltip, tooltip_header)
		{
			this.toggleInfoGroup = toggle_group;
		}

		// Token: 0x060093A9 RID: 37801 RVA: 0x003300E1 File Offset: 0x0032E2E1
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
		}

		// Token: 0x060093AA RID: 37802 RVA: 0x0033010E File Offset: 0x0032E30E
		public OverlayMenu.OverlayToggleInfo GetActiveToggleInfo()
		{
			return this.toggleInfoGroup[this.activeToggleInfo];
		}

		// Token: 0x04007409 RID: 29705
		public List<OverlayMenu.OverlayToggleInfo> toggleInfoGroup;

		// Token: 0x0400740A RID: 29706
		public string requiredTechItem;

		// Token: 0x0400740B RID: 29707
		[SerializeField]
		private int activeToggleInfo;
	}

	// Token: 0x02001914 RID: 6420
	private class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x060093AB RID: 37803 RVA: 0x00330121 File Offset: 0x0032E321
		public OverlayToggleInfo(string text, string icon_name, HashedString sim_view, string required_tech_item = "", global::Action hotKey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hotKey, tooltip, tooltip_header)
		{
			this.originalToolTipText = tooltip;
			tooltip = GameUtil.ReplaceHotkeyString(tooltip, hotKey);
			this.simView = sim_view;
			this.requiredTechItem = required_tech_item;
		}

		// Token: 0x060093AC RID: 37804 RVA: 0x00330154 File Offset: 0x0032E354
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
		}

		// Token: 0x0400740C RID: 29708
		public HashedString simView;

		// Token: 0x0400740D RID: 29709
		public string requiredTechItem;

		// Token: 0x0400740E RID: 29710
		public string originalToolTipText;
	}
}
