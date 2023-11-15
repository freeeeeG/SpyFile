using System;
using System.Collections.Generic;
using System.Linq;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200081C RID: 2076
[AddComponentMenu("KMonoBehaviour/scripts/InterfaceTool")]
public class InterfaceTool : KMonoBehaviour
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x0014C88E File Offset: 0x0014AA8E
	public static InterfaceToolConfig ActiveConfig
	{
		get
		{
			if (InterfaceTool.interfaceConfigMap == null)
			{
				InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
			}
			return InterfaceTool.activeConfigs[InterfaceTool.activeConfigs.Count - 1];
		}
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x0014C8B4 File Offset: 0x0014AAB4
	public static void ToggleConfig(global::Action configKey)
	{
		if (InterfaceTool.interfaceConfigMap == null)
		{
			InterfaceTool.InitializeConfigs(global::Action.Invalid, null);
		}
		InterfaceToolConfig item;
		if (!InterfaceTool.interfaceConfigMap.TryGetValue(configKey, out item))
		{
			global::Debug.LogWarning(string.Format("[InterfaceTool] No config is associated with Key: {0}!", configKey) + " Are you sure the configs were initialized properly!");
			return;
		}
		if (InterfaceTool.activeConfigs.BinarySearch(item, InterfaceToolConfig.ConfigComparer) <= 0)
		{
			global::Debug.Log(string.Format("[InterfaceTool] Pushing config with key: {0}", configKey));
			InterfaceTool.activeConfigs.Add(item);
			InterfaceTool.activeConfigs.Sort(InterfaceToolConfig.ConfigComparer);
			return;
		}
		global::Debug.Log(string.Format("[InterfaceTool] Popping config with key: {0}", configKey));
		InterfaceTool.activeConfigs.Remove(item);
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x0014C964 File Offset: 0x0014AB64
	public static void InitializeConfigs(global::Action defaultKey, List<InterfaceToolConfig> configs)
	{
		string arg = (configs == null) ? "null" : configs.Count.ToString();
		global::Debug.Log(string.Format("[InterfaceTool] Initializing configs with values of DefaultKey: {0} Configs: {1}", defaultKey, arg));
		if (configs == null || configs.Count == 0)
		{
			InterfaceToolConfig interfaceToolConfig = ScriptableObject.CreateInstance<InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap = new Dictionary<global::Action, InterfaceToolConfig>();
			InterfaceTool.interfaceConfigMap[interfaceToolConfig.InputAction] = interfaceToolConfig;
			return;
		}
		InterfaceTool.interfaceConfigMap = configs.ToDictionary((InterfaceToolConfig x) => x.InputAction);
		InterfaceTool.ToggleConfig(defaultKey);
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x0014C9FD File Offset: 0x0014ABFD
	public HashedString ViewMode
	{
		get
		{
			return this.viewMode;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x0014CA05 File Offset: 0x0014AC05
	public virtual string[] DlcIDs
	{
		get
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0014CA0C File Offset: 0x0014AC0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hoverTextConfiguration = base.GetComponent<HoverTextConfiguration>();
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0014CA20 File Offset: 0x0014AC20
	public void ActivateTool()
	{
		this.OnActivateTool();
		this.OnMouseMove(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		Game.Instance.Trigger(1174281782, this);
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x0014CA48 File Offset: 0x0014AC48
	public virtual bool ShowHoverUI()
	{
		if (ManagementMenu.Instance == null || ManagementMenu.Instance.IsFullscreenUIActive())
		{
			return false;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
		if (OverlayScreen.Instance == null || !ClusterManager.Instance.IsPositionInActiveWorld(vector) || vector.x < 0f || vector.x > Grid.WidthInMeters || vector.y < 0f || vector.y > Grid.HeightInMeters)
		{
			return false;
		}
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		return current != null && !current.IsPointerOverGameObject();
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0014CAEC File Offset: 0x0014ACEC
	protected virtual void OnActivateTool()
	{
		if (OverlayScreen.Instance != null && this.viewMode != OverlayModes.None.ID && OverlayScreen.Instance.mode != this.viewMode)
		{
			OverlayScreen.Instance.ToggleOverlay(this.viewMode, true);
			InterfaceTool.toolActivatedViewMode = this.viewMode;
		}
		this.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x0014CB60 File Offset: 0x0014AD60
	public void SetCurrentVirtualInputModuleMousMovementMode(bool mouseMovementOnly, Action<VirtualInputModule> extraActions = null)
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null && current.currentInputModule != null)
		{
			VirtualInputModule virtualInputModule = current.currentInputModule as VirtualInputModule;
			if (virtualInputModule != null)
			{
				virtualInputModule.mouseMovementOnly = mouseMovementOnly;
				if (extraActions != null)
				{
					extraActions(virtualInputModule);
				}
			}
		}
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0014CBB0 File Offset: 0x0014ADB0
	public void DeactivateTool(InterfaceTool new_tool = null)
	{
		this.OnDeactivateTool(new_tool);
		if ((new_tool == null || new_tool == SelectTool.Instance) && InterfaceTool.toolActivatedViewMode != OverlayModes.None.ID && InterfaceTool.toolActivatedViewMode == SimDebugView.Instance.GetMode())
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			InterfaceTool.toolActivatedViewMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x0014CC1B File Offset: 0x0014AE1B
	public virtual void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = null;
	}

	// Token: 0x06003BEF RID: 15343 RVA: 0x0014CC20 File Offset: 0x0014AE20
	protected virtual void OnDeactivateTool(InterfaceTool new_tool)
	{
	}

	// Token: 0x06003BF0 RID: 15344 RVA: 0x0014CC22 File Offset: 0x0014AE22
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x06003BF1 RID: 15345 RVA: 0x0014CC2B File Offset: 0x0014AE2B
	public virtual string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06003BF2 RID: 15346 RVA: 0x0014CC34 File Offset: 0x0014AE34
	public virtual void OnMouseMove(Vector3 cursor_pos)
	{
		if (this.visualizer == null || !this.isAppFocused)
		{
			return;
		}
		cursor_pos = Grid.CellToPosCBC(Grid.PosToCell(cursor_pos), this.visualizerLayer);
		cursor_pos.z += -0.15f;
		this.visualizer.transform.SetLocalPosition(cursor_pos);
	}

	// Token: 0x06003BF3 RID: 15347 RVA: 0x0014CC90 File Offset: 0x0014AE90
	public virtual void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x06003BF4 RID: 15348 RVA: 0x0014CC92 File Offset: 0x0014AE92
	public virtual void OnKeyUp(KButtonEvent e)
	{
	}

	// Token: 0x06003BF5 RID: 15349 RVA: 0x0014CC94 File Offset: 0x0014AE94
	public virtual void OnLeftClickDown(Vector3 cursor_pos)
	{
	}

	// Token: 0x06003BF6 RID: 15350 RVA: 0x0014CC96 File Offset: 0x0014AE96
	public virtual void OnLeftClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x06003BF7 RID: 15351 RVA: 0x0014CC98 File Offset: 0x0014AE98
	public virtual void OnRightClickDown(Vector3 cursor_pos, KButtonEvent e)
	{
	}

	// Token: 0x06003BF8 RID: 15352 RVA: 0x0014CC9A File Offset: 0x0014AE9A
	public virtual void OnRightClickUp(Vector3 cursor_pos)
	{
	}

	// Token: 0x06003BF9 RID: 15353 RVA: 0x0014CC9C File Offset: 0x0014AE9C
	public virtual void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
	}

	// Token: 0x06003BFA RID: 15354 RVA: 0x0014CCC0 File Offset: 0x0014AEC0
	protected Vector2 GetRegularizedPos(Vector2 input, bool minimize)
	{
		Vector3 vector = new Vector3(Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, 0f);
		return Grid.CellToPosCCC(Grid.PosToCell(input), Grid.SceneLayer.Background) + (minimize ? (-vector) : vector);
	}

	// Token: 0x06003BFB RID: 15355 RVA: 0x0014CD08 File Offset: 0x0014AF08
	protected Vector2 GetWorldRestrictedPosition(Vector2 input)
	{
		input.x = Mathf.Clamp(input.x, ClusterManager.Instance.activeWorld.minimumBounds.x, ClusterManager.Instance.activeWorld.maximumBounds.x);
		input.y = Mathf.Clamp(input.y, ClusterManager.Instance.activeWorld.minimumBounds.y, ClusterManager.Instance.activeWorld.maximumBounds.y);
		return input;
	}

	// Token: 0x06003BFC RID: 15356 RVA: 0x0014CD8C File Offset: 0x0014AF8C
	protected void SetCursor(Texture2D new_cursor, Vector2 offset, CursorMode mode)
	{
		if (new_cursor != InterfaceTool.activeCursor && new_cursor != null)
		{
			InterfaceTool.activeCursor = new_cursor;
			try
			{
				Cursor.SetCursor(new_cursor, offset, mode);
				if (PlayerController.Instance.vim != null)
				{
					PlayerController.Instance.vim.SetCursor(new_cursor);
				}
			}
			catch (Exception ex)
			{
				string details = string.Format("SetCursor Failed new_cursor={0} offset={1} mode={2}", new_cursor, offset, mode);
				KCrashReporter.ReportDevNotification("SetCursor Failed", ex.StackTrace, details, false);
			}
		}
	}

	// Token: 0x06003BFD RID: 15357 RVA: 0x0014CE20 File Offset: 0x0014B020
	protected void UpdateHoverElements(List<KSelectable> hits)
	{
		if (this.hoverTextConfiguration != null)
		{
			this.hoverTextConfiguration.UpdateHoverElements(hits);
		}
	}

	// Token: 0x06003BFE RID: 15358 RVA: 0x0014CE3C File Offset: 0x0014B03C
	public virtual void LateUpdate()
	{
		if (!this.populateHitsList)
		{
			this.UpdateHoverElements(null);
			return;
		}
		if (!this.isAppFocused)
		{
			return;
		}
		if (!Grid.IsValidCell(Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()))))
		{
			return;
		}
		this.hits.Clear();
		this.GetSelectablesUnderCursor(this.hits);
		KSelectable objectUnderCursor = this.GetObjectUnderCursor<KSelectable>(false, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, null);
		this.UpdateHoverElements(this.hits);
		if (!this.hasFocus && this.hoverOverride == null)
		{
			this.ClearHover();
		}
		else if (objectUnderCursor != this.hover)
		{
			this.ClearHover();
			this.hover = objectUnderCursor;
			if (objectUnderCursor != null)
			{
				Game.Instance.Trigger(2095258329, objectUnderCursor.gameObject);
				objectUnderCursor.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06003BFF RID: 15359 RVA: 0x0014CF40 File Offset: 0x0014B140
	public void GetSelectablesUnderCursor(List<KSelectable> hits)
	{
		if (this.hoverOverride != null)
		{
			hits.Add(this.hoverOverride);
		}
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 vector2 = new Vector2(vector.x, vector.y);
		int cell = Grid.PosToCell(vector);
		if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
		{
			return;
		}
		Game.Instance.statusItemRenderer.GetIntersections(vector2, hits);
		ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)vector2.x, (int)vector2.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		pooledList.Sort((ScenePartitionerEntry x, ScenePartitionerEntry y) => this.SortHoverCards(x, y));
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
			if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector2.x, vector2.y)))
			{
				KSelectable kselectable = kcollider2D.GetComponent<KSelectable>();
				if (kselectable == null)
				{
					kselectable = kcollider2D.GetComponentInParent<KSelectable>();
				}
				if (!(kselectable == null) && kselectable.isActiveAndEnabled && !hits.Contains(kselectable) && kselectable.IsSelectable)
				{
					hits.Add(kselectable);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x0014D0E4 File Offset: 0x0014B2E4
	public void SetLinkCursor(bool set)
	{
		this.SetCursor(set ? Assets.GetTexture("cursor_hand") : this.cursor, set ? Vector2.zero : this.cursorOffset, CursorMode.Auto);
	}

	// Token: 0x06003C01 RID: 15361 RVA: 0x0014D114 File Offset: 0x0014B314
	protected T GetObjectUnderCursor<T>(bool cycleSelection, Func<T, bool> condition = null, Component previous_selection = null) where T : MonoBehaviour
	{
		this.intersections.Clear();
		this.GetObjectUnderCursor2D<T>(this.intersections, condition, this.layerMask);
		this.intersections.RemoveAll(new Predicate<InterfaceTool.Intersection>(InterfaceTool.is_component_null));
		if (this.intersections.Count <= 0)
		{
			this.prevIntersectionGroup.Clear();
			return default(T);
		}
		this.curIntersectionGroup.Clear();
		foreach (InterfaceTool.Intersection intersection in this.intersections)
		{
			this.curIntersectionGroup.Add(intersection.component);
		}
		if (!this.prevIntersectionGroup.Equals(this.curIntersectionGroup))
		{
			this.hitCycleCount = 0;
			this.prevIntersectionGroup = this.curIntersectionGroup;
		}
		this.intersections.Sort((InterfaceTool.Intersection a, InterfaceTool.Intersection b) => this.SortSelectables(a.component as KMonoBehaviour, b.component as KMonoBehaviour));
		int index = 0;
		if (cycleSelection)
		{
			index = this.hitCycleCount % this.intersections.Count;
			if (this.intersections[index].component != previous_selection || previous_selection == null)
			{
				index = 0;
				this.hitCycleCount = 0;
			}
			else
			{
				int num = this.hitCycleCount + 1;
				this.hitCycleCount = num;
				index = num % this.intersections.Count;
			}
		}
		return this.intersections[index].component as T;
	}

	// Token: 0x06003C02 RID: 15362 RVA: 0x0014D294 File Offset: 0x0014B494
	private void GetObjectUnderCursor2D<T>(List<InterfaceTool.Intersection> intersections, Func<T, bool> condition, int layer_mask) where T : MonoBehaviour
	{
		Camera main = Camera.main;
		Vector3 position = new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -main.transform.GetPosition().z);
		Vector3 vector = main.ScreenToWorldPoint(position);
		Vector2 pos = new Vector2(vector.x, vector.y);
		if (this.hoverOverride != null)
		{
			intersections.Add(new InterfaceTool.Intersection
			{
				component = this.hoverOverride,
				distance = -100f
			});
		}
		int cell = Grid.PosToCell(vector);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
			ListPool<ScenePartitionerEntry, SelectTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
			int x_bottomLeft = 0;
			int y_bottomLeft = 0;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
				if (!(kcollider2D == null) && kcollider2D.Intersects(new Vector2(vector.x, vector.y)))
				{
					T t = kcollider2D.GetComponent<T>();
					if (t == null)
					{
						t = kcollider2D.GetComponentInParent<T>();
					}
					if (!(t == null) && (1 << t.gameObject.layer & layer_mask) != 0 && !(t == null) && (condition == null || condition(t)))
					{
						float num = t.transform.GetPosition().z - vector.z;
						bool flag = false;
						for (int i = 0; i < intersections.Count; i++)
						{
							InterfaceTool.Intersection intersection = intersections[i];
							if (intersection.component.gameObject == t.gameObject)
							{
								intersection.distance = Mathf.Min(intersection.distance, num);
								intersections[i] = intersection;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							intersections.Add(new InterfaceTool.Intersection
							{
								component = t,
								distance = num
							});
						}
					}
				}
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x06003C03 RID: 15363 RVA: 0x0014D538 File Offset: 0x0014B738
	private int SortSelectables(KMonoBehaviour x, KMonoBehaviour y)
	{
		if (x == null && y == null)
		{
			return 0;
		}
		if (x == null)
		{
			return -1;
		}
		if (y == null)
		{
			return 1;
		}
		int num = x.transform.GetPosition().z.CompareTo(y.transform.GetPosition().z);
		if (num != 0)
		{
			return num;
		}
		return x.GetInstanceID().CompareTo(y.GetInstanceID());
	}

	// Token: 0x06003C04 RID: 15364 RVA: 0x0014D5B1 File Offset: 0x0014B7B1
	public void SetHoverOverride(KSelectable hover_override)
	{
		this.hoverOverride = hover_override;
	}

	// Token: 0x06003C05 RID: 15365 RVA: 0x0014D5BC File Offset: 0x0014B7BC
	private int SortHoverCards(ScenePartitionerEntry x, ScenePartitionerEntry y)
	{
		KMonoBehaviour x2 = x.obj as KMonoBehaviour;
		KMonoBehaviour y2 = y.obj as KMonoBehaviour;
		return this.SortSelectables(x2, y2);
	}

	// Token: 0x06003C06 RID: 15366 RVA: 0x0014D5E9 File Offset: 0x0014B7E9
	private static bool is_component_null(InterfaceTool.Intersection intersection)
	{
		return !intersection.component;
	}

	// Token: 0x06003C07 RID: 15367 RVA: 0x0014D5F9 File Offset: 0x0014B7F9
	protected void ClearHover()
	{
		if (this.hover != null)
		{
			KSelectable kselectable = this.hover;
			this.hover = null;
			kselectable.Unhover();
			Game.Instance.Trigger(-1201923725, null);
		}
	}

	// Token: 0x04002761 RID: 10081
	private static Dictionary<global::Action, InterfaceToolConfig> interfaceConfigMap = null;

	// Token: 0x04002762 RID: 10082
	private static List<InterfaceToolConfig> activeConfigs = new List<InterfaceToolConfig>();

	// Token: 0x04002763 RID: 10083
	public const float MaxClickDistance = 0.02f;

	// Token: 0x04002764 RID: 10084
	public const float DepthBias = -0.15f;

	// Token: 0x04002765 RID: 10085
	public GameObject visualizer;

	// Token: 0x04002766 RID: 10086
	public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;

	// Token: 0x04002767 RID: 10087
	public string placeSound;

	// Token: 0x04002768 RID: 10088
	protected bool populateHitsList;

	// Token: 0x04002769 RID: 10089
	[NonSerialized]
	public bool hasFocus;

	// Token: 0x0400276A RID: 10090
	[SerializeField]
	protected Texture2D cursor;

	// Token: 0x0400276B RID: 10091
	public Vector2 cursorOffset = new Vector2(2f, 2f);

	// Token: 0x0400276C RID: 10092
	public System.Action OnDeactivate;

	// Token: 0x0400276D RID: 10093
	private static Texture2D activeCursor = null;

	// Token: 0x0400276E RID: 10094
	private static HashedString toolActivatedViewMode = OverlayModes.None.ID;

	// Token: 0x0400276F RID: 10095
	protected HashedString viewMode = OverlayModes.None.ID;

	// Token: 0x04002770 RID: 10096
	private HoverTextConfiguration hoverTextConfiguration;

	// Token: 0x04002771 RID: 10097
	private KSelectable hoverOverride;

	// Token: 0x04002772 RID: 10098
	public KSelectable hover;

	// Token: 0x04002773 RID: 10099
	protected int layerMask;

	// Token: 0x04002774 RID: 10100
	protected SelectMarker selectMarker;

	// Token: 0x04002775 RID: 10101
	private List<RaycastResult> castResults = new List<RaycastResult>();

	// Token: 0x04002776 RID: 10102
	private bool isAppFocused = true;

	// Token: 0x04002777 RID: 10103
	private List<KSelectable> hits = new List<KSelectable>();

	// Token: 0x04002778 RID: 10104
	protected bool playedSoundThisFrame;

	// Token: 0x04002779 RID: 10105
	private List<InterfaceTool.Intersection> intersections = new List<InterfaceTool.Intersection>();

	// Token: 0x0400277A RID: 10106
	private HashSet<Component> prevIntersectionGroup = new HashSet<Component>();

	// Token: 0x0400277B RID: 10107
	private HashSet<Component> curIntersectionGroup = new HashSet<Component>();

	// Token: 0x0400277C RID: 10108
	private int hitCycleCount;

	// Token: 0x020015EF RID: 5615
	public struct Intersection
	{
		// Token: 0x04006A07 RID: 27143
		public MonoBehaviour component;

		// Token: 0x04006A08 RID: 27144
		public float distance;
	}
}
