using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000007 RID: 7
public class LightControlMixerBehaviour : PlayableBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x000024D4 File Offset: 0x000006D4
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		this.m_TrackBinding = (playerData as Light);
		if (this.m_TrackBinding == null)
		{
			return;
		}
		if (!this.m_FirstFrameHappened)
		{
			this.m_DefaultColor = this.m_TrackBinding.color;
			this.m_DefaultIntensity = this.m_TrackBinding.intensity;
			this.m_DefaultBounceIntensity = this.m_TrackBinding.bounceIntensity;
			this.m_DefaultRange = this.m_TrackBinding.range;
			this.m_FirstFrameHappened = true;
		}
		int inputCount = playable.GetInputCount<Playable>();
		Color a = Color.clear;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		int num6 = 0;
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			LightControlBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour();
			a += behaviour.color * inputWeight;
			num += behaviour.intensity * inputWeight;
			num2 += behaviour.bounceIntensity * inputWeight;
			num3 += behaviour.range * inputWeight;
			num4 += inputWeight;
			if (inputWeight > num5)
			{
				num5 = inputWeight;
			}
			if (!Mathf.Approximately(inputWeight, 0f))
			{
				num6++;
			}
		}
		this.m_TrackBinding.color = a + this.m_DefaultColor * (1f - num4);
		this.m_TrackBinding.intensity = num + this.m_DefaultIntensity * (1f - num4);
		this.m_TrackBinding.bounceIntensity = num2 + this.m_DefaultBounceIntensity * (1f - num4);
		this.m_TrackBinding.range = num3 + this.m_DefaultRange * (1f - num4);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002694 File Offset: 0x00000894
	public override void OnPlayableDestroy(Playable playable)
	{
		this.m_FirstFrameHappened = false;
		if (this.m_TrackBinding == null)
		{
			return;
		}
		this.m_TrackBinding.color = this.m_DefaultColor;
		this.m_TrackBinding.intensity = this.m_DefaultIntensity;
		this.m_TrackBinding.bounceIntensity = this.m_DefaultBounceIntensity;
		this.m_TrackBinding.range = this.m_DefaultRange;
	}

	// Token: 0x0400001B RID: 27
	private Color m_DefaultColor;

	// Token: 0x0400001C RID: 28
	private float m_DefaultIntensity;

	// Token: 0x0400001D RID: 29
	private float m_DefaultBounceIntensity;

	// Token: 0x0400001E RID: 30
	private float m_DefaultRange;

	// Token: 0x0400001F RID: 31
	private Light m_TrackBinding;

	// Token: 0x04000020 RID: 32
	private bool m_FirstFrameHappened;
}
