using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000017 RID: 23
public class TimeDilationMixerBehaviour : PlayableBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x00002C40 File Offset: 0x00000E40
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		int inputCount = playable.GetInputCount<Playable>();
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			if (inputWeight > 0f)
			{
				num3++;
			}
			num2 += inputWeight;
			TimeDilationBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour();
			num += inputWeight * behaviour.timeScale;
		}
		Time.timeScale = num + this.defaultTimeScale * (1f - num2);
		if (num3 == 0)
		{
			Time.timeScale = this.defaultTimeScale;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002CD6 File Offset: 0x00000ED6
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		Time.timeScale = this.defaultTimeScale;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002CE3 File Offset: 0x00000EE3
	public override void OnGraphStop(Playable playable)
	{
		Time.timeScale = this.defaultTimeScale;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002CF0 File Offset: 0x00000EF0
	public override void OnPlayableDestroy(Playable playable)
	{
		Time.timeScale = this.defaultTimeScale;
	}

	// Token: 0x04000035 RID: 53
	private readonly float defaultTimeScale = 1f;
}
