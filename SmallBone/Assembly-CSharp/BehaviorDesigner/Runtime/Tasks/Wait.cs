using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x0200148C RID: 5260
	[TaskIcon("{SkinColor}WaitIcon.png")]
	[TaskDescription("Wait a specified amount of time. The task will return running until the task is done waiting. It will return success after the wait time has elapsed.")]
	public class Wait : Action
	{
		// Token: 0x06006679 RID: 26233 RVA: 0x0012882C File Offset: 0x00126A2C
		public override void OnStart()
		{
			if (this._chronometerReference != null && this._chronometerReference.Value != null)
			{
				this._choronometer = this._chronometerReference.Value.chronometer.master;
			}
			if (this.randomWait.Value)
			{
				this.waitDuration = UnityEngine.Random.Range(this.randomWaitMin.Value, this.randomWaitMax.Value);
			}
			else
			{
				this.waitDuration = this.waitTime.Value;
			}
			this._remainTime = this.waitDuration;
		}

		// Token: 0x0600667A RID: 26234 RVA: 0x001288BC File Offset: 0x00126ABC
		public override TaskStatus OnUpdate()
		{
			this._remainTime -= this._choronometer.deltaTime;
			if (this._remainTime <= 0f)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		// Token: 0x0600667B RID: 26235 RVA: 0x001288E6 File Offset: 0x00126AE6
		public override void OnReset()
		{
			this.waitTime = 1f;
			this.randomWait = false;
			this.randomWaitMin = 1f;
			this.randomWaitMax = 1f;
		}

		// Token: 0x04005285 RID: 21125
		[Tooltip("The amount of time to wait")]
		public SharedFloat waitTime = 1f;

		// Token: 0x04005286 RID: 21126
		[Tooltip("Should the wait be randomized?")]
		public SharedBool randomWait = false;

		// Token: 0x04005287 RID: 21127
		[Tooltip("The minimum wait time if random wait is enabled")]
		public SharedFloat randomWaitMin = 1f;

		// Token: 0x04005288 RID: 21128
		[Tooltip("The maximum wait time if random wait is enabled")]
		public SharedFloat randomWaitMax = 1f;

		// Token: 0x04005289 RID: 21129
		[SerializeField]
		private SharedCharacter _chronometerReference;

		// Token: 0x0400528A RID: 21130
		private float waitDuration;

		// Token: 0x0400528B RID: 21131
		private float startTime;

		// Token: 0x0400528C RID: 21132
		private float pauseTime;

		// Token: 0x0400528D RID: 21133
		private float _remainTime;

		// Token: 0x0400528E RID: 21134
		private Chronometer _choronometer;
	}
}
