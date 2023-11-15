using System;
using System.Collections;

// Token: 0x02000684 RID: 1668
public abstract class FlowroutineBase : IFlowroutineBuilder<FlowroutineData>
{
	// Token: 0x06002004 RID: 8196 RVA: 0x0009BAD5 File Offset: 0x00099ED5
	public virtual IFlowroutine BuildFlowroutine(FlowroutineData flowroutineData)
	{
		this.Setup(flowroutineData);
		return new FlowroutineAdapter(this.Run(), new CallbackVoid(this.Shutdown));
	}

	// Token: 0x06002005 RID: 8197
	protected abstract void Setup(FlowroutineData flowroutineData);

	// Token: 0x06002006 RID: 8198
	protected abstract IEnumerator Run();

	// Token: 0x06002007 RID: 8199
	protected abstract void Shutdown();
}
