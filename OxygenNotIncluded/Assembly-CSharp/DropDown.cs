using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF6 RID: 2806
[AddComponentMenu("KMonoBehaviour/scripts/DropDown")]
public class DropDown : KMonoBehaviour
{
	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06005693 RID: 22163 RVA: 0x001F97BF File Offset: 0x001F79BF
	// (set) Token: 0x06005694 RID: 22164 RVA: 0x001F97C7 File Offset: 0x001F79C7
	public bool open { get; private set; }

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06005695 RID: 22165 RVA: 0x001F97D0 File Offset: 0x001F79D0
	public List<IListableOption> Entries
	{
		get
		{
			return this.entries;
		}
	}

	// Token: 0x06005696 RID: 22166 RVA: 0x001F97D8 File Offset: 0x001F79D8
	public void Initialize(IEnumerable<IListableOption> contentKeys, Action<IListableOption, object> onEntrySelectedAction, Func<IListableOption, IListableOption, object, int> sortFunction = null, Action<DropDownEntry, object> refreshAction = null, bool displaySelectedValueWhenClosed = true, object targetData = null)
	{
		this.targetData = targetData;
		this.sortFunction = sortFunction;
		this.onEntrySelectedAction = onEntrySelectedAction;
		this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
		this.rowRefreshAction = refreshAction;
		this.ChangeContent(contentKeys);
		this.openButton.ClearOnClick();
		this.openButton.onClick += delegate()
		{
			this.OnClick();
		};
		this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
	}

	// Token: 0x06005697 RID: 22167 RVA: 0x001F9849 File Offset: 0x001F7A49
	public void CustomizeEmptyRow(string txt, Sprite icon)
	{
		this.emptyRowLabel = txt;
		this.emptyRowSprite = icon;
	}

	// Token: 0x06005698 RID: 22168 RVA: 0x001F9859 File Offset: 0x001F7A59
	public void OnClick()
	{
		if (!this.open)
		{
			this.Open();
			return;
		}
		this.Close();
	}

	// Token: 0x06005699 RID: 22169 RVA: 0x001F9870 File Offset: 0x001F7A70
	public void ChangeContent(IEnumerable<IListableOption> contentKeys)
	{
		this.entries.Clear();
		foreach (IListableOption item in contentKeys)
		{
			this.entries.Add(item);
		}
		this.built = false;
	}

	// Token: 0x0600569A RID: 22170 RVA: 0x001F98D0 File Offset: 0x001F7AD0
	private void Update()
	{
		if (!this.open)
		{
			return;
		}
		if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Mouse ScrollWheel") == 0f && !KInputManager.steamInputInterpreter.GetSteamInputActionIsDown(global::Action.MouseLeft))
		{
			return;
		}
		float canvasScale = this.canvasScaler.GetCanvasScale();
		if (this.scrollRect.rectTransform().GetPosition().x + this.scrollRect.rectTransform().sizeDelta.x * canvasScale < KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().x > KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().y - this.scrollRect.rectTransform().sizeDelta.y * canvasScale > KInputManager.GetMousePos().y || this.scrollRect.rectTransform().GetPosition().y < KInputManager.GetMousePos().y)
		{
			this.Close();
		}
	}

	// Token: 0x0600569B RID: 22171 RVA: 0x001F99D4 File Offset: 0x001F7BD4
	private void Build(List<IListableOption> contentKeys)
	{
		this.built = true;
		for (int i = this.contentContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.contentContainer.GetChild(i));
		}
		this.rowLookup.Clear();
		if (this.addEmptyRow)
		{
			this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			this.emptyRow.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(null, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = (this.emptyRowLabel ?? UI.DROPDOWN.NONE);
				}
				this.Close();
			};
			string text = this.emptyRowLabel ?? UI.DROPDOWN.NONE;
			this.emptyRow.GetComponent<DropDownEntry>().label.text = text;
			if (this.emptyRowSprite != null)
			{
				this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
			}
		}
		for (int j = 0; j < contentKeys.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			IListableOption id = contentKeys[j];
			gameObject.GetComponent<DropDownEntry>().entryData = id;
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(id, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = id.GetProperName();
				}
				this.Close();
			};
			this.rowLookup.Add(id, gameObject);
		}
		this.RefreshEntries();
		this.Close();
		this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
		this.scrollRect.gameObject.SetActive(false);
	}

	// Token: 0x0600569C RID: 22172 RVA: 0x001F9B74 File Offset: 0x001F7D74
	private void RefreshEntries()
	{
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
			component.label.text = keyValuePair.Key.GetProperName();
			if (component.portrait != null && keyValuePair.Key is IAssignableIdentity)
			{
				component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
			}
		}
		if (this.sortFunction != null)
		{
			this.entries.Sort((IListableOption a, IListableOption b) => this.sortFunction(a, b, this.targetData));
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.rowLookup[this.entries[i]].transform.SetAsFirstSibling();
			}
			if (this.emptyRow != null)
			{
				this.emptyRow.transform.SetAsFirstSibling();
			}
		}
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair2 in this.rowLookup)
		{
			DropDownEntry component2 = keyValuePair2.Value.GetComponent<DropDownEntry>();
			this.rowRefreshAction(component2, this.targetData);
		}
		if (this.emptyRow != null)
		{
			this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
		}
	}

	// Token: 0x0600569D RID: 22173 RVA: 0x001F9D18 File Offset: 0x001F7F18
	protected override void OnCleanUp()
	{
		Util.KDestroyGameObject(this.scrollRect);
		base.OnCleanUp();
	}

	// Token: 0x0600569E RID: 22174 RVA: 0x001F9D2C File Offset: 0x001F7F2C
	public void Open()
	{
		if (this.open)
		{
			return;
		}
		if (!this.built)
		{
			this.Build(this.entries);
		}
		else
		{
			this.RefreshEntries();
		}
		this.open = true;
		this.scrollRect.gameObject.SetActive(true);
		this.scrollRect.rectTransform().localScale = Vector3.one;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(true);
		}
		this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, 32f * (float)Mathf.Min(this.contentContainer.childCount, 8));
		Vector3 vector = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0f);
		Vector2 v = new Vector2(Mathf.Min(0f, (float)Screen.width - (vector.x + (this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.x))), -Mathf.Min(0f, vector.y - (this.scrollRect.rectTransform().sizeDelta.y * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.y)));
		vector += v;
		this.scrollRect.rectTransform().SetPosition(vector);
	}

	// Token: 0x0600569F RID: 22175 RVA: 0x001F9EF8 File Offset: 0x001F80F8
	public void Close()
	{
		if (!this.open)
		{
			return;
		}
		this.open = false;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(false);
		}
		this.scrollRect.SetActive(false);
	}

	// Token: 0x04003A40 RID: 14912
	public GameObject targetDropDownContainer;

	// Token: 0x04003A41 RID: 14913
	public LocText selectedLabel;

	// Token: 0x04003A43 RID: 14915
	public KButton openButton;

	// Token: 0x04003A44 RID: 14916
	public Transform contentContainer;

	// Token: 0x04003A45 RID: 14917
	public GameObject scrollRect;

	// Token: 0x04003A46 RID: 14918
	public RectTransform dropdownAlignmentTarget;

	// Token: 0x04003A47 RID: 14919
	public GameObject rowEntryPrefab;

	// Token: 0x04003A48 RID: 14920
	public bool addEmptyRow = true;

	// Token: 0x04003A49 RID: 14921
	private static Vector2 edgePadding = new Vector2(8f, 8f);

	// Token: 0x04003A4A RID: 14922
	public object targetData;

	// Token: 0x04003A4B RID: 14923
	private List<IListableOption> entries = new List<IListableOption>();

	// Token: 0x04003A4C RID: 14924
	private Action<IListableOption, object> onEntrySelectedAction;

	// Token: 0x04003A4D RID: 14925
	private Action<DropDownEntry, object> rowRefreshAction;

	// Token: 0x04003A4E RID: 14926
	public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();

	// Token: 0x04003A4F RID: 14927
	private Func<IListableOption, IListableOption, object, int> sortFunction;

	// Token: 0x04003A50 RID: 14928
	private GameObject emptyRow;

	// Token: 0x04003A51 RID: 14929
	private string emptyRowLabel;

	// Token: 0x04003A52 RID: 14930
	private Sprite emptyRowSprite;

	// Token: 0x04003A53 RID: 14931
	private bool built;

	// Token: 0x04003A54 RID: 14932
	private bool displaySelectedValueWhenClosed = true;

	// Token: 0x04003A55 RID: 14933
	private const int ROWS_BEFORE_SCROLL = 8;

	// Token: 0x04003A56 RID: 14934
	private KCanvasScaler canvasScaler;
}
