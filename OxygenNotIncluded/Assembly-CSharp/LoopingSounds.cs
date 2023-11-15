using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x020004CE RID: 1230
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSounds")]
public class LoopingSounds : KMonoBehaviour
{
	// Token: 0x06001C20 RID: 7200 RVA: 0x000965F4 File Offset: 0x000947F4
	public bool IsSoundPlaying(string path)
	{
		using (List<LoopingSounds.LoopingSoundEvent>.Enumerator enumerator = this.loopingSounds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.asset == path)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001C21 RID: 7201 RVA: 0x00096654 File Offset: 0x00094854
	public bool StartSound(string asset, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, bool ignore_pause = false, bool enable_camera_scaled_position = true)
	{
		if (asset == null || asset == "")
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = behaviour.GetComponent<Transform>().GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, !ignore_pause, true, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x00096740 File Offset: 0x00094940
	public bool StartSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.StartSound(eventReferencePath);
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x0009675C File Offset: 0x0009495C
	public bool StartSound(string asset)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, true, true, true, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x0009683C File Offset: 0x00094A3C
	public bool StartSound(string asset, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06001C25 RID: 7205 RVA: 0x0009691C File Offset: 0x00094B1C
	public void UpdateVelocity(string asset, Vector2 value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateVelocity(loopingSoundEvent.handle, value);
				break;
			}
		}
	}

	// Token: 0x06001C26 RID: 7206 RVA: 0x0009698C File Offset: 0x00094B8C
	public void UpdateFirstParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06001C27 RID: 7207 RVA: 0x000969FC File Offset: 0x00094BFC
	public void UpdateSecondParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateSecondParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x00096A6C File Offset: 0x00094C6C
	private void StopSoundAtIndex(int i)
	{
		LoopingSoundManager.StopSound(this.loopingSounds[i].handle);
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00096A84 File Offset: 0x00094C84
	public void StopSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.StopSound(eventReferencePath);
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x00096AA0 File Offset: 0x00094CA0
	public void StopSound(string asset)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				this.StopSoundAtIndex(i);
				this.loopingSounds.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x00096AF0 File Offset: 0x00094CF0
	public void PauseSound(string asset, bool paused)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				LoopingSoundManager.PauseSound(this.loopingSounds[i].handle, paused);
				return;
			}
		}
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x00096B44 File Offset: 0x00094D44
	public void StopAllSounds()
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			this.StopSoundAtIndex(i);
		}
		this.loopingSounds.Clear();
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x00096B79 File Offset: 0x00094D79
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopAllSounds();
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x00096B88 File Offset: 0x00094D88
	public void SetParameter(EventReference event_ref, HashedString parameter, float value)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.SetParameter(eventReferencePath, parameter, value);
	}

	// Token: 0x06001C2F RID: 7215 RVA: 0x00096BA8 File Offset: 0x00094DA8
	public void SetParameter(string path, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == path)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x00096C18 File Offset: 0x00094E18
	public void PlayEvent(GameSoundEvents.Event ev)
	{
		if (AudioDebug.Get().debugGameEventSounds)
		{
			string str = "GameSoundEvent: ";
			HashedString name = ev.Name;
			global::Debug.Log(str + name.ToString());
		}
		List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
		if (events == null)
		{
			return;
		}
		Vector2 v = base.transform.GetPosition();
		for (int i = 0; i < events.Count; i++)
		{
			SoundEvent soundEvent = events[i] as SoundEvent;
			if (soundEvent == null || soundEvent.sound == null)
			{
				return;
			}
			if (CameraController.Instance.IsAudibleSound(v, soundEvent.sound))
			{
				if (AudioDebug.Get().debugGameEventSounds)
				{
					global::Debug.Log("GameSound: " + soundEvent.sound);
				}
				float num = 0f;
				if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
				{
					if (Time.time - num > soundEvent.minInterval)
					{
						SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
					}
				}
				else
				{
					SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
				}
				this.lastTimePlayed[soundEvent.soundHash] = Time.time;
			}
		}
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x00096D68 File Offset: 0x00094F68
	public void UpdateObjectSelection(bool selected)
	{
		GameObject gameObject = base.gameObject;
		if (selected && gameObject != null && CameraController.Instance.IsVisiblePos(gameObject.transform.position))
		{
			this.objectIsSelectedAndVisible = true;
			this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.sound_pos);
			this.vol = 1f;
		}
		else
		{
			this.objectIsSelectedAndVisible = false;
			this.sound_pos = base.transform.GetPosition();
			this.sound_pos.z = 0f;
			this.vol = 1f;
		}
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			LoopingSoundManager.Get().UpdateObjectSelection(this.loopingSounds[i].handle, this.sound_pos, this.vol, this.objectIsSelectedAndVisible);
		}
	}

	// Token: 0x04000F81 RID: 3969
	private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();

	// Token: 0x04000F82 RID: 3970
	private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();

	// Token: 0x04000F83 RID: 3971
	[SerializeField]
	public bool updatePosition;

	// Token: 0x04000F84 RID: 3972
	public float vol = 1f;

	// Token: 0x04000F85 RID: 3973
	public bool objectIsSelectedAndVisible;

	// Token: 0x04000F86 RID: 3974
	public Vector3 sound_pos;

	// Token: 0x02001179 RID: 4473
	private struct LoopingSoundEvent
	{
		// Token: 0x04005C75 RID: 23669
		public string asset;

		// Token: 0x04005C76 RID: 23670
		public HandleVector<int>.Handle handle;
	}
}
