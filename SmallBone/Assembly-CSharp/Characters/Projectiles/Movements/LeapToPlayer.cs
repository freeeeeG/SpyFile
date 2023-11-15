using System;
using System.Runtime.CompilerServices;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007BD RID: 1981
	public class LeapToPlayer : Movement
	{
		// Token: 0x0600284E RID: 10318 RVA: 0x0007A198 File Offset: 0x00078398
		public override void Initialize(IProjectile projectile, float direction)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			Vector2 vector = new Vector2(player.transform.position.x, projectile.transform.position.y);
			Vector2 vector2 = projectile.transform.position;
			this._directionVector = vector - vector2;
			this._distance = Mathf.Abs(vector2.x - vector.x);
			base.Initialize(projectile, direction);
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x0007A21C File Offset: 0x0007841C
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float item = (time > this._duration) ? 0f : this._distance;
			return new ValueTuple<Vector2, float>(this._directionVector.normalized, item);
		}

		// Token: 0x040022A4 RID: 8868
		[SerializeField]
		private float _duration = 1f;

		// Token: 0x040022A5 RID: 8869
		private Vector2 _directionVector;

		// Token: 0x040022A6 RID: 8870
		private float _distance;
	}
}
