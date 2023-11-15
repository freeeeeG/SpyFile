using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class VersionDisplay : DebugDisplay
{
	// Token: 0x06000772 RID: 1906 RVA: 0x0002F43B File Offset: 0x0002D83B
	public override void OnSetUp()
	{
		this.m_Text = "Build #" + BuildVersion.m_VersionString;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0002F452 File Offset: 0x0002D852
	public override void OnUpdate()
	{
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002F454 File Offset: 0x0002D854
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		base.DrawText(ref rect, style, this.m_Text);
	}

	// Token: 0x0400060B RID: 1547
	private string m_Text = string.Empty;
}
