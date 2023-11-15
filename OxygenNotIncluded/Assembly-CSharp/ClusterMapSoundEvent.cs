using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class ClusterMapSoundEvent : SoundEvent
{
	// Token: 0x06001707 RID: 5895 RVA: 0x0007742C File Offset: 0x0007562C
	public ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x00077441 File Offset: 0x00075641
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (ClusterMapScreen.Instance.IsActive())
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x00077458 File Offset: 0x00075658
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (Cluster Map Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, true, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
			instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float)Screen.width, false);
			instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float)Screen.height, false);
			instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x00077558 File Offset: 0x00075758
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}

	// Token: 0x04000CCA RID: 3274
	private static string X_POSITION_PARAMETER = "Starmap_Position_X";

	// Token: 0x04000CCB RID: 3275
	private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";

	// Token: 0x04000CCC RID: 3276
	private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";
}
