using System;
using System.Collections;
using Characters.Actions;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x0200129A RID: 4762
	public class ChaseTeleport : Behaviour
	{
		// Token: 0x06005E6A RID: 24170 RVA: 0x001156D6 File Offset: 0x001138D6
		private void Awake()
		{
			this._teleportBounds = this._teleportBoundsCollider.bounds;
			this._destinationTransform.parent = Map.Instance.transform;
		}

		// Token: 0x06005E6B RID: 24171 RVA: 0x001156FE File Offset: 0x001138FE
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this._destinationTransform.position = this.SetDestination(controller);
			yield return this._teleport.CRun(controller);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005E6C RID: 24172 RVA: 0x00115714 File Offset: 0x00113914
		private Vector3 SetDestination(AIController controller)
		{
			Character target = controller.target;
			ChaseTeleport.Type type = this._type;
			if (type != ChaseTeleport.Type.RandomDestination)
			{
				if (type != ChaseTeleport.Type.BackOfTarget)
				{
					return this._destinationTransform.position;
				}
				Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider != null)
				{
					return new Vector2(controller.target.transform.position.x, lastStandingCollider.bounds.max.y);
				}
				return target.transform.position;
			}
			else
			{
				if (target == null)
				{
					return controller.transform.position;
				}
				Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
				Vector3 vector = target.transform.position - this._teleportBounds.size / 2f;
				Vector3 vector2 = target.transform.position + this._teleportBounds.size / 2f;
				return new Vector3(UnityEngine.Random.Range(Mathf.Max(bounds.min.x, vector.x), Mathf.Min(bounds.max.x, vector2.x)), bounds.max.y);
			}
		}

		// Token: 0x06005E6D RID: 24173 RVA: 0x00115869 File Offset: 0x00113A69
		public bool CanUse()
		{
			return this._actionForCooldown.cooldown.canUse;
		}

		// Token: 0x04004BDD RID: 19421
		[SerializeField]
		private ChaseTeleport.Type _type;

		// Token: 0x04004BDE RID: 19422
		[SerializeField]
		[Teleport.SubcomponentAttribute(true)]
		private Behaviour _teleport;

		// Token: 0x04004BDF RID: 19423
		[SerializeField]
		private Transform _destinationTransform;

		// Token: 0x04004BE0 RID: 19424
		[SerializeField]
		private Collider2D _teleportBoundsCollider;

		// Token: 0x04004BE1 RID: 19425
		[SerializeField]
		private Characters.Actions.Action _actionForCooldown;

		// Token: 0x04004BE2 RID: 19426
		private Bounds _teleportBounds;

		// Token: 0x0200129B RID: 4763
		private enum Type
		{
			// Token: 0x04004BE4 RID: 19428
			RandomDestination,
			// Token: 0x04004BE5 RID: 19429
			BackOfTarget
		}
	}
}
