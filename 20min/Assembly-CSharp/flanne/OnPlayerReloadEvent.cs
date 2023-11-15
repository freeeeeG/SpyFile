using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A7 RID: 167
	public class OnPlayerReloadEvent : MonoBehaviour
	{
		// Token: 0x0600059E RID: 1438 RVA: 0x0001AD08 File Offset: 0x00018F08
		private void OnReload()
		{
			UnityEvent unityEvent = this.onReload;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001AD1C File Offset: 0x00018F1C
		private void Start()
		{
			PlayerController componentInParent = base.transform.GetComponentInParent<PlayerController>();
			this.ammo = componentInParent.ammo;
			this.ammo.OnReload.AddListener(new UnityAction(this.OnReload));
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001AD5D File Offset: 0x00018F5D
		private void OnDestroy()
		{
			this.ammo.OnReload.RemoveListener(new UnityAction(this.OnReload));
		}

		// Token: 0x04000383 RID: 899
		public UnityEvent onReload;

		// Token: 0x04000384 RID: 900
		private Ammo ammo;
	}
}
