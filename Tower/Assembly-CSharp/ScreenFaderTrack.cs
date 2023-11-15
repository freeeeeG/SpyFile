using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

// Token: 0x02000010 RID: 16
[TrackColor(0.875f, 0.5944853f, 0.1737132f)]
[TrackClipType(typeof(ScreenFaderClip))]
[TrackBindingType(typeof(Image))]
public class ScreenFaderTrack : TrackAsset
{
	// Token: 0x06000023 RID: 35 RVA: 0x00002998 File Offset: 0x00000B98
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<ScreenFaderMixerBehaviour>.Create(graph, inputCount);
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000029A6 File Offset: 0x00000BA6
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}
}
