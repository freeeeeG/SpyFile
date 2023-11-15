using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000018 RID: 24
[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(TimeDilationClip))]
public class TimeDilationTrack : TrackAsset
{
	// Token: 0x06000039 RID: 57 RVA: 0x00002D10 File Offset: 0x00000F10
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<TimeDilationMixerBehaviour>.Create(graph, inputCount);
	}
}
