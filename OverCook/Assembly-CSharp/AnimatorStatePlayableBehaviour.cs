using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000657 RID: 1623
public class AnimatorStatePlayableBehaviour : PlayableBehaviour
{
	// Token: 0x06001EED RID: 7917 RVA: 0x0009747F File Offset: 0x0009587F
	public void Setup(string _variableName, AnimatorVariableType _variableType, object _value)
	{
		this.m_variableNameHash = Animator.StringToHash(_variableName);
		this.m_variableType = _variableType;
		this.m_variableValue = _value;
	}

	// Token: 0x06001EEE RID: 7918 RVA: 0x0009749B File Offset: 0x0009589B
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback)
		{
			this.m_pending = true;
		}
	}

	// Token: 0x06001EEF RID: 7919 RVA: 0x000974C4 File Offset: 0x000958C4
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		base.OnBehaviourPause(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback && this.m_targetAnimator != null && this.m_originalValue != null)
		{
			AnimatorUtils.SetValue(this.m_targetAnimator, this.m_variableNameHash, this.m_variableType, this.m_originalValue);
		}
		this.m_pending = false;
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x00097534 File Offset: 0x00095934
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback)
		{
			if (this.m_targetAnimator == null)
			{
				this.m_targetAnimator = (playerData as Animator);
			}
			if (this.m_pending && this.m_targetAnimator != null)
			{
				this.m_originalValue = AnimatorUtils.GetValue(this.m_targetAnimator, this.m_variableNameHash, this.m_variableType);
				AnimatorUtils.SetValue(this.m_targetAnimator, this.m_variableNameHash, this.m_variableType, this.m_variableValue);
				if (info.seekOccurred)
				{
				}
			}
		}
	}

	// Token: 0x040017AF RID: 6063
	private AnimatorVariableType m_variableType;

	// Token: 0x040017B0 RID: 6064
	private int m_variableNameHash;

	// Token: 0x040017B1 RID: 6065
	private object m_variableValue;

	// Token: 0x040017B2 RID: 6066
	private Animator m_targetAnimator;

	// Token: 0x040017B3 RID: 6067
	private bool m_pending;

	// Token: 0x040017B4 RID: 6068
	private object m_originalValue;
}
