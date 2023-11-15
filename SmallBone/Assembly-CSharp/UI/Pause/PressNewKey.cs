using System;
using GameResources;
using InControl;
using UnityEngine;
using UserInput;

namespace UI.Pause
{
	// Token: 0x02000425 RID: 1061
	public class PressNewKey : MonoBehaviour
	{
		// Token: 0x06001412 RID: 5138 RVA: 0x0003D450 File Offset: 0x0003B650
		private void Awake()
		{
			BindingListenOptions listenOptions = KeyMapper.Map.ListenOptions;
			listenOptions.OnBindingAdded = (Action<PlayerAction, BindingSource>)Delegate.Combine(listenOptions.OnBindingAdded, new Action<PlayerAction, BindingSource>(delegate(PlayerAction action, BindingSource addedBinding)
			{
				if (addedBinding == PressNewKey.leftShiftBinding || addedBinding == PressNewKey.rightShiftBinding)
				{
					action.ReplaceBinding(addedBinding, PressNewKey.shiftBinding);
				}
				if (addedBinding == PressNewKey.leftControlBinding || addedBinding == PressNewKey.rightControlBinding)
				{
					action.ReplaceBinding(addedBinding, PressNewKey.controlBinding);
				}
				if (addedBinding == PressNewKey.leftAltBinding || addedBinding == PressNewKey.rightAltBinding)
				{
					action.ReplaceBinding(addedBinding, PressNewKey.altBinding);
				}
			}));
			BindingListenOptions listenOptions2 = KeyMapper.Map.ListenOptions;
			listenOptions2.OnBindingFound = (Func<PlayerAction, BindingSource, bool>)Delegate.Combine(listenOptions2.OnBindingFound, new Func<PlayerAction, BindingSource, bool>(delegate(PlayerAction action, BindingSource foundBinding)
			{
				if (KeyMap.SimplifyBindingSourceType(this._oldBinding.BindingSourceType) != KeyMap.SimplifyBindingSourceType(foundBinding.BindingSourceType))
				{
					return false;
				}
				DeviceBindingSource deviceBindingSource = foundBinding as DeviceBindingSource;
				if (deviceBindingSource != null)
				{
					if (deviceBindingSource.Control == InputControlType.LeftStickUp || deviceBindingSource.Control == InputControlType.LeftStickDown || deviceBindingSource.Control == InputControlType.LeftStickLeft || deviceBindingSource.Control == InputControlType.LeftStickRight || deviceBindingSource.Control == InputControlType.LeftStickButton || deviceBindingSource.Control == InputControlType.DPadUp || deviceBindingSource.Control == InputControlType.DPadDown || deviceBindingSource.Control == InputControlType.DPadLeft || deviceBindingSource.Control == InputControlType.DPadRight)
					{
						return false;
					}
					if (deviceBindingSource.Control == InputControlType.Menu || deviceBindingSource.Control == InputControlType.Options || deviceBindingSource.Control == InputControlType.Start || deviceBindingSource.Control == InputControlType.Plus || deviceBindingSource.Control == InputControlType.RightCommand || deviceBindingSource.Control == InputControlType.Select || deviceBindingSource.Control == InputControlType.Back || deviceBindingSource.Control == InputControlType.View || deviceBindingSource.Control == InputControlType.Share || deviceBindingSource.Control == InputControlType.Pause || deviceBindingSource.Control == InputControlType.Minus || deviceBindingSource.Control == InputControlType.LeftCommand || deviceBindingSource.Control == InputControlType.Capture || deviceBindingSource.Control == InputControlType.Command)
					{
						action.StopListeningForBinding();
						base.gameObject.SetActive(false);
						return false;
					}
				}
				if (foundBinding == PressNewKey.enterBinding)
				{
					return false;
				}
				if (foundBinding == PressNewKey.escapeBinding)
				{
					action.StopListeningForBinding();
					base.gameObject.SetActive(false);
					return false;
				}
				if (foundBinding == PressNewKey.leftShiftBinding || foundBinding == PressNewKey.rightShiftBinding)
				{
					foundBinding = PressNewKey.shiftBinding;
				}
				if (foundBinding == PressNewKey.leftControlBinding || foundBinding == PressNewKey.rightControlBinding)
				{
					foundBinding = PressNewKey.controlBinding;
				}
				if (foundBinding == PressNewKey.leftAltBinding || foundBinding == PressNewKey.rightAltBinding)
				{
					foundBinding = PressNewKey.altBinding;
				}
				Sprite sprite;
				if (!CommonResource.instance.TryGetKeyIcon(foundBinding, out sprite, false))
				{
					return false;
				}
				if (KeyMapper.Map.gameActions.Contains(action))
				{
					for (int i = 0; i < KeyMapper.Map.gameActions.Count; i++)
					{
						PlayerAction playerAction = KeyMapper.Map.gameActions[i];
						if (action != playerAction)
						{
							foreach (BindingSource bindingSource in playerAction.Bindings)
							{
								if (bindingSource == foundBinding)
								{
									BindingSource bindingSource2 = null;
									BindingSource oldBinding = this._oldBinding;
									KeyBindingSource keyBindingSource = oldBinding as KeyBindingSource;
									if (keyBindingSource == null)
									{
										MouseBindingSource mouseBindingSource = oldBinding as MouseBindingSource;
										if (mouseBindingSource == null)
										{
											DeviceBindingSource deviceBindingSource2 = oldBinding as DeviceBindingSource;
											if (deviceBindingSource2 != null)
											{
												bindingSource2 = new DeviceBindingSource(deviceBindingSource2.Control);
											}
										}
										else
										{
											bindingSource2 = new MouseBindingSource(mouseBindingSource.Control);
										}
									}
									else
									{
										bindingSource2 = new KeyBindingSource(keyBindingSource.Control);
									}
									if (bindingSource2 != null)
									{
										playerAction.ReplaceBinding(bindingSource, bindingSource2);
										break;
									}
									break;
								}
							}
						}
					}
				}
				base.gameObject.SetActive(false);
				return true;
			}));
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0003D4C6 File Offset: 0x0003B6C6
		public void ListenForBinding(PlayerAction action, BindingSource binding)
		{
			base.gameObject.SetActive(true);
			this._currentAction = action;
			this._oldBinding = binding;
			KeyMapper.Map.SetListenOptions();
			this._currentAction.ListenForBindingReplacing(this._oldBinding);
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0003D4FD File Offset: 0x0003B6FD
		private void Update()
		{
			if (!this._currentAction.IsListeningForBinding)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}

		// Token: 0x04001108 RID: 4360
		private static readonly KeyBindingSource escapeBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.Escape
		});

		// Token: 0x04001109 RID: 4361
		private static readonly KeyBindingSource enterBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.Return
		});

		// Token: 0x0400110A RID: 4362
		private static readonly KeyBindingSource leftShiftBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.LeftShift
		});

		// Token: 0x0400110B RID: 4363
		private static readonly KeyBindingSource rightShiftBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.RightShift
		});

		// Token: 0x0400110C RID: 4364
		private static readonly KeyBindingSource shiftBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.Shift
		});

		// Token: 0x0400110D RID: 4365
		private static readonly KeyBindingSource leftControlBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.LeftControl
		});

		// Token: 0x0400110E RID: 4366
		private static readonly KeyBindingSource rightControlBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.RightControl
		});

		// Token: 0x0400110F RID: 4367
		private static readonly KeyBindingSource controlBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.Control
		});

		// Token: 0x04001110 RID: 4368
		private static readonly KeyBindingSource leftAltBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.LeftAlt
		});

		// Token: 0x04001111 RID: 4369
		private static readonly KeyBindingSource rightAltBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.RightAlt
		});

		// Token: 0x04001112 RID: 4370
		private static readonly KeyBindingSource altBinding = new KeyBindingSource(new InControl.Key[]
		{
			InControl.Key.Alt
		});

		// Token: 0x04001113 RID: 4371
		private PlayerAction _currentAction;

		// Token: 0x04001114 RID: 4372
		private BindingSource _oldBinding;
	}
}
