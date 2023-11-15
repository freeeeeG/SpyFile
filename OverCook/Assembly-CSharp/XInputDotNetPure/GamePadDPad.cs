using System;

namespace XInputDotNetPure
{
	// Token: 0x0200034F RID: 847
	public struct GamePadDPad
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x0005DD9A File Offset: 0x0005C19A
		internal GamePadDPad(ButtonState up, ButtonState down, ButtonState left, ButtonState right)
		{
			this.up = up;
			this.down = down;
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x0005DDB9 File Offset: 0x0005C1B9
		public ButtonState Up
		{
			get
			{
				return this.up;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x0005DDC1 File Offset: 0x0005C1C1
		public ButtonState Down
		{
			get
			{
				return this.down;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x0005DDC9 File Offset: 0x0005C1C9
		public ButtonState Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x0005DDD1 File Offset: 0x0005C1D1
		public ButtonState Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000C3A RID: 3130
		private ButtonState up;

		// Token: 0x04000C3B RID: 3131
		private ButtonState down;

		// Token: 0x04000C3C RID: 3132
		private ButtonState left;

		// Token: 0x04000C3D RID: 3133
		private ButtonState right;
	}
}
