using System;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200045A RID: 1114
[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06001846 RID: 6214 RVA: 0x0007E3FF File Offset: 0x0007C5FF
	// (set) Token: 0x06001847 RID: 6215 RVA: 0x0007E407 File Offset: 0x0007C607
	public string sound { get; private set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06001848 RID: 6216 RVA: 0x0007E410 File Offset: 0x0007C610
	// (set) Token: 0x06001849 RID: 6217 RVA: 0x0007E418 File Offset: 0x0007C618
	public HashedString soundHash { get; private set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600184A RID: 6218 RVA: 0x0007E421 File Offset: 0x0007C621
	// (set) Token: 0x0600184B RID: 6219 RVA: 0x0007E429 File Offset: 0x0007C629
	public bool looping { get; private set; }

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600184C RID: 6220 RVA: 0x0007E432 File Offset: 0x0007C632
	// (set) Token: 0x0600184D RID: 6221 RVA: 0x0007E43A File Offset: 0x0007C63A
	public bool ignorePause { get; set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600184E RID: 6222 RVA: 0x0007E443 File Offset: 0x0007C643
	// (set) Token: 0x0600184F RID: 6223 RVA: 0x0007E44B File Offset: 0x0007C64B
	public bool shouldCameraScalePosition { get; set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06001850 RID: 6224 RVA: 0x0007E454 File Offset: 0x0007C654
	// (set) Token: 0x06001851 RID: 6225 RVA: 0x0007E45C File Offset: 0x0007C65C
	public float minInterval { get; private set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06001852 RID: 6226 RVA: 0x0007E465 File Offset: 0x0007C665
	// (set) Token: 0x06001853 RID: 6227 RVA: 0x0007E46D File Offset: 0x0007C66D
	public bool objectIsSelectedAndVisible { get; set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06001854 RID: 6228 RVA: 0x0007E476 File Offset: 0x0007C676
	// (set) Token: 0x06001855 RID: 6229 RVA: 0x0007E47E File Offset: 0x0007C67E
	public EffectorValues noiseValues { get; set; }

	// Token: 0x06001856 RID: 6230 RVA: 0x0007E487 File Offset: 0x0007C687
	public SoundEvent()
	{
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x0007E490 File Offset: 0x0007C690
	public SoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame)
	{
		this.shouldCameraScalePosition = true;
		if (do_load)
		{
			this.sound = GlobalAssets.GetSound(sound_name, false);
			this.soundHash = new HashedString(this.sound);
			string.IsNullOrEmpty(this.sound);
		}
		this.minInterval = min_interval;
		this.looping = is_looping;
		this.isDynamic = is_dynamic;
		this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x0007E505 File Offset: 0x0007C705
	public static bool ObjectIsSelectedAndVisible(GameObject go)
	{
		return false;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x0007E508 File Offset: 0x0007C708
	public static Vector3 AudioHighlightListenerPosition(Vector3 sound_pos)
	{
		Vector3 position = SoundListenerController.Instance.transform.position;
		float x = 1f * sound_pos.x + 0f * position.x;
		float y = 1f * sound_pos.y + 0f * position.y;
		float z = 0f * position.z;
		return new Vector3(x, y, z);
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x0007E570 File Offset: 0x0007C770
	public static float GetVolume(bool objectIsSelectedAndVisible)
	{
		float result = 1f;
		if (objectIsSelectedAndVisible)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0007E58D File Offset: 0x0007C78D
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, bool is_looping, bool is_dynamic)
	{
		return SoundEvent.ShouldPlaySound(controller, sound, sound, is_looping, is_dynamic);
	}

	// Token: 0x0600185C RID: 6236 RVA: 0x0007E5A0 File Offset: 0x0007C7A0
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, HashedString soundHash, bool is_looping, bool is_dynamic)
	{
		CameraController instance = CameraController.Instance;
		if (instance == null)
		{
			return true;
		}
		Vector3 position = controller.transform.GetPosition();
		Vector3 offset = controller.Offset;
		position.x += offset.x;
		position.y += offset.y;
		if (!SoundCuller.IsAudibleWorld(position))
		{
			return false;
		}
		SpeedControlScreen instance2 = SpeedControlScreen.Instance;
		if (is_dynamic)
		{
			return (!(instance2 != null) || !instance2.IsPaused) && instance.IsAudibleSound(position);
		}
		if (sound == null || SoundEvent.IsLowPrioritySound(sound))
		{
			return false;
		}
		if (!instance.IsAudibleSound(position, soundHash))
		{
			if (!is_looping && !GlobalAssets.IsHighPriority(sound))
			{
				return false;
			}
		}
		else if (instance2 != null && instance2.IsPaused)
		{
			return false;
		}
		return true;
	}

	// Token: 0x0600185D RID: 6237 RVA: 0x0007E66C File Offset: 0x0007C86C
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x0600185E RID: 6238 RVA: 0x0007E6C8 File Offset: 0x0007C8C8
	protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
	{
		Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		KBatchedAnimController component = behaviour.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			Vector3 offset = component.Offset;
			vector.x += offset.x;
			vector.y += offset.y;
		}
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector2 = vector;
			array[num] = vector2.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			if (this.looping)
			{
				LoopingSounds component2 = behaviour.GetComponent<LoopingSounds>();
				if (component2 == null)
				{
					global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
				}
				else if (!component2.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
					});
				}
			}
			else if (!SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues, SoundEvent.GetVolume(this.objectIsSelectedAndVisible), this.objectIsSelectedAndVisible))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
				});
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound != null) ? sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x0600185F RID: 6239 RVA: 0x0007E8C8 File Offset: 0x0007CAC8
	public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour, this.sound);
	}

	// Token: 0x06001860 RID: 6240 RVA: 0x0007E8D8 File Offset: 0x0007CAD8
	public static Vector3 GetCameraScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		Vector3 result = Vector3.zero;
		if (CameraController.Instance != null)
		{
			result = CameraController.Instance.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
		}
		return result;
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x0007E906 File Offset: 0x0007CB06
	public static FMOD.Studio.EventInstance BeginOneShot(EventReference event_ref, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return KFMOD.BeginOneShot(event_ref, SoundEvent.GetCameraScaledPosition(pos, objectIsSelectedAndVisible), volume);
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x0007E916 File Offset: 0x0007CB16
	public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return SoundEvent.BeginOneShot(RuntimeManager.PathToEventReference(ev), pos, volume, false);
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x0007E926 File Offset: 0x0007CB26
	public static bool EndOneShot(FMOD.Studio.EventInstance instance)
	{
		return KFMOD.EndOneShot(instance);
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0007E930 File Offset: 0x0007CB30
	public static bool PlayOneShot(EventReference event_ref, Vector3 sound_pos, float volume = 1f)
	{
		bool result = false;
		if (!event_ref.IsNull)
		{
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(event_ref, sound_pos, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x0007E963 File Offset: 0x0007CB63
	public static bool PlayOneShot(string sound, Vector3 sound_pos, float volume = 1f)
	{
		return SoundEvent.PlayOneShot(RuntimeManager.PathToEventReference(sound), sound_pos, volume);
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0007E974 File Offset: 0x0007CB74
	public static bool PlayOneShot(string sound, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(sound))
		{
			Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
			vector.z = 0f;
			if (objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x0007E9CC File Offset: 0x0007CBCC
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(this.sound);
			}
		}
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x0007E9FE File Offset: 0x0007CBFE
	protected static bool IsLowPrioritySound(string sound)
	{
		return sound != null && Camera.main != null && Camera.main.orthographicSize > AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && !AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound);
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x0007EA38 File Offset: 0x0007CC38
	protected void PrintSoundDebug(string anim_name, string sound, string sound_name, Vector3 sound_pos)
	{
		if (sound != null)
		{
			string[] array = new string[7];
			array[0] = anim_name;
			array[1] = ", ";
			array[2] = sound_name;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = sound_pos;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
			return;
		}
		global::Debug.Log("Missing sound: " + anim_name + ", " + sound_name);
	}

	// Token: 0x04000D72 RID: 3442
	public static int IGNORE_INTERVAL = -1;

	// Token: 0x04000D7B RID: 3451
	protected bool isDynamic;
}
