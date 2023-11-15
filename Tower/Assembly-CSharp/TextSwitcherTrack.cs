using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
[TrackColor(0.1394896f, 0.4411765f, 0.3413077f)]
[TrackClipType(typeof(TextSwitcherClip))]
[TrackBindingType(typeof(Text))]
public class TextSwitcherTrack : TrackAsset
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002BE2 File Offset: 0x00000DE2
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<TextSwitcherMixerBehaviour>.Create(graph, inputCount);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002BF0 File Offset: 0x00000DF0
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}
}
