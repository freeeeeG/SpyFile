using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class AnimationEventData : MonoBehaviour
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060004CB RID: 1227 RVA: 0x000284B1 File Offset: 0x000268B1
	// (set) Token: 0x060004CC RID: 1228 RVA: 0x000284B9 File Offset: 0x000268B9
	public AnimationEventData.ClipData[] clips
	{
		get
		{
			return this.m_clips;
		}
		set
		{
			this.m_clips = value;
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000284C4 File Offset: 0x000268C4
	public void Copy(AnimationEventData _other)
	{
		this.m_clips = new AnimationEventData.ClipData[_other.m_clips.Length];
		for (int i = 0; i < _other.m_clips.Length; i++)
		{
			this.m_clips[i] = new AnimationEventData.ClipData();
			this.m_clips[i].m_name = _other.m_clips[i].m_name;
			this.m_clips[i].m_duration = _other.m_clips[i].m_duration;
			this.m_clips[i].m_events = new AnimationEventData.EventData[_other.m_clips[i].m_events.Length];
			for (int j = 0; j < _other.m_clips[i].m_events.Length; j++)
			{
				this.m_clips[i].m_events[j] = new AnimationEventData.EventData();
				this.m_clips[i].m_events[j].m_name = _other.m_clips[i].m_events[j].m_name;
				this.m_clips[i].m_events[j].m_time = _other.m_clips[i].m_events[j].m_time;
			}
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x000285E4 File Offset: 0x000269E4
	public bool GetTriggerData(string _clipName, string _triggerName, out float o_clipDuration, out float o_triggerTime)
	{
		AnimationEventData.ClipData clipData = Array.Find<AnimationEventData.ClipData>(this.m_clips, (AnimationEventData.ClipData x) => x.m_name == _clipName);
		AnimationEventData.EventData eventData = Array.Find<AnimationEventData.EventData>(clipData.m_events, (AnimationEventData.EventData x) => x.m_name == _triggerName);
		o_clipDuration = clipData.m_duration;
		o_triggerTime = eventData.m_time;
		return true;
	}

	// Token: 0x0400042D RID: 1069
	[SerializeField]
	private AnimationEventData.ClipData[] m_clips = new AnimationEventData.ClipData[0];

	// Token: 0x020000FE RID: 254
	[Serializable]
	public class EventData
	{
		// Token: 0x0400042E RID: 1070
		public string m_name;

		// Token: 0x0400042F RID: 1071
		public float m_time;
	}

	// Token: 0x020000FF RID: 255
	[Serializable]
	public class ClipData
	{
		// Token: 0x04000430 RID: 1072
		public string m_name;

		// Token: 0x04000431 RID: 1073
		public float m_duration;

		// Token: 0x04000432 RID: 1074
		public AnimationEventData.EventData[] m_events = new AnimationEventData.EventData[0];
	}
}
