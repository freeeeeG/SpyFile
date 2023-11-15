using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.DarkQuartzGolem
{
	// Token: 0x0200138F RID: 5007
	public class Rush : Behaviour, IPattern
	{
		// Token: 0x060062CE RID: 25294 RVA: 0x0011F93C File Offset: 0x0011DB3C
		public bool CanUse(AIController controller)
		{
			if (!this._action.canUse)
			{
				return false;
			}
			Character target = controller.target;
			Character character = controller.character;
			UnityEngine.Object lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
			Collider2D lastStandingCollider2 = character.movement.controller.collisionState.lastStandingCollider;
			return lastStandingCollider == lastStandingCollider2;
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x0011F99A File Offset: 0x0011DB9A
		public bool CanUse()
		{
			return this._action.canUse;
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x0011F9A7 File Offset: 0x0011DBA7
		public override IEnumerator CRun(AIController controller)
		{
			Character target = controller.target;
			Character character = controller.character;
			Collider2D ownerPlatform = character.movement.controller.collisionState.lastStandingCollider;
			character.ForceToLookAt(target.transform.position.x);
			this._ready.TryStart();
			while (this._ready.running)
			{
				yield return null;
			}
			this._action.TryStart();
			if (target.transform.position.x > character.transform.position.x)
			{
				this.SetWalkDestinationToMax(controller, ownerPlatform.bounds);
			}
			else
			{
				this.SetWalkDestinationToMin(controller, ownerPlatform.bounds);
			}
			yield return this._moveToDestination.CRun(controller);
			character.CancelAction();
			yield break;
		}

		// Token: 0x060062D1 RID: 25297 RVA: 0x0011F9C0 File Offset: 0x0011DBC0
		private void SetWalkDestinationToMin(AIController controller, Bounds bounds)
		{
			float x = bounds.min.x + controller.character.collider.bounds.size.x;
			float y = bounds.max.y;
			controller.destination = new Vector2(x, y);
		}

		// Token: 0x060062D2 RID: 25298 RVA: 0x0011FA14 File Offset: 0x0011DC14
		private void SetWalkDestinationToMax(AIController controller, Bounds bounds)
		{
			float x = bounds.max.x - controller.character.collider.bounds.size.x;
			float y = bounds.max.y;
			controller.destination = new Vector2(x, y);
		}

		// Token: 0x04004FA6 RID: 20390
		[SerializeField]
		private Characters.Actions.Action _ready;

		// Token: 0x04004FA7 RID: 20391
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004FA8 RID: 20392
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveToDestination))]
		private MoveToDestination _moveToDestination;
	}
}
