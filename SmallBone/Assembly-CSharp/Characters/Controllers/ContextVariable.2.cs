using System;

namespace Characters.Controllers
{
	// Token: 0x02000918 RID: 2328
	public struct ContextVariable<TKey, TVal>
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x0009430A File Offset: 0x0009250A
		// (set) Token: 0x060031DB RID: 12763 RVA: 0x0009431D File Offset: 0x0009251D
		public TVal value
		{
			get
			{
				return this.context.Get<TVal>(this.key);
			}
			set
			{
				this.context.Set<TVal>(this.key, value);
			}
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x00094331 File Offset: 0x00092531
		public ContextVariable(Context<TKey> context, TKey key)
		{
			this.context = context;
			this.key = key;
		}

		// Token: 0x040028D3 RID: 10451
		public readonly Context<TKey> context;

		// Token: 0x040028D4 RID: 10452
		public readonly TKey key;
	}
}
