using System;
using UnityEngine;

// Token: 0x02000822 RID: 2082
[Serializable]
public class AmbiControlsMappingData : ScriptableObject
{
	// Token: 0x060027F8 RID: 10232 RVA: 0x000BB6E4 File Offset: 0x000B9AE4
	public ControlPadInput.Button[] GetRealButtons(AmbiPadButton _gamePadButton)
	{
		if (PlayerManagerShared<PCPlayerManager.PCPlayerProfile>.AcceptAndCancelButtonsInverted)
		{
			if (_gamePadButton == AmbiPadButton.Confirm)
			{
				_gamePadButton = AmbiPadButton.Cancel;
			}
			else if (_gamePadButton == AmbiPadButton.Cancel)
			{
				_gamePadButton = AmbiPadButton.Confirm;
			}
		}
		AmbiControlsMappingData.ButtonMapping buttonMapping = Array.Find<AmbiControlsMappingData.ButtonMapping>(this.m_buttonMapping, (AmbiControlsMappingData.ButtonMapping obj) => obj.GamepadButton == _gamePadButton);
		return buttonMapping.RealButtons;
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x000BB754 File Offset: 0x000B9B54
	public ControlPadInput.Value[] GetRealValues(AmbiPadValue _gamePadValue)
	{
		AmbiControlsMappingData.ValueMapping valueMapping = Array.Find<AmbiControlsMappingData.ValueMapping>(this.m_valueMapping, (AmbiControlsMappingData.ValueMapping obj) => obj.GamepadValue == _gamePadValue);
		return valueMapping.RealValues;
	}

	// Token: 0x04001F83 RID: 8067
	public AmbiControlsMappingData.ButtonMapping[] m_buttonMapping = new AmbiControlsMappingData.ButtonMapping[]
	{
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.One, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.A,
			ControlPadInput.Button.RB,
			ControlPadInput.Button.LB
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Two, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.X,
			ControlPadInput.Button.RTrigger,
			ControlPadInput.Button.LTrigger
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Three, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.B,
			ControlPadInput.Button.DPadLeft,
			ControlPadInput.Button.DPadRight
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Five, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.B,
			ControlPadInput.Button.Y,
			ControlPadInput.Button.DPadUp,
			ControlPadInput.Button.DPadDown
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Start, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.Start,
			ControlPadInput.Button.Start
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Back, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.Back,
			ControlPadInput.Button.Back
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.Horn, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.Y,
			ControlPadInput.Button.DPadUp
		}),
		new AmbiControlsMappingData.ButtonMapping(AmbiPadButton.DebugMenu, new ControlPadInput.Button[]
		{
			ControlPadInput.Button.Back
		})
	};

	// Token: 0x04001F84 RID: 8068
	public AmbiControlsMappingData.ValueMapping[] m_valueMapping = new AmbiControlsMappingData.ValueMapping[]
	{
		new AmbiControlsMappingData.ValueMapping(AmbiPadValue.StickX, new ControlPadInput.Value[]
		{
			ControlPadInput.Value.LStickX,
			ControlPadInput.Value.RStickX
		}),
		new AmbiControlsMappingData.ValueMapping(AmbiPadValue.StickY, new ControlPadInput.Value[]
		{
			ControlPadInput.Value.LStickY,
			ControlPadInput.Value.RStickY
		})
	};

	// Token: 0x02000823 RID: 2083
	[Serializable]
	public class ButtonMapping
	{
		// Token: 0x060027FA RID: 10234 RVA: 0x000BB78C File Offset: 0x000B9B8C
		public ButtonMapping(AmbiPadButton _gamePadButton, ControlPadInput.Button[] _realButtons)
		{
			this.GamepadButton = _gamePadButton;
			this.RealButtons = _realButtons;
		}

		// Token: 0x04001F85 RID: 8069
		public AmbiPadButton GamepadButton;

		// Token: 0x04001F86 RID: 8070
		public ControlPadInput.Button[] RealButtons;
	}

	// Token: 0x02000824 RID: 2084
	[Serializable]
	public class ValueMapping
	{
		// Token: 0x060027FB RID: 10235 RVA: 0x000BB7A2 File Offset: 0x000B9BA2
		public ValueMapping(AmbiPadValue _gamePadValue, ControlPadInput.Value[] _realValues)
		{
			this.GamepadValue = _gamePadValue;
			this.RealValues = _realValues;
		}

		// Token: 0x04001F87 RID: 8071
		public AmbiPadValue GamepadValue;

		// Token: 0x04001F88 RID: 8072
		public ControlPadInput.Value[] RealValues;
	}
}
