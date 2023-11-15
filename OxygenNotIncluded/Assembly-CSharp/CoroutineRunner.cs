using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x06001294 RID: 4756 RVA: 0x00063B68 File Offset: 0x00061D68
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			this.StartCoroutine(this.RunRoutine(routine, resolve));
		});
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00063B90 File Offset: 0x00061D90
	public ValueTuple<Promise, System.Action> RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = base.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
		System.Action item = delegate()
		{
			this.StopCoroutine(coroutine);
		};
		return new ValueTuple<Promise, System.Action>(promise, item);
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00063BE1 File Offset: 0x00061DE1
	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
		yield break;
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x00063BF7 File Offset: 0x00061DF7
	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00063C08 File Offset: 0x00061E08
	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = CoroutineRunner.Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
