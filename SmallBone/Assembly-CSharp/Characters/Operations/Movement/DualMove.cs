using System;
using System.Runtime.CompilerServices;
using Characters.Controllers;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E53 RID: 3667
	public class DualMove : CharacterOperation
	{
		// Token: 0x060048DE RID: 18654 RVA: 0x000D4640 File Offset: 0x000D2840
		public override void Run(Character owner)
		{
			DualMove.<>c__DisplayClass11_0 CS$<>8__locals1;
			CS$<>8__locals1.owner = owner;
			CS$<>8__locals1.<>4__this = this;
			if (CS$<>8__locals1.owner.movement == null)
			{
				return;
			}
			if (this._needDirectionInput)
			{
				PlayerInput component = CS$<>8__locals1.owner.GetComponent<PlayerInput>();
				if (component != null)
				{
					if (CS$<>8__locals1.owner.lookingDirection == Character.LookingDirection.Left && component.direction.x >= -0.66f)
					{
						return;
					}
					if (CS$<>8__locals1.owner.lookingDirection == Character.LookingDirection.Right && component.direction.x <= 0.66f)
					{
						return;
					}
				}
			}
			float extraPower = 0f;
			float extraPower2 = 0f;
			if (this._movementSpeedFactor1 > 0f || this._movementSpeedFactor2 > 0f)
			{
				float num = Mathf.Abs((float)CS$<>8__locals1.owner.stat.GetFinal(Stat.Kind.MovementSpeed));
				float num2 = Mathf.Abs((float)CS$<>8__locals1.owner.stat.Get(Stat.Category.Constant, Stat.Kind.MovementSpeed));
				float num3 = Mathf.Max(0f, num - num2);
				extraPower = num3 * this._curve1.duration * this._movementSpeedFactor1;
				extraPower2 = num3 * this._curve2.duration * this._movementSpeedFactor2;
			}
			this.<Run>g__TriggerMove|11_0(this._force1, this._curve1, extraPower, ref this._coroutineReference1, ref CS$<>8__locals1);
			this.<Run>g__TriggerMove|11_0(this._force2, this._curve2, extraPower2, ref this._coroutineReference2, ref CS$<>8__locals1);
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x000D47A2 File Offset: 0x000D29A2
		public override void Stop()
		{
			this._coroutineReference1.Stop();
			this._coroutineReference2.Stop();
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x000D47CC File Offset: 0x000D29CC
		[CompilerGenerated]
		private void <Run>g__TriggerMove|11_0(Force force, Curve curve, float extraPower, ref CoroutineReference coroutineReference, ref DualMove.<>c__DisplayClass11_0 A_5)
		{
			Vector2 vector = force.Evaluate(A_5.owner, 0f);
			if (this._useDashDistanceStat)
			{
				vector *= (float)A_5.owner.stat.GetFinal(Stat.Kind.DashDistance);
			}
			if (this._curve1.duration > 0f)
			{
				coroutineReference.Stop();
				coroutineReference = A_5.owner.StartCoroutineWithReference(Move.CMove(A_5.owner, curve, vector));
				return;
			}
			A_5.owner.movement.force += vector;
		}

		// Token: 0x040037E8 RID: 14312
		private const float directionThreshold = 0.66f;

		// Token: 0x040037E9 RID: 14313
		[SerializeField]
		private bool _useDashDistanceStat;

		// Token: 0x040037EA RID: 14314
		[SerializeField]
		private float _movementSpeedFactor1;

		// Token: 0x040037EB RID: 14315
		[SerializeField]
		private Force _force1;

		// Token: 0x040037EC RID: 14316
		[SerializeField]
		private Curve _curve1;

		// Token: 0x040037ED RID: 14317
		[SerializeField]
		private float _movementSpeedFactor2;

		// Token: 0x040037EE RID: 14318
		[SerializeField]
		private Force _force2;

		// Token: 0x040037EF RID: 14319
		[SerializeField]
		private Curve _curve2;

		// Token: 0x040037F0 RID: 14320
		[SerializeField]
		private bool _needDirectionInput = true;

		// Token: 0x040037F1 RID: 14321
		private CoroutineReference _coroutineReference1;

		// Token: 0x040037F2 RID: 14322
		private CoroutineReference _coroutineReference2;
	}
}
