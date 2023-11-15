using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000008 RID: 8
[TrackColor(0.9454092f, 0.9779412f, 0.3883002f)]
[TrackClipType(typeof(LightControlClip))]
[TrackBindingType(typeof(Light))]
public class LightControlTrack : TrackAsset
{
	// Token: 0x06000010 RID: 16 RVA: 0x00002703 File Offset: 0x00000903
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		return ScriptPlayable<LightControlMixerBehaviour>.Create(graph, inputCount);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002711 File Offset: 0x00000911
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}
}
