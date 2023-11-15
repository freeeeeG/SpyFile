using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C7E RID: 3198
public class TopLeftControlScreen : KScreen
{
	// Token: 0x060065FA RID: 26106 RVA: 0x00260F07 File Offset: 0x0025F107
	public static void DestroyInstance()
	{
		TopLeftControlScreen.Instance = null;
	}

	// Token: 0x060065FB RID: 26107 RVA: 0x00260F10 File Offset: 0x0025F110
	protected override void OnActivate()
	{
		base.OnActivate();
		TopLeftControlScreen.Instance = this;
		this.RefreshName();
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
		this.UpdateSandboxToggleState();
		MultiToggle multiToggle = this.sandboxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickSandboxToggle));
		MultiToggle multiToggle2 = this.kleiItemDropButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.OnClickKleiItemDropButton));
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(new System.Action(this.RefreshKleiItemDropButton));
		this.RefreshKleiItemDropButton();
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.UpdateSandboxToggleState();
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.secondaryRow);
	}

	// Token: 0x060065FC RID: 26108 RVA: 0x00260FDD File Offset: 0x0025F1DD
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x060065FD RID: 26109 RVA: 0x00260FFB File Offset: 0x0025F1FB
	public void RefreshName()
	{
		if (SaveGame.Instance != null)
		{
			this.locText.text = SaveGame.Instance.BaseName;
		}
	}

	// Token: 0x060065FE RID: 26110 RVA: 0x00261020 File Offset: 0x0025F220
	public void ResetToolTip()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			return;
		}
		this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
	}

	// Token: 0x060065FF RID: 26111 RVA: 0x00261080 File Offset: 0x0025F280
	public void UpdateSandboxToggleState()
	{
		if (this.CheckSandboxModeLocked())
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(0);
		}
		else
		{
			this.sandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString(UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, global::Action.ToggleSandboxTools));
			this.sandboxToggle.ChangeState(Game.Instance.SandboxModeActive ? 2 : 1);
		}
		this.sandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
	}

	// Token: 0x06006600 RID: 26112 RVA: 0x00261120 File Offset: 0x0025F320
	private void OnClickSandboxToggle()
	{
		if (this.CheckSandboxModeLocked())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		else
		{
			Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
			KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
		}
		this.UpdateSandboxToggleState();
	}

	// Token: 0x06006601 RID: 26113 RVA: 0x00261190 File Offset: 0x0025F390
	private void RefreshKleiItemDropButton()
	{
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ERROR_NO_ITEMS);
			this.kleiItemDropButton.ChangeState(1);
			return;
		}
		this.kleiItemDropButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.ITEM_DROP_SCREEN.IN_GAME_BUTTON.TOOLTIP_ITEMS_AVAILABLE);
		this.kleiItemDropButton.ChangeState(2);
	}

	// Token: 0x06006602 RID: 26114 RVA: 0x002611F1 File Offset: 0x0025F3F1
	private void OnClickKleiItemDropButton()
	{
		this.RefreshKleiItemDropButton();
		if (!KleiItemDropScreen.HasItemsToShow())
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
	}

	// Token: 0x06006603 RID: 26115 RVA: 0x0026122D File Offset: 0x0025F42D
	private bool CheckSandboxModeLocked()
	{
		return !SaveGame.Instance.sandboxEnabled;
	}

	// Token: 0x0400463E RID: 17982
	public static TopLeftControlScreen Instance;

	// Token: 0x0400463F RID: 17983
	[SerializeField]
	private MultiToggle sandboxToggle;

	// Token: 0x04004640 RID: 17984
	[SerializeField]
	private MultiToggle kleiItemDropButton;

	// Token: 0x04004641 RID: 17985
	[SerializeField]
	private LocText locText;

	// Token: 0x04004642 RID: 17986
	[SerializeField]
	private RectTransform secondaryRow;

	// Token: 0x02001BB1 RID: 7089
	private enum MultiToggleState
	{
		// Token: 0x04007D95 RID: 32149
		Disabled,
		// Token: 0x04007D96 RID: 32150
		Off,
		// Token: 0x04007D97 RID: 32151
		On
	}
}
