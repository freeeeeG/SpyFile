using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E0B RID: 3595
	public class TakeAimObject : CharacterOperation
	{
		// Token: 0x060047D4 RID: 18388 RVA: 0x000D10E4 File Offset: 0x000CF2E4
		public override void Run(Character owner)
		{
			if (this._target == null)
			{
				this._target = Singleton<Service>.Instance.levelManager.player.transform;
			}
			float y;
			if (this._platformTarget)
			{
				Collider2D collider2D = this.FindTargetPlatform();
				if (collider2D == null)
				{
					return;
				}
				y = collider2D.bounds.max.y;
			}
			else
			{
				y = this._target.transform.position.y;
			}
			Vector3 vector = new Vector3(this._target.transform.position.x, y) - this._centerAxisPosition.transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			this._centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x000D11C4 File Offset: 0x000CF3C4
		private Collider2D FindTargetPlatform()
		{
			RaycastHit2D hit = Physics2D.Raycast(this._target.position, Vector2.down, float.PositiveInfinity, Layers.groundMask);
			if (hit)
			{
				return hit.collider;
			}
			return null;
		}

		// Token: 0x040036F5 RID: 14069
		[SerializeField]
		private Transform _target;

		// Token: 0x040036F6 RID: 14070
		[SerializeField]
		private Transform _centerAxisPosition;

		// Token: 0x040036F7 RID: 14071
		[SerializeField]
		private bool _platformTarget;
	}
}
