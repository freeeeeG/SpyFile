using System;
using UnityEngine;

// Token: 0x02000B30 RID: 2864
public class HeatValueUIController : HoverIconUIController
{
	// Token: 0x06003A07 RID: 14855 RVA: 0x0011437B File Offset: 0x0011277B
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_autoHide && this.m_autoHideTime > 0f)
		{
			this.m_progressBar.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x001143B0 File Offset: 0x001127B0
	public override void LateUpdate()
	{
		base.LateUpdate();
		if (this.m_autoHide && this.m_hideTimer > 0f)
		{
			this.m_hideTimer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_hideTimer <= 0f)
			{
				this.m_progressBar.gameObject.SetActive(false);
			}
		}
		if (this.m_lerpTimer > 0f)
		{
			this.m_lerpTimer -= TimeManager.GetDeltaTime(base.gameObject);
			this.m_lerpTimer = Mathf.Max(this.m_lerpTimer, 0f);
			float value = Mathf.Lerp(this.m_progressBar.Value, this.m_progress, 1f - this.m_lerpTimer / this.m_progressLerpTime);
			this.m_progressBar.SetValue(value);
		}
	}

	// Token: 0x06003A09 RID: 14857 RVA: 0x0011448C File Offset: 0x0011288C
	public void SetProgress(float _value)
	{
		float progress = this.m_progress;
		if (progress != _value)
		{
			this.m_progress = _value;
			this.m_lerpTimer = this.m_progressLerpTime;
			if (this.m_autoHide && this.m_autoHideTime > 0f)
			{
				this.m_progressBar.gameObject.SetActive(true);
				this.m_hideTimer = this.m_autoHideTime;
			}
		}
	}

	// Token: 0x04002EFB RID: 12027
	[SerializeField]
	private ProgressGaugeUI m_progressBar;

	// Token: 0x04002EFC RID: 12028
	[SerializeField]
	private float m_progressLerpTime = 0.1f;

	// Token: 0x04002EFD RID: 12029
	[SerializeField]
	private bool m_autoHide = true;

	// Token: 0x04002EFE RID: 12030
	[SerializeField]
	[HideInInspectorTest("m_autoHide", true)]
	private float m_autoHideTime = 1f;

	// Token: 0x04002EFF RID: 12031
	private float m_progress;

	// Token: 0x04002F00 RID: 12032
	private float m_hideTimer;

	// Token: 0x04002F01 RID: 12033
	private float m_lerpTimer;
}
