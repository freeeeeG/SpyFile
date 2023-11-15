using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007C0 RID: 1984
	public class Ease : Movement
	{
		// Token: 0x06002857 RID: 10327 RVA: 0x0007A3BB File Offset: 0x000785BB
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._easingFunction = EasingFunction.GetEasingFunction(this._easingMethod);
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x0007A3D8 File Offset: 0x000785D8
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float num = this._easingFunction(this._startSpeed, this._targetSpeed, time / this._easingTime);
			return new ValueTuple<Vector2, float>(base.directionVector, num * base.projectile.speedMultiplier);
		}

		// Token: 0x040022AC RID: 8876
		[SerializeField]
		private float _startSpeed;

		// Token: 0x040022AD RID: 8877
		[SerializeField]
		private float _targetSpeed;

		// Token: 0x040022AE RID: 8878
		[SerializeField]
		private float _easingTime;

		// Token: 0x040022AF RID: 8879
		[SerializeField]
		private EasingFunction.Method _easingMethod;

		// Token: 0x040022B0 RID: 8880
		private EasingFunction.Function _easingFunction;
	}
}
