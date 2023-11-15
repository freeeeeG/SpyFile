using System;
using System.Collections;

// Token: 0x02000390 RID: 912
public class Promise : IEnumerator
{
	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060012EF RID: 4847 RVA: 0x0006499C File Offset: 0x00062B9C
	public bool IsResolved
	{
		get
		{
			return this.m_is_resolved;
		}
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x000649A4 File Offset: 0x00062BA4
	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			this.Resolve();
		});
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x000649BE File Offset: 0x00062BBE
	public Promise()
	{
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x000649C6 File Offset: 0x00062BC6
	public void EnsureResolved()
	{
		if (this.IsResolved)
		{
			return;
		}
		this.Resolve();
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x000649D7 File Offset: 0x00062BD7
	public void Resolve()
	{
		DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
		this.m_is_resolved = true;
		if (this.on_complete != null)
		{
			this.on_complete();
			this.on_complete = null;
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x00064A0D File Offset: 0x00062C0D
	public Promise Then(System.Action callback)
	{
		if (this.m_is_resolved)
		{
			callback();
		}
		else
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, callback);
		}
		return this;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x00064A38 File Offset: 0x00062C38
	public Promise ThenWait(Func<Promise> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x00064A80 File Offset: 0x00062C80
	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060012F7 RID: 4855 RVA: 0x00064AC6 File Offset: 0x00062CC6
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x00064AC9 File Offset: 0x00062CC9
	bool IEnumerator.MoveNext()
	{
		return !this.IsResolved;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x00064AD4 File Offset: 0x00062CD4
	void IEnumerator.Reset()
	{
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x00064AD6 File Offset: 0x00062CD6
	static Promise()
	{
		Promise.m_instant.Resolve();
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x060012FB RID: 4859 RVA: 0x00064AEC File Offset: 0x00062CEC
	public static Promise Instant
	{
		get
		{
			return Promise.m_instant;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x060012FC RID: 4860 RVA: 0x00064AF3 File Offset: 0x00062CF3
	public static Promise Fail
	{
		get
		{
			return new Promise();
		}
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x00064AFC File Offset: 0x00062CFC
	public static Promise All(params Promise[] promises)
	{
		Promise.<>c__DisplayClass21_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass21_0();
		CS$<>8__locals1.promises = promises;
		if (CS$<>8__locals1.promises == null || CS$<>8__locals1.promises.Length == 0)
		{
			return Promise.Instant;
		}
		CS$<>8__locals1.all_resolved_promise = new Promise();
		Promise[] promises2 = CS$<>8__locals1.promises;
		for (int i = 0; i < promises2.Length; i++)
		{
			promises2[i].Then(new System.Action(CS$<>8__locals1.<All>g__TryResolve|0));
		}
		return CS$<>8__locals1.all_resolved_promise;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x00064B68 File Offset: 0x00062D68
	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise.<>c__DisplayClass22_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass22_0();
		CS$<>8__locals1.make_promise_fns = make_promise_fns;
		CS$<>8__locals1.all_resolve_promise = new Promise();
		CS$<>8__locals1.current_promise_fn_index = 0;
		CS$<>8__locals1.<Chain>g__TryNext|0();
		return CS$<>8__locals1.all_resolve_promise;
	}

	// Token: 0x04000A43 RID: 2627
	private System.Action on_complete;

	// Token: 0x04000A44 RID: 2628
	private bool m_is_resolved;

	// Token: 0x04000A45 RID: 2629
	private static Promise m_instant = new Promise();
}
