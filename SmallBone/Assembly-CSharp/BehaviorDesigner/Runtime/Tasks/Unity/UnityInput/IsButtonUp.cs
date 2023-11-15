using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput
{
	// Token: 0x020015D1 RID: 5585
	[TaskCategory("Unity/Input")]
	[TaskDescription("Returns success when the specified button is released.")]
	public class IsButtonUp : Conditional
	{
		// Token: 0x06006B08 RID: 27400 RVA: 0x0013326C File Offset: 0x0013146C
		public override TaskStatus OnUpdate()
		{
			if (!Input.GetButtonUp(this.buttonName.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006B09 RID: 27401 RVA: 0x00133283 File Offset: 0x00131483
		public override void OnReset()
		{
			this.buttonName = "Fire1";
		}

		// Token: 0x040056D9 RID: 22233
		[Tooltip("The name of the button")]
		public SharedString buttonName;
	}
}
