using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x020004F4 RID: 1268
[ExecuteInEditMode]
public class AudioManager : Manager
{
	// Token: 0x0600179D RID: 6045 RVA: 0x0007848C File Offset: 0x0007688C
	protected virtual void Awake()
	{
		if (Application.isPlaying)
		{
			AudioMixer audioMixer = this.m_audioMixer;
			this.m_sfxAudioGroup = audioMixer.FindMatchingGroups("SFX")[0];
			this.m_ambAudioGroup = audioMixer.FindMatchingGroups("Amb")[0];
			this.m_musAudioGroup = audioMixer.FindMatchingGroups("Music")[0];
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000784E4 File Offset: 0x000768E4
	protected void OnEnable()
	{
		Transform transform = base.transform.Find("SharedSource");
		if (transform != null)
		{
			this.m_sharedSource = transform.gameObject;
		}
		else
		{
			this.m_sharedSource = GameObjectUtils.CreateOnParent(base.gameObject, "SharedSource");
		}
		if (!Application.isPlaying)
		{
			this.m_sharedSource.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x0007854C File Offset: 0x0007694C
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x00078554 File Offset: 0x00076954
	private void EditorFriendlyStartCoroutine(IEnumerator _coroutine)
	{
		if (Application.isPlaying)
		{
			base.StartCoroutine(_coroutine);
		}
		else
		{
			DebugAdvanceSystem.Add(_coroutine);
		}
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x00078573 File Offset: 0x00076973
	protected virtual void Update()
	{
		this.m_oneShotAudio.RemoveAll(AudioManager.m_destroyInactivePredicate);
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00078586 File Offset: 0x00076986
	public bool AddAudioDirectory(AudioDirectoryData _directory)
	{
		if (!this.m_audioDirectories.Contains(_directory))
		{
			ArrayUtils.PushBack<AudioDirectoryData>(ref this.m_audioDirectories, _directory);
			return true;
		}
		return false;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000785A8 File Offset: 0x000769A8
	public AudioSource TriggerAudio(GameOneShotAudioTag _tag, int _layer)
	{
		return this.TriggerAudio(_tag, this.FindEntry(_tag) as AudioDirectoryData.OneShotAudioDirectoryEntry, _layer);
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000785BE File Offset: 0x000769BE
	public AudioSource TriggerAudio(AudioDirectoryData.OneShotAudioDirectoryEntry _entry, int _layer)
	{
		return this.TriggerAudio(GameOneShotAudioTag.COUNT, _entry, _layer);
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x000785D0 File Offset: 0x000769D0
	public AudioSource TriggerAudio(GameOneShotAudioTag _tag, AudioDirectoryData.OneShotAudioDirectoryEntry _entry, int _layer)
	{
		if (AudioManager.m_activeOneShotTags[(int)_tag] && _entry.SingleInstance)
		{
			return null;
		}
		AudioManager.m_activeOneShotTags[(int)_tag] = true;
		AudioSource audioSource = this.StartAudio(null, AudioManager.AudioGroup.SFX, _entry.AudioFile, _entry, false, _layer);
		this.m_oneShotAudio.Add(new AudioManager.TagAudioSourcePair(_entry.Tag, audioSource));
		return audioSource;
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x00078630 File Offset: 0x00076A30
	public AudioSource StartAudio(GameLoopingAudioTag _tag, object _uniqueToken, int _layer)
	{
		return this.StartAudio(this.FindEntry(_tag), _uniqueToken, _layer);
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x00078641 File Offset: 0x00076A41
	public AudioSource StartAudio(AudioDirectoryData.LoopingAudioDirectoryEntry _entry, object _uniqueToken, int _layer)
	{
		return this.StartAudio(_uniqueToken, _entry.AudioFile, _entry.StartClip, _entry.EndClip, _entry, AudioManager.AudioGroup.SFX, _layer);
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0007865F File Offset: 0x00076A5F
	public void StopAudio(GameLoopingAudioTag _tag, object _uniqueToken)
	{
		this.StopAudio(_uniqueToken);
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x00078668 File Offset: 0x00076A68
	protected AudioSource StartAudio(object _uniqueToken, AudioClip _audioFile, AudioClip _startClip, AudioClip _endClip, AudioDirectoryData.LoopingAudioDirectoryEntry _parameters, AudioManager.AudioGroup _group, int _layer)
	{
		if (this.GetLoopingToken(_parameters, _uniqueToken) != null)
		{
			return null;
		}
		AudioManager.LoopingAudioToken loopingAudioToken = this.AddLoopingToken(_parameters, _uniqueToken);
		if (_parameters.SingleInstance && loopingAudioToken.m_owners.Count > 1)
		{
			return null;
		}
		AudioSource audioSource = this.AddSource(_group, _layer);
		this.EditorFriendlyStartCoroutine(this.LoopingAudioRoutine(audioSource, _uniqueToken, _audioFile, _startClip, _endClip, _parameters, _group, _layer));
		return audioSource;
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000786D4 File Offset: 0x00076AD4
	private IEnumerator LoopingAudioRoutine(AudioSource _source, object _uniqueToken, AudioClip _audioFile, AudioClip _startClip, AudioClip _endClip, AudioDirectoryData.LoopingAudioDirectoryEntry _parameters, AudioManager.AudioGroup _group, int _layer)
	{
		AudioSource startEndSource = null;
		if (_startClip != null || _endClip != null)
		{
			startEndSource = this.AddSource(_group, _layer);
		}
		if (_startClip != null)
		{
			this.StartAudio(startEndSource, _group, _startClip, _parameters, false, _layer);
			IEnumerator wait = CoroutineUtils.TimerRoutine(startEndSource.clip.length - _parameters.m_fadeInTime, base.gameObject.layer);
			while (wait.MoveNext())
			{
				yield return null;
			}
		}
		this.StartAudio(_source, _group, _audioFile, _parameters, true, _layer);
		if (_parameters.m_fadeInTime > 0f)
		{
			float initLoopVolume = _source.volume;
			float initStartVolume = (!(_startClip != null)) ? 0f : startEndSource.volume;
			float elapsedTime = 0f;
			IEnumerator wait2 = CoroutineUtils.TimerRoutine(_parameters.m_fadeInTime, base.gameObject.layer);
			while (wait2.MoveNext())
			{
				if (this.GetLoopingToken(_parameters, (!_parameters.SingleInstance) ? _uniqueToken : null) == null)
				{
					break;
				}
				elapsedTime += TimeManager.GetDeltaTime(base.gameObject.layer);
				float fadeProgress = Mathf.Clamp01(elapsedTime / _parameters.m_fadeInTime);
				if (_startClip != null)
				{
					startEndSource.volume = initStartVolume * (1f - fadeProgress);
				}
				_source.volume = initLoopVolume * fadeProgress;
				yield return null;
			}
			if (_startClip != null)
			{
				startEndSource.volume = 0f;
			}
		}
		while (this.GetLoopingToken(_parameters, (!_parameters.SingleInstance) ? _uniqueToken : null) != null)
		{
			yield return null;
		}
		if (_endClip != null)
		{
			this.StartAudio(startEndSource, _group, _endClip, _parameters, false, _layer);
		}
		if (_parameters.m_fadeOutTime > 0f)
		{
			if (_endClip != null)
			{
			}
			float fadeDuration = _parameters.m_fadeOutTime;
			float initLoopVolume2 = _source.volume;
			float initEndVolume = (!(startEndSource != null)) ? 0f : startEndSource.volume;
			float elapsedTime2 = 0f;
			IEnumerator wait3 = CoroutineUtils.TimerRoutine(fadeDuration, base.gameObject.layer);
			while (wait3.MoveNext())
			{
				elapsedTime2 += TimeManager.GetDeltaTime(base.gameObject.layer);
				float fadeProgress2 = Mathf.Clamp01(elapsedTime2 / _parameters.m_fadeOutTime);
				if (_endClip != null)
				{
					startEndSource.volume = initEndVolume * fadeProgress2;
				}
				_source.volume = initLoopVolume2 * (1f - fadeProgress2);
				yield return null;
			}
			if (_endClip != null)
			{
				startEndSource.volume = initEndVolume;
			}
			_source.volume = 0f;
		}
		_source.Stop();
		if (_endClip != null)
		{
			while (startEndSource.isPlaying)
			{
				yield return null;
			}
		}
		if (startEndSource != null)
		{
			AudioManager.DestroySource(startEndSource);
		}
		AudioManager.DestroySource(_source);
		yield break;
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x0007872C File Offset: 0x00076B2C
	protected void StopAudio(object _uniqueToken)
	{
		for (int i = this.m_loopingAudioTokens.Count - 1; i >= 0; i--)
		{
			AudioManager.LoopingAudioToken loopingAudioToken = this.m_loopingAudioTokens._items[i];
			loopingAudioToken.m_owners.Remove(_uniqueToken);
			if (loopingAudioToken.m_owners.Count == 0)
			{
				this.m_loopingAudioTokens.RemoveAt(i);
			}
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x00078790 File Offset: 0x00076B90
	private AudioSource StartAudio(AudioSource _source, AudioManager.AudioGroup _group, AudioClip _clip, ClipParameters _parameters, bool _isLooping, int _layer)
	{
		if (_clip != null && _parameters != null)
		{
			AudioSource audioSource = _source;
			if (audioSource == null)
			{
				audioSource = this.AddSource(_group, _layer);
			}
			audioSource.clip = _clip;
			audioSource.priority = _parameters.Priority;
			float randomPitchVariance = _parameters.RandomPitchVariance;
			float num = _parameters.Pitch + UnityEngine.Random.Range(-randomPitchVariance, randomPitchVariance);
			audioSource.pitch = num * Time.timeScale;
			audioSource.volume = _parameters.Volume;
			audioSource.loop = _isLooping;
			audioSource.Play();
			if (TimeManager.IsPaused(audioSource.gameObject))
			{
				audioSource.Pause();
			}
			return audioSource;
		}
		return null;
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x00078838 File Offset: 0x00076C38
	private AudioSource AddSource(AudioManager.AudioGroup _group, int _layer)
	{
		GameObject gameObject = GameObjectUtils.CreateOnParent(this.m_sharedSource, "Sound");
		if (!Application.isPlaying)
		{
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
		gameObject.layer = _layer;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.outputAudioMixerGroup = this.GetMixerGroup(_group);
		return audioSource;
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x00078884 File Offset: 0x00076C84
	private static void DestroySource(AudioSource _source)
	{
		UnityEngine.Object.DestroyImmediate(_source.gameObject);
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x00078894 File Offset: 0x00076C94
	private AudioMixerGroup GetMixerGroup(AudioManager.AudioGroup _group)
	{
		AudioMixerGroup result = null;
		switch (_group)
		{
		case AudioManager.AudioGroup.SFX:
			result = this.m_sfxAudioGroup;
			break;
		case AudioManager.AudioGroup.Ambience:
			result = this.m_ambAudioGroup;
			break;
		case AudioManager.AudioGroup.Music:
			result = this.m_musAudioGroup;
			break;
		}
		return result;
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x000788E4 File Offset: 0x00076CE4
	private AudioDirectoryEntry FindEntry(GameOneShotAudioTag _tag)
	{
		for (int i = 0; i < this.m_audioDirectories.Length; i++)
		{
			AudioDirectoryData audioDirectoryData = this.m_audioDirectories[i];
			for (int j = 0; j < audioDirectoryData.OneShotAudio.Length; j++)
			{
				AudioDirectoryData.OneShotAudioDirectoryEntry oneShotAudioDirectoryEntry = audioDirectoryData.OneShotAudio[j];
				if (oneShotAudioDirectoryEntry.Tag == _tag)
				{
					this.m_findEntryList.Add(oneShotAudioDirectoryEntry);
				}
			}
		}
		AudioDirectoryEntry result = this.m_findEntryList[UnityEngine.Random.Range(0, this.m_findEntryList.Count)];
		this.m_findEntryList.Clear();
		return result;
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x00078978 File Offset: 0x00076D78
	private AudioDirectoryData.LoopingAudioDirectoryEntry FindEntry(GameLoopingAudioTag _tag)
	{
		for (int i = 0; i < this.m_audioDirectories.Length; i++)
		{
			AudioDirectoryData audioDirectoryData = this.m_audioDirectories[i];
			for (int j = 0; j < audioDirectoryData.LoopingAudio.Length; j++)
			{
				AudioDirectoryData.LoopingAudioDirectoryEntry loopingAudioDirectoryEntry = audioDirectoryData.LoopingAudio[j];
				if (loopingAudioDirectoryEntry.Tag == _tag)
				{
					this.m_findEntryList.Add(loopingAudioDirectoryEntry);
				}
			}
		}
		AudioDirectoryData.LoopingAudioDirectoryEntry result = this.m_findEntryList[UnityEngine.Random.Range(0, this.m_findEntryList.Count)] as AudioDirectoryData.LoopingAudioDirectoryEntry;
		this.m_findEntryList.Clear();
		return result;
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x00078A14 File Offset: 0x00076E14
	private AudioManager.LoopingAudioToken AddLoopingToken(AudioDirectoryData.LoopingAudioDirectoryEntry _parameters, object _uniqueToken)
	{
		AudioManager.LoopingAudioToken loopingAudioToken = null;
		if (_parameters.SingleInstance)
		{
			loopingAudioToken = this.GetLoopingToken(_parameters, null);
		}
		if (loopingAudioToken == null)
		{
			loopingAudioToken = new AudioManager.LoopingAudioToken();
			loopingAudioToken.m_params = _parameters;
			this.m_loopingAudioTokens.Add(loopingAudioToken);
		}
		loopingAudioToken.m_owners.Add(_uniqueToken);
		return loopingAudioToken;
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x00078A64 File Offset: 0x00076E64
	private AudioManager.LoopingAudioToken GetLoopingToken(AudioDirectoryData.LoopingAudioDirectoryEntry _parameters, object _uniqueToken = null)
	{
		for (int i = 0; i < this.m_loopingAudioTokens.Count; i++)
		{
			AudioManager.LoopingAudioToken loopingAudioToken = this.m_loopingAudioTokens._items[i];
			if (loopingAudioToken.m_params == _parameters && (_uniqueToken == null || loopingAudioToken.m_owners.Contains(_uniqueToken)))
			{
				return loopingAudioToken;
			}
		}
		return null;
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x00078AC1 File Offset: 0x00076EC1
	protected AudioDirectoryData.LoopingAudioDirectoryEntry FindAmbienceData(GameLoopingAudioTag _tag)
	{
		return this.FindEntry(_tag);
	}

	// Token: 0x04001151 RID: 4433
	private const string c_sourceAttachName = "SharedSource";

	// Token: 0x04001152 RID: 4434
	[SerializeField]
	public AudioMixer m_audioMixer;

	// Token: 0x04001153 RID: 4435
	[SerializeField]
	private AudioDirectoryData[] m_audioDirectories;

	// Token: 0x04001154 RID: 4436
	private AudioMixerGroup m_sfxAudioGroup;

	// Token: 0x04001155 RID: 4437
	private AudioMixerGroup m_ambAudioGroup;

	// Token: 0x04001156 RID: 4438
	private AudioMixerGroup m_musAudioGroup;

	// Token: 0x04001157 RID: 4439
	private GameObject m_sharedSource;

	// Token: 0x04001158 RID: 4440
	private List<AudioManager.TagAudioSourcePair> m_oneShotAudio = new List<AudioManager.TagAudioSourcePair>();

	// Token: 0x04001159 RID: 4441
	private FastList<AudioManager.LoopingAudioToken> m_loopingAudioTokens = new FastList<AudioManager.LoopingAudioToken>();

	// Token: 0x0400115A RID: 4442
	protected static BitArray m_activeOneShotTags = new BitArray(320);

	// Token: 0x0400115B RID: 4443
	private List<AudioDirectoryEntry> m_findEntryList = new List<AudioDirectoryEntry>();

	// Token: 0x0400115C RID: 4444
	private static Predicate<AudioManager.TagAudioSourcePair> m_destroyInactivePredicate = delegate(AudioManager.TagAudioSourcePair obj)
	{
		if (!obj.m_source.isPlaying && !TimeManager.IsPaused(obj.m_source.gameObject))
		{
			AudioManager.m_activeOneShotTags[(int)obj.m_tag] = false;
			AudioManager.DestroySource(obj.m_source);
			return true;
		}
		return false;
	};

	// Token: 0x020004F5 RID: 1269
	public enum AudioGroup
	{
		// Token: 0x0400115E RID: 4446
		SFX,
		// Token: 0x0400115F RID: 4447
		Ambience,
		// Token: 0x04001160 RID: 4448
		Music
	}

	// Token: 0x020004F6 RID: 1270
	private struct TagAudioSourcePair
	{
		// Token: 0x060017B7 RID: 6071 RVA: 0x00078B41 File Offset: 0x00076F41
		public TagAudioSourcePair(GameOneShotAudioTag tag, AudioSource source)
		{
			this.m_tag = tag;
			this.m_source = source;
		}

		// Token: 0x04001161 RID: 4449
		public GameOneShotAudioTag m_tag;

		// Token: 0x04001162 RID: 4450
		public AudioSource m_source;
	}

	// Token: 0x020004F7 RID: 1271
	private class LoopingAudioToken
	{
		// Token: 0x04001163 RID: 4451
		public AudioDirectoryData.LoopingAudioDirectoryEntry m_params;

		// Token: 0x04001164 RID: 4452
		public FastList<object> m_owners = new FastList<object>();
	}
}
