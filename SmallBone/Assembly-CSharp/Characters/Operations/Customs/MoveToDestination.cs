using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FC8 RID: 4040
	public class MoveToDestination : CharacterOperation
	{
		// Token: 0x06004E36 RID: 20022 RVA: 0x000EA0F4 File Offset: 0x000E82F4
		public override void Run(Character owner)
		{
			if (owner.movement == null)
			{
				return;
			}
			Vector2 vector = this._destination.position - owner.transform.position;
			if (this._curve.duration > 0f)
			{
				this._coroutineReference.Stop();
				this._coroutineReference = owner.StartCoroutineWithReference(this.CMove(owner, this._curve, vector));
				return;
			}
			owner.movement.force += vector;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x000EA180 File Offset: 0x000E8380
		private IEnumerator CMove(Character character, Curve curve, Vector2 distance)
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

		// Token: 0x06004E38 RID: 20024 RVA: 0x000EA19D File Offset: 0x000E839D
		public override void Stop()
		{
			this._coroutineReference.Stop();
		}

		// Token: 0x04003E3D RID: 15933
		[SerializeField]
		private Transform _destination;

		// Token: 0x04003E3E RID: 15934
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003E3F RID: 15935
		private CoroutineReference _coroutineReference;
	}
}
