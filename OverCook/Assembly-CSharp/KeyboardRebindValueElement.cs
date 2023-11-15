using System;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

// Token: 0x02000AD3 RID: 2771
public class KeyboardRebindValueElement : KeyboardRebindElement
{
	// Token: 0x06003808 RID: 14344 RVA: 0x00107E88 File Offset: 0x00106288
	public override void SetBinding(Key key)
	{
		ControlPadInput.Value[] realValues = this.GetRealValues();
		if (realValues.Length > 0)
		{
			List<Key> list = new List<Key>();
			for (int i = 0; i < realValues.Length; i++)
			{
				List<Key> bindings = PCPadInputProvider.GetBindings(realValues[i], this.m_Positive, this.m_Side != PadSide.Both);
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
			for (int j = 0; j < realValues.Length; j++)
			{
				PCPadInputProvider.SetBindings(realValues[j], list, this.m_Positive, this.m_Side != PadSide.Both);
			}
		}
	}

	// Token: 0x06003809 RID: 14345 RVA: 0x00107F84 File Offset: 0x00106384
	public override void UnsetBinding(Key key)
	{
		ControlPadInput.Value[] realValues = this.GetRealValues();
		for (int i = 0; i < realValues.Length; i++)
		{
			List<Key> bindings = PCPadInputProvider.GetBindings(realValues[i], this.m_Positive, this.m_Side != PadSide.Both);
			if (bindings != null && bindings.Count > 0)
			{
				int num = bindings.RemoveAll((Key x) => x == key);
				if (num > 0)
				{
					PCPadInputProvider.SetBindings(realValues[i], bindings, this.m_Positive, this.m_Side != PadSide.Both);
				}
			}
		}
	}

	// Token: 0x0600380A RID: 14346 RVA: 0x00108020 File Offset: 0x00106420
	public override bool HasAnyBindings()
	{
		ControlPadInput.Value[] realValues = this.GetRealValues();
		for (int i = 0; i < realValues.Length; i++)
		{
			List<Key> bindings = PCPadInputProvider.GetBindings(realValues[i], this.m_Positive, this.m_Side != PadSide.Both);
			if (bindings != null && bindings.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600380B RID: 14347 RVA: 0x00108078 File Offset: 0x00106478
	public override void RefreshBindingText()
	{
		string keyBindingsText = string.Empty;
		ControlPadInput.Value[] realValues = this.GetRealValues();
		if (realValues.Length > 0)
		{
			List<Key> list = new List<Key>();
			for (int i = 0; i < realValues.Length; i++)
			{
				List<Key> bindings = PCPadInputProvider.GetBindings(realValues[i], this.m_Positive, this.m_Side != PadSide.Both);
				if (bindings != null)
				{
					list.AddRange(bindings);
				}
			}
			keyBindingsText = base.KeysToString(list);
		}
		base.SetKeyBindingsText(keyBindingsText);
	}

	// Token: 0x0600380C RID: 14348 RVA: 0x001080F0 File Offset: 0x001064F0
	private ControlPadInput.Value[] GetRealValues()
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		PlayerGameInput playerGameInput = new PlayerGameInput(ControlPadInput.PadNum.One, this.m_Side, (this.m_Side != PadSide.Both) ? playerManager.SidedAmbiMapping : playerManager.UnsidedAmbiMapping);
		ControlPadInput.Value[] array = new ControlPadInput.Value[0];
		AmbiPadValue[] array2 = PlayerInputLookup.LogicalToAmbiValue(this.m_ValueID);
		if (array2 != null)
		{
			for (int i = 0; i < array2.Length; i++)
			{
				ControlPadInput.ValueIdentifier[] realValues = PlayerInputLookup.GetInputConfig().GetRealValues(playerGameInput, array2[i]);
				array = array.Union(realValues.ConvertAll((ControlPadInput.ValueIdentifier x) => x.value));
			}
			return array;
		}
		return new ControlPadInput.Value[0];
	}

	// Token: 0x04002CC3 RID: 11459
	[SerializeField]
	private PlayerInputLookup.LogicalValueID m_ValueID;

	// Token: 0x04002CC4 RID: 11460
	[SerializeField]
	private bool m_Positive;
}
