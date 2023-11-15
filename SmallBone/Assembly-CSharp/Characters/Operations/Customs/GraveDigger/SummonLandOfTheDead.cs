using System;
using Characters.Abilities.Customs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.GraveDigger
{
	// Token: 0x02001010 RID: 4112
	public sealed class SummonLandOfTheDead : CharacterOperation
	{
		// Token: 0x06004F63 RID: 20323 RVA: 0x000EEC8F File Offset: 0x000ECE8F
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x000EECA0 File Offset: 0x000ECEA0
		public override void Run(Character owner)
		{
			Collider2D collider;
			if (!this.TryFindGround(owner, out collider))
			{
				return;
			}
			this.SummonLand(owner, collider);
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x000EECC4 File Offset: 0x000ECEC4
		private bool TryFindGround(Character character, out Collider2D collider)
		{
			if (character.movement.isGrounded)
			{
				collider = character.movement.controller.collisionState.lastStandingCollider;
				return true;
			}
			return character.movement.TryGetClosestBelowCollider(out collider, Layers.groundMask, 100f);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x000EED14 File Offset: 0x000ECF14
		private void SummonLand(Character owner, Collider2D collider)
		{
			Bounds bounds = collider.bounds;
			Vector2 mostLeftTop = bounds.GetMostLeftTop();
			Vector2 mostRightTop = bounds.GetMostRightTop();
			Vector3 position = owner.transform.position;
			mostLeftTop.x = math.max(mostLeftTop.x, position.x - this._range);
			mostRightTop.x = math.min(mostRightTop.x, position.x + this._range);
			float num = (mostRightTop.x - mostLeftTop.x) / this._width;
			float num2 = num - (float)((int)num);
			Vector2 vector = mostLeftTop;
			vector.x = mostLeftTop.x + num2 * this._width / 2f;
			short num3 = 0;
			while ((float)num3 < num)
			{
				Vector2 position2 = vector;
				position2.x += this._width * (float)num3;
				this.Summon(owner, position2, num3);
				num3 += 1;
			}
			this.SummonCorpseSpawner(owner, position, mostLeftTop, mostRightTop);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x000EEE00 File Offset: 0x000ED000
		private void Summon(Character owner, Vector2 position, short sortingOrder)
		{
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.identity);
			if (this._copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = sortingOrder;
				sortingOrder = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x000EEEA8 File Offset: 0x000ED0A8
		private void SummonCorpseSpawner(Character owner, Vector2 position, Vector2 left, Vector2 right)
		{
			OperationInfos operationInfos = this._corpseSpawner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.identity);
			operationInfos.GetComponentInChildren<SpawnCorpseForLandOfTheDead>().Set(this._passive, left, right);
			operationInfos.Run(owner);
		}

		// Token: 0x04003F94 RID: 16276
		[SerializeField]
		private GraveDiggerPassiveComponent _passive;

		// Token: 0x04003F95 RID: 16277
		[SerializeField]
		private float _groundFindingDistance;

		// Token: 0x04003F96 RID: 16278
		[SerializeField]
		private float _range;

		// Token: 0x04003F97 RID: 16279
		[SerializeField]
		private float _width;

		// Token: 0x04003F98 RID: 16280
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x04003F99 RID: 16281
		[SerializeField]
		private OperationRunner _corpseSpawner;

		// Token: 0x04003F9A RID: 16282
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003F9B RID: 16283
		private AttackDamage _attackDamage;
	}
}
