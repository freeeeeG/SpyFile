using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runnables
{
	// Token: 0x020002D6 RID: 726
	public class InvokeUnityEvent : Runnable
	{
		// Token: 0x06000E8F RID: 3727 RVA: 0x0002D6BB File Offset: 0x0002B8BB
		public override void Run()
		{
			UnityEvent unityEvent = this._unityEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000C12 RID: 3090
		[SerializeField]
		private UnityEvent _unityEvent;
	}
}
