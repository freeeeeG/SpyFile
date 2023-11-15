using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000659 RID: 1625
[Serializable]
public class AudioTriggerPlayableAsset : PlayableAsset
{
	// Token: 0x06001EF8 RID: 7928 RVA: 0x0009762C File Offset: 0x00095A2C
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<AudioTriggerPlayableBehaviour> playable = ScriptPlayable<AudioTriggerPlayableBehaviour>.Create(graph, 0);
		AudioTriggerPlayableBehaviour behaviour = playable.GetBehaviour();
		behaviour.Setup(this.m_tag);
		return playable;
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001EF9 RID: 7929 RVA: 0x0009765B File Offset: 0x00095A5B
	public override double duration
	{
		get
		{
			return base.duration;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06001EFA RID: 7930 RVA: 0x00097663 File Offset: 0x00095A63
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x040017B5 RID: 6069
	[SerializeField]
	private GameOneShotAudioTag m_tag = GameOneShotAudioTag.COUNT;
}
