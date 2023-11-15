using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000054 RID: 84
	public class CameraShakePresets
	{
		// Token: 0x0600041F RID: 1055 RVA: 0x00015D11 File Offset: 0x00013F11
		public CameraShakePresets(CameraShaker shaker)
		{
			this.shaker = shaker;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00015D20 File Offset: 0x00013F20
		public void ShortShake2D(float positionStrength = 0.08f, float rotationStrength = 0.1f, float freq = 25f, int numBounces = 5)
		{
			BounceShake.Params parameters = new BounceShake.Params
			{
				positionStrength = positionStrength,
				rotationStrength = rotationStrength,
				freq = freq,
				numBounces = numBounces
			};
			this.shaker.RegisterShake(new BounceShake(parameters, null));
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00015D6C File Offset: 0x00013F6C
		public void ShortShake3D(float strength = 0.3f, float freq = 25f, int numBounces = 5)
		{
			BounceShake.Params parameters = new BounceShake.Params
			{
				axesMultiplier = new Displacement(Vector3.zero, new Vector3(1f, 1f, 0.4f)),
				rotationStrength = strength,
				freq = freq,
				numBounces = numBounces
			};
			this.shaker.RegisterShake(new BounceShake(parameters, null));
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00015DD4 File Offset: 0x00013FD4
		public void Explosion2D(float positionStrength = 1f, float rotationStrength = 3f, float duration = 0.5f)
		{
			PerlinShake.NoiseMode[] noiseModes = new PerlinShake.NoiseMode[]
			{
				new PerlinShake.NoiseMode(8f, 1f),
				new PerlinShake.NoiseMode(20f, 0.3f)
			};
			Envelope.EnvelopeParams envelopeParams = new Envelope.EnvelopeParams();
			envelopeParams.decay = ((duration <= 0f) ? 1f : (1f / duration));
			PerlinShake.Params parameters = new PerlinShake.Params
			{
				strength = new Displacement(new Vector3(1f, 1f) * positionStrength, Vector3.forward * rotationStrength),
				noiseModes = noiseModes,
				envelope = envelopeParams
			};
			this.shaker.RegisterShake(new PerlinShake(parameters, 1f, null, false));
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00015E98 File Offset: 0x00014098
		public void Explosion3D(float strength = 8f, float duration = 0.7f)
		{
			PerlinShake.NoiseMode[] noiseModes = new PerlinShake.NoiseMode[]
			{
				new PerlinShake.NoiseMode(6f, 1f),
				new PerlinShake.NoiseMode(20f, 0.2f)
			};
			Envelope.EnvelopeParams envelopeParams = new Envelope.EnvelopeParams();
			envelopeParams.decay = ((duration <= 0f) ? 1f : (1f / duration));
			PerlinShake.Params parameters = new PerlinShake.Params
			{
				strength = new Displacement(Vector3.zero, new Vector3(1f, 1f, 0.5f) * strength),
				noiseModes = noiseModes,
				envelope = envelopeParams
			};
			this.shaker.RegisterShake(new PerlinShake(parameters, 1f, null, false));
		}

		// Token: 0x04000226 RID: 550
		private readonly CameraShaker shaker;
	}
}
