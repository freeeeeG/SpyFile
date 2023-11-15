using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B6F RID: 2927
[AddComponentMenu("KMonoBehaviour/scripts/TableRow")]
public class TableRow : KMonoBehaviour
{
	// Token: 0x06005AB9 RID: 23225 RVA: 0x00214FE8 File Offset: 0x002131E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.selectMinionButton != null)
		{
			this.selectMinionButton.onClick += this.SelectMinion;
			this.selectMinionButton.onDoubleClick += this.SelectAndFocusMinion;
		}
	}

	// Token: 0x06005ABA RID: 23226 RVA: 0x00215037 File Offset: 0x00213237
	public GameObject GetScroller(string scrollerID)
	{
		return this.scrollers[scrollerID];
	}

	// Token: 0x06005ABB RID: 23227 RVA: 0x00215045 File Offset: 0x00213245
	public GameObject GetScrollerBorder(string scrolledID)
	{
		return this.scrollerBorders[scrolledID];
	}

	// Token: 0x06005ABC RID: 23228 RVA: 0x00215054 File Offset: 0x00213254
	public void SelectMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.Select(minionIdentity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x06005ABD RID: 23229 RVA: 0x00215088 File Offset: 0x00213288
	public void SelectAndFocusMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
	}

	// Token: 0x06005ABE RID: 23230 RVA: 0x002150DC File Offset: 0x002132DC
	public void ConfigureAsWorldDivider(Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		ScrollRect scroll_rect = base.gameObject.GetComponentInChildren<ScrollRect>();
		this.rowType = TableRow.RowType.WorldDivider;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.scrollerID != "")
			{
				TableColumn value = keyValuePair.Value;
				break;
			}
		}
		scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
		{
			if (screen.CheckScrollersDirty())
			{
				return;
			}
			screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
		});
	}

	// Token: 0x06005ABF RID: 23231 RVA: 0x00215188 File Offset: 0x00213388
	public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		this.minion = minion;
		KImage componentInChildren = base.GetComponentInChildren<KImage>(true);
		componentInChildren.colorStyleSetting = ((minion == null) ? this.style_setting_default : this.style_setting_minion);
		componentInChildren.ColorState = KImage.ColorSelector.Inactive;
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null && minion as StoredMinionIdentity != null)
		{
			component.alpha = 0.6f;
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			GameObject gameObject;
			if (minion == null)
			{
				if (this.isDefault)
				{
					gameObject = keyValuePair.Value.GetDefaultWidget(base.gameObject);
				}
				else
				{
					gameObject = keyValuePair.Value.GetHeaderWidget(base.gameObject);
				}
			}
			else
			{
				gameObject = keyValuePair.Value.GetMinionWidget(base.gameObject);
			}
			this.widgets.Add(keyValuePair.Value, gameObject);
			keyValuePair.Value.widgets_by_row.Add(this, gameObject);
			if (keyValuePair.Value.scrollerID != "")
			{
				foreach (string text in keyValuePair.Value.screen.column_scrollers)
				{
					if (!(text != keyValuePair.Value.scrollerID))
					{
						if (!this.scrollers.ContainsKey(text))
						{
							GameObject gameObject2 = Util.KInstantiateUI(this.scrollerPrefab, base.gameObject, true);
							ScrollRect scroll_rect = gameObject2.GetComponent<ScrollRect>();
							scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
							{
								if (screen.CheckScrollersDirty())
								{
									return;
								}
								screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
							});
							this.scrollers.Add(text, scroll_rect.content.gameObject);
							if (scroll_rect.content.transform.parent.Find("Border") != null)
							{
								this.scrollerBorders.Add(text, scroll_rect.content.transform.parent.Find("Border").gameObject);
							}
						}
						gameObject.transform.SetParent(this.scrollers[text].transform);
						this.scrollers[text].transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
					}
				}
			}
		}
		this.RefreshColumns(columns);
		if (minion != null)
		{
			base.gameObject.name = minion.GetProperName();
		}
		else if (this.isDefault)
		{
			base.gameObject.name = "defaultRow";
		}
		if (this.selectMinionButton)
		{
			this.selectMinionButton.transform.SetAsLastSibling();
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			float width = rectTransform.rect.width;
			keyValuePair2.Value.transform.SetParent(base.gameObject.transform);
			rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2(0f, 1f));
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform();
			Vector3 a = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, -1f * (rectTransform2.sizeDelta.y / 2f), 0f);
			a.y = 0f;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 374f);
			rectTransform.SetLocalPosition(a + Vector3.up * rectTransform.GetLocalPosition().y + Vector3.up * -rectTransform.anchoredPosition.y);
		}
	}

	// Token: 0x06005AC0 RID: 23232 RVA: 0x0021566C File Offset: 0x0021386C
	public void RefreshColumns(Dictionary<string, TableColumn> columns)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.on_load_action != null)
			{
				keyValuePair.Value.on_load_action(this.minion, keyValuePair.Value.widgets_by_row[this]);
			}
		}
	}

	// Token: 0x06005AC1 RID: 23233 RVA: 0x002156EC File Offset: 0x002138EC
	public void RefreshScrollers()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollers)
		{
			ScrollRect component = keyValuePair.Value.transform.parent.GetComponent<ScrollRect>();
			component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			rectTransform.sizeDelta = new Vector2(this.scrollers[keyValuePair2.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06005AC2 RID: 23234 RVA: 0x002157FC File Offset: 0x002139FC
	public GameObject GetWidget(TableColumn column)
	{
		if (this.widgets.ContainsKey(column) && this.widgets[column] != null)
		{
			return this.widgets[column];
		}
		global::Debug.LogWarning("Widget is null or row does not contain widget for column " + ((column != null) ? column.ToString() : null));
		return null;
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x00215855 File Offset: 0x00213A55
	public IAssignableIdentity GetIdentity()
	{
		return this.minion;
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x0021585D File Offset: 0x00213A5D
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets.ContainsValue(widget);
	}

	// Token: 0x06005AC5 RID: 23237 RVA: 0x0021586C File Offset: 0x00213A6C
	public void Clear()
	{
		foreach (KeyValuePair<TableColumn, GameObject> keyValuePair in this.widgets)
		{
			keyValuePair.Key.widgets_by_row.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003D42 RID: 15682
	public TableRow.RowType rowType;

	// Token: 0x04003D43 RID: 15683
	private IAssignableIdentity minion;

	// Token: 0x04003D44 RID: 15684
	private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();

	// Token: 0x04003D45 RID: 15685
	private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();

	// Token: 0x04003D46 RID: 15686
	private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();

	// Token: 0x04003D47 RID: 15687
	public bool isDefault;

	// Token: 0x04003D48 RID: 15688
	public KButton selectMinionButton;

	// Token: 0x04003D49 RID: 15689
	[SerializeField]
	private ColorStyleSetting style_setting_default;

	// Token: 0x04003D4A RID: 15690
	[SerializeField]
	private ColorStyleSetting style_setting_minion;

	// Token: 0x04003D4B RID: 15691
	[SerializeField]
	private GameObject scrollerPrefab;

	// Token: 0x04003D4C RID: 15692
	[SerializeField]
	private GameObject scrollbarPrefab;

	// Token: 0x02001A9B RID: 6811
	public enum RowType
	{
		// Token: 0x040079F4 RID: 31220
		Header,
		// Token: 0x040079F5 RID: 31221
		Default,
		// Token: 0x040079F6 RID: 31222
		Minion,
		// Token: 0x040079F7 RID: 31223
		StoredMinon,
		// Token: 0x040079F8 RID: 31224
		WorldDivider
	}
}
