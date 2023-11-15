using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BE RID: 446
[RequireComponent(typeof(Camera))]
public class CameraCanvasUpdater : MonoBehaviour
{
	// Token: 0x060007AF RID: 1967 RVA: 0x0002FF28 File Offset: 0x0002E328
	private void Awake()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Canvas");
		array = array.AllRemoved_Predicate((GameObject x) => x.RequestComponent<CanvasCameraAssignment>() != null);
		GameObject[] array2 = array;
		if (CameraCanvasUpdater.<>f__mg$cache0 == null)
		{
			CameraCanvasUpdater.<>f__mg$cache0 = new Converter<GameObject, Canvas>(GameObjectUtils.RequireComponent<Canvas>);
		}
		this.m_allSceneCanvases = array2.ConvertAll(CameraCanvasUpdater.<>f__mg$cache0);
		GameObject[] array3 = array;
		if (CameraCanvasUpdater.<>f__mg$cache1 == null)
		{
			CameraCanvasUpdater.<>f__mg$cache1 = new Converter<GameObject, CanvasScaler>(GameObjectUtils.RequireComponent<CanvasScaler>);
		}
		this.m_allSceneCanvasScalers = array3.ConvertAll(CameraCanvasUpdater.<>f__mg$cache1);
		this.m_referenceResolutions = this.m_allSceneCanvasScalers.ConvertAll((CanvasScaler x) => x.referenceResolution);
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCamera));
		for (int i = 0; i < this.m_allSceneCanvases.Length; i++)
		{
			if (this.m_allSceneCanvases[i] != null)
			{
				this.m_allSceneCanvases[i].worldCamera = this.m_camera;
			}
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00030044 File Offset: 0x0002E444
	private void OnDestroy()
	{
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCamera));
		for (int i = 0; i < this.m_allSceneCanvases.Length; i++)
		{
			this.m_allSceneCanvasScalers[i].referenceResolution = this.m_referenceResolutions[i];
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x000300A8 File Offset: 0x0002E4A8
	private void OnPreRenderCamera(Camera _camera)
	{
		if (_camera == this.m_camera)
		{
			for (int i = 0; i < this.m_allSceneCanvases.Length; i++)
			{
				if (this.m_allSceneCanvases[i] != null)
				{
					this.m_allSceneCanvases[i].worldCamera = this.m_camera;
					Vector2 referenceResolution = this.m_referenceResolutions[i].DividedBy(this.m_camera.rect.size);
					this.m_allSceneCanvasScalers[i].referenceResolution = referenceResolution;
				}
			}
		}
	}

	// Token: 0x04000616 RID: 1558
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Camera m_camera;

	// Token: 0x04000617 RID: 1559
	private Canvas[] m_allSceneCanvases;

	// Token: 0x04000618 RID: 1560
	private CanvasScaler[] m_allSceneCanvasScalers;

	// Token: 0x04000619 RID: 1561
	private Vector2[] m_referenceResolutions;

	// Token: 0x0400061A RID: 1562
	[CompilerGenerated]
	private static Converter<GameObject, Canvas> <>f__mg$cache0;

	// Token: 0x0400061B RID: 1563
	[CompilerGenerated]
	private static Converter<GameObject, CanvasScaler> <>f__mg$cache1;
}
