using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000C0 RID: 192
	public class HomingProjectile : MonoBehaviour
	{
		// Token: 0x06000630 RID: 1584 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		private void OnEnable()
		{
			this._target = EnemyFinder.GetRandomEnemy(Vector3.zero, Vector3.positiveInfinity);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0001C7E8 File Offset: 0x0001A9E8
		private void FixedUpdate()
		{
			if (this._target != null && this._target.activeSelf)
			{
				Vector2 b = base.transform.position;
				Vector2 vector = this._target.transform.position - b;
				this.moveComponent.vector += vector.normalized * this.acceleration * Time.fixedDeltaTime;
				return;
			}
			this._target = EnemyFinder.GetRandomEnemy(Vector3.zero, Vector3.positiveInfinity);
		}

		// Token: 0x040003FD RID: 1021
		[SerializeField]
		private MoveComponent2D moveComponent;

		// Token: 0x040003FE RID: 1022
		[SerializeField]
		private float acceleration;

		// Token: 0x040003FF RID: 1023
		private GameObject _target;
	}
}
