using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Tantawowa.TimelineEvents
{
	// Token: 0x02000074 RID: 116
	[Serializable]
	public class TimelineEventClip : PlayableAsset, ITimelineClipAsset
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00007CD9 File Offset: 0x00005ED9
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00007CE1 File Offset: 0x00005EE1
		public GameObject TrackTargetObject { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00007CEA File Offset: 0x00005EEA
		public ClipCaps clipCaps
		{
			get
			{
				return ClipCaps.None;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007CF0 File Offset: 0x00005EF0
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			ScriptPlayable<TimelineEventBehaviour> playable = ScriptPlayable<TimelineEventBehaviour>.Create(graph, this.template, 0);
			playable.GetBehaviour().TargetObject = this.TrackTargetObject;
			return playable;
		}

		// Token: 0x040001A0 RID: 416
		public TimelineEventBehaviour template = new TimelineEventBehaviour();
	}
}
