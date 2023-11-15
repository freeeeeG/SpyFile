using System;
using System.Collections;

// Token: 0x020001A6 RID: 422
public static class CoroutineUtils
{
	// Token: 0x06000720 RID: 1824 RVA: 0x0002E684 File Offset: 0x0002CA84
	public static IEnumerator ParallelRoutine(IEnumerator[] routines)
	{
		for (;;)
		{
			for (int i = 0; i < routines.Length; i++)
			{
				if (routines[i] != null && !routines[i].MoveNext())
				{
					routines[i] = null;
				}
			}
			if (!routines.Contains((IEnumerator x) => x != null))
			{
				break;
			}
			yield return true;
		}
		yield break;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002E6A0 File Offset: 0x0002CAA0
	public static IEnumerator TimerRoutine(float _seconds, int _layer)
	{
		for (float t = 0f; t < _seconds; t += TimeManager.GetDeltaTime(_layer))
		{
			yield return null;
		}
		yield break;
	}
}
