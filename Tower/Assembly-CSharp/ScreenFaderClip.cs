using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200000E RID: 14
[Serializable]
public class ScreenFaderClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600001D RID: 29 RVA: 0x0000284B File Offset: 0x00000A4B
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.Blending;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x0000284F File Offset: 0x00000A4F
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<ScreenFaderBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x04000026 RID: 38
	public ScreenFaderBehaviour template = new ScreenFaderBehaviour();
}
