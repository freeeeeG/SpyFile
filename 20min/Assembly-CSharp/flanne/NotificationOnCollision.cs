using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000CC RID: 204
	public class NotificationOnCollision : MonoBehaviour
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x0001D260 File Offset: 0x0001B460
		private void OnCollisionEnter2D(Collision2D collision)
		{
			this.PostNotification(this.notification, collision.gameObject);
		}

		// Token: 0x04000437 RID: 1079
		[SerializeField]
		private string notification;
	}
}
