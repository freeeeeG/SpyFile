using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class OnScreenDebugDisplay : MonoBehaviour
{
	// Token: 0x06000767 RID: 1895 RVA: 0x0002F220 File Offset: 0x0002D620
	public void AddDisplay(DebugDisplay display)
	{
		if (display != null)
		{
			display.OnSetUp();
			this.m_Displays.Add(display);
		}
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0002F23A File Offset: 0x0002D63A
	public void RemoveDisplay(DebugDisplay display)
	{
		if (display != null)
		{
			this.m_Displays.Remove(display);
		}
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0002F250 File Offset: 0x0002D650
	private void Awake()
	{
		this.m_Displays = new List<DebugDisplay>();
		this.m_GUIStyle = new GUIStyle();
		this.m_GUIStyle.alignment = TextAnchor.UpperRight;
		this.m_GUIStyle.fontSize = (int)((float)Screen.height * 0.03f);
		this.m_GUIStyle.normal.textColor = new Color(1f, 1f, 1f, 1f);
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0002F2C0 File Offset: 0x0002D6C0
	private void Update()
	{
		if (!DebugManager.Instance.GetOption("On Screen Debug Text"))
		{
			return;
		}
		for (int i = 0; i < this.m_Displays.Count; i++)
		{
			this.m_Displays[i].OnUpdate();
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0002F310 File Offset: 0x0002D710
	private void OnGUI()
	{
		if (!DebugManager.Instance.GetOption("On Screen Debug Text"))
		{
			return;
		}
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)this.m_GUIStyle.fontSize);
		for (int i = 0; i < this.m_Displays.Count; i++)
		{
			this.m_Displays[i].OnDraw(ref rect, this.m_GUIStyle);
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0002F38C File Offset: 0x0002D78C
	private void OnDestroy()
	{
		for (int i = 0; i < this.m_Displays.Count; i++)
		{
			if (this.m_Displays[i] != null)
			{
				this.m_Displays[i].OnDestroy();
			}
		}
	}

	// Token: 0x04000608 RID: 1544
	private List<DebugDisplay> m_Displays;

	// Token: 0x04000609 RID: 1545
	private GUIStyle m_GUIStyle;
}
