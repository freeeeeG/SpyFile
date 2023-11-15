using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000367 RID: 871
[ExecutionDependency(typeof(RectTransformExtension))]
[RequireComponent(typeof(UI_Move))]
public class HoverIconUIController : UIControllerBase
{
	// Token: 0x060010A6 RID: 4262 RVA: 0x0005FD76 File Offset: 0x0005E176
	public Vector2 GetOffset()
	{
		return this.m_anchorOffset;
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0005FD7E File Offset: 0x0005E17E
	public Transform GetFollowTransform()
	{
		return this.m_followTransform;
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0005FD86 File Offset: 0x0005E186
	public void SetAnchorOffset(Vector2 _anchorOffset)
	{
		this.m_anchorOffset = _anchorOffset;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0005FD8F File Offset: 0x0005E18F
	public void SetFollowOffset(Vector3 _followOffset)
	{
		this.m_followOffset = _followOffset;
		this.UpdateFollow();
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0005FD9E File Offset: 0x0005E19E
	public void SetFollowTransform(Transform _followTransform)
	{
		this.m_followTransform = _followTransform;
		this.UpdateFollow();
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0005FDAD File Offset: 0x0005E1AD
	public void SetFollowTransform(Transform _followTransform, Vector3 _followOffset)
	{
		this.m_followTransform = _followTransform;
		this.m_followOffset = _followOffset;
		this.UpdateFollow();
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0005FDC4 File Offset: 0x0005E1C4
	public void SetVisibility(bool _value)
	{
		if (base.gameObject != null)
		{
			ILayoutElement[] array = base.gameObject.RequestInterfacesRecursive<ILayoutElement>();
			for (int i = 0; i < array.Length; i++)
			{
				(array[i] as MonoBehaviour).enabled = _value;
			}
		}
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0005FE10 File Offset: 0x0005E210
	protected virtual void Awake()
	{
		this.m_rectTransform = (base.transform as RectTransform);
		this.m_uiMove = base.gameObject.RequireComponent<UI_Move>();
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0005FE34 File Offset: 0x0005E234
	protected virtual void OnEnable()
	{
		this.UpdateFollow();
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0005FE3C File Offset: 0x0005E23C
	public virtual void LateUpdate()
	{
		this.UpdateFollow();
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0005FE44 File Offset: 0x0005E244
	private void UpdateFollow()
	{
		if (!this.ShouldFollow)
		{
			return;
		}
		if (this.m_canvasRect == null)
		{
			Canvas canvas = base.gameObject.RequestComponentUpwardsRecursive<Canvas>();
			if (canvas != null)
			{
				this.m_canvasRect = (canvas.transform as RectTransform);
			}
		}
		if (this.m_canvasRect != null && this.m_uiMove != null)
		{
			Vector2 screenPos = this.GetScreenPos();
			Vector2 size = this.m_canvasRect.rect.size;
			Vector2 offset = new Vector2((screenPos.x + this.m_anchorOffset.x) * size.x, (screenPos.y + this.m_anchorOffset.y) * size.y);
			this.m_uiMove.Offset = offset;
		}
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0005FF20 File Offset: 0x0005E320
	private Vector2 GetScreenPos()
	{
		if (Camera.main == null)
		{
			return Vector2.zero;
		}
		if (this.m_followTransform == null)
		{
			return this.m_followOffset;
		}
		return Camera.main.WorldToViewportPoint(this.m_followTransform.TransformPoint(this.m_followOffset));
	}

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	protected Vector2 m_anchorOffset;

	// Token: 0x04000CD6 RID: 3286
	public bool ShouldFollow = true;

	// Token: 0x04000CD7 RID: 3287
	private Transform m_followTransform;

	// Token: 0x04000CD8 RID: 3288
	private Vector3 m_followOffset = Vector3.zero;

	// Token: 0x04000CD9 RID: 3289
	protected RectTransform m_rectTransform;

	// Token: 0x04000CDA RID: 3290
	private UI_Move m_uiMove;

	// Token: 0x04000CDB RID: 3291
	private RectTransform m_canvasRect;
}
