using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject
{
	// Token: 0x020015DF RID: 5599
	[TaskDescription("Returns the component of Type type if the game object has one attached, null if it doesn't. Returns Success.")]
	[TaskCategory("Unity/GameObject")]
	public class GetComponent : Action
	{
		// Token: 0x06006B32 RID: 27442 RVA: 0x0013358B File Offset: 0x0013178B
		public override TaskStatus OnUpdate()
		{
			this.storeValue.Value = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(this.type.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x001335BA File Offset: 0x001317BA
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.type.Value = "";
			this.storeValue.Value = null;
		}

		// Token: 0x040056EE RID: 22254
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056EF RID: 22255
		[Tooltip("The type of component")]
		public SharedString type;

		// Token: 0x040056F0 RID: 22256
		[RequiredField]
		[Tooltip("The component")]
		public SharedObject storeValue;
	}
}
