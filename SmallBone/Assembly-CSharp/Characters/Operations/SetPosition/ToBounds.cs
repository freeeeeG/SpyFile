using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE0 RID: 3808
	public class ToBounds : Policy
	{
		// Token: 0x06004AB8 RID: 19128 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x000DA7CC File Offset: 0x000D89CC
		public override Vector2 GetPosition()
		{
			if (!this._onPlatform)
			{
				return MMMaths.RandomPointWithinBounds(this._collider.bounds);
			}
			return this.GetProjectionPointToPlatform(this._groundMask, 100f);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x000DA7F8 File Offset: 0x000D89F8
		private Vector2 GetProjectionPointToPlatform(LayerMask layerMask, float distance = 100f)
		{
			Vector2 vector = MMMaths.RandomPointWithinBounds(this._collider.bounds);
			ToBounds._belowCaster.contactFilter.SetLayerMask(layerMask);
			ToBounds._belowCaster.RayCast(vector, Vector2.down, distance);
			ReadonlyBoundedList<RaycastHit2D> results = ToBounds._belowCaster.results;
			if (results.Count < 0)
			{
				return vector;
			}
			int index = 0;
			float num = results[0].distance;
			for (int i = 1; i < results.Count; i++)
			{
				float distance2 = results[i].distance;
				if (distance2 < num)
				{
					num = distance2;
					index = i;
				}
			}
			return results[index].point;
		}

		// Token: 0x040039D8 RID: 14808
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;

		// Token: 0x040039D9 RID: 14809
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x040039DA RID: 14810
		[SerializeField]
		private bool _onPlatform;

		// Token: 0x040039DB RID: 14811
		private static NonAllocCaster _belowCaster = new NonAllocCaster(1);
	}
}
