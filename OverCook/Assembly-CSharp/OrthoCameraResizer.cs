using System;
using UnityEngine;

// Token: 0x0200079C RID: 1948
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class OrthoCameraResizer : MonoBehaviour
{
	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x0600259E RID: 9630 RVA: 0x000B1E44 File Offset: 0x000B0244
	public Vector2 TargetDimensions
	{
		get
		{
			if (this.m_resizableRect != null)
			{
				return this.m_resizableRect.GetSize();
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x000B1E79 File Offset: 0x000B0279
	private void Start()
	{
		this.m_camera = base.gameObject.RequireComponent<Camera>();
	}

	// Token: 0x060025A0 RID: 9632 RVA: 0x000B1E8C File Offset: 0x000B028C
	private void Update()
	{
		if (this.m_resizableRect != null)
		{
			Vector3 size = this.m_resizableRect.GetSize();
			this.m_camera.orthographicSize = size.x * 0.5f / (size.x / size.y);
		}
		else
		{
			this.m_camera.orthographicSize = (float)Screen.width * 0.5f / this.m_camera.aspect;
		}
	}

	// Token: 0x04001D2A RID: 7466
	protected Camera m_camera;

	// Token: 0x04001D2B RID: 7467
	[SerializeField]
	protected RectTransform m_resizableRect;
}
