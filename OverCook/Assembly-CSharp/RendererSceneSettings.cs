using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200079F RID: 1951
public class RendererSceneSettings : Manager
{
	// Token: 0x060025AA RID: 9642 RVA: 0x000B2068 File Offset: 0x000B0468
	public bool TryGetSettingsForClass(RendererSceneSettings.RendererClass _class, out RendererSceneSettings.Settings _settings)
	{
		int num = Mathf.Min(this.m_enabled.Length, this.m_settings.Length);
		if (_class >= RendererSceneSettings.RendererClass.Avatar && _class < (RendererSceneSettings.RendererClass)num && this.m_enabled[(int)_class])
		{
			_settings = this.m_settings[(int)_class];
			return true;
		}
		_settings = default(RendererSceneSettings.Settings);
		return false;
	}

	// Token: 0x04001D31 RID: 7473
	[HideInInspector]
	[SerializeField]
	private RendererSceneSettings.Settings[] m_settings = new RendererSceneSettings.Settings[0];

	// Token: 0x04001D32 RID: 7474
	[HideInInspector]
	[SerializeField]
	private bool[] m_enabled = new bool[0];

	// Token: 0x020007A0 RID: 1952
	public enum RendererClass
	{
		// Token: 0x04001D34 RID: 7476
		Invalid = -1,
		// Token: 0x04001D35 RID: 7477
		Avatar,
		// Token: 0x04001D36 RID: 7478
		PhysicalAttachment,
		// Token: 0x04001D37 RID: 7479
		Plate,
		// Token: 0x04001D38 RID: 7480
		MealCosmetic
	}

	// Token: 0x020007A1 RID: 1953
	[Serializable]
	public struct Settings
	{
		// Token: 0x04001D39 RID: 7481
		[SerializeField]
		public LightProbeUsage lightProbeUsage;

		// Token: 0x04001D3A RID: 7482
		[SerializeField]
		public ReflectionProbeUsage reflectionProbeUsage;

		// Token: 0x04001D3B RID: 7483
		[SerializeField]
		public Transform probeAnchor;

		// Token: 0x04001D3C RID: 7484
		[SerializeField]
		public ShadowCastingMode shadowCastingMode;

		// Token: 0x04001D3D RID: 7485
		[SerializeField]
		public bool receiveShadows;
	}
}
