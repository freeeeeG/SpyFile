using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Token: 0x02000662 RID: 1634
public class DialogMixerBehaviour : PlayableBehaviour
{
	// Token: 0x06001F22 RID: 7970 RVA: 0x00098564 File Offset: 0x00096964
	public void CreateLoopsFromClips(IEnumerable<TimelineClip> _clips)
	{
		this.m_loops.Clear();
		int num = 0;
		foreach (TimelineClip timelineClip in _clips)
		{
			this.RegisterLoop(num++, timelineClip.start, timelineClip.end);
		}
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x000985D8 File Offset: 0x000969D8
	private void RegisterLoop(int input, double startTime, double endTime)
	{
		DialogMixerBehaviour.Loop item = default(DialogMixerBehaviour.Loop);
		item.input = input;
		item.start = startTime;
		item.end = endTime;
		int num = this.m_loops.FindLastIndex((DialogMixerBehaviour.Loop x) => x.start <= startTime);
		if (num >= 0)
		{
			this.m_loops.Insert(num, item);
		}
		else
		{
			this.m_loops.Add(item);
		}
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x00098654 File Offset: 0x00096A54
	public override void OnPlayableCreate(Playable playable)
	{
		base.OnPlayableCreate(playable);
		this.m_playableDirector = (PlayableDirector)playable.GetGraph<Playable>().GetResolver();
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x00098681 File Offset: 0x00096A81
	public override void OnPlayableDestroy(Playable playable)
	{
		base.OnPlayableCreate(playable);
		this.m_loops.Clear();
		this.m_playableDirector = null;
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x0009869C File Offset: 0x00096A9C
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		this.m_previousTime = this.m_playableDirector.time;
		this.m_loopedBehaviour = null;
		for (int i = 0; i < this.m_loops.Count; i++)
		{
			DialogPlayableBehaviour dialogPlayableBehaviour = this.ExtractBehaviourForLoop(this.m_loops[i], playable);
			dialogPlayableBehaviour.Mixer = this;
		}
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x00098700 File Offset: 0x00096B00
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		base.OnBehaviourPause(playable, info);
		this.m_loopedBehaviour = null;
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x00098714 File Offset: 0x00096B14
	public override void PrepareFrame(Playable playable, FrameData info)
	{
		if (info.evaluationType == FrameData.EvaluationType.Evaluate)
		{
			return;
		}
		if (info.seekOccurred)
		{
			this.m_previousTime = playable.GetTime<Playable>();
			return;
		}
		if (Application.isPlaying)
		{
			double previousTime = this.m_previousTime;
			double time = this.m_playableDirector.time;
			DialogMixerBehaviour.Loop? loop = null;
			for (int i = 0; i < this.m_loops.Count; i++)
			{
				DialogMixerBehaviour.Loop loop2 = this.m_loops[i];
				if (previousTime >= loop2.start && previousTime <= loop2.end && time >= loop2.end)
				{
					if (loop == null || loop2.duration <= loop.Value.duration)
					{
						DialogPlayableBehaviour dialogPlayableBehaviour = this.ExtractBehaviourForLoop(loop2, playable);
						if (dialogPlayableBehaviour != null && dialogPlayableBehaviour.IsLoopActive())
						{
							loop = new DialogMixerBehaviour.Loop?(loop2);
						}
					}
				}
			}
			if (loop == null)
			{
			}
			if (loop != null)
			{
				DialogPlayableBehaviour loopedBehaviour = this.ExtractBehaviourForLoop(loop.Value, playable);
				this.m_loopedBehaviour = loopedBehaviour;
				this.m_playableDirector.time = loop.Value.start + (double)info.deltaTime;
				this.m_previousTime = loop.Value.start;
			}
			else
			{
				this.m_previousTime = this.m_playableDirector.time;
			}
		}
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x00098898 File Offset: 0x00096C98
	private DialogPlayableBehaviour ExtractBehaviourForLoop(DialogMixerBehaviour.Loop _loop, Playable _playable)
	{
		Playable input = _playable.GetInput(_loop.input);
		return ((ScriptPlayable<T>)input).GetBehaviour();
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x000988C1 File Offset: 0x00096CC1
	public bool HasPendingLoop(DialogPlayableBehaviour _behaviour)
	{
		return _behaviour == this.m_loopedBehaviour;
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x000988CC File Offset: 0x00096CCC
	public void CompletePendingLoop(DialogPlayableBehaviour _behaviour)
	{
		if (this.m_loopedBehaviour == _behaviour)
		{
			this.m_loopedBehaviour = null;
		}
	}

	// Token: 0x040017CC RID: 6092
	private List<DialogMixerBehaviour.Loop> m_loops = new List<DialogMixerBehaviour.Loop>();

	// Token: 0x040017CD RID: 6093
	private PlayableDirector m_playableDirector;

	// Token: 0x040017CE RID: 6094
	private double m_previousTime;

	// Token: 0x040017CF RID: 6095
	private DialogPlayableBehaviour m_loopedBehaviour;

	// Token: 0x02000663 RID: 1635
	private struct Loop
	{
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000988E1 File Offset: 0x00096CE1
		public double duration
		{
			get
			{
				return this.end - this.start;
			}
		}

		// Token: 0x040017D0 RID: 6096
		public int input;

		// Token: 0x040017D1 RID: 6097
		public double start;

		// Token: 0x040017D2 RID: 6098
		public double end;
	}
}
