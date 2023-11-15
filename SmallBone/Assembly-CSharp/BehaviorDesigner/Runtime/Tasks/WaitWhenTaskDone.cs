using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148D RID: 5261
	public sealed class WaitWhenTaskDone : Action
	{
		// Token: 0x0600667D RID: 26237 RVA: 0x00128974 File Offset: 0x00126B74
		public override void OnStart()
		{
			if (this._chronometerReference != null && this._chronometerReference.Value != null)
			{
				this._choronometer = this._chronometerReference.Value.chronometer.master;
			}
			if (this.randomWait.Value)
			{
				this._waitDuration = UnityEngine.Random.Range(this.randomWaitMin.Value, this.randomWaitMax.Value);
			}
			else
			{
				this._waitDuration = this.waitTime.Value;
			}
			this._remainTime = this._waitDuration;
		}

		// Token: 0x0600667E RID: 26238 RVA: 0x00128A04 File Offset: 0x00126C04
		public override TaskStatus OnUpdate()
		{
			bool flag = true;
			Task[] array = this.tasks;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].NodeData.ExecutionStatus == TaskStatus.Running)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this._remainTime -= this._choronometer.deltaTime;
			}
			if (this._remainTime <= 0f)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		// Token: 0x0600667F RID: 26239 RVA: 0x00128A67 File Offset: 0x00126C67
		public override void OnReset()
		{
			this.waitTime = 1f;
			this.randomWait = false;
			this.randomWaitMin = 1f;
			this.randomWaitMax = 1f;
		}

		// Token: 0x0400528F RID: 21135
		public Task[] tasks;

		// Token: 0x04005290 RID: 21136
		[Tooltip("The amount of time to wait")]
		public SharedFloat waitTime = 1f;

		// Token: 0x04005291 RID: 21137
		[Tooltip("Should the wait be randomized?")]
		public SharedBool randomWait = false;

		// Token: 0x04005292 RID: 21138
		[Tooltip("The minimum wait time if random wait is enabled")]
		public SharedFloat randomWaitMin = 1f;

		// Token: 0x04005293 RID: 21139
		[Tooltip("The maximum wait time if random wait is enabled")]
		public SharedFloat randomWaitMax = 1f;

		// Token: 0x04005294 RID: 21140
		[SerializeField]
		private SharedCharacter _chronometerReference;

		// Token: 0x04005295 RID: 21141
		private float _waitDuration;

		// Token: 0x04005296 RID: 21142
		private float _remainTime;

		// Token: 0x04005297 RID: 21143
		private Chronometer _choronometer;
	}
}
