using System;
using UnityEngine;

// Token: 0x020009B5 RID: 2485
public class ScrollingListWidget
{
	// Token: 0x060030AA RID: 12458 RVA: 0x000E49C1 File Offset: 0x000E2DC1
	public ScrollingListWidget(ScrollingListWidgetConfig _config)
	{
		this.m_config = _config;
		this.m_camera = Camera.main;
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000E49DB File Offset: 0x000E2DDB
	public void SetNames(string[] _names)
	{
		this.m_config.m_names = _names;
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x000E49EC File Offset: 0x000E2DEC
	public void MoveDown()
	{
		if (this.m_selected + 1 < this.m_config.m_names.Length)
		{
			this.m_selected++;
			if (this.m_selected >= this.m_anchor + (int)(this.m_config.m_displayArea.m_rect.height / this.m_config.m_textSize))
			{
				this.m_anchor++;
			}
		}
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000E4A63 File Offset: 0x000E2E63
	public void MoveUp()
	{
		if (this.m_selected > 0)
		{
			this.m_selected--;
			if (this.m_selected < this.m_anchor)
			{
				this.m_anchor--;
			}
		}
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x000E4A9E File Offset: 0x000E2E9E
	public int GetSelection()
	{
		return this.m_selected;
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000E4AA8 File Offset: 0x000E2EA8
	public void Draw(Vector3? _position)
	{
		GUIRect guirect = this.m_config.m_displayArea;
		if (_position != null)
		{
			guirect = GUIUtils.ConvertToScreenSpace(guirect, Camera.main, _position.Value);
		}
		Rect inPixels = guirect.GetInPixels(Camera.main);
		GUIRect guirect2 = new GUIRect(new Rect(0f, 0f, this.m_config.m_displayArea.m_rect.width, this.m_config.m_textSize), this.m_config.m_displayArea.m_anchor, this.m_config.m_displayArea.m_coordSystem);
		Rect inPixels2 = guirect2.GetInPixels(Camera.main);
		GUIStyle textStyle = GUIUtils.GetTextStyle(inPixels2, this.m_config.m_textAnchor, this.m_config.m_font, 10);
		int num = (int)(this.m_config.m_displayArea.m_rect.height / this.m_config.m_textSize);
		GUI.BeginGroup(inPixels);
		for (int i = 0; i < num; i++)
		{
			int num2 = this.m_anchor + i;
			if (num2 < this.m_config.m_names.Length)
			{
				Rect position = new Rect(0f, (float)i * inPixels2.height, inPixels.width, inPixels2.height);
				GUI.color = ((num2 != this.m_selected) ? this.m_config.m_regularText : this.m_config.m_selectedText);
				GUI.Label(position, this.m_config.m_names[num2], textStyle);
			}
		}
		GUI.EndGroup();
	}

	// Token: 0x04002718 RID: 10008
	private ScrollingListWidgetConfig m_config;

	// Token: 0x04002719 RID: 10009
	private Camera m_camera;

	// Token: 0x0400271A RID: 10010
	private int m_anchor;

	// Token: 0x0400271B RID: 10011
	private int m_selected;
}
