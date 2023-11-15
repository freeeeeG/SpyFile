using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000653 RID: 1619
[Serializable]
public class AmbiencePlayableAsset : PlayableAsset
{
	// Token: 0x06001EDA RID: 7898 RVA: 0x00097280 File Offset: 0x00095680
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
	{
		ScriptPlayable<AmbiencePlayableBehaviour> playable = ScriptPlayable<AmbiencePlayableBehaviour>.Create(graph, 0);
		AmbiencePlayableBehaviour behaviour = playable.GetBehaviour();
		behaviour.Setup(this.m_tag);
		return playable;
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000972AF File Offset: 0x000956AF
	public override double duration
	{
		get
		{
			return base.duration;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06001EDC RID: 7900 RVA: 0x000972B7 File Offset: 0x000956B7
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x040017A7 RID: 6055
	[SerializeField]
	private GameLoopingAudioTag m_tag = GameLoopingAudioTag.COUNT;
}
