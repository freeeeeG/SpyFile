using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000669 RID: 1641
[TrackClipType(typeof(MusicPlayableAsset))]
[TrackColor(0.1f, 0.4f, 0.1f)]
public class MusicTrackAsset : TrackAsset
{
	// Token: 0x06001F4A RID: 8010 RVA: 0x00098D8F File Offset: 0x0009718F
	protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
	{
		return base.CreatePlayable(graph, go, clip);
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x00098D9A File Offset: 0x0009719A
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return base.CreateTrackMixer(graph, go, inputCount);
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x00098DA5 File Offset: 0x000971A5
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06001F4D RID: 8013 RVA: 0x00098DAF File Offset: 0x000971AF
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x00098DB7 File Offset: 0x000971B7
	public override void OnEnable()
	{
		base.OnEnable();
	}
}
