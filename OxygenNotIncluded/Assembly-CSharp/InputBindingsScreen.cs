using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B24 RID: 2852
public class InputBindingsScreen : KModalScreen
{
	// Token: 0x060057BD RID: 22461 RVA: 0x0020143F File Offset: 0x001FF63F
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x00201442 File Offset: 0x001FF642
	private bool IsKeyDown(KeyCode key_code)
	{
		return Input.GetKey(key_code) || Input.GetKeyDown(key_code);
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x00201454 File Offset: 0x001FF654
	private string GetModifierString(Modifier modifiers)
	{
		string text = "";
		foreach (object obj in Enum.GetValues(typeof(Modifier)))
		{
			Modifier modifier = (Modifier)obj;
			if ((modifiers & modifier) != Modifier.None)
			{
				text = text + " + " + modifier.ToString();
			}
		}
		return text;
	}

	// Token: 0x060057C0 RID: 22464 RVA: 0x002014D4 File Offset: 0x001FF6D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entryPrefab.SetActive(false);
		this.prevScreenButton.onClick += this.OnPrevScreen;
		this.nextScreenButton.onClick += this.OnNextScreen;
	}

	// Token: 0x060057C1 RID: 22465 RVA: 0x00201524 File Offset: 0x001FF724
	protected override void OnActivate()
	{
		this.CollectScreens();
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		this.closeButton.onClick += this.OnBack;
		this.backButton.onClick += this.OnBack;
		this.resetButton.onClick += this.OnReset;
		this.BuildDisplay();
	}

	// Token: 0x060057C2 RID: 22466 RVA: 0x002015C0 File Offset: 0x001FF7C0
	private void CollectScreens()
	{
		this.screens.Clear();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mGroup != null && bindingEntry.mRebindable && !this.screens.Contains(bindingEntry.mGroup) && DlcManager.IsDlcListValidForCurrentContent(bindingEntry.dlcIds))
			{
				if (bindingEntry.mGroup == "Root")
				{
					this.activeScreen = this.screens.Count;
				}
				this.screens.Add(bindingEntry.mGroup);
			}
		}
	}

	// Token: 0x060057C3 RID: 22467 RVA: 0x0020165A File Offset: 0x001FF85A
	protected override void OnDeactivate()
	{
		GameInputMapping.SaveBindings();
		this.DestroyDisplay();
	}

	// Token: 0x060057C4 RID: 22468 RVA: 0x00201667 File Offset: 0x001FF867
	private LocString GetActionString(global::Action action)
	{
		return null;
	}

	// Token: 0x060057C5 RID: 22469 RVA: 0x0020166C File Offset: 0x001FF86C
	private string GetBindingText(BindingEntry binding)
	{
		string text = GameUtil.GetKeycodeLocalized(binding.mKeyCode);
		if (binding.mKeyCode != KKeyCode.LeftAlt && binding.mKeyCode != KKeyCode.RightAlt && binding.mKeyCode != KKeyCode.LeftControl && binding.mKeyCode != KKeyCode.RightControl && binding.mKeyCode != KKeyCode.LeftShift && binding.mKeyCode != KKeyCode.RightShift)
		{
			text += this.GetModifierString(binding.mModifier);
		}
		return text;
	}

	// Token: 0x060057C6 RID: 22470 RVA: 0x002016E8 File Offset: 0x001FF8E8
	private void BuildDisplay()
	{
		string text = this.screens[this.activeScreen];
		string key = "STRINGS.INPUT_BINDINGS." + text.ToUpper() + ".NAME";
		this.screenTitle.text = Strings.Get(key);
		if (this.entryPool == null)
		{
			this.entryPool = new UIPool<HorizontalLayoutGroup>(this.entryPrefab.GetComponent<HorizontalLayoutGroup>());
		}
		this.DestroyDisplay();
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry binding = GameInputMapping.KeyBindings[i];
			if (binding.mGroup == this.screens[this.activeScreen] && binding.mRebindable && DlcManager.IsDlcListValidForCurrentContent(binding.dlcIds))
			{
				GameObject gameObject = this.entryPool.GetFreeElement(this.parent, true).gameObject;
				TMP_Text componentInChildren = gameObject.transform.GetChild(0).GetComponentInChildren<LocText>();
				string key2 = "STRINGS.INPUT_BINDINGS." + binding.mGroup.ToUpper() + "." + binding.mAction.ToString().ToUpper();
				componentInChildren.text = Strings.Get(key2);
				LocText key_label = gameObject.transform.GetChild(1).GetComponentInChildren<LocText>();
				key_label.text = this.GetBindingText(binding);
				KButton button = gameObject.GetComponentInChildren<KButton>();
				button.onClick += delegate()
				{
					this.waitingForKeyPress = true;
					this.actionToRebind = binding.mAction;
					this.ignoreRootConflicts = binding.mIgnoreRootConflics;
					this.activeButton = button;
					key_label.text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.WAITING_FOR_INPUT;
				};
				gameObject.transform.SetSiblingIndex(num);
				num++;
			}
		}
	}

	// Token: 0x060057C7 RID: 22471 RVA: 0x002018C4 File Offset: 0x001FFAC4
	private void DestroyDisplay()
	{
		this.entryPool.ClearAll();
	}

	// Token: 0x060057C8 RID: 22472 RVA: 0x002018D4 File Offset: 0x001FFAD4
	private void Update()
	{
		if (this.waitingForKeyPress)
		{
			Modifier modifier = Modifier.None;
			modifier |= ((this.IsKeyDown(KeyCode.LeftAlt) || this.IsKeyDown(KeyCode.RightAlt)) ? Modifier.Alt : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftControl) || this.IsKeyDown(KeyCode.RightControl)) ? Modifier.Ctrl : Modifier.None);
			modifier |= ((this.IsKeyDown(KeyCode.LeftShift) || this.IsKeyDown(KeyCode.RightShift)) ? Modifier.Shift : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.CapsLock) ? Modifier.CapsLock : Modifier.None);
			modifier |= (this.IsKeyDown(KeyCode.BackQuote) ? Modifier.Backtick : Modifier.None);
			bool flag = false;
			for (int i = 0; i < InputBindingsScreen.validKeys.Length; i++)
			{
				KeyCode keyCode = InputBindingsScreen.validKeys[i];
				if (Input.GetKeyDown(keyCode))
				{
					KKeyCode kkey_code = (KKeyCode)keyCode;
					this.Bind(kkey_code, modifier);
					flag = true;
				}
			}
			if (!flag)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				KKeyCode kkeyCode = KKeyCode.None;
				if (axis < 0f)
				{
					kkeyCode = KKeyCode.MouseScrollDown;
				}
				else if (axis > 0f)
				{
					kkeyCode = KKeyCode.MouseScrollUp;
				}
				if (kkeyCode != KKeyCode.None)
				{
					this.Bind(kkeyCode, modifier);
				}
			}
		}
	}

	// Token: 0x060057C9 RID: 22473 RVA: 0x002019EC File Offset: 0x001FFBEC
	private BindingEntry GetDuplicatedBinding(string activeScreen, BindingEntry new_binding)
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (new_binding.IsBindingEqual(bindingEntry) && (bindingEntry.mGroup == null || bindingEntry.mGroup == activeScreen || bindingEntry.mGroup == "Root" || activeScreen == "Root") && (!(activeScreen == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(bindingEntry.mGroup == "Root") || !new_binding.mIgnoreRootConflics))
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x00201A98 File Offset: 0x001FFC98
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.waitingForKeyPress)
		{
			e.Consumed = true;
			return;
		}
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x00201ACA File Offset: 0x001FFCCA
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x00201AD4 File Offset: 0x001FFCD4
	private void OnBack()
	{
		int num = this.NumUnboundActions();
		if (num == 0)
		{
			this.Deactivate();
			return;
		}
		string text;
		if (num == 1)
		{
			BindingEntry firstUnbound = this.GetFirstUnbound();
			text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.UNBOUND_ACTION, firstUnbound.mAction.ToString());
		}
		else
		{
			text = UI.FRONTEND.INPUT_BINDINGS_SCREEN.MULTIPLE_UNBOUND_ACTIONS;
		}
		this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
		this.confirmDialog.PopupConfirmDialog(text, delegate
		{
			this.Deactivate();
		}, delegate
		{
			this.confirmDialog.Deactivate();
		}, null, null, null, null, null, null);
		this.confirmDialog.gameObject.SetActive(true);
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x00201B90 File Offset: 0x001FFD90
	private int NumUnboundActions()
	{
		int num = 0;
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None && bindingEntry.mRebindable && (BuildMenu.UseHotkeyBuildMenu() || !bindingEntry.mIgnoreRootConflics))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x00201BE4 File Offset: 0x001FFDE4
	private BindingEntry GetFirstUnbound()
	{
		BindingEntry result = default(BindingEntry);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mKeyCode == KKeyCode.None)
			{
				result = bindingEntry;
				break;
			}
		}
		return result;
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x00201C24 File Offset: 0x001FFE24
	private void OnReset()
	{
		GameInputMapping.KeyBindings = (BindingEntry[])GameInputMapping.DefaultBindings.Clone();
		Global.GetInputManager().RebindControls();
		this.BuildDisplay();
	}

	// Token: 0x060057D0 RID: 22480 RVA: 0x00201C4A File Offset: 0x001FFE4A
	public void OnPrevScreen()
	{
		if (this.activeScreen > 0)
		{
			this.activeScreen--;
		}
		else
		{
			this.activeScreen = this.screens.Count - 1;
		}
		this.BuildDisplay();
	}

	// Token: 0x060057D1 RID: 22481 RVA: 0x00201C7E File Offset: 0x001FFE7E
	public void OnNextScreen()
	{
		if (this.activeScreen < this.screens.Count - 1)
		{
			this.activeScreen++;
		}
		else
		{
			this.activeScreen = 0;
		}
		this.BuildDisplay();
	}

	// Token: 0x060057D2 RID: 22482 RVA: 0x00201CB4 File Offset: 0x001FFEB4
	private void Bind(KKeyCode kkey_code, Modifier modifier)
	{
		BindingEntry bindingEntry = new BindingEntry(this.screens[this.activeScreen], GamepadButton.NumButtons, kkey_code, modifier, this.actionToRebind, true, this.ignoreRootConflicts);
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
			if (bindingEntry2.mRebindable && bindingEntry2.mAction == this.actionToRebind)
			{
				BindingEntry duplicatedBinding = this.GetDuplicatedBinding(this.screens[this.activeScreen], bindingEntry);
				bindingEntry.mButton = GameInputMapping.KeyBindings[i].mButton;
				GameInputMapping.KeyBindings[i] = bindingEntry;
				this.activeButton.GetComponentInChildren<LocText>().text = this.GetBindingText(bindingEntry);
				if (duplicatedBinding.mAction != global::Action.Invalid && duplicatedBinding.mAction != this.actionToRebind)
				{
					this.confirmDialog = Util.KInstantiateUI(this.confirmPrefab.gameObject, base.transform.gameObject, false).GetComponent<ConfirmDialogScreen>();
					string arg = Strings.Get("STRINGS.INPUT_BINDINGS." + duplicatedBinding.mGroup.ToUpper() + "." + duplicatedBinding.mAction.ToString().ToUpper());
					string bindingText = this.GetBindingText(duplicatedBinding);
					string text = string.Format(UI.FRONTEND.INPUT_BINDINGS_SCREEN.DUPLICATE, arg, bindingText);
					this.Unbind(duplicatedBinding.mAction);
					this.confirmDialog.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
					this.confirmDialog.gameObject.SetActive(true);
				}
				Global.GetInputManager().RebindControls();
				this.waitingForKeyPress = false;
				this.actionToRebind = global::Action.NumActions;
				this.activeButton = null;
				this.BuildDisplay();
				return;
			}
		}
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x00201E78 File Offset: 0x00200078
	private void Unbind(global::Action action)
	{
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			BindingEntry bindingEntry = GameInputMapping.KeyBindings[i];
			if (bindingEntry.mAction == action)
			{
				bindingEntry.mKeyCode = KKeyCode.None;
				bindingEntry.mModifier = Modifier.None;
				GameInputMapping.KeyBindings[i] = bindingEntry;
			}
		}
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x00201EED File Offset: 0x002000ED
	// Note: this type is marked as 'beforefieldinit'.
	static InputBindingsScreen()
	{
		KeyCode[] array = new KeyCode[111];
		RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.4522A529DBF1D30936B6BCC06D2E607CD76E3B0FB1C18D9DA2635843A2840CD7).FieldHandle);
		InputBindingsScreen.validKeys = array;
	}

	// Token: 0x04003B51 RID: 15185
	private const string ROOT_KEY = "STRINGS.INPUT_BINDINGS.";

	// Token: 0x04003B52 RID: 15186
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x04003B53 RID: 15187
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04003B54 RID: 15188
	public KButton backButton;

	// Token: 0x04003B55 RID: 15189
	public KButton resetButton;

	// Token: 0x04003B56 RID: 15190
	public KButton closeButton;

	// Token: 0x04003B57 RID: 15191
	public KButton prevScreenButton;

	// Token: 0x04003B58 RID: 15192
	public KButton nextScreenButton;

	// Token: 0x04003B59 RID: 15193
	private bool waitingForKeyPress;

	// Token: 0x04003B5A RID: 15194
	private global::Action actionToRebind = global::Action.NumActions;

	// Token: 0x04003B5B RID: 15195
	private bool ignoreRootConflicts;

	// Token: 0x04003B5C RID: 15196
	private KButton activeButton;

	// Token: 0x04003B5D RID: 15197
	[SerializeField]
	private LocText screenTitle;

	// Token: 0x04003B5E RID: 15198
	[SerializeField]
	private GameObject parent;

	// Token: 0x04003B5F RID: 15199
	[SerializeField]
	private GameObject entryPrefab;

	// Token: 0x04003B60 RID: 15200
	private ConfirmDialogScreen confirmDialog;

	// Token: 0x04003B61 RID: 15201
	private int activeScreen = -1;

	// Token: 0x04003B62 RID: 15202
	private List<string> screens = new List<string>();

	// Token: 0x04003B63 RID: 15203
	private UIPool<HorizontalLayoutGroup> entryPool;

	// Token: 0x04003B64 RID: 15204
	private static readonly KeyCode[] validKeys;
}
