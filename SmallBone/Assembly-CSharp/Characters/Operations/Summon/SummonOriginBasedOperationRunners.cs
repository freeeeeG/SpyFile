using System;
using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F58 RID: 3928
	public class SummonOriginBasedOperationRunners : CharacterOperation
	{
		// Token: 0x06004C5E RID: 19550 RVA: 0x000E2406 File Offset: 0x000E0606
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._summonOptions.Dispose();
		}

		// Token: 0x06004C5F RID: 19551 RVA: 0x000E2419 File Offset: 0x000E0619
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C60 RID: 19552 RVA: 0x000E2427 File Offset: 0x000E0627
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CRun(owner));
		}

		// Token: 0x06004C61 RID: 19553 RVA: 0x000E2437 File Offset: 0x000E0637
		private IEnumerator CRun(Character owner)
		{
			Vector3[] preloadedPositions = new Vector3[this._summonOptions.values.Length];
			if (this._preloadPosition)
			{
				for (int i = 0; i < preloadedPositions.Length; i++)
				{
					Transform originPosition = this._summonOptions.values[i].originPosition;
					if (originPosition == null)
					{
						preloadedPositions[i] = base.transform.position;
					}
					else
					{
						preloadedPositions[i] = originPosition.position;
					}
				}
			}
			int optionIndex = 0;
			float time = 0f;
			SummonOriginBasedOperationRunners.SummonOption[] options = this._summonOptions.values;
			while (optionIndex < options.Length)
			{
				while (optionIndex < options.Length && time >= options[optionIndex].timeToSpawn)
				{
					SummonOriginBasedOperationRunners.SummonOption summonOption = options[optionIndex];
					Vector3 vector;
					if (this._preloadPosition)
					{
						vector = preloadedPositions[optionIndex];
					}
					else
					{
						vector = ((summonOption.originPosition == null) ? base.transform.position : summonOption.originPosition.position);
					}
					float num = summonOption.angle.value;
					num += (summonOption.extraMoveAngleByOrigin ? summonOption.originPosition.rotation.eulerAngles.z : 0f);
					num *= 0.017453292f;
					vector += new Vector3(Mathf.Cos(num), Mathf.Sin(num), 0f) * summonOption.moveAmount;
					vector += summonOption.noise.Evaluate();
					Vector3 vector2 = new Vector3(0f, 0f, this._extraAngleBySpawnPositionRotation ? (summonOption.originPosition.rotation.eulerAngles.z + summonOption.angle.value) : summonOption.angle.value);
					bool flag = summonOption.flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
					if (summonOption.flipXByLookingDirection)
					{
						flag = (summonOption.originPosition.localScale.x > 0f);
					}
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
					if (summonOption.attachToOwner)
					{
						operationInfos.transform.parent = base.transform;
					}
					if (summonOption.notFlipOnOwner && owner.attach != null)
					{
						operationInfos.transform.parent = owner.attach.transform;
					}
					int num2 = optionIndex;
					optionIndex = num2 + 1;
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

		// Token: 0x06004C62 RID: 19554 RVA: 0x000D1BB8 File Offset: 0x000CFDB8
		public override void Stop()
		{
			base.Stop();
			base.StopAllCoroutines();
		}

		// Token: 0x04003BF3 RID: 15347
		[Tooltip("발동 시점에 미리 위치를 받아옵니다. 캐릭터가 이동해도 위치가 바뀌지 않게해야할 때 유용합니다.")]
		[SerializeField]
		private bool _preloadPosition;

		// Token: 0x04003BF4 RID: 15348
		[SerializeField]
		private bool _timeIndependant;

		// Token: 0x04003BF5 RID: 15349
		[SerializeField]
		private bool _extraAngleBySpawnPositionRotation;

		// Token: 0x04003BF6 RID: 15350
		[SerializeField]
		private SummonOriginBasedOperationRunners.SummonOption.Reorderable _summonOptions;

		// Token: 0x04003BF7 RID: 15351
		private AttackDamage _attackDamage;

		// Token: 0x02000F59 RID: 3929
		[Serializable]
		private class SummonOption
		{
			// Token: 0x17000F3C RID: 3900
			// (get) Token: 0x06004C64 RID: 19556 RVA: 0x000E244D File Offset: 0x000E064D
			public bool extraMoveAngleByOrigin
			{
				get
				{
					return this._extraMoveAngleByOrigin;
				}
			}

			// Token: 0x17000F3D RID: 3901
			// (get) Token: 0x06004C65 RID: 19557 RVA: 0x000E2455 File Offset: 0x000E0655
			public float moveAmount
			{
				get
				{
					return this._moveAmountFromOrigin.value;
				}
			}

			// Token: 0x06004C66 RID: 19558 RVA: 0x000E2462 File Offset: 0x000E0662
			public void Dispose()
			{
				this.operationRunner = null;
			}

			// Token: 0x04003BF8 RID: 15352
			[Tooltip("오퍼레이션 프리팹")]
			public OperationRunner operationRunner;

			// Token: 0x04003BF9 RID: 15353
			[Tooltip("오퍼레이션이 스폰되는 시간")]
			public float timeToSpawn;

			// Token: 0x04003BFA RID: 15354
			public Transform originPosition;

			// Token: 0x04003BFB RID: 15355
			[SerializeField]
			private CustomAngle _moveAngleFromOrigin;

			// Token: 0x04003BFC RID: 15356
			[SerializeField]
			private CustomFloat _moveAmountFromOrigin;

			// Token: 0x04003BFD RID: 15357
			[SerializeField]
			private bool _extraMoveAngleByOrigin;

			// Token: 0x04003BFE RID: 15358
			public CustomFloat scale = new CustomFloat(1f);

			// Token: 0x04003BFF RID: 15359
			public CustomAngle angle;

			// Token: 0x04003C00 RID: 15360
			public PositionNoise noise;

			// Token: 0x04003C01 RID: 15361
			[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
			public bool flipXByLookingDirection;

			// Token: 0x04003C02 RID: 15362
			[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
			public bool attachToOwner;

			// Token: 0x04003C03 RID: 15363
			[Tooltip("체크하면 캐릭터의 Attach오브젝트에 부착되며, 같이 움직임, 플립안됨")]
			public bool notFlipOnOwner;

			// Token: 0x04003C04 RID: 15364
			public bool copyAttackDamage;

			// Token: 0x02000F5A RID: 3930
			[Serializable]
			public class Reorderable : ReorderableArray<SummonOriginBasedOperationRunners.SummonOption>
			{
				// Token: 0x06004C68 RID: 19560 RVA: 0x000E2484 File Offset: 0x000E0684
				public void Dispose()
				{
					for (int i = 0; i < this.values.Length; i++)
					{
						this.values[i].Dispose();
						this.values[i] = null;
					}
				}
			}
		}
	}
}
