using System;

namespace InControl
{
	// Token: 0x02000296 RID: 662
	public interface BindingSourceListener
	{
		// Token: 0x06000C36 RID: 3126
		void Reset();

		// Token: 0x06000C37 RID: 3127
		BindingSource Listen(BindingListenOptions listenOptions, InputDevice device);
	}
}
