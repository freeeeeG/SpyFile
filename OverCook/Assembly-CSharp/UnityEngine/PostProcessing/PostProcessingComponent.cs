using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E3 RID: 227
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0001EEC0 File Offset: 0x0001D2C0
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0001EEC8 File Offset: 0x0001D2C8
		public T model { get; internal set; }

		// Token: 0x06000441 RID: 1089 RVA: 0x0001EED1 File Offset: 0x0001D2D1
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001EEE1 File Offset: 0x0001D2E1
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}
	}
}
