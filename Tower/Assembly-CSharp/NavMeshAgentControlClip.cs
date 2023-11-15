using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200000A RID: 10
[Serializable]
public class NavMeshAgentControlClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000015 RID: 21 RVA: 0x00002734 File Offset: 0x00000934
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.None;
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002738 File Offset: 0x00000938
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<NavMeshAgentControlBehaviour> playable = ScriptPlayable<NavMeshAgentControlBehaviour>.Create(graph, this.template, 0);
		playable.GetBehaviour().destination = this.destination.Resolve(graph.GetResolver());
		return playable;
	}

	// Token: 0x04000023 RID: 35
	public ExposedReference<Transform> destination;

	// Token: 0x04000024 RID: 36
	[HideInInspector]
	public NavMeshAgentControlBehaviour template = new NavMeshAgentControlBehaviour();
}
