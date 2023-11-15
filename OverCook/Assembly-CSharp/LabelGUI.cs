using System;
using UnityEngine;

// Token: 0x020009AC RID: 2476
public class LabelGUI : MonoBehaviour
{
	// Token: 0x06003076 RID: 12406 RVA: 0x000E3F16 File Offset: 0x000E2316
	public void SetText(string _text)
	{
		this.m_message = _text;
	}

	// Token: 0x06003077 RID: 12407 RVA: 0x000E3F1F File Offset: 0x000E231F
	public string GetText()
	{
		return this.m_message;
	}

	// Token: 0x06003078 RID: 12408 RVA: 0x000E3F27 File Offset: 0x000E2327
	public GUIRect GetGUIRect()
	{
		return this.m_rect;
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x000E3F2F File Offset: 0x000E232F
	public void SetGUIRect(GUIRect _rect)
	{
		this.m_rect = _rect;
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000E3F38 File Offset: 0x000E2338
	private void OnGUI()
	{
		Rect rect;
		if (this.m_screenSpace)
		{
			rect = GUIUtils.ToPixels(this.m_rect, Camera.main);
		}
		else
		{
			rect = GUIUtils.ToPixels(this.m_rect, Camera.main, base.transform.position);
		}
		GUIStyle textStyle = GUIUtils.GetTextStyle(rect, TextAnchor.MiddleCenter, this.m_font, this.m_message);
		Color color = GUI.color;
		GUI.color = this.m_textColour;
		GUI.Label(rect, this.m_message, textStyle);
		GUI.color = color;
	}

	// Token: 0x040026E3 RID: 9955
	[SerializeField]
	private GUIRect m_rect;

	// Token: 0x040026E4 RID: 9956
	[SerializeField]
	private Font m_font;

	// Token: 0x040026E5 RID: 9957
	[SerializeField]
	private string m_message;

	// Token: 0x040026E6 RID: 9958
	[SerializeField]
	private Color m_textColour = Color.white;

	// Token: 0x040026E7 RID: 9959
	[SerializeField]
	private bool m_screenSpace = true;
}
