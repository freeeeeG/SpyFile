using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007CD RID: 1997
	public class ReadyAndFire : Movement
	{
		// Token: 0x0600287E RID: 10366 RVA: 0x0007B2C0 File Offset: 0x000794C0
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._initialLookingDirection = projectile.owner.lookingDirection;
			projectile.transform.SetParent(projectile.owner.attachWithFlip.transform);
			base.StartCoroutine(this.CReadyAndFire());
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x0007B30E File Offset: 0x0007950E
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (time < this._readyTime)
			{
				return new ValueTuple<Vector2, float>(base.directionVector, 0f);
			}
			return new ValueTuple<Vector2, float>(base.directionVector, this._speed * base.projectile.speedMultiplier);
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x0007B347 File Offset: 0x00079547
		private IEnumerator CReadyAndFire()
		{
			yield return base.projectile.owner.chronometer.master.WaitForSeconds(this._readyTime);
			Vector2 vector = new Vector2(-base.directionVector.x, base.directionVector.y);
			base.directionVector = ((base.projectile.owner.lookingDirection == this._initialLookingDirection) ? base.directionVector : vector);
			base.projectile.transform.SetParent(null);
			yield break;
		}

		// Token: 0x040022F5 RID: 8949
		[SerializeField]
		private float _readyTime;

		// Token: 0x040022F6 RID: 8950
		[SerializeField]
		private float _speed;

		// Token: 0x040022F7 RID: 8951
		private Character.LookingDirection _initialLookingDirection;
	}
}
