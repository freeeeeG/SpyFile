using System;

namespace XInputDotNetPure
{
	// Token: 0x0200034E RID: 846
	public struct GamePadButtons
	{
		// Token: 0x0600103D RID: 4157 RVA: 0x0005DCF0 File Offset: 0x0005C0F0
		internal GamePadButtons(ButtonState start, ButtonState back, ButtonState leftStick, ButtonState rightStick, ButtonState leftShoulder, ButtonState rightShoulder, ButtonState a, ButtonState b, ButtonState x, ButtonState y)
		{
			this.start = start;
			this.back = back;
			this.leftStick = leftStick;
			this.rightStick = rightStick;
			this.leftShoulder = leftShoulder;
			this.rightShoulder = rightShoulder;
			this.a = a;
			this.b = b;
			this.x = x;
			this.y = y;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0005DD4A File Offset: 0x0005C14A
		public ButtonState Start
		{
			get
			{
				return this.start;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x0005DD52 File Offset: 0x0005C152
		public ButtonState Back
		{
			get
			{
				return this.back;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0005DD5A File Offset: 0x0005C15A
		public ButtonState LeftStick
		{
			get
			{
				return this.leftStick;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0005DD62 File Offset: 0x0005C162
		public ButtonState RightStick
		{
			get
			{
				return this.rightStick;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0005DD6A File Offset: 0x0005C16A
		public ButtonState LeftShoulder
		{
			get
			{
				return this.leftShoulder;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0005DD72 File Offset: 0x0005C172
		public ButtonState RightShoulder
		{
			get
			{
				return this.rightShoulder;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0005DD7A File Offset: 0x0005C17A
		public ButtonState A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0005DD82 File Offset: 0x0005C182
		public ButtonState B
		{
			get
			{
				return this.b;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0005DD8A File Offset: 0x0005C18A
		public ButtonState X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0005DD92 File Offset: 0x0005C192
		public ButtonState Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x04000C30 RID: 3120
		private ButtonState start;

		// Token: 0x04000C31 RID: 3121
		private ButtonState back;

		// Token: 0x04000C32 RID: 3122
		private ButtonState leftStick;

		// Token: 0x04000C33 RID: 3123
		private ButtonState rightStick;

		// Token: 0x04000C34 RID: 3124
		private ButtonState leftShoulder;

		// Token: 0x04000C35 RID: 3125
		private ButtonState rightShoulder;

		// Token: 0x04000C36 RID: 3126
		private ButtonState a;

		// Token: 0x04000C37 RID: 3127
		private ButtonState b;

		// Token: 0x04000C38 RID: 3128
		private ButtonState x;

		// Token: 0x04000C39 RID: 3129
		private ButtonState y;
	}
}
