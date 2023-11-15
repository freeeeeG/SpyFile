using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000668 RID: 1640
public class MusicPlayableBehaviour : PlayableBehaviour
{
	// Token: 0x06001F46 RID: 8006 RVA: 0x00098CAA File Offset: 0x000970AA
	public void Setup(AudioClip _clip, bool _killPrevious)
	{
		this.m_audioFile = _clip;
		this.m_killPrevious = _killPrevious;
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x00098CBC File Offset: 0x000970BC
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback && this.m_audioFile != null && info.weight > 0f)
		{
			this.m_audioManager = GameUtils.RequireManager<CampaignAudioManager>();
			this.m_audioManager.SetMusic(this.m_audioFile, this.m_killPrevious);
		}
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x00098D2C File Offset: 0x0009712C
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		base.OnBehaviourPause(playable, info);
		if (Application.isPlaying && info.evaluationType == FrameData.EvaluationType.Playback && this.m_audioFile != null && this.m_audioManager != null)
		{
			this.m_audioManager.SetMusic(null, false);
		}
	}

	// Token: 0x040017E3 RID: 6115
	private CampaignAudioManager m_audioManager;

	// Token: 0x040017E4 RID: 6116
	private AudioClip m_audioFile;

	// Token: 0x040017E5 RID: 6117
	private bool m_killPrevious;
}
