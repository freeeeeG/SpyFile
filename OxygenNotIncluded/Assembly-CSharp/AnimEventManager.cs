using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200043D RID: 1085
public class AnimEventManager : Singleton<AnimEventManager>
{
	// Token: 0x060016F2 RID: 5874 RVA: 0x000769FE File Offset: 0x00074BFE
	public void FreeResources()
	{
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x00076A00 File Offset: 0x00074C00
	public HandleVector<int>.Handle PlayAnim(KAnimControllerBase controller, KAnim.Anim anim, KAnim.PlayMode mode, float time, bool use_unscaled_time)
	{
		AnimEventManager.AnimData animData = default(AnimEventManager.AnimData);
		animData.frameRate = anim.frameRate;
		animData.totalTime = anim.totalTime;
		animData.numFrames = anim.numFrames;
		animData.useUnscaledTime = use_unscaled_time;
		AnimEventManager.EventPlayerData eventPlayerData = default(AnimEventManager.EventPlayerData);
		eventPlayerData.elapsedTime = time;
		eventPlayerData.mode = mode;
		eventPlayerData.controller = (controller as KBatchedAnimController);
		eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
		eventPlayerData.previousFrame = -1;
		eventPlayerData.events = null;
		eventPlayerData.updatingEvents = null;
		eventPlayerData.events = GameAudioSheets.Get().GetEvents(anim.id);
		if (eventPlayerData.events == null)
		{
			eventPlayerData.events = AnimEventManager.emptyEventList;
		}
		HandleVector<int>.Handle result;
		if (animData.useUnscaledTime)
		{
			HandleVector<int>.Handle anim_data_handle = this.uiAnimData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle = this.uiEventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle, event_data_handle, true));
		}
		else
		{
			HandleVector<int>.Handle anim_data_handle2 = this.animData.Allocate(animData);
			HandleVector<int>.Handle event_data_handle2 = this.eventData.Allocate(eventPlayerData);
			result = this.indirectionData.Allocate(new AnimEventManager.IndirectionData(anim_data_handle2, event_data_handle2, false));
		}
		return result;
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x00076B34 File Offset: 0x00074D34
	public void SetMode(HandleVector<int>.Handle handle, KAnim.PlayMode mode)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.mode = mode;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x00076B90 File Offset: 0x00074D90
	public void StopAnim(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.AnimData> kcompactedVector = data.isUIData ? this.uiAnimData : this.animData;
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector2 = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector2.GetData(data.eventDataHandle);
		this.StopEvents(data2);
		kcompactedVector.Free(data.animDataHandle);
		kcompactedVector2.Free(data.eventDataHandle);
		this.indirectionData.Free(handle);
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x00076C1C File Offset: 0x00074E1C
	public float GetElapsedTime(HandleVector<int>.Handle handle)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		return (data.isUIData ? this.uiEventData : this.eventData).GetData(data.eventDataHandle).elapsedTime;
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x00076C5C File Offset: 0x00074E5C
	public void SetElapsedTime(HandleVector<int>.Handle handle, float elapsed_time)
	{
		AnimEventManager.IndirectionData data = this.indirectionData.GetData(handle);
		KCompactedVector<AnimEventManager.EventPlayerData> kcompactedVector = data.isUIData ? this.uiEventData : this.eventData;
		AnimEventManager.EventPlayerData data2 = kcompactedVector.GetData(data.eventDataHandle);
		data2.elapsedTime = elapsed_time;
		kcompactedVector.SetData(data.eventDataHandle, data2);
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x00076CB0 File Offset: 0x00074EB0
	public void Update()
	{
		float deltaTime = Time.deltaTime;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		this.Update(deltaTime, this.animData.GetDataList(), this.eventData.GetDataList());
		this.Update(unscaledDeltaTime, this.uiAnimData.GetDataList(), this.uiEventData.GetDataList());
		for (int i = 0; i < this.finishedCalls.Count; i++)
		{
			this.finishedCalls[i].TriggerStop();
		}
		this.finishedCalls.Clear();
	}

	// Token: 0x060016F9 RID: 5881 RVA: 0x00076D38 File Offset: 0x00074F38
	private void Update(float dt, List<AnimEventManager.AnimData> anim_data, List<AnimEventManager.EventPlayerData> event_data)
	{
		if (dt <= 0f)
		{
			return;
		}
		for (int i = 0; i < event_data.Count; i++)
		{
			AnimEventManager.EventPlayerData eventPlayerData = event_data[i];
			if (!(eventPlayerData.controller == null) && eventPlayerData.mode != KAnim.PlayMode.Paused)
			{
				eventPlayerData.currentFrame = eventPlayerData.controller.GetFrameIdx(eventPlayerData.elapsedTime, false);
				event_data[i] = eventPlayerData;
				this.PlayEvents(eventPlayerData);
				eventPlayerData.previousFrame = eventPlayerData.currentFrame;
				eventPlayerData.elapsedTime += dt * eventPlayerData.controller.GetPlaySpeed();
				event_data[i] = eventPlayerData;
				if (eventPlayerData.updatingEvents != null)
				{
					for (int j = 0; j < eventPlayerData.updatingEvents.Count; j++)
					{
						eventPlayerData.updatingEvents[j].OnUpdate(eventPlayerData);
					}
				}
				event_data[i] = eventPlayerData;
				if (eventPlayerData.mode != KAnim.PlayMode.Loop && eventPlayerData.currentFrame >= anim_data[i].numFrames - 1)
				{
					this.StopEvents(eventPlayerData);
					this.finishedCalls.Add(eventPlayerData.controller);
				}
			}
		}
	}

	// Token: 0x060016FA RID: 5882 RVA: 0x00076E50 File Offset: 0x00075050
	private void PlayEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Play(data);
		}
	}

	// Token: 0x060016FB RID: 5883 RVA: 0x00076E88 File Offset: 0x00075088
	private void StopEvents(AnimEventManager.EventPlayerData data)
	{
		for (int i = 0; i < data.events.Count; i++)
		{
			data.events[i].Stop(data);
		}
		if (data.updatingEvents != null)
		{
			data.updatingEvents.Clear();
		}
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x00076ED0 File Offset: 0x000750D0
	public AnimEventManager.DevTools_DebugInfo DevTools_GetDebugInfo()
	{
		return new AnimEventManager.DevTools_DebugInfo(this, this.animData, this.eventData, this.uiAnimData, this.uiEventData);
	}

	// Token: 0x04000CBA RID: 3258
	private static readonly List<AnimEvent> emptyEventList = new List<AnimEvent>();

	// Token: 0x04000CBB RID: 3259
	private const int INITIAL_VECTOR_SIZE = 256;

	// Token: 0x04000CBC RID: 3260
	private KCompactedVector<AnimEventManager.AnimData> animData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04000CBD RID: 3261
	private KCompactedVector<AnimEventManager.EventPlayerData> eventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04000CBE RID: 3262
	private KCompactedVector<AnimEventManager.AnimData> uiAnimData = new KCompactedVector<AnimEventManager.AnimData>(256);

	// Token: 0x04000CBF RID: 3263
	private KCompactedVector<AnimEventManager.EventPlayerData> uiEventData = new KCompactedVector<AnimEventManager.EventPlayerData>(256);

	// Token: 0x04000CC0 RID: 3264
	private KCompactedVector<AnimEventManager.IndirectionData> indirectionData = new KCompactedVector<AnimEventManager.IndirectionData>(0);

	// Token: 0x04000CC1 RID: 3265
	private List<KBatchedAnimController> finishedCalls = new List<KBatchedAnimController>();

	// Token: 0x020010CC RID: 4300
	public struct AnimData
	{
		// Token: 0x04005A18 RID: 23064
		public float frameRate;

		// Token: 0x04005A19 RID: 23065
		public float totalTime;

		// Token: 0x04005A1A RID: 23066
		public int numFrames;

		// Token: 0x04005A1B RID: 23067
		public bool useUnscaledTime;
	}

	// Token: 0x020010CD RID: 4301
	[DebuggerDisplay("{controller.name}, Anim={currentAnim}, Frame={currentFrame}, Mode={mode}")]
	public struct EventPlayerData
	{
		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06007780 RID: 30592 RVA: 0x002D3EF7 File Offset: 0x002D20F7
		// (set) Token: 0x06007781 RID: 30593 RVA: 0x002D3EFF File Offset: 0x002D20FF
		public int currentFrame { readonly get; set; }

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06007782 RID: 30594 RVA: 0x002D3F08 File Offset: 0x002D2108
		// (set) Token: 0x06007783 RID: 30595 RVA: 0x002D3F10 File Offset: 0x002D2110
		public int previousFrame { readonly get; set; }

		// Token: 0x06007784 RID: 30596 RVA: 0x002D3F19 File Offset: 0x002D2119
		public ComponentType GetComponent<ComponentType>()
		{
			return this.controller.GetComponent<ComponentType>();
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06007785 RID: 30597 RVA: 0x002D3F26 File Offset: 0x002D2126
		public string name
		{
			get
			{
				return this.controller.name;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06007786 RID: 30598 RVA: 0x002D3F33 File Offset: 0x002D2133
		public float normalizedTime
		{
			get
			{
				return this.elapsedTime / this.controller.CurrentAnim.totalTime;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06007787 RID: 30599 RVA: 0x002D3F4C File Offset: 0x002D214C
		public Vector3 position
		{
			get
			{
				return this.controller.transform.GetPosition();
			}
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x002D3F5E File Offset: 0x002D215E
		public void AddUpdatingEvent(AnimEvent ev)
		{
			if (this.updatingEvents == null)
			{
				this.updatingEvents = new List<AnimEvent>();
			}
			this.updatingEvents.Add(ev);
		}

		// Token: 0x06007789 RID: 30601 RVA: 0x002D3F7F File Offset: 0x002D217F
		public void SetElapsedTime(float elapsedTime)
		{
			this.elapsedTime = elapsedTime;
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x002D3F88 File Offset: 0x002D2188
		public void FreeResources()
		{
			this.elapsedTime = 0f;
			this.mode = KAnim.PlayMode.Once;
			this.currentFrame = 0;
			this.previousFrame = 0;
			this.events = null;
			this.updatingEvents = null;
			this.controller = null;
		}

		// Token: 0x04005A1C RID: 23068
		public float elapsedTime;

		// Token: 0x04005A1D RID: 23069
		public KAnim.PlayMode mode;

		// Token: 0x04005A20 RID: 23072
		public List<AnimEvent> events;

		// Token: 0x04005A21 RID: 23073
		public List<AnimEvent> updatingEvents;

		// Token: 0x04005A22 RID: 23074
		public KBatchedAnimController controller;
	}

	// Token: 0x020010CE RID: 4302
	private struct IndirectionData
	{
		// Token: 0x0600778B RID: 30603 RVA: 0x002D3FBF File Offset: 0x002D21BF
		public IndirectionData(HandleVector<int>.Handle anim_data_handle, HandleVector<int>.Handle event_data_handle, bool is_ui_data)
		{
			this.isUIData = is_ui_data;
			this.animDataHandle = anim_data_handle;
			this.eventDataHandle = event_data_handle;
		}

		// Token: 0x04005A23 RID: 23075
		public bool isUIData;

		// Token: 0x04005A24 RID: 23076
		public HandleVector<int>.Handle animDataHandle;

		// Token: 0x04005A25 RID: 23077
		public HandleVector<int>.Handle eventDataHandle;
	}

	// Token: 0x020010CF RID: 4303
	public readonly struct DevTools_DebugInfo
	{
		// Token: 0x0600778C RID: 30604 RVA: 0x002D3FD6 File Offset: 0x002D21D6
		public DevTools_DebugInfo(AnimEventManager eventManager, KCompactedVector<AnimEventManager.AnimData> animData, KCompactedVector<AnimEventManager.EventPlayerData> eventData, KCompactedVector<AnimEventManager.AnimData> uiAnimData, KCompactedVector<AnimEventManager.EventPlayerData> uiEventData)
		{
			this.eventManager = eventManager;
			this.animData = animData;
			this.eventData = eventData;
			this.uiAnimData = uiAnimData;
			this.uiEventData = uiEventData;
		}

		// Token: 0x04005A26 RID: 23078
		public readonly AnimEventManager eventManager;

		// Token: 0x04005A27 RID: 23079
		public readonly KCompactedVector<AnimEventManager.AnimData> animData;

		// Token: 0x04005A28 RID: 23080
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> eventData;

		// Token: 0x04005A29 RID: 23081
		public readonly KCompactedVector<AnimEventManager.AnimData> uiAnimData;

		// Token: 0x04005A2A RID: 23082
		public readonly KCompactedVector<AnimEventManager.EventPlayerData> uiEventData;
	}
}
