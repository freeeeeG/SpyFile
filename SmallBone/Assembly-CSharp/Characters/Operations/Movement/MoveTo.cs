using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E62 RID: 3682
	public sealed class MoveTo : CharacterOperation
	{
		// Token: 0x06004915 RID: 18709 RVA: 0x000D52C4 File Offset: 0x000D34C4
		public override void Run(Character owner)
		{
			if (owner.movement == null)
			{
				return;
			}
			Vector2 vector = this._target.transform.position - owner.transform.position;
			float magnitude = vector.magnitude;
			Vector2 vector2 = vector.normalized * magnitude;
			float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			if (vector.x < 0f)
			{
				num -= 180f;
			}
			if (this._rotateTransform != null)
			{
				this._rotateTransform.rotation = Quaternion.Euler(0f, 0f, num);
			}
			if (this._curve.duration > 0f)
			{
				this._coroutineReference.Stop();
				this._coroutineReference = owner.movement.StartCoroutineWithReference(this.CMove(owner, this._curve, vector2));
				return;
			}
			owner.movement.force += vector2;
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x000D53C6 File Offset: 0x000D35C6
		internal IEnumerator CMove(Character character, Curve curve, Vector2 distance)
		{
			float t = 0f;
			float amountBefore = 0f;
			character.transform.position;
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
			if (this._rotateTransform != null)
			{
				this._rotateTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (this._setPostionOnEnd)
			{
				character.movement.force = Vector3.zero;
				character.transform.position = this._target.transform.position;
			}
			yield break;
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x000D53EA File Offset: 0x000D35EA
		public override void Stop()
		{
			if (this._rotateTransform != null)
			{
				this._rotateTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
			this._coroutineReference.Stop();
		}

		// Token: 0x04003837 RID: 14391
		[SerializeField]
		private Transform _target;

		// Token: 0x04003838 RID: 14392
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003839 RID: 14393
		[SerializeField]
		private bool _setPostionOnEnd;

		// Token: 0x0400383A RID: 14394
		[SerializeField]
		private Transform _rotateTransform;

		// Token: 0x0400383B RID: 14395
		private CoroutineReference _coroutineReference;
	}
}
