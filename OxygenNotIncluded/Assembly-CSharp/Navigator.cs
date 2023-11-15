using System;
using System.Collections.Generic;
using System.IO;
using STRINGS;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06001CAE RID: 7342 RVA: 0x00099543 File Offset: 0x00097743
	// (set) Token: 0x06001CAF RID: 7343 RVA: 0x00099550 File Offset: 0x00097750
	public bool IsFacingLeft
	{
		get
		{
			return this.facing.GetFacing();
		}
		set
		{
			this.facing.SetFacing(value);
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x0009955E File Offset: 0x0009775E
	// (set) Token: 0x06001CB1 RID: 7345 RVA: 0x00099566 File Offset: 0x00097766
	public KMonoBehaviour target { get; set; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x0009956F File Offset: 0x0009776F
	// (set) Token: 0x06001CB3 RID: 7347 RVA: 0x00099577 File Offset: 0x00097777
	public CellOffset[] targetOffsets { get; private set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x00099580 File Offset: 0x00097780
	// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x00099588 File Offset: 0x00097788
	public NavGrid NavGrid { get; private set; }

	// Token: 0x06001CB6 RID: 7350 RVA: 0x00099594 File Offset: 0x00097794
	public void Serialize(BinaryWriter writer)
	{
		byte currentNavType = (byte)this.CurrentNavType;
		writer.Write(currentNavType);
		writer.Write(this.distanceTravelledByNavType.Count);
		foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
		{
			byte key = (byte)keyValuePair.Key;
			writer.Write(key);
			writer.Write(keyValuePair.Value);
		}
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x0009961C File Offset: 0x0009781C
	public void Deserialize(IReader reader)
	{
		NavType navType = (NavType)reader.ReadByte();
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				NavType key = (NavType)reader.ReadByte();
				int value = reader.ReadInt32();
				if (this.distanceTravelledByNavType.ContainsKey(key))
				{
					this.distanceTravelledByNavType[key] = value;
				}
			}
		}
		bool flag = false;
		NavType[] validNavTypes = this.NavGrid.ValidNavTypes;
		for (int j = 0; j < validNavTypes.Length; j++)
		{
			if (validNavTypes[j] == navType)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.CurrentNavType = navType;
		}
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x000996C4 File Offset: 0x000978C4
	protected override void OnPrefabInit()
	{
		this.transitionDriver = new TransitionDriver(this);
		this.targetLocator = Util.KInstantiate(Assets.GetPrefab(TargetLocator.ID), null, null).GetComponent<KPrefabID>();
		this.targetLocator.gameObject.SetActive(true);
		this.log = new LoggerFSS("Navigator", 35);
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
		this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
		base.GetComponent<PathProber>().SetValidNavTypes(this.NavGrid.ValidNavTypes, this.maxProbingRadius);
		this.distanceTravelledByNavType = new Dictionary<NavType, int>();
		for (int i = 0; i < 11; i++)
		{
			this.distanceTravelledByNavType.Add((NavType)i, 0);
		}
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x00099788 File Offset: 0x00097988
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
		base.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
		base.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
		if (this.updateProber)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
		this.pathProbeTask = new Navigator.PathProbeTask(this);
		this.SetCurrentNavType(this.CurrentNavType);
		this.SubscribeUnstuckFunctions();
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x00099822 File Offset: 0x00097A22
	private void SubscribeUnstuckFunctions()
	{
		if (this.CurrentNavType == NavType.Tube)
		{
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
		}
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x0009984F File Offset: 0x00097A4F
	private void UnsubscribeUnstuckFunctions()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x00099874 File Offset: 0x00097A74
	private void OnBuildingTileChanged(int cell, object building)
	{
		if (this.CurrentNavType == NavType.Tube && building == null)
		{
			bool flag = cell == Grid.PosToCell(this);
			if (base.smi != null && flag)
			{
				this.SetCurrentNavType(NavType.Floor);
				this.UnsubscribeUnstuckFunctions();
			}
		}
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000998B1 File Offset: 0x00097AB1
	protected override void OnCleanUp()
	{
		this.UnsubscribeUnstuckFunctions();
		base.OnCleanUp();
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000998BF File Offset: 0x00097ABF
	public bool IsMoving()
	{
		return base.smi.IsInsideState(base.smi.sm.normal.moving);
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000998E1 File Offset: 0x00097AE1
	public bool GoTo(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00099919 File Offset: 0x00097B19
	public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, tactic);
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x0009994D File Offset: 0x00097B4D
	public void UpdateTarget(int cell)
	{
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x00099968 File Offset: 0x00097B68
	public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
	{
		if (tactic == null)
		{
			tactic = NavigationTactics.ReduceTravelDistance;
		}
		base.smi.GoTo(base.smi.sm.normal.moving);
		base.smi.sm.moveTarget.Set(target.gameObject, base.smi, false);
		this.tactic = tactic;
		this.target = target;
		this.targetOffsets = offsets;
		this.ClearReservedCell();
		this.AdvancePath(true);
		return this.IsMoving();
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000999EA File Offset: 0x00097BEA
	public void BeginTransition(NavGrid.Transition transition)
	{
		this.transitionDriver.EndTransition();
		base.smi.GoTo(base.smi.sm.normal.moving);
		this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x00099A2C File Offset: 0x00097C2C
	private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
	{
		atNextNode = false;
		bool flag = false;
		if (path.IsValid())
		{
			int target_cell = Grid.PosToCell(this.target);
			flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell));
			flag &= Grid.IsCellOffsetOf(this.reservedCell, target_cell, this.targetOffsets);
		}
		if (flag)
		{
			int num = Grid.PosToCell(this);
			flag = (num == path.nodes[0].cell && this.CurrentNavType == path.nodes[0].navType);
			flag |= (atNextNode = (num == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType));
		}
		if (!flag)
		{
			return false;
		}
		PathFinderAbilities currentAbilities = this.GetCurrentAbilities();
		return PathFinder.ValidatePath(this.NavGrid, currentAbilities, ref path);
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00099B14 File Offset: 0x00097D14
	public void AdvancePath(bool trigger_advance = true)
	{
		int num = Grid.PosToCell(this);
		if (this.target == null)
		{
			base.Trigger(-766531887, null);
			this.Stop(false, true);
		}
		else if (num == this.reservedCell && this.CurrentNavType != NavType.Tube)
		{
			this.Stop(true, true);
		}
		else
		{
			bool flag2;
			bool flag = !this.ValidatePath(ref this.path, out flag2);
			if (flag2)
			{
				this.path.nodes.RemoveAt(0);
			}
			if (flag)
			{
				int root = Grid.PosToCell(this.target);
				int cellPreferences = this.tactic.GetCellPreferences(root, this.targetOffsets, this);
				this.SetReservedCell(cellPreferences);
				if (this.reservedCell == NavigationReservations.InvalidReservation)
				{
					this.Stop(false, true);
				}
				else
				{
					PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(num, this.CurrentNavType, this.flags);
					PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
				}
			}
			if (this.path.IsValid())
			{
				this.BeginTransition(this.NavGrid.transitions[(int)this.path.nodes[1].transitionId]);
				this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
			}
			else if (this.path.HasArrived())
			{
				this.Stop(true, true);
			}
			else
			{
				this.ClearReservedCell();
				this.Stop(false, true);
			}
		}
		if (trigger_advance)
		{
			base.Trigger(1347184327, null);
		}
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x00099CB9 File Offset: 0x00097EB9
	public NavGrid.Transition GetNextTransition()
	{
		return this.NavGrid.transitions[(int)this.path.nodes[1].transitionId];
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x00099CE4 File Offset: 0x00097EE4
	public void Stop(bool arrived_at_destination = false, bool play_idle = true)
	{
		this.target = null;
		this.targetOffsets = null;
		this.path.Clear();
		base.smi.sm.moveTarget.Set(null, base.smi);
		this.transitionDriver.EndTransition();
		if (play_idle)
		{
			HashedString idleAnim = this.NavGrid.GetIdleAnim(this.CurrentNavType);
			base.GetComponent<KAnimControllerBase>().Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (arrived_at_destination)
		{
			base.smi.GoTo(base.smi.sm.normal.arrived);
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.normal.moving)
		{
			this.ClearReservedCell();
			base.smi.GoTo(base.smi.sm.normal.failed);
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00099DC9 File Offset: 0x00097FC9
	private void SimEveryTick(float dt)
	{
		if (this.IsMoving())
		{
			this.transitionDriver.UpdateTransition(dt);
		}
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x00099DDF File Offset: 0x00097FDF
	public void Sim4000ms(float dt)
	{
		this.UpdateProbe(true);
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x00099DE8 File Offset: 0x00097FE8
	public void UpdateProbe(bool forceUpdate = false)
	{
		if (forceUpdate || !this.executePathProbeTaskAsync)
		{
			this.pathProbeTask.Update();
			this.pathProbeTask.Run(null);
		}
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x00099E0C File Offset: 0x0009800C
	public void DrawPath()
	{
		if (base.gameObject.activeInHierarchy && this.IsMoving())
		{
			NavPathDrawer.Instance.DrawPath(base.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), this.path);
		}
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x00099E3E File Offset: 0x0009803E
	public void Pause(string reason)
	{
		base.smi.sm.isPaused.Set(true, base.smi, false);
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x00099E5E File Offset: 0x0009805E
	public void Unpause(string reason)
	{
		base.smi.sm.isPaused.Set(false, base.smi, false);
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x00099E7E File Offset: 0x0009807E
	private void OnDefeated(object data)
	{
		this.ClearReservedCell();
		this.Stop(false, false);
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x00099E8E File Offset: 0x0009808E
	private void ClearReservedCell()
	{
		if (this.reservedCell != NavigationReservations.InvalidReservation)
		{
			NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
			this.reservedCell = NavigationReservations.InvalidReservation;
		}
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x00099EB8 File Offset: 0x000980B8
	private void SetReservedCell(int cell)
	{
		this.ClearReservedCell();
		this.reservedCell = cell;
		NavigationReservations.Instance.AddOccupancy(cell);
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x00099ED2 File Offset: 0x000980D2
	public int GetReservedCell()
	{
		return this.reservedCell;
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x00099EDA File Offset: 0x000980DA
	public int GetAnchorCell()
	{
		return this.AnchorCell;
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x00099EE2 File Offset: 0x000980E2
	public bool IsValidNavType(NavType nav_type)
	{
		return this.NavGrid.HasNavTypeData(nav_type);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x00099EF0 File Offset: 0x000980F0
	public void SetCurrentNavType(NavType nav_type)
	{
		this.CurrentNavType = nav_type;
		this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell(this));
		NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Vector2 one = Vector2.one;
		if (navTypeData.flipX)
		{
			one.x = -1f;
		}
		if (navTypeData.flipY)
		{
			one.y = -1f;
		}
		component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x00099F94 File Offset: 0x00098194
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (NavPathDrawer.Instance.GetNavigator() != this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0.1f);
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x0009A097 File Offset: 0x00098297
	private void OnFollowCam()
	{
		if (CameraController.Instance.followTarget == base.transform)
		{
			CameraController.Instance.ClearFollowTarget();
			return;
		}
		CameraController.Instance.SetFollowTarget(base.transform);
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x0009A0CB File Offset: 0x000982CB
	private void OnDrawPaths()
	{
		if (NavPathDrawer.Instance.GetNavigator() != this)
		{
			NavPathDrawer.Instance.SetNavigator(this);
			return;
		}
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x0009A0F5 File Offset: 0x000982F5
	private void OnSelectObject(object data)
	{
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x0009A101 File Offset: 0x00098301
	public void OnStore(object data)
	{
		if (data is Storage || (data != null && (bool)data))
		{
			this.Stop(false, true);
		}
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x0009A124 File Offset: 0x00098324
	public PathFinderAbilities GetCurrentAbilities()
	{
		this.abilities.Refresh();
		return this.abilities;
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x0009A137 File Offset: 0x00098337
	public void SetAbilities(PathFinderAbilities abilities)
	{
		this.abilities = abilities;
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x0009A140 File Offset: 0x00098340
	public bool CanReach(IApproachable approachable)
	{
		return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x0009A154 File Offset: 0x00098354
	public bool CanReach(int cell, CellOffset[] offsets)
	{
		foreach (CellOffset offset in offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (this.CanReach(cell2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x0009A18D File Offset: 0x0009838D
	public bool CanReach(int cell)
	{
		return this.GetNavigationCost(cell) != -1;
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0009A19C File Offset: 0x0009839C
	public int GetNavigationCost(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return this.PathProber.GetCost(cell);
		}
		return -1;
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x0009A1B4 File Offset: 0x000983B4
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathProber.GetNavigationCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0009A1C4 File Offset: 0x000983C4
	public int GetNavigationCost(int cell, CellOffset[] offsets)
	{
		int num = -1;
		int num2 = offsets.Length;
		for (int i = 0; i < num2; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0009A20C File Offset: 0x0009840C
	public int GetNavigationCost(IApproachable approachable)
	{
		return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0009A220 File Offset: 0x00098420
	public void RunQuery(PathFinderQuery query)
	{
		int cell = Grid.PosToCell(this);
		PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(cell, this.CurrentNavType, this.flags);
		PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x0009A25B File Offset: 0x0009845B
	public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags |= new_flags;
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x0009A26B File Offset: 0x0009846B
	public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags &= ~new_flags;
	}

	// Token: 0x04000FD8 RID: 4056
	public bool DebugDrawPath;

	// Token: 0x04000FDC RID: 4060
	[MyCmpAdd]
	public PathProber PathProber;

	// Token: 0x04000FDD RID: 4061
	[MyCmpAdd]
	private Facing facing;

	// Token: 0x04000FDE RID: 4062
	[MyCmpGet]
	public AnimEventHandler animEventHandler;

	// Token: 0x04000FDF RID: 4063
	public float defaultSpeed = 1f;

	// Token: 0x04000FE0 RID: 4064
	public TransitionDriver transitionDriver;

	// Token: 0x04000FE1 RID: 4065
	public string NavGridName;

	// Token: 0x04000FE2 RID: 4066
	public bool updateProber;

	// Token: 0x04000FE3 RID: 4067
	public int maxProbingRadius;

	// Token: 0x04000FE4 RID: 4068
	public PathFinder.PotentialPath.Flags flags;

	// Token: 0x04000FE5 RID: 4069
	private LoggerFSS log;

	// Token: 0x04000FE6 RID: 4070
	public Dictionary<NavType, int> distanceTravelledByNavType;

	// Token: 0x04000FE7 RID: 4071
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;

	// Token: 0x04000FE8 RID: 4072
	private PathFinderAbilities abilities;

	// Token: 0x04000FE9 RID: 4073
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04000FEA RID: 4074
	[NonSerialized]
	public PathFinder.Path path;

	// Token: 0x04000FEB RID: 4075
	public NavType CurrentNavType;

	// Token: 0x04000FEC RID: 4076
	private int AnchorCell;

	// Token: 0x04000FED RID: 4077
	private KPrefabID targetLocator;

	// Token: 0x04000FEE RID: 4078
	private int reservedCell = NavigationReservations.InvalidReservation;

	// Token: 0x04000FEF RID: 4079
	private NavTactic tactic;

	// Token: 0x04000FF0 RID: 4080
	public Navigator.PathProbeTask pathProbeTask;

	// Token: 0x04000FF1 RID: 4081
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04000FF2 RID: 4082
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04000FF3 RID: 4083
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnSelectObject(data);
	});

	// Token: 0x04000FF4 RID: 4084
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04000FF5 RID: 4085
	public bool executePathProbeTaskAsync;

	// Token: 0x0200117F RID: 4479
	public class ActiveTransition
	{
		// Token: 0x060079CF RID: 31183 RVA: 0x002DA304 File Offset: 0x002D8504
		public void Init(NavGrid.Transition transition, float default_speed)
		{
			this.x = transition.x;
			this.y = transition.y;
			this.isLooping = transition.isLooping;
			this.start = transition.start;
			this.end = transition.end;
			this.preAnim = transition.preAnim;
			this.anim = transition.anim;
			this.speed = default_speed;
			this.animSpeed = transition.animSpeed;
			this.navGridTransition = transition;
		}

		// Token: 0x060079D0 RID: 31184 RVA: 0x002DA38C File Offset: 0x002D858C
		public void Copy(Navigator.ActiveTransition other)
		{
			this.x = other.x;
			this.y = other.y;
			this.isLooping = other.isLooping;
			this.start = other.start;
			this.end = other.end;
			this.preAnim = other.preAnim;
			this.anim = other.anim;
			this.speed = other.speed;
			this.animSpeed = other.animSpeed;
			this.navGridTransition = other.navGridTransition;
		}

		// Token: 0x04005C7F RID: 23679
		public int x;

		// Token: 0x04005C80 RID: 23680
		public int y;

		// Token: 0x04005C81 RID: 23681
		public bool isLooping;

		// Token: 0x04005C82 RID: 23682
		public NavType start;

		// Token: 0x04005C83 RID: 23683
		public NavType end;

		// Token: 0x04005C84 RID: 23684
		public HashedString preAnim;

		// Token: 0x04005C85 RID: 23685
		public HashedString anim;

		// Token: 0x04005C86 RID: 23686
		public float speed;

		// Token: 0x04005C87 RID: 23687
		public float animSpeed = 1f;

		// Token: 0x04005C88 RID: 23688
		public Func<bool> isCompleteCB;

		// Token: 0x04005C89 RID: 23689
		public NavGrid.Transition navGridTransition;
	}

	// Token: 0x02001180 RID: 4480
	public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
	{
		// Token: 0x060079D2 RID: 31186 RVA: 0x002DA424 File Offset: 0x002D8624
		public StatesInstance(Navigator master) : base(master)
		{
		}
	}

	// Token: 0x02001181 RID: 4481
	public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
	{
		// Token: 0x060079D3 RID: 31187 RVA: 0x002DA430 File Offset: 0x002D8630
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal.stopped;
			this.saveHistory = true;
			this.normal.ParamTransition<bool>(this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue).Update("NavigatorProber", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.Sim4000ms(dt);
			}, UpdateRate.SIM_4000ms, false);
			this.normal.moving.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementWakeUp);
			}).Update("UpdateNavigator", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.SimEveryTick(dt);
			}, UpdateRate.SIM_EVERY_TICK, true).Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementSleep);
			});
			this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached, null).GoTo(this.normal.stopped);
			this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed, null).GoTo(this.normal.stopped);
			this.normal.stopped.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.master.SubscribeUnstuckFunctions();
			}).DoNothing().Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.master.UnsubscribeUnstuckFunctions();
			});
			this.paused.ParamTransition<bool>(this.isPaused, this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
		}

		// Token: 0x04005C8A RID: 23690
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;

		// Token: 0x04005C8B RID: 23691
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);

		// Token: 0x04005C8C RID: 23692
		public Navigator.States.NormalStates normal;

		// Token: 0x04005C8D RID: 23693
		public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

		// Token: 0x02002098 RID: 8344
		public class NormalStates : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
		{
			// Token: 0x0400919D RID: 37277
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;

			// Token: 0x0400919E RID: 37278
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;

			// Token: 0x0400919F RID: 37279
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;

			// Token: 0x040091A0 RID: 37280
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
		}
	}

	// Token: 0x02001182 RID: 4482
	public struct PathProbeTask : IWorkItem<object>
	{
		// Token: 0x060079D5 RID: 31189 RVA: 0x002DA5F0 File Offset: 0x002D87F0
		public PathProbeTask(Navigator navigator)
		{
			this.navigator = navigator;
			this.cell = -1;
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x002DA600 File Offset: 0x002D8800
		public void Update()
		{
			this.cell = Grid.PosToCell(this.navigator);
			this.navigator.abilities.Refresh();
		}

		// Token: 0x060079D7 RID: 31191 RVA: 0x002DA624 File Offset: 0x002D8824
		public void Run(object sharedData)
		{
			this.navigator.PathProber.UpdateProbe(this.navigator.NavGrid, this.cell, this.navigator.CurrentNavType, this.navigator.abilities, this.navigator.flags);
		}

		// Token: 0x04005C8E RID: 23694
		private int cell;

		// Token: 0x04005C8F RID: 23695
		private Navigator navigator;
	}
}
