using System;
using System.Collections;
using Characters.Marks;
using Characters.Operations.Attack;
using UnityEngine;

namespace Characters.Operations.Customs.Samurai
{
	// Token: 0x02001000 RID: 4096
	public class NapdoSweepAttak2 : SweepAttack2
	{
		// Token: 0x06004F2B RID: 20267 RVA: 0x000EE2B8 File Offset: 0x000EC4B8
		protected override IEnumerator CAttack(Vector2 origin, Vector2 direction, float distance, RaycastHit2D raycastHit, Target target)
		{
			int index = 0;
			float time = 0f;
			if (target.character == null)
			{
				yield break;
			}
			float stack = target.character.mark.GetStack(this._mark);
			int length = (int)Mathf.Min(stack * this._attackLengthMultiplier, (float)this._attackAndEffect.components.Length);
			while (this != null && index < length)
			{
				CastAttackInfoSequence castAttackInfoSequence;
				while (index < length && time >= (castAttackInfoSequence = this._attackAndEffect.components[index]).timeToTrigger)
				{
					target.character.mark.TakeStack(this._mark, 1f / this._attackLengthMultiplier);
					base.Attack(castAttackInfoSequence.attackInfo, origin, direction, distance, raycastHit, target);
					int num = index;
					index = num + 1;
				}
				yield return null;
				time += base.owner.chronometer.animation.deltaTime;
			}
			target.character.mark.TakeAllStack(this._mark);
			yield break;
		}

		// Token: 0x04003F4E RID: 16206
		[SerializeField]
		private MarkInfo _mark;

		// Token: 0x04003F4F RID: 16207
		[SerializeField]
		[Tooltip("표식 개수 * _attackLengthMultiplier값까지의 AttackInfo가 적용됨")]
		private float _attackLengthMultiplier;
	}
}
