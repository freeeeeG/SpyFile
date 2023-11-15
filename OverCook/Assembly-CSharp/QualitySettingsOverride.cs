using System;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class QualitySettingsOverride : MonoBehaviour
{
	// Token: 0x060007E9 RID: 2025 RVA: 0x00030F2C File Offset: 0x0002F32C
	private void CacheSettings()
	{
		this.m_shadowCachedSettings = new QualitySettingsOverride.ShadowSettings();
		this.m_shadowCachedSettings.m_quality = QualitySettings.shadows;
		this.m_shadowCachedSettings.m_resolution = QualitySettings.shadowResolution;
		this.m_shadowCachedSettings.m_projection = QualitySettings.shadowProjection;
		this.m_shadowCachedSettings.m_distance = QualitySettings.shadowDistance;
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00030F84 File Offset: 0x0002F384
	private void ApplySettings(QualitySettingsOverride.ShadowSettings settings)
	{
		QualitySettings.shadows = settings.m_quality;
		QualitySettings.shadowResolution = settings.m_resolution;
		QualitySettings.shadowProjection = settings.m_projection;
		QualitySettings.shadowDistance = settings.m_distance;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00030FB2 File Offset: 0x0002F3B2
	private void Awake()
	{
		this.CacheSettings();
		this.ApplySettings(this.m_shadowOverrides);
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00030FC6 File Offset: 0x0002F3C6
	private void OnDestroy()
	{
		this.ApplySettings(this.m_shadowCachedSettings);
	}

	// Token: 0x0400064B RID: 1611
	public QualitySettingsOverride.ShadowSettings m_shadowOverrides;

	// Token: 0x0400064C RID: 1612
	private QualitySettingsOverride.ShadowSettings m_shadowCachedSettings;

	// Token: 0x020001D0 RID: 464
	[Serializable]
	public class ShadowSettings
	{
		// Token: 0x0400064D RID: 1613
		public ShadowQuality m_quality;

		// Token: 0x0400064E RID: 1614
		public ShadowResolution m_resolution;

		// Token: 0x0400064F RID: 1615
		public ShadowProjection m_projection;

		// Token: 0x04000650 RID: 1616
		public float m_distance;
	}
}
