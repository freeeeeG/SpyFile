using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask
{
	// Token: 0x020015C7 RID: 5575
	[TaskDescription("Gets the layer of a GameObject.")]
	[TaskCategory("Unity/LayerMask")]
	public class GetLayer : Action
	{
		// Token: 0x06006AEA RID: 27370 RVA: 0x00132FBC File Offset: 0x001311BC
		public override TaskStatus OnUpdate()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			this.storeResult.Value = LayerMask.LayerToName(defaultGameObject.layer);
			return TaskStatus.Success;
		}

		// Token: 0x06006AEB RID: 27371 RVA: 0x00132FF2 File Offset: 0x001311F2
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeResult = "";
		}

		// Token: 0x040056C6 RID: 22214
		[Tooltip("The GameObject to set the layer of")]
		public SharedGameObject targetGameObject;

		// Token: 0x040056C7 RID: 22215
		[Tooltip("The name of the layer to get")]
		[RequiredField]
		public SharedString storeResult;
	}
}
