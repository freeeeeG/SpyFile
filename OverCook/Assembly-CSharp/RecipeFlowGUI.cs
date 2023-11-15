using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
public class RecipeFlowGUI : UIControllerBase
{
	// Token: 0x06003A86 RID: 14982 RVA: 0x00116B3C File Offset: 0x00114F3C
	public RecipeFlowGUI.ElementToken AddElement(OrderDefinitionNode _data, float _timeLimit, VoidGeneric<RecipeFlowGUI.ElementToken> _expirationCallback)
	{
		int tableNumber = this.ClaimUnoccupiedTable();
		GameObject obj = GameUtils.InstantiateUIController(this.m_recipeWidgetPrefab.gameObject, base.transform as RectTransform);
		RecipeWidgetUIController recipeWidgetUIController = obj.RequireComponent<RecipeWidgetUIController>();
		recipeWidgetUIController.SetupFromOrderDefinition(_data, tableNumber);
		RecipeFlowGUI.RecipeWidgetData recipeWidgetData = new RecipeFlowGUI.RecipeWidgetData(recipeWidgetUIController, 0f, this.m_nextIndex, _timeLimit, _expirationCallback);
		this.m_nextIndex++;
		this.m_widgets.Add(recipeWidgetData);
		return new RecipeFlowGUI.ElementToken(recipeWidgetData);
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x00116BB0 File Offset: 0x00114FB0
	private int ClaimUnoccupiedTable()
	{
		int num = 0;
		for (int i = 0; i < this.m_occupiedTables.Length; i++)
		{
			if (!this.m_occupiedTables[i])
			{
				num++;
			}
		}
		int num2 = UnityEngine.Random.Range(0, num);
		for (int j = 0; j < this.m_occupiedTables.Length; j++)
		{
			if (!this.m_occupiedTables[j])
			{
				if (num2 == 0)
				{
					this.m_occupiedTables[j] = true;
					return j;
				}
				num2--;
			}
		}
		return -1;
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x00116C2D File Offset: 0x0011502D
	private void ReleaseTable(int _tableId)
	{
		this.m_occupiedTables[_tableId] = false;
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x00116C38 File Offset: 0x00115038
	private int GetMaxOrderNumber()
	{
		return this.m_maxOrdersAllowed;
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x00116C40 File Offset: 0x00115040
	public bool IsFull()
	{
		int maxOrderNumber = this.GetMaxOrderNumber();
		return maxOrderNumber <= this.m_widgets.Count;
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x00116C65 File Offset: 0x00115065
	public bool IsEmpty()
	{
		return this.m_widgets.Count == 0;
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x00116C78 File Offset: 0x00115078
	public void AdjustTimeLimitForElement(RecipeFlowGUI.ElementToken _token, float _newTimeLimit)
	{
		RecipeFlowGUI.RecipeWidgetData data = this.GetData(_token);
		data.m_timeLimit = _newTimeLimit;
	}

	// Token: 0x06003A8D RID: 14989 RVA: 0x00116C94 File Offset: 0x00115094
	public float GetTimePropRemainingForElement(RecipeFlowGUI.ElementToken _token)
	{
		RecipeFlowGUI.RecipeWidgetData data = this.GetData(_token);
		return data.m_widget.GetTimePropRemaining();
	}

	// Token: 0x06003A8E RID: 14990 RVA: 0x00116CB4 File Offset: 0x001150B4
	public void ResetElementTimer(RecipeFlowGUI.ElementToken _token)
	{
		RecipeFlowGUI.RecipeWidgetData data = this.GetData(_token);
		data.m_widget.SetTimePropRemaining(1f);
	}

	// Token: 0x06003A8F RID: 14991 RVA: 0x00116CDC File Offset: 0x001150DC
	public void SetElementStuck(RecipeFlowGUI.ElementToken _token)
	{
		RecipeFlowGUI.RecipeWidgetData data = this.GetData(_token);
		data.m_widget.SetIsStuck();
	}

	// Token: 0x06003A90 RID: 14992 RVA: 0x00116CFC File Offset: 0x001150FC
	public RecipeFlowGUI.RecipeWidgetData GetData(RecipeFlowGUI.ElementToken _token)
	{
		return this.m_widgets.Find((RecipeFlowGUI.RecipeWidgetData obj) => new RecipeFlowGUI.ElementToken(obj) == _token);
	}

	// Token: 0x06003A91 RID: 14993 RVA: 0x00116D30 File Offset: 0x00115130
	public void PlayAnimationOnElement(RecipeFlowGUI.ElementToken _token, WidgetAnimation _animation)
	{
		RecipeFlowGUI.RecipeWidgetData data = this.GetData(_token);
		data.m_widget.PlayAnimation(_animation);
	}

	// Token: 0x06003A92 RID: 14994 RVA: 0x00116D54 File Offset: 0x00115154
	public void RemoveElement(RecipeFlowGUI.ElementToken _token, WidgetAnimation _deathAnim = null)
	{
		for (int i = this.m_widgets.Count - 1; i >= 0; i--)
		{
			RecipeFlowGUI.RecipeWidgetData recipeWidgetData = this.m_widgets[i];
			if (new RecipeFlowGUI.ElementToken(recipeWidgetData) == _token)
			{
				if (_deathAnim != null)
				{
					this.ReleaseTable(recipeWidgetData.m_widget.GetTableNumber());
					recipeWidgetData.m_widget.PlayAnimation(_deathAnim);
					this.m_dyingWidgets.Add(recipeWidgetData);
				}
				this.m_widgets.RemoveAt(i);
			}
		}
	}

	// Token: 0x06003A93 RID: 14995 RVA: 0x00116DD8 File Offset: 0x001151D8
	public void PlayAnimation(WidgetAnimation _animation)
	{
		for (int i = 0; i < this.m_widgets.Count; i++)
		{
			this.m_widgets[i].m_widget.PlayAnimation(_animation);
		}
	}

	// Token: 0x06003A94 RID: 14996 RVA: 0x00116E18 File Offset: 0x00115218
	private void Awake()
	{
		this.m_mainCamera = Camera.main;
		this.m_occupiedTables = new bool[this.GetMaxOrderNumber()];
	}

	// Token: 0x06003A95 RID: 14997 RVA: 0x00116E36 File Offset: 0x00115236
	private void OnDestroy()
	{
	}

	// Token: 0x06003A96 RID: 14998 RVA: 0x00116E38 File Offset: 0x00115238
	public void UpdateTimers(float _dt)
	{
		for (int i = 0; i < this.m_widgets.Count; i++)
		{
			float timePropRemaining = this.m_widgets[i].m_widget.GetTimePropRemaining();
			float num = timePropRemaining;
			num = Mathf.Max(num - _dt / this.m_widgets[i].m_timeLimit, 0f);
			this.m_widgets[i].m_widget.SetTimePropRemaining(num);
			if (timePropRemaining > 0f && num <= 0f)
			{
				this.m_widgets[i].m_expirationCallback(new RecipeFlowGUI.ElementToken(this.m_widgets[i]));
			}
		}
	}

	// Token: 0x06003A97 RID: 14999 RVA: 0x00116EEE File Offset: 0x001152EE
	private void Update()
	{
		this.m_dyingWidgets.RemoveAll(delegate(RecipeFlowGUI.RecipeWidgetData obj)
		{
			if (!obj.m_widget.IsPlayingAnimation())
			{
				UnityEngine.Object.Destroy(obj.m_widget.gameObject);
				return true;
			}
			return false;
		});
		this.LayoutWidgets();
	}

	// Token: 0x06003A98 RID: 15000 RVA: 0x00116F20 File Offset: 0x00115320
	private void LayoutWidgets()
	{
		this.FindAllWidgetsOrdered(ref this.m_ordererWidgets);
		float distanceFromEndOfScreen = this.m_distanceFromEndOfScreen;
		float distanceBetweenOrders = this.m_distanceBetweenOrders;
		float num = distanceFromEndOfScreen - distanceBetweenOrders;
		for (int i = 0; i < this.m_ordererWidgets.Count; i++)
		{
			RecipeWidgetUIController widget = this.m_ordererWidgets[i].m_widget;
			float width = widget.GetBounds().width;
			RectTransformExtension rectTransformExtension = widget.gameObject.RequireComponent<RectTransformExtension>();
			float num2 = num + distanceBetweenOrders;
			rectTransformExtension.AnchorOffset = new Vector2(0f, 0f);
			rectTransformExtension.PixelOffset = new Vector2(num2, 0f);
			num = num2 + width;
		}
	}

	// Token: 0x06003A99 RID: 15001 RVA: 0x00116FD0 File Offset: 0x001153D0
	private void FindAllWidgetsOrdered(ref List<RecipeFlowGUI.RecipeWidgetData> _widgets)
	{
		_widgets.Clear();
		_widgets.AddRange(this.m_widgets);
		for (int i = 0; i < this.m_dyingWidgets.Count; i++)
		{
			RecipeFlowGUI.RecipeWidgetData recipeWidgetData = this.m_dyingWidgets[i];
			int num = -1;
			for (int j = 0; j < _widgets.Count; j++)
			{
				if (_widgets[j].m_order > recipeWidgetData.m_order)
				{
					num = j;
					break;
				}
			}
			if (num == -1)
			{
				_widgets.Add(recipeWidgetData);
			}
			else
			{
				_widgets.Insert(num, recipeWidgetData);
			}
		}
	}

	// Token: 0x04002F74 RID: 12148
	[SerializeField]
	private float m_distanceBetweenOrders = 5f;

	// Token: 0x04002F75 RID: 12149
	[SerializeField]
	private float m_distanceFromEndOfScreen = 5f;

	// Token: 0x04002F76 RID: 12150
	[SerializeField]
	private int m_maxOrdersAllowed = 5;

	// Token: 0x04002F77 RID: 12151
	[SerializeField]
	private RecipeWidgetUIController m_recipeWidgetPrefab;

	// Token: 0x04002F78 RID: 12152
	private int m_nextIndex;

	// Token: 0x04002F79 RID: 12153
	private Camera m_mainCamera;

	// Token: 0x04002F7A RID: 12154
	private List<RecipeFlowGUI.RecipeWidgetData> m_widgets = new List<RecipeFlowGUI.RecipeWidgetData>();

	// Token: 0x04002F7B RID: 12155
	private List<RecipeFlowGUI.RecipeWidgetData> m_dyingWidgets = new List<RecipeFlowGUI.RecipeWidgetData>();

	// Token: 0x04002F7C RID: 12156
	private bool[] m_occupiedTables;

	// Token: 0x04002F7D RID: 12157
	private List<RecipeFlowGUI.RecipeWidgetData> m_ordererWidgets = new List<RecipeFlowGUI.RecipeWidgetData>();

	// Token: 0x02000B43 RID: 2883
	public class RecipeWidgetData
	{
		// Token: 0x06003A9B RID: 15003 RVA: 0x00117095 File Offset: 0x00115495
		public RecipeWidgetData(RecipeWidgetUIController _widget, float _prop, int _order, float _timeLimit, VoidGeneric<RecipeFlowGUI.ElementToken> _expirationCallback)
		{
			this.m_widget = _widget;
			this.m_order = _order;
			this.m_timeLimit = _timeLimit;
			this.m_expirationCallback = _expirationCallback;
		}

		// Token: 0x04002F7F RID: 12159
		public RecipeWidgetUIController m_widget;

		// Token: 0x04002F80 RID: 12160
		public float m_timeLimit;

		// Token: 0x04002F81 RID: 12161
		public int m_order;

		// Token: 0x04002F82 RID: 12162
		public VoidGeneric<RecipeFlowGUI.ElementToken> m_expirationCallback;
	}

	// Token: 0x02000B44 RID: 2884
	public struct ElementToken
	{
		// Token: 0x06003A9C RID: 15004 RVA: 0x001170BB File Offset: 0x001154BB
		public ElementToken(RecipeFlowGUI.RecipeWidgetData _widget)
		{
			this.m_widget = _widget;
		}

		// Token: 0x06003A9D RID: 15005 RVA: 0x001170C4 File Offset: 0x001154C4
		public static bool operator ==(RecipeFlowGUI.ElementToken _token1, RecipeFlowGUI.ElementToken _token2)
		{
			return _token1.m_widget == _token2.m_widget;
		}

		// Token: 0x06003A9E RID: 15006 RVA: 0x001170D6 File Offset: 0x001154D6
		public static bool operator !=(RecipeFlowGUI.ElementToken _token1, RecipeFlowGUI.ElementToken _token2)
		{
			return _token1.m_widget != _token2.m_widget;
		}

		// Token: 0x04002F83 RID: 12163
		private RecipeFlowGUI.RecipeWidgetData m_widget;
	}
}
