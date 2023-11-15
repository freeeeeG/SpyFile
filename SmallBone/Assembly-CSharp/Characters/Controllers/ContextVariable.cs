using System;

namespace Characters.Controllers
{
	// Token: 0x02000917 RID: 2327
	public struct ContextVariable<T>
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000942D3 File Offset: 0x000924D3
		// (set) Token: 0x060031D8 RID: 12760 RVA: 0x000942E6 File Offset: 0x000924E6
		public T value
		{
			get
			{
				return this.context.Get<T>(this.name);
			}
			set
			{
				this.context.Set<T>(this.name, value);
			}
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x000942FA File Offset: 0x000924FA
		public ContextVariable(Context context, string name)
		{
			this.context = context;
			this.name = name;
		}

		// Token: 0x040028D1 RID: 10449
		public readonly Context context;

		// Token: 0x040028D2 RID: 10450
		public readonly string name;
	}
}
