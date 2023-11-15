using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000830 RID: 2096
[ExecutionDependency(typeof(MetaEnvironmentFactory))]
[ExecutionDependency(typeof(BootstrapManager))]
[AddComponentMenu("Scripts/Game/Input/PlayerInputLookup")]
public class PlayerInputLookup : MonoBehaviour
{
	// Token: 0x06002813 RID: 10259 RVA: 0x000BBD0F File Offset: 0x000BA10F
	public static bool IsAwake()
	{
		return PlayerInputLookup.s_this != null;
	}

	// Token: 0x06002814 RID: 10260 RVA: 0x000BBD1C File Offset: 0x000BA11C
	private void Awake()
	{
		PlayerInputLookup.s_this = this;
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		if (PlayerInputLookup.s_baseInputConfig == null && this.m_defaultInputLookup != null)
		{
			PlayerInputLookup.s_baseInputConfig = this.m_defaultInputLookup.Config;
		}
		if (this.m_getPadAssignmentFromSession)
		{
			GameUtils.EnsureBootstrapSetup();
		}
		PlayerInputLookup.OnRegenerateControls();
	}

	// Token: 0x06002815 RID: 10261 RVA: 0x000BBD80 File Offset: 0x000BA180
	public static int GetSystemControllerMaximum()
	{
		if (typeof(PCPadInputProvider) == typeof(DirectInputProvider))
		{
			return 11;
		}
		if (typeof(PCPadInputProvider) == typeof(PCPadInputProvider))
		{
			return 12;
		}
		if (typeof(PCPadInputProvider) == typeof(XInputProvider))
		{
			return 4;
		}
		return 12;
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000BBDE4 File Offset: 0x000BA1E4
	public static ControlPadInput.PadNum GetPadForPlayer(PlayerInputLookup.Player _player)
	{
		GameInputConfig inputConfig = PlayerInputLookup.GetInputConfig();
		PlayerGameInput inputData = inputConfig.GetInputData(_player);
		if (inputData != null)
		{
			return inputData.Pad;
		}
		return ControlPadInput.PadNum.Count;
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000BBE0E File Offset: 0x000BA20E
	public static GameInputConfig GetInputConfig()
	{
		return PlayerInputLookup.s_baseInputConfig;
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000BBE15 File Offset: 0x000BA215
	public static GameInputConfig GetBaseInputConfig()
	{
		return PlayerInputLookup.s_baseInputConfig;
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000BBE1C File Offset: 0x000BA21C
	public static void SetInputConfig(GameInputConfig _inputConfig)
	{
		PlayerInputLookup.SetBaseInputConfig(_inputConfig);
	}

	// Token: 0x0600281A RID: 10266 RVA: 0x000BBE24 File Offset: 0x000BA224
	public static void SetBaseInputConfig(GameInputConfig _inputConfig)
	{
		PlayerInputLookup.s_baseInputConfig = _inputConfig;
		if (PlayerInputLookup.s_this != null)
		{
			PlayerInputLookup.s_this.RegenerateControls();
		}
	}

	// Token: 0x0600281B RID: 10267 RVA: 0x000BBE46 File Offset: 0x000BA246
	public static void ResetToDefaultInputConfig()
	{
		if (PlayerInputLookup.s_this != null && PlayerInputLookup.s_this.m_defaultInputLookup != null)
		{
			PlayerInputLookup.SetBaseInputConfig(PlayerInputLookup.s_this.m_defaultInputLookup.Config);
		}
	}

	// Token: 0x0600281C RID: 10268 RVA: 0x000BBE84 File Offset: 0x000BA284
	private void RegenerateControls()
	{
		this.m_reassignableElements.RemoveAll((KeyValuePair<WeakReference, CallbackVoid> x) => !x.Key.IsAlive);
		foreach (KeyValuePair<WeakReference, CallbackVoid> keyValuePair in this.m_reassignableElements)
		{
			keyValuePair.Value();
		}
		PCPadInputProvider.UpdateKeyboardButtons(false);
		PlayerInputLookup.OnRegenerateControls();
	}

	// Token: 0x0600281D RID: 10269 RVA: 0x000BBF20 File Offset: 0x000BA320
	public static bool IsPadAttached(ControlPadInput.PadNum _pad)
	{
		return Singleton<PCPadInputProvider>.Get().IsPadAttached(_pad);
	}

	// Token: 0x0600281E RID: 10270 RVA: 0x000BBF2D File Offset: 0x000BA32D
	public static ILogicalButton ConstructDebugBoundButton(ControlPadInput.PadNum _pad, ControlPadInput.Button _button)
	{
		return PlayerInputLookup.ConstructDebugBoundButton(new ControlPadInput.ButtonIdentifier(_pad, _button));
	}

	// Token: 0x0600281F RID: 10271 RVA: 0x000BBF3B File Offset: 0x000BA33B
	private static ILogicalButton ConstructDebugBoundButton(ControlPadInput.ButtonIdentifier _buttonIdentifier)
	{
		return new LogicalPadButton<PCPadInputProvider>(_buttonIdentifier.pad, _buttonIdentifier.button);
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x000BBF50 File Offset: 0x000BA350
	private static ILogicalButton GetSidedLogicalCombo(AmbiPadButton _gamePadButton, PlayerGameInput _playerInput)
	{
		ControlPadInput.ButtonIdentifier[] realButtons = PlayerInputLookup.GetInputConfig().GetRealButtons(_playerInput, _gamePadButton);
		Converter<ControlPadInput.ButtonIdentifier, ILogicalButton> converter = (ControlPadInput.ButtonIdentifier _id) => PlayerInputLookup.ConstructDebugBoundButton(_id);
		ILogicalButton[] buttons = Array.ConvertAll<ControlPadInput.ButtonIdentifier, ILogicalButton>(realButtons, converter);
		return new ComboLogicalButton(buttons, ComboLogicalButton.ComboType.Or);
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000BBF98 File Offset: 0x000BA398
	private static ILogicalValue GetSidedLogicalCombo(AmbiPadValue _gamePadValue, PlayerGameInput _playerInput)
	{
		ControlPadInput.ValueIdentifier[] realValues = PlayerInputLookup.GetInputConfig().GetRealValues(_playerInput, _gamePadValue);
		Converter<ControlPadInput.ValueIdentifier, ILogicalValue> converter = (ControlPadInput.ValueIdentifier _id) => PlayerInputLookup.ConstructDebugBoundValue(_id);
		ILogicalValue[] buttons = Array.ConvertAll<ControlPadInput.ValueIdentifier, ILogicalValue>(realValues, converter);
		return new ComboLogicalValue(buttons, ComboLogicalValue.ComboType.AbsMax);
	}

	// Token: 0x06002822 RID: 10274 RVA: 0x000BBFDF File Offset: 0x000BA3DF
	public static ILogicalButton GetUIButton(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _p = PlayerInputLookup.Player.One)
	{
		return PlayerInputLookup.GetButton(_id, _p);
	}

	// Token: 0x06002823 RID: 10275 RVA: 0x000BBFE8 File Offset: 0x000BA3E8
	public static ILogicalValue GetUIValue(PlayerInputLookup.LogicalValueID _id, PlayerInputLookup.Player _p = PlayerInputLookup.Player.One)
	{
		return PlayerInputLookup.GetValue(_id, _p);
	}

	// Token: 0x06002824 RID: 10276 RVA: 0x000BBFF4 File Offset: 0x000BA3F4
	public static ILogicalButton GetButton(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _player)
	{
		Generic<ILogicalButton> generator = () => PlayerInputLookup.GetFixedButton(_id, _player);
		return PlayerInputLookup.BuildReassignableElement<ReassignableButton, ILogicalButton>(generator);
	}

	// Token: 0x06002825 RID: 10277 RVA: 0x000BC028 File Offset: 0x000BA428
	public static ILogicalButton GetAnyButton(PlayerInputLookup.LogicalButtonID _id)
	{
		ILogicalButton[] buttons = new ILogicalButton[]
		{
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.One),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Two),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Three),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Four)
		};
		return new ComboLogicalButton(buttons, ComboLogicalButton.ComboType.Or);
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x000BC06C File Offset: 0x000BA46C
	public static ILogicalButton GetAnyButton(PlayerInputLookup.LogicalButtonID _id, PadSide _forceSide)
	{
		ILogicalButton[] buttons = new ILogicalButton[]
		{
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.One, _forceSide),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Two, _forceSide),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Three, _forceSide),
			PlayerInputLookup.GetEngagedButton(_id, PlayerInputLookup.Player.Four, _forceSide)
		};
		return new ComboLogicalButton(buttons, ComboLogicalButton.ComboType.Or);
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x000BC0B3 File Offset: 0x000BA4B3
	public static ILogicalButton GetEngagementButton(PlayerGameInput _playerGameInput)
	{
		return new LogicalPCEngagementButton(_playerGameInput.Pad);
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x000BC0C0 File Offset: 0x000BA4C0
	public static ILogicalButton GetEngagedButton(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _player)
	{
		Generic<ILogicalButton> generator = delegate()
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(_player);
			if (padForPlayer != ControlPadInput.PadNum.Count)
			{
				return PlayerInputLookup.GateOnEngagement(PlayerInputLookup.GetFixedButton(_id, _player), (EngagementSlot)padForPlayer);
			}
			return new ComboLogicalButton(new ILogicalButton[0], ComboLogicalButton.ComboType.Or);
		};
		return PlayerInputLookup.BuildReassignableElement<ReassignableButton, ILogicalButton>(generator);
	}

	// Token: 0x06002829 RID: 10281 RVA: 0x000BC0F4 File Offset: 0x000BA4F4
	public static ILogicalButton GetEngagedButton(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _player, PadSide _side)
	{
		Generic<ILogicalButton> generator = delegate()
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(_player);
			if (padForPlayer != ControlPadInput.PadNum.Count && PlayerInputLookup.s_this != null)
			{
				AmbiControlsMappingData mappingData = (_side != PadSide.Both) ? PlayerInputLookup.s_this.m_playerManager.SidedAmbiMapping : PlayerInputLookup.s_this.m_playerManager.UnsidedAmbiMapping;
				PlayerGameInput playerGameInput = new PlayerGameInput(padForPlayer, _side, mappingData);
				return PlayerInputLookup.GateOnEngagement(PlayerInputLookup.GetFixedButton(_id, playerGameInput), (EngagementSlot)_player);
			}
			return new ComboLogicalButton(new ILogicalButton[0], ComboLogicalButton.ComboType.Or);
		};
		return PlayerInputLookup.BuildReassignableElement<ReassignableButton, ILogicalButton>(generator);
	}

	// Token: 0x0600282A RID: 10282 RVA: 0x000BC130 File Offset: 0x000BA530
	public static ILogicalValue GetValue(PlayerInputLookup.LogicalValueID _id, PlayerInputLookup.Player _player)
	{
		Generic<ILogicalValue> generator = () => PlayerInputLookup.GetFixedValue(_id, _player);
		return PlayerInputLookup.BuildReassignableElement<ReassignableValue, ILogicalValue>(generator);
	}

	// Token: 0x0600282B RID: 10283 RVA: 0x000BC164 File Offset: 0x000BA564
	public static ILogicalValue GetAnyValue(PlayerInputLookup.LogicalValueID _id)
	{
		ILogicalValue[] buttons = new ILogicalValue[]
		{
			PlayerInputLookup.GetEngagedValue(_id, PlayerInputLookup.Player.One),
			PlayerInputLookup.GetEngagedValue(_id, PlayerInputLookup.Player.Two),
			PlayerInputLookup.GetEngagedValue(_id, PlayerInputLookup.Player.Three),
			PlayerInputLookup.GetEngagedValue(_id, PlayerInputLookup.Player.Four)
		};
		return new ComboLogicalValue(buttons, ComboLogicalValue.ComboType.AbsMax);
	}

	// Token: 0x0600282C RID: 10284 RVA: 0x000BC1A8 File Offset: 0x000BA5A8
	public static ILogicalValue GetAnyValueForLocals(PlayerInputLookup.LogicalValueID _id)
	{
		FastList<User> users = ClientUserSystem.m_Users;
		List<ILogicalValue> list = new List<ILogicalValue>();
		for (int i = 0; i < users.Count; i++)
		{
			User user = users._items[i];
			if (user.IsLocal)
			{
				list.Add(PlayerInputLookup.GetEngagedValue(_id, (PlayerInputLookup.Player)i));
			}
		}
		return new ComboLogicalValue(list.ToArray(), ComboLogicalValue.ComboType.AbsMax);
	}

	// Token: 0x0600282D RID: 10285 RVA: 0x000BC208 File Offset: 0x000BA608
	private static ILogicalValue GetEngagedValue(PlayerInputLookup.LogicalValueID _id, PlayerInputLookup.Player _player)
	{
		Generic<ILogicalValue> generator = delegate()
		{
			ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(_player);
			if (padForPlayer != ControlPadInput.PadNum.Count)
			{
				return PlayerInputLookup.GateOnEngagement(PlayerInputLookup.GetFixedValue(_id, _player), (EngagementSlot)padForPlayer);
			}
			return new ComboLogicalValue(new ILogicalValue[0], ComboLogicalValue.ComboType.AbsMax);
		};
		return PlayerInputLookup.BuildReassignableElement<ReassignableValue, ILogicalValue>(generator);
	}

	// Token: 0x0600282E RID: 10286 RVA: 0x000BC23C File Offset: 0x000BA63C
	private static Concrete BuildReassignableElement<Concrete, IElement>(Generic<IElement> _generator) where Concrete : class, IReassignable<IElement>, new() where IElement : ILogicalElement
	{
		IElement childNode = _generator();
		Concrete concrete = Activator.CreateInstance<Concrete>();
		concrete.Reassign(childNode);
		WeakReference key = new WeakReference(concrete, false);
		CallbackVoid value = delegate()
		{
			Concrete concrete2 = key.Target as Concrete;
			if (key.IsAlive && concrete2 != null)
			{
				concrete2.Reassign(_generator());
			}
		};
		PlayerInputLookup.s_this.m_reassignableElements.Add(key, value);
		return concrete;
	}

	// Token: 0x0600282F RID: 10287 RVA: 0x000BC2AC File Offset: 0x000BA6AC
	private static ILogicalButton GetFixedButton(PlayerInputLookup.LogicalButtonID _id, PlayerInputLookup.Player _player)
	{
		PlayerGameInput inputData = PlayerInputLookup.GetInputConfig().GetInputData(_player);
		if (inputData != null)
		{
			return PlayerInputLookup.GetFixedButton(_id, inputData);
		}
		return new LogicalKeycodeButton();
	}

	// Token: 0x06002830 RID: 10288 RVA: 0x000BC2D8 File Offset: 0x000BA6D8
	public static AmbiPadButton LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID _id)
	{
		switch (_id)
		{
		case PlayerInputLookup.LogicalButtonID.PickupAndDrop:
			return AmbiPadButton.One;
		case PlayerInputLookup.LogicalButtonID.Dash:
			return AmbiPadButton.Three;
		case PlayerInputLookup.LogicalButtonID.WorkstationInteract:
			return AmbiPadButton.Two;
		default:
			if (_id == PlayerInputLookup.LogicalButtonID.PlayerSwitch)
			{
				return AmbiPadButton.Five;
			}
			if (_id != PlayerInputLookup.LogicalButtonID.Curse)
			{
				return AmbiPadButton.Count;
			}
			return AmbiPadButton.Four;
		}
	}

	// Token: 0x06002831 RID: 10289 RVA: 0x000BC30F File Offset: 0x000BA70F
	public static AmbiPadValue[] LogicalToAmbiValue(PlayerInputLookup.LogicalValueID _id)
	{
		if (_id == PlayerInputLookup.LogicalValueID.MovementX)
		{
			AmbiPadValue[] array = new AmbiPadValue[2];
			array[0] = AmbiPadValue.DPadX;
			return array;
		}
		if (_id != PlayerInputLookup.LogicalValueID.MovementY)
		{
			return null;
		}
		return new AmbiPadValue[]
		{
			AmbiPadValue.DPadY,
			AmbiPadValue.StickY
		};
	}

	// Token: 0x06002832 RID: 10290 RVA: 0x000BC340 File Offset: 0x000BA740
	public static ILogicalButton GetFixedButton(PlayerInputLookup.LogicalButtonID _id, PlayerGameInput _playerGameInput)
	{
		switch (_id)
		{
		case PlayerInputLookup.LogicalButtonID.None:
			return new LogicalKeycodeButton();
		case PlayerInputLookup.LogicalButtonID.PickupAndDrop:
			return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.PickupAndDrop), _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.Dash:
			return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.Dash), _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.WorkstationInteract:
			return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.WorkstationInteract), _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.ResetButton:
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Start, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.QuitButton:
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Start, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UIUp:
		{
			ILogicalButton logicalButton = new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadY, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickY, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f)
			}, ComboLogicalButton.ComboType.Or);
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton2 = new LogicalKeycodeButton(new KeyCode?(KeyCode.UpArrow), ControlPadInput.Button.DPadUp);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton2,
					logicalButton
				}, ComboLogicalButton.ComboType.Or);
			}
			return logicalButton;
		}
		case PlayerInputLookup.LogicalButtonID.UIDown:
		{
			ILogicalButton logicalButton3 = new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadY, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickY, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f)
			}, ComboLogicalButton.ComboType.Or);
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton4 = new LogicalKeycodeButton(new KeyCode?(KeyCode.DownArrow), ControlPadInput.Button.DPadDown);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton4,
					logicalButton3
				}, ComboLogicalButton.ComboType.Or);
			}
			return logicalButton3;
		}
		case PlayerInputLookup.LogicalButtonID.UILeft:
		{
			ILogicalButton logicalButton5 = new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadX, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickX, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f)
			}, ComboLogicalButton.ComboType.Or);
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton6 = new LogicalKeycodeButton(new KeyCode?(KeyCode.LeftArrow), ControlPadInput.Button.DPadLeft);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton6,
					logicalButton5
				}, ComboLogicalButton.ComboType.Or);
			}
			return logicalButton5;
		}
		case PlayerInputLookup.LogicalButtonID.UIRight:
		{
			ILogicalButton logicalButton7 = new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadX, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickX, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f)
			}, ComboLogicalButton.ComboType.Or);
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton8 = new LogicalKeycodeButton(new KeyCode?(KeyCode.RightArrow), ControlPadInput.Button.DPadRight);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton8,
					logicalButton7
				}, ComboLogicalButton.ComboType.Or);
			}
			return logicalButton7;
		}
		case PlayerInputLookup.LogicalButtonID.UISelect:
		{
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton9 = new LogicalKeycodeButton(new KeyCode?(KeyCode.Space), ControlPadInput.Button.A);
				ILogicalButton sidedLogicalCombo = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Confirm, _playerGameInput);
				ILogicalButton sidedLogicalCombo2 = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Start, _playerGameInput);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton9,
					sidedLogicalCombo,
					sidedLogicalCombo2
				}, ComboLogicalButton.ComboType.Or);
			}
			ILogicalButton sidedLogicalCombo3 = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Confirm, _playerGameInput);
			ILogicalButton sidedLogicalCombo4 = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Start, _playerGameInput);
			return new ComboLogicalButton(new ILogicalButton[]
			{
				sidedLogicalCombo3,
				sidedLogicalCombo4
			}, ComboLogicalButton.ComboType.Or);
		}
		case PlayerInputLookup.LogicalButtonID.UISelectNotStart:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				ILogicalButton logicalButton10 = new LogicalKeycodeButton(new KeyCode?(KeyCode.Space), ControlPadInput.Button.A);
				ILogicalButton sidedLogicalCombo5 = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Confirm, _playerGameInput);
				return new ComboLogicalButton(new ILogicalButton[]
				{
					logicalButton10,
					sidedLogicalCombo5
				}, ComboLogicalButton.ComboType.Or);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Confirm, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UICancel:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.Escape), ControlPadInput.Button.B);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Cancel, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.Pause:
		{
			ILogicalButton sidedLogicalCombo6 = PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Start, _playerGameInput);
			ILogicalButton logicalButton11 = new LogicalKeycodeButton(new KeyCode?(KeyCode.Escape), ControlPadInput.Button.Start);
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return logicalButton11;
			}
			return new ComboLogicalButton(new ILogicalButton[]
			{
				sidedLogicalCombo6,
				logicalButton11
			}, ComboLogicalButton.ComboType.Or);
		}
		case PlayerInputLookup.LogicalButtonID.PlayerSwitch:
			return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.PlayerSwitch), _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.Curse:
			return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.Curse), _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UIStick:
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Stick, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UIMultiChefMenu:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.E), ControlPadInput.Button.Y);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Four, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.DebugMenu:
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.DebugMenu, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UIChangeMode:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.LeftControl), ControlPadInput.Button.X);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Four, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UIResultsToggleProfile:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.LeftControl), ControlPadInput.Button.X);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Two, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.Horn:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.T), ControlPadInput.Button.LeftAnalog);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Horn, _playerGameInput);
		case PlayerInputLookup.LogicalButtonID.UILeftPlayerSpecific:
			return new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadX, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickX, _playerGameInput), ValueThresholdButton.ThresholdType.LessThan, -0.75f)
			}, ComboLogicalButton.ComboType.Or);
		case PlayerInputLookup.LogicalButtonID.UIRightPlayerSpecific:
			return new ComboLogicalButton(new ILogicalButton[]
			{
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadX, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f),
				new ValueThresholdButton(PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickX, _playerGameInput), ValueThresholdButton.ThresholdType.Greater, 0.75f)
			}, ComboLogicalButton.ComboType.Or);
		case PlayerInputLookup.LogicalButtonID.UIRestartLevel:
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return PlayerInputLookup.GetSidedLogicalCombo(PlayerInputLookup.LogicalToAmbiButton(PlayerInputLookup.LogicalButtonID.Dash), _playerGameInput);
			}
			return PlayerInputLookup.GetSidedLogicalCombo(AmbiPadButton.Cancel, _playerGameInput);
		default:
			if (_id != PlayerInputLookup.LogicalButtonID.UISkip)
			{
				return new LogicalKeycodeButton();
			}
			if (KeyboardUtils.IsKeyboard(_playerGameInput))
			{
				return new LogicalKeycodeButton(new KeyCode?(KeyCode.LeftAlt), ControlPadInput.Button.B);
			}
			return PlayerInputLookup.GetFixedButton(PlayerInputLookup.LogicalButtonID.UICancel, _playerGameInput);
		}
	}

	// Token: 0x06002833 RID: 10291 RVA: 0x000BC860 File Offset: 0x000BAC60
	public static ControlPadInput.Button[] GetRealButtons(PlayerInputLookup.LogicalButtonID buttonID, PadSide side)
	{
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		PlayerGameInput playerGameInput = new PlayerGameInput(ControlPadInput.PadNum.One, side, (side != PadSide.Both) ? playerManager.SidedAmbiMapping : playerManager.UnsidedAmbiMapping);
		AmbiPadButton ambiPadButton = PlayerInputLookup.LogicalToAmbiButton(buttonID);
		if (ambiPadButton != AmbiPadButton.Count)
		{
			ControlPadInput.ButtonIdentifier[] realButtons = PlayerInputLookup.GetInputConfig().GetRealButtons(playerGameInput, ambiPadButton);
			return realButtons.ConvertAll((ControlPadInput.ButtonIdentifier x) => x.button);
		}
		return new ControlPadInput.Button[0];
	}

	// Token: 0x06002834 RID: 10292 RVA: 0x000BC8DC File Offset: 0x000BACDC
	private static KeyCode GetDebugButton(ControlPadInput.Button _button)
	{
		switch (_button)
		{
		case ControlPadInput.Button.A:
			return KeyCode.G;
		case ControlPadInput.Button.X:
			return KeyCode.F;
		case ControlPadInput.Button.B:
			return KeyCode.H;
		case ControlPadInput.Button.Y:
			return KeyCode.T;
		case ControlPadInput.Button.LB:
			return KeyCode.E;
		case ControlPadInput.Button.RB:
			return KeyCode.U;
		case ControlPadInput.Button.LTrigger:
			return KeyCode.R;
		case ControlPadInput.Button.RTrigger:
			return KeyCode.Y;
		case ControlPadInput.Button.DPadLeft:
			return KeyCode.LeftArrow;
		case ControlPadInput.Button.DPadRight:
			return KeyCode.RightArrow;
		case ControlPadInput.Button.DPadUp:
			return KeyCode.UpArrow;
		case ControlPadInput.Button.DPadDown:
			return KeyCode.DownArrow;
		case ControlPadInput.Button.Back:
			return KeyCode.Backspace;
		case ControlPadInput.Button.Start:
			return KeyCode.Return;
		case ControlPadInput.Button.LeftAnalog:
			return KeyCode.LeftShift;
		case ControlPadInput.Button.RightAnalog:
			return KeyCode.RightShift;
		default:
			return KeyCode.None;
		}
	}

	// Token: 0x06002835 RID: 10293 RVA: 0x000BC978 File Offset: 0x000BAD78
	private static ILogicalValue ConstructDebugBoundValue(ControlPadInput.ValueIdentifier _valueIdentifier)
	{
		return new LogicalPadValue<PCPadInputProvider>(_valueIdentifier.pad, _valueIdentifier.value);
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x000BC990 File Offset: 0x000BAD90
	private static ILogicalValue GetFixedValue(PlayerInputLookup.LogicalValueID _id, PlayerInputLookup.Player _player)
	{
		PlayerGameInput inputData = PlayerInputLookup.GetInputConfig().GetInputData(_player);
		if (inputData != null)
		{
			return PlayerInputLookup.GetFixedValue(_id, inputData);
		}
		return new LogicalKeycodeValue();
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x000BC9BC File Offset: 0x000BADBC
	private static ILogicalValue GetFixedValue(PlayerInputLookup.LogicalValueID _id, PlayerGameInput _playerGameInput)
	{
		switch (_id)
		{
		case PlayerInputLookup.LogicalValueID.None:
			return new LogicalKeycodeValue();
		case PlayerInputLookup.LogicalValueID.MovementX:
			return new ComboLogicalValue(new ILogicalValue[]
			{
				PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadX, _playerGameInput),
				PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickX, _playerGameInput)
			}, ComboLogicalValue.ComboType.AbsMax);
		case PlayerInputLookup.LogicalValueID.MovementY:
			return new ComboLogicalValue(new ILogicalValue[]
			{
				PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.DPadY, _playerGameInput),
				PlayerInputLookup.GetSidedLogicalCombo(AmbiPadValue.StickY, _playerGameInput)
			}, ComboLogicalValue.ComboType.AbsMax);
		default:
			return new LogicalKeycodeValue();
		}
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x000BCA34 File Offset: 0x000BAE34
	private static ILogicalValue GetEngagedFixedValue(PlayerInputLookup.LogicalValueID _id, PlayerInputLookup.Player _player)
	{
		ControlPadInput.PadNum padForPlayer = PlayerInputLookup.GetPadForPlayer(_player);
		return PlayerInputLookup.GateOnEngagement(PlayerInputLookup.GetFixedValue(_id, _player), (EngagementSlot)padForPlayer);
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x000BCA58 File Offset: 0x000BAE58
	private static ILogicalValue GateOnEngagement(ILogicalValue _value, EngagementSlot _e)
	{
		Generic<bool> callback = () => PlayerInputLookup.s_this.m_playerManager.GetUser(_e) != null;
		return new GateLogicalValue(_value, callback);
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x000BCA88 File Offset: 0x000BAE88
	private static ILogicalButton GateOnEngagement(ILogicalButton _button, EngagementSlot _e)
	{
		Generic<bool> callback = () => PlayerInputLookup.s_this.m_playerManager.GetUser(_e) != null;
		return new GateLogicalButton(_button, callback);
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x000BCAB8 File Offset: 0x000BAEB8
	private static PlayerInputLookup.ValueDebugButtons GetDebugButtons(ControlPadInput.Value _value)
	{
		switch (_value)
		{
		case ControlPadInput.Value.LStickX:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.A, KeyCode.D);
		case ControlPadInput.Value.LStickY:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.W, KeyCode.S);
		case ControlPadInput.Value.RStickX:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.J, KeyCode.L);
		case ControlPadInput.Value.RStickY:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.I, KeyCode.K);
		case ControlPadInput.Value.DPadX:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.LeftArrow, KeyCode.RightArrow);
		case ControlPadInput.Value.DPadY:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.DownArrow, KeyCode.UpArrow);
		case ControlPadInput.Value.LTrigger:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.None, KeyCode.R);
		case ControlPadInput.Value.RTrigger:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.None, KeyCode.Y);
		default:
			return new PlayerInputLookup.ValueDebugButtons(KeyCode.None, KeyCode.None);
		}
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x000BCB53 File Offset: 0x000BAF53
	private void Update()
	{
		this.MonitorDebugPadSwitching();
		XInputProvider.Update();
		DirectInputProvider.Update();
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x000BCB65 File Offset: 0x000BAF65
	private void MonitorDebugPadSwitching()
	{
	}

	// Token: 0x04001FA0 RID: 8096
	[SerializeField]
	private bool m_getPadAssignmentFromSession = true;

	// Token: 0x04001FA1 RID: 8097
	[SerializeField]
	private GameInputConfigData m_defaultInputLookup;

	// Token: 0x04001FA2 RID: 8098
	private Dictionary<WeakReference, CallbackVoid> m_reassignableElements = new Dictionary<WeakReference, CallbackVoid>();

	// Token: 0x04001FA3 RID: 8099
	private PlayerManager m_playerManager;

	// Token: 0x04001FA4 RID: 8100
	public static CallbackVoid OnRegenerateControls = delegate()
	{
	};

	// Token: 0x04001FA5 RID: 8101
	private static PlayerInputLookup s_this;

	// Token: 0x04001FA6 RID: 8102
	private static GameInputConfig s_baseInputConfig = null;

	// Token: 0x02000831 RID: 2097
	public enum LogicalButtonID
	{
		// Token: 0x04001FAC RID: 8108
		None,
		// Token: 0x04001FAD RID: 8109
		PickupAndDrop,
		// Token: 0x04001FAE RID: 8110
		Dash,
		// Token: 0x04001FAF RID: 8111
		WorkstationInteract,
		// Token: 0x04001FB0 RID: 8112
		ResetButton,
		// Token: 0x04001FB1 RID: 8113
		QuitButton,
		// Token: 0x04001FB2 RID: 8114
		UIUp,
		// Token: 0x04001FB3 RID: 8115
		UIDown,
		// Token: 0x04001FB4 RID: 8116
		UILeft,
		// Token: 0x04001FB5 RID: 8117
		UIRight,
		// Token: 0x04001FB6 RID: 8118
		UISelect,
		// Token: 0x04001FB7 RID: 8119
		UISelectNotStart,
		// Token: 0x04001FB8 RID: 8120
		UICancel,
		// Token: 0x04001FB9 RID: 8121
		Pause,
		// Token: 0x04001FBA RID: 8122
		PlayerSwitch,
		// Token: 0x04001FBB RID: 8123
		Curse,
		// Token: 0x04001FBC RID: 8124
		UIStick,
		// Token: 0x04001FBD RID: 8125
		UIMultiChefMenu,
		// Token: 0x04001FBE RID: 8126
		DebugMenu,
		// Token: 0x04001FBF RID: 8127
		UIChangeMode,
		// Token: 0x04001FC0 RID: 8128
		UIResultsToggleProfile,
		// Token: 0x04001FC1 RID: 8129
		Horn,
		// Token: 0x04001FC2 RID: 8130
		UILeftPlayerSpecific,
		// Token: 0x04001FC3 RID: 8131
		UIRightPlayerSpecific,
		// Token: 0x04001FC4 RID: 8132
		UIRestartLevel,
		// Token: 0x04001FC5 RID: 8133
		UISkip = 1000
	}

	// Token: 0x02000832 RID: 2098
	public enum LogicalValueID
	{
		// Token: 0x04001FC7 RID: 8135
		None,
		// Token: 0x04001FC8 RID: 8136
		MovementX,
		// Token: 0x04001FC9 RID: 8137
		MovementY
	}

	// Token: 0x02000833 RID: 2099
	public enum Player
	{
		// Token: 0x04001FCB RID: 8139
		One,
		// Token: 0x04001FCC RID: 8140
		Two,
		// Token: 0x04001FCD RID: 8141
		Three,
		// Token: 0x04001FCE RID: 8142
		Four,
		// Token: 0x04001FCF RID: 8143
		Five,
		// Token: 0x04001FD0 RID: 8144
		Six,
		// Token: 0x04001FD1 RID: 8145
		Seven,
		// Token: 0x04001FD2 RID: 8146
		Eight,
		// Token: 0x04001FD3 RID: 8147
		Nine,
		// Token: 0x04001FD4 RID: 8148
		Ten,
		// Token: 0x04001FD5 RID: 8149
		Eleven,
		// Token: 0x04001FD6 RID: 8150
		Count
	}

	// Token: 0x02000834 RID: 2100
	private struct ValueDebugButtons
	{
		// Token: 0x06002844 RID: 10308 RVA: 0x000BCBAC File Offset: 0x000BAFAC
		public ValueDebugButtons(KeyCode _dec, KeyCode _inc)
		{
			this.m_inc = _inc;
			this.m_dec = _dec;
		}

		// Token: 0x04001FD7 RID: 8151
		public KeyCode m_inc;

		// Token: 0x04001FD8 RID: 8152
		public KeyCode m_dec;
	}
}
