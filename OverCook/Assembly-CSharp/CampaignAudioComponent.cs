using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class CampaignAudioComponent : MonoBehaviour
{
	// Token: 0x0600056F RID: 1391 RVA: 0x0002A346 File Offset: 0x00028746
	private void Awake()
	{
		this.m_audioManager = GameUtils.RequireManager<CampaignAudioManager>();
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0002A354 File Offset: 0x00028754
	private void MusicStart(int _musicTrack)
	{
		if (_musicTrack >= 0)
		{
			if (_musicTrack < this.m_musicFiles.Length)
			{
				AudioClip audioFile = this.m_musicFiles[_musicTrack];
				this.m_audioManager.SetMusic(audioFile, false);
			}
		}
		else
		{
			this.m_audioManager.SetMusic(null, false);
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0002A3A0 File Offset: 0x000287A0
	private void AmbienceStart(string _tag)
	{
		GameLoopingAudioTag tag = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
		this.m_audioManager.StartAmbience(tag);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0002A3D0 File Offset: 0x000287D0
	private void AmbienceStop(string _tag)
	{
		GameLoopingAudioTag tag = (GameLoopingAudioTag)Enum.Parse(typeof(GameLoopingAudioTag), _tag, true);
		this.m_audioManager.StopAmbience(tag);
	}

	// Token: 0x040004A5 RID: 1189
	[SerializeField]
	private AudioClip[] m_musicFiles = new AudioClip[0];

	// Token: 0x040004A6 RID: 1190
	private CampaignAudioManager m_audioManager;
}
