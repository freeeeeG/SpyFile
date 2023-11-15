using System;
using System.Runtime.InteropServices;

namespace XInputDotNetPure
{
	// Token: 0x0200034C RID: 844
	internal class Imports
	{
		// Token: 0x06001037 RID: 4151
		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState32(uint playerIndex, IntPtr state);

		// Token: 0x06001038 RID: 4152
		[DllImport("XInputInterface32", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState32(uint playerIndex, float leftMotor, float rightMotor);

		// Token: 0x06001039 RID: 4153
		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadGetState")]
		public static extern uint XInputGamePadGetState64(uint playerIndex, IntPtr state);

		// Token: 0x0600103A RID: 4154
		[DllImport("XInputInterface64", EntryPoint = "XInputGamePadSetState")]
		public static extern void XInputGamePadSetState64(uint playerIndex, float leftMotor, float rightMotor);

		// Token: 0x0600103B RID: 4155 RVA: 0x0005DCB0 File Offset: 0x0005C0B0
		public static uint XInputGamePadGetState(uint playerIndex, IntPtr state)
		{
			if (IntPtr.Size == 4)
			{
				return Imports.XInputGamePadGetState32(playerIndex, state);
			}
			return Imports.XInputGamePadGetState64(playerIndex, state);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0005DCCC File Offset: 0x0005C0CC
		public static void XInputGamePadSetState(uint playerIndex, float leftMotor, float rightMotor)
		{
			if (IntPtr.Size == 4)
			{
				Imports.XInputGamePadSetState32(playerIndex, leftMotor, rightMotor);
			}
			else
			{
				Imports.XInputGamePadSetState64(playerIndex, leftMotor, rightMotor);
			}
		}
	}
}
