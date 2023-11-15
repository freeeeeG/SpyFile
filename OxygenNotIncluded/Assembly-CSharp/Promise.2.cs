using System;
using System.Collections;

// Token: 0x02000391 RID: 913
public class Promise<T> : IEnumerator
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06001300 RID: 4864 RVA: 0x00064B9B File Offset: 0x00062D9B
	public bool IsResolved
	{
		get
		{
			return this.promise.IsResolved;
		}
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x00064BA8 File Offset: 0x00062DA8
	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			this.Resolve(value);
		});
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x00064BCD File Offset: 0x00062DCD
	public Promise()
	{
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00064BE0 File Offset: 0x00062DE0
	public void EnsureResolved(T value)
	{
		this.result = value;
		this.promise.EnsureResolved();
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x00064BF4 File Offset: 0x00062DF4
	public void Resolve(T value)
	{
		this.result = value;
		this.promise.Resolve();
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00064C08 File Offset: 0x00062E08
	public Promise<T> Then(Action<T> fn)
	{
		this.promise.Then(delegate
		{
			fn(this.result);
		});
		return this;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x00064C42 File Offset: 0x00062E42
	public Promise ThenWait(Func<Promise> fn)
	{
		return this.promise.ThenWait(fn);
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00064C50 File Offset: 0x00062E50
	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return this.promise.ThenWait<T>(fn);
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06001308 RID: 4872 RVA: 0x00064C5E File Offset: 0x00062E5E
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x00064C61 File Offset: 0x00062E61
	bool IEnumerator.MoveNext()
	{
		return !this.promise.IsResolved;
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00064C71 File Offset: 0x00062E71
	void IEnumerator.Reset()
	{
	}

	// Token: 0x04000A46 RID: 2630
	private Promise promise = new Promise();

	// Token: 0x04000A47 RID: 2631
	private T result;
}
