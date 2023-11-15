using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000385 RID: 901
public static class SequenceUtil
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600128E RID: 4750 RVA: 0x00063AAC File Offset: 0x00061CAC
	public static YieldInstruction WaitForNextFrame
	{
		get
		{
			return null;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600128F RID: 4751 RVA: 0x00063AAF File Offset: 0x00061CAF
	public static YieldInstruction WaitForEndOfFrame
	{
		get
		{
			if (SequenceUtil.waitForEndOfFrame == null)
			{
				SequenceUtil.waitForEndOfFrame = new WaitForEndOfFrame();
			}
			return SequenceUtil.waitForEndOfFrame;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06001290 RID: 4752 RVA: 0x00063AC7 File Offset: 0x00061CC7
	public static YieldInstruction WaitForFixedUpdate
	{
		get
		{
			if (SequenceUtil.waitForFixedUpdate == null)
			{
				SequenceUtil.waitForFixedUpdate = new WaitForFixedUpdate();
			}
			return SequenceUtil.waitForFixedUpdate;
		}
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00063AE0 File Offset: 0x00061CE0
	public static YieldInstruction WaitForSeconds(float duration)
	{
		WaitForSeconds result;
		if (!SequenceUtil.scaledTimeCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.scaledTimeCache[duration] = new WaitForSeconds(duration));
		}
		return result;
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00063B14 File Offset: 0x00061D14
	public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
	{
		WaitForSecondsRealtime result;
		if (!SequenceUtil.reailTimeWaitCache.TryGetValue(duration, out result))
		{
			result = (SequenceUtil.reailTimeWaitCache[duration] = new WaitForSecondsRealtime(duration));
		}
		return result;
	}

	// Token: 0x04000A0E RID: 2574
	private static WaitForEndOfFrame waitForEndOfFrame = null;

	// Token: 0x04000A0F RID: 2575
	private static WaitForFixedUpdate waitForFixedUpdate = null;

	// Token: 0x04000A10 RID: 2576
	private static Dictionary<float, WaitForSeconds> scaledTimeCache = new Dictionary<float, WaitForSeconds>();

	// Token: 0x04000A11 RID: 2577
	private static Dictionary<float, WaitForSecondsRealtime> reailTimeWaitCache = new Dictionary<float, WaitForSecondsRealtime>();
}
