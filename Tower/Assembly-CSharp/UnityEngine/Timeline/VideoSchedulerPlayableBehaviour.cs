using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x020000C1 RID: 193
	public sealed class VideoSchedulerPlayableBehaviour : PlayableBehaviour
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000B397 File Offset: 0x00009597
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0000B39F File Offset: 0x0000959F
		internal PlayableDirector director
		{
			get
			{
				return this.m_Director;
			}
			set
			{
				this.m_Director = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000B3A8 File Offset: 0x000095A8
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000B3B0 File Offset: 0x000095B0
		internal IEnumerable<TimelineClip> clips
		{
			get
			{
				return this.m_Clips;
			}
			set
			{
				this.m_Clips = value;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B3BC File Offset: 0x000095BC
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.m_Clips == null)
			{
				return;
			}
			int num = 0;
			foreach (TimelineClip timelineClip in this.m_Clips)
			{
				VideoPlayableBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(num)).GetBehaviour();
				if (behaviour != null)
				{
					double num2 = Math.Max(0.0, behaviour.preloadTime);
					if (this.m_Director.time >= timelineClip.start + timelineClip.duration || this.m_Director.time <= timelineClip.start - num2)
					{
						behaviour.StopVideo();
					}
					else if (this.m_Director.time > timelineClip.start - num2)
					{
						behaviour.PrepareVideo();
					}
				}
				num++;
			}
		}

		// Token: 0x04000244 RID: 580
		private IEnumerable<TimelineClip> m_Clips;

		// Token: 0x04000245 RID: 581
		private PlayableDirector m_Director;
	}
}
