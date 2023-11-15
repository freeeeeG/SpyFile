using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class PlantMutationSoundEvent : SoundEvent
{
	// Token: 0x0600183A RID: 6202 RVA: 0x0007DE70 File Offset: 0x0007C070
	public PlantMutationSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x0007DE80 File Offset: 0x0007C080
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
		Vector3 position = behaviour.GetComponent<Transform>().GetPosition();
		if (component != null)
		{
			for (int i = 0; i < component.GetSoundEvents().Count; i++)
			{
				SoundEvent.PlayOneShot(component.GetSoundEvents()[i], position, 1f);
			}
		}
	}
}
