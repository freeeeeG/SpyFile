using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007C1 RID: 1985
	public class Ease2 : Movement
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x0007A41D File Offset: 0x0007861D
		public override void Initialize(IProjectile projectile, float direction)
		{
			base.Initialize(projectile, direction);
			this._currentIndex = 0;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x0007A430 File Offset: 0x00078630
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
				Ease2.Info info = this._infos.values[i];
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

		// Token: 0x040022B1 RID: 8881
		[SerializeField]
		private float _startSpeed;

		// Token: 0x040022B2 RID: 8882
		[SerializeField]
		private Ease2.Info.Reorderable _infos;

		// Token: 0x040022B3 RID: 8883
		private int _currentIndex;

		// Token: 0x020007C2 RID: 1986
		[Serializable]
		private class Info
		{
			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x0600285D RID: 10333 RVA: 0x0007A4FF File Offset: 0x000786FF
			public AnimationCurve curve
			{
				get
				{
					return this._curve;
				}
			}

			// Token: 0x17000840 RID: 2112
			// (get) Token: 0x0600285E RID: 10334 RVA: 0x0007A507 File Offset: 0x00078707
			public float length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17000841 RID: 2113
			// (get) Token: 0x0600285F RID: 10335 RVA: 0x0007A50F File Offset: 0x0007870F
			public float targetSpeed
			{
				get
				{
					return this._targetSpeed;
				}
			}

			// Token: 0x17000842 RID: 2114
			// (get) Token: 0x06002860 RID: 10336 RVA: 0x0007A517 File Offset: 0x00078717
			public bool clearHitHistory
			{
				get
				{
					return this._clearHitHistory;
				}
			}

			// Token: 0x040022B4 RID: 8884
			[SerializeField]
			private AnimationCurve _curve;

			// Token: 0x040022B5 RID: 8885
			[SerializeField]
			private float _length;

			// Token: 0x040022B6 RID: 8886
			[SerializeField]
			private float _targetSpeed;

			// Token: 0x040022B7 RID: 8887
			[SerializeField]
			private bool _clearHitHistory;

			// Token: 0x020007C3 RID: 1987
			[Serializable]
			internal class Reorderable : ReorderableArray<Ease2.Info>
			{
			}
		}
	}
}
