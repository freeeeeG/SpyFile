using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B59 RID: 2905
[AddComponentMenu("KMonoBehaviour/scripts/CrewListEntry")]
public class CrewListEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x060059DC RID: 23004 RVA: 0x0020E582 File Offset: 0x0020C782
	public MinionIdentity Identity
	{
		get
		{
			return this.identity;
		}
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x0020E58A File Offset: 0x0020C78A
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.BGImage.enabled = true;
		this.BorderHighlight.color = new Color(0.65882355f, 0.2901961f, 0.4745098f);
	}

	// Token: 0x060059DE RID: 23006 RVA: 0x0020E5BE File Offset: 0x0020C7BE
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.BGImage.enabled = false;
		this.BorderHighlight.color = new Color(0.8f, 0.8f, 0.8f);
	}

	// Token: 0x060059DF RID: 23007 RVA: 0x0020E5F4 File Offset: 0x0020C7F4
	public void OnPointerClick(PointerEventData eventData)
	{
		bool focus = Time.unscaledTime - this.lastClickTime < 0.3f;
		this.SelectCrewMember(focus);
		this.lastClickTime = Time.unscaledTime;
	}

	// Token: 0x060059E0 RID: 23008 RVA: 0x0020E628 File Offset: 0x0020C828
	public virtual void Populate(MinionIdentity _identity)
	{
		this.identity = _identity;
		if (this.portrait == null)
		{
			GameObject parent = (this.crewPortraitParent != null) ? this.crewPortraitParent : base.gameObject;
			this.portrait = Util.KInstantiateUI<CrewPortrait>(this.PortraitPrefab.gameObject, parent, false);
			if (this.crewPortraitParent == null)
			{
				this.portrait.transform.SetSiblingIndex(2);
			}
		}
		this.portrait.SetIdentityObject(_identity, true);
	}

	// Token: 0x060059E1 RID: 23009 RVA: 0x0020E6AB File Offset: 0x0020C8AB
	public virtual void Refresh()
	{
	}

	// Token: 0x060059E2 RID: 23010 RVA: 0x0020E6AD File Offset: 0x0020C8AD
	public void RefreshCrewPortraitContent()
	{
		if (this.portrait != null)
		{
			this.portrait.ForceRefresh();
		}
	}

	// Token: 0x060059E3 RID: 23011 RVA: 0x0020E6C8 File Offset: 0x0020C8C8
	private string seniorityString()
	{
		return this.identity.GetAttributes().GetProfessionString(true);
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x0020E6DC File Offset: 0x0020C8DC
	public void SelectCrewMember(bool focus)
	{
		if (focus)
		{
			SelectTool.Instance.SelectAndFocus(this.identity.transform.GetPosition(), this.identity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
			return;
		}
		SelectTool.Instance.Select(this.identity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x04003CDB RID: 15579
	protected MinionIdentity identity;

	// Token: 0x04003CDC RID: 15580
	protected CrewPortrait portrait;

	// Token: 0x04003CDD RID: 15581
	public CrewPortrait PortraitPrefab;

	// Token: 0x04003CDE RID: 15582
	public GameObject crewPortraitParent;

	// Token: 0x04003CDF RID: 15583
	protected bool mouseOver;

	// Token: 0x04003CE0 RID: 15584
	public Image BorderHighlight;

	// Token: 0x04003CE1 RID: 15585
	public Image BGImage;

	// Token: 0x04003CE2 RID: 15586
	public float lastClickTime;
}
