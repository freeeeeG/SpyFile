using System;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public class ProgressUI : MonoBehaviour
{
	// Token: 0x060019C7 RID: 6599 RVA: 0x000811F3 File Offset: 0x0007F5F3
	public void SetProgress(float _value)
	{
		this.m_hideAfterTime = 1f;
		this.m_guiBar.enabled = true;
		this.m_guiBar.SetValue(_value);
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x00081218 File Offset: 0x0007F618
	private void Awake()
	{
		this.m_guiBar = base.gameObject.AddComponent<GUIBar>();
		GUIBarWidgetConfig guibarWidgetConfig = new GUIBarWidgetConfig();
		guibarWidgetConfig.m_width = 40f;
		guibarWidgetConfig.m_height = 10f;
		guibarWidgetConfig.m_border = 1f;
		guibarWidgetConfig.m_offset = new Vector2(0f, 25f);
		guibarWidgetConfig.m_fillColor = new Color(0.11764706f, 0.99215686f, 0f);
		guibarWidgetConfig.m_emptyColor = Color.black;
		guibarWidgetConfig.m_borderColor = new Color(0.15686275f, 0.42745098f, 0f);
		this.m_guiBar.SetGUIBarWidgetConfig(guibarWidgetConfig);
		this.m_guiBar.enabled = false;
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x000812CC File Offset: 0x0007F6CC
	private void Update()
	{
		float hideAfterTime = this.m_hideAfterTime;
		this.m_hideAfterTime -= TimeManager.GetDeltaTime(base.gameObject);
		if (hideAfterTime >= 0f && this.m_hideAfterTime < 0f)
		{
			this.m_guiBar.enabled = false;
		}
	}

	// Token: 0x04001477 RID: 5239
	private GUIBar m_guiBar;

	// Token: 0x04001478 RID: 5240
	private float m_hideAfterTime;
}
