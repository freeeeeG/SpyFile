﻿using System;
using XInputDotNetPure;

namespace InControl
{
	// Token: 0x0200034A RID: 842
	public class XInputDevice : InputDevice
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x0005D494 File Offset: 0x0005B894
		public XInputDevice(int deviceIndex, XInputDeviceManager owner) : base("XInput Controller")
		{
			this.owner = owner;
			this.DeviceIndex = deviceIndex;
			this.SortOrder = deviceIndex;
			base.Meta = "XInput Device #" + deviceIndex;
			base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left", 0.4f, 0.9f);
			base.AddControl(InputControlType.LeftStickRight, "Left Stick Right", 0.4f, 0.9f);
			base.AddControl(InputControlType.LeftStickUp, "Left Stick Up", 0.4f, 0.9f);
			base.AddControl(InputControlType.LeftStickDown, "Left Stick Down", 0.4f, 0.9f);
			base.AddControl(InputControlType.RightStickLeft, "Right Stick Left", 0.4f, 0.9f);
			base.AddControl(InputControlType.RightStickRight, "Right Stick Right", 0.4f, 0.9f);
			base.AddControl(InputControlType.RightStickUp, "Right Stick Up", 0.4f, 0.9f);
			base.AddControl(InputControlType.RightStickDown, "Right Stick Down", 0.4f, 0.9f);
			base.AddControl(InputControlType.LeftTrigger, "Left Trigger", 0.4f, 0.9f);
			base.AddControl(InputControlType.RightTrigger, "Right Trigger", 0.4f, 0.9f);
			base.AddControl(InputControlType.DPadUp, "DPad Up", 0.4f, 0.9f);
			base.AddControl(InputControlType.DPadDown, "DPad Down", 0.4f, 0.9f);
			base.AddControl(InputControlType.DPadLeft, "DPad Left", 0.4f, 0.9f);
			base.AddControl(InputControlType.DPadRight, "DPad Right", 0.4f, 0.9f);
			base.AddControl(InputControlType.Action1, "A");
			base.AddControl(InputControlType.Action2, "B");
			base.AddControl(InputControlType.Action3, "X");
			base.AddControl(InputControlType.Action4, "Y");
			base.AddControl(InputControlType.LeftBumper, "Left Bumper");
			base.AddControl(InputControlType.RightBumper, "Right Bumper");
			base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
			base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
			base.AddControl(InputControlType.Start, "Start");
			base.AddControl(InputControlType.Back, "Back");
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x0005D6AB File Offset: 0x0005BAAB
		// (set) Token: 0x06001028 RID: 4136 RVA: 0x0005D6B3 File Offset: 0x0005BAB3
		public int DeviceIndex { get; private set; }

		// Token: 0x06001029 RID: 4137 RVA: 0x0005D6BC File Offset: 0x0005BABC
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.GetState();
			base.UpdateLeftStickWithValue(this.state.ThumbSticks.Left.Vector, updateTick, deltaTime);
			base.UpdateRightStickWithValue(this.state.ThumbSticks.Right.Vector, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.LeftTrigger, this.state.Triggers.Left, updateTick, deltaTime);
			base.UpdateWithValue(InputControlType.RightTrigger, this.state.Triggers.Right, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadUp, this.state.DPad.Up == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadDown, this.state.DPad.Down == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadLeft, this.state.DPad.Left == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.DPadRight, this.state.DPad.Right == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action1, this.state.Buttons.A == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action2, this.state.Buttons.B == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action3, this.state.Buttons.X == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Action4, this.state.Buttons.Y == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftBumper, this.state.Buttons.LeftShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightBumper, this.state.Buttons.RightShoulder == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.LeftStickButton, this.state.Buttons.LeftStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.RightStickButton, this.state.Buttons.RightStick == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Start, this.state.Buttons.Start == ButtonState.Pressed, updateTick, deltaTime);
			base.UpdateWithState(InputControlType.Back, this.state.Buttons.Back == ButtonState.Pressed, updateTick, deltaTime);
			base.Commit(updateTick, deltaTime);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0005D926 File Offset: 0x0005BD26
		public override void Vibrate(float leftMotor, float rightMotor)
		{
			GamePad.SetVibration((PlayerIndex)this.DeviceIndex, leftMotor, rightMotor);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0005D935 File Offset: 0x0005BD35
		internal void GetState()
		{
			this.state = this.owner.GetState(this.DeviceIndex);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x0005D94E File Offset: 0x0005BD4E
		public bool IsConnected
		{
			get
			{
				return this.state.IsConnected;
			}
		}

		// Token: 0x04000C22 RID: 3106
		private const float LowerDeadZone = 0.4f;

		// Token: 0x04000C23 RID: 3107
		private const float UpperDeadZone = 0.9f;

		// Token: 0x04000C24 RID: 3108
		private XInputDeviceManager owner;

		// Token: 0x04000C25 RID: 3109
		private GamePadState state;
	}
}
