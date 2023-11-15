using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x0200005B RID: 91
	public class PerlinShake : ICameraShake
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x000166A8 File Offset: 0x000148A8
		public PerlinShake(PerlinShake.Params parameters, float maxAmplitude = 1f, Vector3? sourcePosition = null, bool manualStrengthControl = false)
		{
			this.pars = parameters;
			this.envelope = new Envelope(this.pars.envelope, maxAmplitude, manualStrengthControl ? Envelope.EnvelopeControlMode.Manual : Envelope.EnvelopeControlMode.Auto);
			this.AmplitudeController = this.envelope;
			this.sourcePosition = sourcePosition;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x000166F4 File Offset: 0x000148F4
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x000166FC File Offset: 0x000148FC
		public Displacement CurrentDisplacement { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00016705 File Offset: 0x00014905
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0001670D File Offset: 0x0001490D
		public bool IsFinished { get; private set; }

		// Token: 0x06000457 RID: 1111 RVA: 0x00016718 File Offset: 0x00014918
		public void Initialize(Vector3 cameraPosition, Quaternion cameraRotation)
		{
			this.seeds = new Vector2[this.pars.noiseModes.Length];
			this.norm = 0f;
			for (int i = 0; i < this.seeds.Length; i++)
			{
				this.seeds[i] = Random.insideUnitCircle * 20f;
				this.norm += this.pars.noiseModes[i].amplitude;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001679C File Offset: 0x0001499C
		public void Update(float deltaTime, Vector3 cameraPosition, Quaternion cameraRotation)
		{
			if (this.envelope.IsFinished)
			{
				this.IsFinished = true;
				return;
			}
			this.time += deltaTime;
			this.envelope.Update(deltaTime);
			Displacement a = Displacement.Zero;
			for (int i = 0; i < this.pars.noiseModes.Length; i++)
			{
				a += this.pars.noiseModes[i].amplitude / this.norm * this.SampleNoise(this.seeds[i], this.pars.noiseModes[i].freq);
			}
			this.CurrentDisplacement = this.envelope.Intensity * Displacement.Scale(a, this.pars.strength);
			if (this.sourcePosition != null)
			{
				this.CurrentDisplacement *= Attenuator.Strength(this.pars.attenuation, this.sourcePosition.Value, cameraPosition);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000168A8 File Offset: 0x00014AA8
		private Displacement SampleNoise(Vector2 seed, float freq)
		{
			Vector3 position = new Vector3(Mathf.PerlinNoise(seed.x + this.time * freq, seed.y), Mathf.PerlinNoise(seed.x, seed.y + this.time * freq), Mathf.PerlinNoise(seed.x + this.time * freq, seed.y + this.time * freq)) - Vector3.one * 0.5f;
			Vector3 vector = new Vector3(Mathf.PerlinNoise(-seed.x - this.time * freq, -seed.y), Mathf.PerlinNoise(-seed.x, -seed.y - this.time * freq), Mathf.PerlinNoise(-seed.x - this.time * freq, -seed.y - this.time * freq));
			vector -= Vector3.one * 0.5f;
			return new Displacement(position, vector);
		}

		// Token: 0x04000243 RID: 579
		private readonly PerlinShake.Params pars;

		// Token: 0x04000244 RID: 580
		private readonly Envelope envelope;

		// Token: 0x04000245 RID: 581
		public IAmplitudeController AmplitudeController;

		// Token: 0x04000246 RID: 582
		private Vector2[] seeds;

		// Token: 0x04000247 RID: 583
		private float time;

		// Token: 0x04000248 RID: 584
		private Vector3? sourcePosition;

		// Token: 0x04000249 RID: 585
		private float norm;

		// Token: 0x02000294 RID: 660
		[Serializable]
		public class Params
		{
			// Token: 0x04000A38 RID: 2616
			[Tooltip("Strength of the shake for each axis.")]
			public Displacement strength = new Displacement(Vector3.zero, new Vector3(2f, 2f, 0.8f));

			// Token: 0x04000A39 RID: 2617
			[Tooltip("Layers of perlin noise with different frequencies.")]
			public PerlinShake.NoiseMode[] noiseModes = new PerlinShake.NoiseMode[]
			{
				new PerlinShake.NoiseMode(12f, 1f)
			};

			// Token: 0x04000A3A RID: 2618
			[Tooltip("Strength of the shake over time.")]
			public Envelope.EnvelopeParams envelope;

			// Token: 0x04000A3B RID: 2619
			[Tooltip("How strength falls with distance from the shake source.")]
			public Attenuator.StrengthAttenuationParams attenuation;
		}

		// Token: 0x02000295 RID: 661
		[Serializable]
		public struct NoiseMode
		{
			// Token: 0x06000DD6 RID: 3542 RVA: 0x00032B9D File Offset: 0x00030D9D
			public NoiseMode(float freq, float amplitude)
			{
				this.freq = freq;
				this.amplitude = amplitude;
			}

			// Token: 0x04000A3C RID: 2620
			[Tooltip("Frequency multiplier for the noise.")]
			public float freq;

			// Token: 0x04000A3D RID: 2621
			[Tooltip("Amplitude of the mode.")]
			[Range(0f, 1f)]
			public float amplitude;
		}
	}
}
