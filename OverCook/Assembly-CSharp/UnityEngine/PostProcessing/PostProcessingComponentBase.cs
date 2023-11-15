using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E2 RID: 226
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x0001EEB1 File Offset: 0x0001D2B1
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600043A RID: 1082
		public abstract bool active { get; }

		// Token: 0x0600043B RID: 1083 RVA: 0x0001EEB4 File Offset: 0x0001D2B4
		public virtual void OnEnable()
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001EEB6 File Offset: 0x0001D2B6
		public virtual void OnDisable()
		{
		}

		// Token: 0x0600043D RID: 1085
		public abstract PostProcessingModel GetModel();

		// Token: 0x040003D8 RID: 984
		public PostProcessingContext context;
	}
}
