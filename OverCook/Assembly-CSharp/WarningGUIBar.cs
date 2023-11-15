using System;
using UnityEngine;

// Token: 0x020009B7 RID: 2487
[AddComponentMenu("Scripts/Game/GUI/WarningGUIBar")]
public class WarningGUIBar : GUIBar
{
	// Token: 0x060030B4 RID: 12468 RVA: 0x000E4D18 File Offset: 0x000E3118
	public void StartWarning()
	{
		if (!this.m_flashing)
		{
			this.m_flashTimer = 0f;
			this.m_flashing = true;
		}
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000E4D37 File Offset: 0x000E3137
	public void StopWarning()
	{
		this.m_flashing = false;
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x000E4D40 File Offset: 0x000E3140
	public override void SetValue(float _value)
	{
		if ((double)_value <= 1.0)
		{
			base.SetValue(_value);
		}
		else
		{
			base.SetValue(_value - 1f);
		}
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000E4D6B File Offset: 0x000E316B
	private new void Awake()
	{
		base.Awake();
		this.m_preFlashBorderColor = this.m_guiBarConfig.m_borderColor;
		this.m_preFlashFillColor = this.m_guiBarConfig.m_fillColor;
		this.m_preFlashEmptyColor = this.m_guiBarConfig.m_emptyColor;
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000E4DA8 File Offset: 0x000E31A8
	private void Update()
	{
		if (this.m_flashing)
		{
			this.m_guiBarConfig.m_borderColor = Color.Lerp(this.m_flashBorderColor, this.m_preFlashBorderColor, this.m_flashTimer / this.m_flashTime);
			this.m_guiBarConfig.m_emptyColor = Color.Lerp(this.m_flashFillColor, this.m_preFlashFillColor, this.m_flashTimer / this.m_flashTime);
			this.m_guiBarConfig.m_fillColor = this.m_burningFillColor;
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			this.m_flashTimer = (this.m_flashTimer + deltaTime) % this.m_flashTime;
		}
		else
		{
			this.m_flashTimer = 0f;
			this.m_guiBarConfig.m_borderColor = this.m_preFlashBorderColor;
			this.m_guiBarConfig.m_fillColor = this.m_preFlashFillColor;
			this.m_guiBarConfig.m_emptyColor = this.m_preFlashEmptyColor;
		}
	}

	// Token: 0x0400271F RID: 10015
	[SerializeField]
	private Color m_flashBorderColor = Color.red;

	// Token: 0x04002720 RID: 10016
	[SerializeField]
	private Color m_flashFillColor = Color.red;

	// Token: 0x04002721 RID: 10017
	[SerializeField]
	private Color m_burningFillColor = Color.red;

	// Token: 0x04002722 RID: 10018
	[SerializeField]
	private float m_flashTime = 1f;

	// Token: 0x04002723 RID: 10019
	private Color m_preFlashBorderColor = Color.black;

	// Token: 0x04002724 RID: 10020
	private Color m_preFlashFillColor = Color.black;

	// Token: 0x04002725 RID: 10021
	public Color m_preFlashEmptyColor = Color.black;

	// Token: 0x04002726 RID: 10022
	private float m_flashTimer;

	// Token: 0x04002727 RID: 10023
	private bool m_flashing;
}
