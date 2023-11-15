using System;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class FPSCounter : DebugDisplay
{
	// Token: 0x06000758 RID: 1880 RVA: 0x0002F173 File Offset: 0x0002D573
	public override void OnSetUp()
	{
		this.m_FPSCounter = new FPS_No_String_Allocs();
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0002F180 File Offset: 0x0002D580
	public override void OnUpdate()
	{
		this.m_FPSCounter.Update();
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0002F18D File Offset: 0x0002D58D
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		base.DrawText(ref rect, style, this.m_FPSCounter.GetString());
	}

	// Token: 0x04000605 RID: 1541
	private FPS_No_String_Allocs m_FPSCounter;
}
