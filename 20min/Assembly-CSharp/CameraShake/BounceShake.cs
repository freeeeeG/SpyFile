using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000053 RID: 83
	public class BounceShake : ICameraShake
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x00015A30 File Offset: 0x00013C30
		public BounceShake(BounceShake.Params parameters, Vector3? sourcePosition = null)
		{
			this.sourcePosition = sourcePosition;
			this.pars = parameters;
			Displacement a = Displacement.InsideUnitSpheres();
			this.direction = Displacement.Scale(a, this.pars.axesMultiplier).Normalized;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00015AA0 File Offset: 0x00013CA0
		public BounceShake(BounceShake.Params parameters, Displacement initialDirection, Vector3? sourcePosition = null)
		{
			this.sourcePosition = sourcePosition;
			this.pars = parameters;
			this.direction = Displacement.Scale(initialDirection, this.pars.axesMultiplier).Normalized;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00015B0A File Offset: 0x00013D0A
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00015B12 File Offset: 0x00013D12
		public Displacement CurrentDisplacement { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00015B1B File Offset: 0x00013D1B
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00015B23 File Offset: 0x00013D23
		public bool IsFinished { get; private set; }

		// Token: 0x0600041D RID: 1053 RVA: 0x00015B2C File Offset: 0x00013D2C
		public void Initialize(Vector3 cameraPosition, Quaternion cameraRotation)
		{
			this.attenuation = ((this.sourcePosition == null) ? 1f : Attenuator.Strength(this.pars.attenuation, this.sourcePosition.Value, cameraPosition));
			this.currentWaypoint = this.attenuation * this.direction.ScaledBy(this.pars.positionStrength, this.pars.rotationStrength);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00015BA4 File Offset: 0x00013DA4
		public void Update(float deltaTime, Vector3 cameraPosition, Quaternion cameraRotation)
		{
			if (this.t < 1f)
			{
				this.t += deltaTime * this.pars.freq;
				if (this.pars.freq == 0f)
				{
					this.t = 1f;
				}
				this.CurrentDisplacement = Displacement.Lerp(this.previousWaypoint, this.currentWaypoint, this.moveCurve.Evaluate(this.t));
				return;
			}
			this.t = 0f;
			this.CurrentDisplacement = this.currentWaypoint;
			this.previousWaypoint = this.currentWaypoint;
			this.bounceIndex++;
			if (this.bounceIndex > this.pars.numBounces)
			{
				this.IsFinished = true;
				return;
			}
			Displacement a = Displacement.InsideUnitSpheres();
			this.direction = -this.direction + this.pars.randomness * Displacement.Scale(a, this.pars.axesMultiplier).Normalized;
			this.direction = this.direction.Normalized;
			float num = 1f - (float)this.bounceIndex / (float)this.pars.numBounces;
			this.currentWaypoint = num * num * this.attenuation * this.direction.ScaledBy(this.pars.positionStrength, this.pars.rotationStrength);
		}

		// Token: 0x0400021B RID: 539
		private readonly BounceShake.Params pars;

		// Token: 0x0400021C RID: 540
		private readonly AnimationCurve moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x0400021D RID: 541
		private readonly Vector3? sourcePosition;

		// Token: 0x0400021E RID: 542
		private float attenuation = 1f;

		// Token: 0x0400021F RID: 543
		private Displacement direction;

		// Token: 0x04000220 RID: 544
		private Displacement previousWaypoint;

		// Token: 0x04000221 RID: 545
		private Displacement currentWaypoint;

		// Token: 0x04000222 RID: 546
		private int bounceIndex;

		// Token: 0x04000223 RID: 547
		private float t;

		// Token: 0x0200028F RID: 655
		[Serializable]
		public class Params
		{
			// Token: 0x04000A20 RID: 2592
			[Tooltip("Strength of the shake for positional axes.")]
			public float positionStrength = 0.05f;

			// Token: 0x04000A21 RID: 2593
			[Tooltip("Strength of the shake for rotational axes.")]
			public float rotationStrength = 0.1f;

			// Token: 0x04000A22 RID: 2594
			[Tooltip("Preferred direction of shaking.")]
			public Displacement axesMultiplier = new Displacement(Vector2.one, Vector3.forward);

			// Token: 0x04000A23 RID: 2595
			[Tooltip("Frequency of shaking.")]
			public float freq = 25f;

			// Token: 0x04000A24 RID: 2596
			[Tooltip("Number of vibrations before stop.")]
			public int numBounces = 5;

			// Token: 0x04000A25 RID: 2597
			[Range(0f, 1f)]
			[Tooltip("Randomness of motion.")]
			public float randomness = 0.5f;

			// Token: 0x04000A26 RID: 2598
			[Tooltip("How strength falls with distance from the shake source.")]
			public Attenuator.StrengthAttenuationParams attenuation;
		}
	}
}
