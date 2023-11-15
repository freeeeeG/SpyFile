using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000655 RID: 1621
[TrackClipType(typeof(AmbiencePlayableAsset))]
[TrackColor(0.16f, 0.41f, 0.45f)]
public class AmbienceTrackAsset : TrackAsset
{
	// Token: 0x06001EE2 RID: 7906 RVA: 0x0009739E File Offset: 0x0009579E
	protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
	{
		return base.CreatePlayable(graph, go, clip);
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x000973A9 File Offset: 0x000957A9
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return base.CreateTrackMixer(graph, go, inputCount);
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x000973B4 File Offset: 0x000957B4
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000973BE File Offset: 0x000957BE
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000973C6 File Offset: 0x000957C6
	public override void OnEnable()
	{
		base.OnEnable();
	}
}
