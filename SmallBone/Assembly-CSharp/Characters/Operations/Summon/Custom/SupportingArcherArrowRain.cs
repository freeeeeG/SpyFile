using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations.Summon.SummonInRange.Policy;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon.Custom
{
	// Token: 0x02000F60 RID: 3936
	public class SupportingArcherArrowRain : CharacterOperation
	{
		// Token: 0x06004C77 RID: 19575 RVA: 0x000E2A40 File Offset: 0x000E0C40
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x000E2A50 File Offset: 0x000E0C50
		public override void Run(Character owner)
		{
			Vector3 euler = new Vector3(0f, 0f, this._angle.value);
			List<OperationRunner> list = new List<OperationRunner>();
			Vector2 originPosition = this._summonPosition.position;
			for (int i = 0; i < this._summonCount; i++)
			{
				Vector2 position = this._positionPolicy.GetPosition(originPosition, this._summonRange.size.x, this._summonCount, i);
				OperationRunner operationRunner = this._operationRunner.Spawn();
				list.Add(operationRunner);
				operationRunner.transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
			}
			foreach (OperationRunner operationRunner2 in list)
			{
				if (this._copyAttackDamage && this._attackDamage != null)
				{
					operationRunner2.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
					operationRunner2.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
				}
				SortingGroup component = operationRunner2.GetComponent<SortingGroup>();
				if (component != null)
				{
					SortingGroup sortingGroup = component;
					short num = SupportingArcherArrowRain.spriteLayer;
					SupportingArcherArrowRain.spriteLayer = num + 1;
					sortingGroup.sortingOrder = (int)num;
				}
				if (this._attachToOwner)
				{
					operationRunner2.transform.parent = base.transform;
				}
			}
			base.StartCoroutine(this.CRunAfterDelay(owner, list));
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x000E2BCC File Offset: 0x000E0DCC
		private IEnumerator CRunAfterDelay(Character owner, List<OperationRunner> operationRunners)
		{
			foreach (OperationRunner operationRunner in operationRunners)
			{
				operationRunner.operationInfos.Run(owner);
				yield return new WaitForSeconds(this._delay.value);
			}
			List<OperationRunner>.Enumerator enumerator = default(List<OperationRunner>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x04003C13 RID: 15379
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003C14 RID: 15380
		[SerializeField]
		[Space]
		internal OperationRunner _operationRunner;

		// Token: 0x04003C15 RID: 15381
		[SerializeField]
		private Transform _summonPosition;

		// Token: 0x04003C16 RID: 15382
		[SerializeField]
		private BoxCollider2D _summonRange;

		// Token: 0x04003C17 RID: 15383
		[SerializeField]
		private int _summonCount;

		// Token: 0x04003C18 RID: 15384
		[SerializeField]
		private CustomFloat _delay;

		// Token: 0x04003C19 RID: 15385
		[SerializeReference]
		[SubclassSelector]
		private ISummonPosition _positionPolicy;

		// Token: 0x04003C1A RID: 15386
		[Space]
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003C1B RID: 15387
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003C1C RID: 15388
		[Space]
		[SerializeField]
		private bool _attachToOwner;

		// Token: 0x04003C1D RID: 15389
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003C1E RID: 15390
		private AttackDamage _attackDamage;
	}
}
