using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B78 RID: 2936
public class T17ScrollView : T17NavigableGrid, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06003BBD RID: 15293 RVA: 0x0011C9F0 File Offset: 0x0011ADF0
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		if (this.m_ContentParent != null)
		{
			if (!base.isAltLayout)
			{
				HorizontalOrVerticalLayoutGroup component = this.m_ContentParent.GetComponent<HorizontalOrVerticalLayoutGroup>();
				if (component != null)
				{
					this.m_Spacing.y = component.spacing;
				}
			}
			else
			{
				T17GridLayoutGroup component2 = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
				if (component2 != null)
				{
					this.m_Spacing = component2.spacing;
				}
			}
			this.m_DesiredPosition = this.m_ContentParent.localPosition;
			this.m_OriginalPosition = this.m_DesiredPosition;
		}
		if (this.m_VerticalScrollBar != null)
		{
			this.m_VerticalScrollBar.onValueChanged.AddListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
		}
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x0011CAC0 File Offset: 0x0011AEC0
	protected override void Update()
	{
		base.Update();
		if (this.m_ContentParent != null && this.m_LerpTime > 0f)
		{
			this.m_LerpTime -= Time.unscaledDeltaTime;
			this.m_ContentParent.localPosition = Vector2.Lerp(this.m_OriginalPosition, this.m_DesiredPosition, (0.3f - this.m_LerpTime) / 0.3f);
		}
		if (this.m_ScrollToItem != null)
		{
			if (this.m_ScrollTimer <= 0)
			{
				this.InternalScrollToItem();
				this.m_ScrollToItem = null;
			}
			this.m_ScrollTimer--;
		}
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x0011CB71 File Offset: 0x0011AF71
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		this.ResetPosition();
		return true;
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x0011CB8C File Offset: 0x0011AF8C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		bool result = base.Show(currentGamer, parent, invoker, hideInvoker);
		if (this.m_VerticalScrollBar != null && this.m_CachedEventSystem != null)
		{
			this.m_VerticalScrollBar.SetEventSystem(this.m_CachedEventSystem);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_ContentParent);
		this.ResetSelection(true);
		this.UpdateBounds();
		this.UpdateScrollbars(Vector2.zero);
		this.UpdatePrevData();
		return result;
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x0011CC02 File Offset: 0x0011B002
	public void ResetPosition()
	{
		this.m_CurrentSelected = 0;
		this.m_PreviousSelected = 0;
		this.SetNormalizedPosition(1f, 1, false);
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x0011CC20 File Offset: 0x0011B020
	public void PageUp()
	{
		float y = this.m_ViewBounds.size.y;
		float num = this.m_DesiredPosition.y;
		int num2 = 0;
		if (num - y > 0f)
		{
			num -= y;
			float num3 = 0f;
			while (num3 < num)
			{
				num3 += this.m_ContentSelectables[num2].rect.height + this.m_Spacing.y;
				num2++;
			}
			if (num2 < this.m_ContentSelectables.Count)
			{
				num3 -= this.m_ContentSelectables[num2].rect.height + this.m_Spacing.y;
			}
			num2--;
			this.m_DesiredPosition.y = num3;
		}
		else
		{
			this.m_DesiredPosition.y = 0f;
		}
		T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(this.m_CurrentGamepadUser);
		eventSystemForGamepadUser.SetSelectedGameObject(this.m_ContentSelectables[num2].gameObject);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x0011CD30 File Offset: 0x0011B130
	public void PageDown()
	{
		float y = this.m_ViewBounds.size.y;
		float num = this.m_DesiredPosition.y;
		float y2 = this.m_ContentBounds.size.y;
		int num2 = 0;
		if (num + y < y2)
		{
			num += y;
			float num3 = 0f;
			while (num3 < num && num2 < this.m_ContentSelectables.Count)
			{
				num3 += this.m_ContentSelectables[num2].rect.height + this.m_Spacing.y;
				num2++;
			}
			if (num2 < this.m_ContentSelectables.Count)
			{
				num3 -= this.m_ContentSelectables[num2].rect.height + this.m_Spacing.y;
			}
			num2--;
			this.m_DesiredPosition.y = num3;
		}
		else
		{
			this.m_DesiredPosition.y = y2;
			num2 = this.m_ContentSelectables.Count - 1;
		}
		T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(this.m_CurrentGamepadUser);
		eventSystemForGamepadUser.SetSelectedGameObject(this.m_ContentSelectables[num2].gameObject);
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x0011CE78 File Offset: 0x0011B278
	protected override void OnElementSelected(Selectable sel, int index)
	{
		base.OnElementSelected(sel, index);
		bool flag = false;
		if (this.m_ContentSelectables != null && this.m_ContentSelectables.Count > 1)
		{
			if (this.m_bScrollToSelectedElement && !flag)
			{
				this.m_OriginalPosition = this.m_ContentParent.localPosition;
				this.m_LerpTime = 0.3f;
				if (this.m_CurrentSelected > this.m_PreviousSelected && this.m_CurrentSelected < this.m_ContentSelectables.Count)
				{
					this.m_ContentSelectables[this.m_ContentSelectables.Count - 1].GetWorldCorners(this.m_ElementCorners);
					this.m_ViewPort.GetWorldCorners(this.m_ViewCorners);
					if (this.m_ElementCorners[0].y < this.m_ViewCorners[0].y || this.m_ElementCorners[1].y < this.m_ViewCorners[0].y || this.m_ElementCorners[2].y < this.m_ViewCorners[3].y || this.m_ElementCorners[3].y < this.m_ViewCorners[3].y)
					{
						if (!base.isAltLayout)
						{
							this.m_DesiredPosition += new Vector2(0f, this.m_ContentSelectables[this.m_PreviousSelected].rect.height + this.m_Spacing.y);
						}
						else
						{
							float y = this.m_ContentSelectables[this.m_PreviousSelected].localPosition.y;
							float y2 = this.m_ContentSelectables[this.m_CurrentSelected].localPosition.y;
							this.m_DesiredPosition += new Vector2(0f, y - y2);
						}
					}
				}
				else if (this.m_PreviousSelected > 0 && this.m_CurrentSelected < this.m_ContentSelectables.Count - 1)
				{
					this.m_ViewPort.GetWorldCorners(this.m_ViewCorners);
					this.m_ContentSelectables[0].GetWorldCorners(this.m_ElementCorners);
					if (this.m_ElementCorners[0].y > this.m_ViewCorners[1].y || this.m_ElementCorners[1].y > this.m_ViewCorners[1].y || this.m_ElementCorners[2].y > this.m_ViewCorners[2].y || this.m_ElementCorners[3].y > this.m_ViewCorners[2].y)
					{
						if (!base.isAltLayout)
						{
							this.m_DesiredPosition -= new Vector2(0f, this.m_ContentSelectables[this.m_PreviousSelected].rect.height + this.m_Spacing.y);
						}
						else
						{
							float y3 = this.m_ContentSelectables[this.m_PreviousSelected].localPosition.y;
							float y4 = this.m_ContentSelectables[this.m_CurrentSelected].localPosition.y;
							this.m_DesiredPosition -= new Vector2(0f, y4 - y3);
						}
					}
				}
			}
			else if (flag)
			{
				this.m_OriginalPosition = this.m_ContentParent.localPosition;
				this.m_LerpTime = 0.3f;
				if (this.m_CurrentSelected != this.m_PreviousSelected && this.m_CurrentSelected < this.m_ContentSelectables.Count)
				{
					this.m_ContentSelectables[this.m_CurrentSelected].GetWorldCorners(this.m_ElementCorners);
					this.m_ViewPort.GetWorldCorners(this.m_ViewCorners);
					if (this.m_ElementCorners[0].y < this.m_ViewCorners[1].y && this.m_ElementCorners[1].y > this.m_ViewCorners[1].y)
					{
						float num = this.m_ElementCorners[1].y - this.m_ViewCorners[1].y;
						float num2 = this.m_ElementCorners[1].y - this.m_ElementCorners[0].y;
						float num3 = Mathf.Abs(num / num2);
						this.m_DesiredPosition -= new Vector2(0f, this.m_ContentSelectables[this.m_CurrentSelected].rect.size.y * num3 + this.m_Spacing.y);
					}
					else if (this.m_ElementCorners[1].y > this.m_ViewCorners[1].y || this.m_ElementCorners[0].y < this.m_ViewCorners[0].y)
					{
						if (this.m_ElementCorners[1].y > this.m_ViewCorners[0].y && this.m_ElementCorners[0].y < this.m_ViewCorners[0].y)
						{
							float num4 = this.m_ViewCorners[0].y - this.m_ElementCorners[0].y;
							float num5 = this.m_ElementCorners[1].y - this.m_ElementCorners[0].y;
							float num6 = Mathf.Abs(num4 / num5);
							this.m_DesiredPosition += new Vector2(0f, this.m_ContentSelectables[this.m_CurrentSelected].rect.size.y * num6 + this.m_Spacing.y);
						}
					}
				}
			}
			this.UpdateBounds();
			float num7 = this.m_ContentBounds.size.y - this.m_ViewBounds.size.y;
			float max = this.m_ContentParent.localPosition.y + this.m_ViewBounds.min.y - this.m_ContentBounds.min.y;
			float min = this.m_ContentParent.localPosition.y + this.m_ViewBounds.min.y - num7 - this.m_ContentBounds.min.y;
			this.m_DesiredPosition.y = Mathf.Clamp(this.m_DesiredPosition.y, min, max);
		}
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x0011D5C4 File Offset: 0x0011B9C4
	protected override bool ReselectCurrent(ref T17EventSystem system)
	{
		bool flag = base.ReselectCurrent(ref system);
		if (flag)
		{
			if (system.currentSelectedGameObject == this.m_ContentSelectables[0].gameObject)
			{
				this.ResetPosition();
			}
			this.m_LerpTime = 0f;
		}
		return flag;
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x0011D613 File Offset: 0x0011BA13
	private void SetVerticalNormalizedPosition(float value)
	{
		this.SetNormalizedPosition(value, 1, true);
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x0011D620 File Offset: 0x0011BA20
	private void SetNormalizedPosition(float value, int axis, bool fromScrollbar = false)
	{
		this.UpdateBounds();
		float num = this.m_ContentBounds.size[axis] - this.m_ViewBounds.size[axis];
		float num2 = this.m_ViewBounds.min[axis] - value * num;
		float num3 = this.m_ContentParent.localPosition[axis] + num2 - this.m_ContentBounds.min[axis];
		Vector3 localPosition = this.m_ContentParent.localPosition;
		if (Mathf.Abs(localPosition[axis] - num3) > 0.01f)
		{
			localPosition[axis] = num3;
			this.m_ContentParent.localPosition = localPosition;
			this.m_DesiredPosition = localPosition;
			this.UpdateBounds();
		}
	}

	// Token: 0x06003BC8 RID: 15304 RVA: 0x0011D6F8 File Offset: 0x0011BAF8
	private void UpdateBounds()
	{
		this.m_ViewBounds = new Bounds(this.m_ViewPort.rect.center, this.m_ViewPort.rect.size);
		this.m_ContentBounds = this.GetContentBounds();
		if (this.m_ContentParent == null)
		{
			return;
		}
		Vector3 size = this.m_ContentBounds.size;
		Vector3 center = this.m_ContentBounds.center;
		Vector3 vector = this.m_ViewBounds.size - size;
		if (vector.x > 0f)
		{
			center.x -= vector.x * (this.m_ContentParent.pivot.x - 0.5f);
			size.x = this.m_ViewBounds.size.x;
		}
		if (vector.y > 0f)
		{
			center.y -= vector.y * (this.m_ContentParent.pivot.y - 0.5f);
			size.y = this.m_ViewBounds.size.y;
		}
		this.m_ContentBounds.size = size;
		this.m_ContentBounds.center = center;
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x0011D85C File Offset: 0x0011BC5C
	private Bounds GetContentBounds()
	{
		if (this.m_ContentParent == null)
		{
			return default(Bounds);
		}
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Matrix4x4 worldToLocalMatrix = this.m_ViewPort.worldToLocalMatrix;
		this.m_ContentParent.GetWorldCorners(this.m_ContentCorners);
		for (int i = 0; i < 4; i++)
		{
			Vector3 lhs = worldToLocalMatrix.MultiplyPoint3x4(this.m_ContentCorners[i]);
			vector = Vector3.Min(lhs, vector);
			vector2 = Vector3.Max(lhs, vector2);
		}
		Bounds result = new Bounds(vector, Vector3.zero);
		result.Encapsulate(vector2);
		return result;
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x0011D928 File Offset: 0x0011BD28
	private Vector2 CalculateOffset(Vector2 delta)
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = this.m_ContentBounds.min;
		Vector2 vector2 = this.m_ContentBounds.max;
		if (this.m_VerticalScrollBar != null)
		{
			vector.y += delta.y;
			vector2.y += delta.y;
			if (vector2.y < this.m_ViewBounds.max.y)
			{
				zero.y = this.m_ViewBounds.max.y - vector2.y;
			}
			else if (vector.y > this.m_ViewBounds.min.y)
			{
				zero.y = this.m_ViewBounds.min.y - vector.y;
			}
		}
		return zero;
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x0011DA24 File Offset: 0x0011BE24
	protected virtual void LateUpdate()
	{
		if (!this.m_ContentParent)
		{
			return;
		}
		this.UpdateBounds();
		Vector2 offset = this.CalculateOffset(Vector2.zero);
		if (this.m_ViewBounds != this.m_PrevViewBounds || this.m_ContentBounds != this.m_PrevContentBounds || this.m_ContentParent.anchoredPosition != this.m_PrevPosition)
		{
			this.UpdateScrollbars(offset);
			this.UpdatePrevData();
		}
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x0011DAA8 File Offset: 0x0011BEA8
	private void UpdatePrevData()
	{
		if (this.m_ContentParent == null)
		{
			this.m_PrevPosition = Vector2.zero;
		}
		else
		{
			this.m_PrevPosition = this.m_ContentParent.anchoredPosition;
		}
		this.m_PrevViewBounds = this.m_ViewBounds;
		this.m_PrevContentBounds = this.m_ContentBounds;
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x0011DB00 File Offset: 0x0011BF00
	private void UpdateScrollbars(Vector2 offset)
	{
		if (this.m_VerticalScrollBar)
		{
			if (this.m_ContentBounds.size.y > 0f)
			{
				this.m_VerticalScrollBar.size = Mathf.Clamp01((this.m_ViewBounds.size.y - Mathf.Abs(offset.y)) / this.m_ContentBounds.size.y);
			}
			else
			{
				this.m_VerticalScrollBar.size = 1f;
			}
			this.m_VerticalScrollBar.value = this.verticalNormalizedPosition;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06003BCE RID: 15310 RVA: 0x0011DBA4 File Offset: 0x0011BFA4
	// (set) Token: 0x06003BCF RID: 15311 RVA: 0x0011DC69 File Offset: 0x0011C069
	public float verticalNormalizedPosition
	{
		get
		{
			this.UpdateBounds();
			if (this.m_ContentBounds.size.y <= this.m_ViewBounds.size.y)
			{
				return (float)((this.m_ViewBounds.min.y <= this.m_ContentBounds.min.y) ? 0 : 1);
			}
			return (this.m_ViewBounds.min.y - this.m_ContentBounds.min.y) / (this.m_ContentBounds.size.y - this.m_ViewBounds.size.y);
		}
		set
		{
			this.SetNormalizedPosition(value, 1, false);
		}
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x0011DC74 File Offset: 0x0011C074
	public void ScrollToEntry(GameObject scrollTo, bool bSelect)
	{
		this.m_bSelectScrollItem = bSelect;
		this.m_ScrollToItem = scrollTo;
		this.m_ScrollTimer = 5;
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x0011DC8C File Offset: 0x0011C08C
	private void InternalScrollToItem()
	{
		int count = this.m_ContentSelectables.Count;
		float num = 0f;
		T17GridLayoutGroup component = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
		for (int i = 0; i < count; i++)
		{
			RectTransform rectTransform = this.m_ContentSelectables[i];
			if (rectTransform != null && rectTransform.gameObject != this.m_ScrollToItem)
			{
				bool flag = true;
				if (base.isAltLayout && component != null && (i + 1) % component.m_CellCountX != 0)
				{
					flag = false;
				}
				if (flag)
				{
					float height = rectTransform.rect.height;
					num += height + this.m_Spacing.y;
				}
			}
			if (rectTransform.gameObject == this.m_ScrollToItem)
			{
				if (this.m_bSelectScrollItem)
				{
					this.m_PreviousSelected = i;
					this.m_CurrentSelected = i;
				}
				this.m_DesiredPosition.y = num;
				this.m_LerpTime = 0.3f;
				break;
			}
		}
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x0011DDA0 File Offset: 0x0011C1A0
	public void ResetSelection(bool isTop)
	{
		if (this.m_ContentSelectables.Count == 0)
		{
			this.m_PreviousSelected = 0;
			this.m_CurrentSelected = 0;
			return;
		}
		int num = 0;
		if (!isTop)
		{
			num = this.m_ContentSelectables.Count - 1;
		}
		this.m_PreviousSelected = num;
		this.m_CurrentSelected = num;
		T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(this.m_CurrentGamepadUser);
		eventSystemForGamepadUser.SetSelectedGameObject(this.m_ContentSelectables[num].gameObject);
		this.ScrollToEntry(this.m_ContentSelectables[num].gameObject, true);
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x0011DE30 File Offset: 0x0011C230
	public int GetCurrentSelected()
	{
		return this.m_CurrentSelected;
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x0011DE38 File Offset: 0x0011C238
	public void InstantlyScrollToItem(GameObject item, bool bSelect)
	{
		int count = this.m_ContentSelectables.Count;
		float num = 0f;
		T17GridLayoutGroup component = this.m_ContentParent.GetComponent<T17GridLayoutGroup>();
		for (int i = 0; i < count; i++)
		{
			RectTransform rectTransform = this.m_ContentSelectables[i];
			if (rectTransform != null && rectTransform.gameObject != item)
			{
				bool flag = true;
				if (base.isAltLayout && component != null && (i + 1) % component.m_CellCountX != 0)
				{
					flag = false;
				}
				if (flag)
				{
					float height = rectTransform.rect.height;
					num += height + this.m_Spacing.y;
				}
			}
			if (rectTransform.gameObject == item)
			{
				if (this.m_bSelectScrollItem)
				{
					this.m_PreviousSelected = i;
					this.m_CurrentSelected = i;
				}
				float num2 = this.m_ContentBounds.size.y - this.m_ViewBounds.size.y;
				float max = this.m_ContentParent.localPosition.y + this.m_ViewBounds.min.y - this.m_ContentBounds.min.y;
				float min = this.m_ContentParent.localPosition.y + this.m_ViewBounds.min.y - num2 - this.m_ContentBounds.min.y;
				num = Mathf.Clamp(num, min, max);
				this.m_ContentParent.localPosition = new Vector2(this.m_ContentParent.localPosition.x, num);
				break;
			}
		}
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x0011E00B File Offset: 0x0011C40B
	public void OnScroll(PointerEventData eventData)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.m_VerticalScrollBar != null)
		{
			this.m_VerticalScrollBar.OnScroll(eventData);
		}
	}

	// Token: 0x04003086 RID: 12422
	public RectTransform m_ViewPort;

	// Token: 0x04003087 RID: 12423
	public T17Scrollbar m_VerticalScrollBar;

	// Token: 0x04003088 RID: 12424
	public bool m_bScrollToSelectedElement = true;

	// Token: 0x04003089 RID: 12425
	private Vector2 m_Spacing = Vector2.zero;

	// Token: 0x0400308A RID: 12426
	private Vector3[] m_ViewCorners = new Vector3[4];

	// Token: 0x0400308B RID: 12427
	private Vector3[] m_ElementCorners = new Vector3[4];

	// Token: 0x0400308C RID: 12428
	private Vector3[] m_ContentCorners = new Vector3[4];

	// Token: 0x0400308D RID: 12429
	private Bounds m_ContentBounds;

	// Token: 0x0400308E RID: 12430
	private Bounds m_ViewBounds;

	// Token: 0x0400308F RID: 12431
	private Vector2 m_OriginalPosition = Vector2.zero;

	// Token: 0x04003090 RID: 12432
	private Vector2 m_DesiredPosition = Vector2.zero;

	// Token: 0x04003091 RID: 12433
	private Vector2 m_PrevPosition = Vector2.zero;

	// Token: 0x04003092 RID: 12434
	private Bounds m_PrevContentBounds;

	// Token: 0x04003093 RID: 12435
	private Bounds m_PrevViewBounds;

	// Token: 0x04003094 RID: 12436
	private float m_LerpTime;

	// Token: 0x04003095 RID: 12437
	private const float LERP_STEP = 0.3f;

	// Token: 0x04003096 RID: 12438
	private GameObject m_ScrollToItem;

	// Token: 0x04003097 RID: 12439
	private bool m_bSelectScrollItem;

	// Token: 0x04003098 RID: 12440
	private int m_ScrollTimer;

	// Token: 0x04003099 RID: 12441
	private const int SCROLL_WAIT = 5;
}
