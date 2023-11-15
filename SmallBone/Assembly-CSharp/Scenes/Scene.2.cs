using System;

namespace Scenes
{
	// Token: 0x02000147 RID: 327
	public abstract class Scene<T> : Scene where T : Scene<T>
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001312D File Offset: 0x0001132D
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00013134 File Offset: 0x00011334
		public static T instance { get; private set; }

		// Token: 0x06000689 RID: 1673 RVA: 0x0001313C File Offset: 0x0001133C
		public Scene()
		{
			Scene<T>.instance = (this as T);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00013154 File Offset: 0x00011354
		protected virtual void OnDestroy()
		{
			Scene<T>.instance = default(T);
		}
	}
}
