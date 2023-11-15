using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000680 RID: 1664
public abstract class IntroFlowroutineBase : MonoBehaviour
{
	// Token: 0x06001FF8 RID: 8184
	public abstract void Setup(CallbackVoid _startRoundCallback);

	// Token: 0x06001FF9 RID: 8185
	public abstract IEnumerator Run();
}
