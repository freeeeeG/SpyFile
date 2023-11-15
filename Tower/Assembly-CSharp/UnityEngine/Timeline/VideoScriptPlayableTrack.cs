using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x020000C3 RID: 195
	[TrackClipType(typeof(VideoScriptPlayableAsset))]
	[TrackColor(0.008f, 0.698f, 0.655f)]
	[Serializable]
	public class VideoScriptPlayableTrack : TrackAsset
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x0000B53C File Offset: 0x0000973C
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			PlayableDirector component = go.GetComponent<PlayableDirector>();
			ScriptPlayable<VideoSchedulerPlayableBehaviour> playable = ScriptPlayable<VideoSchedulerPlayableBehaviour>.Create(graph, inputCount);
			VideoSchedulerPlayableBehaviour behaviour = playable.GetBehaviour();
			if (behaviour != null)
			{
				behaviour.director = component;
				behaviour.clips = base.GetClips();
			}
			return playable;
		}
	}
}
