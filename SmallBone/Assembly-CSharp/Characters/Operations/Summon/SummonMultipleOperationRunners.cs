using System;
using System.Collections;
using Characters.Operations.Attack;
using Characters.Utils;
using FX;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F40 RID: 3904
	public class SummonMultipleOperationRunners : CharacterOperation
	{
		// Token: 0x06004BEF RID: 19439 RVA: 0x000DF8D9 File Offset: 0x000DDAD9
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
			if (this._attackGroup)
			{
				this._hitHistoryManager = new HitHistoryManager(15);
			}
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x000DF8FC File Offset: 0x000DDAFC
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x000DF90C File Offset: 0x000DDB0C
		private IEnumerator CRun(Character owner)
		{
			Vector3[] preloadedPositions = new Vector3[this._summonOptions.values.Length];
			if (this._preloadPosition)
			{
				for (int i = 0; i < preloadedPositions.Length; i++)
				{
					Transform spawnPosition = this._summonOptions.values[i].spawnPosition;
					if (spawnPosition == null)
					{
						preloadedPositions[i] = base.transform.position;
					}
					else
					{
						preloadedPositions[i] = spawnPosition.position;
					}
				}
			}
			int optionIndex = 0;
			float time = 0f;
			SummonMultipleOperationRunners.SummonOption[] options = this._summonOptions.values;
			if (this._attackGroup)
			{
				this._hitHistoryManager.Clear();
			}
			while (optionIndex < options.Length)
			{
				while (optionIndex < options.Length && time >= options[optionIndex].timeToSpawn)
				{
					SummonMultipleOperationRunners.SummonOption summonOption = options[optionIndex];
					Vector3 vector;
					if (this._preloadPosition)
					{
						vector = preloadedPositions[optionIndex];
					}
					else
					{
						vector = ((summonOption.spawnPosition == null) ? base.transform.position : summonOption.spawnPosition.position);
					}
					vector += summonOption.noise.Evaluate();
					Vector3 vector2 = new Vector3(0f, 0f, this._extraAngleBySpawnPositionRotation ? (summonOption.spawnPosition.rotation.eulerAngles.z + summonOption.angle.value) : summonOption.angle.value);
					bool flag = summonOption.flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
					if (flag)
					{
						vector2.z = (180f - vector2.z) % 360f;
					}
					OperationRunner operationRunner = summonOption.operationRunner.Spawn();
					OperationInfos operationInfos = operationRunner.operationInfos;
					operationInfos.transform.SetPositionAndRotation(vector, Quaternion.Euler(vector2));
					if (summonOption.copyAttackDamage && this._attackDamage != null)
					{
						operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
						operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
					}
					if (flag)
					{
						operationInfos.transform.localScale = new Vector3(1f, -1f, 1f);
					}
					else
					{
						operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
					}
					operationInfos.Run(owner);
					if (this._attackGroup)
					{
						foreach (SweepAttack sweepAttack in operationInfos.GetComponentsInChildren<SweepAttack>())
						{
							sweepAttack.collisionDetector.group = true;
							sweepAttack.collisionDetector.hits = this._hitHistoryManager;
						}
					}
					if (summonOption.attachToOwner)
					{
						operationInfos.transform.parent = base.transform;
					}
					if (summonOption.notFlipOnOwner && owner.attach != null)
					{
						operationInfos.transform.parent = owner.attach.transform;
					}
					int j = optionIndex;
					optionIndex = j + 1;
				}
				yield return null;
				if (this._timeIndependant)
				{
					time += Chronometer.global.deltaTime;
				}
				else
				{
					time += owner.chronometer.animation.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x000D1BB8 File Offset: 0x000CFDB8
		public override void Stop()
		{
			base.Stop();
			base.StopAllCoroutines();
		}

		// Token: 0x04003B26 RID: 15142
		[SerializeField]
		[Tooltip("발동 시점에 미리 위치를 받아옵니다. 캐릭터가 이동해도 위치가 바뀌지 않게해야할 때 유용합니다.")]
		private bool _preloadPosition;

		// Token: 0x04003B27 RID: 15143
		[SerializeField]
		private bool _timeIndependant;

		// Token: 0x04003B28 RID: 15144
		[SerializeField]
		private bool _extraAngleBySpawnPositionRotation;

		// Token: 0x04003B29 RID: 15145
		[SerializeField]
		[Header("Sweepattack만 가능")]
		private bool _attackGroup;

		// Token: 0x04003B2A RID: 15146
		[SerializeField]
		private SummonMultipleOperationRunners.SummonOption.Reorderable _summonOptions;

		// Token: 0x04003B2B RID: 15147
		private AttackDamage _attackDamage;

		// Token: 0x04003B2C RID: 15148
		private HitHistoryManager _hitHistoryManager;

		// Token: 0x02000F41 RID: 3905
		[Serializable]
		private class SummonOption
		{
			// Token: 0x04003B2D RID: 15149
			[Tooltip("오퍼레이션 프리팹")]
			public OperationRunner operationRunner;

			// Token: 0x04003B2E RID: 15150
			[Tooltip("오퍼레이션이 스폰되는 시간")]
			public float timeToSpawn;

			// Token: 0x04003B2F RID: 15151
			public Transform spawnPosition;

			// Token: 0x04003B30 RID: 15152
			public CustomFloat scale = new CustomFloat(1f);

			// Token: 0x04003B31 RID: 15153
			public CustomAngle angle;

			// Token: 0x04003B32 RID: 15154
			public PositionNoise noise;

			// Token: 0x04003B33 RID: 15155
			[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
			public bool flipXByLookingDirection;

			// Token: 0x04003B34 RID: 15156
			[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
			public bool attachToOwner;

			// Token: 0x04003B35 RID: 15157
			[Tooltip("체크하면 캐릭터의 Attach오브젝트에 부착되며, 같이 움직임, 플립안됨")]
			public bool notFlipOnOwner;

			// Token: 0x04003B36 RID: 15158
			public bool copyAttackDamage;

			// Token: 0x02000F42 RID: 3906
			[Serializable]
			public class Reorderable : ReorderableArray<SummonMultipleOperationRunners.SummonOption>
			{
			}
		}
	}
}
