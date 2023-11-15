﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200067F RID: 1663
public class ReorderableBuilding : KMonoBehaviour
{
	// Token: 0x06002C52 RID: 11346 RVA: 0x000EB2E8 File Offset: 0x000E94E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		GameObject gameObject = new GameObject();
		gameObject.name = "ReorderArm";
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.SetLocalPosition(Vector3.up * Grid.CellSizeInMeters * ((float)base.GetComponent<Building>().Def.HeightInCells / 2f - 0.5f));
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(false);
		this.reorderArmController = gameObject.AddComponent<KBatchedAnimController>();
		this.reorderArmController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("rocket_module_switching_arm_kanim")
		};
		this.reorderArmController.initialAnim = "off";
		gameObject.SetActive(true);
		int cell = Grid.PosToCell(gameObject);
		this.ShowReorderArm(Grid.IsValidCell(cell));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			LaunchPad currentPad = component.CraftInterface.CurrentPad;
			if (currentPad != null)
			{
				this.m_animLink = new KAnimLink(currentPad.GetComponent<KAnimControllerBase>(), this.reorderArmController);
			}
		}
		if (this.m_animLink == null)
		{
			this.m_animLink = new KAnimLink(base.GetComponent<KAnimControllerBase>(), this.reorderArmController);
		}
	}

	// Token: 0x06002C53 RID: 11347 RVA: 0x000EB471 File Offset: 0x000E9671
	private void OnCancel(object data)
	{
		if (base.GetComponent<BuildingUnderConstruction>() != null && !this.cancelShield && !ReorderableBuilding.toBeRemoved.Contains(this))
		{
			ReorderableBuilding.toBeRemoved.Add(this);
		}
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000EB4A4 File Offset: 0x000E96A4
	public GameObject AddModule(BuildingDef def, IList<Tag> buildMaterials)
	{
		if (Assets.GetPrefab(base.GetComponent<KPrefabID>().PrefabID()).GetComponent<ReorderableBuilding>().buildConditions.Find((SelectModuleCondition match) => match is TopOnly) == null)
		{
			if (def.BuildingComplete.GetComponent<ReorderableBuilding>().buildConditions.Find((SelectModuleCondition match) => match is EngineOnBottom) == null)
			{
				return this.AddModuleAbove(def, buildMaterials);
			}
		}
		return this.AddModuleBelow(def, buildMaterials);
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000EB538 File Offset: 0x000E9738
	private GameObject AddModuleAbove(BuildingDef def, IList<Tag> buildMaterials)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null)
		{
			return null;
		}
		BuildingAttachPoint.HardPoint hardPoint = component.points[0];
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), hardPoint.position);
		int heightInCells = def.HeightInCells;
		if (hardPoint.attachedBuilding != null)
		{
			if (!hardPoint.attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(heightInCells, null))
			{
				return null;
			}
			hardPoint.attachedBuilding.GetComponent<ReorderableBuilding>().MoveVertical(heightInCells);
		}
		return this.AddModuleCommon(def, buildMaterials, cell);
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000EB5C0 File Offset: 0x000E97C0
	private GameObject AddModuleBelow(BuildingDef def, IList<Tag> buildMaterials)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int heightInCells = def.HeightInCells;
		if (!this.CanMoveVertically(heightInCells, null))
		{
			return null;
		}
		this.MoveVertical(heightInCells);
		return this.AddModuleCommon(def, buildMaterials, cell);
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x000EB5FC File Offset: 0x000E97FC
	private GameObject AddModuleCommon(BuildingDef def, IList<Tag> buildMaterials, int cell)
	{
		GameObject gameObject;
		if (DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild))
		{
			gameObject = def.Build(cell, Orientation.Neutral, null, buildMaterials, 273.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject = def.TryPlace(null, Grid.CellToPosCBC(cell, def.SceneLayer), Orientation.Neutral, buildMaterials, 0);
		}
		ReorderableBuilding.RebuildNetworks();
		this.RocketSpecificPostAdd(gameObject, cell);
		return gameObject;
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x000EB670 File Offset: 0x000E9870
	private void RocketSpecificPostAdd(GameObject obj, int cell)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		RocketModuleCluster component2 = obj.GetComponent<RocketModuleCluster>();
		if (component != null && component2 != null)
		{
			component.CraftInterface.AddModule(component2);
		}
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000EB6AC File Offset: 0x000E98AC
	public void RemoveModule()
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		AttachableBuilding attachableBuilding = null;
		if (component != null)
		{
			attachableBuilding = component.points[0].attachedBuilding;
		}
		int heightInCells = base.GetComponent<Building>().Def.HeightInCells;
		if (base.GetComponent<Deconstructable>() != null)
		{
			base.GetComponent<Deconstructable>().CompleteWork(null);
		}
		if (base.GetComponent<BuildingUnderConstruction>() != null)
		{
			this.DeleteObject();
		}
		Building component2 = base.GetComponent<Building>();
		component2.Def.UnmarkArea(Grid.PosToCell(this), component2.Orientation, component2.Def.ObjectLayer, base.gameObject);
		if (attachableBuilding != null)
		{
			attachableBuilding.GetComponent<ReorderableBuilding>().MoveVertical(-heightInCells);
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x000EB764 File Offset: 0x000E9964
	public void LateUpdate()
	{
		this.cancelShield = false;
		ReorderableBuilding.ProcessToBeRemoved();
		if (this.reorderingAnimUnderway)
		{
			float num = 10f;
			if (Mathf.Abs(this.animController.Offset.y) < Time.unscaledDeltaTime * num)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, 0f, this.animController.Offset.z);
				this.reorderingAnimUnderway = false;
				string s = base.GetComponent<Building>().Def.WidthInCells.ToString() + "x" + base.GetComponent<Building>().Def.HeightInCells.ToString() + "_ungrab";
				if (!this.reorderArmController.HasAnimation(s))
				{
					s = "3x3_ungrab";
				}
				this.reorderArmController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
				this.reorderArmController.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
				this.loopingSounds.StopSound(GlobalAssets.GetSound(this.reorderSound, false));
			}
			else if (this.animController.Offset.y > 0f)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y - Time.unscaledDeltaTime * num, this.animController.Offset.z);
			}
			else if (this.animController.Offset.y < 0f)
			{
				this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + Time.unscaledDeltaTime * num, this.animController.Offset.z);
			}
			this.reorderArmController.Offset = this.animController.Offset;
		}
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000EB96C File Offset: 0x000E9B6C
	private static void ProcessToBeRemoved()
	{
		if (ReorderableBuilding.toBeRemoved.Count > 0)
		{
			ReorderableBuilding.toBeRemoved.Sort(delegate(ReorderableBuilding a, ReorderableBuilding b)
			{
				if (a.transform.position.y < b.transform.position.y)
				{
					return -1;
				}
				return 1;
			});
			for (int i = 0; i < ReorderableBuilding.toBeRemoved.Count; i++)
			{
				ReorderableBuilding.toBeRemoved[i].RemoveModule();
			}
			ReorderableBuilding.toBeRemoved.Clear();
		}
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x000EB9E0 File Offset: 0x000E9BE0
	public void MoveVertical(int amount)
	{
		if (amount == 0)
		{
			return;
		}
		this.cancelShield = true;
		List<GameObject> list = new List<GameObject>();
		list.Add(base.gameObject);
		AttachableBuilding.GetAttachedAbove(base.GetComponent<AttachableBuilding>(), ref list);
		if (amount > 0)
		{
			list.Reverse();
		}
		foreach (GameObject gameObject in list)
		{
			ReorderableBuilding.UnmarkBuilding(gameObject, null);
			int cell = Grid.OffsetCell(Grid.PosToCell(gameObject), 0, amount);
			gameObject.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
			ReorderableBuilding.MarkBuilding(gameObject, null);
			gameObject.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float)(-(float)amount));
		}
		if (amount > 0)
		{
			foreach (GameObject gameObject2 in list)
			{
				gameObject2.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
			}
		}
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000EBADC File Offset: 0x000E9CDC
	public void SwapWithAbove(bool selectOnComplete = true)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null || component.points[0].attachedBuilding == null)
		{
			return;
		}
		int num = Grid.PosToCell(base.gameObject);
		ReorderableBuilding.UnmarkBuilding(base.gameObject, null);
		AttachableBuilding attachedBuilding = component.points[0].attachedBuilding;
		BuildingAttachPoint component2 = attachedBuilding.GetComponent<BuildingAttachPoint>();
		AttachableBuilding aboveBuilding = (component2 != null) ? component2.points[0].attachedBuilding : null;
		ReorderableBuilding.UnmarkBuilding(attachedBuilding.gameObject, aboveBuilding);
		Building component3 = attachedBuilding.GetComponent<Building>();
		int cell = num;
		attachedBuilding.transform.SetPosition(Grid.CellToPos(cell, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
		ReorderableBuilding.MarkBuilding(attachedBuilding.gameObject, null);
		int cell2 = Grid.OffsetCell(num, 0, component3.Def.HeightInCells);
		base.transform.SetPosition(Grid.CellToPos(cell2, CellAlignment.Bottom, Grid.SceneLayer.BuildingFront));
		ReorderableBuilding.MarkBuilding(base.gameObject, aboveBuilding);
		ReorderableBuilding.RebuildNetworks();
		this.ApplyAnimOffset((float)(-(float)component3.Def.HeightInCells));
		Building component4 = base.GetComponent<Building>();
		component3.GetComponent<ReorderableBuilding>().ApplyAnimOffset((float)component4.Def.HeightInCells);
		if (selectOnComplete)
		{
			SelectTool.Instance.Select(component4.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000EBC1F File Offset: 0x000E9E1F
	protected override void OnCleanUp()
	{
		if (base.GetComponent<BuildingUnderConstruction>() == null && !this.HasTag(GameTags.RocketInSpace))
		{
			this.RemoveModule();
		}
		if (this.m_animLink != null)
		{
			this.m_animLink.Unregister();
		}
		base.OnCleanUp();
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000EBC5C File Offset: 0x000E9E5C
	private void ApplyAnimOffset(float amount)
	{
		this.animController.Offset = new Vector3(this.animController.Offset.x, this.animController.Offset.y + amount, this.animController.Offset.z);
		this.reorderArmController.Offset = this.animController.Offset;
		string s = base.GetComponent<Building>().Def.WidthInCells.ToString() + "x" + base.GetComponent<Building>().Def.HeightInCells.ToString() + "_grab";
		if (!this.reorderArmController.HasAnimation(s))
		{
			s = "3x3_grab";
		}
		this.reorderArmController.Play(s, KAnim.PlayMode.Once, 1f, 0f);
		this.reorderArmController.onAnimComplete += this.StartReorderingAnim;
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000EBD48 File Offset: 0x000E9F48
	private void StartReorderingAnim(HashedString data)
	{
		this.loopingSounds.StartSound(GlobalAssets.GetSound(this.reorderSound, false));
		this.reorderingAnimUnderway = true;
		this.reorderArmController.onAnimComplete -= this.StartReorderingAnim;
		base.gameObject.Trigger(-1447108533, null);
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000EBD9C File Offset: 0x000E9F9C
	public void SwapWithBelow(bool selectOnComplete = true)
	{
		if (base.GetComponent<AttachableBuilding>() == null || base.GetComponent<AttachableBuilding>().GetAttachedTo() == null)
		{
			return;
		}
		base.GetComponent<AttachableBuilding>().GetAttachedTo().GetComponent<ReorderableBuilding>().SwapWithAbove(!selectOnComplete);
		if (selectOnComplete)
		{
			SelectTool.Instance.Select(base.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000EBDF8 File Offset: 0x000E9FF8
	public bool CanMoveVertically(int moveAmount, GameObject ignoreBuilding = null)
	{
		if (moveAmount == 0)
		{
			return true;
		}
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		AttachableBuilding component2 = base.GetComponent<AttachableBuilding>();
		if (moveAmount > 0)
		{
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.gameObject != ignoreBuilding && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount, null))
			{
				return false;
			}
		}
		else if (component2 != null)
		{
			BuildingAttachPoint attachedTo = component2.GetAttachedTo();
			if (attachedTo != null && attachedTo.gameObject != ignoreBuilding && !component2.GetAttachedTo().GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount, null))
			{
				return false;
			}
		}
		foreach (CellOffset offset in this.GetOccupiedOffsets())
		{
			if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset), 0, moveAmount), base.gameObject))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000EBF0C File Offset: 0x000EA10C
	public static bool CheckCellClear(int checkCell, GameObject ignoreObject = null)
	{
		return Grid.IsValidCell(checkCell) && Grid.IsValidBuildingCell(checkCell) && !Grid.Solid[checkCell] && Grid.WorldIdx[checkCell] != byte.MaxValue && (!(Grid.Objects[checkCell, 1] != null) || !(Grid.Objects[checkCell, 1] != ignoreObject) || !(Grid.Objects[checkCell, 1].GetComponent<ReorderableBuilding>() == null));
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000EBF8C File Offset: 0x000EA18C
	public GameObject ConvertModule(BuildingDef toModule, IList<Tag> materials)
	{
		int cell = Grid.PosToCell(base.gameObject);
		int num = toModule.HeightInCells - base.GetComponent<Building>().Def.HeightInCells;
		base.gameObject.GetComponent<Building>();
		BuildingAttachPoint component = base.gameObject.GetComponent<BuildingAttachPoint>();
		GameObject gameObject = null;
		if (component != null && component.points[0].attachedBuilding != null)
		{
			gameObject = component.points[0].attachedBuilding.gameObject;
			component.points[0].attachedBuilding = null;
			Components.BuildingAttachPoints.Remove(component);
		}
		ReorderableBuilding.UnmarkBuilding(base.gameObject, null);
		if (num != 0 && gameObject != null)
		{
			gameObject.GetComponent<ReorderableBuilding>().MoveVertical(num);
		}
		string text;
		if (!DebugHandler.InstantBuildMode && !toModule.IsValidPlaceLocation(base.gameObject, cell, Orientation.Neutral, out text))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, base.transform, 1.5f, false);
			if (num != 0 && gameObject != null)
			{
				num *= -1;
				gameObject.GetComponent<ReorderableBuilding>().MoveVertical(num);
			}
			ReorderableBuilding.MarkBuilding(base.gameObject, (gameObject != null) ? gameObject.GetComponent<AttachableBuilding>() : null);
			if (component != null && gameObject != null)
			{
				component.points[0].attachedBuilding = gameObject.GetComponent<AttachableBuilding>();
				Components.BuildingAttachPoints.Add(component);
			}
			return null;
		}
		if (materials == null)
		{
			materials = toModule.DefaultElements();
		}
		GameObject gameObject2;
		if (DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild))
		{
			gameObject2 = toModule.Build(cell, Orientation.Neutral, null, materials, 273.15f, true, GameClock.Instance.GetTime());
		}
		else
		{
			gameObject2 = toModule.TryPlace(base.gameObject, Grid.CellToPosCBC(cell, toModule.SceneLayer), Orientation.Neutral, materials, 0);
		}
		RocketModuleCluster component2 = base.GetComponent<RocketModuleCluster>();
		RocketModuleCluster component3 = gameObject2.GetComponent<RocketModuleCluster>();
		if (component2 != null && component3 != null)
		{
			component2.CraftInterface.AddModule(component3);
		}
		Deconstructable component4 = base.GetComponent<Deconstructable>();
		if (component4 != null)
		{
			component4.SetAllowDeconstruction(true);
			component4.ForceDestroyAndGetMaterials();
		}
		else
		{
			Util.KDestroyGameObject(base.gameObject);
		}
		return gameObject2;
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000EC1D4 File Offset: 0x000EA3D4
	private CellOffset[] GetOccupiedOffsets()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			return component.OccupiedCellsOffsets;
		}
		return base.GetComponent<BuildingUnderConstruction>().Def.PlacementOffsets;
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x000EC208 File Offset: 0x000EA408
	public bool CanChangeModule()
	{
		if (base.GetComponent<BuildingUnderConstruction>() != null)
		{
			string prefabID = base.GetComponent<BuildingUnderConstruction>().Def.PrefabID;
		}
		else
		{
			string prefabID2 = base.GetComponent<Building>().Def.PrefabID;
		}
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			if (component.CraftInterface != null)
			{
				if (component.CraftInterface.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
				{
					return false;
				}
			}
			else if (component.conditionManager != null && SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(component.conditionManager).state != Spacecraft.MissionState.Grounded)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x000EC2A1 File Offset: 0x000EA4A1
	public bool CanRemoveModule()
	{
		return true;
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x000EC2A4 File Offset: 0x000EA4A4
	public bool CanSwapUp(bool alsoCheckAboveCanSwapDown = true)
	{
		BuildingAttachPoint component = base.GetComponent<BuildingAttachPoint>();
		if (component == null)
		{
			return false;
		}
		if (base.GetComponent<AttachableBuilding>() == null || base.GetComponent<RocketEngineCluster>() != null)
		{
			return false;
		}
		AttachableBuilding attachedBuilding = component.points[0].attachedBuilding;
		return !(attachedBuilding == null) && !(attachedBuilding.GetComponent<BuildingAttachPoint>() == null) && !attachedBuilding.HasTag(GameTags.NoseRocketModule) && this.CanMoveVertically(attachedBuilding.GetComponent<Building>().Def.HeightInCells, attachedBuilding.gameObject) && (!alsoCheckAboveCanSwapDown || attachedBuilding.GetComponent<ReorderableBuilding>().CanSwapDown(false));
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x000EC350 File Offset: 0x000EA550
	public bool CanSwapDown(bool alsoCheckBelowCanSwapUp = true)
	{
		if (base.gameObject.HasTag(GameTags.NoseRocketModule))
		{
			return false;
		}
		AttachableBuilding component = base.GetComponent<AttachableBuilding>();
		if (component == null)
		{
			return false;
		}
		BuildingAttachPoint attachedTo = component.GetAttachedTo();
		return !(attachedTo == null) && !(base.GetComponent<BuildingAttachPoint>() == null) && !(attachedTo.GetComponent<AttachableBuilding>() == null) && !(attachedTo.GetComponent<RocketEngineCluster>() != null) && this.CanMoveVertically(attachedTo.GetComponent<Building>().Def.HeightInCells * -1, attachedTo.gameObject) && (!alsoCheckBelowCanSwapUp || attachedTo.GetComponent<ReorderableBuilding>().CanSwapUp(false));
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x000EC3F9 File Offset: 0x000EA5F9
	public void ShowReorderArm(bool show)
	{
		if (this.reorderArmController != null)
		{
			this.reorderArmController.gameObject.SetActive(show);
		}
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000EC41C File Offset: 0x000EA61C
	private static void RebuildNetworks()
	{
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		Game.Instance.gasConduitSystem.ForceRebuildNetworks();
		Game.Instance.liquidConduitSystem.ForceRebuildNetworks();
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		Game.Instance.solidConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000EC474 File Offset: 0x000EA674
	private static void UnmarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
	{
		int cell = Grid.PosToCell(go);
		Building component = go.GetComponent<Building>();
		component.Def.UnmarkArea(cell, component.Orientation, component.Def.ObjectLayer, go);
		AttachableBuilding component2 = go.GetComponent<AttachableBuilding>();
		if (component2 != null)
		{
			component2.RegisterWithAttachPoint(false);
		}
		if (aboveBuilding != null)
		{
			aboveBuilding.RegisterWithAttachPoint(false);
		}
		RocketModule component3 = go.GetComponent<RocketModule>();
		if (component3 != null)
		{
			component3.DeregisterComponents();
		}
		RocketConduitSender[] components = go.GetComponents<RocketConduitSender>();
		if (components.Length != 0)
		{
			RocketConduitSender[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RemoveConduitPortFromNetwork();
			}
		}
		RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
		if (components2.Length != 0)
		{
			RocketConduitReceiver[] array2 = components2;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].RemoveConduitPortFromNetwork();
			}
		}
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x000EC548 File Offset: 0x000EA748
	private static void MarkBuilding(GameObject go, AttachableBuilding aboveBuilding)
	{
		int cell = Grid.PosToCell(go);
		Building component = go.GetComponent<Building>();
		component.Def.MarkArea(cell, component.Orientation, component.Def.ObjectLayer, go);
		if (component.GetComponent<OccupyArea>() != null)
		{
			component.GetComponent<OccupyArea>().UpdateOccupiedArea();
		}
		LogicPorts component2 = component.GetComponent<LogicPorts>();
		if (component2 && go.GetComponent<BuildingComplete>() != null)
		{
			component2.OnMove();
		}
		component.GetComponent<AttachableBuilding>().RegisterWithAttachPoint(true);
		if (aboveBuilding != null)
		{
			aboveBuilding.RegisterWithAttachPoint(true);
		}
		RocketModule component3 = go.GetComponent<RocketModule>();
		if (component3 != null)
		{
			component3.RegisterComponents();
		}
		VerticalModuleTiler component4 = go.GetComponent<VerticalModuleTiler>();
		if (component4 != null)
		{
			component4.PostReorderMove();
		}
		RocketConduitSender[] components = go.GetComponents<RocketConduitSender>();
		if (components.Length != 0)
		{
			RocketConduitSender[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddConduitPortToNetwork();
			}
		}
		RocketConduitReceiver[] components2 = go.GetComponents<RocketConduitReceiver>();
		if (components2.Length != 0)
		{
			RocketConduitReceiver[] array2 = components2;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].AddConduitPortToNetwork();
			}
		}
	}

	// Token: 0x04001A26 RID: 6694
	private bool cancelShield;

	// Token: 0x04001A27 RID: 6695
	private bool reorderingAnimUnderway;

	// Token: 0x04001A28 RID: 6696
	private KBatchedAnimController animController;

	// Token: 0x04001A29 RID: 6697
	public List<SelectModuleCondition> buildConditions = new List<SelectModuleCondition>();

	// Token: 0x04001A2A RID: 6698
	private KBatchedAnimController reorderArmController;

	// Token: 0x04001A2B RID: 6699
	private KAnimLink m_animLink;

	// Token: 0x04001A2C RID: 6700
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04001A2D RID: 6701
	private string reorderSound = "RocketModuleSwitchingArm_moving_LP";

	// Token: 0x04001A2E RID: 6702
	private static List<ReorderableBuilding> toBeRemoved = new List<ReorderableBuilding>();

	// Token: 0x02001388 RID: 5000
	public enum MoveSource
	{
		// Token: 0x040062C8 RID: 25288
		Push,
		// Token: 0x040062C9 RID: 25289
		Pull
	}
}
