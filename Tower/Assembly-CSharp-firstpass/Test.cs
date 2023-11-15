using System;
using Highlighters;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class Test : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Start()
	{
		this.m_highlighter = base.GetComponent<Highlighter>();
		this.m_highlighter.GetRenderersInChildren();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
	private void Update()
	{
		this.m_timer += Time.deltaTime;
		if (this.m_timer > 0.25f)
		{
			this.m_timer = 0f;
			this.m_highlighter.enabled = !this.m_highlighter.enabled;
			return;
		}
		if (!this.m_highlighter.enabled)
		{
			return;
		}
		float num = Mathf.Sin(Time.realtimeSinceStartup);
		num = num * 0.5f + 0.5f;
		this.m_highlighter.Settings.UseOuterGlow = true;
		this.m_highlighter.Settings.BoxBlurSize = Mathf.Lerp(0.01f, 0.025f, num);
		this.m_highlighter.Settings.OuterGlowColorFront = Color.Lerp(Color.red, Color.green, num);
		this.m_highlighter.Settings.BlurAdaptiveThickness = 0.5f;
		this.m_highlighter.Settings.UseOverlay = true;
		Color a = new Color(1f, 0f, 0f, 0.1f);
		Color b = new Color(0f, 1f, 0f, 0.1f);
		this.m_highlighter.Settings.OverlayFront.Color = Color.Lerp(a, b, num);
	}

	// Token: 0x04000001 RID: 1
	private Highlighter m_highlighter;

	// Token: 0x04000002 RID: 2
	private float m_timer;
}
