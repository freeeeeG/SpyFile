using System;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace UnityEngine.Timeline
{
	// Token: 0x020000C0 RID: 192
	public class VideoPlayableBehaviour : PlayableBehaviour
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x0000AF78 File Offset: 0x00009178
		public void PrepareVideo()
		{
			if (this.videoPlayer == null || this.videoClip == null)
			{
				return;
			}
			this.videoPlayer.targetCameraAlpha = 0f;
			if (this.videoPlayer.clip != this.videoClip)
			{
				this.StopVideo();
			}
			if (this.videoPlayer.isPrepared || this.preparing)
			{
				return;
			}
			this.videoPlayer.source = VideoSource.VideoClip;
			this.videoPlayer.clip = this.videoClip;
			this.videoPlayer.playOnAwake = false;
			this.videoPlayer.waitForFirstFrame = true;
			this.videoPlayer.isLooping = this.loop;
			for (ushort num = 0; num < this.videoClip.audioTrackCount; num += 1)
			{
				if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
				{
					this.videoPlayer.SetDirectAudioMute(num, this.mute || !Application.isPlaying);
				}
				else if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
				{
					AudioSource targetAudioSource = this.videoPlayer.GetTargetAudioSource(num);
					if (targetAudioSource != null)
					{
						targetAudioSource.mute = (this.mute || !Application.isPlaying);
					}
				}
			}
			this.videoPlayer.loopPointReached += this.LoopPointReached;
			this.videoPlayer.time = this.clipInTime;
			this.videoPlayer.Prepare();
			this.preparing = true;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B0E9 File Offset: 0x000092E9
		private void LoopPointReached(VideoPlayer vp)
		{
			this.playedOnce = !this.loop;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B0FC File Offset: 0x000092FC
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (this.videoPlayer == null || this.videoClip == null)
			{
				return;
			}
			this.videoPlayer.timeReference = (Application.isPlaying ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun);
			if (this.videoPlayer.isPlaying && Application.isPlaying)
			{
				this.videoPlayer.externalReferenceTime = playable.GetTime<Playable>();
				return;
			}
			if (!Application.isPlaying)
			{
				this.SyncVideoToPlayable(playable);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B170 File Offset: 0x00009370
		public override void OnBehaviourPlay(Playable playable, FrameData info)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			if (!this.playedOnce)
			{
				this.PlayVideo();
				this.SyncVideoToPlayable(playable);
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000B196 File Offset: 0x00009396
		public override void OnBehaviourPause(Playable playable, FrameData info)
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				this.PauseVideo();
				return;
			}
			this.StopVideo();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000B1BC File Offset: 0x000093BC
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			if (this.videoPlayer == null || this.videoPlayer.clip == null)
			{
				return;
			}
			this.videoPlayer.targetCameraAlpha = info.weight;
			if (Application.isPlaying)
			{
				for (ushort num = 0; num < this.videoPlayer.clip.audioTrackCount; num += 1)
				{
					if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
					{
						this.videoPlayer.SetDirectAudioVolume(num, info.weight);
					}
					else if (this.videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
					{
						AudioSource targetAudioSource = this.videoPlayer.GetTargetAudioSource(num);
						if (targetAudioSource != null)
						{
							targetAudioSource.volume = info.weight;
						}
					}
				}
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000B274 File Offset: 0x00009474
		public override void OnGraphStart(Playable playable)
		{
			this.playedOnce = false;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000B27D File Offset: 0x0000947D
		public override void OnGraphStop(Playable playable)
		{
			if (!Application.isPlaying)
			{
				this.StopVideo();
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000B28C File Offset: 0x0000948C
		public override void OnPlayableDestroy(Playable playable)
		{
			this.StopVideo();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B294 File Offset: 0x00009494
		public void PlayVideo()
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.Play();
			this.preparing = false;
			if (!Application.isPlaying)
			{
				this.PauseVideo();
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public void PauseVideo()
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.videoPlayer.Pause();
			this.preparing = false;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000B2E7 File Offset: 0x000094E7
		public void StopVideo()
		{
			if (this.videoPlayer == null)
			{
				return;
			}
			this.playedOnce = false;
			this.videoPlayer.Stop();
			this.preparing = false;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B314 File Offset: 0x00009514
		private void SyncVideoToPlayable(Playable playable)
		{
			if (this.videoPlayer == null || this.videoPlayer.clip == null)
			{
				return;
			}
			this.videoPlayer.time = (this.clipInTime + playable.GetTime<Playable>() * (double)this.videoPlayer.playbackSpeed) % this.videoPlayer.clip.length;
		}

		// Token: 0x0400023C RID: 572
		public VideoPlayer videoPlayer;

		// Token: 0x0400023D RID: 573
		public VideoClip videoClip;

		// Token: 0x0400023E RID: 574
		public bool mute;

		// Token: 0x0400023F RID: 575
		public bool loop = true;

		// Token: 0x04000240 RID: 576
		public double preloadTime = 0.3;

		// Token: 0x04000241 RID: 577
		public double clipInTime;

		// Token: 0x04000242 RID: 578
		private bool playedOnce;

		// Token: 0x04000243 RID: 579
		private bool preparing;
	}
}
