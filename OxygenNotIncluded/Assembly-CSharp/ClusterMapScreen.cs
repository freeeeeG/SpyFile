using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000AB8 RID: 2744
public class ClusterMapScreen : KScreen
{
	// Token: 0x060053D8 RID: 21464 RVA: 0x001E33C2 File Offset: 0x001E15C2
	public static void DestroyInstance()
	{
		ClusterMapScreen.Instance = null;
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001E33CA File Offset: 0x001E15CA
	public ClusterMapVisualizer GetEntityVisAnim(ClusterGridEntity entity)
	{
		if (this.m_gridEntityAnims.ContainsKey(entity))
		{
			return this.m_gridEntityAnims[entity];
		}
		return null;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001E33E8 File Offset: 0x001E15E8
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 20f;
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001E33FD File Offset: 0x001E15FD
	public float CurrentZoomPercentage()
	{
		return (this.m_currentZoomScale - 50f) / 100f;
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x001E3411 File Offset: 0x001E1611
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.m_selectMarker = global::Util.KInstantiateUI<SelectMarker>(this.selectMarkerPrefab, base.gameObject, false);
		this.m_selectMarker.gameObject.SetActive(false);
		ClusterMapScreen.Instance = this;
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001E3448 File Offset: 0x001E1648
	protected override void OnSpawn()
	{
		base.OnSpawn();
		global::Debug.Assert(this.cellVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the cellVisPrefab hex must be 1");
		global::Debug.Assert(this.terrainVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the terrainVisPrefab hex must be 1");
		global::Debug.Assert(this.mobileVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the mobileVisPrefab hex must be 1");
		global::Debug.Assert(this.staticVisPrefab.rectTransform().sizeDelta == new Vector2(2f, 2f), "The radius of the staticVisPrefab hex must be 1");
		int num;
		int num2;
		int num3;
		int num4;
		this.GenerateGridVis(out num, out num2, out num3, out num4);
		this.Show(false);
		this.mapScrollRect.content.sizeDelta = new Vector2((float)(num2 * 4), (float)(num4 * 4));
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		this.m_onDestinationChangedDelegate = new Action<object>(this.OnDestinationChanged);
		this.m_onSelectObjectDelegate = new Action<object>(this.OnSelectObject);
		base.Subscribe(1980521255, new Action<object>(this.UpdateVis));
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x001E35A8 File Offset: 0x001E17A8
	protected void MoveToNISPosition()
	{
		if (!this.movingToTargetNISPosition)
		{
			return;
		}
		Vector3 b = new Vector3(-this.targetNISPosition.x * this.mapScrollRect.content.localScale.x, -this.targetNISPosition.y * this.mapScrollRect.content.localScale.y, this.targetNISPosition.z);
		this.m_targetZoomScale = Mathf.Lerp(this.m_targetZoomScale, this.targetNISZoom, Time.unscaledDeltaTime * 2f);
		this.mapScrollRect.content.SetLocalPosition(Vector3.Lerp(this.mapScrollRect.content.GetLocalPosition(), b, Time.unscaledDeltaTime * 2.5f));
		float num = Vector3.Distance(this.mapScrollRect.content.GetLocalPosition(), b);
		if (num < 100f)
		{
			ClusterMapHex component = this.m_cellVisByLocation[this.selectOnMoveNISComplete].GetComponent<ClusterMapHex>();
			if (this.m_selectedHex != component)
			{
				this.SelectHex(component);
			}
			if (num < 10f)
			{
				this.movingToTargetNISPosition = false;
			}
		}
	}

	// Token: 0x060053DF RID: 21471 RVA: 0x001E36C2 File Offset: 0x001E18C2
	public void SetTargetFocusPosition(AxialI targetPosition, float delayBeforeMove = 0.5f)
	{
		if (this.activeMoveToTargetRoutine != null)
		{
			base.StopCoroutine(this.activeMoveToTargetRoutine);
		}
		this.activeMoveToTargetRoutine = base.StartCoroutine(this.MoveToTargetRoutine(targetPosition, delayBeforeMove));
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x001E36EC File Offset: 0x001E18EC
	private IEnumerator MoveToTargetRoutine(AxialI targetPosition, float delayBeforeMove)
	{
		delayBeforeMove = Mathf.Max(delayBeforeMove, 0f);
		yield return SequenceUtil.WaitForSecondsRealtime(delayBeforeMove);
		this.targetNISPosition = AxialUtil.AxialToWorld((float)targetPosition.r, (float)targetPosition.q);
		this.targetNISZoom = 150f;
		this.movingToTargetNISPosition = true;
		this.selectOnMoveNISComplete = targetPosition;
		yield break;
	}

	// Token: 0x060053E1 RID: 21473 RVA: 0x001E370C File Offset: 0x001E190C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && (e.IsAction(global::Action.ZoomIn) || e.IsAction(global::Action.ZoomOut)))
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
			pointerEventData.position = KInputManager.GetMousePos();
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current != null)
			{
				current.RaycastAll(pointerEventData, list);
				bool flag = false;
				foreach (RaycastResult raycastResult in list)
				{
					if (!raycastResult.gameObject.transform.IsChildOf(base.transform))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					float num;
					if (KInputManager.currentControllerIsGamepad)
					{
						num = 25f;
						num *= (float)(e.IsAction(global::Action.ZoomIn) ? 1 : -1);
					}
					else
					{
						num = Input.mouseScrollDelta.y * 25f;
					}
					this.m_targetZoomScale = Mathf.Clamp(this.m_targetZoomScale + num, 50f, 150f);
					e.TryConsume(global::Action.ZoomIn);
					if (!e.Consumed)
					{
						e.TryConsume(global::Action.ZoomOut);
					}
				}
			}
		}
		CameraController.Instance.ChangeWorldInput(e);
		base.OnKeyDown(e);
	}

	// Token: 0x060053E2 RID: 21474 RVA: 0x001E3858 File Offset: 0x001E1A58
	public bool TryHandleCancel()
	{
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination && !this.m_closeOnSelect)
		{
			this.SetMode(ClusterMapScreen.Mode.Default);
			return true;
		}
		return false;
	}

	// Token: 0x060053E3 RID: 21475 RVA: 0x001E3878 File Offset: 0x001E1A78
	public void ShowInSelectDestinationMode(ClusterDestinationSelector destination_selector)
	{
		this.m_destinationSelector = destination_selector;
		if (!base.gameObject.activeSelf)
		{
			ManagementMenu.Instance.ToggleClusterMap();
			this.m_closeOnSelect = true;
		}
		ClusterGridEntity component = destination_selector.GetComponent<ClusterGridEntity>();
		this.SetSelectedEntity(component, false);
		if (this.m_selectedEntity != null)
		{
			this.m_selectedHex = this.m_cellVisByLocation[this.m_selectedEntity.Location].GetComponent<ClusterMapHex>();
		}
		else
		{
			AxialI myWorldLocation = destination_selector.GetMyWorldLocation();
			ClusterMapHex component2 = this.m_cellVisByLocation[myWorldLocation].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component2;
		}
		this.SetMode(ClusterMapScreen.Mode.SelectDestination);
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x001E3911 File Offset: 0x001E1B11
	private void SetMode(ClusterMapScreen.Mode mode)
	{
		this.m_mode = mode;
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			this.m_destinationSelector = null;
		}
		this.UpdateVis(null);
	}

	// Token: 0x060053E5 RID: 21477 RVA: 0x001E3930 File Offset: 0x001E1B30
	public ClusterMapScreen.Mode GetMode()
	{
		return this.m_mode;
	}

	// Token: 0x060053E6 RID: 21478 RVA: 0x001E3938 File Offset: 0x001E1B38
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.MoveToNISPosition();
			this.UpdateVis(null);
			if (this.m_mode == ClusterMapScreen.Mode.Default)
			{
				this.TrySelectDefault();
			}
			Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
			Game.Instance.Subscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
			Game.Instance.Subscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
			ClusterMapSelectTool.Instance.Activate();
			this.SetShowingNonClusterMapHud(false);
			CameraController.Instance.DisableUserCameraControl = true;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot);
			MusicManager.instance.PlaySong("Music_Starmap", false);
			this.UpdateTearStatus();
			return;
		}
		Game.Instance.Unsubscribe(-1554423969, new Action<object>(this.OnNewTelescopeTarget));
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnFogOfWarRevealed));
		Game.Instance.Unsubscribe(-1298331547, new Action<object>(this.OnClusterLocationChanged));
		this.m_mode = ClusterMapScreen.Mode.Default;
		this.m_closeOnSelect = false;
		this.m_destinationSelector = null;
		SelectTool.Instance.Activate();
		this.SetShowingNonClusterMapHud(true);
		CameraController.Instance.DisableUserCameraControl = false;
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUStarmapNotPausedSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_Starmap"))
		{
			MusicManager.instance.StopSong("Music_Starmap", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060053E7 RID: 21479 RVA: 0x001E3AC3 File Offset: 0x001E1CC3
	private void SetShowingNonClusterMapHud(bool show)
	{
		PlanScreen.Instance.gameObject.SetActive(show);
		ToolMenu.Instance.gameObject.SetActive(show);
		OverlayScreen.Instance.gameObject.SetActive(show);
	}

	// Token: 0x060053E8 RID: 21480 RVA: 0x001E3AF8 File Offset: 0x001E1CF8
	private void SetSelectedEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Unsubscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Unsubscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		this.m_selectedEntity = entity;
		if (this.m_selectedEntity != null)
		{
			this.m_selectedEntity.Subscribe(543433792, this.m_onDestinationChangedDelegate);
			this.m_selectedEntity.Subscribe(-1503271301, this.m_onSelectObjectDelegate);
		}
		KSelectable new_selected = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<KSelectable>() : null;
		if (frameDelay)
		{
			ClusterMapSelectTool.Instance.SelectNextFrame(new_selected, false);
			return;
		}
		ClusterMapSelectTool.Instance.Select(new_selected, false);
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x001E3BBB File Offset: 0x001E1DBB
	private void OnDestinationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060053EA RID: 21482 RVA: 0x001E3BC4 File Offset: 0x001E1DC4
	private void OnSelectObject(object data)
	{
		if (this.m_selectedEntity == null)
		{
			return;
		}
		KSelectable component = this.m_selectedEntity.GetComponent<KSelectable>();
		if (component == null || component.IsSelected)
		{
			return;
		}
		this.SetSelectedEntity(null, false);
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			if (this.m_closeOnSelect)
			{
				ManagementMenu.Instance.CloseAll();
			}
			else
			{
				this.SetMode(ClusterMapScreen.Mode.Default);
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001E3C31 File Offset: 0x001E1E31
	private void OnFogOfWarRevealed(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060053EC RID: 21484 RVA: 0x001E3C3A File Offset: 0x001E1E3A
	private void OnNewTelescopeTarget(object data = null)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001E3C43 File Offset: 0x001E1E43
	private void Update()
	{
		if (KInputManager.currentControllerIsGamepad)
		{
			this.mapScrollRect.AnalogUpdate(KInputManager.steamInputInterpreter.GetSteamCameraMovement() * this.scrollSpeed);
		}
	}

	// Token: 0x060053EE RID: 21486 RVA: 0x001E3C6C File Offset: 0x001E1E6C
	private void TrySelectDefault()
	{
		if (this.m_selectedHex != null && this.m_selectedEntity != null)
		{
			this.UpdateVis(null);
			return;
		}
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		if (activeWorld == null)
		{
			return;
		}
		ClusterGridEntity component = activeWorld.GetComponent<ClusterGridEntity>();
		if (component == null)
		{
			return;
		}
		this.SelectEntity(component, false);
	}

	// Token: 0x060053EF RID: 21487 RVA: 0x001E3CCC File Offset: 0x001E1ECC
	private void GenerateGridVis(out int minR, out int maxR, out int minQ, out int maxQ)
	{
		minR = int.MaxValue;
		maxR = int.MinValue;
		minQ = int.MaxValue;
		maxQ = int.MinValue;
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(this.cellVisPrefab, Vector3.zero, Quaternion.identity, this.cellVisContainer.transform);
			clusterMapVisualizer.rectTransform().SetLocalPosition(keyValuePair.Key.ToWorld());
			clusterMapVisualizer.gameObject.SetActive(true);
			ClusterMapHex component = clusterMapVisualizer.GetComponent<ClusterMapHex>();
			component.SetLocation(keyValuePair.Key);
			this.m_cellVisByLocation.Add(keyValuePair.Key, clusterMapVisualizer);
			minR = Mathf.Min(minR, component.location.R);
			maxR = Mathf.Max(maxR, component.location.R);
			minQ = Mathf.Min(minQ, component.location.Q);
			maxQ = Mathf.Max(maxQ, component.location.Q);
		}
		this.SetupVisGameObjects();
		this.UpdateVis(null);
	}

	// Token: 0x060053F0 RID: 21488 RVA: 0x001E3E20 File Offset: 0x001E2020
	public Transform GetGridEntityNameTarget(ClusterGridEntity entity)
	{
		ClusterMapVisualizer clusterMapVisualizer;
		if (this.m_currentZoomScale >= 115f && this.m_gridEntityVis.TryGetValue(entity, out clusterMapVisualizer))
		{
			return clusterMapVisualizer.nameTarget;
		}
		return null;
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x001E3E54 File Offset: 0x001E2054
	public override void ScreenUpdate(bool topLevel)
	{
		float t = Mathf.Min(4f * Time.unscaledDeltaTime, 0.9f);
		this.m_currentZoomScale = Mathf.Lerp(this.m_currentZoomScale, this.m_targetZoomScale, t);
		Vector2 v = KInputManager.GetMousePos();
		Vector3 b = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localScale = new Vector3(this.m_currentZoomScale, this.m_currentZoomScale, 1f);
		Vector3 a = this.mapScrollRect.content.InverseTransformPoint(v);
		this.mapScrollRect.content.localPosition += (a - b) * this.m_currentZoomScale;
		this.MoveToNISPosition();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001E3F28 File Offset: 0x001E2128
	private void FloatyAsteroidAnimation()
	{
		float num = 0f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			AsteroidGridEntity component = worldContainer.GetComponent<AsteroidGridEntity>();
			if (component != null && this.m_gridEntityVis.ContainsKey(component) && ClusterMapScreen.GetRevealLevel(component) == ClusterRevealLevel.Visible)
			{
				KAnimControllerBase firstAnimController = this.m_gridEntityVis[component].GetFirstAnimController();
				float y = this.floatCycleOffset + this.floatCycleScale * Mathf.Sin(this.floatCycleSpeed * (num + GameClock.Instance.GetTime()));
				firstAnimController.Offset = new Vector2(0f, y);
			}
			num += 1f;
		}
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x001E3FF8 File Offset: 0x001E21F8
	private void SetupVisGameObjects()
	{
		foreach (KeyValuePair<AxialI, List<ClusterGridEntity>> keyValuePair in ClusterGrid.Instance.cellContents)
		{
			foreach (ClusterGridEntity clusterGridEntity in keyValuePair.Value)
			{
				ClusterGrid.Instance.GetCellRevealLevel(keyValuePair.Key);
				ClusterRevealLevel isVisibleInFOW = clusterGridEntity.IsVisibleInFOW;
				ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(clusterGridEntity);
				if (clusterGridEntity.IsVisible && revealLevel != ClusterRevealLevel.Hidden && !this.m_gridEntityVis.ContainsKey(clusterGridEntity))
				{
					ClusterMapVisualizer original = null;
					GameObject gameObject = null;
					switch (clusterGridEntity.Layer)
					{
					case EntityLayer.Asteroid:
						original = this.terrainVisPrefab;
						gameObject = this.terrainVisContainer;
						break;
					case EntityLayer.Craft:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.POI:
						original = this.staticVisPrefab;
						gameObject = this.POIVisContainer;
						break;
					case EntityLayer.Telescope:
						original = this.staticVisPrefab;
						gameObject = this.telescopeVisContainer;
						break;
					case EntityLayer.Payload:
						original = this.mobileVisPrefab;
						gameObject = this.mobileVisContainer;
						break;
					case EntityLayer.FX:
						original = this.staticVisPrefab;
						gameObject = this.FXVisContainer;
						break;
					}
					ClusterNameDisplayScreen.Instance.AddNewEntry(clusterGridEntity);
					ClusterMapVisualizer clusterMapVisualizer = UnityEngine.Object.Instantiate<ClusterMapVisualizer>(original, gameObject.transform);
					clusterMapVisualizer.Init(clusterGridEntity, this.pathDrawer);
					clusterMapVisualizer.gameObject.SetActive(true);
					this.m_gridEntityAnims.Add(clusterGridEntity, clusterMapVisualizer);
					this.m_gridEntityVis.Add(clusterGridEntity, clusterMapVisualizer);
					clusterGridEntity.positionDirty = false;
					clusterGridEntity.Subscribe(1502190696, new Action<object>(this.RemoveDeletedEntities));
				}
			}
		}
		this.RemoveDeletedEntities(null);
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair2 in this.m_gridEntityVis)
		{
			ClusterGridEntity key = keyValuePair2.Key;
			if (key.Layer == EntityLayer.Asteroid)
			{
				int id = key.GetComponent<WorldContainer>().id;
				keyValuePair2.Value.alertVignette.worldID = id;
			}
		}
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x001E427C File Offset: 0x001E247C
	private void RemoveDeletedEntities(object obj = null)
	{
		foreach (ClusterGridEntity key in (from x in this.m_gridEntityVis.Keys
		where x == null || x.gameObject == (GameObject)obj
		select x).ToList<ClusterGridEntity>())
		{
			global::Util.KDestroyGameObject(this.m_gridEntityVis[key]);
			this.m_gridEntityVis.Remove(key);
			this.m_gridEntityAnims.Remove(key);
		}
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001E431C File Offset: 0x001E251C
	private void OnClusterLocationChanged(object data)
	{
		this.UpdateVis(null);
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001E4328 File Offset: 0x001E2528
	public static ClusterRevealLevel GetRevealLevel(ClusterGridEntity entity)
	{
		ClusterRevealLevel cellRevealLevel = ClusterGrid.Instance.GetCellRevealLevel(entity.Location);
		ClusterRevealLevel isVisibleInFOW = entity.IsVisibleInFOW;
		if (cellRevealLevel == ClusterRevealLevel.Visible || isVisibleInFOW == ClusterRevealLevel.Visible)
		{
			return ClusterRevealLevel.Visible;
		}
		if (cellRevealLevel == ClusterRevealLevel.Peeked && isVisibleInFOW == ClusterRevealLevel.Peeked)
		{
			return ClusterRevealLevel.Peeked;
		}
		return ClusterRevealLevel.Hidden;
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001E4364 File Offset: 0x001E2564
	private void UpdateVis(object data = null)
	{
		this.SetupVisGameObjects();
		this.UpdatePaths();
		foreach (KeyValuePair<ClusterGridEntity, ClusterMapVisualizer> keyValuePair in this.m_gridEntityAnims)
		{
			ClusterRevealLevel revealLevel = ClusterMapScreen.GetRevealLevel(keyValuePair.Key);
			keyValuePair.Value.Show(revealLevel);
			bool selected = this.m_selectedEntity == keyValuePair.Key;
			keyValuePair.Value.Select(selected);
			if (keyValuePair.Key.positionDirty)
			{
				Vector3 position = ClusterGrid.Instance.GetPosition(keyValuePair.Key);
				keyValuePair.Value.rectTransform().SetLocalPosition(position);
				keyValuePair.Key.positionDirty = false;
			}
		}
		if (this.m_selectedEntity != null && this.m_gridEntityVis.ContainsKey(this.m_selectedEntity))
		{
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.m_selectedEntity];
			this.m_selectMarker.SetTargetTransform(clusterMapVisualizer.transform);
			this.m_selectMarker.gameObject.SetActive(true);
			clusterMapVisualizer.transform.SetAsLastSibling();
		}
		else
		{
			this.m_selectMarker.gameObject.SetActive(false);
		}
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair2 in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair2.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair2.Key;
			component.SetRevealed(ClusterGrid.Instance.GetCellRevealLevel(key));
		}
		this.UpdateHexToggleStates();
		this.FloatyAsteroidAnimation();
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x001E4524 File Offset: 0x001E2724
	private void OnEntityDestroyed(object obj)
	{
		this.RemoveDeletedEntities(null);
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001E4530 File Offset: 0x001E2730
	private void UpdateHexToggleStates()
	{
		bool flag = this.m_hoveredHex != null && ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_hoveredHex.location, EntityLayer.Asteroid);
		foreach (KeyValuePair<AxialI, ClusterMapVisualizer> keyValuePair in this.m_cellVisByLocation)
		{
			ClusterMapHex component = keyValuePair.Value.GetComponent<ClusterMapHex>();
			AxialI key = keyValuePair.Key;
			ClusterMapHex.ToggleState state;
			if (this.m_selectedHex != null && this.m_selectedHex.location == key)
			{
				state = ClusterMapHex.ToggleState.Selected;
			}
			else if (flag && this.m_hoveredHex.location.IsAdjacent(key))
			{
				state = ClusterMapHex.ToggleState.OrbitHighlight;
			}
			else
			{
				state = ClusterMapHex.ToggleState.Unselected;
			}
			component.UpdateToggleState(state);
		}
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001E4608 File Offset: 0x001E2808
	public void SelectEntity(ClusterGridEntity entity, bool frameDelay = false)
	{
		if (entity != null)
		{
			this.SetSelectedEntity(entity, frameDelay);
			ClusterMapHex component = this.m_cellVisByLocation[entity.Location].GetComponent<ClusterMapHex>();
			this.m_selectedHex = component;
		}
		this.UpdateVis(null);
	}

	// Token: 0x060053FB RID: 21499 RVA: 0x001E464C File Offset: 0x001E284C
	public void SelectHex(ClusterMapHex newSelectionHex)
	{
		if (this.m_mode == ClusterMapScreen.Mode.Default)
		{
			List<ClusterGridEntity> visibleEntitiesAtCell = ClusterGrid.Instance.GetVisibleEntitiesAtCell(newSelectionHex.location);
			for (int i = visibleEntitiesAtCell.Count - 1; i >= 0; i--)
			{
				KSelectable component = visibleEntitiesAtCell[i].GetComponent<KSelectable>();
				if (component == null || !component.IsSelectable)
				{
					visibleEntitiesAtCell.RemoveAt(i);
				}
			}
			if (visibleEntitiesAtCell.Count == 0)
			{
				this.SetSelectedEntity(null, false);
			}
			else
			{
				int num = visibleEntitiesAtCell.IndexOf(this.m_selectedEntity);
				int index = 0;
				if (num >= 0)
				{
					index = (num + 1) % visibleEntitiesAtCell.Count;
				}
				this.SetSelectedEntity(visibleEntitiesAtCell[index], false);
			}
			this.m_selectedHex = newSelectionHex;
		}
		else if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			global::Debug.Assert(this.m_destinationSelector != null, "Selected a hex in SelectDestination mode with no ClusterDestinationSelector");
			if (ClusterGrid.Instance.GetPath(this.m_selectedHex.location, newSelectionHex.location, this.m_destinationSelector) != null)
			{
				this.m_destinationSelector.SetDestination(newSelectionHex.location);
				if (this.m_closeOnSelect)
				{
					ManagementMenu.Instance.CloseAll();
				}
				else
				{
					this.SetMode(ClusterMapScreen.Mode.Default);
				}
			}
		}
		this.UpdateVis(null);
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x001E476C File Offset: 0x001E296C
	public bool HasCurrentHover()
	{
		return this.m_hoveredHex != null;
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x001E477A File Offset: 0x001E297A
	public AxialI GetCurrentHoverLocation()
	{
		return this.m_hoveredHex.location;
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001E4787 File Offset: 0x001E2987
	public void OnHoverHex(ClusterMapHex newHoverHex)
	{
		this.m_hoveredHex = newHoverHex;
		if (this.m_mode == ClusterMapScreen.Mode.SelectDestination)
		{
			this.UpdateVis(null);
		}
		this.UpdateHexToggleStates();
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001E47A6 File Offset: 0x001E29A6
	public void OnUnhoverHex(ClusterMapHex unhoveredHex)
	{
		if (this.m_hoveredHex == unhoveredHex)
		{
			this.m_hoveredHex = null;
			this.UpdateHexToggleStates();
		}
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001E47C3 File Offset: 0x001E29C3
	public void SetLocationHighlight(AxialI location, bool highlight)
	{
		this.m_cellVisByLocation[location].GetComponent<ClusterMapHex>().ChangeState(highlight ? 1 : 0);
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001E47E4 File Offset: 0x001E29E4
	private void UpdatePaths()
	{
		ClusterDestinationSelector clusterDestinationSelector = (this.m_selectedEntity != null) ? this.m_selectedEntity.GetComponent<ClusterDestinationSelector>() : null;
		if (this.m_mode != ClusterMapScreen.Mode.SelectDestination || !(this.m_hoveredHex != null))
		{
			if (this.m_previewMapPath != null)
			{
				global::Util.KDestroyGameObject(this.m_previewMapPath);
				this.m_previewMapPath = null;
			}
			return;
		}
		global::Debug.Assert(this.m_destinationSelector != null, "In SelectDestination mode without a destination selector");
		AxialI myWorldLocation = this.m_destinationSelector.GetMyWorldLocation();
		string text;
		List<AxialI> path = ClusterGrid.Instance.GetPath(myWorldLocation, this.m_hoveredHex.location, this.m_destinationSelector, out text, false);
		if (path != null)
		{
			if (this.m_previewMapPath == null)
			{
				this.m_previewMapPath = this.pathDrawer.AddPath();
			}
			ClusterMapVisualizer clusterMapVisualizer = this.m_gridEntityVis[this.GetSelectorGridEntity(this.m_destinationSelector)];
			this.m_previewMapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(clusterMapVisualizer.transform.localPosition, path));
			this.m_previewMapPath.SetColor(this.rocketPreviewPathColor);
		}
		else if (this.m_previewMapPath != null)
		{
			global::Util.KDestroyGameObject(this.m_previewMapPath);
			this.m_previewMapPath = null;
		}
		int num = (path != null) ? path.Count : -1;
		if (this.m_selectedEntity != null)
		{
			float range = this.m_selectedEntity.GetComponent<IClusterRange>().GetRange();
			if ((float)num > range / 600f && string.IsNullOrEmpty(text))
			{
				text = string.Format(UI.CLUSTERMAP.TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE, range / 600f);
			}
			bool repeat = clusterDestinationSelector.GetComponent<RocketClusterDestinationSelector>().Repeat;
			this.m_hoveredHex.SetDestinationStatus(text, num, (int)range, repeat);
			return;
		}
		this.m_hoveredHex.SetDestinationStatus(text);
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001E49AC File Offset: 0x001E2BAC
	private ClusterGridEntity GetSelectorGridEntity(ClusterDestinationSelector selector)
	{
		ClusterGridEntity component = selector.GetComponent<ClusterGridEntity>();
		if (component != null && ClusterGrid.Instance.IsVisible(component))
		{
			return component;
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(selector.GetMyWorldLocation(), EntityLayer.Asteroid);
		global::Debug.Assert(component != null || visibleEntityOfLayerAtCell != null, string.Format("{0} has no grid entity and isn't located at a visible asteroid at {1}", selector, selector.GetMyWorldLocation()));
		if (visibleEntityOfLayerAtCell)
		{
			return visibleEntityOfLayerAtCell;
		}
		return component;
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001E4A24 File Offset: 0x001E2C24
	private void UpdateTearStatus()
	{
		ClusterPOIManager clusterPOIManager = null;
		if (ClusterManager.Instance != null)
		{
			clusterPOIManager = ClusterManager.Instance.GetComponent<ClusterPOIManager>();
		}
		if (clusterPOIManager != null)
		{
			TemporalTear temporalTear = clusterPOIManager.GetTemporalTear();
			if (temporalTear != null)
			{
				temporalTear.UpdateStatus();
			}
		}
	}

	// Token: 0x0400380D RID: 14349
	public static ClusterMapScreen Instance;

	// Token: 0x0400380E RID: 14350
	public GameObject cellVisContainer;

	// Token: 0x0400380F RID: 14351
	public GameObject terrainVisContainer;

	// Token: 0x04003810 RID: 14352
	public GameObject mobileVisContainer;

	// Token: 0x04003811 RID: 14353
	public GameObject telescopeVisContainer;

	// Token: 0x04003812 RID: 14354
	public GameObject POIVisContainer;

	// Token: 0x04003813 RID: 14355
	public GameObject FXVisContainer;

	// Token: 0x04003814 RID: 14356
	public ClusterMapVisualizer cellVisPrefab;

	// Token: 0x04003815 RID: 14357
	public ClusterMapVisualizer terrainVisPrefab;

	// Token: 0x04003816 RID: 14358
	public ClusterMapVisualizer mobileVisPrefab;

	// Token: 0x04003817 RID: 14359
	public ClusterMapVisualizer staticVisPrefab;

	// Token: 0x04003818 RID: 14360
	public Color rocketPathColor;

	// Token: 0x04003819 RID: 14361
	public Color rocketSelectedPathColor;

	// Token: 0x0400381A RID: 14362
	public Color rocketPreviewPathColor;

	// Token: 0x0400381B RID: 14363
	private ClusterMapHex m_selectedHex;

	// Token: 0x0400381C RID: 14364
	private ClusterMapHex m_hoveredHex;

	// Token: 0x0400381D RID: 14365
	private ClusterGridEntity m_selectedEntity;

	// Token: 0x0400381E RID: 14366
	public KButton closeButton;

	// Token: 0x0400381F RID: 14367
	private const float ZOOM_SCALE_MIN = 50f;

	// Token: 0x04003820 RID: 14368
	private const float ZOOM_SCALE_MAX = 150f;

	// Token: 0x04003821 RID: 14369
	private const float ZOOM_SCALE_INCREMENT = 25f;

	// Token: 0x04003822 RID: 14370
	private const float ZOOM_SCALE_SPEED = 4f;

	// Token: 0x04003823 RID: 14371
	private const float ZOOM_NAME_THRESHOLD = 115f;

	// Token: 0x04003824 RID: 14372
	private float m_currentZoomScale = 75f;

	// Token: 0x04003825 RID: 14373
	private float m_targetZoomScale = 75f;

	// Token: 0x04003826 RID: 14374
	private ClusterMapPath m_previewMapPath;

	// Token: 0x04003827 RID: 14375
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityVis = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x04003828 RID: 14376
	private Dictionary<ClusterGridEntity, ClusterMapVisualizer> m_gridEntityAnims = new Dictionary<ClusterGridEntity, ClusterMapVisualizer>();

	// Token: 0x04003829 RID: 14377
	private Dictionary<AxialI, ClusterMapVisualizer> m_cellVisByLocation = new Dictionary<AxialI, ClusterMapVisualizer>();

	// Token: 0x0400382A RID: 14378
	private Action<object> m_onDestinationChangedDelegate;

	// Token: 0x0400382B RID: 14379
	private Action<object> m_onSelectObjectDelegate;

	// Token: 0x0400382C RID: 14380
	[SerializeField]
	private KScrollRect mapScrollRect;

	// Token: 0x0400382D RID: 14381
	[SerializeField]
	private float scrollSpeed = 15f;

	// Token: 0x0400382E RID: 14382
	public GameObject selectMarkerPrefab;

	// Token: 0x0400382F RID: 14383
	public ClusterMapPathDrawer pathDrawer;

	// Token: 0x04003830 RID: 14384
	private SelectMarker m_selectMarker;

	// Token: 0x04003831 RID: 14385
	private bool movingToTargetNISPosition;

	// Token: 0x04003832 RID: 14386
	private Vector3 targetNISPosition;

	// Token: 0x04003833 RID: 14387
	private float targetNISZoom;

	// Token: 0x04003834 RID: 14388
	private AxialI selectOnMoveNISComplete;

	// Token: 0x04003835 RID: 14389
	private ClusterMapScreen.Mode m_mode;

	// Token: 0x04003836 RID: 14390
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x04003837 RID: 14391
	private bool m_closeOnSelect;

	// Token: 0x04003838 RID: 14392
	private Coroutine activeMoveToTargetRoutine;

	// Token: 0x04003839 RID: 14393
	public float floatCycleScale = 4f;

	// Token: 0x0400383A RID: 14394
	public float floatCycleOffset = 0.75f;

	// Token: 0x0400383B RID: 14395
	public float floatCycleSpeed = 0.75f;

	// Token: 0x020019CB RID: 6603
	public enum Mode
	{
		// Token: 0x04007762 RID: 30562
		Default,
		// Token: 0x04007763 RID: 30563
		SelectDestination
	}
}
