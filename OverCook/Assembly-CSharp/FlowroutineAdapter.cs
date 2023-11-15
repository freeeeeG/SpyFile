using System;
using System.Collections;

// Token: 0x02000133 RID: 307
internal class FlowroutineAdapter : IFlowroutine, IEnumerator
{
	// Token: 0x0600059D RID: 1437 RVA: 0x0002A9CB File Offset: 0x00028DCB
	public FlowroutineAdapter(IEnumerator _coroutine, CallbackVoid _shutdown)
	{
		this.m_coroutine = _coroutine;
		this.m_shutdown = _shutdown;
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x0002A9E1 File Offset: 0x00028DE1
	public object Current
	{
		get
		{
			return this.m_coroutine.Current;
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0002A9EE File Offset: 0x00028DEE
	public bool MoveNext()
	{
		if (!this.m_coroutine.MoveNext())
		{
			this.Shutdown();
			return false;
		}
		return true;
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0002AA09 File Offset: 0x00028E09
	public void Reset()
	{
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0002AA0B File Offset: 0x00028E0B
	public void Shutdown()
	{
		if (this.m_shutdown != null)
		{
			this.m_shutdown();
			this.m_shutdown = null;
		}
	}

	// Token: 0x040004B3 RID: 1203
	private IEnumerator m_coroutine;

	// Token: 0x040004B4 RID: 1204
	private CallbackVoid m_shutdown;
}
