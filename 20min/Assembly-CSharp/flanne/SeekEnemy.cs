using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200010D RID: 269
	public class SeekEnemy : MonoBehaviour
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x00020D78 File Offset: 0x0001EF78
		private void Start()
		{
			this.player = PlayerController.Instance.transform;
			this.GetNewTarget();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00020D90 File Offset: 0x0001EF90
		private void FixedUpdate()
		{
			if (this._target != null)
			{
				Vector2 b = base.transform.position;
				Vector2 a = this._target.position;
				if (Vector2.Distance(a, b) < 1f)
				{
					this.GetNewTarget();
				}
				Vector2 vector = a - b;
				this.moveComponent.vector += vector.normalized * this.acceleration * Time.fixedDeltaTime;
				return;
			}
			this.GetNewTarget();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00020E20 File Offset: 0x0001F020
		private void GetNewTarget()
		{
			Vector2 center = this.player.transform.position;
			Vector2 range = new Vector2(this.seekDistanceX, this.seekDistanceY);
			GameObject randomEnemy = EnemyFinder.GetRandomEnemy(center, range);
			if (randomEnemy == null)
			{
				this._target = this.player;
				return;
			}
			this._target = randomEnemy.transform;
		}

		// Token: 0x04000571 RID: 1393
		[SerializeField]
		private MoveComponent2D moveComponent;

		// Token: 0x04000572 RID: 1394
		public float acceleration;

		// Token: 0x04000573 RID: 1395
		[SerializeField]
		private float seekDistanceX;

		// Token: 0x04000574 RID: 1396
		[SerializeField]
		private float seekDistanceY;

		// Token: 0x04000575 RID: 1397
		public Transform player;

		// Token: 0x04000576 RID: 1398
		private Transform _target;
	}
}
