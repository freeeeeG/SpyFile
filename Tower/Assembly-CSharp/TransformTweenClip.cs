using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200001A RID: 26
[Serializable]
public class TransformTweenClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600003F RID: 63 RVA: 0x00002F5A File Offset: 0x0000115A
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.Blending;
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002F60 File Offset: 0x00001160
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		ScriptPlayable<TransformTweenBehaviour> playable = ScriptPlayable<TransformTweenBehaviour>.Create(graph, this.template, 0);
		TransformTweenBehaviour behaviour = playable.GetBehaviour();
		behaviour.startLocation = this.startLocation.Resolve(graph.GetResolver());
		behaviour.endLocation = this.endLocation.Resolve(graph.GetResolver());
		return playable;
	}

	// Token: 0x04000042 RID: 66
	public TransformTweenBehaviour template = new TransformTweenBehaviour();

	// Token: 0x04000043 RID: 67
	public ExposedReference<Transform> startLocation;

	// Token: 0x04000044 RID: 68
	public ExposedReference<Transform> endLocation;
}
