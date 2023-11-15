using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x02000358 RID: 856
	public class GamePad
	{
		// Token: 0x0600105F RID: 4191 RVA: 0x0005E144 File Offset: 0x0005C544
		public unsafe static GamePadState GetState(PlayerIndex playerIndex)
		{
			uint num = Imports.XInputGamePadGetState((uint)playerIndex, GamePad.gamePadStatePointer);
			GamePadState.RawState rawState = *(GamePadState.RawState*)((void*)GamePad.gamePadStatePointer);
			return new GamePadState(num == 0U, rawState);
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005E177 File Offset: 0x0005C577
		public static void SetVibration(PlayerIndex playerIndex, float leftMotor, float rightMotor)
		{
			Imports.XInputGamePadSetState((uint)playerIndex, leftMotor, rightMotor);
		}

		// Token: 0x04000C66 RID: 3174
		private static IntPtr gamePadStatePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GamePadState.RawState)));
	}
}
