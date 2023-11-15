using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06003059 RID: 12377 RVA: 0x000FFB61 File Offset: 0x000FDD61
	// (set) Token: 0x0600305A RID: 12378 RVA: 0x000FFB69 File Offset: 0x000FDD69
	[MyCmpAdd]
	public Health health { get; private set; }

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x0600305B RID: 12379 RVA: 0x000FFB72 File Offset: 0x000FDD72
	// (set) Token: 0x0600305C RID: 12380 RVA: 0x000FFB7A File Offset: 0x000FDD7A
	public AttackableBase attackable { get; private set; }

	// Token: 0x0600305D RID: 12381 RVA: 0x000FFB84 File Offset: 0x000FDD84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.health = base.GetComponent<Health>();
		this.attackable = base.GetComponent<AttackableBase>();
		Components.FactionAlignments.Add(this);
		base.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
		base.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
		if (this.alignmentActive)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
		}
		GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
		this.UpdateStatusItem();
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x000FFC12 File Offset: 0x000FDE12
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000FFC14 File Offset: 0x000FDE14
	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000FFC20 File Offset: 0x000FDE20
	public void SetAlignmentActive(bool active)
	{
		this.SetPlayerTargetable(active);
		this.alignmentActive = active;
		if (active)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
			return;
		}
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000FFC77 File Offset: 0x000FDE77
	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000FFC94 File Offset: 0x000FDE94
	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000FFC9C File Offset: 0x000FDE9C
	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x000FFCBA File Offset: 0x000FDEBA
	public void SetPlayerTargeted(bool state)
	{
		this.targeted = (this.canBePlayerTargeted && state && this.targetable);
		this.UpdateStatusItem();
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x000FFCDC File Offset: 0x000FDEDC
	private void UpdateStatusItem()
	{
		this.TogglePrioritizable(this.targeted);
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x000FFD38 File Offset: 0x000FDF38
	private void TogglePrioritizable(bool enable)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component == null || !this.updatePrioritizable)
		{
			return;
		}
		if (enable && !this.hasBeenRegisterInPriority)
		{
			Prioritizable.AddRef(base.gameObject);
			this.hasBeenRegisterInPriority = true;
			return;
		}
		if (component.IsPrioritizable() && this.hasBeenRegisterInPriority)
		{
			Prioritizable.RemoveRef(base.gameObject);
			this.hasBeenRegisterInPriority = false;
		}
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x000FFD9E File Offset: 0x000FDF9E
	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x000FFDB5 File Offset: 0x000FDFB5
	protected override void OnCleanUp()
	{
		Components.FactionAlignments.Remove(this);
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x000FFDE4 File Offset: 0x000FDFE4
	private void OnRefreshUserMenu(object data)
	{
		if (this.Alignment == FactionManager.FactionID.Duplicant)
		{
			return;
		}
		if (!this.canBePlayerTargeted)
		{
			return;
		}
		if (!this.IsAlignmentActive())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.targeted) ? new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.ATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.CANCELATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001C8E RID: 7310
	[SerializeField]
	public bool canBePlayerTargeted = true;

	// Token: 0x04001C8F RID: 7311
	[SerializeField]
	public bool updatePrioritizable = true;

	// Token: 0x04001C90 RID: 7312
	[Serialize]
	private bool alignmentActive = true;

	// Token: 0x04001C91 RID: 7313
	public FactionManager.FactionID Alignment;

	// Token: 0x04001C92 RID: 7314
	[Serialize]
	private bool targeted;

	// Token: 0x04001C93 RID: 7315
	[Serialize]
	private bool targetable = true;

	// Token: 0x04001C94 RID: 7316
	private bool hasBeenRegisterInPriority;

	// Token: 0x04001C95 RID: 7317
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04001C96 RID: 7318
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001C97 RID: 7319
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});
}
