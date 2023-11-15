using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A6 RID: 166
	public class OnPlayerHurtEvent : MonoBehaviour
	{
		// Token: 0x0600059A RID: 1434 RVA: 0x0001AC9C File Offset: 0x00018E9C
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.health = componentInParent.playerHealth;
			this.health.onHurt.AddListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001ACD8 File Offset: 0x00018ED8
		private void OnDestroy()
		{
			this.health.onHurt.RemoveListener(new UnityAction(this.OnHurt));
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001ACF6 File Offset: 0x00018EF6
		private void OnHurt()
		{
			UnityEvent unityEvent = this.onHurt;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000381 RID: 897
		public UnityEvent onHurt;

		// Token: 0x04000382 RID: 898
		private PlayerHealth health;
	}
}
