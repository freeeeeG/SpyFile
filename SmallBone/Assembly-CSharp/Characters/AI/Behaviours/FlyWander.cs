using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001333 RID: 4915
	public class FlyWander : Behaviour
	{
		// Token: 0x060060F9 RID: 24825 RVA: 0x0011C2EB File Offset: 0x0011A4EB
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
				if (character.movement.controller.isGrounded)
				{
					this._move.direction = (MMMaths.RandomBool() ? Vector2.left : Vector2.right);
					yield return this._move.CRun(controller);
				}
			}
			yield return this._idleWhenEndWander.CRun(controller);
			yield break;
		}

		// Token: 0x04004E36 RID: 20022
		[Move.SubcomponentAttribute(true)]
		[SerializeField]
		protected Move _move;

		// Token: 0x04004E37 RID: 20023
		[UnityEditor.Subcomponent(typeof(Idle))]
		[SerializeField]
		protected Idle _idleWhenEndWander;

		// Token: 0x04004E38 RID: 20024
		[SerializeField]
		protected Collider2D _sightRange;

		// Token: 0x02001334 RID: 4916
		[AttributeUsage(AttributeTargets.Field)]
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060060FB RID: 24827 RVA: 0x0011C301 File Offset: 0x0011A501
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, FlyWander.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04004E39 RID: 20025
			public new static readonly Type[] types = new Type[]
			{
				typeof(FlyWander)
			};
		}
	}
}
