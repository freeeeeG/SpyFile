using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x0200065A RID: 1626
public class AudioTriggerPlayableBehaviour : PlayableBehaviour
{
	// Token: 0x06001EFC RID: 7932 RVA: 0x0009767E File Offset: 0x00095A7E
	public void Setup(GameOneShotAudioTag _tag)
	{
		this.m_tag = _tag;
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x00097688 File Offset: 0x00095A88
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.m_tag != GameOneShotAudioTag.COUNT && info.weight > 0f)
		{
			PlayableDirector playableDirector = playable.GetGraph<Playable>().GetResolver() as PlayableDirector;
			GameUtils.TriggerAudio(this.m_tag, playableDirector.gameObject.layer);
		}
	}

	// Token: 0x040017B6 RID: 6070
	private GameOneShotAudioTag m_tag = GameOneShotAudioTag.COUNT;
}
