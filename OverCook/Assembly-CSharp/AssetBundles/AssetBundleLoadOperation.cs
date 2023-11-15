using System;
using System.Collections;

namespace AssetBundles
{
	// Token: 0x02000285 RID: 645
	public abstract class AssetBundleLoadOperation : IEnumerator
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0003DEDE File Offset: 0x0003C2DE
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0003DEE4 File Offset: 0x0003C2E4
		public bool MoveNext()
		{
			bool flag = this.IsDone();
			if (flag && this.OperationComplete != null)
			{
				this.OperationComplete();
				this.OperationComplete = null;
			}
			return !flag;
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0003DF1F File Offset: 0x0003C31F
		public void Reset()
		{
		}

		// Token: 0x06000BE3 RID: 3043
		public abstract bool Update();

		// Token: 0x06000BE4 RID: 3044
		public abstract bool IsDone();

		// Token: 0x0400090C RID: 2316
		public AssetBundleLoadOperation.OperationCompleteDelegate OperationComplete;

		// Token: 0x02000286 RID: 646
		// (Invoke) Token: 0x06000BE6 RID: 3046
		public delegate void OperationCompleteDelegate();
	}
}
