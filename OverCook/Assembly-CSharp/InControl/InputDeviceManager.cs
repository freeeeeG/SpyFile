using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x020002BD RID: 701
	public abstract class InputDeviceManager
	{
		// Token: 0x06000DCB RID: 3531
		public abstract void Update(ulong updateTick, float deltaTime);

		// Token: 0x06000DCC RID: 3532 RVA: 0x00044363 File Offset: 0x00042763
		public virtual void Destroy()
		{
		}

		// Token: 0x04000AC9 RID: 2761
		protected List<InputDevice> devices = new List<InputDevice>();
	}
}
