using System;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000057 RID: 87
	public class Envelope : IAmplitudeController
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x00016299 File Offset: 0x00014499
		public Envelope(Envelope.EnvelopeParams pars, float initialTargetAmplitude, Envelope.EnvelopeControlMode controlMode)
		{
			this.pars = pars;
			this.controlMode = controlMode;
			this.SetTarget(initialTargetAmplitude);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x000162B6 File Offset: 0x000144B6
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x000162BE File Offset: 0x000144BE
		public float Intensity { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600043C RID: 1084 RVA: 0x000162C7 File Offset: 0x000144C7
		public bool IsFinished
		{
			get
			{
				return this.finishImmediately || ((this.finishWhenAmplitudeZero || this.controlMode == Envelope.EnvelopeControlMode.Auto) && this.amplitude <= 0f && this.targetAmplitude <= 0f);
			}
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00016302 File Offset: 0x00014502
		public void Finish()
		{
			this.finishWhenAmplitudeZero = true;
			this.SetTarget(0f);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00016316 File Offset: 0x00014516
		public void FinishImmediately()
		{
			this.finishImmediately = true;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00016320 File Offset: 0x00014520
		public void Update(float deltaTime)
		{
			if (this.IsFinished)
			{
				return;
			}
			if (this.state == Envelope.EnvelopeState.Increase)
			{
				if (this.pars.attack > 0f)
				{
					this.amplitude += deltaTime * this.pars.attack;
				}
				if (this.amplitude > this.targetAmplitude || this.pars.attack <= 0f)
				{
					this.amplitude = this.targetAmplitude;
					this.state = Envelope.EnvelopeState.Sustain;
					if (this.controlMode == Envelope.EnvelopeControlMode.Auto)
					{
						this.sustainEndTime = Time.time + this.pars.sustain;
					}
				}
			}
			else if (this.state == Envelope.EnvelopeState.Decrease)
			{
				if (this.pars.decay > 0f)
				{
					this.amplitude -= deltaTime * this.pars.decay;
				}
				if (this.amplitude < this.targetAmplitude || this.pars.decay <= 0f)
				{
					this.amplitude = this.targetAmplitude;
					this.state = Envelope.EnvelopeState.Sustain;
				}
			}
			else if (this.controlMode == Envelope.EnvelopeControlMode.Auto && Time.time > this.sustainEndTime)
			{
				this.SetTarget(0f);
			}
			this.amplitude = Mathf.Clamp01(this.amplitude);
			this.Intensity = Power.Evaluate(this.amplitude, this.pars.degree);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00016482 File Offset: 0x00014682
		public void SetTargetAmplitude(float value)
		{
			if (this.controlMode == Envelope.EnvelopeControlMode.Manual && !this.finishWhenAmplitudeZero)
			{
				this.SetTarget(value);
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001649C File Offset: 0x0001469C
		private void SetTarget(float value)
		{
			this.targetAmplitude = Mathf.Clamp01(value);
			this.state = ((this.targetAmplitude > this.amplitude) ? Envelope.EnvelopeState.Increase : Envelope.EnvelopeState.Decrease);
		}

		// Token: 0x04000230 RID: 560
		private readonly Envelope.EnvelopeParams pars;

		// Token: 0x04000231 RID: 561
		private readonly Envelope.EnvelopeControlMode controlMode;

		// Token: 0x04000232 RID: 562
		private float amplitude;

		// Token: 0x04000233 RID: 563
		private float targetAmplitude;

		// Token: 0x04000234 RID: 564
		private float sustainEndTime;

		// Token: 0x04000235 RID: 565
		private bool finishWhenAmplitudeZero;

		// Token: 0x04000236 RID: 566
		private bool finishImmediately;

		// Token: 0x04000237 RID: 567
		private Envelope.EnvelopeState state;

		// Token: 0x02000290 RID: 656
		[Serializable]
		public class EnvelopeParams
		{
			// Token: 0x04000A27 RID: 2599
			[Tooltip("How fast the amplitude increases.")]
			public float attack = 10f;

			// Token: 0x04000A28 RID: 2600
			[Tooltip("How long in seconds the amplitude holds maximum value.")]
			public float sustain;

			// Token: 0x04000A29 RID: 2601
			[Tooltip("How fast the amplitude decreases.")]
			public float decay = 1f;

			// Token: 0x04000A2A RID: 2602
			[Tooltip("Power in which the amplitude is raised to get intensity.")]
			public Degree degree = Degree.Cubic;
		}

		// Token: 0x02000291 RID: 657
		public enum EnvelopeControlMode
		{
			// Token: 0x04000A2C RID: 2604
			Auto,
			// Token: 0x04000A2D RID: 2605
			Manual
		}

		// Token: 0x02000292 RID: 658
		public enum EnvelopeState
		{
			// Token: 0x04000A2F RID: 2607
			Sustain,
			// Token: 0x04000A30 RID: 2608
			Increase,
			// Token: 0x04000A31 RID: 2609
			Decrease
		}
	}
}
