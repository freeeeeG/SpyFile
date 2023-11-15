using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200131C RID: 4892
	public class MoveToDestinationWithFly : Move
	{
		// Token: 0x060060A2 RID: 24738 RVA: 0x0011B1B6 File Offset: 0x001193B6
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				while (character.stunedOrFreezed)
				{
					yield return null;
				}
				if (Mathf.Abs((controller.destination - character.transform.position).sqrMagnitude) < this._minimumDistance)
				{
					base.result = Behaviour.Result.Success;
					yield return this.idle.CRun(controller);
					break;
				}
				character.movement.MoveTo(controller.destination);
			}
			yield break;
		}

		// Token: 0x04004DDD RID: 19933
		[SerializeField]
		private float _minimumDistance = 1f;
	}
}
