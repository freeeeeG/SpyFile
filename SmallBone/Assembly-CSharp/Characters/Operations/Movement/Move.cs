using System;
using System.Collections;
using Characters.Controllers;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E60 RID: 3680
	public class Move : CharacterOperation
	{
		// Token: 0x0600490B RID: 18699 RVA: 0x000D5024 File Offset: 0x000D3224
		public override void Run(Character owner)
		{
			if (owner.movement == null)
			{
				return;
			}
			if (this._needDirectionInput)
			{
				PlayerInput component = owner.GetComponent<PlayerInput>();
				if (component != null)
				{
					if (owner.lookingDirection == Character.LookingDirection.Left && component.direction.x >= -0.66f)
					{
						return;
					}
					if (owner.lookingDirection == Character.LookingDirection.Right && component.direction.x <= 0.66f)
					{
						return;
					}
				}
			}
			float extraPower = 0f;
			if (this._movementSpeedFactor > 0f)
			{
				float num = Mathf.Abs((float)owner.stat.Get(Stat.Category.Constant, Stat.Kind.MovementSpeed));
				float num2 = Mathf.Abs((float)owner.stat.GetFinal(Stat.Kind.MovementSpeed));
				extraPower = Mathf.Max(0f, num2 - num) * this._curve.duration * this._movementSpeedFactor;
			}
			Vector2 vector = this._force.Evaluate(owner, extraPower);
			if (this._useDashDistanceStat)
			{
				vector *= (float)owner.stat.GetFinal(Stat.Kind.DashDistance);
			}
			if (this._curve.duration > 0f)
			{
				this._coroutineReference.Stop();
				this._coroutineReference = owner.movement.StartCoroutineWithReference(Move.CMove(owner, this._curve, vector));
				return;
			}
			owner.movement.force += vector;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x000D5179 File Offset: 0x000D3379
		internal static IEnumerator CMove(Character character, Curve curve, Vector2 distance)
		{
			float t = 0f;
			float amountBefore = 0f;
			while (t < curve.duration)
			{
				if (character == null || !character.liveAndActive)
				{
					yield break;
				}
				float num = curve.Evaluate(t);
				character.movement.force += distance * (num - amountBefore);
				amountBefore = num;
				yield return null;
				t += character.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x000D5196 File Offset: 0x000D3396
		public override void Stop()
		{
			this._coroutineReference.Stop();
		}

		// Token: 0x04003829 RID: 14377
		private const float directionThreshold = 0.66f;

		// Token: 0x0400382A RID: 14378
		[SerializeField]
		private bool _useDashDistanceStat;

		// Token: 0x0400382B RID: 14379
		[SerializeField]
		private float _movementSpeedFactor;

		// Token: 0x0400382C RID: 14380
		[SerializeField]
		private Force _force;

		// Token: 0x0400382D RID: 14381
		[SerializeField]
		private Curve _curve;

		// Token: 0x0400382E RID: 14382
		[SerializeField]
		private bool _needDirectionInput = true;

		// Token: 0x0400382F RID: 14383
		private CoroutineReference _coroutineReference;
	}
}
