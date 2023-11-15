using System;
using Level;
using Runnables;
using UnityEngine;

namespace CutScenes
{
	// Token: 0x020001AA RID: 426
	public sealed class OnPropDestroyedInvoker : MonoBehaviour
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x0001A345 File Offset: 0x00018545
		private void Awake()
		{
			this._prop.onDestroy += this._runable.Run;
		}

		// Token: 0x0400076B RID: 1899
		[SerializeField]
		private Prop _prop;

		// Token: 0x0400076C RID: 1900
		[SerializeField]
		private Runnable _runable;
	}
}
