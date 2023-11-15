﻿using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class WallDamageSoundEvent : SoundEvent
{
	// Token: 0x06001904 RID: 6404 RVA: 0x00082B60 File Offset: 0x00080D60
	public WallDamageSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00082B70 File Offset: 0x00080D70
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = default(Vector3);
		AggressiveChore.StatesInstance smi = behaviour.controller.gameObject.GetSMI<AggressiveChore.StatesInstance>();
		if (smi != null)
		{
			this.tile = smi.sm.wallCellToBreak;
			int audioCategory = WallDamageSoundEvent.GetAudioCategory(this.tile);
			vector = Grid.CellToPos(this.tile);
			vector.z = 0f;
			GameObject gameObject = behaviour.controller.gameObject;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
			{
				EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
				instance.setParameterByName("material_ID", (float)audioCategory, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00082C4C File Offset: 0x00080E4C
	private static int GetAudioCategory(int tile)
	{
		Element element = Grid.Element[tile];
		if (Grid.Foundation[tile])
		{
			return 12;
		}
		if (element.id == SimHashes.Dirt)
		{
			return 0;
		}
		if (element.id == SimHashes.CrushedIce || element.id == SimHashes.Ice || element.id == SimHashes.DirtyIce)
		{
			return 1;
		}
		if (element.id == SimHashes.OxyRock)
		{
			return 3;
		}
		if (element.HasTag(GameTags.Metal))
		{
			return 5;
		}
		if (element.HasTag(GameTags.RefinedMetal))
		{
			return 6;
		}
		if (element.id == SimHashes.Sand)
		{
			return 8;
		}
		if (element.id == SimHashes.Algae)
		{
			return 10;
		}
		return 7;
	}

	// Token: 0x04000DD2 RID: 3538
	public int tile;
}
