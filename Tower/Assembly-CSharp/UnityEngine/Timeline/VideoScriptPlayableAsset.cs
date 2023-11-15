using System;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace UnityEngine.Timeline
{
	// Token: 0x020000C2 RID: 194
	[Serializable]
	public class VideoScriptPlayableAsset : PlayableAsset
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000B4A8 File Offset: 0x000096A8
		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			ScriptPlayable<VideoPlayableBehaviour> playable = ScriptPlayable<VideoPlayableBehaviour>.Create(graph, 0);
			VideoPlayableBehaviour behaviour = playable.GetBehaviour();
			behaviour.videoPlayer = this.videoPlayer.Resolve(graph.GetResolver());
			behaviour.videoClip = this.videoClip;
			behaviour.mute = this.mute;
			behaviour.loop = this.loop;
			behaviour.preloadTime = this.preloadTime;
			behaviour.clipInTime = this.clipInTime;
			return playable;
		}

		// Token: 0x04000246 RID: 582
		public ExposedReference<VideoPlayer> videoPlayer;

		// Token: 0x04000247 RID: 583
		[SerializeField]
		[NotKeyable]
		public VideoClip videoClip;

		// Token: 0x04000248 RID: 584
		[SerializeField]
		[NotKeyable]
		public bool mute;

		// Token: 0x04000249 RID: 585
		[SerializeField]
		[NotKeyable]
		public bool loop = true;

		// Token: 0x0400024A RID: 586
		[SerializeField]
		[NotKeyable]
		public double preloadTime = 0.3;

		// Token: 0x0400024B RID: 587
		[SerializeField]
		[NotKeyable]
		public double clipInTime;
	}
}
