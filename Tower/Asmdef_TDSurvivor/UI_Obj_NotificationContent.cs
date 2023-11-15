using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017B RID: 379
public class UI_Obj_NotificationContent : MonoBehaviour
{
	// Token: 0x06000A02 RID: 2562 RVA: 0x00025824 File Offset: 0x00023A24
	public void SetupContent(string msg, float duration)
	{
		if (this.animator == null || this.text_Content == null)
		{
			Debug.LogError("Animator or Text Content is not assigned");
			return;
		}
		this.text_Content.text = msg;
		if (this.currentCoroutine != null)
		{
			base.StopCoroutine(this.currentCoroutine);
		}
		this.currentCoroutine = base.StartCoroutine(this.CR_NotificationProc(duration));
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0002588C File Offset: 0x00023A8C
	public void CancelNotification()
	{
		if (this.currentCoroutine != null)
		{
			base.StopCoroutine(this.currentCoroutine);
			this.animator.SetBool("isOn", false);
			base.gameObject.SetActive(false);
			UnityEvent onNotificationEnd = this.OnNotificationEnd;
			if (onNotificationEnd != null)
			{
				onNotificationEnd.Invoke();
			}
			this.OnNotificationEnd.RemoveAllListeners();
		}
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000258E6 File Offset: 0x00023AE6
	private IEnumerator CR_NotificationProc(float duration)
	{
		this.animator.SetBool("isOn", true);
		yield return new WaitForSeconds(duration);
		this.animator.SetBool("isOn", false);
		yield return new WaitForSeconds(0.5f);
		base.gameObject.SetActive(false);
		UnityEvent onNotificationEnd = this.OnNotificationEnd;
		if (onNotificationEnd != null)
		{
			onNotificationEnd.Invoke();
		}
		yield break;
	}

	// Token: 0x040007C0 RID: 1984
	[SerializeField]
	private Animator animator;

	// Token: 0x040007C1 RID: 1985
	[SerializeField]
	private TMP_Text text_Content;

	// Token: 0x040007C2 RID: 1986
	public UnityEvent OnNotificationEnd;

	// Token: 0x040007C3 RID: 1987
	private Coroutine currentCoroutine;
}
