using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BF RID: 1983
	public class SimpleRandomSpeed : Movement
	{
		// Token: 0x06002854 RID: 10324 RVA: 0x0007A379 File Offset: 0x00078579
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._speed = this._speedRange.value;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x0007A394 File Offset: 0x00078594
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			return new ValueTuple<Vector2, float>(base.directionVector, this._speed * base.projectile.speedMultiplier);
		}

		// Token: 0x040022AA RID: 8874
		[SerializeField]
		private CustomFloat _speedRange;

		// Token: 0x040022AB RID: 8875
		private float _speed;
	}
}
