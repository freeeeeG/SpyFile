using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A5 RID: 165
	public class OnPlayerHealedEvent : MonoBehaviour
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0001AC30 File Offset: 0x00018E30
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.health = componentInParent.playerHealth;
			this.health.onHealed.AddListener(new UnityAction(this.OnHeal));
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001AC6C File Offset: 0x00018E6C
		private void OnDestroy()
		{
			this.health.onHealed.RemoveListener(new UnityAction(this.OnHeal));
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001AC8A File Offset: 0x00018E8A
		private void OnHeal()
		{
			UnityEvent unityEvent = this.onHealed;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x0400037F RID: 895
		public UnityEvent onHealed;

		// Token: 0x04000380 RID: 896
		private PlayerHealth health;
	}
}
