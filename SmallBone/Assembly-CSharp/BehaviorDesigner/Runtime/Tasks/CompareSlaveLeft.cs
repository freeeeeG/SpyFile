using System;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014AD RID: 5293
	[TaskDescription("Character의 Master컴퍼넌트에 접근해 남은 Slave 숫자를 비교합니다.")]
	public class CompareSlaveLeft : Conditional
	{
		// Token: 0x0600671A RID: 26394 RVA: 0x0012A580 File Offset: 0x00128780
		public override TaskStatus OnUpdate()
		{
			if (this._owner == null)
			{
				return TaskStatus.Failure;
			}
			Master componentInChildren = this._owner.Value.GetComponentInChildren<Master>();
			if (componentInChildren == null)
			{
				return TaskStatus.Failure;
			}
			switch (this.operation)
			{
			case CompareSlaveLeft.Operation.LessThan:
				if (componentInChildren.GetSlavesLeft() >= this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case CompareSlaveLeft.Operation.LessThanOrEqualTo:
				if (componentInChildren.GetSlavesLeft() > this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case CompareSlaveLeft.Operation.EqualTo:
				if (componentInChildren.GetSlavesLeft() != this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case CompareSlaveLeft.Operation.NotEqualTo:
				if (componentInChildren.GetSlavesLeft() == this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case CompareSlaveLeft.Operation.GreaterThanOrEqualTo:
				if (componentInChildren.GetSlavesLeft() < this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case CompareSlaveLeft.Operation.GreaterThan:
				if (componentInChildren.GetSlavesLeft() <= this.integer.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x0012A668 File Offset: 0x00128868
		public override void OnReset()
		{
			this.operation = CompareSlaveLeft.Operation.LessThan;
			this.integer.Value = 0;
		}

		// Token: 0x04005316 RID: 21270
		[Tooltip("master 컴퍼넌트를 자식으로 가지는 Character입니다. Master 컴퍼넌트가 없을 경우 Failure를 반환합니다.")]
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005317 RID: 21271
		public CompareSlaveLeft.Operation operation;

		// Token: 0x04005318 RID: 21272
		[Tooltip("Slave 갯수와 비교되는 대상입니다.")]
		public SharedInt integer;

		// Token: 0x04005319 RID: 21273
		private Master _master;

		// Token: 0x020014AE RID: 5294
		public enum Operation
		{
			// Token: 0x0400531B RID: 21275
			LessThan,
			// Token: 0x0400531C RID: 21276
			LessThanOrEqualTo,
			// Token: 0x0400531D RID: 21277
			EqualTo,
			// Token: 0x0400531E RID: 21278
			NotEqualTo,
			// Token: 0x0400531F RID: 21279
			GreaterThanOrEqualTo,
			// Token: 0x04005320 RID: 21280
			GreaterThan
		}
	}
}
