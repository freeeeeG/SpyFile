using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E5 RID: 229
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x06000448 RID: 1096 RVA: 0x0001F217 File Offset: 0x0001D617
		public virtual void Prepare(Material material)
		{
		}
	}
}
