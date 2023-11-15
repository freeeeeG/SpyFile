using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C13 RID: 3091
public class DispenserSideScreen : SideScreenContent
{
	// Token: 0x060061E2 RID: 25058 RVA: 0x0024227A File Offset: 0x0024047A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDispenser>() != null;
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x00242285 File Offset: 0x00240485
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetDispenser = target.GetComponent<IDispenser>();
		this.Refresh();
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x002422A0 File Offset: 0x002404A0
	private void Refresh()
	{
		this.dispenseButton.ClearOnClick();
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.targetDispenser.DispensedItems())
		{
			GameObject gameObject = Util.KInstantiateUI(this.itemRowPrefab, this.itemRowContainer.gameObject, true);
			this.rows.Add(tag, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(tag, "ui", false).first;
			component.GetReference<LocText>("Label").text = Assets.GetPrefab(tag).GetProperName();
			gameObject.GetComponent<MultiToggle>().ChangeState((tag == this.targetDispenser.SelectedItem()) ? 0 : 1);
		}
		if (this.targetDispenser.HasOpenChore())
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnCancelDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
		}
		else
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnOrderDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
		}
		this.targetDispenser.OnStopWorkEvent -= this.Refresh;
		this.targetDispenser.OnStopWorkEvent += this.Refresh;
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x00242484 File Offset: 0x00240684
	private void SelectTag(Tag tag)
	{
		this.targetDispenser.SelectItem(tag);
		this.Refresh();
	}

	// Token: 0x040042B2 RID: 17074
	[SerializeField]
	private KButton dispenseButton;

	// Token: 0x040042B3 RID: 17075
	[SerializeField]
	private RectTransform itemRowContainer;

	// Token: 0x040042B4 RID: 17076
	[SerializeField]
	private GameObject itemRowPrefab;

	// Token: 0x040042B5 RID: 17077
	private IDispenser targetDispenser;

	// Token: 0x040042B6 RID: 17078
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
