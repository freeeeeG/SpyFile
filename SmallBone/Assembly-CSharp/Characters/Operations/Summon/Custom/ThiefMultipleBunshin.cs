using System;
using System.Collections.Generic;
using Characters.Operations.Summon.SummonInRange.Policy;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon.Custom
{
	// Token: 0x02000F62 RID: 3938
	public class ThiefMultipleBunshin : CharacterOperation
	{
		// Token: 0x06004C83 RID: 19587 RVA: 0x000E2D2A File Offset: 0x000E0F2A
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x000E2D38 File Offset: 0x000E0F38
		public override void Run(Character owner)
		{
			List<OperationRunner> list = new List<OperationRunner>();
			Vector2 originPosition = this._summonRange.transform.position;
			List<int> list2 = new List<int>();
			for (int i = 0; i < this._operationRunnerInfos.Length; i++)
			{
				for (int j = 0; j < this._operationRunnerInfos[i].count; j++)
				{
					list2.Add(i);
				}
			}
			list2.Shuffle<int>();
			for (int k = 0; k < list2.Count; k++)
			{
				Vector2 position = this._positionPolicy.GetPosition(originPosition, this._summonRange.size.x, list2.Count, k);
				int num = list2[k];
				OperationRunner operationRunner = this._operationRunnerInfos[num].operationRunner.Spawn();
				list.Add(operationRunner);
				operationRunner.transform.localScale = new Vector3(1f, 1f, 1f);
				operationRunner.transform.SetPositionAndRotation(position, Quaternion.identity);
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
					short num2 = ThiefMultipleBunshin.spriteLayer;
					ThiefMultipleBunshin.spriteLayer = num2 + 1;
					sortingGroup.sortingOrder = (int)num2;
				}
				if (this._attachToOwner)
				{
					operationRunner2.transform.parent = base.transform;
				}
			}
			foreach (OperationRunner operationRunner3 in list)
			{
				operationRunner3.operationInfos.Run(owner);
			}
		}

		// Token: 0x04003C25 RID: 15397
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003C26 RID: 15398
		[SerializeField]
		private ThiefMultipleBunshin.OperationRunnerInfo[] _operationRunnerInfos;

		// Token: 0x04003C27 RID: 15399
		[SerializeField]
		private BoxCollider2D _summonRange;

		// Token: 0x04003C28 RID: 15400
		[SerializeReference]
		[SubclassSelector]
		private ISummonPosition _positionPolicy;

		// Token: 0x04003C29 RID: 15401
		[SerializeField]
		[Space]
		private bool _attachToOwner;

		// Token: 0x04003C2A RID: 15402
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003C2B RID: 15403
		private AttackDamage _attackDamage;

		// Token: 0x02000F63 RID: 3939
		[Serializable]
		public struct OperationRunnerInfo
		{
			// Token: 0x04003C2C RID: 15404
			[SerializeField]
			internal OperationRunner operationRunner;

			// Token: 0x04003C2D RID: 15405
			[SerializeField]
			internal int count;
		}
	}
}
