using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x0200000C RID: 12
[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(NavMeshAgentControlClip))]
[TrackBindingType(typeof(NavMeshAgent))]
public class NavMeshAgentControlTrack : TrackAsset
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002822 File Offset: 0x00000A22
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<NavMeshAgentControlMixerBehaviour>.Create(graph, inputCount);
	}
}
