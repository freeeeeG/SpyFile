using System;
using UnityEngine;

// Token: 0x020009B4 RID: 2484
[Serializable]
public class ScrollingListWidgetConfig
{
	// Token: 0x04002711 RID: 10001
	public string[] m_names = new string[0];

	// Token: 0x04002712 RID: 10002
	public GUIRect m_displayArea = new GUIRect(new Rect(0.2f, 0.3f, 0.6f, 0.6f), GUIAnchor.TopLeft, GUICoordSystem.Normalised);

	// Token: 0x04002713 RID: 10003
	public float m_textSize = 0.1f;

	// Token: 0x04002714 RID: 10004
	public Font m_font;

	// Token: 0x04002715 RID: 10005
	public TextAnchor m_textAnchor = TextAnchor.MiddleRight;

	// Token: 0x04002716 RID: 10006
	public Color m_selectedText = Color.black;

	// Token: 0x04002717 RID: 10007
	public Color m_regularText = Color.black;
}
