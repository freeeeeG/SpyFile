using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A8C RID: 2700
public class EmoteSelector
{
	// Token: 0x06003578 RID: 13688 RVA: 0x000F9AC4 File Offset: 0x000F7EC4
	public EmoteSelector(EmoteWheel _emoteWheel, Transform _follow = null)
	{
		this.m_emoteWheel = _emoteWheel;
		this.m_gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_emoteWheel.m_emoteWheelOptions.m_wheelPrefab);
		if (this.m_emoteWheel.ForUI)
		{
			this.m_gameObject.transform.SetParent(this.m_emoteWheel.m_uiPlayer.EmoteWheelAnchor, false);
		}
		else
		{
			HoverIconUIController hoverIconUIController = this.m_gameObject.RequestComponent<HoverIconUIController>();
			if (hoverIconUIController == null)
			{
				hoverIconUIController = this.m_gameObject.AddComponent<HoverIconUIController>();
			}
			this.m_gameObject.transform.SetParent(GameUtils.GetNamedCanvas("ScalingHUDCanvas").transform, false);
			hoverIconUIController.SetFollowTransform(_follow, new Vector2(0f, 0f));
			hoverIconUIController.ShouldFollow = (_follow != null);
		}
		this.m_rect = this.m_gameObject.RequestComponent<RectTransform>();
		this.m_rect.rotation = Quaternion.Euler(0f, 0f, 0f);
		this.m_uiMove = this.m_gameObject.RequestComponent<UI_Move>();
		Canvas canvas = this.m_gameObject.RequestComponentUpwardsRecursive<Canvas>();
		if (canvas != null)
		{
			this.m_canvasRect = canvas.transform;
		}
		this.m_selector = (this.m_gameObject.transform.FindChildRecursive("Selector") as RectTransform);
		if (this.m_selector == null)
		{
		}
		this.SpawnEmoteOptions();
		this.Hide();
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x000F9C60 File Offset: 0x000F8060
	private void SpawnEmoteOptions()
	{
		float radius = this.m_emoteWheel.m_emoteWheelOptions.m_radius;
		for (int i = 0; i < this.m_emoteWheel.m_emoteWheelOptions.m_options.Length; i++)
		{
			if (!(this.m_emoteWheel.m_emoteWheelOptions.m_options[i].m_wheelButtonPrefab == null))
			{
				if (!(this.m_emoteWheel.m_emoteWheelOptions.m_options[i].m_wheelButtonHighlightPrefab == null))
				{
					float x = radius * Mathf.Cos(-1.0471976f * (float)i);
					float y = radius * Mathf.Sin(-1.0471976f * (float)i);
					GameObject gameObject = new GameObject("Emote_" + i.ToString());
					gameObject.transform.SetParent(this.m_gameObject.transform, false);
					gameObject.transform.localPosition = new Vector3(x, y, 0f);
					GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_emoteWheel.m_emoteWheelOptions.m_options[i].m_wheelButtonPrefab, gameObject.transform, false);
					gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.m_emoteWheel.m_emoteWheelOptions.m_options[i].m_wheelButtonHighlightPrefab, gameObject.transform, false);
					gameObject3.transform.localPosition = new Vector3(0f, 0f, 0f);
					gameObject3.SetActive(false);
					this.m_options[i] = new EmoteSelector.EmoteButton(gameObject, gameObject2, gameObject3);
				}
			}
		}
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x000F9E04 File Offset: 0x000F8204
	public bool IsActive()
	{
		return this.m_gameObject.activeInHierarchy;
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x000F9E14 File Offset: 0x000F8214
	public void Show()
	{
		if (Input.mousePresent)
		{
			this.m_prevMousePos = Input.mousePosition;
		}
		this.SetSelected(-1);
		this.m_analogOffsetThreshold = 0f;
		this.m_analogPos = new Vector2(0f, 0f);
		this.Update(0f, 0f);
		this.m_gameObject.SetActive(true);
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x000F9E79 File Offset: 0x000F8279
	public void Hide()
	{
		this.m_gameObject.SetActive(false);
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x000F9E88 File Offset: 0x000F8288
	private void Update(float _x, float _y)
	{
		Vector2 vector = new Vector2(_x, _y);
		float num = this.m_emoteWheel.m_emoteWheelOptions.m_radius * this.m_emoteWheel.m_emoteWheelOptions.m_radius;
		if (Mathf.Abs(vector.sqrMagnitude) > num)
		{
			Vector3 vector2 = new Vector3(_x, _y, 0f);
			vector = vector2.normalized * this.m_emoteWheel.m_emoteWheelOptions.m_radius;
		}
		this.m_selector.anchoredPosition = new Vector3(vector.x, vector.y, 0f);
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x000F9F2C File Offset: 0x000F832C
	public void AnalogUpdate(float _x, float _y)
	{
		Vector2 vector = new Vector2(_x, _y);
		float magnitude = (this.m_analogPos - vector).magnitude;
		if (Mathf.Abs(magnitude) > this.m_analogOffsetThreshold)
		{
			int closestButton = this.GetClosestButton(_x, _y, null);
			if (closestButton != -1)
			{
				this.SetSelected(closestButton);
				this.Update(this.m_selected.Position.x, this.m_selected.Position.y);
				int closestButton2 = this.GetClosestButton(_x, _y, this.m_selected);
				float magnitude2 = (vector - this.m_selected.Position.XY()).magnitude;
				float magnitude3;
				if (closestButton2 != -1)
				{
					magnitude3 = (vector - this.m_options[closestButton2].Position.XY()).magnitude;
				}
				else
				{
					magnitude3 = vector.magnitude;
				}
				this.m_analogOffsetThreshold = magnitude3 - magnitude2;
				this.m_analogPos.Set(_x, _y);
			}
			else
			{
				this.SetSelected(-1);
				this.Update(0f, 0f);
				this.m_analogOffsetThreshold = this.m_emoteWheel.m_emoteWheelOptions.m_radius * 0.5f;
				this.m_analogPos.Set(_x, _y);
			}
		}
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x000FA078 File Offset: 0x000F8478
	public void Update(EmoteWheelOption.Connection.Direction _direction)
	{
		if (this.m_selected == null)
		{
			int selected = this.GetSelected();
			if (selected != -1)
			{
				this.SetSelected(selected);
			}
		}
		EmoteWheelOption.Connection[] array = this.m_emoteWheel.m_emoteWheelOptions.ConnectionsForButton((this.m_selected == null) ? -1 : this.m_options.FindIndex_Predicate((EmoteSelector.EmoteButton x) => x == this.m_selected));
		int connectedTo = array[(int)_direction].m_connectedTo;
		if (connectedTo != -2)
		{
			this.SetSelected(connectedTo);
			if (this.m_selected != null)
			{
				this.Update(this.m_selected.Position.x, this.m_selected.Position.y);
			}
			else
			{
				this.Update(0f, 0f);
			}
		}
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x000FA144 File Offset: 0x000F8544
	public void PointerUpdate()
	{
		if (!Input.mousePresent)
		{
			return;
		}
		Vector3 vector = this.m_prevMousePos - Input.mousePosition;
		if (vector.x != 0f || vector.y != 0f)
		{
			Vector3 vector2 = (!(this.m_uiMove != null)) ? Vector2.zero : this.m_uiMove.Offset;
			vector2 = vector2.MultipliedBy(this.m_canvasRect.lossyScale);
			Vector2 vector3;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_rect, Input.mousePosition - vector2, null, out vector3);
			this.Update(vector3.x, vector3.y);
			this.SetSelected(this.GetClosestButton(vector3.x, vector3.y, null));
		}
		this.m_prevMousePos = Input.mousePosition;
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x000FA228 File Offset: 0x000F8628
	protected void SetSelected(int _idx)
	{
		int num = -1;
		if (this.m_selected != null)
		{
			num = this.m_options.FindIndex_Predicate((EmoteSelector.EmoteButton x) => x == this.m_selected);
		}
		if (_idx != num && num != -1)
		{
			this.m_options[num].SetHighlighted(false);
		}
		if (_idx != -1)
		{
			this.m_selected = this.m_options[_idx];
			this.m_options[_idx].SetHighlighted(true);
		}
		else
		{
			this.m_selected = null;
		}
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x000FA2A8 File Offset: 0x000F86A8
	public int GetSelected()
	{
		return this.GetClosestButton(this.m_selector.anchoredPosition.x, this.m_selector.anchoredPosition.y, null);
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000FA2E4 File Offset: 0x000F86E4
	protected int GetClosestButton(float _x, float _y, EmoteSelector.EmoteButton _exclude = null)
	{
		Vector2 pos = new Vector2(_x, _y);
		float sqDistToCenter = Vector2.SqrMagnitude(pos);
		Generic<float, EmoteSelector.EmoteButton> scoreFunction = delegate(EmoteSelector.EmoteButton _emote)
		{
			if (_exclude == _emote)
			{
				return float.MaxValue;
			}
			float num = Vector2.SqrMagnitude(_emote.Position.XY() - pos);
			if (num > sqDistToCenter)
			{
				return float.MaxValue;
			}
			return num;
		};
		KeyValuePair<int, EmoteSelector.EmoteButton> selected = this.m_options.FindLowestScoring(scoreFunction);
		if (selected.Value == null || (float)selected.Key == 3.4028235E+38f)
		{
			return -1;
		}
		return this.m_options.FindIndex_Predicate((EmoteSelector.EmoteButton x) => x == selected.Value);
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000FA37C File Offset: 0x000F877C
	public void Destroy()
	{
		UnityEngine.Object.Destroy(this.m_gameObject);
	}

	// Token: 0x04002ADF RID: 10975
	private EmoteWheel m_emoteWheel;

	// Token: 0x04002AE0 RID: 10976
	private EmoteSelector.EmoteButton[] m_options = new EmoteSelector.EmoteButton[6];

	// Token: 0x04002AE1 RID: 10977
	private GameObject m_gameObject;

	// Token: 0x04002AE2 RID: 10978
	private RectTransform m_rect;

	// Token: 0x04002AE3 RID: 10979
	private RectTransform m_selector;

	// Token: 0x04002AE4 RID: 10980
	private EmoteSelector.EmoteButton m_selected;

	// Token: 0x04002AE5 RID: 10981
	private UI_Move m_uiMove;

	// Token: 0x04002AE6 RID: 10982
	private Transform m_canvasRect;

	// Token: 0x04002AE7 RID: 10983
	private Vector3 m_prevMousePos;

	// Token: 0x04002AE8 RID: 10984
	private Vector2 m_analogPos = new Vector2(0f, 0f);

	// Token: 0x04002AE9 RID: 10985
	private float m_analogOffsetThreshold;

	// Token: 0x02000A8D RID: 2701
	protected class EmoteButton
	{
		// Token: 0x06003587 RID: 13703 RVA: 0x000FA39F File Offset: 0x000F879F
		public EmoteButton(GameObject _container, GameObject _normal, GameObject _highlight)
		{
			this.m_container = _container;
			this.m_normal = _normal;
			this.m_highlight = _highlight;
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x000FA3BC File Offset: 0x000F87BC
		public Vector3 Position
		{
			get
			{
				return this.m_container.transform.localPosition;
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x000FA3D0 File Offset: 0x000F87D0
		public void SetHighlighted(bool _highlighted)
		{
			if (!this.m_highlight.activeInHierarchy && _highlighted)
			{
				GameUtils.TriggerAudio(GameOneShotAudioTag.UIEmoteToggle, this.m_container.layer);
			}
			this.m_normal.SetActive(!_highlighted);
			this.m_highlight.SetActive(_highlighted);
		}

		// Token: 0x04002AEA RID: 10986
		private GameObject m_container;

		// Token: 0x04002AEB RID: 10987
		private GameObject m_normal;

		// Token: 0x04002AEC RID: 10988
		private GameObject m_highlight;
	}
}
