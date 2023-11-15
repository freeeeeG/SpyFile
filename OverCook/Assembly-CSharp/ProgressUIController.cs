using System;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
public class ProgressUIController : HoverIconUIController
{
	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06003A80 RID: 14976 RVA: 0x00116A26 File Offset: 0x00114E26
	// (set) Token: 0x06003A81 RID: 14977 RVA: 0x00116A2E File Offset: 0x00114E2E
	public bool AutoHide
	{
		get
		{
			return this.m_autoHide;
		}
		set
		{
			this.m_autoHide = value;
		}
	}

	// Token: 0x06003A82 RID: 14978 RVA: 0x00116A37 File Offset: 0x00114E37
	protected override void Awake()
	{
		base.Awake();
		this.m_progressBar.gameObject.SetActive(false);
	}

	// Token: 0x06003A83 RID: 14979 RVA: 0x00116A50 File Offset: 0x00114E50
	public void SetProgress(float _value)
	{
		this.m_progressBar.SetValue(_value);
		this.m_progressBar.gameObject.SetActive(true);
		this.m_timer = this.m_hideAftertime;
	}

	// Token: 0x06003A84 RID: 14980 RVA: 0x00116A7C File Offset: 0x00114E7C
	public override void LateUpdate()
	{
		base.LateUpdate();
		float timer = this.m_timer;
		this.m_timer -= TimeManager.GetDeltaTime(base.gameObject);
		if (this.m_autoHide && timer >= 0f && this.m_timer < 0f)
		{
			this.m_progressBar.gameObject.SetActive(false);
		}
	}

	// Token: 0x04002F70 RID: 12144
	[SerializeField]
	private ProgressBarUI m_progressBar;

	// Token: 0x04002F71 RID: 12145
	[SerializeField]
	[HideInInspectorTest("m_autoHide", true)]
	private float m_hideAftertime = 1f;

	// Token: 0x04002F72 RID: 12146
	[SerializeField]
	private bool m_autoHide = true;

	// Token: 0x04002F73 RID: 12147
	private float m_timer;
}
