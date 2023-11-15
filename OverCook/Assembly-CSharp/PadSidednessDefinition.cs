using System;

// Token: 0x0200082F RID: 2095
internal static class PadSidednessDefinition
{
	// Token: 0x0600280F RID: 10255 RVA: 0x000BBC22 File Offset: 0x000BA022
	public static ControlPadInput.Button[] FilterForSide(PadSide _side, ControlPadInput.Button[] _buttons)
	{
		if (_side == PadSide.Both)
		{
			return _buttons;
		}
		if (_side == PadSide.Left)
		{
			return _buttons.Intersection(PadSidednessDefinition.LeftSideButtons);
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return _buttons.Intersection(PadSidednessDefinition.RightSideButtons);
	}

	// Token: 0x06002810 RID: 10256 RVA: 0x000BBC58 File Offset: 0x000BA058
	public static ControlPadInput.Value[] FilterForSide(PadSide _side, ControlPadInput.Value[] _values)
	{
		if (_side == PadSide.Both)
		{
			return _values;
		}
		if (_side == PadSide.Left)
		{
			return _values.Intersection(PadSidednessDefinition.LeftSideValues);
		}
		if (_side != PadSide.Right)
		{
			return null;
		}
		return _values.Intersection(PadSidednessDefinition.RightSideValues);
	}

	// Token: 0x04001F9C RID: 8092
	private static ControlPadInput.Button[] LeftSideButtons = new ControlPadInput.Button[]
	{
		ControlPadInput.Button.LB,
		ControlPadInput.Button.LTrigger,
		ControlPadInput.Button.Back,
		ControlPadInput.Button.LeftAnalog,
		ControlPadInput.Button.DPadLeft,
		ControlPadInput.Button.DPadRight,
		ControlPadInput.Button.DPadUp,
		ControlPadInput.Button.DPadDown
	};

	// Token: 0x04001F9D RID: 8093
	private static ControlPadInput.Button[] RightSideButtons = new ControlPadInput.Button[]
	{
		ControlPadInput.Button.RB,
		ControlPadInput.Button.RTrigger,
		ControlPadInput.Button.A,
		ControlPadInput.Button.B,
		ControlPadInput.Button.X,
		ControlPadInput.Button.Y,
		ControlPadInput.Button.Start,
		ControlPadInput.Button.RightAnalog
	};

	// Token: 0x04001F9E RID: 8094
	private static ControlPadInput.Value[] LeftSideValues = new ControlPadInput.Value[]
	{
		ControlPadInput.Value.LStickX,
		ControlPadInput.Value.LStickY,
		ControlPadInput.Value.DPadX,
		ControlPadInput.Value.DPadY,
		ControlPadInput.Value.LTrigger
	};

	// Token: 0x04001F9F RID: 8095
	private static ControlPadInput.Value[] RightSideValues = new ControlPadInput.Value[]
	{
		ControlPadInput.Value.RStickX,
		ControlPadInput.Value.RStickY,
		ControlPadInput.Value.RTrigger
	};
}
