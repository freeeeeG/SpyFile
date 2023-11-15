using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000174 RID: 372
public class UI_Notification : MonoBehaviour
{
	// Token: 0x060009D6 RID: 2518 RVA: 0x00025196 File Offset: 0x00023396
	private void OnEnable()
	{
		EventMgr.Register<string>(eGameEvents.TriggerNotification, new Action<string>(this.OnTriggerNotification));
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x000251B3 File Offset: 0x000233B3
	private void OnDisable()
	{
		EventMgr.Remove<string>(eGameEvents.TriggerNotification, new Action<string>(this.OnTriggerNotification));
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000251D0 File Offset: 0x000233D0
	private void OnTriggerNotification(string msg)
	{
		UI_Notification.NotificationData notificationData = new UI_Notification.NotificationData
		{
			msg = msg,
			duration = this.notificationStayTime
		};
		using (Queue<UI_Notification.NotificationData>.Enumerator enumerator = this.queue_pendingNotification.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.msg == notificationData.msg)
				{
					return;
				}
			}
		}
		this.queue_pendingNotification.Enqueue(notificationData);
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00025254 File Offset: 0x00023454
	private void Update()
	{
		if (this.queue_pendingNotification.Count > 0 && this.activatedNotificationCount < this.maxActiveNotificationCount)
		{
			UI_Notification.NotificationData notificationData = this.queue_pendingNotification.Dequeue();
			UI_Obj_NotificationContent component = Object.Instantiate<GameObject>(this.prefab_Notification, base.transform.position, Quaternion.identity, this.layoutGroup.transform).GetComponent<UI_Obj_NotificationContent>();
			component.SetupContent(notificationData.msg, notificationData.duration);
			component.OnNotificationEnd.AddListener(new UnityAction(this.Callback_NotificationEnd));
			this.activatedNotificationCount++;
		}
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x000252EA File Offset: 0x000234EA
	private void Callback_NotificationEnd()
	{
		this.activatedNotificationCount--;
	}

	// Token: 0x040007AA RID: 1962
	[SerializeField]
	private GameObject prefab_Notification;

	// Token: 0x040007AB RID: 1963
	[SerializeField]
	private VerticalLayoutGroup layoutGroup;

	// Token: 0x040007AC RID: 1964
	[SerializeField]
	private int maxActiveNotificationCount = 5;

	// Token: 0x040007AD RID: 1965
	[SerializeField]
	private float notificationStayTime = 3f;

	// Token: 0x040007AE RID: 1966
	[SerializeField]
	private Queue<UI_Notification.NotificationData> queue_pendingNotification = new Queue<UI_Notification.NotificationData>();

	// Token: 0x040007AF RID: 1967
	private int activatedNotificationCount;

	// Token: 0x0200029E RID: 670
	[Serializable]
	public class NotificationData
	{
		// Token: 0x04000C5D RID: 3165
		public string msg;

		// Token: 0x04000C5E RID: 3166
		public float duration;
	}
}
