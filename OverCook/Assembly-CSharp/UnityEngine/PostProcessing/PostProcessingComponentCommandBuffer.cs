using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E4 RID: 228
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000444 RID: 1092
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x06000445 RID: 1093
		public abstract string GetName();

		// Token: 0x06000446 RID: 1094
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
