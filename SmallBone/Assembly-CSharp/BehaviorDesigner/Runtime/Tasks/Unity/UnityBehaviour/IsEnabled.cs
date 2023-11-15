using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
	// Token: 0x020015F2 RID: 5618
	[TaskDescription("Returns Success if the object is enabled, otherwise Failure.")]
	[TaskCategory("Unity/Behaviour")]
	public class IsEnabled : Conditional
	{
		// Token: 0x06006B73 RID: 27507 RVA: 0x00133EB8 File Offset: 0x001320B8
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedObject == null && !(this.specifiedObject.Value is Behaviour))
			{
				Debug.LogWarning("SpecifiedObject is null or not a subclass of UnityEngine.Behaviour");
				return TaskStatus.Failure;
			}
			if (!(this.specifiedObject.Value as Behaviour).enabled)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x00133F05 File Offset: 0x00132105
		public override void OnReset()
		{
			if (this.specifiedObject != null)
			{
				this.specifiedObject.Value = null;
			}
		}

		// Token: 0x0400572B RID: 22315
		[Tooltip("The Object to use")]
		public SharedObject specifiedObject;
	}
}
