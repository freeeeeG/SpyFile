using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class HatchDrillSoundEvent : SoundEvent
{
	// Token: 0x0600171A RID: 5914 RVA: 0x00077D85 File Offset: 0x00075F85
	public HatchDrillSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x00077D98 File Offset: 0x00075F98
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		float value = (float)HatchDrillSoundEvent.GetAudioCategory(Grid.CellBelow(Grid.PosToCell(vector)));
		EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, 1f, false);
		instance.setParameterByName("material_ID", value, false);
		SoundEvent.EndOneShot(instance);
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x00077E14 File Offset: 0x00076014
	private static int GetAudioCategory(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return 7;
		}
		Element element = Grid.Element[cell];
		if (element.id == SimHashes.Dirt)
		{
			return 0;
		}
		if (element.HasTag(GameTags.IceOre))
		{
			return 1;
		}
		if (element.id == SimHashes.CrushedIce)
		{
			return 12;
		}
		if (element.id == SimHashes.DirtyIce)
		{
			return 13;
		}
		if (Grid.Foundation[cell])
		{
			return 2;
		}
		if (element.id == SimHashes.OxyRock)
		{
			return 3;
		}
		if (element.id == SimHashes.PhosphateNodules || element.id == SimHashes.Phosphorus || element.id == SimHashes.Phosphorite)
		{
			return 4;
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
		if (element.id == SimHashes.Clay)
		{
			return 9;
		}
		if (element.id == SimHashes.Algae)
		{
			return 10;
		}
		if (element.id == SimHashes.SlimeMold)
		{
			return 11;
		}
		return 7;
	}
}
