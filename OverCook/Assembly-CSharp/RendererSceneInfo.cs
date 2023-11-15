using System;
using UnityEngine;

// Token: 0x0200079E RID: 1950
[ExecuteInEditMode]
public class RendererSceneInfo : MonoBehaviour
{
	// Token: 0x060025A5 RID: 9637 RVA: 0x000B1F21 File Offset: 0x000B0321
	protected void Start()
	{
		this.UpdateRendererSettings();
		this.ApplySettings(true);
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000B1F30 File Offset: 0x000B0330
	protected void UpdateRendererSettings()
	{
		RendererSceneSettings rendererSceneSettings = GameUtils.RequestManager<RendererSceneSettings>();
		RendererSceneSettings.Settings value;
		if (rendererSceneSettings != null && rendererSceneSettings.TryGetSettingsForClass(this.m_rendererClass, out value))
		{
			this.m_settings = new RendererSceneSettings.Settings?(value);
			return;
		}
		this.m_settings = null;
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000B1F80 File Offset: 0x000B0380
	public void ApplySettings(bool _updateHirearchy)
	{
		if (this.m_settings != null)
		{
			if (_updateHirearchy)
			{
				if (this.m_rendererClass == RendererSceneSettings.RendererClass.Avatar)
				{
					this.m_renderers = base.gameObject.RequestComponentsRecursive<SkinnedMeshRenderer>();
				}
				else
				{
					this.m_renderers = base.gameObject.RequestComponentsRecursive<Renderer>();
				}
			}
			RendererSceneSettings.Settings value = this.m_settings.Value;
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				this.ApplySettingsToRenderer(this.m_renderers[i], ref value);
			}
		}
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000B200A File Offset: 0x000B040A
	protected void ApplySettingsToRenderer(Renderer _renderer, ref RendererSceneSettings.Settings _settings)
	{
		_renderer.lightProbeUsage = _settings.lightProbeUsage;
		_renderer.reflectionProbeUsage = _settings.reflectionProbeUsage;
		_renderer.probeAnchor = _settings.probeAnchor;
		_renderer.shadowCastingMode = _settings.shadowCastingMode;
		_renderer.receiveShadows = _settings.receiveShadows;
	}

	// Token: 0x04001D2E RID: 7470
	[SerializeField]
	public RendererSceneSettings.RendererClass m_rendererClass = RendererSceneSettings.RendererClass.PhysicalAttachment;

	// Token: 0x04001D2F RID: 7471
	private RendererSceneSettings.Settings? m_settings;

	// Token: 0x04001D30 RID: 7472
	private Renderer[] m_renderers = new Renderer[0];
}
