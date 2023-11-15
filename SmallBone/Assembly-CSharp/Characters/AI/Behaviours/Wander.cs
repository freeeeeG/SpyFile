using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001330 RID: 4912
	public class Wander : Behaviour
	{
		// Token: 0x060060EF RID: 24815 RVA: 0x0011C154 File Offset: 0x0011A354
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				if (controller.target != null)
				{
					base.result = Behaviour.Result.Success;
					break;
				}
				if (Precondition.CanMove(character) && character.movement.controller.isGrounded)
				{
					this._move.direction = (MMMaths.RandomBool() ? Vector2.left : Vector2.right);
					yield return this._move.CRun(controller);
				}
			}
			yield return this._idleWhenEndWander.CRun(controller);
			yield break;
		}

		// Token: 0x04004E2D RID: 20013
		[SerializeField]
		[Move.SubcomponentAttribute(true)]
		protected Move _move;

		// Token: 0x04004E2E RID: 20014
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Idle))]
		protected Idle _idleWhenEndWander;

		// Token: 0x04004E2F RID: 20015
		[SerializeField]
		protected Collider2D _sightRange;

		// Token: 0x02001331 RID: 4913
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060060F1 RID: 24817 RVA: 0x0011C16A File Offset: 0x0011A36A
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, Wander.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004E30 RID: 20016
			public new static readonly Type[] types = new Type[]
			{
				typeof(Wander),
				typeof(RangeWander),
				typeof(WanderForDuration)
			};
		}
	}
}
