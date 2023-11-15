using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007CF RID: 1999
	public class SequenceMovment : Movement
	{
		// Token: 0x06002888 RID: 10376 RVA: 0x0007B428 File Offset: 0x00079628
		public override void Initialize(IProjectile projectile, float direction)
		{
			this._firstMovement.Initialize(projectile, direction);
			this._secondMovement.Initialize(projectile, direction);
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x0007B444 File Offset: 0x00079644
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (time < this._firstMovePlayTime)
			{
				return this._firstMovement.GetSpeed(time, deltaTime);
			}
			return this._secondMovement.GetSpeed(time, deltaTime);
		}

		// Token: 0x040022FB RID: 8955
		[SerializeField]
		[Movement.SubcomponentAttribute]
		private Movement _firstMovement;

		// Token: 0x040022FC RID: 8956
		[SerializeField]
		private float _firstMovePlayTime;

		// Token: 0x040022FD RID: 8957
		[SerializeField]
		[Movement.SubcomponentAttribute]
		private Movement _secondMovement;
	}
}
