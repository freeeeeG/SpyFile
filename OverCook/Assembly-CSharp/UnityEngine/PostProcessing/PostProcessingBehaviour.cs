﻿using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E1 RID: 225
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("Effects/Post-Processing Behaviour", -1)]
	public class PostProcessingBehaviour : MonoBehaviour
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x00025A8C File Offset: 0x00023E8C
		private void OnEnable()
		{
			this.m_CommandBuffers = new Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>>();
			this.m_MaterialFactory = new MaterialFactory();
			this.m_RenderTextureFactory = new RenderTextureFactory();
			this.m_Context = new PostProcessingContext();
			this.m_Components = new List<PostProcessingComponentBase>();
			this.m_DebugViews = this.AddComponent<BuiltinDebugViewsComponent>(new BuiltinDebugViewsComponent());
			this.m_AmbientOcclusion = this.AddComponent<AmbientOcclusionComponent>(new AmbientOcclusionComponent());
			this.m_ScreenSpaceReflection = this.AddComponent<ScreenSpaceReflectionComponent>(new ScreenSpaceReflectionComponent());
			this.m_FogComponent = this.AddComponent<FogComponent>(new FogComponent());
			this.m_MotionBlur = this.AddComponent<MotionBlurComponent>(new MotionBlurComponent());
			this.m_Taa = this.AddComponent<TaaComponent>(new TaaComponent());
			this.m_EyeAdaptation = this.AddComponent<EyeAdaptationComponent>(new EyeAdaptationComponent());
			this.m_DepthOfField = this.AddComponent<DepthOfFieldComponent>(new DepthOfFieldComponent());
			this.m_Bloom = this.AddComponent<BloomComponent>(new BloomComponent());
			this.m_ChromaticAberration = this.AddComponent<ChromaticAberrationComponent>(new ChromaticAberrationComponent());
			this.m_ColorGrading = this.AddComponent<ColorGradingComponent>(new ColorGradingComponent());
			this.m_UserLut = this.AddComponent<UserLutComponent>(new UserLutComponent());
			this.m_Grain = this.AddComponent<GrainComponent>(new GrainComponent());
			this.m_Vignette = this.AddComponent<VignetteComponent>(new VignetteComponent());
			this.m_Dithering = this.AddComponent<DitheringComponent>(new DitheringComponent());
			this.m_Fxaa = this.AddComponent<FxaaComponent>(new FxaaComponent());
			this.m_TiltShift = this.AddComponent<TiltShiftComponent>(new TiltShiftComponent());
			this.m_ComponentStates = new Dictionary<PostProcessingComponentBase, bool>();
			for (int i = 0; i < this.m_Components.Count; i++)
			{
				PostProcessingComponentBase key = this.m_Components[i];
				this.m_ComponentStates.Add(key, false);
			}
			base.useGUILayout = false;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00025C3C File Offset: 0x0002403C
		private void OnPreCull()
		{
			this.m_Camera = base.GetComponent<Camera>();
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			PostProcessingContext postProcessingContext = this.m_Context.Reset();
			postProcessingContext.profile = this.profile;
			postProcessingContext.renderTextureFactory = this.m_RenderTextureFactory;
			postProcessingContext.materialFactory = this.m_MaterialFactory;
			postProcessingContext.camera = this.m_Camera;
			this.m_DebugViews.Init(postProcessingContext, this.profile.debugViews);
			this.m_AmbientOcclusion.Init(postProcessingContext, this.profile.ambientOcclusion);
			this.m_ScreenSpaceReflection.Init(postProcessingContext, this.profile.screenSpaceReflection);
			this.m_FogComponent.Init(postProcessingContext, this.profile.fog);
			this.m_MotionBlur.Init(postProcessingContext, this.profile.motionBlur);
			this.m_Taa.Init(postProcessingContext, this.profile.antialiasing);
			this.m_EyeAdaptation.Init(postProcessingContext, this.profile.eyeAdaptation);
			this.m_DepthOfField.Init(postProcessingContext, this.profile.depthOfField);
			this.m_Bloom.Init(postProcessingContext, this.profile.bloom);
			this.m_ChromaticAberration.Init(postProcessingContext, this.profile.chromaticAberration);
			this.m_ColorGrading.Init(postProcessingContext, this.profile.colorGrading);
			this.m_UserLut.Init(postProcessingContext, this.profile.userLut);
			this.m_Grain.Init(postProcessingContext, this.profile.grain);
			this.m_Vignette.Init(postProcessingContext, this.profile.vignette);
			this.m_Dithering.Init(postProcessingContext, this.profile.dithering);
			this.m_Fxaa.Init(postProcessingContext, this.profile.antialiasing);
			this.m_TiltShift.Init(postProcessingContext, this.profile.tiltshift);
			if (this.m_PreviousProfile != this.profile)
			{
				this.DisableComponents();
				this.m_PreviousProfile = this.profile;
			}
			this.CheckObservers();
			DepthTextureMode depthTextureMode = postProcessingContext.camera.depthTextureMode;
			for (int i = 0; i < this.m_Components.Count; i++)
			{
				PostProcessingComponentBase postProcessingComponentBase = this.m_Components[i];
				if (postProcessingComponentBase.active)
				{
					depthTextureMode |= postProcessingComponentBase.GetCameraFlags();
				}
			}
			postProcessingContext.camera.depthTextureMode = depthTextureMode;
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Taa.SetProjectionMatrix(this.jitteredMatrixFunc);
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00025F00 File Offset: 0x00024300
		private void OnPreRender()
		{
			if (this.profile == null)
			{
				return;
			}
			this.TryExecuteCommandBuffer<BuiltinDebugViewsModel>(this.m_DebugViews);
			this.TryExecuteCommandBuffer<AmbientOcclusionModel>(this.m_AmbientOcclusion);
			this.TryExecuteCommandBuffer<ScreenSpaceReflectionModel>(this.m_ScreenSpaceReflection);
			this.TryExecuteCommandBuffer<FogModel>(this.m_FogComponent);
			this.TryExecuteCommandBuffer<TiltShiftModel>(this.m_TiltShift);
			if (!this.m_RenderingInSceneView)
			{
				this.TryExecuteCommandBuffer<MotionBlurModel>(this.m_MotionBlur);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00025F74 File Offset: 0x00024374
		private void OnPostRender()
		{
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (!this.m_RenderingInSceneView && this.m_Taa.active && !this.profile.debugViews.willInterrupt)
			{
				this.m_Context.camera.ResetProjectionMatrix();
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00025FE4 File Offset: 0x000243E4
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.profile == null || this.m_Camera == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			bool flag = false;
			bool active = this.m_Fxaa.active;
			bool flag2 = this.m_Taa.active && !this.m_RenderingInSceneView;
			bool flag3 = this.m_DepthOfField.active && !this.m_RenderingInSceneView;
			Material material = this.m_MaterialFactory.Get("Hidden/Post FX/Uber Shader");
			material.shaderKeywords = null;
			RenderTexture renderTexture = source;
			if (flag2)
			{
				RenderTexture renderTexture2 = this.m_RenderTextureFactory.Get(renderTexture);
				this.m_Taa.Render(renderTexture, renderTexture2);
				renderTexture = renderTexture2;
			}
			Texture texture = GraphicsUtils.whiteTexture;
			if (this.m_EyeAdaptation.active)
			{
				flag = true;
				texture = this.m_EyeAdaptation.Prepare(renderTexture, material);
			}
			material.SetTexture("_AutoExposure", texture);
			if (flag3)
			{
				flag = true;
				this.m_DepthOfField.Prepare(renderTexture, material, flag2, this.m_Taa.jitterVector, this.m_Taa.model.settings.taaSettings.motionBlending);
			}
			if (this.m_Bloom.active)
			{
				flag = true;
				this.m_Bloom.Prepare(renderTexture, material, texture);
			}
			flag |= this.TryPrepareUberImageEffect<ChromaticAberrationModel>(this.m_ChromaticAberration, material);
			flag |= this.TryPrepareUberImageEffect<ColorGradingModel>(this.m_ColorGrading, material);
			flag |= this.TryPrepareUberImageEffect<VignetteModel>(this.m_Vignette, material);
			flag |= this.TryPrepareUberImageEffect<UserLutModel>(this.m_UserLut, material);
			Material material2 = (!active) ? null : this.m_MaterialFactory.Get("Hidden/Post FX/FXAA");
			if (active)
			{
				material2.shaderKeywords = null;
				this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material2);
				this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material2);
				if (flag)
				{
					RenderTexture renderTexture3 = this.m_RenderTextureFactory.Get(renderTexture);
					Graphics.Blit(renderTexture, renderTexture3, material, 0);
					renderTexture = renderTexture3;
				}
				this.m_Fxaa.Render(renderTexture, destination);
			}
			else
			{
				flag |= this.TryPrepareUberImageEffect<GrainModel>(this.m_Grain, material);
				flag |= this.TryPrepareUberImageEffect<DitheringModel>(this.m_Dithering, material);
				if (flag)
				{
					if (!GraphicsUtils.isLinearColorSpace)
					{
						material.EnableKeyword("UNITY_COLORSPACE_GAMMA");
					}
					Graphics.Blit(renderTexture, destination, material, 0);
				}
			}
			if (!flag && !active)
			{
				Graphics.Blit(renderTexture, destination);
			}
			this.m_RenderTextureFactory.ReleaseAll();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00026278 File Offset: 0x00024678
		private void OnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (this.profile == null || this.m_Camera == null)
			{
				return;
			}
			if (this.m_EyeAdaptation.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				this.m_EyeAdaptation.OnGUI();
			}
			else if (this.m_ColorGrading.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut))
			{
				this.m_ColorGrading.OnGUI();
			}
			else if (this.m_UserLut.active && this.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut))
			{
				this.m_UserLut.OnGUI();
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00026358 File Offset: 0x00024758
		private void OnDisable()
		{
			foreach (KeyValuePair<CameraEvent, CommandBuffer> keyValuePair in this.m_CommandBuffers.Values)
			{
				this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
				keyValuePair.Value.Dispose();
			}
			this.m_CommandBuffers.Clear();
			if (this.profile != null)
			{
				this.DisableComponents();
			}
			this.m_Components.Clear();
			this.m_MaterialFactory.Dispose();
			this.m_RenderTextureFactory.Dispose();
			GraphicsUtils.Dispose();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00026420 File Offset: 0x00024820
		public void ResetTemporalEffects()
		{
			this.m_Taa.ResetHistory();
			this.m_MotionBlur.ResetHistory();
			this.m_EyeAdaptation.ResetHistory();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00026444 File Offset: 0x00024844
		private void CheckObservers()
		{
			foreach (KeyValuePair<PostProcessingComponentBase, bool> keyValuePair in this.m_ComponentStates)
			{
				PostProcessingComponentBase key = keyValuePair.Key;
				bool enabled = key.GetModel().enabled;
				if (enabled != keyValuePair.Value)
				{
					if (enabled)
					{
						this.m_ComponentsToEnable.Add(key);
					}
					else
					{
						this.m_ComponentsToDisable.Add(key);
					}
				}
			}
			for (int i = 0; i < this.m_ComponentsToDisable.Count; i++)
			{
				PostProcessingComponentBase postProcessingComponentBase = this.m_ComponentsToDisable[i];
				this.m_ComponentStates[postProcessingComponentBase] = false;
				postProcessingComponentBase.OnDisable();
			}
			for (int j = 0; j < this.m_ComponentsToEnable.Count; j++)
			{
				PostProcessingComponentBase postProcessingComponentBase2 = this.m_ComponentsToEnable[j];
				this.m_ComponentStates[postProcessingComponentBase2] = true;
				postProcessingComponentBase2.OnEnable();
			}
			this.m_ComponentsToDisable.Clear();
			this.m_ComponentsToEnable.Clear();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0002657C File Offset: 0x0002497C
		private void DisableComponents()
		{
			foreach (PostProcessingComponentBase postProcessingComponentBase in this.m_Components)
			{
				PostProcessingModel model = postProcessingComponentBase.GetModel();
				if (model != null && model.enabled)
				{
					postProcessingComponentBase.OnDisable();
				}
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000265F0 File Offset: 0x000249F0
		private CommandBuffer AddCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			CommandBuffer value = new CommandBuffer
			{
				name = name
			};
			KeyValuePair<CameraEvent, CommandBuffer> value2 = new KeyValuePair<CameraEvent, CommandBuffer>(evt, value);
			this.m_CommandBuffers.Add(typeof(T), value2);
			this.m_Camera.AddCommandBuffer(evt, value2.Value);
			return value2.Value;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00026648 File Offset: 0x00024A48
		private void RemoveCommandBuffer<T>() where T : PostProcessingModel
		{
			Type typeFromHandle = typeof(T);
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			if (!this.m_CommandBuffers.TryGetValue(typeFromHandle, out keyValuePair))
			{
				return;
			}
			this.m_Camera.RemoveCommandBuffer(keyValuePair.Key, keyValuePair.Value);
			this.m_CommandBuffers.Remove(typeFromHandle);
			keyValuePair.Value.Dispose();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000266A8 File Offset: 0x00024AA8
		private CommandBuffer GetCommandBuffer<T>(CameraEvent evt, string name) where T : PostProcessingModel
		{
			KeyValuePair<CameraEvent, CommandBuffer> keyValuePair;
			CommandBuffer result;
			if (!this.m_CommandBuffers.TryGetValue(typeof(T), out keyValuePair))
			{
				result = this.AddCommandBuffer<T>(evt, name);
			}
			else if (keyValuePair.Key != evt)
			{
				this.RemoveCommandBuffer<T>();
				result = this.AddCommandBuffer<T>(evt, name);
			}
			else
			{
				result = keyValuePair.Value;
			}
			return result;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0002670C File Offset: 0x00024B0C
		private void TryExecuteCommandBuffer<T>(PostProcessingComponentCommandBuffer<T> component) where T : PostProcessingModel
		{
			if (component.active)
			{
				CommandBuffer commandBuffer = this.GetCommandBuffer<T>(component.GetCameraEvent(), component.GetName());
				commandBuffer.Clear();
				component.PopulateCommandBuffer(commandBuffer);
			}
			else
			{
				this.RemoveCommandBuffer<T>();
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0002674F File Offset: 0x00024B4F
		private bool TryPrepareUberImageEffect<T>(PostProcessingComponentRenderTexture<T> component, Material material) where T : PostProcessingModel
		{
			if (!component.active)
			{
				return false;
			}
			component.Prepare(material);
			return true;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00026766 File Offset: 0x00024B66
		private T AddComponent<T>(T component) where T : PostProcessingComponentBase
		{
			this.m_Components.Add(component);
			return component;
		}

		// Token: 0x040003BA RID: 954
		public PostProcessingProfile profile;

		// Token: 0x040003BB RID: 955
		public Func<Vector2, Matrix4x4> jitteredMatrixFunc;

		// Token: 0x040003BC RID: 956
		private Dictionary<Type, KeyValuePair<CameraEvent, CommandBuffer>> m_CommandBuffers;

		// Token: 0x040003BD RID: 957
		private List<PostProcessingComponentBase> m_Components;

		// Token: 0x040003BE RID: 958
		private Dictionary<PostProcessingComponentBase, bool> m_ComponentStates;

		// Token: 0x040003BF RID: 959
		private MaterialFactory m_MaterialFactory;

		// Token: 0x040003C0 RID: 960
		private RenderTextureFactory m_RenderTextureFactory;

		// Token: 0x040003C1 RID: 961
		private PostProcessingContext m_Context;

		// Token: 0x040003C2 RID: 962
		private Camera m_Camera;

		// Token: 0x040003C3 RID: 963
		private PostProcessingProfile m_PreviousProfile;

		// Token: 0x040003C4 RID: 964
		private bool m_RenderingInSceneView;

		// Token: 0x040003C5 RID: 965
		private BuiltinDebugViewsComponent m_DebugViews;

		// Token: 0x040003C6 RID: 966
		private AmbientOcclusionComponent m_AmbientOcclusion;

		// Token: 0x040003C7 RID: 967
		private ScreenSpaceReflectionComponent m_ScreenSpaceReflection;

		// Token: 0x040003C8 RID: 968
		private FogComponent m_FogComponent;

		// Token: 0x040003C9 RID: 969
		private MotionBlurComponent m_MotionBlur;

		// Token: 0x040003CA RID: 970
		private TaaComponent m_Taa;

		// Token: 0x040003CB RID: 971
		private EyeAdaptationComponent m_EyeAdaptation;

		// Token: 0x040003CC RID: 972
		private DepthOfFieldComponent m_DepthOfField;

		// Token: 0x040003CD RID: 973
		private BloomComponent m_Bloom;

		// Token: 0x040003CE RID: 974
		private ChromaticAberrationComponent m_ChromaticAberration;

		// Token: 0x040003CF RID: 975
		private ColorGradingComponent m_ColorGrading;

		// Token: 0x040003D0 RID: 976
		private UserLutComponent m_UserLut;

		// Token: 0x040003D1 RID: 977
		private GrainComponent m_Grain;

		// Token: 0x040003D2 RID: 978
		private VignetteComponent m_Vignette;

		// Token: 0x040003D3 RID: 979
		private DitheringComponent m_Dithering;

		// Token: 0x040003D4 RID: 980
		private FxaaComponent m_Fxaa;

		// Token: 0x040003D5 RID: 981
		private TiltShiftComponent m_TiltShift;

		// Token: 0x040003D6 RID: 982
		private List<PostProcessingComponentBase> m_ComponentsToEnable = new List<PostProcessingComponentBase>();

		// Token: 0x040003D7 RID: 983
		private List<PostProcessingComponentBase> m_ComponentsToDisable = new List<PostProcessingComponentBase>();
	}
}
