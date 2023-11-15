using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001B5 RID: 437
public class SceneNameDisplay : DebugDisplay
{
	// Token: 0x0600076E RID: 1902 RVA: 0x0002F3EC File Offset: 0x0002D7EC
	public override void OnSetUp()
	{
		this.m_displayString = "Scene: " + SceneManager.GetActiveScene().name;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0002F416 File Offset: 0x0002D816
	public override void OnUpdate()
	{
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0002F418 File Offset: 0x0002D818
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		base.DrawText(ref rect, style, this.m_displayString);
	}

	// Token: 0x0400060A RID: 1546
	public string m_displayString = string.Empty;
}
