using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A2 RID: 162
	public class OnEnemyDeathEvent : MonoBehaviour
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0001AA21 File Offset: 0x00018C21
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001AA3A File Offset: 0x00018C3A
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnDeath), Health.DeathEvent);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001AA53 File Offset: 0x00018C53
		private void OnDeath(object sender, object args)
		{
			UnityEvent unityEvent = this.onEnemyDeath;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000378 RID: 888
		public UnityEvent onEnemyDeath;
	}
}
