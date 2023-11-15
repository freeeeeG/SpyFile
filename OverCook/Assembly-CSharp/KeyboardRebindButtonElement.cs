using System;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

// Token: 0x02000ACF RID: 2767
public class KeyboardRebindButtonElement : KeyboardRebindElement
{
	// Token: 0x060037D8 RID: 14296 RVA: 0x00107544 File Offset: 0x00105944
	public override void SetBinding(Key key)
	{
		ControlPadInput.Button[] realButtons = PlayerInputLookup.GetRealButtons(this.m_ButtonID, this.m_Side);
		if (realButtons.Length > 0)
		{
			List<Key> list = new List<Key>();
			for (int i = 0; i < realButtons.Length; i++)
			{
				List<Key> bindings = PCPadInputProvider.GetBindings(realButtons[i], this.m_Side != PadSide.Both);
				if (bindings != null && bindings.Count > 0)
				{
					list.AddRange(bindings);
				}
			}
			list = list.Distinct<Key>().ToList<Key>();
			if (this.m_AllowSecondaryKey && list.Count == 1)
			{
				list = new List<Key>
				{
					list[0],
					key
				};
			}
			else
			{
				list = new List<Key>
				{
					key
				};
			}
			for (int j = 0; j < realButtons.Length; j++)
			{
				PCPadInputProvider.SetBindings(realButtons[j], list, this.m_Side != PadSide.Both);
			}
		}
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0010763C File Offset: 0x00105A3C
	public override void UnsetBinding(Key key)
	{
		ControlPadInput.Button[] realButtons = PlayerInputLookup.GetRealButtons(this.m_ButtonID, this.m_Side);
		for (int i = 0; i < realButtons.Length; i++)
		{
			List<Key> bindings = PCPadInputProvider.GetBindings(realButtons[i], this.m_Side != PadSide.Both);
			if (bindings != null && bindings.Count > 0)
			{
				int num = bindings.RemoveAll((Key x) => x == key);
				if (num > 0)
				{
					PCPadInputProvider.SetBindings(realButtons[i], bindings, this.m_Side != PadSide.Both);
				}
			}
		}
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x001076D4 File Offset: 0x00105AD4
	public override bool HasAnyBindings()
	{
		ControlPadInput.Button[] realButtons = PlayerInputLookup.GetRealButtons(this.m_ButtonID, this.m_Side);
		for (int i = 0; i < realButtons.Length; i++)
		{
			List<Key> bindings = PCPadInputProvider.GetBindings(realButtons[i], this.m_Side != PadSide.Both);
			if (bindings != null && bindings.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x00107734 File Offset: 0x00105B34
	public override void RefreshBindingText()
	{
		string keyBindingsText = string.Empty;
		ControlPadInput.Button[] realButtons = PlayerInputLookup.GetRealButtons(this.m_ButtonID, this.m_Side);
		if (realButtons.Length > 0)
		{
			List<Key> list = new List<Key>();
			for (int i = 0; i < realButtons.Length; i++)
			{
				List<Key> bindings = PCPadInputProvider.GetBindings(realButtons[i], this.m_Side != PadSide.Both);
				if (bindings != null)
				{
					list.AddRange(bindings);
				}
			}
			keyBindingsText = base.KeysToString(list);
		}
		base.SetKeyBindingsText(keyBindingsText);
	}

	// Token: 0x04002CB4 RID: 11444
	[SerializeField]
	private PlayerInputLookup.LogicalButtonID m_ButtonID;
}
