using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200132E RID: 4910
	public class TeleportInRangeWithFly : Behaviour
	{
		// Token: 0x060060E7 RID: 24807 RVA: 0x0011BF5A File Offset: 0x0011A15A
		public override IEnumerator CRun(AIController controller)
		{
			Vector3 amount = UnityEngine.Random.insideUnitSphere;
			float d = UnityEngine.Random.Range(this._distance.x, this._distance.y);
			amount *= d;
			base.result = Behaviour.Result.Doing;
			float num = controller.target.transform.position.x - controller.character.transform.position.x;
			controller.character.lookingDirection = ((num > 0f) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			if (this._teleportStart.TryStart())
			{
				while (this._teleportStart.running)
				{
					yield return null;
				}
				yield return this._hide.CRun(controller);
				controller.character.transform.position = controller.target.transform.position + amount;
				this._teleportEnd.TryStart();
				while (this._teleportEnd.running)
				{
					yield return null;
				}
				yield return this._idle.CRun(controller);
				base.result = Behaviour.Result.Success;
			}
			else
			{
				base.result = Behaviour.Result.Fail;
			}
			yield break;
		}

		// Token: 0x04004E23 RID: 20003
		[SerializeField]
		private Characters.Actions.Action _teleportStart;

		// Token: 0x04004E24 RID: 20004
		[SerializeField]
		private Characters.Actions.Action _teleportEnd;

		// Token: 0x04004E25 RID: 20005
		[UnityEditor.Subcomponent(typeof(Hide))]
		[SerializeField]
		private Hide _hide;

		// Token: 0x04004E26 RID: 20006
		[UnityEditor.Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04004E27 RID: 20007
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _distance;
	}
}
