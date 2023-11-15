using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne
{
	// Token: 0x020000A0 RID: 160
	public class OnCollisionEvent : MonoBehaviour
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x0001A9E5 File Offset: 0x00018BE5
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.targetTag))
			{
				UnityEvent unityEvent = this.onCollision;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
			}
		}

		// Token: 0x04000375 RID: 885
		[SerializeField]
		private string targetTag;

		// Token: 0x04000376 RID: 886
		public UnityEvent onCollision;
	}
}
