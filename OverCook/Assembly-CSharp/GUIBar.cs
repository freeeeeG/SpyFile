using System;
using UnityEngine;

// Token: 0x020009A0 RID: 2464
[AddComponentMenu("Scripts/Game/GUI/GUIBar")]
public class GUIBar : MonoBehaviour
{
	// Token: 0x06003042 RID: 12354 RVA: 0x000E3062 File Offset: 0x000E1462
	protected void Awake()
	{
		this.SetGUIBarWidgetConfig(this.m_guiBarConfig);
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x000E3070 File Offset: 0x000E1470
	public void SetGUIBarWidgetConfig(GUIBarWidgetConfig _guiBarConfig)
	{
		this.m_guiBarConfig = _guiBarConfig;
		if (!this.m_guiBarConfig.m_transform)
		{
			this.m_guiBarConfig.m_transform = base.transform;
		}
		this.m_guiBar = new GUIBarWidget(this.m_guiBarConfig);
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x000E30B0 File Offset: 0x000E14B0
	public virtual void SetValue(float _value)
	{
		this.m_value = _value;
		this.m_guiBar.SetValue(this.m_value);
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x000E30CA File Offset: 0x000E14CA
	public virtual float GetValue()
	{
		return this.m_value;
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000E30D2 File Offset: 0x000E14D2
	public void SetCroppedWidth(float _cropWidth)
	{
		this.m_guiBar.SetCroppedWidth(_cropWidth);
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x000E30E0 File Offset: 0x000E14E0
	private void OnGUI()
	{
		this.m_guiBar.SetValue(this.m_value);
		this.m_guiBar.Draw(null, 1f, 1f);
	}

	// Token: 0x040026C0 RID: 9920
	[SerializeField]
	[Range(0f, 1f)]
	private float m_value;

	// Token: 0x040026C1 RID: 9921
	public GUIBarWidgetConfig m_guiBarConfig = new GUIBarWidgetConfig();

	// Token: 0x040026C2 RID: 9922
	private GUIBarWidget m_guiBar;

	// Token: 0x040026C3 RID: 9923
	private GUIStyle m_blockColourStyle;

	// Token: 0x040026C4 RID: 9924
	private float m_croppedSize = 1f;
}
