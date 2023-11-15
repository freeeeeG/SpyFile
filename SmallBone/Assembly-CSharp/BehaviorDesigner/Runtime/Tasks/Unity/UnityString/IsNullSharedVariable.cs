using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString
{
	// Token: 0x02001646 RID: 5702
	[TaskDescription("Global SharedVariable이 Null 인지 체크")]
	[TaskIcon("Assets/Behavior Designer/Icon/IsNullSharedVariable.png")]
	public class IsNullSharedVariable : Conditional
	{
		// Token: 0x06006CCB RID: 27851 RVA: 0x00137155 File Offset: 0x00135355
		public override TaskStatus OnUpdate()
		{
			if (this._variable.GetValue() != null)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005895 RID: 22677
		[SerializeField]
		private SharedVariable _variable;
	}
}
