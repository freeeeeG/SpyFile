using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class PhotoCameraController : MonoBehaviour
{
	// Token: 0x06000B24 RID: 2852 RVA: 0x0002B2CE File Offset: 0x000294CE
	private void Reset()
	{
		if (this.cam == null)
		{
			this.cam = base.GetComponent<Camera>();
		}
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0002B2EC File Offset: 0x000294EC
	public void TakePhoto()
	{
		RenderTexture renderTexture = new RenderTexture(512, 512, 24);
		this.cam.targetTexture = renderTexture;
		this.cam.Render();
		this.list_Photos.Add(renderTexture);
		this.cam.targetTexture = null;
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0002B33A File Offset: 0x0002953A
	public bool DoHavePhoto(int index)
	{
		return index >= 0 && index < this.list_Photos.Count;
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0002B350 File Offset: 0x00029550
	public int GetPhotoCount()
	{
		return this.list_Photos.Count;
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0002B35D File Offset: 0x0002955D
	public RenderTexture GetPhoto(int index)
	{
		if (!this.DoHavePhoto(index))
		{
			Debug.LogError("Index out of range!");
			return null;
		}
		return this.list_Photos[index];
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0002B380 File Offset: 0x00029580
	public void ClearPhotos()
	{
		foreach (RenderTexture renderTexture in this.list_Photos)
		{
			if (renderTexture)
			{
				renderTexture.Release();
			}
		}
		this.list_Photos.Clear();
	}

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private Camera cam;

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	private List<RenderTexture> list_Photos = new List<RenderTexture>();
}
