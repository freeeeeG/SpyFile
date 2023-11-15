using System;
using System.Runtime.CompilerServices;
using Characters.Projectiles.Customs;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007DC RID: 2012
	public class WreckMovement : Movement
	{
		// Token: 0x060028B2 RID: 10418 RVA: 0x0007BDBD File Offset: 0x00079FBD
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._currentIndex = 0;
			this._terrainCollisionDetector.Run();
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0007BDDC File Offset: 0x00079FDC
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			float num = this._startSpeed;
			for (int i = 0; i < this._infos.values.Length; i++)
			{
				WreckMovement.Info info = this._infos.values[i];
				if (time <= info.length)
				{
					if (info.clearHitHistory && this._currentIndex != i)
					{
						base.projectile.ClearHitHistroy();
					}
					this._currentIndex = i;
					float num2 = num + (info.targetSpeed - num) * info.curve.Evaluate(time / info.length);
					return new ValueTuple<Vector2, float>(base.directionVector, num2 * base.projectile.speedMultiplier);
				}
				num = info.targetSpeed;
				time -= info.length;
			}
			return new ValueTuple<Vector2, float>(base.directionVector, num * base.projectile.speedMultiplier);
		}

		// Token: 0x0400232D RID: 9005
		[SerializeField]
		private TerrainCollisionDetector _terrainCollisionDetector;

		// Token: 0x0400232E RID: 9006
		[SerializeField]
		private float _startSpeed;

		// Token: 0x0400232F RID: 9007
		[SerializeField]
		private WreckMovement.Info.Reorderable _infos;

		// Token: 0x04002330 RID: 9008
		private int _currentIndex;

		// Token: 0x020007DD RID: 2013
		[Serializable]
		private class Info
		{
			// Token: 0x17000851 RID: 2129
			// (get) Token: 0x060028B5 RID: 10421 RVA: 0x0007BEAB File Offset: 0x0007A0AB
			public AnimationCurve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x17000852 RID: 2130
			// (get) Token: 0x060028B6 RID: 10422 RVA: 0x0007BEB3 File Offset: 0x0007A0B3
			public float length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17000853 RID: 2131
			// (get) Token: 0x060028B7 RID: 10423 RVA: 0x0007BEBB File Offset: 0x0007A0BB
			public float targetSpeed
			{
				get
				{
					return this._targetSpeed;
				}
			}

			// Token: 0x17000854 RID: 2132
			// (get) Token: 0x060028B8 RID: 10424 RVA: 0x0007BEC3 File Offset: 0x0007A0C3
			public bool clearHitHistory
			{
				get
				{
					return this._clearHitHistory;
				}
			}

			// Token: 0x04002331 RID: 9009
			[SerializeField]
			private AnimationCurve _curve;

			// Token: 0x04002332 RID: 9010
			[SerializeField]
			private float _length;

			// Token: 0x04002333 RID: 9011
			[SerializeField]
			private float _targetSpeed;

			// Token: 0x04002334 RID: 9012
			[SerializeField]
			private bool _clearHitHistory;

			// Token: 0x020007DE RID: 2014
			[Serializable]
			internal class Reorderable : ReorderableArray<WreckMovement.Info>
			{
			}
		}
	}
}
