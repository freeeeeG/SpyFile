using System;
using System.Collections;
using Characters.AI;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FC6 RID: 4038
	public sealed class FlameStorm : CharacterOperation
	{
		// Token: 0x06004E2B RID: 20011 RVA: 0x000E9ED4 File Offset: 0x000E80D4
		private void Awake()
		{
			this._numbers = new int[this._spawnPointContainer.childCount];
			for (int i = 0; i < this._spawnPointContainer.childCount; i++)
			{
				this._numbers[i] = i;
			}
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x000E9F16 File Offset: 0x000E8116
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x000E9F24 File Offset: 0x000E8124
		public override void Run(Character owner)
		{
			this._numbers.Shuffle<int>();
			for (int i = 0; i < this._spawnPointContainer.childCount - this._emptyCount; i++)
			{
				int index = this._numbers[i];
				Vector3 position = this._spawnPointContainer.GetChild(index).position;
				OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
				operationInfos.transform.position = position;
				operationInfos.Run(owner);
			}
			int index2 = this._numbers[UnityEngine.Random.Range(0, this._spawnPointContainer.childCount - this._emptyCount)];
			base.StartCoroutine(this.CFire(owner, this._spawnPointContainer.GetChild(index2)));
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x000E9FD0 File Offset: 0x000E81D0
		private IEnumerator CFire(Character owner, Transform fireTransform)
		{
			yield return Chronometer.global.WaitForSeconds(this._fireDelay);
			float direction;
			if (this._ai.target.transform.position.x > fireTransform.position.x)
			{
				direction = 0f;
			}
			else
			{
				direction = 180f;
			}
			this._projectile.reusable.Spawn(fireTransform.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, direction, false, false, 1f, null, 0f);
			yield break;
		}

		// Token: 0x04003E30 RID: 15920
		[SerializeField]
		private AIController _ai;

		// Token: 0x04003E31 RID: 15921
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x04003E32 RID: 15922
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04003E33 RID: 15923
		[SerializeField]
		private Transform _spawnPointContainer;

		// Token: 0x04003E34 RID: 15924
		[SerializeField]
		private int _emptyCount = 2;

		// Token: 0x04003E35 RID: 15925
		[SerializeField]
		private float _fireDelay = 1.5f;

		// Token: 0x04003E36 RID: 15926
		private int[] _numbers;

		// Token: 0x04003E37 RID: 15927
		private IAttackDamage _attackDamage;
	}
}
