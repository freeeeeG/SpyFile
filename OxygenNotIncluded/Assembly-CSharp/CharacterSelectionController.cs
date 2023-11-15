using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000A58 RID: 2648
public class CharacterSelectionController : KModalScreen
{
	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06005013 RID: 20499 RVA: 0x001C5FC5 File Offset: 0x001C41C5
	// (set) Token: 0x06005014 RID: 20500 RVA: 0x001C5FCD File Offset: 0x001C41CD
	public bool IsStarterMinion { get; set; }

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06005015 RID: 20501 RVA: 0x001C5FD6 File Offset: 0x001C41D6
	public bool AllowsReplacing
	{
		get
		{
			return this.allowsReplacing;
		}
	}

	// Token: 0x06005016 RID: 20502 RVA: 0x001C5FDE File Offset: 0x001C41DE
	protected virtual void OnProceed()
	{
	}

	// Token: 0x06005017 RID: 20503 RVA: 0x001C5FE0 File Offset: 0x001C41E0
	protected virtual void OnDeliverableAdded()
	{
	}

	// Token: 0x06005018 RID: 20504 RVA: 0x001C5FE2 File Offset: 0x001C41E2
	protected virtual void OnDeliverableRemoved()
	{
	}

	// Token: 0x06005019 RID: 20505 RVA: 0x001C5FE4 File Offset: 0x001C41E4
	protected virtual void OnLimitReached()
	{
	}

	// Token: 0x0600501A RID: 20506 RVA: 0x001C5FE6 File Offset: 0x001C41E6
	protected virtual void OnLimitUnreached()
	{
	}

	// Token: 0x0600501B RID: 20507 RVA: 0x001C5FE8 File Offset: 0x001C41E8
	protected virtual void InitializeContainers()
	{
		this.DisableProceedButton();
		if (this.containers != null && this.containers.Count > 0)
		{
			return;
		}
		this.OnReplacedEvent = null;
		this.containers = new List<ITelepadDeliverableContainer>();
		if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
		{
			this.numberOfDuplicantOptions = 3;
			this.numberOfCarePackageOptions = 0;
		}
		else
		{
			this.numberOfCarePackageOptions = ((UnityEngine.Random.Range(0, 101) > 70) ? 2 : 1);
			this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
		}
		for (int i = 0; i < this.numberOfDuplicantOptions; i++)
		{
			CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
			characterContainer.SetController(this);
			this.containers.Add(characterContainer);
		}
		for (int j = 0; j < this.numberOfCarePackageOptions; j++)
		{
			CarePackageContainer carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
			carePackageContainer.SetController(this);
			this.containers.Add(carePackageContainer);
			carePackageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, carePackageContainer.transform.parent.childCount));
		}
		this.selectedDeliverables = new List<ITelepadDeliverable>();
	}

	// Token: 0x0600501C RID: 20508 RVA: 0x001C612C File Offset: 0x001C432C
	public virtual void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
		this.Show(false);
	}

	// Token: 0x0600501D RID: 20509 RVA: 0x001C6194 File Offset: 0x001C4394
	public void RemoveLast()
	{
		if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
		{
			return;
		}
		ITelepadDeliverable obj = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
		if (this.OnReplacedEvent != null)
		{
			this.OnReplacedEvent(obj);
		}
	}

	// Token: 0x0600501E RID: 20510 RVA: 0x001C61E4 File Offset: 0x001C43E4
	public void AddDeliverable(ITelepadDeliverable deliverable)
	{
		if (this.selectedDeliverables.Contains(deliverable))
		{
			global::Debug.Log("Tried to add the same minion twice.");
			return;
		}
		if (this.selectedDeliverables.Count >= this.selectableCount)
		{
			global::Debug.LogError("Tried to add minions beyond the allowed limit");
			return;
		}
		this.selectedDeliverables.Add(deliverable);
		this.OnDeliverableAdded();
		if (this.selectedDeliverables.Count == this.selectableCount)
		{
			this.EnableProceedButton();
			if (this.OnLimitReachedEvent != null)
			{
				this.OnLimitReachedEvent();
			}
			this.OnLimitReached();
		}
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x001C626C File Offset: 0x001C446C
	public void RemoveDeliverable(ITelepadDeliverable deliverable)
	{
		bool flag = this.selectedDeliverables.Count >= this.selectableCount;
		this.selectedDeliverables.Remove(deliverable);
		this.OnDeliverableRemoved();
		if (flag && this.selectedDeliverables.Count < this.selectableCount)
		{
			this.DisableProceedButton();
			if (this.OnLimitUnreachedEvent != null)
			{
				this.OnLimitUnreachedEvent();
			}
			this.OnLimitUnreached();
		}
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x001C62D6 File Offset: 0x001C44D6
	public bool IsSelected(ITelepadDeliverable deliverable)
	{
		return this.selectedDeliverables.Contains(deliverable);
	}

	// Token: 0x06005021 RID: 20513 RVA: 0x001C62E4 File Offset: 0x001C44E4
	protected void EnableProceedButton()
	{
		this.proceedButton.isInteractable = true;
		this.proceedButton.ClearOnClick();
		this.proceedButton.onClick += delegate()
		{
			this.OnProceed();
		};
	}

	// Token: 0x06005022 RID: 20514 RVA: 0x001C6314 File Offset: 0x001C4514
	protected void DisableProceedButton()
	{
		this.proceedButton.ClearOnClick();
		this.proceedButton.isInteractable = false;
		this.proceedButton.onClick += delegate()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		};
	}

	// Token: 0x04003463 RID: 13411
	[SerializeField]
	private CharacterContainer containerPrefab;

	// Token: 0x04003464 RID: 13412
	[SerializeField]
	private CarePackageContainer carePackageContainerPrefab;

	// Token: 0x04003465 RID: 13413
	[SerializeField]
	private GameObject containerParent;

	// Token: 0x04003466 RID: 13414
	[SerializeField]
	protected KButton proceedButton;

	// Token: 0x04003467 RID: 13415
	protected int numberOfDuplicantOptions = 3;

	// Token: 0x04003468 RID: 13416
	protected int numberOfCarePackageOptions;

	// Token: 0x04003469 RID: 13417
	[SerializeField]
	protected int selectableCount;

	// Token: 0x0400346A RID: 13418
	[SerializeField]
	private bool allowsReplacing;

	// Token: 0x0400346C RID: 13420
	protected List<ITelepadDeliverable> selectedDeliverables;

	// Token: 0x0400346D RID: 13421
	protected List<ITelepadDeliverableContainer> containers;

	// Token: 0x0400346E RID: 13422
	public System.Action OnLimitReachedEvent;

	// Token: 0x0400346F RID: 13423
	public System.Action OnLimitUnreachedEvent;

	// Token: 0x04003470 RID: 13424
	public Action<bool> OnReshuffleEvent;

	// Token: 0x04003471 RID: 13425
	public Action<ITelepadDeliverable> OnReplacedEvent;

	// Token: 0x04003472 RID: 13426
	public System.Action OnProceedEvent;
}
