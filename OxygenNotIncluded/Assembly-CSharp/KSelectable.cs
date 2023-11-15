using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x00094577 File Offset: 0x00092777
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0009457F File Offset: 0x0009277F
	// (set) Token: 0x06001BC4 RID: 7108 RVA: 0x00094591 File Offset: 0x00092791
	public bool IsSelectable
	{
		get
		{
			return this.selectable && base.isActiveAndEnabled;
		}
		set
		{
			this.selectable = value;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x0009459A File Offset: 0x0009279A
	public bool DisableSelectMarker
	{
		get
		{
			return this.disableSelectMarker;
		}
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x000945A4 File Offset: 0x000927A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.statusItemGroup = new StatusItemGroup(base.gameObject);
		base.GetComponent<KPrefabID>() != null;
		if (this.entityName == null || this.entityName.Length <= 0)
		{
			this.SetName(base.name);
		}
		if (this.entityGender == null)
		{
			this.entityGender = "NB";
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x0009460C File Offset: 0x0009280C
	public virtual string GetName()
	{
		if (this.entityName == null || this.entityName == "" || this.entityName.Length <= 0)
		{
			global::Debug.Log("Warning Item has blank name!", base.gameObject);
			return base.name;
		}
		return this.entityName;
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x0009465E File Offset: 0x0009285E
	public void SetStatusIndicatorOffset(Vector3 offset)
	{
		if (this.statusItemGroup == null)
		{
			return;
		}
		this.statusItemGroup.SetOffset(offset);
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x00094675 File Offset: 0x00092875
	public void SetName(string name)
	{
		this.entityName = name;
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x0009467E File Offset: 0x0009287E
	public void SetGender(string Gender)
	{
		this.entityGender = Gender;
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x00094688 File Offset: 0x00092888
	public float GetZoom()
	{
		Bounds bounds = Util.GetBounds(base.gameObject);
		return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x000946C4 File Offset: 0x000928C4
	public Vector3 GetPortraitLocation()
	{
		return Util.GetBounds(base.gameObject).center;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000946E4 File Offset: 0x000928E4
	private void ClearHighlight()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(0f, 0f, 0f, 0f);
		}
		base.Trigger(-1201923725, false);
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x00094738 File Offset: 0x00092938
	private void ApplyHighlight(float highlight)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(highlight, highlight, highlight, highlight);
		}
		base.Trigger(-1201923725, true);
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0009477C File Offset: 0x0009297C
	public void Select()
	{
		this.selected = true;
		this.ClearHighlight();
		this.ApplyHighlight(0.2f);
		base.Trigger(-1503271301, true);
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			int childCount2 = base.transform.GetChild(i).childCount;
			for (int j = 0; j < childCount2; j++)
			{
				if (base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>() != null)
				{
					base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
				}
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00094894 File Offset: 0x00092A94
	public void Unselect()
	{
		if (this.selected)
		{
			this.selected = false;
			this.ClearHighlight();
			base.Trigger(-1503271301, false);
		}
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<LoopingSounds>() != null)
			{
				transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x0009498C File Offset: 0x00092B8C
	public void Hover(bool playAudio)
	{
		this.ClearHighlight();
		if (!DebugHandler.HideUI)
		{
			this.ApplyHighlight(0.25f);
		}
		if (playAudio)
		{
			this.PlayHoverSound();
		}
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000949AF File Offset: 0x00092BAF
	private void PlayHoverSound()
	{
		if (CellSelectionObject.IsSelectionObject(base.gameObject))
		{
			return;
		}
		UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000949C5 File Offset: 0x00092BC5
	public void Unhover()
	{
		if (!this.selected)
		{
			this.ClearHighlight();
		}
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000949D5 File Offset: 0x00092BD5
	public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
	{
		if (on)
		{
			return this.AddStatusItem(status_item, data);
		}
		return this.RemoveStatusItem(status_item, false);
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000949EB File Offset: 0x00092BEB
	public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
	{
		if (show)
		{
			if (guid != Guid.Empty)
			{
				return guid;
			}
			return this.AddStatusItem(status_item, data);
		}
		else
		{
			if (guid != Guid.Empty)
			{
				return this.RemoveStatusItem(guid, false);
			}
			return guid;
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00094A20 File Offset: 0x00092C20
	public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.SetStatusItem(category, status_item, data);
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x00094A3E File Offset: 0x00092C3E
	public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		if (guid != Guid.Empty)
		{
			this.statusItemGroup.RemoveStatusItem(guid, false);
		}
		return this.AddStatusItem(status_item, data);
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00094A71 File Offset: 0x00092C71
	public Guid AddStatusItem(StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.AddStatusItem(status_item, data, null);
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x00094A8F File Offset: 0x00092C8F
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(status_item, immediate);
		return Guid.Empty;
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x00094AB2 File Offset: 0x00092CB2
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(guid, immediate);
		return Guid.Empty;
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x00094AD5 File Offset: 0x00092CD5
	public bool HasStatusItem(StatusItem status_item)
	{
		return this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x00094AED File Offset: 0x00092CED
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		return this.statusItemGroup.GetStatusItem(category);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00094AFB File Offset: 0x00092CFB
	public StatusItemGroup GetStatusItemGroup()
	{
		return this.statusItemGroup;
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x00094B04 File Offset: 0x00092D04
	public void UpdateWorkerSelection(bool selected)
	{
		Workable[] components = base.GetComponents<Workable>();
		if (components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].worker != null && components[i].GetComponent<LoopingSounds>() != null)
				{
					components[i].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
				}
			}
		}
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x00094B58 File Offset: 0x00092D58
	public void UpdateWorkableSelection(bool selected)
	{
		Worker component = base.GetComponent<Worker>();
		if (component != null && component.workable != null)
		{
			Workable workable = base.GetComponent<Worker>().workable;
			if (workable.GetComponent<LoopingSounds>() != null)
			{
				workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
			}
		}
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00094BA9 File Offset: 0x00092DA9
	protected override void OnLoadLevel()
	{
		this.OnCleanUp();
		base.OnLoadLevel();
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00094BB8 File Offset: 0x00092DB8
	protected override void OnCleanUp()
	{
		if (this.statusItemGroup != null)
		{
			this.statusItemGroup.Destroy();
			this.statusItemGroup = null;
		}
		if (this.selected && SelectTool.Instance != null)
		{
			if (SelectTool.Instance.selected == this)
			{
				SelectTool.Instance.Select(null, true);
			}
			else
			{
				this.Unselect();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x04000F66 RID: 3942
	private const float hoverHighlight = 0.25f;

	// Token: 0x04000F67 RID: 3943
	private const float selectHighlight = 0.2f;

	// Token: 0x04000F68 RID: 3944
	public string entityName;

	// Token: 0x04000F69 RID: 3945
	public string entityGender;

	// Token: 0x04000F6A RID: 3946
	private bool selected;

	// Token: 0x04000F6B RID: 3947
	[SerializeField]
	private bool selectable = true;

	// Token: 0x04000F6C RID: 3948
	[SerializeField]
	private bool disableSelectMarker;

	// Token: 0x04000F6D RID: 3949
	private StatusItemGroup statusItemGroup;
}
