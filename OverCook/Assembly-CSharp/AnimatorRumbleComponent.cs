using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class AnimatorRumbleComponent : MonoBehaviour
{
	// Token: 0x0600055D RID: 1373 RVA: 0x0002A0B4 File Offset: 0x000284B4
	private void RumbleTrigger(string _tag)
	{
		Animator animator = base.gameObject.RequireComponent<Animator>();
		if (AnimatorAudioComponent.ShouldPlayEvent(animator, "RumbleTrigger", _tag))
		{
			GameOneShotAudioTag audio = (GameOneShotAudioTag)Enum.Parse(typeof(GameOneShotAudioTag), _tag, true);
			GameUtils.TriggerNXRumble(audio);
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0002A0FC File Offset: 0x000284FC
	private void RumbleStart(string _tag)
	{
		Animator animator = base.gameObject.RequireComponent<Animator>();
		if (AnimatorAudioComponent.ShouldPlayEvent(animator, "RumbleTrigger", _tag))
		{
			GameLoopingAudioTag audio = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
			GameUtils.StartNXRumble(audio);
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0002A144 File Offset: 0x00028544
	private void RumbleStop(string _tag)
	{
		GameLoopingAudioTag audio = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
		GameUtils.StopNXRumble(audio);
	}
}
