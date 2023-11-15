using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F1 RID: 5361
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Clamps the magnitude of the Vector2.")]
	public class ClampMagnitude : Action
	{
		// Token: 0x06006812 RID: 26642 RVA: 0x0012CC73 File Offset: 0x0012AE73
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.ClampMagnitude(this.vector2Variable.Value, this.maxLength.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x0012CC9C File Offset: 0x0012AE9C
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeResult = Vector2.zero;
			this.maxLength = 0f;
		}

		// Token: 0x04005401 RID: 21505
		[Tooltip("The Vector2 to clamp the magnitude of")]
		public SharedVector2 vector2Variable;

		// Token: 0x04005402 RID: 21506
		[Tooltip("The max length of the magnitude")]
		public SharedFloat maxLength;

		// Token: 0x04005403 RID: 21507
		[Tooltip("The clamp magnitude resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
