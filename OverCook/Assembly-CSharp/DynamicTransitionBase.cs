using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public abstract class DynamicTransitionBase : MonoBehaviour
{
	// Token: 0x06001F65 RID: 8037
	public abstract void Setup(CallbackVoid _endTransitionCallback);

	// Token: 0x06001F66 RID: 8038
	public abstract IEnumerator Run();
}
