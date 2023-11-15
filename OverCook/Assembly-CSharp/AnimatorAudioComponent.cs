using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class AnimatorAudioComponent : MonoBehaviour
{
	// Token: 0x0600054E RID: 1358 RVA: 0x00029DA8 File Offset: 0x000281A8
	private void AudioTrigger(string _tag)
	{
		Animator animator = base.gameObject.RequireComponent<Animator>();
		if (AnimatorAudioComponent.ShouldPlayEvent(animator, "AudioTrigger", _tag))
		{
			GameOneShotAudioTag audio = (GameOneShotAudioTag)Enum.Parse(typeof(GameOneShotAudioTag), _tag, true);
			GameUtils.TriggerAudio(audio, base.gameObject.layer);
		}
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00029DFC File Offset: 0x000281FC
	private void AudioStart(string _tag)
	{
		Animator animator = base.gameObject.RequireComponent<Animator>();
		if (AnimatorAudioComponent.ShouldPlayEvent(animator, "AudioTrigger", _tag))
		{
			GameLoopingAudioTag audio = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
			GameUtils.StartAudio(audio, this, base.gameObject.layer);
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00029E50 File Offset: 0x00028250
	private void AudioStop(string _tag)
	{
		GameLoopingAudioTag audio = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
		GameUtils.StopAudio(audio, this);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00029E7C File Offset: 0x0002827C
	public static bool ShouldPlayEvent(Animator _animator, string _eventName, string _eventArguement)
	{
		bool flag = false;
		int layerCount = _animator.layerCount;
		for (int i = 0; i < layerCount; i++)
		{
			AnimatorClipInfo[] currentAnimatorClipInfo = _animator.GetCurrentAnimatorClipInfo(i);
			for (int j = 0; j < currentAnimatorClipInfo.Length; j++)
			{
				AnimationClip clip = currentAnimatorClipInfo[j].clip;
				foreach (AnimationEvent animationEvent in clip.events)
				{
					if (animationEvent.functionName == _eventName && animationEvent.stringParameter == _eventArguement)
					{
						flag = true;
						if (currentAnimatorClipInfo[j].weight > 0.5f)
						{
							return true;
						}
					}
				}
			}
		}
		return !flag;
	}
}
