using System;
using Characters;
using Characters.Operations;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D9 RID: 729
	public sealed class RunOperationInfos : Runnable
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x0002D6F1 File Offset: 0x0002B8F1
		private void Awake()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0002D700 File Offset: 0x0002B900
		public override void Run()
		{
			if (this._owner == null)
			{
				this._owner = base.gameObject.GetComponentInParent<Character>();
			}
			if (this._owner.health.dead)
			{
				return;
			}
			if (this._reuseOperation)
			{
				this._operationInfos.gameObject.SetActive(true);
			}
			if (this._operationInfos.gameObject.activeSelf)
			{
				this._operationInfos.Run(this._owner);
			}
		}

		// Token: 0x04000C14 RID: 3092
		[SerializeField]
		private Character _owner;

		// Token: 0x04000C15 RID: 3093
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x04000C16 RID: 3094
		[SerializeField]
		private bool _reuseOperation;
	}
}
