using System;
using UnityEngine;

// Token: 0x02000158 RID: 344
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class RectTransformExtension : MonoBehaviour
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x0002BD76 File Offset: 0x0002A176
	// (set) Token: 0x06000606 RID: 1542 RVA: 0x0002BD67 File Offset: 0x0002A167
	public Vector2 AnchorOffset
	{
		get
		{
			return this.m_anchorOffset;
		}
		set
		{
			this.m_anchorOffset = value;
			this.UpdateAspectRatio();
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x0002BD8D File Offset: 0x0002A18D
	// (set) Token: 0x06000608 RID: 1544 RVA: 0x0002BD7E File Offset: 0x0002A17E
	public Vector2 PixelOffset
	{
		get
		{
			return this.m_pixelOffset;
		}
		set
		{
			this.m_pixelOffset = value;
			this.UpdateAspectRatio();
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x0600060A RID: 1546 RVA: 0x0002BD95 File Offset: 0x0002A195
	public Vector2 anchorMin
	{
		get
		{
			return this.m_rectTransform.anchorMin;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x0600060B RID: 1547 RVA: 0x0002BDA2 File Offset: 0x0002A1A2
	public Vector2 anchorMax
	{
		get
		{
			return this.m_rectTransform.anchorMax;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x0002BDAF File Offset: 0x0002A1AF
	public Vector2 offsetMin
	{
		get
		{
			return this.m_rectTransform.offsetMin;
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600060D RID: 1549 RVA: 0x0002BDBC File Offset: 0x0002A1BC
	public Vector2 offsetMax
	{
		get
		{
			return this.m_rectTransform.offsetMax;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x0002BDC9 File Offset: 0x0002A1C9
	// (set) Token: 0x0600060F RID: 1551 RVA: 0x0002BDD1 File Offset: 0x0002A1D1
	public Vector3 rotation
	{
		get
		{
			return this.m_rotation;
		}
		set
		{
			this.m_rotation = value;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000610 RID: 1552 RVA: 0x0002BDDA File Offset: 0x0002A1DA
	// (set) Token: 0x06000611 RID: 1553 RVA: 0x0002BDE2 File Offset: 0x0002A1E2
	public Vector2 scale
	{
		get
		{
			return this.m_scale;
		}
		set
		{
			this.m_scale = value;
		}
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002BDEB File Offset: 0x0002A1EB
	private void Awake()
	{
		this.UpdateAspectRatio();
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x0002BDF4 File Offset: 0x0002A1F4
	private void Setup()
	{
		if (this.m_rectTransform == null)
		{
			this.m_rectTransform = base.gameObject.RequireComponent<RectTransform>();
			this.m_rectTransform.hideFlags = HideFlags.None;
			this.m_rectDriver.Add(this, this.m_rectTransform, DrivenTransformProperties.All);
		}
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x0002BE42 File Offset: 0x0002A242
	private void LateUpdate()
	{
		this.UpdateAspectRatio();
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0002BE4C File Offset: 0x0002A24C
	private void UpdateAspectRatio()
	{
		this.Setup();
		if (Camera.main == null)
		{
			return;
		}
		float aspect = Camera.main.aspect;
		this.m_rectTransform.offsetMin = this.m_pixelOffset;
		this.m_rectTransform.offsetMax = this.m_pixelOffset;
		this.m_rectTransform.localScale = Vector2.one;
		this.m_rectTransform.anchorMin = this.m_anchorOffset + this.ConvertFixedRatioCoordToReal(this.m_fixedRatioAnchorMin, aspect, this.m_scalingBehaviour);
		this.m_rectTransform.anchorMax = this.m_anchorOffset + this.ConvertFixedRatioCoordToReal(this.m_fixedRatioAnchorMax, aspect, this.m_scalingBehaviour);
		this.m_rectTransform.localRotation = Quaternion.Euler(this.m_rotation);
		this.m_rectTransform.pivot = this.m_rotationPivot;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0002BF2C File Offset: 0x0002A32C
	private Vector2 GetScale(float _aspect, RectTransformExtension.ScalingBehaviour _scalingBehaviour)
	{
		if (_scalingBehaviour == RectTransformExtension.ScalingBehaviour.PreserveHorizontalAnchors)
		{
			return new Vector2(this.m_scale.x, this.m_scale.y * _aspect);
		}
		if (_scalingBehaviour != RectTransformExtension.ScalingBehaviour.PreserveVerticalAnchors)
		{
			return new Vector3(this.m_scale.x, this.m_scale.y);
		}
		return new Vector2(this.m_scale.x / _aspect, this.m_scale.y);
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0002BFA8 File Offset: 0x0002A3A8
	private Vector2 GetFixedRatioCoord(Vector2 _pos, float _aspect, RectTransformExtension.ScalingBehaviour _scalingBehaviour)
	{
		Vector2 scale = this.GetScale(_aspect, _scalingBehaviour);
		return new Vector2((_pos.x - this.m_scalePivot.x) / scale.x + this.m_scalePivot.x, (_pos.y - this.m_scalePivot.y) / scale.y + this.m_scalePivot.y);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0002C014 File Offset: 0x0002A414
	private Vector2 ConvertFixedRatioCoordToReal(Vector2 _fixedRatio, float _aspect, RectTransformExtension.ScalingBehaviour _scalingBehaviour)
	{
		Vector2 scale = this.GetScale(_aspect, _scalingBehaviour);
		return new Vector2((_fixedRatio.x - this.m_scalePivot.x) * scale.x + this.m_scalePivot.x, (_fixedRatio.y - this.m_scalePivot.y) * scale.y + this.m_scalePivot.y);
	}

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	private RectTransformExtension.ScalingBehaviour m_scalingBehaviour = RectTransformExtension.ScalingBehaviour.PreserveVerticalAnchors;

	// Token: 0x0400050F RID: 1295
	[SerializeField]
	private Vector2 m_scalePivot = Vector2.zero;

	// Token: 0x04000510 RID: 1296
	[SerializeField]
	private Vector2 m_fixedRatioAnchorMin = Vector2.zero;

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private Vector2 m_fixedRatioAnchorMax = Vector2.one;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	private Vector3 m_rotation = Vector3.zero;

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private Vector2 m_rotationPivot = Vector2.zero;

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private Vector2 m_scale = new Vector2(1f, 1f);

	// Token: 0x04000515 RID: 1301
	private DrivenRectTransformTracker m_rectDriver = default(DrivenRectTransformTracker);

	// Token: 0x04000516 RID: 1302
	private RectTransform m_rectTransform;

	// Token: 0x04000517 RID: 1303
	private Vector2 m_anchorOffset = Vector2.zero;

	// Token: 0x04000518 RID: 1304
	private Vector2 m_pixelOffset = Vector2.zero;

	// Token: 0x02000159 RID: 345
	public enum ScalingBehaviour
	{
		// Token: 0x0400051A RID: 1306
		DontPreserve,
		// Token: 0x0400051B RID: 1307
		PreserveHorizontalAnchors,
		// Token: 0x0400051C RID: 1308
		PreserveVerticalAnchors
	}
}
