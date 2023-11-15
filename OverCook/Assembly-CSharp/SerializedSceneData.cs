using System;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;

// Token: 0x020007A2 RID: 1954
[Serializable]
public class SerializedSceneData : ScriptableObject
{
	// Token: 0x04001D3E RID: 7486
	[Header("Scene Data")]
	[SerializeField]
	public SerializedSceneData.CameraData CameraSettings = new SerializedSceneData.CameraData();

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	public SerializedSceneData.RenderData SceneSettings = new SerializedSceneData.RenderData();

	// Token: 0x04001D40 RID: 7488
	[Space]
	[SerializeField]
	public SerializedSceneData.AudioData AudioSettings = new SerializedSceneData.AudioData();

	// Token: 0x020007A3 RID: 1955
	[Serializable]
	public class CameraData
	{
		// Token: 0x04001D41 RID: 7489
		[SerializeField]
		public PostProcessingProfile PostProcessing;

		// Token: 0x04001D42 RID: 7490
		[SerializeField]
		public SerializedSceneData.CameraData.FogData Fog = new SerializedSceneData.CameraData.FogData();

		// Token: 0x020007A4 RID: 1956
		[Serializable]
		public class FogData
		{
			// Token: 0x04001D43 RID: 7491
			[SerializeField]
			public FogConfig.Kind m_kind;

			// Token: 0x04001D44 RID: 7492
			[SerializeField]
			public float m_fogOffset;

			// Token: 0x04001D45 RID: 7493
			[SerializeField]
			public float m_fogNear;

			// Token: 0x04001D46 RID: 7494
			[SerializeField]
			public float m_fogFar = 100f;

			// Token: 0x04001D47 RID: 7495
			[SerializeField]
			public Color m_fogColour = Color.grey;
		}
	}

	// Token: 0x020007A5 RID: 1957
	[Serializable]
	public class RenderData
	{
		// Token: 0x04001D48 RID: 7496
		[SerializeField]
		public Material SkyboxMaterial;

		// Token: 0x04001D49 RID: 7497
		[SerializeField]
		public AmbientMode AmbientSource = AmbientMode.Trilight;

		// Token: 0x04001D4A RID: 7498
		[HideInInspectorTest("AmbientSource", AmbientMode.Skybox)]
		[SerializeField]
		public SerializedSceneData.RenderData.SkyboxAmbientData SkyboxData = new SerializedSceneData.RenderData.SkyboxAmbientData();

		// Token: 0x04001D4B RID: 7499
		[HideInInspectorTest("AmbientSource", AmbientMode.Trilight)]
		[SerializeField]
		public SerializedSceneData.RenderData.GradientAmbientData GradientData = new SerializedSceneData.RenderData.GradientAmbientData();

		// Token: 0x04001D4C RID: 7500
		[HideInInspectorTest("AmbientSource", AmbientMode.Flat)]
		[SerializeField]
		public SerializedSceneData.RenderData.ColorAmbientData ColorData = new SerializedSceneData.RenderData.ColorAmbientData();

		// Token: 0x04001D4D RID: 7501
		[SerializeField]
		public DefaultReflectionMode ReflectionSource = DefaultReflectionMode.Custom;

		// Token: 0x04001D4E RID: 7502
		[HideInInspectorTest("ReflectionSource", DefaultReflectionMode.Skybox)]
		[SerializeField]
		public SerializedSceneData.RenderData.SkyboxEnvironmentData SkyboxReflectionData = new SerializedSceneData.RenderData.SkyboxEnvironmentData();

		// Token: 0x04001D4F RID: 7503
		[HideInInspectorTest("ReflectionSource", DefaultReflectionMode.Custom)]
		[SerializeField]
		public SerializedSceneData.RenderData.CustomEnvironmentData CustomReflectionData = new SerializedSceneData.RenderData.CustomEnvironmentData();

		// Token: 0x020007A6 RID: 1958
		[Serializable]
		public class GradientAmbientData
		{
			// Token: 0x04001D50 RID: 7504
			[SerializeField]
			[ColorUsage(false)]
			public Color SkyColour = Color.white;

			// Token: 0x04001D51 RID: 7505
			[SerializeField]
			[ColorUsage(false)]
			public Color EquatorColour = Color.white;

			// Token: 0x04001D52 RID: 7506
			[SerializeField]
			[ColorUsage(false)]
			public Color GroundColour = Color.white;
		}

		// Token: 0x020007A7 RID: 1959
		[Serializable]
		public class SkyboxAmbientData
		{
			// Token: 0x04001D53 RID: 7507
			[SerializeField]
			[ColorUsage(false)]
			public Color SkyboxColour = Color.white;
		}

		// Token: 0x020007A8 RID: 1960
		[Serializable]
		public class ColorAmbientData
		{
			// Token: 0x04001D54 RID: 7508
			[SerializeField]
			[ColorUsage(false)]
			public Color Colour = Color.white;
		}

		// Token: 0x020007A9 RID: 1961
		[Serializable]
		public class SkyboxEnvironmentData
		{
			// Token: 0x04001D55 RID: 7509
			[SerializeField]
			public int Resolution = 32;

			// Token: 0x04001D56 RID: 7510
			[SerializeField]
			public float IntensityMultiplier = 1f;

			// Token: 0x04001D57 RID: 7511
			[SerializeField]
			public int Bounces = 5;
		}

		// Token: 0x020007AA RID: 1962
		[Serializable]
		public class CustomEnvironmentData
		{
			// Token: 0x04001D58 RID: 7512
			[SerializeField]
			public Cubemap Cubemap;

			// Token: 0x04001D59 RID: 7513
			[SerializeField]
			public float IntensityMultiplier = 1f;

			// Token: 0x04001D5A RID: 7514
			[SerializeField]
			public int Bounces = 5;
		}
	}

	// Token: 0x020007AB RID: 1963
	[Serializable]
	public class AudioData
	{
		// Token: 0x04001D5B RID: 7515
		[SerializeField]
		public AudioDirectoryData[] Directories = new AudioDirectoryData[0];

		// Token: 0x04001D5C RID: 7516
		[SerializeField]
		public AudioClip Music;

		// Token: 0x04001D5D RID: 7517
		[SerializeField]
		public AudioClip Ambience;
	}

	// Token: 0x020007AC RID: 1964
	[Serializable]
	public class SkyColours
	{
		// Token: 0x04001D5E RID: 7518
		[SerializeField]
		[ColorUsage(false)]
		public Color m_waterColour;

		// Token: 0x04001D5F RID: 7519
		[SerializeField]
		[ColorUsage(false)]
		public Color m_cloudColour;

		// Token: 0x04001D60 RID: 7520
		[SerializeField]
		[ColorUsage(false)]
		public Color m_cloudShadowColour;
	}
}
