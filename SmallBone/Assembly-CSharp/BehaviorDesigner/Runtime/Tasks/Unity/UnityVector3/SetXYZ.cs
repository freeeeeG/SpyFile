using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014F0 RID: 5360
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Sets the X, Y, and Z values of the Vector3.")]
	public class SetXYZ : Action
	{
		// Token: 0x0600680F RID: 26639 RVA: 0x0012CBB0 File Offset: 0x0012ADB0
		public override TaskStatus OnUpdate()
		{
			Vector3 value = this.vector3Variable.Value;
			if (!this.xValue.IsNone)
			{
				value.x = this.xValue.Value;
			}
			if (!this.yValue.IsNone)
			{
				value.y = this.yValue.Value;
			}
			if (!this.zValue.IsNone)
			{
				value.z = this.zValue.Value;
			}
			this.vector3Variable.Value = value;
			return TaskStatus.Success;
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x0012CC34 File Offset: 0x0012AE34
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.xValue = (this.yValue = (this.zValue = 0f));
		}

		// Token: 0x040053FD RID: 21501
		[Tooltip("The Vector3 to set the values of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053FE RID: 21502
		[Tooltip("The X value. Set to None to have the value ignored")]
		public SharedFloat xValue;

		// Token: 0x040053FF RID: 21503
		[Tooltip("The Y value. Set to None to have the value ignored")]
		public SharedFloat yValue;

		// Token: 0x04005400 RID: 21504
		[Tooltip("The Z value. Set to None to have the value ignored")]
		public SharedFloat zValue;
	}
}
