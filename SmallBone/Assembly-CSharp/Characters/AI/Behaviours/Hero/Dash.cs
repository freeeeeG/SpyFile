using System;
using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero
{
	// Token: 0x020013B1 RID: 5041
	public class Dash : Behaviour
	{
		// Token: 0x0600636A RID: 25450 RVA: 0x00121144 File Offset: 0x0011F344
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._readyAction.TryStart();
			while (this._readyAction.running)
			{
				yield return null;
			}
			this.SetDestination(controller);
			this._attackAction.TryStart();
			yield return this.CMoveToDestination(controller, controller.character);
			if (!controller.dead)
			{
				controller.character.CancelAction();
			}
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x0600636B RID: 25451 RVA: 0x0012115C File Offset: 0x0011F35C
		private void SetDestination(AIController controller)
		{
			float num = UnityEngine.Random.Range(this._distanceRange.x, this._distanceRange.y);
			if (controller.target == null)
			{
				throw new Exception("target is null");
			}
			float y = controller.character.movement.controller.collisionState.lastStandingCollider.bounds.max.y;
			float x;
			if (MMMaths.RandomBool())
			{
				x = Math.Min(controller.character.movement.controller.collisionState.lastStandingCollider.bounds.max.x, controller.target.transform.position.x + num);
			}
			else
			{
				x = Math.Max(controller.character.movement.controller.collisionState.lastStandingCollider.bounds.min.x, controller.target.transform.position.x - num);
			}
			controller.destination = new Vector2(x, y);
		}

		// Token: 0x0600636C RID: 25452 RVA: 0x00121273 File Offset: 0x0011F473
		private IEnumerator CMoveToDestination(AIController controller, Character owner)
		{
			Vector2 destination = controller.destination;
			Vector3 source = owner.transform.position;
			float elapsed = 0f;
			float num = Mathf.Abs(destination.x - source.x);
			float duration = num * owner.stat.GetInterpolatedMovementSpeed() / 60f;
			this.curve.duration = duration * this._durationMultiplierPerDistance;
			while (elapsed < this.curve.duration)
			{
				yield return null;
				if (!owner.stunedOrFreezed)
				{
					if (elapsed < duration)
					{
						owner.ForceToLookAt(destination.x);
					}
					float num2 = Mathf.Lerp(source.x, destination.x, this.curve.Evaluate(elapsed));
					owner.movement.force = new Vector2(num2 - owner.transform.position.x, 0f);
					elapsed += owner.chronometer.master.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x04005027 RID: 20519
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _readyAction;

		// Token: 0x04005028 RID: 20520
		[UnityEditor.Subcomponent(typeof(ChainAction))]
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04005029 RID: 20521
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _distanceRange;

		// Token: 0x0400502A RID: 20522
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(MoveToDestination))]
		private MoveToDestination _moveToDestination;

		// Token: 0x0400502B RID: 20523
		[SerializeField]
		private Curve curve;

		// Token: 0x0400502C RID: 20524
		[SerializeField]
		private float _durationMultiplierPerDistance = 1f;
	}
}
