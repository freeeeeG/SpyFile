using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012ED RID: 4845
	public class KeepDistance : Behaviour
	{
		// Token: 0x06005FCD RID: 24525 RVA: 0x001187C2 File Offset: 0x001169C2
		private void Start()
		{
			this._childs = new List<Behaviour>
			{
				this._moveToDestination,
				this._backStep
			};
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x001187E7 File Offset: 0x001169E7
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			switch (this._type)
			{
			case KeepDistance.Type.Move:
				yield return this.MoveToDestination(controller);
				break;
			case KeepDistance.Type.BackStepFromTarget:
				yield return this.BackStepFromTarget(controller);
				break;
			case KeepDistance.Type.BackStepToWide:
				yield return this.BackStepToWide(controller);
				break;
			}
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x001187FD File Offset: 0x001169FD
		private IEnumerator MoveToDestination(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			if (character.movement.controller.collisionState.lastStandingCollider == null)
			{
				yield break;
			}
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector2 lhs = (target.transform.position.x - character.transform.position.x > 0f) ? Vector2.left : Vector2.right;
			float num = UnityEngine.Random.Range(this._distance.x, this._distance.y);
			float x = (lhs == Vector2.left) ? Mathf.Max(bounds.min.x, character.transform.position.x - num) : Mathf.Min(bounds.max.x, character.transform.position.x + num);
			Vector3 position = character.transform.position;
			if (lhs == Vector2.right && bounds.max.x < position.x + num)
			{
				x = Mathf.Max(bounds.min.x, position.x - num);
			}
			else if (lhs == Vector2.left && bounds.min.x > position.x - num)
			{
				x = Mathf.Min(bounds.max.x, position.x + num);
			}
			controller.destination = new Vector2(x, 0f);
			this._moveCanUse = false;
			base.StartCoroutine(this.CCheckMoveCoolDown(controller.character.chronometer.master));
			yield return this._moveToDestination.CRun(controller);
			yield break;
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x00118813 File Offset: 0x00116A13
		private IEnumerator BackStepFromTarget(AIController controller)
		{
			Character character = controller.character;
			Vector2 lhs = (controller.target.transform.position.x - character.transform.position.x > 0f) ? Vector2.right : Vector2.left;
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			float num = UnityEngine.Random.Range(this._distance.x, this._distance.y);
			if (lhs == Vector2.right && bounds.min.x > character.transform.position.x - num)
			{
				character.ForceToLookAt(Character.LookingDirection.Left);
			}
			else if (lhs == Vector2.left && bounds.max.x < character.transform.position.x + num)
			{
				character.ForceToLookAt(Character.LookingDirection.Right);
			}
			yield return this._backStep.CRun(controller);
			yield break;
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x00118829 File Offset: 0x00116A29
		private IEnumerator BackStepToWide(AIController controller)
		{
			Character character = controller.character;
			Character target = controller.target;
			Bounds targetPlatformBounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector2 move = (targetPlatformBounds.center.x > character.transform.position.x) ? Vector2.right : Vector2.left;
			character.movement.move = move;
			yield return this._backStep.CRun(controller);
			if (targetPlatformBounds.center.x > character.transform.position.x)
			{
				character.lookingDirection = Character.LookingDirection.Left;
			}
			else
			{
				character.lookingDirection = Character.LookingDirection.Right;
			}
			yield break;
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0011883F File Offset: 0x00116A3F
		private IEnumerator CCheckMoveCoolDown(Chronometer chronometer)
		{
			yield return chronometer.WaitForSeconds(this._moveCooldownTime);
			this._moveCanUse = true;
			yield break;
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x00118855 File Offset: 0x00116A55
		public bool CanUseBackStep()
		{
			return this._backStep.CanUse();
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x00118862 File Offset: 0x00116A62
		public bool CanUseBackMove()
		{
			return this._moveCanUse;
		}

		// Token: 0x04004D11 RID: 19729
		[SerializeField]
		private KeepDistance.Type _type;

		// Token: 0x04004D12 RID: 19730
		[SerializeField]
		private float _moveCooldownTime;

		// Token: 0x04004D13 RID: 19731
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _distance;

		// Token: 0x04004D14 RID: 19732
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveToDestination))]
		private MoveToDestination _moveToDestination;

		// Token: 0x04004D15 RID: 19733
		[UnityEditor.Subcomponent(typeof(BackStep))]
		[SerializeField]
		private BackStep _backStep;

		// Token: 0x04004D16 RID: 19734
		private bool _moveCanUse = true;

		// Token: 0x020012EE RID: 4846
		private enum Type
		{
			// Token: 0x04004D18 RID: 19736
			Move,
			// Token: 0x04004D19 RID: 19737
			MoveToDistanceWithTarget,
			// Token: 0x04004D1A RID: 19738
			BackStepFromTarget,
			// Token: 0x04004D1B RID: 19739
			BackStepToWide
		}
	}
}
