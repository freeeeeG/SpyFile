using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Tantawowa.TimelineEvents
{
	// Token: 0x02000076 RID: 118
	[TrackColor(0.4448276f, 0f, 1f)]
	[TrackClipType(typeof(TimelineEventClip))]
	[TrackBindingType(typeof(GameObject))]
	public class TimelineEventTrack : TrackAsset
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007D40 File Offset: 0x00005F40
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			GameObject gameObject = go.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
			foreach (TimelineClip timelineClip in base.GetClips())
			{
				TimelineEventClip timelineEventClip = timelineClip.asset as TimelineEventClip;
				if (timelineEventClip && gameObject)
				{
					timelineEventClip.TrackTargetObject = gameObject;
				}
			}
			return ScriptPlayable<TimelineEventMixerBehaviour>.Create(graph, inputCount);
		}
	}
}
