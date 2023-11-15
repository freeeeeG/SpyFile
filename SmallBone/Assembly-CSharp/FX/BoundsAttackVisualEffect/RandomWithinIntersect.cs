using System;
using Characters;
using UnityEngine;

namespace FX.BoundsAttackVisualEffect
{
	// Token: 0x02000291 RID: 657
	public class RandomWithinIntersect : BoundsAttackVisualEffect
	{
		// Token: 0x06000CC0 RID: 3264 RVA: 0x000297C2 File Offset: 0x000279C2
		private void Awake()
		{
			if (this._critical.effect == null)
			{
				this._critical = this._normal;
			}
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000297E4 File Offset: 0x000279E4
		public override void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target)
		{
			EffectInfo effectInfo = damage.critical ? this._critical : this._normal;
			Bounds bounds2 = target.collider.bounds;
			Vector3 position;
			if (bounds.Intersects(bounds2))
			{
				position = MMMaths.RandomVector3(MMMaths.Max(bounds.min, bounds2.min), MMMaths.Min(bounds.max, bounds2.max));
			}
			else
			{
				position = MMMaths.RandomPointWithinBounds(bounds2);
			}
			effectInfo.Spawn(position, owner, 0f, 1f);
		}

		// Token: 0x04000AE8 RID: 2792
		[SerializeField]
		private EffectInfo _normal;

		// Token: 0x04000AE9 RID: 2793
		[SerializeField]
		private EffectInfo _critical;
	}
}
