using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000666 RID: 1638
[TrackClipType(typeof(DialogPlayableAsset))]
[TrackColor(0.56f, 0.1f, 0.52f)]
public class DialogTrackAsset : TrackAsset
{
	// Token: 0x06001F3C RID: 7996 RVA: 0x00098BE6 File Offset: 0x00096FE6
	protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
	{
		return base.CreatePlayable(graph, go, clip);
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x00098BF4 File Offset: 0x00096FF4
	public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
	{
		ScriptPlayable<DialogMixerBehaviour> playable = ScriptPlayable<DialogMixerBehaviour>.Create(graph, inputCount);
		DialogMixerBehaviour behaviour = playable.GetBehaviour();
		behaviour.CreateLoopsFromClips(base.GetClips());
		return playable;
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x00098C23 File Offset: 0x00097023
	public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
	{
		base.GatherProperties(director, driver);
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00098C2D File Offset: 0x0009702D
	public override IEnumerable<PlayableBinding> outputs
	{
		get
		{
			return base.outputs;
		}
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x00098C35 File Offset: 0x00097035
	public override void OnEnable()
	{
		base.OnEnable();
	}
}
