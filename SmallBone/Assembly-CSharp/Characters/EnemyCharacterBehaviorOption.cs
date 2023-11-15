using System;
using BehaviorDesigner.Runtime;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F6 RID: 1782
	[RequireComponent(typeof(BehaviorDesignerCommunicator))]
	public class EnemyCharacterBehaviorOption : MonoBehaviour
	{
		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x0006C23F File Offset: 0x0006A43F
		public bool IsStunAdaptable
		{
			get
			{
				return this._isStunAdaptable;
			}
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0006C247 File Offset: 0x0006A447
		private void Start()
		{
			this.SetSharedVariableOptions();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0006C24F File Offset: 0x0006A44F
		public void SetBehaviorOption(bool setTargetToPlayer = false, bool idleUntilFindTarget = false, bool staticMovement = false)
		{
			this._setTargetToPlayer = setTargetToPlayer;
			this._idleUntilFindTarget = idleUntilFindTarget;
			this._staticMovement = staticMovement;
			this.SetSharedVariableOptions();
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0006C26C File Offset: 0x0006A46C
		private void SetSharedVariableOptions()
		{
			if (this._communicator == null)
			{
				this._communicator = base.GetComponent<BehaviorDesignerCommunicator>();
			}
			this._communicator.SetVariable<SharedBool>("IsChasable", !this._staticMovement);
			this._communicator.SetVariable<SharedBool>("IsWander", !this._idleUntilFindTarget);
			if (this._setTargetToPlayer)
			{
				this.SetTargetToPlayer();
			}
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x0006C2E0 File Offset: 0x0006A4E0
		public void SetTargetToPlayer()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			this._communicator.SetVariable<SharedCharacter>("Target", player);
		}

		// Token: 0x04001EB4 RID: 7860
		public const string Target = "Target";

		// Token: 0x04001EB5 RID: 7861
		public const string IsWander = "IsWander";

		// Token: 0x04001EB6 RID: 7862
		public const string IsChasable = "IsChasable";

		// Token: 0x04001EB7 RID: 7863
		[SerializeField]
		private BehaviorDesignerCommunicator _communicator;

		// Token: 0x04001EB8 RID: 7864
		[Header("타겟찾기 옵션")]
		[SerializeField]
		private bool _setTargetToPlayer;

		// Token: 0x04001EB9 RID: 7865
		[SerializeField]
		private bool _idleUntilFindTarget;

		// Token: 0x04001EBA RID: 7866
		[Header("공격&추적 옵션")]
		[SerializeField]
		private bool _staticMovement;

		// Token: 0x04001EBB RID: 7867
		[Header("상태이상 옵션")]
		[SerializeField]
		private bool _isStunAdaptable = true;
	}
}
