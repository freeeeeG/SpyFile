using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007D0 RID: 2000
	public class Simple : Movement
	{
		// Token: 0x0600288B RID: 10379 RVA: 0x0007B46A File Offset: 0x0007966A
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			return new ValueTuple<Vector2, float>(base.directionVector, this._speed * base.projectile.speedMultiplier);
		}

		// Token: 0x040022FE RID: 8958
		[SerializeField]
		private float _speed;
	}
}
