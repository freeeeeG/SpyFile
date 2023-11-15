using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001317 RID: 4887
	public class MoveToBehindWithFly : Behaviour
	{
		// Token: 0x0600608B RID: 24715 RVA: 0x0011ACD8 File Offset: 0x00118ED8
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			float num = target.transform.position.x - character.transform.position.x;
			Vector2 midPoint = new Vector2(target.transform.position.x, target.transform.position.y + this._midPointHeight);
			float behindPosition = (num > 0f) ? (target.transform.position.x - this._distanceX) : (target.transform.position.x + this._distanceX);
			base.result = Behaviour.Result.Doing;
			while (base.result == Behaviour.Result.Doing)
			{
				yield return null;
				controller.destination = midPoint;
				yield return this._moveToDestinationWithFly.CRun(controller);
				controller.destination = new Vector2(behindPosition, target.transform.position.y);
				yield return this._moveToDestinationWithFly.CRun(controller);
			}
			yield break;
		}

		// Token: 0x04004DC7 RID: 19911
		[UnityEditor.Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		private MoveToDestinationWithFly _moveToDestinationWithFly;

		// Token: 0x04004DC8 RID: 19912
		[SerializeField]
		private float _distanceX;

		// Token: 0x04004DC9 RID: 19913
		[SerializeField]
		private float _midPointHeight;
	}
}
