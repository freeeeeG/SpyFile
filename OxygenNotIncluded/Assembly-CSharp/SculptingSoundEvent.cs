﻿using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class SculptingSoundEvent : SoundEvent
{
	// Token: 0x0600183E RID: 6206 RVA: 0x0007DFE8 File Offset: 0x0007C1E8
	private static string BaseSoundName(string sound_name)
	{
		int num = sound_name.IndexOf(":");
		if (num > 0)
		{
			return sound_name.Substring(0, num);
		}
		return sound_name;
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x0007E010 File Offset: 0x0007C210
	public SculptingSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, SculptingSoundEvent.BaseSoundName(sound_name), frame, do_load, is_looping, min_interval, is_dynamic)
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

	// Token: 0x06001840 RID: 6208 RVA: 0x0007E134 File Offset: 0x0007C334
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
			float volume = 1f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
				volume = SoundEvent.GetVolume(base.objectIsSelectedAndVisible);
			}
			else
			{
				vector.z = 0f;
			}
			string sound = GlobalAssets.GetSound("Hammer_sculpture", false);
			Worker component = behaviour.GetComponent<Worker>();
			if (component != null)
			{
				Workable workable = component.workable;
				if (workable != null)
				{
					Building component2 = workable.GetComponent<Building>();
					if (component2 != null)
					{
						string name = component2.Def.name;
						if (name != null)
						{
							if (!(name == "MetalSculpture"))
							{
								if (name == "MarbleSculpture")
								{
									sound = GlobalAssets.GetSound("Hammer_sculpture_marble", false);
								}
							}
							else
							{
								sound = GlobalAssets.GetSound("Hammer_sculpture_metal", false);
							}
						}
					}
				}
			}
			EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
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

	// Token: 0x06001841 RID: 6209 RVA: 0x0007E327 File Offset: 0x0007C527
	private void ParseParameter(string param)
	{
		this.counterModulus = int.Parse(param);
		if (this.counterModulus != -1 && this.counterModulus < 2)
		{
			throw new ArgumentException("CountedSoundEvent modulus must be 2 or larger");
		}
	}

	// Token: 0x04000D6E RID: 3438
	private const int COUNTER_MODULUS_INVALID = -2147483648;

	// Token: 0x04000D6F RID: 3439
	private const int COUNTER_MODULUS_CLEAR = -1;

	// Token: 0x04000D70 RID: 3440
	private int counterModulus = int.MinValue;
}
