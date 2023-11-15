using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000BF RID: 191
	public class HomeTowardsPlayer : MonoBehaviour
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x0001C751 File Offset: 0x0001A951
		private void Start()
		{
			this.playerTransform = PlayerController.Instance.transform;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001C764 File Offset: 0x0001A964
		private void FixedUpdate()
		{
			Vector2 vector = this.playerTransform.position - base.transform.position;
			this.moveComponent.vector += vector.normalized * this.acceleration * Time.fixedDeltaTime;
		}

		// Token: 0x040003FA RID: 1018
		[SerializeField]
		private MoveComponent2D moveComponent;

		// Token: 0x040003FB RID: 1019
		[SerializeField]
		private float acceleration;

		// Token: 0x040003FC RID: 1020
		private Transform playerTransform;
	}
}
