using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
[RequireComponent(typeof(Camera))]
public class FixedAspectRatioCamera : MonoBehaviour
{
	// Token: 0x060007B9 RID: 1977 RVA: 0x00030479 File Offset: 0x0002E879
	private void Awake()
	{
		this.m_camera = base.gameObject.RequireComponent<Camera>();
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0003048C File Offset: 0x0002E88C
	private void Start()
	{
		this.m_ratioManager = GameUtils.RequestManager<FixedAspectRatioManager>();
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.RegisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
			this.m_ratioManager.InitialiseComponent(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
		else
		{
			Vector2 defaultAspect = FixedAspectRatio.DefaultAspect;
			this.m_camera.rect = FixedAspectRatio.ComputeAspectRatioRect(defaultAspect.x, defaultAspect.y, (float)Screen.width, (float)Screen.height);
		}
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00030518 File Offset: 0x0002E918
	private void OnDestroy()
	{
		if (this.m_ratioManager != null)
		{
			this.m_ratioManager.UnregisterOnResolutionChanged(new GenericVoid<Rect, float, float>(this.OnResolutionChanged));
		}
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00030542 File Offset: 0x0002E942
	private void OnResolutionChanged(Rect _rect, float _screenWidth, float _screenHeight)
	{
		this.m_camera.rect = _rect;
	}

	// Token: 0x04000622 RID: 1570
	private FixedAspectRatioManager m_ratioManager;

	// Token: 0x04000623 RID: 1571
	private Camera m_camera;
}
