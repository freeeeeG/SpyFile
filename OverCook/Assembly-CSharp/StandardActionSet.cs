using System;
using System.Collections.Generic;
using InControl;

// Token: 0x020001F7 RID: 503
public class StandardActionSet : PlayerActionSet
{
	// Token: 0x06000871 RID: 2161 RVA: 0x00033604 File Offset: 0x00031A04
	private StandardActionSet()
	{
		this.m_buttons = (ControlPadInput.Button[])Enum.GetValues(typeof(ControlPadInput.Button));
		this.m_values = (ControlPadInput.Value[])Enum.GetValues(typeof(ControlPadInput.Value));
		this.ResetActions();
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x00033694 File Offset: 0x00031A94
	private void ResetActions()
	{
		base.ClearActions();
		this.EngagementButton = base.CreatePlayerAction("Engagment");
		this.ButtonActions.Clear();
		for (int i = 0; i < this.m_buttons.Length; i++)
		{
			PlayerAction value = base.CreatePlayerAction(this.m_buttons[i].ToString());
			this.ButtonActions.Add(this.m_buttons[i], value);
		}
		this.m_pveValueActions.Clear();
		this.m_nveValueActions.Clear();
		this.ValueActions.Clear();
		for (int j = 0; j < this.m_values.Length; j++)
		{
			PlayerAction playerAction = base.CreatePlayerAction(this.m_values[j].ToString() + " Plus");
			PlayerAction playerAction2 = base.CreatePlayerAction(this.m_values[j].ToString() + " Minus");
			this.m_pveValueActions.Add(this.m_values[j], playerAction);
			this.m_nveValueActions.Add(this.m_values[j], playerAction2);
			PlayerOneAxisAction value2 = base.CreateOneAxisPlayerAction(playerAction2, playerAction);
			this.ValueActions.Add(this.m_values[j], value2);
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000337E4 File Offset: 0x00031BE4
	public static StandardActionSet CreateForJoystick(InputDevice _device)
	{
		StandardActionSet standardActionSet = new StandardActionSet();
		standardActionSet.EngagementButton.AddDefaultBinding(InputControlType.Action1);
		standardActionSet.ButtonActions[ControlPadInput.Button.A].AddDefaultBinding(InputControlType.Action1);
		standardActionSet.ButtonActions[ControlPadInput.Button.B].AddDefaultBinding(InputControlType.Action2);
		standardActionSet.ButtonActions[ControlPadInput.Button.X].AddDefaultBinding(InputControlType.Action3);
		standardActionSet.ButtonActions[ControlPadInput.Button.Y].AddDefaultBinding(InputControlType.Action4);
		standardActionSet.ButtonActions[ControlPadInput.Button.LB].AddDefaultBinding(InputControlType.LeftBumper);
		standardActionSet.ButtonActions[ControlPadInput.Button.RB].AddDefaultBinding(InputControlType.RightBumper);
		standardActionSet.ButtonActions[ControlPadInput.Button.LTrigger].AddDefaultBinding(InputControlType.LeftTrigger);
		standardActionSet.ButtonActions[ControlPadInput.Button.RTrigger].AddDefaultBinding(InputControlType.RightTrigger);
		standardActionSet.ButtonActions[ControlPadInput.Button.DPadLeft].AddDefaultBinding(InputControlType.DPadLeft);
		standardActionSet.ButtonActions[ControlPadInput.Button.DPadRight].AddDefaultBinding(InputControlType.DPadRight);
		standardActionSet.ButtonActions[ControlPadInput.Button.DPadUp].AddDefaultBinding(InputControlType.DPadUp);
		standardActionSet.ButtonActions[ControlPadInput.Button.DPadDown].AddDefaultBinding(InputControlType.DPadDown);
		standardActionSet.ButtonActions[ControlPadInput.Button.Back].AddDefaultBinding(InputControlType.Back);
		standardActionSet.ButtonActions[ControlPadInput.Button.Start].AddDefaultBinding(InputControlType.Start);
		standardActionSet.ButtonActions[ControlPadInput.Button.Start].AddDefaultBinding(InputControlType.Options);
		standardActionSet.ButtonActions[ControlPadInput.Button.LeftAnalog].AddDefaultBinding(InputControlType.LeftStickButton);
		standardActionSet.ButtonActions[ControlPadInput.Button.RightAnalog].AddDefaultBinding(InputControlType.RightStickButton);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.DPadX].AddDefaultBinding(InputControlType.DPadRight);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.DPadX].AddDefaultBinding(InputControlType.DPadLeft);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.DPadY].AddDefaultBinding(InputControlType.DPadDown);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.DPadY].AddDefaultBinding(InputControlType.DPadUp);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.LStickX].AddDefaultBinding(InputControlType.LeftStickRight);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.LStickX].AddDefaultBinding(InputControlType.LeftStickLeft);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.LStickY].AddDefaultBinding(InputControlType.LeftStickDown);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.LStickY].AddDefaultBinding(InputControlType.LeftStickUp);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.RStickX].AddDefaultBinding(InputControlType.RightStickRight);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.RStickX].AddDefaultBinding(InputControlType.RightStickLeft);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.RStickY].AddDefaultBinding(InputControlType.RightStickDown);
		standardActionSet.m_nveValueActions[ControlPadInput.Value.RStickY].AddDefaultBinding(InputControlType.RightStickUp);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.LTrigger].AddDefaultBinding(InputControlType.LeftTrigger);
		standardActionSet.m_pveValueActions[ControlPadInput.Value.RTrigger].AddDefaultBinding(InputControlType.RightTrigger);
		standardActionSet.Device = _device;
		return standardActionSet;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x00033A5C File Offset: 0x00031E5C
	public static bool IsKeyboardSplit()
	{
		GameInputConfig baseInputConfig = PlayerInputLookup.GetBaseInputConfig();
		if (baseInputConfig == null)
		{
			return false;
		}
		for (int i = 0; i < baseInputConfig.m_playerConfigs.Length; i++)
		{
			GameInputConfig.ConfigEntry configEntry = baseInputConfig.m_playerConfigs[i];
			if (PCPadInputProvider.IsKeyboard(configEntry.Pad) && configEntry.Side != PadSide.Both)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x00033AB8 File Offset: 0x00031EB8
	private static void RefreshKeyboardActions(StandardActionSet actionSet, KeyboardBindings bindings, bool bForce)
	{
		bool isSplit = actionSet.m_isSplit;
		actionSet.m_isSplit = StandardActionSet.IsKeyboardSplit();
		if (bForce || isSplit != actionSet.m_isSplit)
		{
			actionSet.ResetActions();
			if (actionSet.m_isSplit)
			{
				StandardActionSet.ModifyForSplitKeyboard(actionSet, bindings);
			}
			else
			{
				StandardActionSet.ModifyForCombinedKeyboard(actionSet, bindings);
			}
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x00033B10 File Offset: 0x00031F10
	public static StandardActionSet CreateForKeyboard(KeyboardBindings bindings)
	{
		StandardActionSet actionSet = new StandardActionSet();
		PCPadInputProvider.OnUpdateKeyboardButtons = (CallbackBool)Delegate.Combine(PCPadInputProvider.OnUpdateKeyboardButtons, new CallbackBool(delegate(bool bForce)
		{
			StandardActionSet.RefreshKeyboardActions(actionSet, bindings, bForce);
		}));
		StandardActionSet.ModifyForCombinedKeyboard(actionSet, bindings);
		StandardActionSet.RefreshKeyboardActions(actionSet, bindings, false);
		return actionSet;
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x00033B7E File Offset: 0x00031F7E
	public static void ModifyForCombinedKeyboard(StandardActionSet actionSet, KeyboardBindings bindings)
	{
		StandardActionSet.SetBindings(actionSet, bindings.m_CombinedKeyboard);
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00033B8C File Offset: 0x00031F8C
	public static void ModifyForSplitKeyboard(StandardActionSet actionSet, KeyboardBindings bindings)
	{
		StandardActionSet.SetBindings(actionSet, bindings.m_SplitKeyboard);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00033B9C File Offset: 0x00031F9C
	private static void SetBindings(StandardActionSet actionSet, KeyboardBindingSet bindingSet)
	{
		actionSet.EngagementButton.AddDefaultBinding(new Key[]
		{
			Key.Space
		});
		foreach (KeyValuePair<ControlPadInput.Button, List<Key>> keyValuePair in bindingSet.m_ButtonBindings)
		{
			foreach (Key key in keyValuePair.Value)
			{
				actionSet.ButtonActions[keyValuePair.Key].AddDefaultBinding(new Key[]
				{
					key
				});
			}
		}
		foreach (KeyValuePair<ControlPadInput.Value, List<Key>> keyValuePair2 in bindingSet.m_PositiveValueBindings)
		{
			foreach (Key key2 in keyValuePair2.Value)
			{
				actionSet.m_pveValueActions[keyValuePair2.Key].AddDefaultBinding(new Key[]
				{
					key2
				});
			}
		}
		foreach (KeyValuePair<ControlPadInput.Value, List<Key>> keyValuePair3 in bindingSet.m_NegativeValueBindings)
		{
			foreach (Key key3 in keyValuePair3.Value)
			{
				actionSet.m_nveValueActions[keyValuePair3.Key].AddDefaultBinding(new Key[]
				{
					key3
				});
			}
		}
	}

	// Token: 0x04000711 RID: 1809
	private ControlPadInput.Button[] m_buttons;

	// Token: 0x04000712 RID: 1810
	private ControlPadInput.Value[] m_values;

	// Token: 0x04000713 RID: 1811
	public PlayerAction EngagementButton;

	// Token: 0x04000714 RID: 1812
	public Dictionary<ControlPadInput.Button, PlayerAction> ButtonActions = new Dictionary<ControlPadInput.Button, PlayerAction>(new StandardActionSet.ButtonComparer());

	// Token: 0x04000715 RID: 1813
	public Dictionary<ControlPadInput.Value, PlayerOneAxisAction> ValueActions = new Dictionary<ControlPadInput.Value, PlayerOneAxisAction>(new StandardActionSet.ValueComparer());

	// Token: 0x04000716 RID: 1814
	private Dictionary<ControlPadInput.Value, PlayerAction> m_pveValueActions = new Dictionary<ControlPadInput.Value, PlayerAction>(new StandardActionSet.ValueComparer());

	// Token: 0x04000717 RID: 1815
	private Dictionary<ControlPadInput.Value, PlayerAction> m_nveValueActions = new Dictionary<ControlPadInput.Value, PlayerAction>(new StandardActionSet.ValueComparer());

	// Token: 0x04000718 RID: 1816
	private bool m_isSplit;

	// Token: 0x020001F8 RID: 504
	private class ButtonComparer : IEqualityComparer<ControlPadInput.Button>
	{
		// Token: 0x0600087B RID: 2171 RVA: 0x00033DD4 File Offset: 0x000321D4
		public bool Equals(ControlPadInput.Button x, ControlPadInput.Button y)
		{
			return x == y;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00033DDA File Offset: 0x000321DA
		public int GetHashCode(ControlPadInput.Button obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x020001F9 RID: 505
	private class ValueComparer : IEqualityComparer<ControlPadInput.Value>
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x00033DE5 File Offset: 0x000321E5
		public bool Equals(ControlPadInput.Value x, ControlPadInput.Value y)
		{
			return x == y;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00033DEB File Offset: 0x000321EB
		public int GetHashCode(ControlPadInput.Value obj)
		{
			return (int)obj;
		}
	}
}
