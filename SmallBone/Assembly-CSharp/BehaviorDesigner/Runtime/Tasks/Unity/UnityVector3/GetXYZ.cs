using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E7 RID: 5351
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Stores the X, Y, and Z values of the Vector3.")]
	public class GetXYZ : Action
	{
		// Token: 0x060067F7 RID: 26615 RVA: 0x0012C7A0 File Offset: 0x0012A9A0
		public override TaskStatus OnUpdate()
		{
			this.storeX.Value = this.vector3Variable.Value.x;
			this.storeY.Value = this.vector3Variable.Value.y;
			this.storeZ.Value = this.vector3Variable.Value.z;
			return TaskStatus.Success;
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x0012C800 File Offset: 0x0012AA00
		public override void OnReset()
		{
			this.vector3Variable = Vector3.zero;
			this.storeX = 0f;
			this.storeY = 0f;
			this.storeZ = 0f;
		}

		// Token: 0x040053DD RID: 21469
		[Tooltip("The Vector3 to get the values of")]
		public SharedVector3 vector3Variable;

		// Token: 0x040053DE RID: 21470
		[Tooltip("The X value")]
		[RequiredField]
		public SharedFloat storeX;

		// Token: 0x040053DF RID: 21471
		[Tooltip("The Y value")]
		[RequiredField]
		public SharedFloat storeY;

		// Token: 0x040053E0 RID: 21472
		[RequiredField]
		[Tooltip("The Z value")]
		public SharedFloat storeZ;
	}
}
