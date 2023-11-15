using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000C8E RID: 3214
public class VideoScreen : KModalScreen
{
	// Token: 0x06006660 RID: 26208 RVA: 0x00262ED8 File Offset: 0x002610D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
		this.proceedButton.onClick += delegate()
		{
			this.Stop();
		};
		this.videoPlayer.isLooping = false;
		this.videoPlayer.loopPointReached += delegate(VideoPlayer data)
		{
			if (this.victoryLoopQueued)
			{
				base.StartCoroutine(this.SwitchToVictoryLoop());
				return;
			}
			if (!this.videoPlayer.isLooping)
			{
				this.Stop();
			}
		};
		VideoScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06006661 RID: 26209 RVA: 0x00262F50 File Offset: 0x00261150
	protected override void OnForcedCleanUp()
	{
		VideoScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006662 RID: 26210 RVA: 0x00262F5E File Offset: 0x0026115E
	protected override void OnShow(bool show)
	{
		base.transform.SetAsLastSibling();
		base.OnShow(show);
		this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
	}

	// Token: 0x06006663 RID: 26211 RVA: 0x00262F88 File Offset: 0x00261188
	public void DisableAllMedia()
	{
		this.overlayContainer.gameObject.SetActive(false);
		this.videoPlayer.gameObject.SetActive(false);
		this.slideshow.gameObject.SetActive(false);
	}

	// Token: 0x06006664 RID: 26212 RVA: 0x00262FC0 File Offset: 0x002611C0
	public void PlaySlideShow(Sprite[] sprites)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetSprites(sprites);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06006665 RID: 26213 RVA: 0x00263010 File Offset: 0x00261210
	public void PlaySlideShow(string[] files)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetFiles(files, 0);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x06006666 RID: 26214 RVA: 0x00263060 File Offset: 0x00261260
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			if (this.slideshow.gameObject.activeSelf && e.TryConsume(global::Action.Escape))
			{
				this.Stop();
				return;
			}
			if (e.TryConsume(global::Action.Escape))
			{
				if (this.videoSkippable)
				{
					this.Stop();
				}
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006667 RID: 26215 RVA: 0x002630B8 File Offset: 0x002612B8
	public void PlayVideo(VideoClip clip, bool unskippable = false, EventReference overrideAudioSnapshot = default(EventReference), bool showProceedButton = false)
	{
		global::Debug.Assert(clip != null);
		for (int i = 0; i < this.overlayContainer.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.overlayContainer.GetChild(i).gameObject);
		}
		this.Show(true);
		this.videoPlayer.isLooping = false;
		this.activeAudioSnapshot = (overrideAudioSnapshot.IsNull ? AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot : overrideAudioSnapshot);
		AudioMixer.instance.Start(this.activeAudioSnapshot);
		this.DisableAllMedia();
		this.videoPlayer.gameObject.SetActive(true);
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.screen.texture = this.renderTexture;
		this.videoPlayer.targetTexture = this.renderTexture;
		this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.videoPlayer.clip = clip;
		this.videoPlayer.Play();
		if (this.audioHandle.isValid())
		{
			KFMOD.EndOneShot(this.audioHandle);
			this.audioHandle.clearHandle();
		}
		this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name, false), Vector3.zero, 1f);
		KFMOD.EndOneShot(this.audioHandle);
		this.videoSkippable = !unskippable;
		this.closeButton.gameObject.SetActive(this.videoSkippable);
		this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
	}

	// Token: 0x06006668 RID: 26216 RVA: 0x00263254 File Offset: 0x00261454
	public void QueueVictoryVideoLoop(bool queue, string message = "", string victoryAchievement = "", string loopVideo = "")
	{
		this.victoryLoopQueued = queue;
		this.victoryLoopMessage = message;
		this.victoryLoopClip = loopVideo;
		this.OnStop = (System.Action)Delegate.Combine(this.OnStop, new System.Action(delegate()
		{
			RetireColonyUtility.SaveColonySummaryData();
			MainMenu.ActivateRetiredColoniesScreenFromData(base.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		}));
	}

	// Token: 0x06006669 RID: 26217 RVA: 0x00263290 File Offset: 0x00261490
	public void SetOverlayText(string overlayTemplate, List<string> strings)
	{
		VideoOverlay videoOverlay = null;
		foreach (VideoOverlay videoOverlay2 in this.overlayPrefabs)
		{
			if (videoOverlay2.name == overlayTemplate)
			{
				videoOverlay = videoOverlay2;
				break;
			}
		}
		DebugUtil.Assert(videoOverlay != null, "Could not find a template named ", overlayTemplate);
		global::Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
		this.overlayContainer.gameObject.SetActive(true);
	}

	// Token: 0x0600666A RID: 26218 RVA: 0x00263330 File Offset: 0x00261530
	private IEnumerator SwitchToVictoryLoop()
	{
		this.victoryLoopQueued = false;
		Color color = this.fadeOverlay.color;
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
		MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary", false);
		MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f, true);
		this.closeButton.gameObject.SetActive(true);
		this.proceedButton.gameObject.SetActive(true);
		this.SetOverlayText("VictoryEnd", new List<string>
		{
			this.victoryLoopMessage
		});
		this.videoPlayer.clip = Assets.GetVideo(this.victoryLoopClip);
		this.videoPlayer.isLooping = true;
		this.videoPlayer.Play();
		this.proceedButton.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		for (float i = 1f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 0f);
		yield break;
	}

	// Token: 0x0600666B RID: 26219 RVA: 0x00263340 File Offset: 0x00261540
	public void Stop()
	{
		this.videoPlayer.Stop();
		this.screen.texture = null;
		this.videoPlayer.targetTexture = null;
		if (!this.activeAudioSnapshot.IsNull)
		{
			AudioMixer.instance.Stop(this.activeAudioSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.audioHandle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.OnStop != null)
		{
			this.OnStop();
		}
		this.Show(false);
	}

	// Token: 0x0600666C RID: 26220 RVA: 0x002633B8 File Offset: 0x002615B8
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.audioHandle.isValid())
		{
			int num;
			this.audioHandle.getTimelinePosition(out num);
			double num2 = this.videoPlayer.time * 1000.0;
			if ((double)num - num2 > 33.0)
			{
				VideoPlayer videoPlayer = this.videoPlayer;
				long frame = videoPlayer.frame;
				videoPlayer.frame = frame + 1L;
				return;
			}
			if (num2 - (double)num > 33.0)
			{
				VideoPlayer videoPlayer2 = this.videoPlayer;
				long frame = videoPlayer2.frame;
				videoPlayer2.frame = frame - 1L;
			}
		}
	}

	// Token: 0x04004684 RID: 18052
	public static VideoScreen Instance;

	// Token: 0x04004685 RID: 18053
	[SerializeField]
	private VideoPlayer videoPlayer;

	// Token: 0x04004686 RID: 18054
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x04004687 RID: 18055
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004688 RID: 18056
	[SerializeField]
	private KButton proceedButton;

	// Token: 0x04004689 RID: 18057
	[SerializeField]
	private RectTransform overlayContainer;

	// Token: 0x0400468A RID: 18058
	[SerializeField]
	private List<VideoOverlay> overlayPrefabs;

	// Token: 0x0400468B RID: 18059
	private RawImage screen;

	// Token: 0x0400468C RID: 18060
	private RenderTexture renderTexture;

	// Token: 0x0400468D RID: 18061
	private EventReference activeAudioSnapshot;

	// Token: 0x0400468E RID: 18062
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x0400468F RID: 18063
	private EventInstance audioHandle;

	// Token: 0x04004690 RID: 18064
	private bool victoryLoopQueued;

	// Token: 0x04004691 RID: 18065
	private string victoryLoopMessage = "";

	// Token: 0x04004692 RID: 18066
	private string victoryLoopClip = "";

	// Token: 0x04004693 RID: 18067
	private bool videoSkippable = true;

	// Token: 0x04004694 RID: 18068
	public System.Action OnStop;
}
