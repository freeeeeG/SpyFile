using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200132B RID: 4907
	public class TeleportBehind : Behaviour
	{
		// Token: 0x060060D8 RID: 24792 RVA: 0x0011BC8E File Offset: 0x00119E8E
		public override IEnumerator CRun(AIController controller)
		{
			base.StartCoroutine(this.SetDestination(controller));
			yield return this._teleport.CRun(controller);
			yield break;
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x0011BCA4 File Offset: 0x00119EA4
		private IEnumerator SetDestination(AIController controller)
		{
			Character target = controller.target;
			float amount = UnityEngine.Random.Range(this._distance.x, this._distance.y);
			yield return controller.character.chronometer.master.WaitForSeconds(this._destinationSettingDelay);
			float num = (target.lookingDirection == Character.LookingDirection.Right) ? (amount * -1f) : amount;
			float num2 = target.transform.position.x + num;
			Bounds bounds;
			Collider2D collider2D;
			if (this._lastStandingCollider)
			{
				bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			}
			else if (target.movement.TryGetClosestBelowCollider(out collider2D, this._groundMask, 100f))
			{
				bounds = collider2D.bounds;
			}
			else
			{
				bounds = controller.character.movement.controller.collisionState.lastStandingCollider.bounds;
			}
			if (num2 <= bounds.min.x + 0.5f && target.lookingDirection == Character.LookingDirection.Right)
			{
				num2 = bounds.min.x + 0.5f;
			}
			else if (num2 >= bounds.max.x - 0.5f && target.lookingDirection == Character.LookingDirection.Left)
			{
				num2 = bounds.max.x - 0.5f;
			}
			this._destinationTransform.position = new Vector3(num2, bounds.max.y + controller.character.collider.size.y);
			yield break;
		}

		// Token: 0x04004E13 RID: 19987
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Teleport))]
		private Teleport _teleport;

		// Token: 0x04004E14 RID: 19988
		[SerializeField]
		private Transform _destinationTransform;

		// Token: 0x04004E15 RID: 19989
		[Information("Hide의 최소 시간 이하", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _destinationSettingDelay;

		// Token: 0x04004E16 RID: 19990
		[SerializeField]
		[MinMaxSlider(-10f, 10f)]
		private Vector2 _distance;

		// Token: 0x04004E17 RID: 19991
		[SerializeField]
		private bool _lastStandingCollider = true;

		// Token: 0x04004E18 RID: 19992
		[SerializeField]
		private LayerMask _groundMask = Layers.groundMask;
	}
}
