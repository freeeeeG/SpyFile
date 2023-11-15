using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class ExceptionNotificationHandler : IExceptionDisplayer
{
	// Token: 0x06001E6E RID: 7790 RVA: 0x000931D8 File Offset: 0x000915D8
	public void Initialize()
	{
		float num = (float)Screen.width / 2.5f;
		this.m_rect = new Rect((float)Screen.width - num, 0f, num, 0f);
		this.m_style = new GUIStyle();
		this.m_style.alignment = TextAnchor.LowerRight;
		this.m_style.fontSize = (int)((float)Screen.height * 0.025f);
		this.m_style.normal.textColor = new Color(1f, 0f, 0f, 1f);
		this.m_style.wordWrap = true;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x00093274 File Offset: 0x00091674
	public void OnGUI()
	{
		if (!string.IsNullOrEmpty(this.m_displayString))
		{
			GUI.Box(this.m_rect, string.Empty);
			GUI.Label(this.m_rect, this.m_displayString, this.m_style);
		}
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x000932B0 File Offset: 0x000916B0
	public void Display(string exceptionString, string stackTrace, bool bJustOccured)
	{
		this.m_displayString = exceptionString + "\n" + stackTrace;
		float num = this.m_style.CalcHeight(new GUIContent(this.m_displayString), this.m_rect.width);
		this.m_rect.height = num;
		this.m_rect.y = (float)Screen.height - num;
	}

	// Token: 0x04001764 RID: 5988
	private Rect m_rect;

	// Token: 0x04001765 RID: 5989
	private GUIStyle m_style;

	// Token: 0x04001766 RID: 5990
	private string m_displayString = string.Empty;
}
