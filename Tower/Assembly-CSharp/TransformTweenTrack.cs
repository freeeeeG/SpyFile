using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200001C RID: 28
[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(TransformTweenClip))]
[TrackBindingType(typeof(Transform))]
public class TransformTweenTrack : TrackAsset
{
	// Token: 0x06000049 RID: 73 RVA: 0x00003289 File Offset: 0x00001489
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<TransformTweenMixerBehaviour>.Create(graph, inputCount);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003297 File Offset: 0x00001497
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}
}
