using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012D4 RID: 4820
	public sealed class LightJump : Behaviour
	{
		// Token: 0x06005F58 RID: 24408 RVA: 0x00117471 File Offset: 0x00115671
		private void Awake()
		{
			this._childs = new List<Behaviour>
			{
				this._teleport,
				this._fall,
				this._attack,
				this._escapeTeleport
			};
		}

		// Token: 0x06005F59 RID: 24409 RVA: 0x001174AE File Offset: 0x001156AE
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._hide.CRun(controller);
			if (!controller.character.movement.controller.Teleport(this._destination.position))
			{
				yield break;
			}
			if (!this._teleportEnd.TryStart())
			{
				yield break;
			}
			while (this._teleportEnd.running && base.result == Behaviour.Result.Doing)
			{
				yield return null;
			}
			if (base.result != Behaviour.Result.Doing)
			{
				yield break;
			}
			base.StartCoroutine(this._fall.CRun(controller));
			while (this._fall.result == Behaviour.Result.Doing)
			{
				if (controller.character.movement.isGrounded)
				{
					controller.character.CancelAction();
					this._fall.StopPropagation();
					yield break;
				}
				yield return null;
			}
			if (base.result != Behaviour.Result.Doing)
			{
				yield break;
			}
			yield return this._attack.CRun(controller);
			if (base.result != Behaviour.Result.Doing)
			{
				yield break;
			}
			yield return this._escapeTeleport.CRun(controller);
			base.result = Behaviour.Result.Done;
			yield break;
		}

		// Token: 0x06005F5A RID: 24410 RVA: 0x001174C4 File Offset: 0x001156C4
		public bool CanUse(Character character)
		{
			if (!this._fall.CanUse() || !this._attack.CanUse())
			{
				return false;
			}
			if (!character.movement.isGrounded)
			{
				return false;
			}
			Bounds bounds = character.collider.bounds;
			bounds.center = new Vector2(this._destination.position.x, this._destination.position.y + (bounds.center.y - bounds.min.y));
			NonAllocOverlapper.shared.contactFilter.SetLayerMask(Layers.terrainMask | 17);
			return NonAllocOverlapper.shared.OverlapBox(bounds.center, bounds.size, 0f).results.Count == 0;
		}

		// Token: 0x04004C9C RID: 19612
		[UnityEditor.Subcomponent(typeof(Teleport))]
		[SerializeField]
		private Teleport _teleport;

		// Token: 0x04004C9D RID: 19613
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _fall;

		// Token: 0x04004C9E RID: 19614
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _attack;

		// Token: 0x04004C9F RID: 19615
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(EscapeTeleport))]
		private EscapeTeleport _escapeTeleport;

		// Token: 0x04004CA0 RID: 19616
		[UnityEditor.Subcomponent(typeof(ShiftObject))]
		[SerializeField]
		private ShiftObject _shiftObject;

		// Token: 0x04004CA1 RID: 19617
		[SerializeField]
		private Transform _destination;

		// Token: 0x04004CA2 RID: 19618
		[UnityEditor.Subcomponent(typeof(Hide))]
		[SerializeField]
		private Hide _hide;

		// Token: 0x04004CA3 RID: 19619
		[SerializeField]
		private Characters.Actions.Action _teleportStart;

		// Token: 0x04004CA4 RID: 19620
		[SerializeField]
		private Characters.Actions.Action _teleportEnd;
	}
}
