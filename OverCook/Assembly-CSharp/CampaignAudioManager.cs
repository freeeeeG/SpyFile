using System;
using System.Collections;
using GameModes;
using UnityEngine;

// Token: 0x020004F9 RID: 1273
[ExecuteInEditMode]
public class CampaignAudioManager : AudioManager
{
	// Token: 0x060017BE RID: 6078 RVA: 0x000791F3 File Offset: 0x000775F3
	protected override void Awake()
	{
		base.Awake();
		if (Application.isPlaying)
		{
			this.m_dataStore = GameUtils.RequireManager<DataStore>();
			this.m_dataStore.Register(CampaignAudioManager.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
		}
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x0007922C File Offset: 0x0007762C
	protected void OnDestroy()
	{
		if (this.m_dataStore != null)
		{
			this.m_dataStore.Unregister(CampaignAudioManager.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
		}
		AudioManager.m_activeOneShotTags.SetAll(false);
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x00079268 File Offset: 0x00077668
	public void SetAudioState(CampaignAudioManager.AudioState _state)
	{
		if (this.m_state != null && this.m_state.Value == _state)
		{
			return;
		}
		this.m_state = new CampaignAudioManager.AudioState?(_state);
		if (_state != CampaignAudioManager.AudioState.IntroState)
		{
			if (_state != CampaignAudioManager.AudioState.InLevel)
			{
				if (_state == CampaignAudioManager.AudioState.SummaryScreen)
				{
					this.m_activeTimePair = null;
					this.SetMusic_Internal(null, true);
					if (this.m_summaryScreenMusic != null)
					{
						base.StartCoroutine(this.DelayedSetMusic(this.m_summaryScreenMusic, 6f));
					}
					for (int i = 0; i < this.m_ambienceTokens.Length; i++)
					{
						base.StopAudio(this.m_ambienceTokens[i]);
					}
					this.m_ambienceTokens.AllRemoved_Predicate((object x) => true);
				}
			}
			else
			{
				this.SetMusic_Internal(this.m_inLevelMusic, false);
				for (int j = 0; j < this.m_inLevelAmbiences.Length; j++)
				{
					object obj = this.StartAmbience_Internal(this.m_inLevelAmbiences[j], this.m_ambienceLayer);
					if (obj != null)
					{
						ArrayUtils.PushBack<object>(ref this.m_ambienceTokens, obj);
					}
				}
			}
		}
		else
		{
			this.SetMusic_Internal(null, false);
			for (int k = 0; k < this.m_introAmbiences.Length; k++)
			{
				object obj2 = this.StartAmbience_Internal(this.m_introAmbiences[k], this.m_ambienceLayer);
				if (obj2 != null)
				{
					ArrayUtils.PushBack<object>(ref this.m_ambienceTokens, obj2);
				}
			}
		}
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x000793F0 File Offset: 0x000777F0
	private void SetMusic_Internal(AudioClip _audioFile, bool _killPrevious = false)
	{
		if (this.m_musicObject != null)
		{
			if (_killPrevious || _audioFile == null)
			{
				this.m_musicObject.StopMusic(_killPrevious);
			}
			this.m_musicObject = null;
		}
		if (_audioFile != null)
		{
			this.m_musicObject = UnityEngine.Object.Instantiate<PersistentMusic>(this.m_persistentMusicPrefab);
			if (this.m_musicObject != null)
			{
				AudioSource audioSource = this.m_musicObject.GetAudioSource();
				audioSource.gameObject.layer = this.m_musicLayer;
				audioSource.clip = _audioFile;
				if (this.m_activeTimePair != null)
				{
					audioSource.pitch = this.m_activeTimePair.MusicPitch;
				}
				audioSource.Play();
			}
		}
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000794A8 File Offset: 0x000778A8
	private IEnumerator DelayedSetMusic(AudioClip _audioFile, float _delay)
	{
		yield return CoroutineUtils.TimerRoutine(_delay, base.gameObject.layer);
		this.SetMusic_Internal(this.m_summaryScreenMusic, false);
		yield break;
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000794CC File Offset: 0x000778CC
	private object StartAmbience_Internal(GameLoopingAudioTag _tag, int _layer)
	{
		AudioDirectoryData.LoopingAudioDirectoryEntry loopingAudioDirectoryEntry = base.FindAmbienceData(_tag);
		if (loopingAudioDirectoryEntry != null)
		{
			object obj = _tag;
			AudioSource x = base.StartAudio(obj, loopingAudioDirectoryEntry.AudioFile, loopingAudioDirectoryEntry.StartClip, loopingAudioDirectoryEntry.EndClip, loopingAudioDirectoryEntry, AudioManager.AudioGroup.Ambience, _layer);
			if (x != null)
			{
				return obj;
			}
		}
		return null;
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x0007951A File Offset: 0x0007791A
	public void SetMusic(AudioClip _audioFile, bool _killPrevious = false)
	{
		this.SetMusic_Internal(_audioFile, _killPrevious);
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x00079524 File Offset: 0x00077924
	public void StartAmbience(GameLoopingAudioTag _tag)
	{
		object obj = this.StartAmbience_Internal(_tag, this.m_ambienceLayer);
		if (obj != null)
		{
			ArrayUtils.PushBack<object>(ref this.m_ambienceTokens, obj);
		}
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x00079554 File Offset: 0x00077954
	public void StopAmbience(GameLoopingAudioTag _tag)
	{
		base.StopAudio(_tag);
		this.m_ambienceTokens.AllRemoved_Predicate((object x) => x == _tag);
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x00079598 File Offset: 0x00077998
	private void OnTimeUpdatedNotification(DataStore.Id id, object value)
	{
		if (!GameUtils.GetGameSession().PendingGameModeSessionConfigChanges && GameUtils.GetGameSession().GameModeKind == Kind.Campaign && this.m_musicObject != null)
		{
			float time = Convert.ToSingle(value);
			Generic<float, CampaignAudioManager.TimePair> scoreFunction = delegate(CampaignAudioManager.TimePair _timePair)
			{
				if (time < (float)_timePair.TimeValue)
				{
					return (float)_timePair.TimeValue;
				}
				return float.MaxValue;
			};
			CampaignAudioManager.TimePair value2 = this.m_timedMusicModifiers.FindLowestScoring(scoreFunction).Value;
			if (value2 != this.m_activeTimePair)
			{
				this.m_activeTimePair = value2;
				if (this.m_activeTimePair != null)
				{
					base.TriggerAudio(this.m_activeTimePair.Tag, base.gameObject.layer);
				}
			}
			if (this.m_activeTimePair != null)
			{
				AudioSource audioSource = this.m_musicObject.GetAudioSource();
				if (audioSource != null)
				{
					audioSource.pitch = value2.MusicPitch;
				}
			}
		}
	}

	// Token: 0x04001167 RID: 4455
	[Header("Music")]
	[SerializeField]
	[Layer]
	private int m_musicLayer;

	// Token: 0x04001168 RID: 4456
	[SerializeField]
	private PersistentMusic m_persistentMusicPrefab;

	// Token: 0x04001169 RID: 4457
	[SerializeField]
	private AudioClip m_inLevelMusic;

	// Token: 0x0400116A RID: 4458
	[SerializeField]
	private AudioClip m_summaryScreenMusic;

	// Token: 0x0400116B RID: 4459
	[SerializeField]
	private CampaignAudioManager.TimePair[] m_timedMusicModifiers = new CampaignAudioManager.TimePair[0];

	// Token: 0x0400116C RID: 4460
	[Header("Ambience")]
	[SerializeField]
	[Layer]
	private int m_ambienceLayer;

	// Token: 0x0400116D RID: 4461
	[SerializeField]
	private GameLoopingAudioTag[] m_introAmbiences;

	// Token: 0x0400116E RID: 4462
	[SerializeField]
	private GameLoopingAudioTag[] m_inLevelAmbiences;

	// Token: 0x0400116F RID: 4463
	private CampaignAudioManager.AudioState? m_state;

	// Token: 0x04001170 RID: 4464
	private PersistentMusic m_musicObject;

	// Token: 0x04001171 RID: 4465
	private CampaignAudioManager.TimePair m_activeTimePair;

	// Token: 0x04001172 RID: 4466
	private object[] m_ambienceTokens = new object[0];

	// Token: 0x04001173 RID: 4467
	private DataStore m_dataStore;

	// Token: 0x04001174 RID: 4468
	private static readonly DataStore.Id k_timeUpdatedId = new DataStore.Id("time.updated");

	// Token: 0x020004FA RID: 1274
	public enum AudioState
	{
		// Token: 0x04001177 RID: 4471
		IntroState,
		// Token: 0x04001178 RID: 4472
		InLevel,
		// Token: 0x04001179 RID: 4473
		SummaryScreen
	}

	// Token: 0x020004FB RID: 1275
	public enum StopBehaviour
	{
		// Token: 0x0400117B RID: 4475
		Cutoff,
		// Token: 0x0400117C RID: 4476
		ContinueToEnd,
		// Token: 0x0400117D RID: 4477
		Fade
	}

	// Token: 0x020004FC RID: 1276
	[Serializable]
	private class TimePair
	{
		// Token: 0x0400117E RID: 4478
		public int TimeValue;

		// Token: 0x0400117F RID: 4479
		public float MusicPitch;

		// Token: 0x04001180 RID: 4480
		public GameOneShotAudioTag Tag;
	}
}
