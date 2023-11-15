using System;
using UnityEngine;

// Token: 0x02000C5A RID: 3162
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
	// Token: 0x0600646E RID: 25710 RVA: 0x00251C7D File Offset: 0x0024FE7D
	public Tag GetElementTag()
	{
		return this.elementTag;
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x0600646F RID: 25711 RVA: 0x00251C85 File Offset: 0x0024FE85
	public bool IsSelected
	{
		get
		{
			return this.checkBox.CurrentState == 1;
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06006470 RID: 25712 RVA: 0x00251C98 File Offset: 0x0024FE98
	// (remove) Token: 0x06006471 RID: 25713 RVA: 0x00251CD0 File Offset: 0x0024FED0
	public event Action<Tag, bool> OnSelectionChanged;

	// Token: 0x06006472 RID: 25714 RVA: 0x00251D05 File Offset: 0x0024FF05
	public MultiToggle GetCheckboxToggle()
	{
		return this.checkBox;
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06006473 RID: 25715 RVA: 0x00251D0D File Offset: 0x0024FF0D
	// (set) Token: 0x06006474 RID: 25716 RVA: 0x00251D15 File Offset: 0x0024FF15
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x06006475 RID: 25717 RVA: 0x00251D1E File Offset: 0x0024FF1E
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
		this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
		this.initialized = true;
	}

	// Token: 0x06006476 RID: 25718 RVA: 0x00251D5D File Offset: 0x0024FF5D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
	}

	// Token: 0x06006477 RID: 25719 RVA: 0x00251D6C File Offset: 0x0024FF6C
	public Sprite GetStorageObjectSprite(Tag t)
	{
		Sprite result = null;
		GameObject prefab = Assets.GetPrefab(t);
		if (prefab != null)
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x06006478 RID: 25720 RVA: 0x00251DB8 File Offset: 0x0024FFB8
	public void SetSprite(Tag t)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(t, "ui", false);
		this.elementImg.sprite = uisprite.first;
		this.elementImg.color = uisprite.second;
		this.elementImg.gameObject.SetActive(true);
	}

	// Token: 0x06006479 RID: 25721 RVA: 0x00251E0C File Offset: 0x0025000C
	public void SetTag(Tag newTag)
	{
		this.Initialize();
		this.elementTag = newTag;
		this.SetSprite(this.elementTag);
		string text = this.elementTag.ProperName();
		if (this.parent.IsStorage)
		{
			float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
			text = text + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}
		this.elementName.text = text;
	}

	// Token: 0x0600647A RID: 25722 RVA: 0x00251E83 File Offset: 0x00250083
	private void CheckBoxClicked()
	{
		this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
	}

	// Token: 0x0600647B RID: 25723 RVA: 0x00251E9F File Offset: 0x0025009F
	public void SetCheckBox(bool checkBoxState)
	{
		this.checkBox.ChangeState(checkBoxState ? 1 : 0);
		this.checkBoxImg.enabled = checkBoxState;
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
		}
	}

	// Token: 0x04004491 RID: 17553
	[SerializeField]
	private LocText elementName;

	// Token: 0x04004492 RID: 17554
	[SerializeField]
	private MultiToggle checkBox;

	// Token: 0x04004493 RID: 17555
	[SerializeField]
	private KImage elementImg;

	// Token: 0x04004494 RID: 17556
	private KImage checkBoxImg;

	// Token: 0x04004495 RID: 17557
	private Tag elementTag;

	// Token: 0x04004497 RID: 17559
	private TreeFilterableSideScreen parent;

	// Token: 0x04004498 RID: 17560
	private bool initialized;
}
