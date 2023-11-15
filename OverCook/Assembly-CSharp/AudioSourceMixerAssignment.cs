using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000125 RID: 293
public class AudioSourceMixerAssignment : MonoBehaviour
{
	// Token: 0x06000563 RID: 1379 RVA: 0x0002A19C File Offset: 0x0002859C
	private void Awake()
	{
		AudioManager audioManager = GameUtils.RequestManager<AudioManager>();
		AudioMixer audioMixer = audioManager.m_audioMixer;
		this.m_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(this.m_groupName)[0];
	}

	// Token: 0x0400049E RID: 1182
	[SerializeField]
	private string m_groupName;

	// Token: 0x0400049F RID: 1183
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private AudioSource m_audioSource;
}
