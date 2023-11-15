using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E5C RID: 3676
	public class ModifyVerticalVelocity : CharacterOperation
	{
		// Token: 0x060048FA RID: 18682 RVA: 0x000D4D60 File Offset: 0x000D2F60
		public override void Run(Character owner)
		{
			this._owner = owner;
			if (this._curve.duration > 0f)
			{
				ModifyVerticalVelocity.Method method = this._method;
				if (method == ModifyVerticalVelocity.Method.Add)
				{
					this._coroutineReference = owner.StartCoroutineWithReference(this.CRun(owner));
					return;
				}
				if (method != ModifyVerticalVelocity.Method.Set)
				{
					return;
				}
				this._coroutineReference = owner.StartCoroutineWithReference(this.CRunWithIgnoreGravity(owner));
				return;
			}
			else
			{
				ModifyVerticalVelocity.Method method = this._method;
				if (method == ModifyVerticalVelocity.Method.Add)
				{
					owner.movement.verticalVelocity += this._amount;
					return;
				}
				if (method != ModifyVerticalVelocity.Method.Set)
				{
					return;
				}
				owner.movement.verticalVelocity = this._amount;
				return;
			}
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x000D4DF6 File Offset: 0x000D2FF6
		private IEnumerator CRunWithIgnoreGravity(Character character)
		{
			character.movement.ignoreGravity.Attach(this);
			yield return this.CRun(character);
			character.movement.ignoreGravity.Detach(this);
			yield break;
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x000D4E0C File Offset: 0x000D300C
		private IEnumerator CRun(Character character)
		{
			float t = 0f;
			float normAmountBefore = 0f;
			while (t < this._curve.duration)
			{
				float num = this._curve.Evaluate(t);
				ModifyVerticalVelocity.Method method = this._method;
				if (method != ModifyVerticalVelocity.Method.Add)
				{
					if (method == ModifyVerticalVelocity.Method.Set)
					{
						character.movement.verticalVelocity = this._amount;
					}
				}
				else
				{
					character.movement.verticalVelocity += this._amount * (num - normAmountBefore);
				}
				normAmountBefore = num;
				yield return null;
				t += character.chronometer.animation.deltaTime;
			}
			yield break;
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x000D4E24 File Offset: 0x000D3024
		public override void Stop()
		{
			if (this._owner == this._coroutineReference.monoBehaviour)
			{
				this._coroutineReference.Stop();
			}
			if (this._method == ModifyVerticalVelocity.Method.Set)
			{
				Character owner = this._owner;
				if (owner == null)
				{
					return;
				}
				owner.movement.ignoreGravity.Detach(this);
			}
		}

		// Token: 0x04003817 RID: 14359
		[SerializeField]
		private float _amount;

		// Token: 0x04003818 RID: 14360
		[SerializeField]
		private ModifyVerticalVelocity.Method _method;

		// Token: 0x04003819 RID: 14361
		[Information("For Add method only", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Curve _curve;

		// Token: 0x0400381A RID: 14362
		private Character _owner;

		// Token: 0x0400381B RID: 14363
		private CoroutineReference _coroutineReference;

		// Token: 0x02000E5D RID: 3677
		private enum Method
		{
			// Token: 0x0400381D RID: 14365
			Add,
			// Token: 0x0400381E RID: 14366
			Set
		}
	}
}
