using System;
using System.Collections;
using Characters.Actions;
using Characters.Movements;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013DC RID: 5084
	public class ChaseAttack : Attack
	{
		// Token: 0x06006426 RID: 25638 RVA: 0x00122948 File Offset: 0x00120B48
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			base.result = Behaviour.Result.Doing;
			base.StartCoroutine(base.CExpire(controller, this._duration));
			this._action.TryStart();
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				float num = character.transform.position.x - target.transform.position.x;
				if (Mathf.Abs(num) > 1f)
				{
					character.movement.move = ((num > 0f) ? Vector2.left : Vector2.right);
				}
			}
			character.movement.config.type = Movement.Config.Type.Walking;
			yield break;
		}

		// Token: 0x040050C2 RID: 20674
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x040050C3 RID: 20675
		[SerializeField]
		private float _duration;
	}
}
