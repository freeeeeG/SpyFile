using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007D1 RID: 2001
	public class Spiral : Movement
	{
		// Token: 0x0600288D RID: 10381 RVA: 0x0007B489 File Offset: 0x00079689
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._rotation = Quaternion.Euler(0f, 0f, direction);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x0007B4AC File Offset: 0x000796AC
		[return: TupleElementNames(new string[]
		{
			"direction",
			"speed"
		})]
		public override ValueTuple<Vector2, float> GetSpeed(float time, float deltaTime)
		{
			if (time >= this._delay)
			{
				float num = time;
				for (int i = 0; i < this._rotationInfos.values.Length; i++)
				{
					Spiral.RotationInfo rotationInfo = this._rotationInfos.values[i];
					if (num > rotationInfo.length)
					{
						num -= rotationInfo.length;
					}
					else
					{
						this.UpdateDirection(deltaTime, rotationInfo);
					}
				}
			}
			float num2 = this._startSpeed;
			for (int j = 0; j < this._moveInfos.values.Length; j++)
			{
				Spiral.MoveInfo moveInfo = this._moveInfos.values[j];
				if (time <= moveInfo.length)
				{
					if (moveInfo.clearHitHistory && this._currentMoveIndex != j)
					{
						base.projectile.ClearHitHistroy();
					}
					this._currentMoveIndex = j;
					float num3 = num2 + (moveInfo.targetSpeed - num2) * moveInfo.curve.Evaluate(time / moveInfo.length);
					return new ValueTuple<Vector2, float>(base.directionVector, num3 * base.projectile.speedMultiplier);
				}
				num2 = moveInfo.targetSpeed;
				time -= moveInfo.length;
			}
			return new ValueTuple<Vector2, float>(base.directionVector, num2 * base.projectile.speedMultiplier);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x0007B5DC File Offset: 0x000797DC
		private void UpdateDirection(float deltaTime, Spiral.RotationInfo info)
		{
			float angle = base.direction + info.angle;
			switch (info.rotateMethod)
			{
			case Spiral.RotateMethod.Constant:
				this._rotation = Quaternion.RotateTowards(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), info.rotateSpeed * 100f * deltaTime);
				break;
			case Spiral.RotateMethod.Lerp:
				this._rotation = Quaternion.Lerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), info.rotateSpeed * deltaTime);
				break;
			case Spiral.RotateMethod.Slerp:
				this._rotation = Quaternion.Slerp(this._rotation, Quaternion.AngleAxis(angle, Vector3.forward), info.rotateSpeed * deltaTime);
				break;
			}
			base.direction = this._rotation.eulerAngles.z;
			base.directionVector = this._rotation * Vector3.right;
		}

		// Token: 0x040022FF RID: 8959
		[SerializeField]
		private float _delay;

		// Token: 0x04002300 RID: 8960
		[SerializeField]
		private float _startSpeed;

		// Token: 0x04002301 RID: 8961
		[SerializeField]
		private Spiral.MoveInfo.Reorderable _moveInfos;

		// Token: 0x04002302 RID: 8962
		[SerializeField]
		private Spiral.RotationInfo.Reorderable _rotationInfos;

		// Token: 0x04002303 RID: 8963
		private int _currentMoveIndex;

		// Token: 0x04002304 RID: 8964
		private Quaternion _rotation;

		// Token: 0x020007D2 RID: 2002
		private enum RotateMethod
		{
			// Token: 0x04002306 RID: 8966
			Constant,
			// Token: 0x04002307 RID: 8967
			Lerp,
			// Token: 0x04002308 RID: 8968
			Slerp
		}

		// Token: 0x020007D3 RID: 2003
		[Serializable]
		private class MoveInfo
		{
			// Token: 0x17000848 RID: 2120
			// (get) Token: 0x06002891 RID: 10385 RVA: 0x0007B6B9 File Offset: 0x000798B9
			public AnimationCurve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x17000849 RID: 2121
			// (get) Token: 0x06002892 RID: 10386 RVA: 0x0007B6C1 File Offset: 0x000798C1
			public float length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x1700084A RID: 2122
			// (get) Token: 0x06002893 RID: 10387 RVA: 0x0007B6C9 File Offset: 0x000798C9
			public float targetSpeed
			{
				get
				{
					return this._targetSpeed;
				}
			}

			// Token: 0x1700084B RID: 2123
			// (get) Token: 0x06002894 RID: 10388 RVA: 0x0007B6D1 File Offset: 0x000798D1
			public bool clearHitHistory
			{
				get
				{
					return this._clearHitHistory;
				}
			}

			// Token: 0x04002309 RID: 8969
			[SerializeField]
			private AnimationCurve _curve;

			// Token: 0x0400230A RID: 8970
			[SerializeField]
			private float _length;

			// Token: 0x0400230B RID: 8971
			[SerializeField]
			private float _targetSpeed;

			// Token: 0x0400230C RID: 8972
			[SerializeField]
			private bool _clearHitHistory;

			// Token: 0x020007D4 RID: 2004
			[Serializable]
			internal class Reorderable : ReorderableArray<Spiral.MoveInfo>
			{
			}
		}

		// Token: 0x020007D5 RID: 2005
		[Serializable]
		private class RotationInfo
		{
			// Token: 0x1700084C RID: 2124
			// (get) Token: 0x06002897 RID: 10391 RVA: 0x0007B6E1 File Offset: 0x000798E1
			public float length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x1700084D RID: 2125
			// (get) Token: 0x06002898 RID: 10392 RVA: 0x0007B6E9 File Offset: 0x000798E9
			public float rotateSpeed
			{
				get
				{
					return this._rotateSpeed;
				}
			}

			// Token: 0x1700084E RID: 2126
			// (get) Token: 0x06002899 RID: 10393 RVA: 0x0007B6F1 File Offset: 0x000798F1
			public float angle
			{
				get
				{
					return this._angle;
				}
			}

			// Token: 0x1700084F RID: 2127
			// (get) Token: 0x0600289A RID: 10394 RVA: 0x0007B6F9 File Offset: 0x000798F9
			public Spiral.RotateMethod rotateMethod
			{
				get
				{
					return this._rotateMethod;
				}
			}

			// Token: 0x0400230D RID: 8973
			[SerializeField]
			private float _length;

			// Token: 0x0400230E RID: 8974
			[SerializeField]
			private float _rotateSpeed;

			// Token: 0x0400230F RID: 8975
			[SerializeField]
			private float _angle;

			// Token: 0x04002310 RID: 8976
			[SerializeField]
			private Spiral.RotateMethod _rotateMethod;

			// Token: 0x020007D6 RID: 2006
			[Serializable]
			internal class Reorderable : ReorderableArray<Spiral.RotationInfo>
			{
			}
		}
	}
}
