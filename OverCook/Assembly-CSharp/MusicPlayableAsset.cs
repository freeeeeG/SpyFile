using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000667 RID: 1639
[Serializable]
public class MusicPlayableAsset : PlayableAsset
{
	// Token: 0x06001F42 RID: 8002 RVA: 0x00098C48 File Offset: 0x00097048
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<MusicPlayableBehaviour> playable = ScriptPlayable<MusicPlayableBehaviour>.Create(graph, 0);
		MusicPlayableBehaviour behaviour = playable.GetBehaviour();
		AudioClip clip = this.m_audioFile.Resolve(playable.GetGraph<ScriptPlayable<MusicPlayableBehaviour>>().GetResolver());
		behaviour.Setup(clip, this.m_killPrevious);
		return playable;
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06001F43 RID: 8003 RVA: 0x00098C92 File Offset: 0x00097092
	public override double duration
	{
		get
		{
			return base.duration;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06001F44 RID: 8004 RVA: 0x00098C9A File Offset: 0x0009709A
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x040017E1 RID: 6113
	[SerializeField]
	private ExposedReference<AudioClip> m_audioFile;

	// Token: 0x040017E2 RID: 6114
	[SerializeField]
	private bool m_killPrevious;
}
