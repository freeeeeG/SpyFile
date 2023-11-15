using System;
using UnityEngine;

// Token: 0x02000B81 RID: 2945
public class MessageDialogFrame : KScreen
{
	// Token: 0x06005B75 RID: 23413 RVA: 0x00218EAB File Offset: 0x002170AB
	public override float GetSortKey()
	{
		return 15f;
	}

	// Token: 0x06005B76 RID: 23414 RVA: 0x00218EB4 File Offset: 0x002170B4
	protected override void OnActivate()
	{
		this.closeButton.onClick += this.OnClickClose;
		this.nextMessageButton.onClick += this.OnClickNextMessage;
		MultiToggle multiToggle = this.dontShowAgainButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickDontShowAgain));
		bool flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
		this.dontShowAgainButton.ChangeState(flag ? 0 : 1);
		base.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
		this.OnMessagesChanged(null);
	}

	// Token: 0x06005B77 RID: 23415 RVA: 0x00218F60 File Offset: 0x00217160
	protected override void OnDeactivate()
	{
		base.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
	}

	// Token: 0x06005B78 RID: 23416 RVA: 0x00218F83 File Offset: 0x00217183
	private void OnClickClose()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06005B79 RID: 23417 RVA: 0x00218F96 File Offset: 0x00217196
	private void OnClickNextMessage()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
		NotificationScreen.Instance.OnClickNextMessage();
	}

	// Token: 0x06005B7A RID: 23418 RVA: 0x00218FB4 File Offset: 0x002171B4
	private void OnClickDontShowAgain()
	{
		this.dontShowAgainButton.NextState();
		bool flag = this.dontShowAgainButton.CurrentState == 0;
		KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
	}

	// Token: 0x06005B7B RID: 23419 RVA: 0x00218FEC File Offset: 0x002171EC
	private void OnMessagesChanged(object data)
	{
		this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
	}

	// Token: 0x06005B7C RID: 23420 RVA: 0x0021900C File Offset: 0x0021720C
	public void SetMessage(MessageDialog dialog, Message message)
	{
		this.title.text = message.GetTitle().ToUpper();
		dialog.GetComponent<RectTransform>().SetParent(this.body.GetComponent<RectTransform>());
		RectTransform component = dialog.GetComponent<RectTransform>();
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		dialog.transform.SetLocalPosition(Vector3.zero);
		dialog.SetMessage(message);
		dialog.OnClickAction();
		if (dialog.CanDontShowAgain)
		{
			this.dontShowAgainElement.SetActive(true);
			this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
			return;
		}
		this.dontShowAgainElement.SetActive(false);
		this.dontShowAgainDelegate = null;
	}

	// Token: 0x06005B7D RID: 23421 RVA: 0x002190B9 File Offset: 0x002172B9
	private void TryDontShowAgain()
	{
		if (this.dontShowAgainDelegate != null && this.dontShowAgainButton.CurrentState == 0)
		{
			this.dontShowAgainDelegate();
		}
	}

	// Token: 0x04003DA8 RID: 15784
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003DA9 RID: 15785
	[SerializeField]
	private KToggle nextMessageButton;

	// Token: 0x04003DAA RID: 15786
	[SerializeField]
	private GameObject dontShowAgainElement;

	// Token: 0x04003DAB RID: 15787
	[SerializeField]
	private MultiToggle dontShowAgainButton;

	// Token: 0x04003DAC RID: 15788
	[SerializeField]
	private LocText title;

	// Token: 0x04003DAD RID: 15789
	[SerializeField]
	private RectTransform body;

	// Token: 0x04003DAE RID: 15790
	private System.Action dontShowAgainDelegate;
}
