using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BAB RID: 2987
public class NotificationHighlightController : KMonoBehaviour
{
	// Token: 0x06005D2E RID: 23854 RVA: 0x002219EE File Offset: 0x0021FBEE
	protected override void OnSpawn()
	{
		this.highlightBox = Util.KInstantiateUI<RectTransform>(this.highlightBoxPrefab.gameObject, base.gameObject, false);
		this.HideBox();
	}

	// Token: 0x06005D2F RID: 23855 RVA: 0x00221A14 File Offset: 0x0021FC14
	[ContextMenu("Force Update")]
	protected void LateUpdate()
	{
		bool flag = false;
		if (this.activeTargetNotification != null)
		{
			foreach (NotificationHighlightTarget notificationHighlightTarget in this.targets)
			{
				if (notificationHighlightTarget.targetKey == this.activeTargetNotification.highlightTarget)
				{
					this.SnapBoxToTarget(notificationHighlightTarget);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			this.HideBox();
		}
	}

	// Token: 0x06005D30 RID: 23856 RVA: 0x00221A98 File Offset: 0x0021FC98
	public void AddTarget(NotificationHighlightTarget target)
	{
		this.targets.Add(target);
	}

	// Token: 0x06005D31 RID: 23857 RVA: 0x00221AA6 File Offset: 0x0021FCA6
	public void RemoveTarget(NotificationHighlightTarget target)
	{
		this.targets.Remove(target);
	}

	// Token: 0x06005D32 RID: 23858 RVA: 0x00221AB5 File Offset: 0x0021FCB5
	public void SetActiveTarget(ManagementMenuNotification notification)
	{
		this.activeTargetNotification = notification;
	}

	// Token: 0x06005D33 RID: 23859 RVA: 0x00221ABE File Offset: 0x0021FCBE
	public void ClearActiveTarget(ManagementMenuNotification checkNotification)
	{
		if (checkNotification == this.activeTargetNotification)
		{
			this.activeTargetNotification = null;
		}
	}

	// Token: 0x06005D34 RID: 23860 RVA: 0x00221AD0 File Offset: 0x0021FCD0
	public void ClearActiveTarget()
	{
		this.activeTargetNotification = null;
	}

	// Token: 0x06005D35 RID: 23861 RVA: 0x00221AD9 File Offset: 0x0021FCD9
	public void TargetViewed(NotificationHighlightTarget target)
	{
		if (this.activeTargetNotification != null && this.activeTargetNotification.highlightTarget == target.targetKey)
		{
			this.activeTargetNotification.View();
		}
	}

	// Token: 0x06005D36 RID: 23862 RVA: 0x00221B08 File Offset: 0x0021FD08
	private void SnapBoxToTarget(NotificationHighlightTarget target)
	{
		RectTransform rectTransform = target.rectTransform();
		Vector3 position = rectTransform.GetPosition();
		this.highlightBox.sizeDelta = rectTransform.rect.size;
		this.highlightBox.SetPosition(position + new Vector3(rectTransform.rect.position.x, rectTransform.rect.position.y, 0f));
		RectMask2D componentInParent = rectTransform.GetComponentInParent<RectMask2D>();
		if (componentInParent != null)
		{
			RectTransform rectTransform2 = componentInParent.rectTransform();
			Vector3 a = rectTransform2.TransformPoint(rectTransform2.rect.min);
			Vector3 a2 = rectTransform2.TransformPoint(rectTransform2.rect.max);
			Vector3 b = this.highlightBox.TransformPoint(this.highlightBox.rect.min);
			Vector3 b2 = this.highlightBox.TransformPoint(this.highlightBox.rect.max);
			Vector3 vector = a - b;
			Vector3 vector2 = a2 - b2;
			if (vector.x > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(vector.x, 0f);
				this.highlightBox.sizeDelta -= new Vector2(vector.x, 0f);
			}
			else if (vector.y > 0f)
			{
				this.highlightBox.anchoredPosition = this.highlightBox.anchoredPosition + new Vector2(0f, vector.y);
				this.highlightBox.sizeDelta -= new Vector2(0f, vector.y);
			}
			if (vector2.x < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(vector2.x, 0f);
			}
			if (vector2.y < 0f)
			{
				this.highlightBox.sizeDelta += new Vector2(0f, vector2.y);
			}
		}
		this.highlightBox.gameObject.SetActive(this.highlightBox.sizeDelta.x > 0f && this.highlightBox.sizeDelta.y > 0f);
	}

	// Token: 0x06005D37 RID: 23863 RVA: 0x00221D97 File Offset: 0x0021FF97
	private void HideBox()
	{
		this.highlightBox.gameObject.SetActive(false);
	}

	// Token: 0x04003EBA RID: 16058
	public RectTransform highlightBoxPrefab;

	// Token: 0x04003EBB RID: 16059
	private RectTransform highlightBox;

	// Token: 0x04003EBC RID: 16060
	private List<NotificationHighlightTarget> targets = new List<NotificationHighlightTarget>();

	// Token: 0x04003EBD RID: 16061
	private ManagementMenuNotification activeTargetNotification;
}
