using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class LeanTester : MonoBehaviour
{
	// Token: 0x0600023E RID: 574 RVA: 0x0000EC8C File Offset: 0x0000CE8C
	public void Start()
	{
		base.StartCoroutine(this.timeoutCheck());
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000EC9B File Offset: 0x0000CE9B
	private IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		if (!LeanTest.testsFinished)
		{
			Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
		yield break;
	}

	// Token: 0x04000100 RID: 256
	public float timeout = 15f;
}
