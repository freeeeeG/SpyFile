using System;
using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DF9 RID: 3577
	public class MoveEnemyBack : CharacterOperation
	{
		// Token: 0x06004795 RID: 18325 RVA: 0x000CFF81 File Offset: 0x000CE181
		static MoveEnemyBack()
		{
			MoveEnemyBack._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x000CFFA8 File Offset: 0x000CE1A8
		private void Awake()
		{
			this._onSuccess.Initialize();
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x000CFFB8 File Offset: 0x000CE1B8
		public override void Run(Character owner)
		{
			Character character = this.FindClosestPlayerBody(owner, this._collider2D);
			if (character == null)
			{
				return;
			}
			if (character.movement.config.type == Movement.Config.Type.Walking)
			{
				Vector2 destination;
				Vector2 direction;
				if (character.lookingDirection == Character.LookingDirection.Right)
				{
					destination = new Vector2(character.transform.position.x - this._distance, character.transform.position.y);
					direction = Vector2.left;
				}
				else
				{
					destination = new Vector2(character.transform.position.x + this._distance, character.transform.position.y);
					direction = Vector2.right;
				}
				if (owner.movement.controller.Teleport(destination, direction, this._distance))
				{
					owner.ForceToLookAt(character.transform.position.x);
					owner.movement.verticalVelocity = 0f;
					base.StartCoroutine(this._onSuccess.CRun(owner));
					return;
				}
				if (owner.movement.controller.Teleport(character.transform.position, direction, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
					base.StartCoroutine(this._onSuccess.CRun(owner));
					return;
				}
			}
			else
			{
				Vector2 destination = character.transform.position;
				if (owner.movement.controller.Teleport(destination, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
					base.StartCoroutine(this._onSuccess.CRun(owner));
				}
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x000D0154 File Offset: 0x000CE354
		private void OnDisable()
		{
			OperationInfo.Subcomponents onSuccess = this._onSuccess;
			if (onSuccess == null)
			{
				return;
			}
			onSuccess.StopAll();
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x000D0168 File Offset: 0x000CE368
		private Character FindClosestPlayerBody(Character character, Collider2D collider)
		{
			collider.enabled = true;
			List<Target> components = MoveEnemyBack._enemyOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				if (this._optimizeCollider)
				{
					collider.enabled = false;
				}
				return null;
			}
			if (components.Count == 1)
			{
				if (this._optimizeCollider)
				{
					collider.enabled = false;
				}
				return components[0].character;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < components.Count; i++)
			{
				if (!(components[i].character == null) && (components[i].character.type == Character.Type.TrashMob || components[i].character.type == Character.Type.Adventurer || components[i].character.type == Character.Type.Boss) && components[i].character.movement.isGrounded)
				{
					float distance = Physics2D.Distance(components[i].character.collider, character.collider).distance;
					if (num > distance)
					{
						index = i;
						num = distance;
					}
				}
			}
			if (this._optimizeCollider)
			{
				collider.enabled = false;
			}
			return components[index].character;
		}

		// Token: 0x04003698 RID: 13976
		[SerializeField]
		private bool _optimizeCollider = true;

		// Token: 0x04003699 RID: 13977
		[SerializeField]
		private Collider2D _collider2D;

		// Token: 0x0400369A RID: 13978
		[SerializeField]
		private float _distance = 1f;

		// Token: 0x0400369B RID: 13979
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(31);

		// Token: 0x0400369C RID: 13980
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onSuccess;
	}
}
