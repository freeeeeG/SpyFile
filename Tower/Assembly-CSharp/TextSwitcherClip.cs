using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000012 RID: 18
[Serializable]
public class TextSwitcherClip : PlayableAsset, ITimelineClipAsset
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000027 RID: 39 RVA: 0x000029D3 File Offset: 0x00000BD3
	public ClipCaps clipCaps
	{
		get
		{
			return ClipCaps.Blending;
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000029D7 File Offset: 0x00000BD7
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
	{
		return ScriptPlayable<TextSwitcherBehaviour>.Create(graph, this.template, 0);
	}

	// Token: 0x0400002D RID: 45
	public TextSwitcherBehaviour template = new TextSwitcherBehaviour();
}
