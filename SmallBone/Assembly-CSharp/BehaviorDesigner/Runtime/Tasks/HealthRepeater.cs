using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014CF RID: 5327
	[TaskIcon("{SkinColor}RepeaterIcon.png")]
	[TaskDescription("체력 조건을 만족할 경우 반복 실행")]
	public class HealthRepeater : Decorator
	{
		// Token: 0x0600679D RID: 26525 RVA: 0x0012BD2C File Offset: 0x00129F2C
		public override bool CanExecute()
		{
			return this.CheackHealthCondition() && (this.repeatForever.Value || this.executionCount < this.count.Value) && (!this.endOnFailure.Value || (this.endOnFailure.Value && this.executionStatus != TaskStatus.Failure));
		}

		// Token: 0x0600679E RID: 26526 RVA: 0x0012BD8D File Offset: 0x00129F8D
		public override void OnChildExecuted(TaskStatus childStatus)
		{
			this.executionCount++;
			this.executionStatus = childStatus;
		}

		// Token: 0x0600679F RID: 26527 RVA: 0x0012BDA4 File Offset: 0x00129FA4
		public override void OnEnd()
		{
			this.executionCount = 0;
			this.executionStatus = TaskStatus.Inactive;
		}

		// Token: 0x060067A0 RID: 26528 RVA: 0x0012BDB4 File Offset: 0x00129FB4
		public override void OnReset()
		{
			this.count = 0;
			this.endOnFailure = true;
		}

		// Token: 0x060067A1 RID: 26529 RVA: 0x0012BDD0 File Offset: 0x00129FD0
		private bool CheackHealthCondition()
		{
			float num = (this._healthType == HealthRepeater.HealthType.Constant) ? ((float)this._owner.Value.health.currentHealth) : ((float)this._owner.Value.health.percent * 100f);
			switch (this._operation)
			{
			case HealthRepeater.Operation.LessThan:
				return num < this._value.Value;
			case HealthRepeater.Operation.LessThanOrEqualTo:
				return num <= this._value.Value;
			case HealthRepeater.Operation.EqualTo:
				return Mathf.Approximately(num, this._value.Value);
			case HealthRepeater.Operation.NotEqualTo:
				return !Mathf.Approximately(num, this._value.Value);
			case HealthRepeater.Operation.GreaterThanOrEqualTo:
				return num >= this._value.Value;
			case HealthRepeater.Operation.GreaterThan:
				return num > this._value.Value;
			default:
				return false;
			}
		}

		// Token: 0x04005394 RID: 21396
		[SerializeField]
		private HealthRepeater.Operation _operation;

		// Token: 0x04005395 RID: 21397
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005396 RID: 21398
		[SerializeField]
		private SharedFloat _value;

		// Token: 0x04005397 RID: 21399
		[SerializeField]
		private HealthRepeater.HealthType _healthType;

		// Token: 0x04005398 RID: 21400
		[Tooltip("The number of times to repeat the execution of its child task")]
		public SharedInt count = 1;

		// Token: 0x04005399 RID: 21401
		[Tooltip("Allows the repeater to repeat forever")]
		public SharedBool repeatForever;

		// Token: 0x0400539A RID: 21402
		[Tooltip("Should the task return if the child task returns a failure")]
		public SharedBool endOnFailure;

		// Token: 0x0400539B RID: 21403
		private int executionCount;

		// Token: 0x0400539C RID: 21404
		private TaskStatus executionStatus;

		// Token: 0x020014D0 RID: 5328
		public enum Operation
		{
			// Token: 0x0400539E RID: 21406
			LessThan,
			// Token: 0x0400539F RID: 21407
			LessThanOrEqualTo,
			// Token: 0x040053A0 RID: 21408
			EqualTo,
			// Token: 0x040053A1 RID: 21409
			NotEqualTo,
			// Token: 0x040053A2 RID: 21410
			GreaterThanOrEqualTo,
			// Token: 0x040053A3 RID: 21411
			GreaterThan
		}

		// Token: 0x020014D1 RID: 5329
		public enum HealthType
		{
			// Token: 0x040053A5 RID: 21413
			Percent,
			// Token: 0x040053A6 RID: 21414
			Constant
		}
	}
}
