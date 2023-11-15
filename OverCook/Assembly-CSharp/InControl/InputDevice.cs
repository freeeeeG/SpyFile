using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002BC RID: 700
	public class InputDevice
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x000434C8 File Offset: 0x000418C8
		public InputDevice(string name)
		{
			this.Name = name;
			this.Meta = string.Empty;
			this.LastChangeTick = 0UL;
			this.Controls = new InputControl[83];
			this.LeftStickX = new OneAxisInputControl();
			this.LeftStickY = new OneAxisInputControl();
			this.LeftStick = new TwoAxisInputControl();
			this.RightStickX = new OneAxisInputControl();
			this.RightStickY = new OneAxisInputControl();
			this.RightStick = new TwoAxisInputControl();
			this.DPadX = new OneAxisInputControl();
			this.DPadY = new OneAxisInputControl();
			this.DPad = new TwoAxisInputControl();
			this.Command = this.AddControl(InputControlType.Command, "Command");
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00043583 File Offset: 0x00041983
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x0004358B File Offset: 0x0004198B
		public string Name { get; protected set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00043594 File Offset: 0x00041994
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0004359C File Offset: 0x0004199C
		public string Meta { get; protected set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x000435A5 File Offset: 0x000419A5
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x000435AD File Offset: 0x000419AD
		public ulong LastChangeTick { get; protected set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x000435B6 File Offset: 0x000419B6
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x000435BE File Offset: 0x000419BE
		public InputControl[] Controls { get; protected set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x000435C7 File Offset: 0x000419C7
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x000435CF File Offset: 0x000419CF
		public OneAxisInputControl LeftStickX { get; protected set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x000435D8 File Offset: 0x000419D8
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x000435E0 File Offset: 0x000419E0
		public OneAxisInputControl LeftStickY { get; protected set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x000435E9 File Offset: 0x000419E9
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x000435F1 File Offset: 0x000419F1
		public TwoAxisInputControl LeftStick { get; protected set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x000435FA File Offset: 0x000419FA
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x00043602 File Offset: 0x00041A02
		public OneAxisInputControl RightStickX { get; protected set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0004360B File Offset: 0x00041A0B
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00043613 File Offset: 0x00041A13
		public OneAxisInputControl RightStickY { get; protected set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x0004361C File Offset: 0x00041A1C
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x00043624 File Offset: 0x00041A24
		public TwoAxisInputControl RightStick { get; protected set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x0004362D File Offset: 0x00041A2D
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00043635 File Offset: 0x00041A35
		public OneAxisInputControl DPadX { get; protected set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0004363E File Offset: 0x00041A3E
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00043646 File Offset: 0x00041A46
		public OneAxisInputControl DPadY { get; protected set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0004364F File Offset: 0x00041A4F
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00043657 File Offset: 0x00041A57
		public TwoAxisInputControl DPad { get; protected set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00043660 File Offset: 0x00041A60
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00043668 File Offset: 0x00041A68
		public InputControl Command { get; protected set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00043671 File Offset: 0x00041A71
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00043679 File Offset: 0x00041A79
		public bool IsAttached { get; internal set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00043682 File Offset: 0x00041A82
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0004368A File Offset: 0x00041A8A
		internal bool RawSticks { get; set; }

		// Token: 0x06000D92 RID: 3474 RVA: 0x00043693 File Offset: 0x00041A93
		public bool HasControl(InputControlType inputControlType)
		{
			return this.Controls[(int)inputControlType] != null;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x000436A4 File Offset: 0x00041AA4
		public InputControl GetControl(InputControlType inputControlType)
		{
			InputControl inputControl = this.Controls[(int)inputControlType];
			return inputControl ?? InputControl.Null;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x000436C7 File Offset: 0x00041AC7
		public static InputControlType GetInputControlTypeByName(string inputControlName)
		{
			return (InputControlType)Enum.Parse(typeof(InputControlType), inputControlName);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000436E0 File Offset: 0x00041AE0
		public InputControl GetControlByName(string inputControlName)
		{
			InputControlType inputControlTypeByName = InputDevice.GetInputControlTypeByName(inputControlName);
			return this.GetControl(inputControlTypeByName);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x000436FC File Offset: 0x00041AFC
		public InputControl AddControl(InputControlType inputControlType, string handle)
		{
			InputControl inputControl = new InputControl(handle, inputControlType);
			this.Controls[(int)inputControlType] = inputControl;
			return inputControl;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0004371C File Offset: 0x00041B1C
		public InputControl AddControl(InputControlType inputControlType, string handle, float lowerDeadZone, float upperDeadZone)
		{
			InputControl inputControl = this.AddControl(inputControlType, handle);
			inputControl.LowerDeadZone = lowerDeadZone;
			inputControl.UpperDeadZone = upperDeadZone;
			return inputControl;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00043744 File Offset: 0x00041B44
		public void ClearInputState()
		{
			this.LeftStickX.ClearInputState();
			this.LeftStickY.ClearInputState();
			this.LeftStick.ClearInputState();
			this.RightStickX.ClearInputState();
			this.RightStickY.ClearInputState();
			this.RightStick.ClearInputState();
			this.DPadX.ClearInputState();
			this.DPadY.ClearInputState();
			this.DPad.ClearInputState();
			int num = this.Controls.Length;
			for (int i = 0; i < num; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.ClearInputState();
				}
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x000437E4 File Offset: 0x00041BE4
		internal void UpdateWithState(InputControlType inputControlType, bool state, ulong updateTick, float deltaTime)
		{
			this.GetControl(inputControlType).UpdateWithState(state, updateTick, deltaTime);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000437F7 File Offset: 0x00041BF7
		internal void UpdateWithValue(InputControlType inputControlType, float value, ulong updateTick, float deltaTime)
		{
			this.GetControl(inputControlType).UpdateWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0004380C File Offset: 0x00041C0C
		internal void UpdateLeftStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000438E8 File Offset: 0x00041CE8
		internal void UpdateLeftStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.LeftStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.LeftStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.LeftStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.LeftStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x000439C1 File Offset: 0x00041DC1
		internal void CommitLeftStick()
		{
			this.LeftStickUp.Commit();
			this.LeftStickDown.Commit();
			this.LeftStickLeft.Commit();
			this.LeftStickRight.Commit();
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000439F0 File Offset: 0x00041DF0
		internal void UpdateRightStickWithValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00043ACC File Offset: 0x00041ECC
		internal void UpdateRightStickWithRawValue(Vector2 value, ulong updateTick, float deltaTime)
		{
			this.RightStickLeft.UpdateWithRawValue(Mathf.Max(0f, -value.x), updateTick, deltaTime);
			this.RightStickRight.UpdateWithRawValue(Mathf.Max(0f, value.x), updateTick, deltaTime);
			if (InputManager.InvertYAxis)
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
			}
			else
			{
				this.RightStickUp.UpdateWithRawValue(Mathf.Max(0f, value.y), updateTick, deltaTime);
				this.RightStickDown.UpdateWithRawValue(Mathf.Max(0f, -value.y), updateTick, deltaTime);
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00043BA5 File Offset: 0x00041FA5
		internal void CommitRightStick()
		{
			this.RightStickUp.Commit();
			this.RightStickDown.Commit();
			this.RightStickLeft.Commit();
			this.RightStickRight.Commit();
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00043BD3 File Offset: 0x00041FD3
		public virtual void Update(ulong updateTick, float deltaTime)
		{
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00043BD8 File Offset: 0x00041FD8
		private bool AnyCommandControlIsPressed()
		{
			for (int i = 24; i <= 34; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null && inputControl.IsPressed)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00043C18 File Offset: 0x00042018
		internal void ProcessLeftStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.LeftStickLeft.NextRawValue, this.LeftStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.LeftStickDown.NextRawValue, this.LeftStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.LeftStickLeft.LowerDeadZone, this.LeftStickRight.LowerDeadZone, this.LeftStickUp.LowerDeadZone, this.LeftStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.LeftStickLeft.UpperDeadZone, this.LeftStickRight.UpperDeadZone, this.LeftStickUp.UpperDeadZone, this.LeftStickDown.UpperDeadZone);
				vector = Utility.ApplyCircularDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.LeftStick.Raw = true;
			this.LeftStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.LeftStickX.Raw = true;
			this.LeftStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.LeftStickY.Raw = true;
			this.LeftStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.LeftStickLeft.SetValue(this.LeftStick.Left.Value, updateTick);
			this.LeftStickRight.SetValue(this.LeftStick.Right.Value, updateTick);
			this.LeftStickUp.SetValue(this.LeftStick.Up.Value, updateTick);
			this.LeftStickDown.SetValue(this.LeftStick.Down.Value, updateTick);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00043DC4 File Offset: 0x000421C4
		internal void ProcessRightStick(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.RightStickLeft.NextRawValue, this.RightStickRight.NextRawValue);
			float y = Utility.ValueFromSides(this.RightStickDown.NextRawValue, this.RightStickUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector;
			if (this.RawSticks)
			{
				vector = new Vector2(x, y);
			}
			else
			{
				float lowerDeadZone = Utility.Max(this.RightStickLeft.LowerDeadZone, this.RightStickRight.LowerDeadZone, this.RightStickUp.LowerDeadZone, this.RightStickDown.LowerDeadZone);
				float upperDeadZone = Utility.Min(this.RightStickLeft.UpperDeadZone, this.RightStickRight.UpperDeadZone, this.RightStickUp.UpperDeadZone, this.RightStickDown.UpperDeadZone);
				vector = Utility.ApplyCircularDeadZone(x, y, lowerDeadZone, upperDeadZone);
			}
			this.RightStick.Raw = true;
			this.RightStick.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.RightStickX.Raw = true;
			this.RightStickX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.RightStickY.Raw = true;
			this.RightStickY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.RightStickLeft.SetValue(this.RightStick.Left.Value, updateTick);
			this.RightStickRight.SetValue(this.RightStick.Right.Value, updateTick);
			this.RightStickUp.SetValue(this.RightStick.Up.Value, updateTick);
			this.RightStickDown.SetValue(this.RightStick.Down.Value, updateTick);
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00043F70 File Offset: 0x00042370
		internal void ProcessDPad(ulong updateTick, float deltaTime)
		{
			float lowerDeadZone = Utility.Max(this.DPadLeft.LowerDeadZone, this.DPadRight.LowerDeadZone, this.DPadUp.LowerDeadZone, this.DPadDown.LowerDeadZone);
			float upperDeadZone = Utility.Min(this.DPadLeft.UpperDeadZone, this.DPadRight.UpperDeadZone, this.DPadUp.UpperDeadZone, this.DPadDown.UpperDeadZone);
			float x = Utility.ValueFromSides(this.DPadLeft.NextRawValue, this.DPadRight.NextRawValue);
			float y = Utility.ValueFromSides(this.DPadDown.NextRawValue, this.DPadUp.NextRawValue, InputManager.InvertYAxis);
			Vector2 vector = Utility.ApplyCircularDeadZone(x, y, lowerDeadZone, upperDeadZone);
			this.DPad.Raw = true;
			this.DPad.UpdateWithAxes(vector.x, vector.y, updateTick, deltaTime);
			this.DPadX.Raw = true;
			this.DPadX.CommitWithValue(vector.x, updateTick, deltaTime);
			this.DPadY.Raw = true;
			this.DPadY.CommitWithValue(vector.y, updateTick, deltaTime);
			this.DPadLeft.SetValue(this.DPad.Left.Value, updateTick);
			this.DPadRight.SetValue(this.DPad.Right.Value, updateTick);
			this.DPadUp.SetValue(this.DPad.Up.Value, updateTick);
			this.DPadDown.SetValue(this.DPad.Down.Value, updateTick);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00044100 File Offset: 0x00042500
		public void Commit(ulong updateTick, float deltaTime)
		{
			this.ProcessLeftStick(updateTick, deltaTime);
			this.ProcessRightStick(updateTick, deltaTime);
			this.ProcessDPad(updateTick, deltaTime);
			int num = this.Controls.Length;
			for (int i = 0; i < num; i++)
			{
				InputControl inputControl = this.Controls[i];
				if (inputControl != null)
				{
					inputControl.Commit();
					if (inputControl.HasChanged)
					{
						this.LastChangeTick = updateTick;
					}
				}
			}
			if (this.IsKnown)
			{
				this.Command.CommitWithState(this.AnyCommandControlIsPressed(), updateTick, deltaTime);
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00044185 File Offset: 0x00042585
		public bool LastChangedAfter(InputDevice otherDevice)
		{
			return this.LastChangeTick > otherDevice.LastChangeTick;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00044195 File Offset: 0x00042595
		internal void RequestActivation()
		{
			this.LastChangeTick = InputManager.CurrentTick;
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000441A2 File Offset: 0x000425A2
		public virtual void Vibrate(float leftMotor, float rightMotor)
		{
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000441A4 File Offset: 0x000425A4
		public void Vibrate(float intensity)
		{
			this.Vibrate(intensity, intensity);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x000441AE File Offset: 0x000425AE
		public void StopVibration()
		{
			this.Vibrate(0f);
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x000441BB File Offset: 0x000425BB
		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x000441BE File Offset: 0x000425BE
		public virtual bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x000441C1 File Offset: 0x000425C1
		public bool IsUnknown
		{
			get
			{
				return !this.IsKnown;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x000441CC File Offset: 0x000425CC
		public bool MenuWasPressed
		{
			get
			{
				return this.GetControl(InputControlType.Command).WasPressed;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000441DC File Offset: 0x000425DC
		public InputControl AnyButton
		{
			get
			{
				int length = this.Controls.GetLength(0);
				for (int i = 0; i < length; i++)
				{
					InputControl inputControl = this.Controls[i];
					if (inputControl != null && inputControl.IsButton && inputControl.IsPressed)
					{
						return inputControl;
					}
				}
				return InputControl.Null;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00044234 File Offset: 0x00042634
		public InputControl LeftStickUp
		{
			get
			{
				return this.GetControl(InputControlType.LeftStickUp);
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x0004423D File Offset: 0x0004263D
		public InputControl LeftStickDown
		{
			get
			{
				return this.GetControl(InputControlType.LeftStickDown);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00044246 File Offset: 0x00042646
		public InputControl LeftStickLeft
		{
			get
			{
				return this.GetControl(InputControlType.LeftStickLeft);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0004424F File Offset: 0x0004264F
		public InputControl LeftStickRight
		{
			get
			{
				return this.GetControl(InputControlType.LeftStickRight);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00044258 File Offset: 0x00042658
		public InputControl RightStickUp
		{
			get
			{
				return this.GetControl(InputControlType.RightStickUp);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00044261 File Offset: 0x00042661
		public InputControl RightStickDown
		{
			get
			{
				return this.GetControl(InputControlType.RightStickDown);
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0004426A File Offset: 0x0004266A
		public InputControl RightStickLeft
		{
			get
			{
				return this.GetControl(InputControlType.RightStickLeft);
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00044273 File Offset: 0x00042673
		public InputControl RightStickRight
		{
			get
			{
				return this.GetControl(InputControlType.RightStickRight);
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0004427D File Offset: 0x0004267D
		public InputControl DPadUp
		{
			get
			{
				return this.GetControl(InputControlType.DPadUp);
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00044287 File Offset: 0x00042687
		public InputControl DPadDown
		{
			get
			{
				return this.GetControl(InputControlType.DPadDown);
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x00044291 File Offset: 0x00042691
		public InputControl DPadLeft
		{
			get
			{
				return this.GetControl(InputControlType.DPadLeft);
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0004429B File Offset: 0x0004269B
		public InputControl DPadRight
		{
			get
			{
				return this.GetControl(InputControlType.DPadRight);
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x000442A5 File Offset: 0x000426A5
		public InputControl Action1
		{
			get
			{
				return this.GetControl(InputControlType.Action1);
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x000442AF File Offset: 0x000426AF
		public InputControl Action2
		{
			get
			{
				return this.GetControl(InputControlType.Action2);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x000442B9 File Offset: 0x000426B9
		public InputControl Action3
		{
			get
			{
				return this.GetControl(InputControlType.Action3);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x000442C3 File Offset: 0x000426C3
		public InputControl Action4
		{
			get
			{
				return this.GetControl(InputControlType.Action4);
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000442CD File Offset: 0x000426CD
		public InputControl LeftTrigger
		{
			get
			{
				return this.GetControl(InputControlType.LeftTrigger);
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x000442D7 File Offset: 0x000426D7
		public InputControl RightTrigger
		{
			get
			{
				return this.GetControl(InputControlType.RightTrigger);
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000442E1 File Offset: 0x000426E1
		public InputControl LeftBumper
		{
			get
			{
				return this.GetControl(InputControlType.LeftBumper);
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x000442EB File Offset: 0x000426EB
		public InputControl RightBumper
		{
			get
			{
				return this.GetControl(InputControlType.RightBumper);
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x000442F5 File Offset: 0x000426F5
		public InputControl LeftStickButton
		{
			get
			{
				return this.GetControl(InputControlType.LeftStickButton);
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x000442FE File Offset: 0x000426FE
		public InputControl RightStickButton
		{
			get
			{
				return this.GetControl(InputControlType.RightStickButton);
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00044308 File Offset: 0x00042708
		public TwoAxisInputControl Direction
		{
			get
			{
				return (this.DPad.UpdateTick <= this.LeftStick.UpdateTick) ? this.LeftStick : this.DPad;
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00044336 File Offset: 0x00042736
		public static implicit operator bool(InputDevice device)
		{
			return device != null;
		}

		// Token: 0x04000AB7 RID: 2743
		public static readonly InputDevice Null = new InputDevice("None");

		// Token: 0x04000AB8 RID: 2744
		internal int SortOrder = int.MaxValue;
	}
}
