using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000BC0 RID: 3008
public class WorldMapRegionDynamics : MonoBehaviour
{
	// Token: 0x06003D91 RID: 15761 RVA: 0x00125C84 File Offset: 0x00124084
	private void Awake()
	{
		if (this.m_transitioner != null)
		{
			this.m_transitioner.RegisterRegionChangeCallback(new WorldMapRegionTransitioner.RegionChangeCallback(this.OnRegionChanged));
		}
		if (this.m_camera != null && this.m_camera.gameObject != null)
		{
			PostProcessingBehaviour postProcessingBehaviour = this.m_camera.gameObject.RequestComponent<PostProcessingBehaviour>();
			if (postProcessingBehaviour != null)
			{
				this.m_postProcessing = postProcessingBehaviour;
			}
		}
		if (this.m_skyRenderer != null)
		{
			this.m_skyMaterials = this.m_skyRenderer.materials.AllRemoved_Predicate((Material material) => material == null || !material.HasProperty(this.m_skyWaterID) || !material.HasProperty(this.m_skyCloudID) || !material.HasProperty(this.m_skyCloudShadowID));
		}
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00125D38 File Offset: 0x00124138
	private void OnRegionChanged(WorldMapRegion _oldRegion, WorldMapRegion _newRegion)
	{
		IEnumerator enumerator = this.TransitionRoutine(_oldRegion, _newRegion);
		enumerator.MoveNext();
		this.m_transitionRoutine = enumerator;
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x00125D5C File Offset: 0x0012415C
	private IEnumerator TransitionRoutine(WorldMapRegion _oldRegion, WorldMapRegion _newRegion)
	{
		int layer = base.gameObject.layer;
		float time = 0f;
		float progress = 0f;
		bool passedHalfway = false;
		SerializedSceneData.RenderData existingRenderData = StartScreenBackgroundDataUtils.CopyCurrentRenderData();
		PostProcessingProfile existingPostProcProfile = this.CopyCurrentPostProcessingProfile(_oldRegion);
		do
		{
			time = Mathf.Min(time + TimeManager.GetDeltaTime(layer), this.m_transitionData.m_duration);
			progress = ((this.m_transitionData.m_duration <= 0f) ? (progress = 1f) : Mathf.Clamp01(this.m_transitionData.m_curve.Evaluate(time / this.m_transitionData.m_duration)));
			if (progress >= 0.5f && !passedHalfway)
			{
				StartScreenBackgroundDataUtils.SwapBackgroundAudio(_newRegion.m_data.AudioSettings, ref this.m_MusicSource, ref this.m_AmbienceSource, false);
				passedHalfway = true;
			}
			StartScreenBackgroundDataUtils.LerpRenderData(existingRenderData, _newRegion.m_data.SceneSettings, progress);
			if (this.m_skyMaterials != null)
			{
				SerializedSceneData.SkyColours skyColourSettings = _newRegion.m_data.SkyColourSettings;
				SerializedSceneData.SkyColours start = (!(_oldRegion != null)) ? skyColourSettings : _oldRegion.m_data.SkyColourSettings;
				for (int i = 0; i < this.m_skyMaterials.Length; i++)
				{
					Material material = this.m_skyMaterials[i];
					this.LerpSky(start, skyColourSettings, ref material, progress);
				}
			}
			if (existingPostProcProfile != null && _newRegion.m_data.CameraSettings.PostProcessing != null)
			{
				bool enabled = existingPostProcProfile.motionBlur.enabled;
				PostProcessingBehaviourUtils.Lerp(existingPostProcProfile, _newRegion.m_data.CameraSettings.PostProcessing, ref this.m_postProcessing.profile, progress, this.m_transitionData.m_postProcLerpFilter, int.MaxValue);
				existingPostProcProfile.motionBlur.enabled = enabled;
			}
			yield return null;
		}
		while (time < this.m_transitionData.m_duration);
		yield break;
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x00125D88 File Offset: 0x00124188
	private void LerpSky(SerializedSceneData.SkyColours _start, SerializedSceneData.SkyColours _end, ref Material _material, float _progress)
	{
		_material.SetColor(this.m_skyWaterID, Color.Lerp(_start.m_waterColour, _end.m_waterColour, _progress));
		_material.SetColor(this.m_skyCloudID, Color.Lerp(_start.m_cloudColour, _end.m_cloudColour, _progress));
		_material.SetColor(this.m_skyCloudShadowID, Color.Lerp(_start.m_cloudShadowColour, _end.m_cloudShadowColour, _progress));
	}

	// Token: 0x06003D95 RID: 15765 RVA: 0x00125DF8 File Offset: 0x001241F8
	private PostProcessingProfile CopyCurrentPostProcessingProfile(WorldMapRegion _oldRegion)
	{
		if ((this.m_transitionRoutine != null || _oldRegion == null) && this.m_postProcessing != null)
		{
			return UnityEngine.Object.Instantiate<PostProcessingProfile>(this.m_postProcessing.profile);
		}
		if (_oldRegion != null && _oldRegion.m_data != null && _oldRegion.m_data.CameraSettings.PostProcessing != null)
		{
			return _oldRegion.m_data.CameraSettings.PostProcessing;
		}
		return null;
	}

	// Token: 0x06003D96 RID: 15766 RVA: 0x00125E88 File Offset: 0x00124288
	private void Update()
	{
		if (this.m_transitionRoutine != null && !this.m_transitionRoutine.MoveNext())
		{
			this.m_transitionRoutine = null;
		}
	}

	// Token: 0x06003D97 RID: 15767 RVA: 0x00125EAC File Offset: 0x001242AC
	private void OnDestroy()
	{
		if (this.m_transitioner != null)
		{
			this.m_transitioner.UnregisterRegionChangeCallback(new WorldMapRegionTransitioner.RegionChangeCallback(this.OnRegionChanged));
		}
	}

	// Token: 0x0400316E RID: 12654
	[SerializeField]
	[AssignComponent(Editorbility.Editable)]
	private WorldMapRegionTransitioner m_transitioner;

	// Token: 0x0400316F RID: 12655
	[SerializeField]
	public WorldMapRegionDynamics.TransitionData m_transitionData;

	// Token: 0x04003170 RID: 12656
	private IEnumerator m_transitionRoutine;

	// Token: 0x04003171 RID: 12657
	[SerializeField]
	public Camera m_camera;

	// Token: 0x04003172 RID: 12658
	private PostProcessingBehaviour m_postProcessing;

	// Token: 0x04003173 RID: 12659
	[SerializeField]
	private PersistentMusic m_MusicSource;

	// Token: 0x04003174 RID: 12660
	[SerializeField]
	private AudioSource m_AmbienceSource;

	// Token: 0x04003175 RID: 12661
	[SerializeField]
	private Renderer m_skyRenderer;

	// Token: 0x04003176 RID: 12662
	private Material[] m_skyMaterials;

	// Token: 0x04003177 RID: 12663
	private readonly int m_skyWaterID = Shader.PropertyToID("_Water_col");

	// Token: 0x04003178 RID: 12664
	private readonly int m_skyCloudID = Shader.PropertyToID("_Cloud_col");

	// Token: 0x04003179 RID: 12665
	private readonly int m_skyCloudShadowID = Shader.PropertyToID("_Cloud_Shadow_col");

	// Token: 0x02000BC1 RID: 3009
	[Serializable]
	public class TransitionData
	{
		// Token: 0x0400317A RID: 12666
		[SerializeField]
		public float m_duration = 1f;

		// Token: 0x0400317B RID: 12667
		[SerializeField]
		public AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400317C RID: 12668
		[SerializeField]
		[Mask(typeof(PostProcessingBehaviourUtils.LerpFilter))]
		public int m_postProcLerpFilter = 2147482623;
	}
}
