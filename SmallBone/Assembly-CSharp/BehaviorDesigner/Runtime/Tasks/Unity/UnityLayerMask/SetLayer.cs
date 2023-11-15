using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask
{
	// Token: 0x020015C8 RID: 5576
	[TaskDescription("Sets the layer of a GameObject.")]
	[TaskCategory("Unity/LayerMask")]
	public class SetLayer : Action
	{
		// Token: 0x06006AED RID: 27373 RVA: 0x0013300B File Offset: 0x0013120B
		public override TaskStatus OnUpdate()
		{
			base.GetDefaultGameObject(this.targetGameObject.Value).layer = LayerMask.NameToLayer(this.layerName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006AEE RID: 27374 RVA: 0x00133034 File Offset: 0x00131234
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.layerName = "Default";
		}

		// Token: 0x040056C8 RID: 22216
		[Tooltip("The GameObject to set the layer of")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056C9 RID: 22217
		[Tooltip("The name of the layer to set")]
		public SharedString layerName = "Default";
	}
}
