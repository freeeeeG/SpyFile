using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000C8F RID: 3215
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
	// Token: 0x06006672 RID: 26226 RVA: 0x002634CA File Offset: 0x002616CA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.Clicked;
		this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
	}

	// Token: 0x06006673 RID: 26227 RVA: 0x002634FC File Offset: 0x002616FC
	private void Clicked()
	{
		VideoScreen.Instance.PlayVideo(this.clip, false, default(EventReference), false);
		if (!string.IsNullOrEmpty(this.overlayName))
		{
			VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
		}
	}

	// Token: 0x06006674 RID: 26228 RVA: 0x00263548 File Offset: 0x00261748
	public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
	{
		if (clip == null)
		{
			global::Debug.LogWarning("Tried to assign null video clip to VideoWidget");
			return;
		}
		this.clip = clip;
		this.overlayName = overlayName;
		this.texts = texts;
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.thumbnailPlayer.targetTexture = this.renderTexture;
		this.rawImage.texture = this.renderTexture;
		base.StartCoroutine(this.ConfigureThumbnail());
	}

	// Token: 0x06006675 RID: 26229 RVA: 0x002635D0 File Offset: 0x002617D0
	private IEnumerator ConfigureThumbnail()
	{
		this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.thumbnailPlayer.clip = this.clip;
		this.thumbnailPlayer.time = 0.0;
		this.thumbnailPlayer.Play();
		yield return null;
		yield break;
	}

	// Token: 0x06006676 RID: 26230 RVA: 0x002635DF File Offset: 0x002617DF
	private void Update()
	{
		if (this.thumbnailPlayer.isPlaying && this.thumbnailPlayer.time > 2.0)
		{
			this.thumbnailPlayer.Pause();
		}
	}

	// Token: 0x04004695 RID: 18069
	[SerializeField]
	private VideoClip clip;

	// Token: 0x04004696 RID: 18070
	[SerializeField]
	private VideoPlayer thumbnailPlayer;

	// Token: 0x04004697 RID: 18071
	[SerializeField]
	private KButton button;

	// Token: 0x04004698 RID: 18072
	[SerializeField]
	private string overlayName;

	// Token: 0x04004699 RID: 18073
	[SerializeField]
	private List<string> texts;

	// Token: 0x0400469A RID: 18074
	private RenderTexture renderTexture;

	// Token: 0x0400469B RID: 18075
	private RawImage rawImage;
}
