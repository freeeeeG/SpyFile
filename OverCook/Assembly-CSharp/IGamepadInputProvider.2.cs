using System;

// Token: 0x020001EC RID: 492
public interface IGamepadInputProvider<ButtonID, ValueID>
{
	// Token: 0x06000829 RID: 2089
	bool IsPadAttached(ControlPadInput.PadNum _pad);

	// Token: 0x0600082A RID: 2090
	bool IsDown(ControlPadInput.PadNum _pad, ButtonID _button);

	// Token: 0x0600082B RID: 2091
	float GetValue(ControlPadInput.PadNum _pad, ValueID _value);
}
