using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA6 RID: 2726
[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06005335 RID: 21301 RVA: 0x001DD2C4 File Offset: 0x001DB4C4
	// (remove) Token: 0x06005336 RID: 21302 RVA: 0x001DD2FC File Offset: 0x001DB4FC
	public event Action<MinionIdentity> OnDuplicantSelected;

	// Token: 0x06005337 RID: 21303 RVA: 0x001DD331 File Offset: 0x001DB531
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005338 RID: 21304 RVA: 0x001DD358 File Offset: 0x001DB558
	public void Open()
	{
		base.gameObject.SetActive(true);
		this.buttonPool.ClearAll();
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
			CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
			componentInChildren.SetIdentityObject(minionIdentity, true);
			this.portraitList.Add(componentInChildren);
			btn.ClearOnClick();
			btn.onClick += delegate()
			{
				this.SelectDuplicant(btn);
			};
			this.buttonIdentityMap.Add(btn, minionIdentity);
		}
	}

	// Token: 0x06005339 RID: 21305 RVA: 0x001DD440 File Offset: 0x001DB640
	public void Close()
	{
		this.buttonPool.DestroyAllActive();
		this.buttonIdentityMap.Clear();
		this.portraitList.Clear();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600533A RID: 21306 RVA: 0x001DD46F File Offset: 0x001DB66F
	private void SelectDuplicant(KButton btn)
	{
		if (this.OnDuplicantSelected != null)
		{
			this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
		}
		this.Close();
	}

	// Token: 0x04003768 RID: 14184
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04003769 RID: 14185
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x0400376A RID: 14186
	private UIPool<KButton> buttonPool;

	// Token: 0x0400376B RID: 14187
	private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();

	// Token: 0x0400376C RID: 14188
	private List<CrewPortrait> portraitList = new List<CrewPortrait>();
}
