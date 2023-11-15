using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F6 RID: 5366
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the square magnitude of the Vector2.")]
	public class GetSqrMagnitude : Action
	{
		// Token: 0x06006821 RID: 26657 RVA: 0x0012CE40 File Offset: 0x0012B040
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value.sqrMagnitude;
			return TaskStatus.Success;
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x0012CE6C File Offset: 0x0012B06C
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeResult = 0f;
		}

		// Token: 0x0400540E RID: 21518
		[Tooltip("The Vector2 to get the square magnitude of")]
		public SharedVector2 vector2Variable;

		// Token: 0x0400540F RID: 21519
		[RequiredField]
		[Tooltip("The square magnitude of the vector")]
		public SharedFloat storeResult;
	}
}
