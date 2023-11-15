using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x020000B4 RID: 180
	[AddComponentMenu("Modular Options/Tooltip")]
	[RequireComponent(typeof(Selectable))]
	public class UITooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x0600026C RID: 620 RVA: 0x00009948 File Offset: 0x00007B48
		private void Awake()
		{
			this.ttTrans = this.tooltip.GetComponent<RectTransform>();
			this.rTrans = base.GetComponent<RectTransform>();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009967 File Offset: 0x00007B67
		public void OnPointerEnter(PointerEventData _eventData)
		{
			this.EnterOrSelect();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000996F File Offset: 0x00007B6F
		public void OnPointerExit(PointerEventData _eventData)
		{
			this.tooltip.SetActive(false);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000997D File Offset: 0x00007B7D
		public void OnSelect(BaseEventData _eventData)
		{
			this.EnterOrSelect();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009985 File Offset: 0x00007B85
		public void OnDeselect(BaseEventData _eventData)
		{
			this.tooltip.SetActive(false);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009994 File Offset: 0x00007B94
		private void EnterOrSelect()
		{
			Vector3 position = this.rTrans.position;
			switch (this.relativePosition)
			{
			case UITooltip.RelativePosition.left:
				this.ttTrans.pivot = this.middleRightPivot;
				this.ttTrans.position = new Vector3(position.x + this.rTrans.rect.xMin * this.rTrans.lossyScale.x, position.y, position.z) + this.offset;
				break;
			case UITooltip.RelativePosition.right:
				this.ttTrans.pivot = this.middleLeftPivot;
				this.ttTrans.position = new Vector3(position.x + this.rTrans.rect.xMax * this.rTrans.lossyScale.x, position.y, position.z) + this.offset;
				break;
			case UITooltip.RelativePosition.bottom:
				this.ttTrans.pivot = this.topCenterPivot;
				this.ttTrans.position = new Vector3(position.x, position.y + this.rTrans.rect.yMin * this.rTrans.lossyScale.y, position.z) + this.offset;
				break;
			case UITooltip.RelativePosition.top:
				this.ttTrans.pivot = this.bottomCenterPivot;
				this.ttTrans.position = new Vector3(position.x, position.y + this.rTrans.rect.yMax * this.rTrans.lossyScale.y, position.z) + this.offset;
				break;
			default:
				Debug.LogError("Invalid direction.", this);
				break;
			}
			this.tooltip.SetActive(true);
		}

		// Token: 0x040001FF RID: 511
		public GameObject tooltip;

		// Token: 0x04000200 RID: 512
		[Tooltip("Direction relative to this object in which to show Tooltip.")]
		public UITooltip.RelativePosition relativePosition = UITooltip.RelativePosition.right;

		// Token: 0x04000201 RID: 513
		[Tooltip("Optional offset for Tooltip position.")]
		public Vector3 offset = Vector3.zero;

		// Token: 0x04000202 RID: 514
		private RectTransform ttTrans;

		// Token: 0x04000203 RID: 515
		private RectTransform rTrans;

		// Token: 0x04000204 RID: 516
		private readonly Vector2 middleLeftPivot = new Vector2(0f, 0.5f);

		// Token: 0x04000205 RID: 517
		private readonly Vector2 middleRightPivot = new Vector2(1f, 0.5f);

		// Token: 0x04000206 RID: 518
		private readonly Vector2 bottomCenterPivot = new Vector2(0.5f, 0f);

		// Token: 0x04000207 RID: 519
		private readonly Vector2 topCenterPivot = new Vector2(0.5f, 1f);

		// Token: 0x0200010A RID: 266
		public enum RelativePosition
		{
			// Token: 0x040003DE RID: 990
			left,
			// Token: 0x040003DF RID: 991
			right,
			// Token: 0x040003E0 RID: 992
			bottom,
			// Token: 0x040003E1 RID: 993
			top
		}
	}
}
