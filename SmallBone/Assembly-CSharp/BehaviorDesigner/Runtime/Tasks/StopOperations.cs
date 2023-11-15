using System;
using Characters.Operations;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001488 RID: 5256
	[TaskDescription("실행중인 Operation을 취소합니다.")]
	public sealed class StopOperations : Action
	{
		// Token: 0x0600666B RID: 26219 RVA: 0x00128651 File Offset: 0x00126851
		public override void OnAwake()
		{
			this._operationValue = this._operation.Value;
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x00128664 File Offset: 0x00126864
		public override TaskStatus OnUpdate()
		{
			if (this._operationValue == null)
			{
				return TaskStatus.Failure;
			}
			if (this._operationValue.running)
			{
				this._operationValue.Stop();
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005278 RID: 21112
		[SerializeField]
		private SharedOperations _operation;

		// Token: 0x04005279 RID: 21113
		private OperationInfos _operationValue;
	}
}
