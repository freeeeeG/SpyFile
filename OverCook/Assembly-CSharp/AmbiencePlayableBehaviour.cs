using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000654 RID: 1620
public class AmbiencePlayableBehaviour : PlayableBehaviour
{
	// Token: 0x06001EDE RID: 7902 RVA: 0x000972D2 File Offset: 0x000956D2
	public void Setup(GameLoopingAudioTag _tag)
	{
		this.m_tag = _tag;
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x000972DC File Offset: 0x000956DC
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback && this.m_tag != GameLoopingAudioTag.COUNT && info.weight > 0f)
		{
			this.m_audioManager = GameUtils.RequireManager<CampaignAudioManager>();
			this.m_audioManager.StartAmbience(this.m_tag);
		}
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x00097348 File Offset: 0x00095748
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		base.OnBehaviourPause(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback && this.m_audioManager != null)
		{
			this.m_audioManager.StopAmbience(this.m_tag);
		}
	}

	// Token: 0x040017A8 RID: 6056
	private CampaignAudioManager m_audioManager;

	// Token: 0x040017A9 RID: 6057
	private GameLoopingAudioTag m_tag = GameLoopingAudioTag.COUNT;
}
