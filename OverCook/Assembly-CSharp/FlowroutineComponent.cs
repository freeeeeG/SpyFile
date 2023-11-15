using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000132 RID: 306
public abstract class FlowroutineComponent<SetupData> : MonoBehaviour, IFlowroutineBuilder<SetupData>
{
	// Token: 0x06000599 RID: 1433 RVA: 0x0002A9AA File Offset: 0x00028DAA
	public IFlowroutine BuildFlowroutine(SetupData _setupData)
	{
		this.Setup(_setupData);
		return new FlowroutineAdapter(this.Run(), new CallbackVoid(this.Shutdown));
	}

	// Token: 0x0600059A RID: 1434
	protected abstract void Setup(SetupData _setupData);

	// Token: 0x0600059B RID: 1435
	protected abstract IEnumerator Run();

	// Token: 0x0600059C RID: 1436
	protected abstract void Shutdown();
}
