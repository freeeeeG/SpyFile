using System;
using XInputDotNetPure;

// Token: 0x020001FC RID: 508
public class XInputProvider : Singleton<XInputProvider>, IGamepadInputProvider
{
	// Token: 0x06000886 RID: 2182 RVA: 0x00033F04 File Offset: 0x00032304
	public bool IsPadAttached(ControlPadInput.PadNum _pad)
	{
		return XInputProvider.GetState(_pad).IsConnected;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00033F20 File Offset: 0x00032320
	public bool IsDown(ControlPadInput.PadNum _pad, ControlPadInput.Button _button)
	{
		if (_button == ControlPadInput.Button.LTrigger)
		{
			return this.GetValue(_pad, ControlPadInput.Value.LTrigger) > 0.5f;
		}
		if (_button != ControlPadInput.Button.RTrigger)
		{
			GamePadState state = XInputProvider.GetState(_pad);
			return XInputProvider.GetButtonState(state, _button) == ButtonState.Pressed;
		}
		return this.GetValue(_pad, ControlPadInput.Value.RTrigger) > 0.5f;
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00033F74 File Offset: 0x00032374
	private static ButtonState GetButtonState(GamePadState _padState, ControlPadInput.Button _button)
	{
		switch (_button)
		{
		case ControlPadInput.Button.A:
			return _padState.Buttons.A;
		case ControlPadInput.Button.X:
			return _padState.Buttons.X;
		case ControlPadInput.Button.B:
			return _padState.Buttons.B;
		case ControlPadInput.Button.Y:
			return _padState.Buttons.Y;
		case ControlPadInput.Button.LB:
			return _padState.Buttons.LeftShoulder;
		case ControlPadInput.Button.RB:
			return _padState.Buttons.RightShoulder;
		case ControlPadInput.Button.DPadLeft:
			return _padState.DPad.Left;
		case ControlPadInput.Button.DPadRight:
			return _padState.DPad.Right;
		case ControlPadInput.Button.DPadUp:
			return _padState.DPad.Up;
		case ControlPadInput.Button.DPadDown:
			return _padState.DPad.Down;
		case ControlPadInput.Button.Back:
			return _padState.Buttons.Back;
		case ControlPadInput.Button.Start:
			return _padState.Buttons.Start;
		case ControlPadInput.Button.LeftAnalog:
			return _padState.Buttons.LeftStick;
		case ControlPadInput.Button.RightAnalog:
			return _padState.Buttons.RightStick;
		}
		return ButtonState.Released;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x000340BC File Offset: 0x000324BC
	public float GetValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value)
	{
		GamePadState state = XInputProvider.GetState(_pad);
		switch (_value)
		{
		case ControlPadInput.Value.LStickX:
			return state.ThumbSticks.Left.X;
		case ControlPadInput.Value.LStickY:
			return -state.ThumbSticks.Left.Y;
		case ControlPadInput.Value.RStickX:
			return state.ThumbSticks.Right.X;
		case ControlPadInput.Value.RStickY:
			return -state.ThumbSticks.Right.Y;
		case ControlPadInput.Value.DPadX:
			return XInputProvider.ButtonsToValue(state.DPad.Left, state.DPad.Right);
		case ControlPadInput.Value.DPadY:
			return -XInputProvider.ButtonsToValue(state.DPad.Down, state.DPad.Up);
		case ControlPadInput.Value.LTrigger:
			return state.Triggers.Left;
		case ControlPadInput.Value.RTrigger:
			return state.Triggers.Right;
		default:
			return 0f;
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x000341D8 File Offset: 0x000325D8
	private static float ButtonsToValue(ButtonState _negative, ButtonState _positive)
	{
		float num = (_negative != ButtonState.Pressed) ? 0f : -1f;
		float num2 = (_positive != ButtonState.Pressed) ? 0f : 1f;
		return num + num2;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00034214 File Offset: 0x00032614
	private static bool IsDown(ButtonState _state)
	{
		return _state == ButtonState.Pressed;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003421A File Offset: 0x0003261A
	private static GamePadState GetState(ControlPadInput.PadNum _pad)
	{
		return XInputProvider.s_stateCache[(int)_pad];
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003422C File Offset: 0x0003262C
	public static void Update()
	{
		for (int i = 0; i < XInputProvider.s_stateCache.Length; i++)
		{
			XInputProvider.s_stateCache[i] = GamePad.GetState((PlayerIndex)i);
		}
	}

	// Token: 0x0400078A RID: 1930
	private static GamePadState[] s_stateCache = new GamePadState[4];

	// Token: 0x0400078B RID: 1931
	public static int ControllerCount = 4;
}
