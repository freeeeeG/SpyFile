using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000006 RID: 6
[Serializable]
public class LightControlClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600000A RID: 10 RVA: 0x000024A7 File Offset: 0x000006A7
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.Blending;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000024AB File Offset: 0x000006AB
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<LightControlBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x0400001A RID: 26
	public LightControlBehaviour template = new LightControlBehaviour();
}
