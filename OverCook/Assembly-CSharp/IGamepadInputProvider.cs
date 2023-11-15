using System;

// Token: 0x020001EB RID: 491
public interface IGamepadInputProvider
{
	// Token: 0x06000826 RID: 2086
	bool IsPadAttached(ControlPadInput.PadNum _pad);

	// Token: 0x06000827 RID: 2087
	bool IsDown(ControlPadInput.PadNum _pad, ControlPadInput.Button _button);

	// Token: 0x06000828 RID: 2088
	float GetValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value);
}
