using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x020004CD RID: 1229
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSoundManager")]
public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06001C0B RID: 7179 RVA: 0x00095984 File Offset: 0x00093B84
	public static void DestroyInstance()
	{
		LoopingSoundManager.instance = null;
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x0009598C File Offset: 0x00093B8C
	protected override void OnPrefabInit()
	{
		LoopingSoundManager.instance = this;
		this.CollectParameterUpdaters();
	}

	// Token: 0x06001C0D RID: 7181 RVA: 0x0009599C File Offset: 0x00093B9C
	protected override void OnSpawn()
	{
		if (SpeedControlScreen.Instance != null && Game.Instance != null)
		{
			Game.Instance.Subscribe(-1788536802, new Action<object>(LoopingSoundManager.instance.OnPauseChanged));
		}
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			this.OnActiveWorldChanged();
		});
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x000959FF File Offset: 0x00093BFF
	private void OnActiveWorldChanged()
	{
		this.StopAllSounds();
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x00095A08 File Offset: 0x00093C08
	private void CollectParameterUpdaters()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (!type.IsAbstract)
			{
				bool flag = false;
				Type baseType = type.BaseType;
				while (baseType != null)
				{
					if (baseType == typeof(LoopingSoundParameterUpdater))
					{
						flag = true;
						break;
					}
					baseType = baseType.BaseType;
				}
				if (flag)
				{
					LoopingSoundParameterUpdater loopingSoundParameterUpdater = (LoopingSoundParameterUpdater)Activator.CreateInstance(type);
					DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(loopingSoundParameterUpdater.parameter));
					this.parameterUpdaters[loopingSoundParameterUpdater.parameter] = loopingSoundParameterUpdater;
				}
			}
		}
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x00095AD0 File Offset: 0x00093CD0
	public void UpdateFirstParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.firstParameterValue = value;
		data.firstParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x00095B34 File Offset: 0x00093D34
	public void UpdateSecondParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.secondParameterValue = value;
		data.secondParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x06001C12 RID: 7186 RVA: 0x00095B98 File Offset: 0x00093D98
	public void UpdateObjectSelection(HandleVector<int>.Handle handle, Vector3 sound_pos, float vol, bool objectIsSelectedAndVisible)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.pos = sound_pos;
		data.vol = vol;
		data.objectIsSelectedAndVisible = objectIsSelectedAndVisible;
		ATTRIBUTES_3D attributes = sound_pos.To3DAttributes();
		if (data.IsPlaying)
		{
			data.ev.set3DAttributes(attributes);
			data.ev.setVolume(vol);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x06001C13 RID: 7187 RVA: 0x00095C04 File Offset: 0x00093E04
	public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.velocity = velocity;
		this.sounds.SetData(handle, data);
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x00095C34 File Offset: 0x00093E34
	public void RenderEveryTick(float dt)
	{
		ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
		List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
		bool flag = Time.timeScale == 0f;
		SoundCuller soundCuller = CameraController.Instance.soundCuller;
		for (int i = 0; i < dataList.Count; i++)
		{
			LoopingSoundManager.Sound sound = dataList[i];
			if (sound.objectIsSelectedAndVisible)
			{
				sound.pos = SoundEvent.AudioHighlightListenerPosition(sound.transform.GetPosition());
				sound.vol = 1f;
			}
			else if (sound.transform != null)
			{
				sound.pos = sound.transform.GetPosition();
				sound.pos.z = 0f;
			}
			if (sound.animController != null)
			{
				Vector3 offset = sound.animController.Offset;
				sound.pos.x = sound.pos.x + offset.x;
				sound.pos.y = sound.pos.y + offset.y;
			}
			bool flag2 = !sound.IsCullingEnabled || (sound.ShouldCameraScalePosition && soundCuller.IsAudible(sound.pos, sound.falloffDistanceSq)) || soundCuller.IsAudibleNoCameraScaling(sound.pos, sound.falloffDistanceSq);
			bool isPlaying = sound.IsPlaying;
			if (flag2)
			{
				pooledList.Add(sound);
				if (!isPlaying)
				{
					SoundDescription soundDescription = this.GetSoundDescription(sound.path);
					sound.ev = KFMOD.CreateInstance(soundDescription.path);
					dataList[i] = sound;
					pooledList2.Add(i);
				}
			}
			else if (isPlaying)
			{
				pooledList3.Add(i);
			}
		}
		foreach (int index in pooledList2)
		{
			LoopingSoundManager.Sound sound2 = dataList[index];
			SoundDescription soundDescription2 = this.GetSoundDescription(sound2.path);
			sound2.ev.setPaused(flag && sound2.ShouldPauseOnGamePaused);
			sound2.pos.z = 0f;
			Vector3 pos = sound2.pos;
			if (sound2.objectIsSelectedAndVisible)
			{
				sound2.pos = SoundEvent.AudioHighlightListenerPosition(sound2.transform.GetPosition());
				sound2.vol = 1f;
			}
			else if (sound2.transform != null)
			{
				sound2.pos = sound2.transform.GetPosition();
			}
			sound2.ev.set3DAttributes(pos.To3DAttributes());
			sound2.ev.setVolume(sound2.vol);
			sound2.ev.start();
			sound2.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
			if (sound2.firstParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.firstParameter), sound2.firstParameterValue, false);
			}
			if (sound2.secondParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.secondParameter), sound2.secondParameterValue, false);
			}
			LoopingSoundParameterUpdater.Sound sound3 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound2.ev,
				path = sound2.path,
				description = soundDescription2,
				transform = sound2.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter in soundDescription2.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (this.parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					loopingSoundParameterUpdater.Add(sound3);
				}
			}
			dataList[index] = sound2;
		}
		pooledList2.Recycle();
		foreach (int index2 in pooledList3)
		{
			LoopingSoundManager.Sound sound4 = dataList[index2];
			SoundDescription soundDescription3 = this.GetSoundDescription(sound4.path);
			LoopingSoundParameterUpdater.Sound sound5 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound4.ev,
				path = sound4.path,
				description = soundDescription3,
				transform = sound4.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter2 in soundDescription3.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater2 = null;
				if (this.parameterUpdaters.TryGetValue(parameter2.name, out loopingSoundParameterUpdater2))
				{
					loopingSoundParameterUpdater2.Remove(sound5);
				}
			}
			if (sound4.ShouldCameraScalePosition)
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			sound4.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
			sound4.ev.release();
			dataList[index2] = sound4;
		}
		pooledList3.Recycle();
		float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
		foreach (LoopingSoundManager.Sound sound6 in pooledList)
		{
			ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition(sound6.pos, sound6.objectIsSelectedAndVisible).To3DAttributes();
			attributes.velocity = (sound6.velocity * velocityScale).ToFMODVector();
			EventInstance ev = sound6.ev;
			ev.set3DAttributes(attributes);
		}
		foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> keyValuePair in this.parameterUpdaters)
		{
			keyValuePair.Value.Update(dt);
		}
		pooledList.Recycle();
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x00096270 File Offset: 0x00094470
	public static LoopingSoundManager Get()
	{
		return LoopingSoundManager.instance;
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x00096278 File Offset: 0x00094478
	public void StopAllSounds()
	{
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				ev = sound.ev;
				ev.release();
			}
		}
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x000962F4 File Offset: 0x000944F4
	private SoundDescription GetSoundDescription(HashedString path)
	{
		return KFMOD.GetSoundEventDescription(path);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000962FC File Offset: 0x000944FC
	public HandleVector<int>.Handle Add(string path, Vector3 pos, Transform transform = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true, float vol = 1f, bool objectIsSelectedAndVisible = false)
	{
		SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(path);
		LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags)0;
		if (pause_on_game_pause)
		{
			flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
		}
		if (enable_culling)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
		}
		if (enable_camera_scaled_position)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
		}
		KBatchedAnimController animController = null;
		if (transform != null)
		{
			animController = transform.GetComponent<KBatchedAnimController>();
		}
		LoopingSoundManager.Sound initial_data = new LoopingSoundManager.Sound
		{
			transform = transform,
			animController = animController,
			falloffDistanceSq = soundEventDescription.falloffDistanceSq,
			path = path,
			pos = pos,
			flags = flags,
			firstParameter = HashedString.Invalid,
			secondParameter = HashedString.Invalid,
			vol = vol,
			objectIsSelectedAndVisible = objectIsSelectedAndVisible
		};
		return this.sounds.Allocate(initial_data);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x000963BC File Offset: 0x000945BC
	public static HandleVector<int>.Handle StartSound(EventReference event_ref, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		return LoopingSoundManager.StartSound(KFMOD.GetEventReferencePath(event_ref), pos, pause_on_game_pause, enable_culling);
	}

	// Token: 0x06001C1A RID: 7194 RVA: 0x000963CC File Offset: 0x000945CC
	public static HandleVector<int>.Handle StartSound(string path, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogWarning("Missing sound");
			return HandleVector<int>.InvalidHandle;
		}
		return LoopingSoundManager.Get().Add(path, pos, null, pause_on_game_pause, enable_culling, true, 1f, false);
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x00096408 File Offset: 0x00094608
	public static void StopSound(HandleVector<int>.Handle handle)
	{
		if (LoopingSoundManager.Get() == null)
		{
			return;
		}
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.stop(LoopingSoundManager.Get().GameIsPaused ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			data.ev.release();
			SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(data.path);
			foreach (SoundDescription.Parameter parameter in soundEventDescription.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound
					{
						ev = data.ev,
						path = data.path,
						description = soundEventDescription,
						transform = data.transform,
						objectIsSelectedAndVisible = false
					};
					loopingSoundParameterUpdater.Remove(sound);
				}
			}
		}
		LoopingSoundManager.Get().sounds.Free(handle);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x00096510 File Offset: 0x00094710
	public static void PauseSound(HandleVector<int>.Handle handle, bool paused)
	{
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.setPaused(paused);
		}
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x00096548 File Offset: 0x00094748
	private void OnPauseChanged(object data)
	{
		bool flag = (bool)data;
		this.GameIsPaused = flag;
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.setPaused(flag && sound.ShouldPauseOnGamePaused);
			}
		}
	}

	// Token: 0x04000F7D RID: 3965
	private static LoopingSoundManager instance;

	// Token: 0x04000F7E RID: 3966
	private bool GameIsPaused;

	// Token: 0x04000F7F RID: 3967
	private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();

	// Token: 0x04000F80 RID: 3968
	private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>(0);

	// Token: 0x02001177 RID: 4471
	public class Tuning : TuningData<LoopingSoundManager.Tuning>
	{
		// Token: 0x04005C66 RID: 23654
		public float velocityScale;
	}

	// Token: 0x02001178 RID: 4472
	public struct Sound
	{
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x060079BB RID: 31163 RVA: 0x002DA150 File Offset: 0x002D8350
		public bool IsPlaying
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x060079BC RID: 31164 RVA: 0x002DA15D File Offset: 0x002D835D
		public bool ShouldPauseOnGamePaused
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x060079BD RID: 31165 RVA: 0x002DA16A File Offset: 0x002D836A
		public bool IsCullingEnabled
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x060079BE RID: 31166 RVA: 0x002DA177 File Offset: 0x002D8377
		public bool ShouldCameraScalePosition
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x04005C67 RID: 23655
		public EventInstance ev;

		// Token: 0x04005C68 RID: 23656
		public Transform transform;

		// Token: 0x04005C69 RID: 23657
		public KBatchedAnimController animController;

		// Token: 0x04005C6A RID: 23658
		public float falloffDistanceSq;

		// Token: 0x04005C6B RID: 23659
		public HashedString path;

		// Token: 0x04005C6C RID: 23660
		public Vector3 pos;

		// Token: 0x04005C6D RID: 23661
		public Vector2 velocity;

		// Token: 0x04005C6E RID: 23662
		public HashedString firstParameter;

		// Token: 0x04005C6F RID: 23663
		public HashedString secondParameter;

		// Token: 0x04005C70 RID: 23664
		public float firstParameterValue;

		// Token: 0x04005C71 RID: 23665
		public float secondParameterValue;

		// Token: 0x04005C72 RID: 23666
		public float vol;

		// Token: 0x04005C73 RID: 23667
		public bool objectIsSelectedAndVisible;

		// Token: 0x04005C74 RID: 23668
		public LoopingSoundManager.Sound.Flags flags;

		// Token: 0x02002097 RID: 8343
		[Flags]
		public enum Flags
		{
			// Token: 0x04009199 RID: 37273
			PLAYING = 1,
			// Token: 0x0400919A RID: 37274
			PAUSE_ON_GAME_PAUSED = 2,
			// Token: 0x0400919B RID: 37275
			ENABLE_CULLING = 4,
			// Token: 0x0400919C RID: 37276
			ENABLE_CAMERA_SCALED_POSITION = 8
		}
	}
}
