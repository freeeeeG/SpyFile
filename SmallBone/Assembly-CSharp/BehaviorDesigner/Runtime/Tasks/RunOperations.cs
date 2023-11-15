using System;
using Characters;
using Characters.Operations;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200147D RID: 5245
	[TaskIcon("Assets/Behavior Designer/Icon/RunOperation.png")]
	public sealed class RunOperations : Action
	{
		// Token: 0x06006642 RID: 26178 RVA: 0x00127BB9 File Offset: 0x00125DB9
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
			this._operationsValue = this._operations.Value;
			this._operationsValue.Initialize();
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x00127BE8 File Offset: 0x00125DE8
		public override void OnStart()
		{
			if (!this._operationsValue.gameObject.activeSelf)
			{
				this._operationsValue.gameObject.SetActive(true);
			}
			this._operationsValue.Run(this._ownerValue);
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x00127C1E File Offset: 0x00125E1E
		public override TaskStatus OnUpdate()
		{
			if (this._operationsValue.running)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0400524B RID: 21067
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400524C RID: 21068
		[SerializeField]
		private SharedOperations _operations;

		// Token: 0x0400524D RID: 21069
		private Character _ownerValue;

		// Token: 0x0400524E RID: 21070
		private OperationInfos _operationsValue;
	}
}
