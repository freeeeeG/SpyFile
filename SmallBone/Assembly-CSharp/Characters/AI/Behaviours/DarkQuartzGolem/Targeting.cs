using System;
using System.Collections;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem
{
	// Token: 0x02001391 RID: 5009
	public class Targeting : Behaviour, IPattern
	{
		// Token: 0x060062DA RID: 25306 RVA: 0x00099F2B File Offset: 0x0009812B
		public bool CanUse()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x0011FBE8 File Offset: 0x0011DDE8
		public bool CanUse(AIController controller)
		{
			Character target = controller.target;
			Character character = controller.character;
			UnityEngine.Object lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
			Collider2D lastStandingCollider2 = character.movement.controller.collisionState.lastStandingCollider;
			return lastStandingCollider != lastStandingCollider2;
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x0011FC37 File Offset: 0x0011DE37
		public override IEnumerator CRun(AIController controller)
		{
			while (controller.target == null || controller.target.movement == null || !controller.target.movement.isGrounded)
			{
				yield return null;
			}
			yield return this._attack.CRun(controller);
			yield break;
		}

		// Token: 0x04004FB0 RID: 20400
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;
	}
}
