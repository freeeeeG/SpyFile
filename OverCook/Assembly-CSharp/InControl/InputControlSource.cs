using System;

namespace InControl
{
	// Token: 0x020002B0 RID: 688
	public interface InputControlSource
	{
		// Token: 0x06000D2F RID: 3375
		float GetValue(InputDevice inputDevice);

		// Token: 0x06000D30 RID: 3376
		bool GetState(InputDevice inputDevice);
	}
}
