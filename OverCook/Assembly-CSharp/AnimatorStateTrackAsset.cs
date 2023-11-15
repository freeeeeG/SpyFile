using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000658 RID: 1624
[TrackBindingType(typeof(Animator))]
[TrackClipType(typeof(AnimatorStatePlayableAsset))]
[TrackColor(0.86f, 0.24f, 0.1f)]
public class AnimatorStateTrackAsset : TrackAsset
{
	// Token: 0x06001EF2 RID: 7922 RVA: 0x000975E9 File Offset: 0x000959E9
	protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
	{
		return base.CreatePlayable(graph, go, clip);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x000975F4 File Offset: 0x000959F4
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return base.CreateTrackMixer(graph, go, inputCount);
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000975FF File Offset: 0x000959FF
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x00097609 File Offset: 0x00095A09
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x00097611 File Offset: 0x00095A11
	public override void OnEnable()
	{
		base.OnEnable();
	}
}
