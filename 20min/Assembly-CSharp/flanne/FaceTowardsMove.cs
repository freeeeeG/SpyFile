using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000097 RID: 151
	public class FaceTowardsMove : MonoBehaviour
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0001A2C8 File Offset: 0x000184C8
		private void Update()
		{
			this.sprite.flipX = (this.movement.vector.x < 0f);
		}

		// Token: 0x0400035C RID: 860
		[SerializeField]
		private SpriteRenderer sprite;

		// Token: 0x0400035D RID: 861
		[SerializeField]
		private MoveComponent2D movement;
	}
}
