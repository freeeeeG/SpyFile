using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A9C RID: 2716
[RequireComponent(typeof(Image))]
public class ImagePulser : MonoBehaviour
{
	// Token: 0x060035B3 RID: 13747 RVA: 0x000FB1EC File Offset: 0x000F95EC
	public void SetPulseCallback(CallbackVoid _callback)
	{
		this.m_pulseCallback = _callback;
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x000FB1F5 File Offset: 0x000F95F5
	public void SetPulseSpeedMultiplier(float _multiplier)
	{
		this.m_speedMultiplier = _multiplier;
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x000FB1FE File Offset: 0x000F95FE
	private void Awake()
	{
		this.m_image = base.gameObject.GetComponent<Image>();
		this.SetAlpha(0f);
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x000FB21C File Offset: 0x000F961C
	private void Update()
	{
		float pulseTimer = this.m_pulseTimer;
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		this.m_pulseTimer += this.m_speedMultiplier * 2f * 3.1415927f * deltaTime / this.m_pulseTimePeriod;
		if (pulseTimer < 3.1415927f && this.m_pulseTimer >= 3.1415927f)
		{
			this.m_pulseCallback();
		}
		this.m_pulseTimer %= 6.2831855f;
		this.SetAlpha(0.5f * (1f - Mathf.Cos(this.m_pulseTimer)));
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x000FB2BC File Offset: 0x000F96BC
	private void SetAlpha(float _a)
	{
		Color color = this.m_image.color;
		color.a = _a;
		this.m_image.color = color;
	}

	// Token: 0x04002B39 RID: 11065
	[SerializeField]
	private float m_pulseTimePeriod = 1f;

	// Token: 0x04002B3A RID: 11066
	private CallbackVoid m_pulseCallback = delegate()
	{
	};

	// Token: 0x04002B3B RID: 11067
	private float m_speedMultiplier = 1f;

	// Token: 0x04002B3C RID: 11068
	private float m_pulseTimer;

	// Token: 0x04002B3D RID: 11069
	private Image m_image;
}
