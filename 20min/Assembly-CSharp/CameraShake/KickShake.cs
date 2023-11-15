using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x0200005A RID: 90
	public class KickShake : ICameraShake
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x000164C2 File Offset: 0x000146C2
		public KickShake(KickShake.Params parameters, Vector3 sourcePosition, bool attenuateStrength)
		{
			this.pars = parameters;
			this.sourcePosition = new Vector3?(sourcePosition);
			this.attenuateStrength = attenuateStrength;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000164E4 File Offset: 0x000146E4
		public KickShake(KickShake.Params parameters, Displacement direction)
		{
			this.pars = parameters;
			this.direction = direction.Normalized;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00016500 File Offset: 0x00014700
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x00016508 File Offset: 0x00014708
		public Displacement CurrentDisplacement { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00016511 File Offset: 0x00014711
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00016519 File Offset: 0x00014719
		public bool IsFinished { get; private set; }

		// Token: 0x0600044F RID: 1103 RVA: 0x00016524 File Offset: 0x00014724
		public void Initialize(Vector3 cameraPosition, Quaternion cameraRotation)
		{
			if (this.sourcePosition != null)
			{
				this.direction = Attenuator.Direction(this.sourcePosition.Value, cameraPosition, cameraRotation);
				if (this.attenuateStrength)
				{
					this.direction *= Attenuator.Strength(this.pars.attenuation, this.sourcePosition.Value, cameraPosition);
				}
			}
			this.currentWaypoint = Displacement.Scale(this.direction, this.pars.strength);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000165A8 File Offset: 0x000147A8
		public void Update(float deltaTime, Vector3 cameraPosition, Quaternion cameraRotation)
		{
			if (this.t < 1f)
			{
				this.Move(deltaTime, this.release ? this.pars.releaseTime : this.pars.attackTime, this.release ? this.pars.releaseCurve : this.pars.attackCurve);
				return;
			}
			this.CurrentDisplacement = this.currentWaypoint;
			this.prevWaypoint = this.currentWaypoint;
			if (this.release)
			{
				this.IsFinished = true;
				return;
			}
			this.release = true;
			this.t = 0f;
			this.currentWaypoint = Displacement.Zero;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00016650 File Offset: 0x00014850
		private void Move(float deltaTime, float duration, AnimationCurve curve)
		{
			if (duration > 0f)
			{
				this.t += deltaTime / duration;
			}
			else
			{
				this.t = 1f;
			}
			this.CurrentDisplacement = Displacement.Lerp(this.prevWaypoint, this.currentWaypoint, curve.Evaluate(this.t));
		}

		// Token: 0x04000239 RID: 569
		private readonly KickShake.Params pars;

		// Token: 0x0400023A RID: 570
		private readonly Vector3? sourcePosition;

		// Token: 0x0400023B RID: 571
		private readonly bool attenuateStrength;

		// Token: 0x0400023C RID: 572
		private Displacement direction;

		// Token: 0x0400023D RID: 573
		private Displacement prevWaypoint;

		// Token: 0x0400023E RID: 574
		private Displacement currentWaypoint;

		// Token: 0x0400023F RID: 575
		private bool release;

		// Token: 0x04000240 RID: 576
		private float t;

		// Token: 0x02000293 RID: 659
		[Serializable]
		public class Params
		{
			// Token: 0x04000A32 RID: 2610
			[Tooltip("Strength of the shake for each axis.")]
			public Displacement strength = new Displacement(Vector3.zero, Vector3.one);

			// Token: 0x04000A33 RID: 2611
			[Tooltip("How long it takes to move forward.")]
			public float attackTime = 0.05f;

			// Token: 0x04000A34 RID: 2612
			public AnimationCurve attackCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

			// Token: 0x04000A35 RID: 2613
			[Tooltip("How long it takes to move back.")]
			public float releaseTime = 0.2f;

			// Token: 0x04000A36 RID: 2614
			public AnimationCurve releaseCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

			// Token: 0x04000A37 RID: 2615
			[Tooltip("How strength falls with distance from the shake source.")]
			public Attenuator.StrengthAttenuationParams attenuation;
		}
	}
}
