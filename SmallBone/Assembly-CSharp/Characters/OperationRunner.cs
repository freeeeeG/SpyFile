using System;
using Characters.Operations;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200070F RID: 1807
	[RequireComponent(typeof(PoolObject), typeof(OperationInfos))]
	public class OperationRunner : MonoBehaviour
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x0006E11A File Offset: 0x0006C31A
		public OperationInfos operationInfos
		{
			get
			{
				return this._operationInfos;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x0006E122 File Offset: 0x0006C322
		public PoolObject poolObject
		{
			get
			{
				return this._poolObject;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x0006E12A File Offset: 0x0006C32A
		public AttackDamage attackDamage
		{
			get
			{
				return this._attackDamage;
			}
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0006E132 File Offset: 0x0006C332
		private void Awake()
		{
			this._operationInfos.Initialize();
			this._operationInfos.onEnd += this._poolObject.Despawn;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x0006E15B File Offset: 0x0006C35B
		public OperationRunner Spawn()
		{
			return this._poolObject.Spawn(true).GetComponent<OperationRunner>();
		}

		// Token: 0x04001F12 RID: 7954
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x04001F13 RID: 7955
		[SerializeField]
		[Information("소환할 때 Copy Attack Damage 옵션 사용할 경우 반드시 설정해줘야 함. 이 때 min max는 어차피 덮어씌워지므로 무관", InformationAttribute.InformationType.Info, false)]
		private AttackDamage _attackDamage;

		// Token: 0x04001F14 RID: 7956
		[GetComponent]
		[SerializeField]
		private OperationInfos _operationInfos;
	}
}
