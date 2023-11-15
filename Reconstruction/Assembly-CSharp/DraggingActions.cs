using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public abstract class DraggingActions : MonoBehaviour
{
	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x0001A2CA File Offset: 0x000184CA
	// (set) Token: 0x060009B5 RID: 2485 RVA: 0x0001A2D1 File Offset: 0x000184D1
	public static DraggingActions DraggingThis
	{
		get
		{
			return DraggingActions._draggingThis;
		}
		set
		{
			DraggingActions._draggingThis = value;
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0001A2D9 File Offset: 0x000184D9
	protected virtual void Awake()
	{
		this.cam = Camera.main;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0001A2E6 File Offset: 0x000184E6
	protected virtual void Update()
	{
		if (this.isDragging)
		{
			this.OnDraggingInUpdate();
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0001A2F6 File Offset: 0x000184F6
	public virtual void StartDragging()
	{
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0001A2F8 File Offset: 0x000184F8
	public virtual void EndDragging()
	{
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0001A2FA File Offset: 0x000184FA
	public virtual void OnDraggingInUpdate()
	{
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0001A2FC File Offset: 0x000184FC
	protected Vector3 MouseInWorldCoords()
	{
		Vector3 mousePosition = Input.mousePosition;
		return this.cam.ScreenToWorldPoint(mousePosition);
	}

	// Token: 0x0400050E RID: 1294
	protected bool isDragging;

	// Token: 0x0400050F RID: 1295
	protected Vector3 pointerOffset;

	// Token: 0x04000510 RID: 1296
	private Camera cam;

	// Token: 0x04000511 RID: 1297
	private static DraggingActions _draggingThis;
}
