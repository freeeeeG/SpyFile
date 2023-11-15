using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000016 RID: 22
[Serializable]
public class TimeDilationClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000031 RID: 49 RVA: 0x00002C15 File Offset: 0x00000E15
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.Extrapolation | ClipCaps.Blending;
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002C19 File Offset: 0x00000E19
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<TimeDilationBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x04000034 RID: 52
	public TimeDilationBehaviour template = new TimeDilationBehaviour();
}
