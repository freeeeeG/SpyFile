using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C98 RID: 3224
public class GroupSelectorWidget : MonoBehaviour
{
	// Token: 0x060066A5 RID: 26277 RVA: 0x002644F7 File Offset: 0x002626F7
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	// Token: 0x060066A6 RID: 26278 RVA: 0x00264528 File Offset: 0x00262728
	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	// Token: 0x060066A7 RID: 26279 RVA: 0x00264580 File Offset: 0x00262780
	private void OnAddItemClicked()
	{
		if (!this.IsSubPanelOpen())
		{
			if (this.RebuildSubPanelOptions() > 0)
			{
				this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
				this.unselectedItemsPanel.gameObject.SetActive(true);
				this.unselectedItemsPanel.GetComponent<Selectable>().Select();
				return;
			}
		}
		else
		{
			this.CloseSubPanel();
		}
	}

	// Token: 0x060066A8 RID: 26280 RVA: 0x002645EC File Offset: 0x002627EC
	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	// Token: 0x060066A9 RID: 26281 RVA: 0x00264629 File Offset: 0x00262829
	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	// Token: 0x060066AA RID: 26282 RVA: 0x00264660 File Offset: 0x00262860
	private void RebuildSelectedVisualizers()
	{
		foreach (GameObject original in this.selectedVisualizers)
		{
			Util.KDestroyGameObject(original);
		}
		this.selectedVisualizers.Clear();
		foreach (int idx in this.selectedOptionIndices)
		{
			GameObject item = this.CreateItem(idx, new Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true);
			this.selectedVisualizers.Add(item);
		}
	}

	// Token: 0x060066AB RID: 26283 RVA: 0x00264724 File Offset: 0x00262924
	private GameObject CreateItem(int idx, Action<int> on_click, GameObject parent, bool is_selected_item)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
		KButton component = gameObject.GetComponent<KButton>();
		component.onClick += delegate()
		{
			on_click(idx);
		};
		component.fgImage.sprite = this.options[idx].sprite;
		if (parent == this.selectedItemsPanel.gameObject)
		{
			HierarchyReferences component2 = component.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				Component reference = component2.GetReference("CancelImg");
				if (reference != null)
				{
					reference.gameObject.SetActive(true);
				}
			}
		}
		gameObject.GetComponent<ToolTip>().OnToolTip = (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
		return gameObject;
	}

	// Token: 0x060066AC RID: 26284 RVA: 0x002647F6 File Offset: 0x002629F6
	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	// Token: 0x060066AD RID: 26285 RVA: 0x00264808 File Offset: 0x00262A08
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x060066AE RID: 26286 RVA: 0x00264824 File Offset: 0x00262A24
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x060066AF RID: 26287 RVA: 0x00264884 File Offset: 0x00262A84
	private int RebuildSubPanelOptions()
	{
		IList<int> list = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			using (IEnumerator<int> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					if (!this.selectedOptionIndices.Contains(num))
					{
						this.CreateItem(num, new Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
					}
				}
				goto IL_7E;
			}
		}
		this.CloseSubPanel();
		IL_7E:
		return list.Count;
	}

	// Token: 0x040046D2 RID: 18130
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x040046D3 RID: 18131
	[SerializeField]
	private RectTransform selectedItemsPanel;

	// Token: 0x040046D4 RID: 18132
	[SerializeField]
	private RectTransform unselectedItemsPanel;

	// Token: 0x040046D5 RID: 18133
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x040046D6 RID: 18134
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x040046D7 RID: 18135
	private object widgetID;

	// Token: 0x040046D8 RID: 18136
	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	// Token: 0x040046D9 RID: 18137
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x040046DA RID: 18138
	private List<int> selectedOptionIndices = new List<int>();

	// Token: 0x040046DB RID: 18139
	private List<GameObject> selectedVisualizers = new List<GameObject>();

	// Token: 0x02001BC0 RID: 7104
	[Serializable]
	public struct ItemData
	{
		// Token: 0x06009AE9 RID: 39657 RVA: 0x00347CC5 File Offset: 0x00345EC5
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		// Token: 0x04007DCE RID: 32206
		public Sprite sprite;

		// Token: 0x04007DCF RID: 32207
		public object userData;
	}

	// Token: 0x02001BC1 RID: 7105
	public struct ItemCallbacks
	{
		// Token: 0x04007DD0 RID: 32208
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		// Token: 0x04007DD1 RID: 32209
		public Action<object, object> onItemAdded;

		// Token: 0x04007DD2 RID: 32210
		public Action<object, object> onItemRemoved;

		// Token: 0x04007DD3 RID: 32211
		public Func<object, object, bool, string> getItemHoverText;
	}
}
