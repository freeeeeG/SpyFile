using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x02001326 RID: 4902
	public class EscapeTeleport : Behaviour
	{
		// Token: 0x060060C3 RID: 24771 RVA: 0x0011B91D File Offset: 0x00119B1D
		private void Awake()
		{
			this._teleportBounds = this._teleportBoundsCollider.bounds;
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0011B930 File Offset: 0x00119B30
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			Character target = controller.target;
			Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector3 vector = target.transform.position - this._teleportBounds.size / 2f;
			Vector3 vector2 = target.transform.position + this._teleportBounds.size / 2f;
			float x = UnityEngine.Random.Range(Mathf.Max(bounds.min.x, vector.x), Mathf.Min(bounds.max.x, vector2.x));
			this._destinationTransform.position = new Vector3(x, bounds.max.y);
			yield return this._teleport.CRun(controller);
			yield break;
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x0011B946 File Offset: 0x00119B46
		public bool CanUse()
		{
			return this._actionForCooldown.cooldown.canUse;
		}

		// Token: 0x04004E01 RID: 19969
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(Teleport))]
		private Teleport _teleport;

		// Token: 0x04004E02 RID: 19970
		[SerializeField]
		private Transform _destinationTransform;

		// Token: 0x04004E03 RID: 19971
		[SerializeField]
		private Collider2D _teleportBoundsCollider;

		// Token: 0x04004E04 RID: 19972
		[SerializeField]
		private Characters.Actions.Action _actionForCooldown;

		// Token: 0x04004E05 RID: 19973
		private Bounds _teleportBounds;
	}
}
