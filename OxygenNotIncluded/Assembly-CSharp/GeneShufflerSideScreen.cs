using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C1D RID: 3101
public class GeneShufflerSideScreen : SideScreenContent
{
	// Token: 0x06006220 RID: 25120 RVA: 0x00243AC9 File Offset: 0x00241CC9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += this.OnButtonClick;
		this.Refresh();
	}

	// Token: 0x06006221 RID: 25121 RVA: 0x00243AEE File Offset: 0x00241CEE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<GeneShuffler>() != null;
	}

	// Token: 0x06006222 RID: 25122 RVA: 0x00243AFC File Offset: 0x00241CFC
	public override void SetTarget(GameObject target)
	{
		GeneShuffler component = target.GetComponent<GeneShuffler>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a GeneShuffler associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x06006223 RID: 25123 RVA: 0x00243B34 File Offset: 0x00241D34
	private void OnButtonClick()
	{
		if (this.target.WorkComplete)
		{
			this.target.SetWorkTime(0f);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.target.RequestRecharge(!this.target.RechargeRequested);
			this.Refresh();
		}
	}

	// Token: 0x06006224 RID: 25124 RVA: 0x00243B8C File Offset: 0x00241D8C
	private void Refresh()
	{
		if (!(this.target != null))
		{
			this.contents.SetActive(false);
			return;
		}
		if (this.target.WorkComplete)
		{
			this.contents.SetActive(true);
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
			this.button.gameObject.SetActive(true);
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
			return;
		}
		if (this.target.IsConsumed)
		{
			this.contents.SetActive(true);
			this.button.gameObject.SetActive(true);
			if (this.target.RechargeRequested)
			{
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
				this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
				return;
			}
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
			return;
		}
		else
		{
			if (this.target.IsWorking)
			{
				this.contents.SetActive(true);
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
				this.button.gameObject.SetActive(false);
				return;
			}
			this.contents.SetActive(false);
			return;
		}
	}

	// Token: 0x040042E1 RID: 17121
	[SerializeField]
	private LocText label;

	// Token: 0x040042E2 RID: 17122
	[SerializeField]
	private KButton button;

	// Token: 0x040042E3 RID: 17123
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x040042E4 RID: 17124
	[SerializeField]
	private GeneShuffler target;

	// Token: 0x040042E5 RID: 17125
	[SerializeField]
	private GameObject contents;
}
