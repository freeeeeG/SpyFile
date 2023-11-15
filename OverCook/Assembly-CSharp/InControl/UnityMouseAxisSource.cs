using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D8 RID: 728
	public class UnityMouseAxisSource : InputControlSource
	{
		// Token: 0x06000F17 RID: 3863 RVA: 0x0004893F File Offset: 0x00046D3F
		public UnityMouseAxisSource()
		{
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00048947 File Offset: 0x00046D47
		public UnityMouseAxisSource(string axis)
		{
			this.MouseAxisQuery = "mouse " + axis;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00048960 File Offset: 0x00046D60
		public float GetValue(InputDevice inputDevice)
		{
			return Input.GetAxisRaw(this.MouseAxisQuery);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0004896D File Offset: 0x00046D6D
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000BC2 RID: 3010
		public string MouseAxisQuery;
	}
}
