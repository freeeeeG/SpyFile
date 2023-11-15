using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200065B RID: 1627
[TrackClipType(typeof(AudioTriggerPlayableAsset))]
[TrackColor(0.24f, 0.42f, 0.78f)]
public class AudioTriggerTrackAsset : TrackAsset
{
	// Token: 0x06001EFF RID: 7935 RVA: 0x000976FC File Offset: 0x00095AFC
	protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
	{
		return base.CreatePlayable(graph, go, clip);
	}

	// Token: 0x06001F00 RID: 7936 RVA: 0x00097707 File Offset: 0x00095B07
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return base.CreateTrackMixer(graph, go, inputCount);
	}

	// Token: 0x06001F01 RID: 7937 RVA: 0x00097712 File Offset: 0x00095B12
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06001F02 RID: 7938 RVA: 0x0009771C File Offset: 0x00095B1C
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x06001F03 RID: 7939 RVA: 0x00097724 File Offset: 0x00095B24
	public override void OnEnable()
	{
		base.OnEnable();
	}
}
