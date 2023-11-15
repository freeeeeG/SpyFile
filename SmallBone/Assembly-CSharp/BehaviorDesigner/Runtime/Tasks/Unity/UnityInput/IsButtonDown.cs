using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D0 RID: 5584
	[TaskCategory("Unity/Input")]
	[TaskDescription("Returns success when the specified button is pressed.")]
	public class IsButtonDown : Conditional
	{
		// Token: 0x06006B05 RID: 27397 RVA: 0x00133243 File Offset: 0x00131443
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetButtonDown(this.buttonName.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B06 RID: 27398 RVA: 0x0013325A File Offset: 0x0013145A
		public override void OnReset()
		{
			this.buttonName = "Fire1";
		}

		// Token: 0x040056D8 RID: 22232
		[Tooltip("The name of the button")]
		public SharedString buttonName;
	}
}
