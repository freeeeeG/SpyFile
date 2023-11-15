using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014AF RID: 5295
	[TaskDescription("Grab Board에 성공한 대상이 있는지 확인합니다.")]
	public sealed class Grabbed : Conditional
	{
		// Token: 0x0600671D RID: 26397 RVA: 0x0012A67D File Offset: 0x0012887D
		public override TaskStatus OnUpdate()
		{
			if (this._grabBoard == null)
			{
				return TaskStatus.Failure;
			}
			if (this._grabBoard.Value.targets.Count > 0)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Failure;
		}

		// Token: 0x04005321 RID: 21281
		[SerializeField]
		private SharedGrabBoard _grabBoard;
	}
}
