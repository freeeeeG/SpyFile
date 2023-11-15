using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A1 RID: 161
	public class OnDisableEvent : MonoBehaviour
	{
		// Token: 0x06000584 RID: 1412 RVA: 0x0001AA0F File Offset: 0x00018C0F
		private void OnDisable()
		{
			UnityEvent unityEvent = this.onDisableEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000377 RID: 887
		[SerializeField]
		private UnityEvent onDisableEvent;
	}
}
