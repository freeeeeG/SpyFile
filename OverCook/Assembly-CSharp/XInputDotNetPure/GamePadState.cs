using System;

namespace XInputDotNetPure
{
	// Token: 0x02000353 RID: 851
	public struct GamePadState
	{
		// Token: 0x06001057 RID: 4183 RVA: 0x0005DE4C File Offset: 0x0005C24C
		internal GamePadState(bool isConnected, GamePadState.RawState rawState)
		{
			this.isConnected = isConnected;
			if (!isConnected)
			{
				rawState.dwPacketNumber = 0U;
				rawState.Gamepad.dwButtons = 0;
				rawState.Gamepad.bLeftTrigger = 0;
				rawState.Gamepad.bRightTrigger = 0;
				rawState.Gamepad.sThumbLX = 0;
				rawState.Gamepad.sThumbLY = 0;
				rawState.Gamepad.sThumbRX = 0;
				rawState.Gamepad.sThumbRY = 0;
			}
			this.packetNumber = rawState.dwPacketNumber;
			this.buttons = new GamePadButtons(((rawState.Gamepad.dwButtons & 16) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 64) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 128) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 256) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 512) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4096) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8192) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 16384) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 32768) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.dPad = new GamePadDPad(((rawState.Gamepad.dwButtons & 1) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 2) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 4) == 0) ? ButtonState.Released : ButtonState.Pressed, ((rawState.Gamepad.dwButtons & 8) == 0) ? ButtonState.Released : ButtonState.Pressed);
			this.thumbSticks = new GamePadThumbSticks(new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbLX / 32767f, (float)rawState.Gamepad.sThumbLY / 32767f), new GamePadThumbSticks.StickValue((float)rawState.Gamepad.sThumbRX / 32767f, (float)rawState.Gamepad.sThumbRY / 32767f));
			this.triggers = new GamePadTriggers((float)rawState.Gamepad.bLeftTrigger / 255f, (float)rawState.Gamepad.bRightTrigger / 255f);
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001058 RID: 4184 RVA: 0x0005E109 File Offset: 0x0005C509
		public uint PacketNumber
		{
			get
			{
				return this.packetNumber;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0005E111 File Offset: 0x0005C511
		public bool IsConnected
		{
			get
			{
				return this.isConnected;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x0005E119 File Offset: 0x0005C519
		public GamePadButtons Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x0005E121 File Offset: 0x0005C521
		public GamePadDPad DPad
		{
			get
			{
				return this.dPad;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0005E129 File Offset: 0x0005C529
		public GamePadTriggers Triggers
		{
			get
			{
				return this.triggers;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x0005E131 File Offset: 0x0005C531
		public GamePadThumbSticks ThumbSticks
		{
			get
			{
				return this.thumbSticks;
			}
		}

		// Token: 0x04000C43 RID: 3139
		private bool isConnected;

		// Token: 0x04000C44 RID: 3140
		private uint packetNumber;

		// Token: 0x04000C45 RID: 3141
		private GamePadButtons buttons;

		// Token: 0x04000C46 RID: 3142
		private GamePadDPad dPad;

		// Token: 0x04000C47 RID: 3143
		private GamePadThumbSticks thumbSticks;

		// Token: 0x04000C48 RID: 3144
		private GamePadTriggers triggers;

		// Token: 0x02000354 RID: 852
		internal struct RawState
		{
			// Token: 0x04000C49 RID: 3145
			public uint dwPacketNumber;

			// Token: 0x04000C4A RID: 3146
			public GamePadState.RawState.GamePad Gamepad;

			// Token: 0x02000355 RID: 853
			public struct GamePad
			{
				// Token: 0x04000C4B RID: 3147
				public ushort dwButtons;

				// Token: 0x04000C4C RID: 3148
				public byte bLeftTrigger;

				// Token: 0x04000C4D RID: 3149
				public byte bRightTrigger;

				// Token: 0x04000C4E RID: 3150
				public short sThumbLX;

				// Token: 0x04000C4F RID: 3151
				public short sThumbLY;

				// Token: 0x04000C50 RID: 3152
				public short sThumbRX;

				// Token: 0x04000C51 RID: 3153
				public short sThumbRY;
			}
		}

		// Token: 0x02000356 RID: 854
		private enum ButtonsConstants
		{
			// Token: 0x04000C53 RID: 3155
			DPadUp = 1,
			// Token: 0x04000C54 RID: 3156
			DPadDown,
			// Token: 0x04000C55 RID: 3157
			DPadLeft = 4,
			// Token: 0x04000C56 RID: 3158
			DPadRight = 8,
			// Token: 0x04000C57 RID: 3159
			Start = 16,
			// Token: 0x04000C58 RID: 3160
			Back = 32,
			// Token: 0x04000C59 RID: 3161
			LeftThumb = 64,
			// Token: 0x04000C5A RID: 3162
			RightThumb = 128,
			// Token: 0x04000C5B RID: 3163
			LeftShoulder = 256,
			// Token: 0x04000C5C RID: 3164
			RightShoulder = 512,
			// Token: 0x04000C5D RID: 3165
			A = 4096,
			// Token: 0x04000C5E RID: 3166
			B = 8192,
			// Token: 0x04000C5F RID: 3167
			X = 16384,
			// Token: 0x04000C60 RID: 3168
			Y = 32768
		}
	}
}
