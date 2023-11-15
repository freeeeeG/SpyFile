using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5B RID: 2907
[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x060059F4 RID: 23028 RVA: 0x0020ED79 File Offset: 0x0020CF79
	// (set) Token: 0x060059F5 RID: 23029 RVA: 0x0020ED81 File Offset: 0x0020CF81
	public IAssignableIdentity identityObject { get; private set; }

	// Token: 0x060059F6 RID: 23030 RVA: 0x0020ED8A File Offset: 0x0020CF8A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.startTransparent)
		{
			base.StartCoroutine(this.AlphaIn());
		}
		this.requiresRefresh = true;
	}

	// Token: 0x060059F7 RID: 23031 RVA: 0x0020EDAE File Offset: 0x0020CFAE
	private IEnumerator AlphaIn()
	{
		this.SetAlpha(0f);
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime * 4f)
		{
			this.SetAlpha(i);
			yield return 0;
		}
		this.SetAlpha(1f);
		yield break;
	}

	// Token: 0x060059F8 RID: 23032 RVA: 0x0020EDBD File Offset: 0x0020CFBD
	private void OnRoleChanged(object data)
	{
		if (this.controller == null)
		{
			return;
		}
		CrewPortrait.RefreshHat(this.identityObject, this.controller);
	}

	// Token: 0x060059F9 RID: 23033 RVA: 0x0020EDE0 File Offset: 0x0020CFE0
	private void RegisterEvents()
	{
		if (this.areEventsRegistered)
		{
			return;
		}
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Subscribe(540773776, new Action<object>(this.OnRoleChanged));
		this.areEventsRegistered = true;
	}

	// Token: 0x060059FA RID: 23034 RVA: 0x0020EE2C File Offset: 0x0020D02C
	private void UnregisterEvents()
	{
		if (!this.areEventsRegistered)
		{
			return;
		}
		this.areEventsRegistered = false;
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Unsubscribe(540773776, new Action<object>(this.OnRoleChanged));
	}

	// Token: 0x060059FB RID: 23035 RVA: 0x0020EE76 File Offset: 0x0020D076
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RegisterEvents();
		this.ForceRefresh();
	}

	// Token: 0x060059FC RID: 23036 RVA: 0x0020EE8A File Offset: 0x0020D08A
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterEvents();
	}

	// Token: 0x060059FD RID: 23037 RVA: 0x0020EE98 File Offset: 0x0020D098
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.UnregisterEvents();
	}

	// Token: 0x060059FE RID: 23038 RVA: 0x0020EEA8 File Offset: 0x0020D0A8
	public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
	{
		this.UnregisterEvents();
		this.identityObject = identity;
		this.RegisterEvents();
		this.targetImage.enabled = true;
		if (this.identityObject != null)
		{
			this.targetImage.enabled = false;
		}
		if (this.useLabels && (identity is MinionIdentity || identity is MinionAssignablesProxy))
		{
			this.SetDuplicantJobTitleActive(jobEnabled);
		}
		this.requiresRefresh = true;
	}

	// Token: 0x060059FF RID: 23039 RVA: 0x0020EF10 File Offset: 0x0020D110
	public void SetSubTitle(string newTitle)
	{
		if (this.subTitle != null)
		{
			if (string.IsNullOrEmpty(newTitle))
			{
				this.subTitle.gameObject.SetActive(false);
				return;
			}
			this.subTitle.gameObject.SetActive(true);
			this.subTitle.SetText(newTitle);
		}
	}

	// Token: 0x06005A00 RID: 23040 RVA: 0x0020EF62 File Offset: 0x0020D162
	public void SetDuplicantJobTitleActive(bool state)
	{
		if (this.duplicantJob != null && this.duplicantJob.gameObject.activeInHierarchy != state)
		{
			this.duplicantJob.gameObject.SetActive(state);
		}
	}

	// Token: 0x06005A01 RID: 23041 RVA: 0x0020EF96 File Offset: 0x0020D196
	public void ForceRefresh()
	{
		this.requiresRefresh = true;
	}

	// Token: 0x06005A02 RID: 23042 RVA: 0x0020EF9F File Offset: 0x0020D19F
	public void Update()
	{
		if (this.requiresRefresh && (this.controller == null || this.controller.enabled))
		{
			this.requiresRefresh = false;
			this.Rebuild();
		}
	}

	// Token: 0x06005A03 RID: 23043 RVA: 0x0020EFD4 File Offset: 0x0020D1D4
	private void Rebuild()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponentInChildren<KBatchedAnimController>();
			if (this.controller == null)
			{
				if (this.targetImage != null)
				{
					this.targetImage.enabled = true;
				}
				global::Debug.LogWarning("Controller for [" + base.name + "] null");
				return;
			}
		}
		CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
		if (this.useLabels && this.duplicantName != null)
		{
			this.duplicantName.SetText((!this.identityObject.IsNullOrDestroyed()) ? this.identityObject.GetProperName() : "");
			if (this.identityObject is MinionIdentity && this.duplicantJob != null)
			{
				this.duplicantJob.SetText((this.identityObject != null) ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
				this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
			}
		}
	}

	// Token: 0x06005A04 RID: 23044 RVA: 0x0020F10C File Offset: 0x0020D30C
	private static void RefreshHat(IAssignableIdentity identityObject, KBatchedAnimController controller)
	{
		string hat_id = "";
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity != null)
		{
			hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
		}
		else if (identityObject as StoredMinionIdentity != null)
		{
			hat_id = (identityObject as StoredMinionIdentity).currentHat;
		}
		MinionResume.ApplyHat(hat_id, controller);
	}

	// Token: 0x06005A05 RID: 23045 RVA: 0x0020F160 File Offset: 0x0020D360
	public static void SetPortraitData(IAssignableIdentity identityObject, KBatchedAnimController controller, bool useDefaultExpression = true)
	{
		if (identityObject == null)
		{
			controller.gameObject.SetActive(false);
			return;
		}
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity == null)
		{
			MinionAssignablesProxy minionAssignablesProxy = identityObject as MinionAssignablesProxy;
			if (minionAssignablesProxy != null && minionAssignablesProxy.target != null)
			{
				minionIdentity = (minionAssignablesProxy.target as MinionIdentity);
			}
		}
		controller.gameObject.SetActive(true);
		controller.Play("ui_idle", KAnim.PlayMode.Once, 1f, 0f);
		SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		if (minionIdentity != null)
		{
			HashSet<KAnimHashedString> hashSet = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet2 = new HashSet<KAnimHashedString>();
			Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory = component2.GetAccessory(accessorySlot);
				if (accessory != null)
				{
					component.AddSymbolOverride(accessorySlot.targetSymbolId, accessory.symbol, 0);
					hashSet.Add(accessorySlot.targetSymbolId);
				}
				else
				{
					hashSet2.Add(accessorySlot.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet, true);
			controller.BatchSetSymbolsVisiblity(hashSet2, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(minionIdentity, controller);
		}
		else
		{
			HashSet<KAnimHashedString> hashSet3 = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet4 = new HashSet<KAnimHashedString>();
			StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
			if (storedMinionIdentity == null)
			{
				MinionAssignablesProxy minionAssignablesProxy2 = identityObject as MinionAssignablesProxy;
				if (minionAssignablesProxy2 != null && minionAssignablesProxy2.target != null)
				{
					storedMinionIdentity = (minionAssignablesProxy2.target as StoredMinionIdentity);
				}
			}
			if (!(storedMinionIdentity != null))
			{
				controller.gameObject.SetActive(false);
				return;
			}
			foreach (AccessorySlot accessorySlot2 in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory2 = storedMinionIdentity.GetAccessory(accessorySlot2);
				if (accessory2 != null)
				{
					component.AddSymbolOverride(accessorySlot2.targetSymbolId, accessory2.symbol, 0);
					hashSet3.Add(accessorySlot2.targetSymbolId);
				}
				else
				{
					hashSet4.Add(accessorySlot2.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet3, true);
			controller.BatchSetSymbolsVisiblity(hashSet4, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(storedMinionIdentity, controller);
		}
		float animScale = 0.25f;
		controller.animScale = animScale;
		string s = "ui_idle";
		controller.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		controller.SetSymbolVisiblity("snapTo_neck", false);
		controller.SetSymbolVisiblity("snapTo_goggles", false);
	}

	// Token: 0x06005A06 RID: 23046 RVA: 0x0020F4F8 File Offset: 0x0020D6F8
	public void SetAlpha(float value)
	{
		if (this.controller == null)
		{
			return;
		}
		if ((float)this.controller.TintColour.a != value)
		{
			this.controller.TintColour = new Color(1f, 1f, 1f, value);
		}
	}

	// Token: 0x04003CF2 RID: 15602
	public Image targetImage;

	// Token: 0x04003CF3 RID: 15603
	public bool startTransparent;

	// Token: 0x04003CF4 RID: 15604
	public bool useLabels = true;

	// Token: 0x04003CF5 RID: 15605
	[SerializeField]
	public KBatchedAnimController controller;

	// Token: 0x04003CF6 RID: 15606
	public float animScaleBase = 0.2f;

	// Token: 0x04003CF7 RID: 15607
	public LocText duplicantName;

	// Token: 0x04003CF8 RID: 15608
	public LocText duplicantJob;

	// Token: 0x04003CF9 RID: 15609
	public LocText subTitle;

	// Token: 0x04003CFA RID: 15610
	public bool useDefaultExpression = true;

	// Token: 0x04003CFB RID: 15611
	private bool requiresRefresh;

	// Token: 0x04003CFC RID: 15612
	private bool areEventsRegistered;
}
