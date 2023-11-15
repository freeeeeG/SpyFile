using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x0200009F RID: 159
	public class OnBurnEvent : MonoBehaviour
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0001A9A1 File Offset: 0x00018BA1
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001A9BA File Offset: 0x00018BBA
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnInflictBurn), BurnSystem.InflictBurnEvent);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001A9D3 File Offset: 0x00018BD3
		private void OnInflictBurn(object sender, object args)
		{
			UnityEvent unityEvent = this.onBurn;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x04000374 RID: 884
		public UnityEvent onBurn;
	}
}
