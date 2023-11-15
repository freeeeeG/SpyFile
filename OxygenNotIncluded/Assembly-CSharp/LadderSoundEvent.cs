using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class LadderSoundEvent : SoundEvent
{
	// Token: 0x0600182E RID: 6190 RVA: 0x0007D952 File Offset: 0x0007BB52
	public LadderSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, false, false, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x0007D968 File Offset: 0x0007BB68
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.looping, this.isDynamic))
		{
			Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
			vector.z = 0f;
			float volume = 1f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
				volume = SoundEvent.GetVolume(base.objectIsSelectedAndVisible);
			}
			int cell = Grid.PosToCell(vector);
			BuildingDef buildingDef = null;
			if (Grid.IsValidCell(cell))
			{
				GameObject gameObject2 = Grid.Objects[cell, 1];
				if (gameObject2 != null && gameObject2.GetComponent<Ladder>() != null)
				{
					Building component = gameObject2.GetComponent<BuildingComplete>();
					if (component != null)
					{
						buildingDef = component.Def;
					}
				}
			}
			if (buildingDef != null)
			{
				string sound = GlobalAssets.GetSound((buildingDef.PrefabID == "LadderFast") ? StringFormatter.Combine(base.name, "_Plastic") : base.name, false);
				if (sound != null)
				{
					SoundEvent.PlayOneShot(sound, vector, volume);
				}
			}
		}
	}
}
