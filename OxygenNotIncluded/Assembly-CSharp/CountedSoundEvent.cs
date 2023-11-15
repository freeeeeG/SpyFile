﻿using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class CountedSoundEvent : SoundEvent
{
	// Token: 0x0600170C RID: 5900 RVA: 0x000775AC File Offset: 0x000757AC
	private static string BaseSoundName(string sound_name)
	{
		int num = sound_name.IndexOf(":");
		if (num > 0)
		{
			return sound_name.Substring(0, num);
		}
		return sound_name;
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x000775D4 File Offset: 0x000757D4
	public CountedSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, CountedSoundEvent.BaseSoundName(sound_name), frame, do_load, is_looping, min_interval, is_dynamic)
	{
		if (sound_name.Contains(":"))
		{
			string[] array = sound_name.Split(new char[]
			{
				':'
			});
			if (array.Length != 2)
			{
				DebugUtil.LogErrorArgs(new object[]
				{
					"Invalid CountedSoundEvent parameter for",
					string.Concat(new string[]
					{
						file_name,
						".",
						sound_name,
						".",
						frame.ToString(),
						":"
					}),
					"'" + sound_name + "'"
				});
			}
			for (int i = 1; i < array.Length; i++)
			{
				this.ParseParameter(array[i]);
			}
			return;
		}
		DebugUtil.LogErrorArgs(new object[]
		{
			"CountedSoundEvent for",
			string.Concat(new string[]
			{
				file_name,
				".",
				sound_name,
				".",
				frame.ToString()
			}),
			" - Must specify max number of steps on event: '" + sound_name + "'"
		});
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x000776F8 File Offset: 0x000758F8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (string.IsNullOrEmpty(base.sound))
		{
			return;
		}
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
		{
			int num = -1;
			if (this.counterModulus >= -1)
			{
				HandleVector<int>.Handle h = GameComps.WhiteBoards.GetHandle(gameObject);
				if (!h.IsValid())
				{
					h = GameComps.WhiteBoards.Add(gameObject);
				}
				num = (GameComps.WhiteBoards.HasValue(h, base.soundHash) ? ((int)GameComps.WhiteBoards.GetValue(h, base.soundHash)) : 0);
				int num2 = (this.counterModulus == -1) ? 0 : ((num + 1) % this.counterModulus);
				GameComps.WhiteBoards.SetValue(h, base.soundHash, num2);
			}
			Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
			vector.z = 0f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
			if (instance.isValid())
			{
				if (num >= 0)
				{
					instance.setParameterByName("eventCount", (float)num, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x00077851 File Offset: 0x00075A51
	private void ParseParameter(string param)
	{
		this.counterModulus = int.Parse(param);
		if (this.counterModulus != -1 && this.counterModulus < 2)
		{
			throw new ArgumentException("CountedSoundEvent modulus must be 2 or larger");
		}
	}

	// Token: 0x04000CCD RID: 3277
	private const int COUNTER_MODULUS_INVALID = -2147483648;

	// Token: 0x04000CCE RID: 3278
	private const int COUNTER_MODULUS_CLEAR = -1;

	// Token: 0x04000CCF RID: 3279
	private int counterModulus = int.MinValue;
}
