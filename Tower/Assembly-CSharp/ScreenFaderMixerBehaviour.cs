using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public class ScreenFaderMixerBehaviour : PlayableBehaviour
{
	// Token: 0x06000020 RID: 32 RVA: 0x00002878 File Offset: 0x00000A78
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		this.m_TrackBinding = (playerData as Image);
		if (this.m_TrackBinding == null)
		{
			return;
		}
		if (!this.m_FirstFrameHappened)
		{
			this.m_DefaultColor = this.m_TrackBinding.color;
			this.m_FirstFrameHappened = true;
		}
		int inputCount = playable.GetInputCount<Playable>();
		Color a = Color.clear;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			ScreenFaderBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour();
			a += behaviour.color * inputWeight;
			num += inputWeight;
			if (inputWeight > num2)
			{
				num2 = inputWeight;
			}
			if (!Mathf.Approximately(inputWeight, 0f))
			{
				num3++;
			}
		}
		this.m_TrackBinding.color = a + this.m_DefaultColor * (1f - num);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002967 File Offset: 0x00000B67
	public override void OnPlayableDestroy(Playable playable)
	{
		this.m_FirstFrameHappened = false;
		if (this.m_TrackBinding == null)
		{
			return;
		}
		this.m_TrackBinding.color = this.m_DefaultColor;
	}

	// Token: 0x04000027 RID: 39
	private Color m_DefaultColor;

	// Token: 0x04000028 RID: 40
	private Image m_TrackBinding;

	// Token: 0x04000029 RID: 41
	private bool m_FirstFrameHappened;
}
