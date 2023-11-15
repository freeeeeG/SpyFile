using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BD3 RID: 3027
public class ResearchScreen : KModalScreen
{
	// Token: 0x06005F29 RID: 24361 RVA: 0x0022F787 File Offset: 0x0022D987
	public bool IsBeingResearched(Tech tech)
	{
		return Research.Instance.IsBeingResearched(tech);
	}

	// Token: 0x06005F2A RID: 24362 RVA: 0x0022F794 File Offset: 0x0022D994
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x06005F2B RID: 24363 RVA: 0x0022F7AC File Offset: 0x0022D9AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		Transform transform = base.transform;
		while (this.m_Raycaster == null)
		{
			this.m_Raycaster = transform.GetComponent<GraphicRaycaster>();
			if (this.m_Raycaster == null)
			{
				transform = transform.parent;
			}
		}
	}

	// Token: 0x06005F2C RID: 24364 RVA: 0x0022F7FE File Offset: 0x0022D9FE
	private void ZoomOut()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x06005F2D RID: 24365 RVA: 0x0022F82B File Offset: 0x0022DA2B
	private void ZoomIn()
	{
		this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerButton, this.minZoom, this.maxZoom);
		this.zoomCenterLock = true;
	}

	// Token: 0x06005F2E RID: 24366 RVA: 0x0022F858 File Offset: 0x0022DA58
	public void ZoomToTech(string techID)
	{
		Vector2 a = this.entryMap[Db.Get().Techs.Get(techID)].rectTransform().GetLocalPosition() + new Vector2(-this.foreground.rectTransform().rect.size.x / 2f, this.foreground.rectTransform().rect.size.y / 2f);
		this.forceTargetPosition = -a;
		this.zoomingToTarget = true;
		this.targetZoom = this.maxZoom;
	}

	// Token: 0x06005F2F RID: 24367 RVA: 0x0022F900 File Offset: 0x0022DB00
	private void Update()
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		RectTransform component = this.scrollContent.GetComponent<RectTransform>();
		if (this.isDragging && !KInputManager.isFocused)
		{
			this.AbortDragging();
		}
		Vector2 anchoredPosition = component.anchoredPosition;
		float t = Mathf.Min(this.effectiveZoomSpeed * Time.unscaledDeltaTime, 0.9f);
		this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, t);
		Vector2 b = Vector2.zero;
		Vector2 v = KInputManager.GetMousePos();
		Vector2 b2 = this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom);
		component.localScale = new Vector3(this.currentZoom, this.currentZoom, 1f);
		b = (this.zoomCenterLock ? (component.InverseTransformPoint(new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2))) * this.currentZoom) : (component.InverseTransformPoint(v) * this.currentZoom)) - b2;
		float d = this.keyboardScrollSpeed;
		if (this.panUp)
		{
			this.keyPanDelta -= Vector2.up * Time.unscaledDeltaTime * d;
		}
		else if (this.panDown)
		{
			this.keyPanDelta += Vector2.up * Time.unscaledDeltaTime * d;
		}
		if (this.panLeft)
		{
			this.keyPanDelta += Vector2.right * Time.unscaledDeltaTime * d;
		}
		else if (this.panRight)
		{
			this.keyPanDelta -= Vector2.right * Time.unscaledDeltaTime * d;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			Vector2 a = KInputManager.steamInputInterpreter.GetSteamCameraMovement();
			a *= -1f;
			this.keyPanDelta = a * Time.unscaledDeltaTime * d * 2f;
		}
		Vector2 b3 = new Vector2(Mathf.Lerp(0f, this.keyPanDelta.x, Time.unscaledDeltaTime * this.keyPanEasing), Mathf.Lerp(0f, this.keyPanDelta.y, Time.unscaledDeltaTime * this.keyPanEasing));
		this.keyPanDelta -= b3;
		Vector2 vector = Vector2.zero;
		if (this.isDragging)
		{
			Vector2 b4 = KInputManager.GetMousePos() - this.dragLastPosition;
			vector += b4;
			this.dragLastPosition = KInputManager.GetMousePos();
			this.dragInteria = Vector2.ClampMagnitude(this.dragInteria + b4, 400f);
		}
		this.dragInteria *= Mathf.Max(0f, 1f - Time.unscaledDeltaTime * 4f);
		Vector2 vector2 = anchoredPosition + b + this.keyPanDelta + vector;
		if (!this.isDragging)
		{
			Vector2 size = base.GetComponent<RectTransform>().rect.size;
			Vector2 vector3 = new Vector2((-component.rect.size.x / 2f - 250f) * this.currentZoom, -250f * this.currentZoom);
			Vector2 vector4 = new Vector2(250f * this.currentZoom, (component.rect.size.y + 250f) * this.currentZoom - size.y);
			Vector2 a2 = new Vector2(Mathf.Clamp(vector2.x, vector3.x, vector4.x), Mathf.Clamp(vector2.y, vector3.y, vector4.y));
			this.forceTargetPosition = new Vector2(Mathf.Clamp(this.forceTargetPosition.x, vector3.x, vector4.x), Mathf.Clamp(this.forceTargetPosition.y, vector3.y, vector4.y));
			Vector2 vector5 = a2 + this.dragInteria - vector2;
			if (!this.panLeft && !this.panRight && !this.panUp && !this.panDown)
			{
				vector2 += vector5 * this.edgeClampFactor * Time.unscaledDeltaTime;
			}
			else
			{
				vector2 += vector5;
				if (vector5.x < 0f)
				{
					this.keyPanDelta.x = Mathf.Min(0f, this.keyPanDelta.x);
				}
				if (vector5.x > 0f)
				{
					this.keyPanDelta.x = Mathf.Max(0f, this.keyPanDelta.x);
				}
				if (vector5.y < 0f)
				{
					this.keyPanDelta.y = Mathf.Min(0f, this.keyPanDelta.y);
				}
				if (vector5.y > 0f)
				{
					this.keyPanDelta.y = Mathf.Max(0f, this.keyPanDelta.y);
				}
			}
		}
		if (this.zoomingToTarget)
		{
			vector2 = Vector2.Lerp(vector2, this.forceTargetPosition, Time.unscaledDeltaTime * 4f);
			if (Vector3.Distance(vector2, this.forceTargetPosition) < 1f || this.isDragging || this.panLeft || this.panRight || this.panUp || this.panDown)
			{
				this.zoomingToTarget = false;
			}
		}
		component.anchoredPosition = vector2;
	}

	// Token: 0x06005F30 RID: 24368 RVA: 0x0022FEFC File Offset: 0x0022E0FC
	protected override void OnSpawn()
	{
		base.Subscribe(Research.Instance.gameObject, -1914338957, new Action<object>(this.OnActiveResearchChanged));
		base.Subscribe(Game.Instance.gameObject, -107300940, new Action<object>(this.OnResearchComplete));
		base.Subscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Show(false);
		});
		this.pointDisplayMap = new Dictionary<string, LocText>();
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id] = Util.KInstantiateUI(this.pointDisplayCountPrefab, this.pointDisplayContainer, true).GetComponentInChildren<LocText>();
			this.pointDisplayMap[researchType.id].text = Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString();
			this.pointDisplayMap[researchType.id].transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(researchType.description);
			this.pointDisplayMap[researchType.id].transform.parent.GetComponentInChildren<Image>().sprite = researchType.sprite;
		}
		this.pointDisplayContainer.transform.parent.gameObject.SetActive(Research.Instance.UseGlobalPointInventory);
		this.entryMap = new Dictionary<Tech, ResearchEntry>();
		List<Tech> resources = Db.Get().Techs.resources;
		resources.Sort((Tech x, Tech y) => y.center.y.CompareTo(x.center.y));
		List<TechTreeTitle> resources2 = Db.Get().TechTreeTitles.resources;
		resources2.Sort((TechTreeTitle x, TechTreeTitle y) => y.center.y.CompareTo(x.center.y));
		float x3 = 0f;
		float y3 = 125f;
		Vector2 b = new Vector2(x3, y3);
		for (int i = 0; i < resources2.Count; i++)
		{
			ResearchTreeTitle researchTreeTitle = Util.KInstantiateUI<ResearchTreeTitle>(this.researchTreeTitlePrefab.gameObject, this.treeTitles, false);
			TechTreeTitle techTreeTitle = resources2[i];
			researchTreeTitle.name = techTreeTitle.Name + " Title";
			Vector3 vector = techTreeTitle.center + b;
			researchTreeTitle.transform.rectTransform().anchoredPosition = vector;
			float num = techTreeTitle.height;
			if (i + 1 < resources2.Count)
			{
				TechTreeTitle techTreeTitle2 = resources2[i + 1];
				Vector3 vector2 = techTreeTitle2.center + b;
				num += vector.y - (vector2.y + techTreeTitle2.height);
			}
			else
			{
				num += 600f;
			}
			researchTreeTitle.transform.rectTransform().sizeDelta = new Vector2(techTreeTitle.width, num);
			researchTreeTitle.SetLabel(techTreeTitle.Name);
			researchTreeTitle.SetColor(i);
		}
		List<Vector2> list = new List<Vector2>();
		float x2 = 0f;
		float y2 = 0f;
		Vector2 b2 = new Vector2(x2, y2);
		for (int j = 0; j < resources.Count; j++)
		{
			ResearchEntry researchEntry = Util.KInstantiateUI<ResearchEntry>(this.entryPrefab.gameObject, this.scrollContent, false);
			Tech tech = resources[j];
			researchEntry.name = tech.Name + " Panel";
			Vector3 v = tech.center + b2;
			researchEntry.transform.rectTransform().anchoredPosition = v;
			researchEntry.transform.rectTransform().sizeDelta = new Vector2(tech.width, tech.height);
			this.entryMap.Add(tech, researchEntry);
			if (tech.edges.Count > 0)
			{
				for (int k = 0; k < tech.edges.Count; k++)
				{
					ResourceTreeNode.Edge edge = tech.edges[k];
					if (edge.path == null)
					{
						list.AddRange(edge.SrcTarget);
					}
					else
					{
						ResourceTreeNode.Edge.EdgeType edgeType = edge.edgeType;
						if (edgeType <= ResourceTreeNode.Edge.EdgeType.QuadCurveEdge || edgeType - ResourceTreeNode.Edge.EdgeType.BezierEdge <= 1)
						{
							list.Add(edge.SrcTarget[0]);
							list.Add(edge.path[0]);
							for (int l = 1; l < edge.path.Count; l++)
							{
								list.Add(edge.path[l - 1]);
								list.Add(edge.path[l]);
							}
							list.Add(edge.path[edge.path.Count - 1]);
							list.Add(edge.SrcTarget[1]);
						}
						else
						{
							list.AddRange(edge.path);
						}
					}
				}
			}
		}
		for (int m = 0; m < list.Count; m++)
		{
			list[m] = new Vector2(list[m].x, list[m].y + this.foreground.transform.rectTransform().rect.height);
		}
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetTech(keyValuePair.Key);
		}
		this.CloseButton.soundPlayer.Enabled = false;
		this.CloseButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		base.StartCoroutine(this.WaitAndSetActiveResearch());
		base.OnSpawn();
		this.scrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(250f, -250f);
		this.zoomOutButton.onClick += delegate()
		{
			this.ZoomOut();
		};
		this.zoomInButton.onClick += delegate()
		{
			this.ZoomIn();
		};
		base.gameObject.SetActive(true);
		this.Show(false);
	}

	// Token: 0x06005F31 RID: 24369 RVA: 0x002305A4 File Offset: 0x0022E7A4
	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		this.isDragging = true;
	}

	// Token: 0x06005F32 RID: 24370 RVA: 0x002305B4 File Offset: 0x0022E7B4
	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		this.AbortDragging();
	}

	// Token: 0x06005F33 RID: 24371 RVA: 0x002305C3 File Offset: 0x0022E7C3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(Game.Instance.gameObject, -1974454597, delegate(object o)
		{
			this.Deactivate();
		});
	}

	// Token: 0x06005F34 RID: 24372 RVA: 0x002305EC File Offset: 0x0022E7EC
	private IEnumerator WaitAndSetActiveResearch()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		TechInstance targetResearch = Research.Instance.GetTargetResearch();
		if (targetResearch != null)
		{
			this.SetActiveResearch(targetResearch.tech);
		}
		yield break;
	}

	// Token: 0x06005F35 RID: 24373 RVA: 0x002305FB File Offset: 0x0022E7FB
	public Vector3 GetEntryPosition(Tech tech)
	{
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return Vector3.zero;
		}
		return this.entryMap[tech].transform.GetPosition();
	}

	// Token: 0x06005F36 RID: 24374 RVA: 0x00230631 File Offset: 0x0022E831
	public ResearchEntry GetEntry(Tech tech)
	{
		if (this.entryMap == null)
		{
			return null;
		}
		if (!this.entryMap.ContainsKey(tech))
		{
			global::Debug.LogError("The Tech provided was not present in the dictionary");
			return null;
		}
		return this.entryMap[tech];
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x00230664 File Offset: 0x0022E864
	public void SetEntryPercentage(Tech tech, float percent)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.SetPercentage(percent);
		}
	}

	// Token: 0x06005F38 RID: 24376 RVA: 0x0023068C File Offset: 0x0022E88C
	public void TurnEverythingOff()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOff();
		}
	}

	// Token: 0x06005F39 RID: 24377 RVA: 0x002306E4 File Offset: 0x0022E8E4
	public void TurnEverythingOn()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.SetEverythingOn();
		}
	}

	// Token: 0x06005F3A RID: 24378 RVA: 0x0023073C File Offset: 0x0022E93C
	private void SelectAllEntries(Tech tech, bool isSelected)
	{
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.QueueStateChanged(isSelected);
		}
		foreach (Tech tech2 in tech.requiredTech)
		{
			this.SelectAllEntries(tech2, isSelected);
		}
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x002307A8 File Offset: 0x0022E9A8
	private void OnResearchComplete(object data)
	{
		Tech tech = (Tech)data;
		ResearchEntry entry = this.GetEntry(tech);
		if (entry != null)
		{
			entry.ResearchCompleted(true);
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x002307E0 File Offset: 0x0022E9E0
	private void UpdatePointDisplay()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.pointDisplayMap[researchType.id].text = string.Format("{0}: {1}", Research.Instance.researchTypes.GetResearchType(researchType.id).name, Research.Instance.globalPointInventory.PointsByTypeID[researchType.id].ToString());
		}
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x00230894 File Offset: 0x0022EA94
	private void OnActiveResearchChanged(object data)
	{
		List<TechInstance> list = (List<TechInstance>)data;
		foreach (TechInstance techInstance in list)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(true);
			}
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
		if (list.Count > 0)
		{
			this.currentResearch = list[list.Count - 1].tech;
		}
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x00230930 File Offset: 0x0022EB30
	private void UpdateProgressBars()
	{
		foreach (KeyValuePair<Tech, ResearchEntry> keyValuePair in this.entryMap)
		{
			keyValuePair.Value.UpdateProgressBars();
		}
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x00230988 File Offset: 0x0022EB88
	public void CancelResearch()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		foreach (TechInstance techInstance in researchQueue)
		{
			ResearchEntry entry = this.GetEntry(techInstance.tech);
			if (entry != null)
			{
				entry.QueueStateChanged(false);
			}
		}
		researchQueue.Clear();
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x00230A00 File Offset: 0x0022EC00
	private void SetActiveResearch(Tech newResearch)
	{
		if (newResearch != this.currentResearch && this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, false);
		}
		this.currentResearch = newResearch;
		if (this.currentResearch != null)
		{
			this.SelectAllEntries(this.currentResearch, true);
		}
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x00230A3C File Offset: 0x0022EC3C
	public override void Show(bool show = true)
	{
		this.mouseOver = false;
		this.scrollContentChildFitter.enabled = show;
		foreach (Canvas canvas in base.GetComponentsInChildren<Canvas>(true))
		{
			if (canvas.enabled != show)
			{
				canvas.enabled = show;
			}
		}
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null)
		{
			component.interactable = show;
			component.blocksRaycasts = show;
			component.ignoreParentGroups = true;
		}
		this.OnShow(show);
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x00230AB4 File Offset: 0x0022ECB4
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			if (DetailsScreen.Instance != null)
			{
				DetailsScreen.Instance.gameObject.SetActive(false);
			}
		}
		else if (SelectTool.Instance.selected != null && !DetailsScreen.Instance.gameObject.activeSelf)
		{
			DetailsScreen.Instance.gameObject.SetActive(true);
			DetailsScreen.Instance.Refresh(SelectTool.Instance.selected.gameObject);
		}
		this.UpdateProgressBars();
		this.UpdatePointDisplay();
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x00230B42 File Offset: 0x0022ED42
	private void AbortDragging()
	{
		this.isDragging = false;
		this.draggingJustEnded = true;
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x00230B52 File Offset: 0x0022ED52
	private void LateUpdate()
	{
		this.draggingJustEnded = false;
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x00230B5C File Offset: 0x0022ED5C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.MouseRight) && !this.isDragging && !this.draggingJustEnded)
			{
				ManagementMenu.Instance.CloseAll();
			}
			if (e.IsAction(global::Action.MouseRight) || e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.MouseMiddle))
			{
				this.AbortDragging();
			}
			if (this.panUp && e.TryConsume(global::Action.PanUp))
			{
				this.panUp = false;
				return;
			}
			if (this.panDown && e.TryConsume(global::Action.PanDown))
			{
				this.panDown = false;
				return;
			}
			if (this.panRight && e.TryConsume(global::Action.PanRight))
			{
				this.panRight = false;
				return;
			}
			if (this.panLeft && e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = false;
				return;
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x00230C44 File Offset: 0x0022EE44
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!base.canvas.enabled)
		{
			return;
		}
		if (!e.Consumed)
		{
			if (e.TryConsume(global::Action.MouseRight))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (e.TryConsume(global::Action.MouseLeft))
			{
				this.dragStartPosition = KInputManager.GetMousePos();
				this.dragLastPosition = KInputManager.GetMousePos();
				return;
			}
			if (KInputManager.GetMousePos().x > this.sideBar.rectTransform().sizeDelta.x)
			{
				if (e.TryConsume(global::Action.ZoomIn))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom + this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
				if (e.TryConsume(global::Action.ZoomOut))
				{
					this.targetZoom = Mathf.Clamp(this.targetZoom - this.zoomAmountPerScroll, this.minZoom, this.maxZoom);
					this.zoomCenterLock = false;
					return;
				}
			}
			if (e.TryConsume(global::Action.Escape))
			{
				ManagementMenu.Instance.CloseAll();
				return;
			}
			if (e.TryConsume(global::Action.PanLeft))
			{
				this.panLeft = true;
				return;
			}
			if (e.TryConsume(global::Action.PanRight))
			{
				this.panRight = true;
				return;
			}
			if (e.TryConsume(global::Action.PanUp))
			{
				this.panUp = true;
				return;
			}
			if (e.TryConsume(global::Action.PanDown))
			{
				this.panDown = true;
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x00230DA4 File Offset: 0x0022EFA4
	public static bool TechPassesSearchFilter(string techID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			bool flag = false;
			Tech tech = Db.Get().Techs.Get(techID);
			flag = UI.StripLinkFormatting(tech.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = tech.category.ToUpper().Contains(filterString);
				foreach (TechItem techItem in tech.unlockedItems)
				{
					if (UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString))
					{
						flag = true;
						break;
					}
					if (UI.StripLinkFormatting(techItem.description).ToLower().ToUpper().Contains(filterString))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}
		return true;
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x00230E90 File Offset: 0x0022F090
	public static bool TechItemPassesSearchFilter(string techItemID, string filterString)
	{
		if (!string.IsNullOrEmpty(filterString))
		{
			filterString = filterString.ToUpper();
			TechItem techItem = Db.Get().TechItems.Get(techItemID);
			bool flag = UI.StripLinkFormatting(techItem.Name).ToLower().ToUpper().Contains(filterString);
			if (!flag)
			{
				flag = techItem.Name.ToUpper().Contains(filterString);
				flag = (flag && techItem.description.ToUpper().Contains(filterString));
			}
			return flag;
		}
		return true;
	}

	// Token: 0x04004073 RID: 16499
	private const float SCROLL_BUFFER = 250f;

	// Token: 0x04004074 RID: 16500
	[SerializeField]
	private Image BG;

	// Token: 0x04004075 RID: 16501
	public ResearchEntry entryPrefab;

	// Token: 0x04004076 RID: 16502
	public ResearchTreeTitle researchTreeTitlePrefab;

	// Token: 0x04004077 RID: 16503
	public GameObject foreground;

	// Token: 0x04004078 RID: 16504
	public GameObject scrollContent;

	// Token: 0x04004079 RID: 16505
	public GameObject treeTitles;

	// Token: 0x0400407A RID: 16506
	public GameObject pointDisplayCountPrefab;

	// Token: 0x0400407B RID: 16507
	public GameObject pointDisplayContainer;

	// Token: 0x0400407C RID: 16508
	private Dictionary<string, LocText> pointDisplayMap;

	// Token: 0x0400407D RID: 16509
	private Dictionary<Tech, ResearchEntry> entryMap;

	// Token: 0x0400407E RID: 16510
	[SerializeField]
	private KButton zoomOutButton;

	// Token: 0x0400407F RID: 16511
	[SerializeField]
	private KButton zoomInButton;

	// Token: 0x04004080 RID: 16512
	[SerializeField]
	private ResearchScreenSideBar sideBar;

	// Token: 0x04004081 RID: 16513
	private Tech currentResearch;

	// Token: 0x04004082 RID: 16514
	public KButton CloseButton;

	// Token: 0x04004083 RID: 16515
	private GraphicRaycaster m_Raycaster;

	// Token: 0x04004084 RID: 16516
	private PointerEventData m_PointerEventData;

	// Token: 0x04004085 RID: 16517
	private Vector3 currentScrollPosition;

	// Token: 0x04004086 RID: 16518
	private bool panUp;

	// Token: 0x04004087 RID: 16519
	private bool panDown;

	// Token: 0x04004088 RID: 16520
	private bool panLeft;

	// Token: 0x04004089 RID: 16521
	private bool panRight;

	// Token: 0x0400408A RID: 16522
	[SerializeField]
	private KChildFitter scrollContentChildFitter;

	// Token: 0x0400408B RID: 16523
	private bool isDragging;

	// Token: 0x0400408C RID: 16524
	private Vector3 dragStartPosition;

	// Token: 0x0400408D RID: 16525
	private Vector3 dragLastPosition;

	// Token: 0x0400408E RID: 16526
	private Vector2 dragInteria;

	// Token: 0x0400408F RID: 16527
	private Vector2 forceTargetPosition;

	// Token: 0x04004090 RID: 16528
	private bool zoomingToTarget;

	// Token: 0x04004091 RID: 16529
	private bool draggingJustEnded;

	// Token: 0x04004092 RID: 16530
	private float targetZoom = 1f;

	// Token: 0x04004093 RID: 16531
	private float currentZoom = 1f;

	// Token: 0x04004094 RID: 16532
	private bool zoomCenterLock;

	// Token: 0x04004095 RID: 16533
	private Vector2 keyPanDelta = Vector3.zero;

	// Token: 0x04004096 RID: 16534
	[SerializeField]
	private float effectiveZoomSpeed = 5f;

	// Token: 0x04004097 RID: 16535
	[SerializeField]
	private float zoomAmountPerScroll = 0.05f;

	// Token: 0x04004098 RID: 16536
	[SerializeField]
	private float zoomAmountPerButton = 0.5f;

	// Token: 0x04004099 RID: 16537
	[SerializeField]
	private float minZoom = 0.15f;

	// Token: 0x0400409A RID: 16538
	[SerializeField]
	private float maxZoom = 1f;

	// Token: 0x0400409B RID: 16539
	[SerializeField]
	private float keyboardScrollSpeed = 200f;

	// Token: 0x0400409C RID: 16540
	[SerializeField]
	private float keyPanEasing = 1f;

	// Token: 0x0400409D RID: 16541
	[SerializeField]
	private float edgeClampFactor = 0.5f;

	// Token: 0x02001B16 RID: 6934
	public enum ResearchState
	{
		// Token: 0x04007BA9 RID: 31657
		Available,
		// Token: 0x04007BAA RID: 31658
		ActiveResearch,
		// Token: 0x04007BAB RID: 31659
		ResearchComplete,
		// Token: 0x04007BAC RID: 31660
		MissingPrerequisites,
		// Token: 0x04007BAD RID: 31661
		StateCount
	}
}
