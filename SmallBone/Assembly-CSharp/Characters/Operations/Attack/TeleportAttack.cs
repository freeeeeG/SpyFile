using System;
using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Attack
{
	// Token: 0x02000FA2 RID: 4002
	public sealed class TeleportAttack : CharacterOperation
	{
		// Token: 0x06004DC0 RID: 19904 RVA: 0x000E8475 File Offset: 0x000E6675
		static TeleportAttack()
		{
			TeleportAttack._enemyOverlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x000E849C File Offset: 0x000E669C
		private void Awake()
		{
			this._onHitTargetOperations.Initialize();
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x000E84AC File Offset: 0x000E66AC
		public override void Run(Character owner)
		{
			Character character = this.FindClosestTarget(owner, this._collider2D);
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
					this.Attack(owner, character);
					return;
				}
				if (owner.movement.controller.Teleport(character.transform.position, direction, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
					this.Attack(owner, character);
					return;
				}
			}
			else
			{
				Vector2 destination = character.transform.position;
				if (owner.movement.controller.Teleport(destination, this._distance))
				{
					owner.movement.verticalVelocity = 0f;
					this.Attack(owner, character);
				}
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x000E8628 File Offset: 0x000E6828
		private void Attack(Character attacker, Character target)
		{
			if (target.health.dead || !target.transform.gameObject.activeSelf)
			{
				return;
			}
			if (this._targetPoint != null)
			{
				Vector3 center = target.collider.bounds.center;
				Vector3 size = target.collider.bounds.size;
				size.x *= this._targetPointInfo.pivotValue.x;
				size.y *= this._targetPointInfo.pivotValue.y;
				Vector3 position = center + size;
				this._targetPoint.position = position;
			}
			if (this._adaptiveForce)
			{
				this._additionalHit.ChangeAdaptiveDamageAttribute(attacker);
			}
			Damage damage = attacker.stat.GetDamage((double)this._additionalDamageAmount.value, MMMaths.RandomPointWithinBounds(target.collider.bounds), this._additionalHit);
			attacker.StartCoroutine(this._onHitTargetOperations.CRun(attacker, target));
			attacker.Attack(target, ref damage);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x000E8734 File Offset: 0x000E6934
		private Character FindClosestTarget(Character character, Collider2D collider)
		{
			collider.enabled = true;
			List<Target> components = TeleportAttack._enemyOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
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
				if (!(components[i].character == null) && this._targetFilter[components[i].character.type] && components[i].character.movement.isGrounded)
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

		// Token: 0x04003D91 RID: 15761
		private static readonly NonAllocOverlapper _enemyOverlapper = new NonAllocOverlapper(31);

		// Token: 0x04003D92 RID: 15762
		[SerializeField]
		[Header("타겟 지정")]
		private CharacterTypeBoolArray _targetFilter;

		// Token: 0x04003D93 RID: 15763
		[SerializeField]
		private bool _optimizeCollider = true;

		// Token: 0x04003D94 RID: 15764
		[SerializeField]
		private Collider2D _collider2D;

		// Token: 0x04003D95 RID: 15765
		[SerializeField]
		private float _distance = 1f;

		// Token: 0x04003D96 RID: 15766
		[Header("공격")]
		[SerializeField]
		private CustomFloat _additionalDamageAmount;

		// Token: 0x04003D97 RID: 15767
		[SerializeField]
		private bool _adaptiveForce;

		// Token: 0x04003D98 RID: 15768
		[SerializeField]
		private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

		// Token: 0x04003D99 RID: 15769
		[SerializeField]
		private PositionInfo _targetPointInfo;

		// Token: 0x04003D9A RID: 15770
		[SerializeField]
		private Transform _targetPoint;

		// Token: 0x04003D9B RID: 15771
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onHitTargetOperations;
	}
}
