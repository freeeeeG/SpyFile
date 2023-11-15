using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BA RID: 1978
	[Serializable]
	public class BouncyProjectileMovement
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x00079CC0 File Offset: 0x00077EC0
		// (set) Token: 0x06002842 RID: 10306 RVA: 0x00079CC8 File Offset: 0x00077EC8
		public IProjectile projectile { get; private set; }

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x00079CD1 File Offset: 0x00077ED1
		// (set) Token: 0x06002844 RID: 10308 RVA: 0x00079CD9 File Offset: 0x00077ED9
		public Vector2 directionVector { get; set; }

		// Token: 0x06002845 RID: 10309 RVA: 0x00079CE4 File Offset: 0x00077EE4
		public void Initialize(IProjectile projectile, float direction)
		{
			this.projectile = projectile;
			float f = direction * 0.017453292f;
			this.directionVector = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
			this.ySpeed = 0f;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x00079D24 File Offset: 0x00077F24
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public ValueTuple<Vector2, float> GetSpeed(float deltaTime)
		{
			float num = this._speed * this.projectile.speedMultiplier;
			this._velocity = num * this.directionVector;
			this.ySpeed -= this._gravity * deltaTime;
			this._velocity.y = this._velocity.y + this.ySpeed;
			if (this._velocity.y < this._maxFallSpeed)
			{
				this._velocity.y = this._maxFallSpeed;
			}
			if (this._fixedXSpeed)
			{
				this._velocity.x = ((this.directionVector.x > 0f) ? num : (-num));
			}
			return new ValueTuple<Vector2, float>(this._velocity.normalized, this._velocity.magnitude);
		}

		// Token: 0x04002290 RID: 8848
		[SerializeField]
		private float _speed;

		// Token: 0x04002291 RID: 8849
		[SerializeField]
		private float _gravity;

		// Token: 0x04002292 RID: 8850
		[SerializeField]
		private float _maxFallSpeed;

		// Token: 0x04002293 RID: 8851
		[SerializeField]
		[Tooltip("X축 스피드가 speed 값으로 고정")]
		private bool _fixedXSpeed;

		// Token: 0x04002294 RID: 8852
		public float ySpeed;

		// Token: 0x04002295 RID: 8853
		private Vector2 _velocity;
	}
}
