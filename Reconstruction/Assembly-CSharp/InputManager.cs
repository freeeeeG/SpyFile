using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class InputManager : Singleton<InputManager>
{
	// Token: 0x0600072A RID: 1834 RVA: 0x00013A08 File Offset: 0x00011C08
	public KeyCode GetKeyForAction(KeyBindingActions keybindingAction)
	{
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			if (keybingdingCheck.keybingdingAction == keybindingAction)
			{
				return keybingdingCheck.keyCode;
			}
		}
		return KeyCode.None;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x00013A44 File Offset: 0x00011C44
	public bool GetKeyDown(KeyBindingActions key)
	{
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			if (keybingdingCheck.keybingdingAction == key)
			{
				return Input.GetKeyDown(keybingdingCheck.keyCode);
			}
		}
		return false;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x00013A88 File Offset: 0x00011C88
	public bool GetKeyUp(KeyBindingActions key)
	{
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			if (keybingdingCheck.keybingdingAction == key)
			{
				return Input.GetKeyUp(keybingdingCheck.keyCode);
			}
		}
		return false;
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x00013ACC File Offset: 0x00011CCC
	public bool GetKey(KeyBindingActions key)
	{
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			if (keybingdingCheck.keybingdingAction == key)
			{
				return Input.GetKey(keybingdingCheck.keyCode);
			}
		}
		return false;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x00013B10 File Offset: 0x00011D10
	public bool SetKeyBinding(KeyBindingActions action, KeyCode keycode)
	{
		Keybindings.KeybingdingCheck[] keybindingChecks = this.KeyBindings.keybindingChecks;
		for (int i = 0; i < keybindingChecks.Length; i++)
		{
			if (keybindingChecks[i].keyCode == keycode)
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("SAMEKEY"));
				return false;
			}
		}
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			if (keybingdingCheck.keybingdingAction == action)
			{
				keybingdingCheck.keyCode = keycode;
				return true;
			}
		}
		return true;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x00013B8C File Offset: 0x00011D8C
	public void ResetAllKeys()
	{
		foreach (Keybindings.KeybingdingCheck keybingdingCheck in this.KeyBindings.keybindingChecks)
		{
			keybingdingCheck.keyCode = keybingdingCheck.defaultKeycode;
		}
	}

	// Token: 0x0400038F RID: 911
	[SerializeField]
	private Keybindings KeyBindings;
}
