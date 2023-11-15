using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B85 RID: 2949
	public static class PostProcessingBehaviourUtils
	{
		// Token: 0x06003C85 RID: 15493 RVA: 0x001214BC File Offset: 0x0011F8BC
		public static void Lerp(PostProcessingProfile _start, PostProcessingProfile _end, ref PostProcessingProfile _delta, float _progress, int _filter = 2147483647, int _valuesToLerp = 2147483647)
		{
			for (int i = 0; i < PostProcessingBehaviourUtils.lerpers.Length; i++)
			{
				PostProcessingBehaviourUtils.Lerper lerper = PostProcessingBehaviourUtils.lerpers[i];
				if (MaskUtils.HasFlag<PostProcessingBehaviourUtils.LerpFilter>(_filter, lerper.LerpTarget))
				{
					PostProcessingModel requiredModel = lerper.GetRequiredModel(_delta);
					lerper.Lerp(lerper.GetRequiredModel(_start), lerper.GetRequiredModel(_end), ref requiredModel, _progress, _valuesToLerp);
				}
			}
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00121524 File Offset: 0x0011F924
		private static int GetValueArrayIndex<T>(T[] _array, T _value)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (EqualityComparer<T>.Default.Equals(_array[i], _value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040030E7 RID: 12519
		private static PostProcessingBehaviourUtils.Lerper[] lerpers = new PostProcessingBehaviourUtils.Lerper[]
		{
			new PostProcessingBehaviourUtils.AmbientOcclusionLerper(),
			new PostProcessingBehaviourUtils.AntialiasingLerper(),
			new PostProcessingBehaviourUtils.BloomLerper(),
			new PostProcessingBehaviourUtils.ChromaticAberrationLerper(),
			new PostProcessingBehaviourUtils.ColorGradingLerper(),
			new PostProcessingBehaviourUtils.DepthOfFieldLerper(),
			new PostProcessingBehaviourUtils.DitheringLerper(),
			new PostProcessingBehaviourUtils.EyeAdaptionLerper(),
			new PostProcessingBehaviourUtils.FogLerper(),
			new PostProcessingBehaviourUtils.GrainLerper(),
			new PostProcessingBehaviourUtils.MotionBlurLerper(),
			new PostProcessingBehaviourUtils.ScreenSpaceReflectionLerper(),
			new PostProcessingBehaviourUtils.TiltShiftLerper(),
			new PostProcessingBehaviourUtils.UserLutLerper(),
			new PostProcessingBehaviourUtils.VignetteLerper()
		};

		// Token: 0x02000B86 RID: 2950
		public enum LerpFilter
		{
			// Token: 0x040030E9 RID: 12521
			AmbientOcclusion,
			// Token: 0x040030EA RID: 12522
			Antialiasing,
			// Token: 0x040030EB RID: 12523
			Bloom,
			// Token: 0x040030EC RID: 12524
			ChromaticAberration,
			// Token: 0x040030ED RID: 12525
			ColorGrading,
			// Token: 0x040030EE RID: 12526
			DepthOfField,
			// Token: 0x040030EF RID: 12527
			Dithering,
			// Token: 0x040030F0 RID: 12528
			EyeAdaptation,
			// Token: 0x040030F1 RID: 12529
			Fog,
			// Token: 0x040030F2 RID: 12530
			Grain,
			// Token: 0x040030F3 RID: 12531
			MotionBlur,
			// Token: 0x040030F4 RID: 12532
			ScreenSpaceReflection,
			// Token: 0x040030F5 RID: 12533
			TiltShift,
			// Token: 0x040030F6 RID: 12534
			UserLut,
			// Token: 0x040030F7 RID: 12535
			Vignette
		}

		// Token: 0x02000B87 RID: 2951
		public enum ValuesToLerp
		{
			// Token: 0x040030F9 RID: 12537
			EnabledState,
			// Token: 0x040030FA RID: 12538
			Parameters
		}

		// Token: 0x02000B88 RID: 2952
		private abstract class Lerper
		{
			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x06003C89 RID: 15497
			public abstract PostProcessingBehaviourUtils.LerpFilter LerpTarget { get; }

			// Token: 0x06003C8A RID: 15498 RVA: 0x00121600 File Offset: 0x0011FA00
			public void Lerp(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, int _valuesToLerp)
			{
				PostProcessingBehaviourUtils.Lerper.LerpType lerpType = this.GetLerpType(_start, _end, _delta, _valuesToLerp);
				if (lerpType == PostProcessingBehaviourUtils.Lerper.LerpType.OnToOff)
				{
					_delta.enabled = !Mathf.Approximately(_progress, 1f);
				}
				if (lerpType == PostProcessingBehaviourUtils.Lerper.LerpType.OffToOn)
				{
					_delta.enabled = !Mathf.Approximately(_progress, 0f);
				}
				if (lerpType != PostProcessingBehaviourUtils.Lerper.LerpType.OffToOff && MaskUtils.HasFlag<PostProcessingBehaviourUtils.ValuesToLerp>(_valuesToLerp, PostProcessingBehaviourUtils.ValuesToLerp.Parameters))
				{
					this.LerpValues((lerpType != PostProcessingBehaviourUtils.Lerper.LerpType.OffToOn) ? _start : this.CreateOffState(_end), (lerpType != PostProcessingBehaviourUtils.Lerper.LerpType.OnToOff) ? _end : this.CreateOffState(_start), ref _delta, _progress, lerpType);
				}
			}

			// Token: 0x06003C8B RID: 15499
			protected abstract void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type);

			// Token: 0x06003C8C RID: 15500
			public abstract PostProcessingModel GetRequiredModel(PostProcessingProfile _profile);

			// Token: 0x06003C8D RID: 15501 RVA: 0x0012169C File Offset: 0x0011FA9C
			private PostProcessingBehaviourUtils.Lerper.LerpType GetLerpType(PostProcessingModel _start, PostProcessingModel _end, PostProcessingModel _delta, int _valuesToLerp)
			{
				if (!MaskUtils.HasFlag<PostProcessingBehaviourUtils.ValuesToLerp>(_valuesToLerp, PostProcessingBehaviourUtils.ValuesToLerp.EnabledState))
				{
					return (!_delta.enabled) ? PostProcessingBehaviourUtils.Lerper.LerpType.OffToOff : PostProcessingBehaviourUtils.Lerper.LerpType.OnToOn;
				}
				if (_start.enabled == _end.enabled)
				{
					return (!_start.enabled) ? PostProcessingBehaviourUtils.Lerper.LerpType.OffToOff : PostProcessingBehaviourUtils.Lerper.LerpType.OnToOn;
				}
				if (_start.enabled && !_end.enabled)
				{
					return PostProcessingBehaviourUtils.Lerper.LerpType.OnToOff;
				}
				return PostProcessingBehaviourUtils.Lerper.LerpType.OffToOn;
			}

			// Token: 0x06003C8E RID: 15502
			protected abstract PostProcessingModel CreateOffState(PostProcessingModel _state);

			// Token: 0x02000B89 RID: 2953
			protected enum LerpType
			{
				// Token: 0x040030FC RID: 12540
				OnToOn,
				// Token: 0x040030FD RID: 12541
				OnToOff,
				// Token: 0x040030FE RID: 12542
				OffToOff,
				// Token: 0x040030FF RID: 12543
				OffToOn
			}
		}

		// Token: 0x02000B8A RID: 2954
		private class AmbientOcclusionLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x06003C90 RID: 15504 RVA: 0x0012170E File Offset: 0x0011FB0E
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.AmbientOcclusion;
				}
			}

			// Token: 0x06003C91 RID: 15505 RVA: 0x00121714 File Offset: 0x0011FB14
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				AmbientOcclusionModel.Settings settings = (_start as AmbientOcclusionModel).settings;
				AmbientOcclusionModel.Settings settings2 = (_end as AmbientOcclusionModel).settings;
				AmbientOcclusionModel.Settings settings3 = (_delta as AmbientOcclusionModel).settings;
				settings3.blurSize = Mathf.Lerp(settings.blurSize, settings2.blurSize, _progress);
				settings3.distanceFalloff = Mathf.Lerp(settings.distanceFalloff, settings2.distanceFalloff, _progress);
				settings3.intensity = Mathf.Lerp(settings.intensity, settings2.intensity, _progress);
				settings3.radius = Mathf.Lerp(settings.radius, settings2.radius, _progress);
				settings3.downsampling = ((_progress >= 0.5f) ? settings2.downsampling : settings.downsampling);
				settings3.RotationTexture = ((_progress >= 0.5f) ? settings2.RotationTexture : settings.RotationTexture);
				AmbientOcclusionModel.SampleCount[] array = new AmbientOcclusionModel.SampleCount[]
				{
					AmbientOcclusionModel.SampleCount.Lowest,
					AmbientOcclusionModel.SampleCount.Low,
					AmbientOcclusionModel.SampleCount.Medium,
					AmbientOcclusionModel.SampleCount.High
				};
				int valueArrayIndex = PostProcessingBehaviourUtils.GetValueArrayIndex<AmbientOcclusionModel.SampleCount>(array, settings.sampleCount);
				int valueArrayIndex2 = PostProcessingBehaviourUtils.GetValueArrayIndex<AmbientOcclusionModel.SampleCount>(array, settings2.sampleCount);
				settings3.sampleCount = array[(int)Mathf.Lerp((float)valueArrayIndex, (float)valueArrayIndex2, _progress)];
				(_delta as AmbientOcclusionModel).settings = settings3;
			}

			// Token: 0x06003C92 RID: 15506 RVA: 0x00121856 File Offset: 0x0011FC56
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.ambientOcclusion;
			}

			// Token: 0x06003C93 RID: 15507 RVA: 0x00121860 File Offset: 0x0011FC60
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				AmbientOcclusionModel ambientOcclusionModel = new AmbientOcclusionModel();
				AmbientOcclusionModel.Settings settings = (_state as AmbientOcclusionModel).settings;
				settings.intensity = 0f;
				ambientOcclusionModel.settings = settings;
				return ambientOcclusionModel;
			}
		}

		// Token: 0x02000B8B RID: 2955
		private class AntialiasingLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000411 RID: 1041
			// (get) Token: 0x06003C95 RID: 15509 RVA: 0x0012189B File Offset: 0x0011FC9B
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Antialiasing;
				}
			}

			// Token: 0x06003C96 RID: 15510 RVA: 0x001218A0 File Offset: 0x0011FCA0
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				AntialiasingModel.Settings settings = (_start as AntialiasingModel).settings;
				AntialiasingModel.Settings settings2 = (_end as AntialiasingModel).settings;
				AntialiasingModel.Settings settings3 = (_delta as AntialiasingModel).settings;
				if (settings.method == settings2.method)
				{
					settings3.method = settings2.method;
					AntialiasingModel.Method method = settings3.method;
					if (method != AntialiasingModel.Method.Fxaa)
					{
						if (method == AntialiasingModel.Method.Taa)
						{
							AntialiasingModel.TaaSettings taaSettings = (_progress >= 0.5f) ? settings2.taaSettings : settings.taaSettings;
							settings3.taaSettings.jitterSpread = Mathf.Lerp(settings.taaSettings.jitterSpread, settings2.taaSettings.jitterSpread, _progress);
							settings3.taaSettings.motionBlending = Mathf.Lerp(settings.taaSettings.motionBlending, settings2.taaSettings.motionBlending, _progress);
							settings3.taaSettings.sharpen = Mathf.Lerp(settings.taaSettings.sharpen, settings2.taaSettings.sharpen, _progress);
							settings3.taaSettings.stationaryBlending = Mathf.Lerp(settings.taaSettings.stationaryBlending, settings2.taaSettings.stationaryBlending, _progress);
						}
					}
					else
					{
						settings3.fxaaSettings.preset = ((_progress >= 0.5f) ? settings2.fxaaSettings : settings.fxaaSettings).preset;
					}
				}
				else
				{
					settings3.method = ((_progress >= 0.5f) ? settings2.method : settings.method);
					AntialiasingModel.Method method2 = settings3.method;
					if (method2 != AntialiasingModel.Method.Fxaa)
					{
						if (method2 == AntialiasingModel.Method.Taa)
						{
							AntialiasingModel.TaaSettings taaSettings2 = (_progress >= 0.5f) ? settings2.taaSettings : settings.taaSettings;
							settings3.taaSettings.jitterSpread = taaSettings2.jitterSpread;
							settings3.taaSettings.motionBlending = taaSettings2.motionBlending;
							settings3.taaSettings.sharpen = taaSettings2.sharpen;
							settings3.taaSettings.stationaryBlending = taaSettings2.stationaryBlending;
						}
					}
					else
					{
						settings3.fxaaSettings.preset = ((_progress >= 0.5f) ? settings2.fxaaSettings : settings.fxaaSettings).preset;
					}
				}
				(_delta as AntialiasingModel).settings = settings3;
			}

			// Token: 0x06003C97 RID: 15511 RVA: 0x00121B18 File Offset: 0x0011FF18
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.antialiasing;
			}

			// Token: 0x06003C98 RID: 15512 RVA: 0x00121B20 File Offset: 0x0011FF20
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				AntialiasingModel antialiasingModel = new AntialiasingModel();
				AntialiasingModel.Settings settings = (_state as AntialiasingModel).settings;
				antialiasingModel.settings = settings;
				return antialiasingModel;
			}
		}

		// Token: 0x02000B8C RID: 2956
		private class BloomLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000412 RID: 1042
			// (get) Token: 0x06003C9A RID: 15514 RVA: 0x00121B4F File Offset: 0x0011FF4F
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Bloom;
				}
			}

			// Token: 0x06003C9B RID: 15515 RVA: 0x00121B54 File Offset: 0x0011FF54
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				BloomModel.Settings settings = (_start as BloomModel).settings;
				BloomModel.Settings settings2 = (_end as BloomModel).settings;
				BloomModel.Settings settings3 = (_delta as BloomModel).settings;
				settings3.bloom.antiFlicker = ((_progress >= 0.5f) ? settings2.bloom.antiFlicker : settings.bloom.antiFlicker);
				settings3.bloom.intensity = Mathf.Lerp(settings.bloom.intensity, settings2.bloom.intensity, _progress);
				settings3.bloom.radius = Mathf.Lerp(settings.bloom.radius, settings2.bloom.radius, _progress);
				settings3.bloom.softKnee = Mathf.Lerp(settings.bloom.softKnee, settings2.bloom.softKnee, _progress);
				settings3.bloom.threshold = Mathf.Lerp(settings.bloom.threshold, settings2.bloom.threshold, _progress);
				settings3.bloom.thresholdLinear = Mathf.Lerp(settings.bloom.thresholdLinear, settings2.bloom.thresholdLinear, _progress);
				settings3.lensDirt.intensity = Mathf.Lerp(settings.lensDirt.intensity, settings2.lensDirt.intensity, _progress);
				settings3.lensDirt.texture = ((_progress >= 0.5f) ? settings2.lensDirt.texture : settings.lensDirt.texture);
				(_delta as BloomModel).settings = settings3;
			}

			// Token: 0x06003C9C RID: 15516 RVA: 0x00121CFF File Offset: 0x001200FF
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.bloom;
			}

			// Token: 0x06003C9D RID: 15517 RVA: 0x00121D08 File Offset: 0x00120108
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				BloomModel bloomModel = new BloomModel();
				BloomModel.Settings settings = (_state as BloomModel).settings;
				settings.bloom.intensity = 0f;
				settings.lensDirt.intensity = 0f;
				bloomModel.settings = settings;
				return bloomModel;
			}
		}

		// Token: 0x02000B8D RID: 2957
		private class ChromaticAberrationLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000413 RID: 1043
			// (get) Token: 0x06003C9F RID: 15519 RVA: 0x00121D59 File Offset: 0x00120159
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.ChromaticAberration;
				}
			}

			// Token: 0x06003CA0 RID: 15520 RVA: 0x00121D5C File Offset: 0x0012015C
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				ChromaticAberrationModel.Settings settings = (_start as ChromaticAberrationModel).settings;
				ChromaticAberrationModel.Settings settings2 = (_end as ChromaticAberrationModel).settings;
				ChromaticAberrationModel.Settings settings3 = (_delta as ChromaticAberrationModel).settings;
				settings3.intensity = Mathf.Lerp(settings.intensity, settings2.intensity, _progress);
				settings3.spectralTexture = ((_progress >= 0.5f) ? settings2.spectralTexture : settings.spectralTexture);
				(_delta as ChromaticAberrationModel).settings = settings3;
			}

			// Token: 0x06003CA1 RID: 15521 RVA: 0x00121DDD File Offset: 0x001201DD
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.chromaticAberration;
			}

			// Token: 0x06003CA2 RID: 15522 RVA: 0x00121DE8 File Offset: 0x001201E8
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				ChromaticAberrationModel chromaticAberrationModel = new ChromaticAberrationModel();
				ChromaticAberrationModel.Settings settings = (_state as ChromaticAberrationModel).settings;
				settings.intensity = 0f;
				chromaticAberrationModel.settings = settings;
				return chromaticAberrationModel;
			}
		}

		// Token: 0x02000B8E RID: 2958
		private class ColorGradingLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000414 RID: 1044
			// (get) Token: 0x06003CA4 RID: 15524 RVA: 0x00121E23 File Offset: 0x00120223
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.ColorGrading;
				}
			}

			// Token: 0x06003CA5 RID: 15525 RVA: 0x00121E28 File Offset: 0x00120228
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				ColorGradingModel.Settings settings = (_start as ColorGradingModel).settings;
				ColorGradingModel.Settings settings2 = (_end as ColorGradingModel).settings;
				ColorGradingModel.Settings settings3 = (_delta as ColorGradingModel).settings;
				settings3.basic.contrast = Mathf.Lerp(settings.basic.contrast, settings2.basic.contrast, _progress);
				settings3.basic.hueShift = Mathf.Lerp(settings.basic.hueShift, settings2.basic.hueShift, _progress);
				settings3.basic.postExposure = Mathf.Lerp(settings.basic.postExposure, settings2.basic.postExposure, _progress);
				settings3.basic.saturation = Mathf.Lerp(settings.basic.saturation, settings2.basic.saturation, _progress);
				settings3.basic.temperature = Mathf.Lerp(settings.basic.temperature, settings2.basic.temperature, _progress);
				settings3.basic.tint = Mathf.Lerp(settings.basic.tint, settings2.basic.tint, _progress);
				settings3.channelMixer.red = Vector3.Lerp(settings.channelMixer.red, settings2.channelMixer.red, _progress);
				settings3.channelMixer.green = Vector3.Lerp(settings.channelMixer.green, settings2.channelMixer.green, _progress);
				settings3.channelMixer.blue = Vector3.Lerp(settings.channelMixer.blue, settings2.channelMixer.blue, _progress);
				if (settings.colorWheels.mode == settings2.colorWheels.mode)
				{
					settings3.colorWheels.mode = settings2.colorWheels.mode;
					ColorGradingModel.ColorWheelMode mode = settings3.colorWheels.mode;
					if (mode != ColorGradingModel.ColorWheelMode.Linear)
					{
						if (mode == ColorGradingModel.ColorWheelMode.Log)
						{
							settings3.colorWheels.log.offset = Color.Lerp(settings.colorWheels.log.offset, settings2.colorWheels.log.offset, _progress);
							settings3.colorWheels.log.power = Color.Lerp(settings.colorWheels.log.power, settings2.colorWheels.log.power, _progress);
							settings3.colorWheels.log.slope = Color.Lerp(settings.colorWheels.log.slope, settings2.colorWheels.log.slope, _progress);
						}
					}
					else
					{
						settings3.colorWheels.linear.gain = Color.Lerp(settings.colorWheels.linear.gain, settings2.colorWheels.linear.gain, _progress);
						settings3.colorWheels.linear.gamma = Color.Lerp(settings.colorWheels.linear.gamma, settings2.colorWheels.linear.gamma, _progress);
						settings3.colorWheels.linear.lift = Color.Lerp(settings.colorWheels.linear.lift, settings2.colorWheels.linear.lift, _progress);
					}
				}
				else
				{
					settings3.colorWheels.mode = ((_progress >= 0.5f) ? settings2.colorWheels.mode : settings.colorWheels.mode);
					ColorGradingModel.ColorWheelMode mode2 = settings3.colorWheels.mode;
					if (mode2 != ColorGradingModel.ColorWheelMode.Linear)
					{
						if (mode2 == ColorGradingModel.ColorWheelMode.Log)
						{
							ColorGradingModel.LogWheelsSettings logWheelsSettings = (_progress >= 0.5f) ? settings2.colorWheels.log : settings.colorWheels.log;
							settings3.colorWheels.log.offset = logWheelsSettings.offset;
							settings3.colorWheels.log.power = logWheelsSettings.power;
							settings3.colorWheels.log.slope = logWheelsSettings.slope;
						}
					}
					else
					{
						ColorGradingModel.LinearWheelsSettings linearWheelsSettings = (_progress >= 0.5f) ? settings2.colorWheels.linear : settings.colorWheels.linear;
						settings3.colorWheels.linear.gain = linearWheelsSettings.gain;
						settings3.colorWheels.linear.gamma = linearWheelsSettings.gamma;
						settings3.colorWheels.linear.lift = linearWheelsSettings.lift;
					}
				}
				settings3.curves.red = ((_progress >= 0.5f) ? settings2.curves.red : settings.curves.red);
				settings3.curves.green = ((_progress >= 0.5f) ? settings2.curves.green : settings.curves.green);
				settings3.curves.blue = ((_progress >= 0.5f) ? settings2.curves.blue : settings.curves.blue);
				settings3.curves.hueVShue = ((_progress >= 0.5f) ? settings2.curves.hueVShue : settings.curves.hueVShue);
				settings3.curves.hueVSsat = ((_progress >= 0.5f) ? settings2.curves.hueVSsat : settings.curves.hueVSsat);
				settings3.curves.lumVSsat = ((_progress >= 0.5f) ? settings2.curves.lumVSsat : settings.curves.lumVSsat);
				settings3.curves.master = ((_progress >= 0.5f) ? settings2.curves.master : settings.curves.master);
				settings3.curves.satVSsat = ((_progress >= 0.5f) ? settings2.curves.satVSsat : settings.curves.satVSsat);
				settings3.tonemapping.tonemapper = ((_progress >= 0.5f) ? settings2.tonemapping.tonemapper : settings.tonemapping.tonemapper);
				settings3.tonemapping.neutralBlackIn = Mathf.Lerp(settings.tonemapping.neutralBlackIn, settings2.tonemapping.neutralBlackIn, _progress);
				settings3.tonemapping.neutralBlackOut = Mathf.Lerp(settings.tonemapping.neutralBlackOut, settings2.tonemapping.neutralBlackOut, _progress);
				settings3.tonemapping.neutralWhiteClip = Mathf.Lerp(settings.tonemapping.neutralWhiteClip, settings2.tonemapping.neutralWhiteClip, _progress);
				settings3.tonemapping.neutralWhiteIn = Mathf.Lerp(settings.tonemapping.neutralWhiteIn, settings2.tonemapping.neutralWhiteIn, _progress);
				settings3.tonemapping.neutralWhiteLevel = Mathf.Lerp(settings.tonemapping.neutralWhiteLevel, settings2.tonemapping.neutralWhiteLevel, _progress);
				settings3.tonemapping.neutralWhiteOut = Mathf.Lerp(settings.tonemapping.neutralWhiteOut, settings2.tonemapping.neutralWhiteOut, _progress);
				(_delta as ColorGradingModel).settings = settings3;
			}

			// Token: 0x06003CA6 RID: 15526 RVA: 0x001225CF File Offset: 0x001209CF
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.colorGrading;
			}

			// Token: 0x06003CA7 RID: 15527 RVA: 0x001225D8 File Offset: 0x001209D8
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				ColorGradingModel colorGradingModel = new ColorGradingModel();
				ColorGradingModel.Settings settings = (_state as ColorGradingModel).settings;
				colorGradingModel.settings = settings;
				return colorGradingModel;
			}
		}

		// Token: 0x02000B8F RID: 2959
		private class DepthOfFieldLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000415 RID: 1045
			// (get) Token: 0x06003CA9 RID: 15529 RVA: 0x00122607 File Offset: 0x00120A07
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.DepthOfField;
				}
			}

			// Token: 0x06003CAA RID: 15530 RVA: 0x0012260C File Offset: 0x00120A0C
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				DepthOfFieldModel.Settings settings = (_start as DepthOfFieldModel).settings;
				DepthOfFieldModel.Settings settings2 = (_end as DepthOfFieldModel).settings;
				DepthOfFieldModel.Settings settings3 = (_delta as DepthOfFieldModel).settings;
				settings3.useCameraFov = ((_progress >= 0.5f) ? settings2.useCameraFov : settings.useCameraFov);
				settings3.aperture = Mathf.Lerp(settings.aperture, settings2.aperture, _progress);
				settings3.focalLength = Mathf.Lerp(settings.focalLength, settings2.focalLength, _progress);
				settings3.focusDistance = Mathf.Lerp(settings.focusDistance, settings2.focusDistance, _progress);
				DepthOfFieldModel.KernelSize[] array = new DepthOfFieldModel.KernelSize[]
				{
					DepthOfFieldModel.KernelSize.Small,
					DepthOfFieldModel.KernelSize.Medium,
					DepthOfFieldModel.KernelSize.Large,
					DepthOfFieldModel.KernelSize.VeryLarge
				};
				int valueArrayIndex = PostProcessingBehaviourUtils.GetValueArrayIndex<DepthOfFieldModel.KernelSize>(array, settings.kernelSize);
				int valueArrayIndex2 = PostProcessingBehaviourUtils.GetValueArrayIndex<DepthOfFieldModel.KernelSize>(array, settings2.kernelSize);
				settings3.kernelSize = array[(int)Mathf.Lerp((float)valueArrayIndex, (float)valueArrayIndex2, _progress)];
				(_delta as DepthOfFieldModel).settings = settings3;
			}

			// Token: 0x06003CAB RID: 15531 RVA: 0x0012270C File Offset: 0x00120B0C
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.depthOfField;
			}

			// Token: 0x06003CAC RID: 15532 RVA: 0x00122714 File Offset: 0x00120B14
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				DepthOfFieldModel depthOfFieldModel = new DepthOfFieldModel();
				DepthOfFieldModel.Settings settings = (_state as DepthOfFieldModel).settings;
				settings.focalLength = 1f;
				depthOfFieldModel.settings = settings;
				return depthOfFieldModel;
			}
		}

		// Token: 0x02000B90 RID: 2960
		private class DitheringLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000416 RID: 1046
			// (get) Token: 0x06003CAE RID: 15534 RVA: 0x0012274F File Offset: 0x00120B4F
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Dithering;
				}
			}

			// Token: 0x06003CAF RID: 15535 RVA: 0x00122752 File Offset: 0x00120B52
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
			}

			// Token: 0x06003CB0 RID: 15536 RVA: 0x00122754 File Offset: 0x00120B54
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.dithering;
			}

			// Token: 0x06003CB1 RID: 15537 RVA: 0x0012275C File Offset: 0x00120B5C
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				DitheringModel ditheringModel = new DitheringModel();
				DitheringModel.Settings settings = (_state as DitheringModel).settings;
				ditheringModel.settings = settings;
				return ditheringModel;
			}
		}

		// Token: 0x02000B91 RID: 2961
		private class EyeAdaptionLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x0012278B File Offset: 0x00120B8B
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.EyeAdaptation;
				}
			}

			// Token: 0x06003CB4 RID: 15540 RVA: 0x00122790 File Offset: 0x00120B90
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				EyeAdaptationModel.Settings settings = (_start as EyeAdaptationModel).settings;
				EyeAdaptationModel.Settings settings2 = (_end as EyeAdaptationModel).settings;
				EyeAdaptationModel.Settings settings3 = (_delta as EyeAdaptationModel).settings;
				settings3.adaptationType = ((_progress >= 0.5f) ? settings2.adaptationType : settings.adaptationType);
				settings3.dynamicKeyValue = ((_progress >= 0.5f) ? settings2.dynamicKeyValue : settings.dynamicKeyValue);
				settings3.highPercent = Mathf.Lerp(settings.highPercent, settings2.highPercent, _progress);
				settings3.keyValue = Mathf.Lerp(settings.keyValue, settings2.keyValue, _progress);
				settings3.logMax = (int)Mathf.Lerp((float)settings.logMax, (float)settings2.logMax, _progress);
				settings3.logMin = (int)Mathf.Lerp((float)settings.logMin, (float)settings2.logMin, _progress);
				settings3.lowPercent = Mathf.Lerp(settings.lowPercent, settings2.lowPercent, _progress);
				settings3.maxLuminance = Mathf.Lerp(settings.maxLuminance, settings2.maxLuminance, _progress);
				settings3.minLuminance = Mathf.Lerp(settings.minLuminance, settings2.minLuminance, _progress);
				settings3.speedDown = Mathf.Lerp(settings.speedDown, settings2.speedDown, _progress);
				settings3.speedUp = Mathf.Lerp(settings.speedUp, settings2.speedUp, _progress);
				(_delta as EyeAdaptationModel).settings = settings3;
			}

			// Token: 0x06003CB5 RID: 15541 RVA: 0x0012291D File Offset: 0x00120D1D
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.eyeAdaptation;
			}

			// Token: 0x06003CB6 RID: 15542 RVA: 0x00122928 File Offset: 0x00120D28
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				EyeAdaptationModel eyeAdaptationModel = new EyeAdaptationModel();
				EyeAdaptationModel.Settings settings = (_state as EyeAdaptationModel).settings;
				eyeAdaptationModel.settings = settings;
				return eyeAdaptationModel;
			}
		}

		// Token: 0x02000B92 RID: 2962
		private class FogLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x00122957 File Offset: 0x00120D57
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Fog;
				}
			}

			// Token: 0x06003CB9 RID: 15545 RVA: 0x0012295C File Offset: 0x00120D5C
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				FogModel.Settings settings = (_start as FogModel).settings;
				FogModel.Settings settings2 = (_end as FogModel).settings;
				FogModel.Settings settings3 = (_delta as FogModel).settings;
				settings3.excludeSkybox = ((_progress >= 0.5f) ? settings2.excludeSkybox : settings.excludeSkybox);
				(_delta as FogModel).settings = settings3;
			}

			// Token: 0x06003CBA RID: 15546 RVA: 0x001229C1 File Offset: 0x00120DC1
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.fog;
			}

			// Token: 0x06003CBB RID: 15547 RVA: 0x001229CC File Offset: 0x00120DCC
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				FogModel fogModel = new FogModel();
				FogModel.Settings settings = (_state as FogModel).settings;
				fogModel.settings = settings;
				return fogModel;
			}
		}

		// Token: 0x02000B93 RID: 2963
		private class GrainLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06003CBD RID: 15549 RVA: 0x001229FB File Offset: 0x00120DFB
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Grain;
				}
			}

			// Token: 0x06003CBE RID: 15550 RVA: 0x00122A00 File Offset: 0x00120E00
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				GrainModel.Settings settings = (_start as GrainModel).settings;
				GrainModel.Settings settings2 = (_end as GrainModel).settings;
				GrainModel.Settings settings3 = (_delta as GrainModel).settings;
				settings3.colored = ((_progress >= 0.5f) ? settings2.colored : settings.colored);
				settings3.intensity = Mathf.Lerp(settings.intensity, settings2.intensity, _progress);
				settings3.luminanceContribution = Mathf.Lerp(settings.luminanceContribution, settings2.luminanceContribution, _progress);
				settings3.size = Mathf.Lerp(settings.size, settings2.size, _progress);
				(_delta as GrainModel).settings = settings3;
			}

			// Token: 0x06003CBF RID: 15551 RVA: 0x00122AB9 File Offset: 0x00120EB9
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.grain;
			}

			// Token: 0x06003CC0 RID: 15552 RVA: 0x00122AC4 File Offset: 0x00120EC4
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				GrainModel grainModel = new GrainModel();
				GrainModel.Settings settings = (_state as GrainModel).settings;
				settings.intensity = 0f;
				grainModel.settings = settings;
				return grainModel;
			}
		}

		// Token: 0x02000B94 RID: 2964
		private class MotionBlurLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x06003CC2 RID: 15554 RVA: 0x00122AFF File Offset: 0x00120EFF
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.MotionBlur;
				}
			}

			// Token: 0x06003CC3 RID: 15555 RVA: 0x00122B04 File Offset: 0x00120F04
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				MotionBlurModel.Settings settings = (_start as MotionBlurModel).settings;
				MotionBlurModel.Settings settings2 = (_end as MotionBlurModel).settings;
				MotionBlurModel.Settings settings3 = (_delta as MotionBlurModel).settings;
				settings3.frameBlending = ((_progress >= 0.5f) ? settings2.frameBlending : settings.frameBlending);
				settings3.sampleCount = (int)Mathf.Lerp((float)settings.sampleCount, (float)settings2.sampleCount, _progress);
				settings3.shutterAngle = Mathf.Lerp(settings.shutterAngle, settings2.shutterAngle, _progress);
				(_delta as MotionBlurModel).settings = settings3;
			}

			// Token: 0x06003CC4 RID: 15556 RVA: 0x00122BA4 File Offset: 0x00120FA4
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.motionBlur;
			}

			// Token: 0x06003CC5 RID: 15557 RVA: 0x00122BAC File Offset: 0x00120FAC
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				MotionBlurModel motionBlurModel = new MotionBlurModel();
				MotionBlurModel.Settings settings = (_state as MotionBlurModel).settings;
				settings.frameBlending = 0f;
				motionBlurModel.settings = settings;
				return motionBlurModel;
			}
		}

		// Token: 0x02000B95 RID: 2965
		private class ScreenSpaceReflectionLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x00122BE7 File Offset: 0x00120FE7
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.ScreenSpaceReflection;
				}
			}

			// Token: 0x06003CC8 RID: 15560 RVA: 0x00122BEC File Offset: 0x00120FEC
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				ScreenSpaceReflectionModel.Settings settings = (_start as ScreenSpaceReflectionModel).settings;
				ScreenSpaceReflectionModel.Settings settings2 = (_end as ScreenSpaceReflectionModel).settings;
				ScreenSpaceReflectionModel.Settings settings3 = (_delta as ScreenSpaceReflectionModel).settings;
				settings3.intensity.fadeDistance = Mathf.Lerp(settings.intensity.fadeDistance, settings2.intensity.fadeDistance, _progress);
				settings3.intensity.fresnelFade = Mathf.Lerp(settings.intensity.fresnelFade, settings2.intensity.fresnelFade, _progress);
				settings3.intensity.fresnelFadePower = Mathf.Lerp(settings.intensity.fresnelFadePower, settings2.intensity.fresnelFadePower, _progress);
				settings3.intensity.reflectionMultiplier = Mathf.Lerp(settings.intensity.reflectionMultiplier, settings2.intensity.reflectionMultiplier, _progress);
				settings3.reflection.blendType = ((_progress >= 0.5f) ? settings2.reflection.blendType : settings.reflection.blendType);
				settings3.reflection.iterationCount = (int)Mathf.Lerp((float)settings.reflection.iterationCount, (float)settings2.reflection.iterationCount, _progress);
				settings3.reflection.maxDistance = Mathf.Lerp(settings.reflection.maxDistance, settings2.reflection.maxDistance, _progress);
				settings3.reflection.reflectBackfaces = ((_progress >= 0.5f) ? settings2.reflection.reflectBackfaces : settings.reflection.reflectBackfaces);
				settings3.reflection.reflectionBlur = Mathf.Lerp(settings.reflection.reflectionBlur, settings2.reflection.reflectionBlur, _progress);
				ScreenSpaceReflectionModel.SSRResolution[] array = new ScreenSpaceReflectionModel.SSRResolution[2];
				array[0] = ScreenSpaceReflectionModel.SSRResolution.Low;
				ScreenSpaceReflectionModel.SSRResolution[] array2 = array;
				int valueArrayIndex = PostProcessingBehaviourUtils.GetValueArrayIndex<ScreenSpaceReflectionModel.SSRResolution>(array2, settings.reflection.reflectionQuality);
				int valueArrayIndex2 = PostProcessingBehaviourUtils.GetValueArrayIndex<ScreenSpaceReflectionModel.SSRResolution>(array2, settings2.reflection.reflectionQuality);
				settings3.reflection.reflectionQuality = array2[(int)Mathf.Lerp((float)valueArrayIndex, (float)valueArrayIndex2, _progress)];
				settings3.reflection.stepSize = (int)Mathf.Lerp((float)settings.reflection.stepSize, (float)settings2.reflection.stepSize, _progress);
				settings3.reflection.widthModifier = Mathf.Lerp(settings.reflection.widthModifier, settings2.reflection.widthModifier, _progress);
				settings3.screenEdgeMask.intensity = Mathf.Lerp(settings.screenEdgeMask.intensity, settings2.screenEdgeMask.intensity, _progress);
				(_delta as ScreenSpaceReflectionModel).settings = settings3;
			}

			// Token: 0x06003CC9 RID: 15561 RVA: 0x00122E98 File Offset: 0x00121298
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.screenSpaceReflection;
			}

			// Token: 0x06003CCA RID: 15562 RVA: 0x00122EA0 File Offset: 0x001212A0
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				ScreenSpaceReflectionModel screenSpaceReflectionModel = new ScreenSpaceReflectionModel();
				ScreenSpaceReflectionModel.Settings settings = (_state as ScreenSpaceReflectionModel).settings;
				settings.intensity.reflectionMultiplier = 0f;
				screenSpaceReflectionModel.settings = settings;
				return screenSpaceReflectionModel;
			}
		}

		// Token: 0x02000B96 RID: 2966
		private class TiltShiftLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x06003CCC RID: 15564 RVA: 0x00122EE0 File Offset: 0x001212E0
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.TiltShift;
				}
			}

			// Token: 0x06003CCD RID: 15565 RVA: 0x00122EE4 File Offset: 0x001212E4
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				TiltShiftModel.Settings settings = (_start as TiltShiftModel).settings;
				TiltShiftModel.Settings settings2 = (_end as TiltShiftModel).settings;
				TiltShiftModel.Settings settings3 = (_delta as TiltShiftModel).settings;
				settings3.BlurArea = Mathf.Lerp(settings.BlurArea, settings2.BlurArea, _progress);
				settings3.Downsample = ((_progress >= 0.5f) ? settings2.Downsample : settings.Downsample);
				settings3.Iterations = (int)Mathf.Lerp((float)settings.Iterations, (float)settings2.Iterations, _progress);
				(_delta as TiltShiftModel).settings = settings3;
			}

			// Token: 0x06003CCE RID: 15566 RVA: 0x00122F84 File Offset: 0x00121384
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.tiltshift;
			}

			// Token: 0x06003CCF RID: 15567 RVA: 0x00122F8C File Offset: 0x0012138C
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				TiltShiftModel tiltShiftModel = new TiltShiftModel();
				TiltShiftModel.Settings settings = (_state as TiltShiftModel).settings;
				settings.Iterations = 0;
				tiltShiftModel.settings = settings;
				return tiltShiftModel;
			}
		}

		// Token: 0x02000B97 RID: 2967
		private class UserLutLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x00122FC3 File Offset: 0x001213C3
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.UserLut;
				}
			}

			// Token: 0x06003CD2 RID: 15570 RVA: 0x00122FC8 File Offset: 0x001213C8
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				UserLutModel.Settings settings = (_start as UserLutModel).settings;
				UserLutModel.Settings settings2 = (_end as UserLutModel).settings;
				UserLutModel.Settings settings3 = (_delta as UserLutModel).settings;
				settings3.contribution = Mathf.Lerp(settings.contribution, settings2.contribution, _progress);
				settings3.lut = ((_progress >= 0.5f) ? settings2.lut : settings.lut);
				(_delta as UserLutModel).settings = settings3;
			}

			// Token: 0x06003CD3 RID: 15571 RVA: 0x00123049 File Offset: 0x00121449
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.userLut;
			}

			// Token: 0x06003CD4 RID: 15572 RVA: 0x00123054 File Offset: 0x00121454
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				UserLutModel userLutModel = new UserLutModel();
				UserLutModel.Settings settings = (_state as UserLutModel).settings;
				settings.contribution = 0f;
				userLutModel.settings = settings;
				return userLutModel;
			}

			// Token: 0x04003100 RID: 12544
			private Material mat;
		}

		// Token: 0x02000B98 RID: 2968
		private class VignetteLerper : PostProcessingBehaviourUtils.Lerper
		{
			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x06003CD6 RID: 15574 RVA: 0x0012308F File Offset: 0x0012148F
			public override PostProcessingBehaviourUtils.LerpFilter LerpTarget
			{
				get
				{
					return PostProcessingBehaviourUtils.LerpFilter.Vignette;
				}
			}

			// Token: 0x06003CD7 RID: 15575 RVA: 0x00123094 File Offset: 0x00121494
			protected override void LerpValues(PostProcessingModel _start, PostProcessingModel _end, ref PostProcessingModel _delta, float _progress, PostProcessingBehaviourUtils.Lerper.LerpType _type)
			{
				VignetteModel.Settings settings = (_start as VignetteModel).settings;
				VignetteModel.Settings settings2 = (_end as VignetteModel).settings;
				VignetteModel.Settings settings3 = (_delta as VignetteModel).settings;
				settings3.center = Vector3.Lerp(settings.center, settings2.center, _progress);
				settings3.color = Color.Lerp(settings.color, settings2.color, _progress);
				settings3.intensity = Mathf.Lerp(settings.intensity, settings2.intensity, _progress);
				settings3.mask = ((_progress >= 0.5f) ? settings2.mask : settings.mask);
				settings3.mode = ((_progress >= 0.5f) ? settings2.mode : settings.mode);
				settings3.opacity = Mathf.Lerp(settings.opacity, settings2.opacity, _progress);
				settings3.rounded = ((_progress >= 0.5f) ? settings2.rounded : settings.rounded);
				settings3.roundness = Mathf.Lerp(settings.roundness, settings2.roundness, _progress);
				settings3.smoothness = Mathf.Lerp(settings.smoothness, settings2.smoothness, _progress);
				(_delta as VignetteModel).settings = settings3;
			}

			// Token: 0x06003CD8 RID: 15576 RVA: 0x001231FC File Offset: 0x001215FC
			public override PostProcessingModel GetRequiredModel(PostProcessingProfile _profile)
			{
				return _profile.vignette;
			}

			// Token: 0x06003CD9 RID: 15577 RVA: 0x00123204 File Offset: 0x00121604
			protected override PostProcessingModel CreateOffState(PostProcessingModel _state)
			{
				VignetteModel vignetteModel = new VignetteModel();
				VignetteModel.Settings settings = (_state as VignetteModel).settings;
				settings.intensity = 0f;
				settings.opacity = 0f;
				vignetteModel.settings = settings;
				return vignetteModel;
			}
		}
	}
}
