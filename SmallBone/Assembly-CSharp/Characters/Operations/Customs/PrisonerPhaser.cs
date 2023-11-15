using System;
using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE2 RID: 4066
	public class PrisonerPhaser : CharacterOperation
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x000EB75A File Offset: 0x000E995A
		static PrisonerPhaser()
		{
			PrisonerPhaser._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x000EB784 File Offset: 0x000E9984
		public override void Run(Character owner)
		{
			Target target = this.FindClosestPlayerBody(owner, this._collider2D);
			if (target == null)
			{
				return;
			}
			if (target.character != null && target.character.movement.config.type == Movement.Config.Type.Walking)
			{
				Vector2 destination;
				Vector2 direction;
				if (target.character.lookingDirection == Character.LookingDirection.Right)
				{
					destination = new Vector2(target.transform.position.x - this._distance, target.transform.position.y);
					direction = Vector2.left;
				}
				else
				{
					destination = new Vector2(target.transform.position.x + this._distance, target.transform.position.y);
					direction = Vector2.right;
				}
				if (owner.movement.controller.Teleport(destination, direction, this._distance))
				{
					owner.ForceToLookAt(target.character.lookingDirection);
					owner.movement.verticalVelocity = 0f;
					return;
				}
				if (owner.movement.controller.Teleport(target.transform.position, direction, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
					return;
				}
			}
			else
			{
				Vector2 destination;
				Vector2 direction;
				if (MMMaths.RandomBool())
				{
					destination = new Vector2(target.transform.position.x - this._distance, target.transform.position.y);
					direction = Vector2.left;
				}
				else
				{
					destination = new Vector2(target.transform.position.x + this._distance, target.transform.position.y);
					direction = Vector2.right;
				}
				if (owner.movement.controller.Teleport(destination, direction, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
				}
			}
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x000EB960 File Offset: 0x000E9B60
		private Target FindClosestPlayerBody(Character character, Collider2D collider)
		{
			List<Target> components = PrisonerPhaser._enemyOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				return null;
			}
			if (components.Count == 1)
			{
				return components[0];
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < components.Count; i++)
			{
				if (!(components[i].character == null) && components[i].character.movement.isGrounded)
				{
					float distance = Physics2D.Distance(components[i].character.collider, character.collider).distance;
					if (num > distance)
					{
						index = i;
						num = distance;
					}
				}
			}
			return components[index];
		}

		// Token: 0x04003EA9 RID: 16041
		[SerializeField]
		private Collider2D _collider2D;

		// Token: 0x04003EAA RID: 16042
		[SerializeField]
		private float _distance = 1f;

		// Token: 0x04003EAB RID: 16043
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(31);
	}
}
