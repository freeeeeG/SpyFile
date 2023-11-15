using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InControl;

// Token: 0x020001F5 RID: 501
public class PCPadInputProvider : Singleton<PCPadInputProvider>, IGamepadInputProvider
{
	// Token: 0x06000852 RID: 2130 RVA: 0x000325F8 File Offset: 0x000309F8
	static PCPadInputProvider()
	{
		PCPadInputProvider.InitialiseBindings();
		GameDebugConfig debugConfig = GameUtils.GetDebugConfig();
		if (debugConfig.m_keyboardType == GameDebugConfig.KeyboardType.Actual)
		{
			PCPadInputProvider.m_allDevices.Add(StandardActionSet.CreateForKeyboard(PCPadInputProvider.m_UserKeyboardBindings));
		}
		for (int i = 0; i < InputManager.Devices.Count; i++)
		{
			PCPadInputProvider.m_allDevices.Add(StandardActionSet.CreateForJoystick(InputManager.Devices[i]));
		}
		if (PCPadInputProvider.<>f__mg$cache0 == null)
		{
			PCPadInputProvider.<>f__mg$cache0 = new Action<InputDevice>(PCPadInputProvider.OnDeviceAttached);
		}
		InputManager.OnDeviceAttached += PCPadInputProvider.<>f__mg$cache0;
		if (PCPadInputProvider.<>f__mg$cache1 == null)
		{
			PCPadInputProvider.<>f__mg$cache1 = new Action<InputDevice>(PCPadInputProvider.OnDeviceDetached);
		}
		InputManager.OnDeviceDetached += PCPadInputProvider.<>f__mg$cache1;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000326F0 File Offset: 0x00030AF0
	public static KeyboardBindingSet GetDefaultSplitKeyboardBindings()
	{
		return new KeyboardBindingSet
		{
			m_ButtonBindings = 
			{
				{
					ControlPadInput.Button.DPadDown,
					new List<Key>
					{
						Key.LeftShift
					}
				},
				{
					ControlPadInput.Button.DPadLeft,
					new List<Key>
					{
						Key.LeftControl
					}
				},
				{
					ControlPadInput.Button.DPadRight,
					new List<Key>
					{
						Key.LeftAlt
					}
				},
				{
					ControlPadInput.Button.DPadUp,
					new List<Key>
					{
						Key.E
					}
				},
				{
					ControlPadInput.Button.LeftAnalog,
					new List<Key>
					{
						Key.T
					}
				},
				{
					ControlPadInput.Button.LB,
					new List<Key>
					{
						Key.LeftShift
					}
				},
				{
					ControlPadInput.Button.LTrigger,
					new List<Key>
					{
						Key.LeftControl
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.LStickX,
					new List<Key>
					{
						Key.D
					}
				}
			},
			m_NegativeValueBindings = 
			{
				{
					ControlPadInput.Value.LStickX,
					new List<Key>
					{
						Key.A
					}
				},
				{
					ControlPadInput.Value.LStickY,
					new List<Key>
					{
						Key.W
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.LStickY,
					new List<Key>
					{
						Key.S
					}
				}
			},
			m_ButtonBindings = 
			{
				{
					ControlPadInput.Button.A,
					new List<Key>
					{
						Key.RightShift
					}
				},
				{
					ControlPadInput.Button.B,
					new List<Key>
					{
						Key.RightAlt
					}
				},
				{
					ControlPadInput.Button.X,
					new List<Key>
					{
						Key.RightControl
					}
				},
				{
					ControlPadInput.Button.Y,
					new List<Key>
					{
						Key.I
					}
				},
				{
					ControlPadInput.Button.RightAnalog,
					new List<Key>
					{
						Key.P
					}
				},
				{
					ControlPadInput.Button.RB,
					new List<Key>
					{
						Key.RightShift
					}
				},
				{
					ControlPadInput.Button.RTrigger,
					new List<Key>
					{
						Key.RightControl
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.RStickX,
					new List<Key>
					{
						Key.RightArrow
					}
				}
			},
			m_NegativeValueBindings = 
			{
				{
					ControlPadInput.Value.RStickX,
					new List<Key>
					{
						Key.LeftArrow
					}
				},
				{
					ControlPadInput.Value.RStickY,
					new List<Key>
					{
						Key.UpArrow
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.RStickY,
					new List<Key>
					{
						Key.DownArrow
					}
				}
			},
			m_NegativeValueBindings = 
			{
				{
					ControlPadInput.Value.DPadY,
					new List<Key>
					{
						Key.UpArrow
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.DPadY,
					new List<Key>
					{
						Key.DownArrow
					}
				}
			},
			m_ButtonBindings = 
			{
				{
					ControlPadInput.Button.Back,
					new List<Key>
					{
						Key.Tab
					}
				},
				{
					ControlPadInput.Button.Start,
					new List<Key>
					{
						Key.PadEnter
					}
				}
			}
		};
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000329C8 File Offset: 0x00030DC8
	public static KeyboardBindingSet GetDefaultCombinedKeyboardBindings()
	{
		return new KeyboardBindingSet
		{
			m_ButtonBindings = 
			{
				{
					ControlPadInput.Button.A,
					new List<Key>
					{
						Key.Space
					}
				},
				{
					ControlPadInput.Button.B,
					new List<Key>
					{
						Key.LeftAlt
					}
				},
				{
					ControlPadInput.Button.X,
					new List<Key>
					{
						Key.LeftControl
					}
				},
				{
					ControlPadInput.Button.Y,
					new List<Key>
					{
						Key.E
					}
				},
				{
					ControlPadInput.Button.LB,
					new List<Key>
					{
						Key.LeftShift
					}
				},
				{
					ControlPadInput.Button.RB,
					new List<Key>
					{
						Key.RightShift
					}
				},
				{
					ControlPadInput.Button.Start,
					new List<Key>
					{
						Key.PadEnter
					}
				},
				{
					ControlPadInput.Button.LeftAnalog,
					new List<Key>
					{
						Key.T
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.LStickX,
					new List<Key>
					{
						Key.D,
						Key.RightArrow
					}
				}
			},
			m_NegativeValueBindings = 
			{
				{
					ControlPadInput.Value.LStickX,
					new List<Key>
					{
						Key.A,
						Key.LeftArrow
					}
				},
				{
					ControlPadInput.Value.LStickY,
					new List<Key>
					{
						Key.W,
						Key.UpArrow
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.LStickY,
					new List<Key>
					{
						Key.S,
						Key.DownArrow
					}
				}
			},
			m_NegativeValueBindings = 
			{
				{
					ControlPadInput.Value.DPadY,
					new List<Key>
					{
						Key.UpArrow
					}
				}
			},
			m_PositiveValueBindings = 
			{
				{
					ControlPadInput.Value.DPadY,
					new List<Key>
					{
						Key.DownArrow
					}
				}
			}
		};
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00032B78 File Offset: 0x00030F78
	public static void InitialiseBindings()
	{
		PCPadInputProvider.m_DefaultKeyboardBindings = new KeyboardBindings("DefaultKeyBindings");
		PCPadInputProvider.m_DefaultKeyboardBindings.m_SplitKeyboard = PCPadInputProvider.GetDefaultSplitKeyboardBindings();
		PCPadInputProvider.m_DefaultKeyboardBindings.m_CombinedKeyboard = PCPadInputProvider.GetDefaultCombinedKeyboardBindings();
		PCPadInputProvider.m_UserKeyboardBindings = new KeyboardBindings("UserKeyBindings");
		PCPadInputProvider.m_UserKeyboardBindings.CopyFrom(PCPadInputProvider.m_DefaultKeyboardBindings);
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00032BD0 File Offset: 0x00030FD0
	public static void SaveBindings(GlobalSave saveData)
	{
		if (saveData != null)
		{
			PCPadInputProvider.m_UserKeyboardBindings.Save(saveData);
		}
		PCPadInputProvider.UpdateKeyboardButtons(true);
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00032BE9 File Offset: 0x00030FE9
	public static void LoadBindings(GlobalSave saveData)
	{
		if (saveData != null)
		{
			if (!PCPadInputProvider.m_UserKeyboardBindings.Load(saveData))
			{
				PCPadInputProvider.RestoreDefaultBindings();
				return;
			}
			PCPadInputProvider.UpdateKeyboardButtons(true);
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00032C0D File Offset: 0x0003100D
	public static void RestoreDefaultBindings()
	{
		PCPadInputProvider.m_UserKeyboardBindings.CopyFrom(PCPadInputProvider.m_DefaultKeyboardBindings);
		PCPadInputProvider.UpdateKeyboardButtons(true);
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00032C24 File Offset: 0x00031024
	public static void RestoreDefaultCombinedBindings()
	{
		PCPadInputProvider.m_UserKeyboardBindings.m_CombinedKeyboard.CopyFrom(PCPadInputProvider.m_DefaultKeyboardBindings.m_CombinedKeyboard);
		PCPadInputProvider.UpdateKeyboardButtons(true);
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00032C45 File Offset: 0x00031045
	public static void RestoreDefaultSplitBindings()
	{
		PCPadInputProvider.m_UserKeyboardBindings.m_SplitKeyboard.CopyFrom(PCPadInputProvider.m_DefaultKeyboardBindings.m_SplitKeyboard);
		PCPadInputProvider.UpdateKeyboardButtons(true);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00032C68 File Offset: 0x00031068
	public static List<Key> GetBindings(ControlPadInput.Button button, bool bSplit)
	{
		KeyboardBindingSet keyboardBindingSet = (!bSplit) ? PCPadInputProvider.m_UserKeyboardBindings.m_CombinedKeyboard : PCPadInputProvider.m_UserKeyboardBindings.m_SplitKeyboard;
		if (keyboardBindingSet.m_ButtonBindings.ContainsKey(button))
		{
			return keyboardBindingSet.m_ButtonBindings[button];
		}
		return null;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00032CB4 File Offset: 0x000310B4
	public static void SetBindings(ControlPadInput.Button button, List<Key> keys, bool bSplit)
	{
		KeyboardBindingSet keyboardBindingSet = (!bSplit) ? PCPadInputProvider.m_UserKeyboardBindings.m_CombinedKeyboard : PCPadInputProvider.m_UserKeyboardBindings.m_SplitKeyboard;
		if (keyboardBindingSet.m_ButtonBindings.ContainsKey(button))
		{
			keyboardBindingSet.m_ButtonBindings[button] = keys;
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00032D00 File Offset: 0x00031100
	public static List<Key> GetBindings(ControlPadInput.Value value, bool bPositive, bool bSplit)
	{
		KeyboardBindingSet keyboardBindingSet = (!bSplit) ? PCPadInputProvider.m_UserKeyboardBindings.m_CombinedKeyboard : PCPadInputProvider.m_UserKeyboardBindings.m_SplitKeyboard;
		Dictionary<ControlPadInput.Value, List<Key>> dictionary = (!bPositive) ? keyboardBindingSet.m_NegativeValueBindings : keyboardBindingSet.m_PositiveValueBindings;
		if (dictionary.ContainsKey(value))
		{
			return dictionary[value];
		}
		return null;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00032D5C File Offset: 0x0003115C
	public static void SetBindings(ControlPadInput.Value value, List<Key> keys, bool bPositive, bool bSplit)
	{
		KeyboardBindingSet keyboardBindingSet = (!bSplit) ? PCPadInputProvider.m_UserKeyboardBindings.m_CombinedKeyboard : PCPadInputProvider.m_UserKeyboardBindings.m_SplitKeyboard;
		Dictionary<ControlPadInput.Value, List<Key>> dictionary = (!bPositive) ? keyboardBindingSet.m_NegativeValueBindings : keyboardBindingSet.m_PositiveValueBindings;
		if (dictionary.ContainsKey(value))
		{
			dictionary[value] = keys;
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00032DB5 File Offset: 0x000311B5
	public static void StartListeningForBinding(VoidGeneric<Key> OnBindingReceived)
	{
		PCPadInputProvider.m_BindingListener.StartListening(OnBindingReceived);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00032DC2 File Offset: 0x000311C2
	public static void StopListeningForBinding()
	{
		PCPadInputProvider.m_BindingListener.StopListening();
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00032DCE File Offset: 0x000311CE
	private static void OnDeviceAttached(InputDevice _device)
	{
		PCPadInputProvider.m_allDevices.Add(StandardActionSet.CreateForJoystick(_device));
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00032DE0 File Offset: 0x000311E0
	private static void OnDeviceDetached(InputDevice _device)
	{
		PCPadInputProvider.m_allDevices.RemoveAll((StandardActionSet x) => x.Device == _device);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00032E14 File Offset: 0x00031214
	private static StandardActionSet GetActionSet(ControlPadInput.PadNum _pad)
	{
		if (PCPadInputProvider.s_engagedPads.ContainsKey(_pad))
		{
			return PCPadInputProvider.s_engagedPads[_pad];
		}
		int num = -1;
		for (int i = 0; i <= (int)_pad; i++)
		{
			if (!PCPadInputProvider.s_engagedPads.ContainsKey((ControlPadInput.PadNum)i))
			{
				num++;
			}
		}
		int num2 = 0;
		for (int j = 0; j < PCPadInputProvider.m_allDevices.Count; j++)
		{
			if (!PCPadInputProvider.s_engagedPads.ContainsValue(PCPadInputProvider.m_allDevices[j]))
			{
				if (num2 == num)
				{
					return PCPadInputProvider.m_allDevices[j];
				}
				num2++;
			}
		}
		return null;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00032EBC File Offset: 0x000312BC
	public static bool IsKeyboard(ControlPadInput.PadNum _padNum)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_padNum);
		return actionSet != null && actionSet.Device == null;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00032EE4 File Offset: 0x000312E4
	public static void UpdateKeyboardButtons(bool bForce = false)
	{
		PCPadInputProvider.OnUpdateKeyboardButtons(bForce);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x00032EF4 File Offset: 0x000312F4
	public static PlayerActionSet EngagePad(ControlPadInput.PadNum _oldPadNum, ControlPadInput.PadNum _newPadNum)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_oldPadNum);
		if (actionSet != null)
		{
			PCPadInputProvider.s_engagedPads.SafeAdd(_newPadNum, actionSet);
		}
		return actionSet;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00032F1B File Offset: 0x0003131B
	public static void DisengagePad(ControlPadInput.PadNum _padNum)
	{
		PCPadInputProvider.s_engagedPads.SafeRemove(_padNum);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00032F28 File Offset: 0x00031328
	public bool IsPadAttached(ControlPadInput.PadNum _pad)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_pad);
		return actionSet != null && (actionSet.Device == null || actionSet.Device.IsAttached);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x00032F5C File Offset: 0x0003135C
	public bool IsEngagementDown(ControlPadInput.PadNum _pad)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_pad);
		return actionSet != null && actionSet.EngagementButton.State;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00032F84 File Offset: 0x00031384
	public bool IsDown(ControlPadInput.PadNum _pad, ControlPadInput.Button _button)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_pad);
		return actionSet != null && actionSet.ButtonActions[_button].State;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00032FB4 File Offset: 0x000313B4
	public float GetValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value)
	{
		StandardActionSet actionSet = PCPadInputProvider.GetActionSet(_pad);
		if (actionSet != null)
		{
			return actionSet.ValueActions[_value].Value;
		}
		return 0f;
	}

	// Token: 0x04000709 RID: 1801
	private static Dictionary<ControlPadInput.PadNum, StandardActionSet> s_engagedPads = new Dictionary<ControlPadInput.PadNum, StandardActionSet>(new PCPadInputProvider.PadNumComparer());

	// Token: 0x0400070A RID: 1802
	private static List<StandardActionSet> m_allDevices = new List<StandardActionSet>();

	// Token: 0x0400070B RID: 1803
	public static CallbackBool OnUpdateKeyboardButtons = delegate(bool bForce)
	{
	};

	// Token: 0x0400070C RID: 1804
	private static BindingListener m_BindingListener = new BindingListener();

	// Token: 0x0400070D RID: 1805
	private static KeyboardBindings m_DefaultKeyboardBindings = null;

	// Token: 0x0400070E RID: 1806
	private static KeyboardBindings m_UserKeyboardBindings = null;

	// Token: 0x0400070F RID: 1807
	[CompilerGenerated]
	private static Action<InputDevice> <>f__mg$cache0;

	// Token: 0x04000710 RID: 1808
	[CompilerGenerated]
	private static Action<InputDevice> <>f__mg$cache1;

	// Token: 0x020001F6 RID: 502
	private class PadNumComparer : IEqualityComparer<ControlPadInput.PadNum>
	{
		// Token: 0x0600086F RID: 2159 RVA: 0x00032FEF File Offset: 0x000313EF
		public bool Equals(ControlPadInput.PadNum x, ControlPadInput.PadNum y)
		{
			return x == y;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00032FF5 File Offset: 0x000313F5
		public int GetHashCode(ControlPadInput.PadNum obj)
		{
			return (int)obj;
		}
	}
}
