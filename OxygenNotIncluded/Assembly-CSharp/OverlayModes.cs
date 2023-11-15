using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A74 RID: 2676
public abstract class OverlayModes
{
	// Token: 0x02001915 RID: 6421
	public class GasConduits : OverlayModes.ConduitMode
	{
		// Token: 0x060093AD RID: 37805 RVA: 0x00330181 File Offset: 0x0032E381
		public override HashedString ViewMode()
		{
			return OverlayModes.GasConduits.ID;
		}

		// Token: 0x060093AE RID: 37806 RVA: 0x00330188 File Offset: 0x0032E388
		public override string GetSoundName()
		{
			return "GasVent";
		}

		// Token: 0x060093AF RID: 37807 RVA: 0x0033018F File Offset: 0x0032E38F
		public GasConduits() : base(OverlayScreen.GasVentIDs)
		{
		}

		// Token: 0x0400740F RID: 29711
		public static readonly HashedString ID = "GasConduit";
	}

	// Token: 0x02001916 RID: 6422
	public class LiquidConduits : OverlayModes.ConduitMode
	{
		// Token: 0x060093B1 RID: 37809 RVA: 0x003301AD File Offset: 0x0032E3AD
		public override HashedString ViewMode()
		{
			return OverlayModes.LiquidConduits.ID;
		}

		// Token: 0x060093B2 RID: 37810 RVA: 0x003301B4 File Offset: 0x0032E3B4
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x060093B3 RID: 37811 RVA: 0x003301BB File Offset: 0x0032E3BB
		public LiquidConduits() : base(OverlayScreen.LiquidVentIDs)
		{
		}

		// Token: 0x04007410 RID: 29712
		public static readonly HashedString ID = "LiquidConduit";
	}

	// Token: 0x02001917 RID: 6423
	public abstract class ConduitMode : OverlayModes.Mode
	{
		// Token: 0x060093B5 RID: 37813 RVA: 0x003301DC File Offset: 0x0032E3DC
		public ConduitMode(ICollection<Tag> ids)
		{
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = ids;
		}

		// Token: 0x060093B6 RID: 37814 RVA: 0x00330264 File Offset: 0x0032E464
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x060093B7 RID: 37815 RVA: 0x003302C0 File Offset: 0x0032E4C0
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x060093B8 RID: 37816 RVA: 0x003302F4 File Offset: 0x0032E4F4
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x060093B9 RID: 37817 RVA: 0x00330340 File Offset: 0x0032E540
		public override void Disable()
		{
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = saveLoadRoot.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x00330430 File Offset: 0x0032E630
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
				Vector3 position = root.transform.GetPosition();
				position.z = defaultDepth;
				root.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			});
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
				if (saveLoadRoot.GetComponent<Conduit>() != null)
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.conduitTargetLayer, null, null);
				}
				else
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
					{
						Vector3 position = root.transform.GetPosition();
						float z = position.z;
						KPrefabID component3 = root.GetComponent<KPrefabID>();
						if (component3 != null)
						{
							if (component3.HasTag(GameTags.OverlayInFrontOfConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) - 0.2f;
							}
							else if (component3.HasTag(GameTags.OverlayBehindConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) + 0.2f;
							}
						}
						position.z = z;
						root.transform.SetPosition(position);
						KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							this.TriggerResorting(componentsInChildren[i]);
						}
					}, null);
				}
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component != null)
				{
					int networkCell = component.GetNetworkCell();
					UtilityNetworkManager<FlowUtilityNetwork, Vent> mgr = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitSystem : Game.Instance.gasConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, mgr, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			Game.ConduitVisInfo conduitVisInfo = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitVisInfo : Game.Instance.gasConduitVisInfo;
			foreach (SaveLoadRoot saveLoadRoot2 in this.layerTargets)
			{
				if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<IBridgedNetworkItem>() != null)
				{
					BuildingDef def = saveLoadRoot2.GetComponent<Building>().Def;
					Color32 colorByName;
					if (def.ThermalConductivity == 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayTintName);
					}
					else if (def.ThermalConductivity < 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayInsulatedTintName);
					}
					else
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayRadiantTintName);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component2 = saveLoadRoot2.GetComponent<IBridgedNetworkItem>();
						if (component2 != null && component2.IsConnectedToNetworks(this.connectedNetworks))
						{
							colorByName.r = (byte)((float)colorByName.r * num);
							colorByName.g = (byte)((float)colorByName.g * num);
							colorByName.b = (byte)((float)colorByName.b * num);
						}
					}
					saveLoadRoot2.GetComponent<KBatchedAnimController>().TintColour = colorByName;
				}
			}
		}

		// Token: 0x060093BB RID: 37819 RVA: 0x00330764 File Offset: 0x0032E964
		private void TriggerResorting(KBatchedAnimController kbac)
		{
			if (kbac.enabled)
			{
				kbac.enabled = false;
				kbac.enabled = true;
			}
		}

		// Token: 0x060093BC RID: 37820 RVA: 0x0033077C File Offset: 0x0032E97C
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						IBridgedNetworkItem component = networkItem.GameObject.GetComponent<IBridgedNetworkItem>();
						if (component != null)
						{
							component.AddNetworks(networks);
						}
					}
				}
			}
		}

		// Token: 0x04007411 RID: 29713
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04007412 RID: 29714
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04007413 RID: 29715
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04007414 RID: 29716
		private List<int> visited = new List<int>();

		// Token: 0x04007415 RID: 29717
		private ICollection<Tag> targetIDs;

		// Token: 0x04007416 RID: 29718
		private int objectTargetLayer;

		// Token: 0x04007417 RID: 29719
		private int conduitTargetLayer;

		// Token: 0x04007418 RID: 29720
		private int cameraLayerMask;

		// Token: 0x04007419 RID: 29721
		private int selectionMask;
	}

	// Token: 0x02001918 RID: 6424
	public class Crop : OverlayModes.BasePlantMode
	{
		// Token: 0x060093BF RID: 37823 RVA: 0x00330960 File Offset: 0x0032EB60
		public override HashedString ViewMode()
		{
			return OverlayModes.Crop.ID;
		}

		// Token: 0x060093C0 RID: 37824 RVA: 0x00330967 File Offset: 0x0032EB67
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x060093C1 RID: 37825 RVA: 0x00330970 File Offset: 0x0032EB70
		public Crop(Canvas ui_root, GameObject harvestable_notification_prefab)
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[3];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropHalted, delegate(KMonoBehaviour h)
			{
				WiltCondition component = h.GetComponent<WiltCondition>();
				return component != null && component.IsWilting();
			});
			array[1] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrowing, (KMonoBehaviour h) => !(h as HarvestDesignatable).CanBeHarvested());
			array[2] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrown, (KMonoBehaviour h) => (h as HarvestDesignatable).CanBeHarvested());
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
			this.uiRoot = ui_root;
			this.harvestableNotificationPrefab = harvestable_notification_prefab;
		}

		// Token: 0x060093C2 RID: 37826 RVA: 0x00330A8C File Offset: 0x0032EC8C
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.CROP.FULLY_GROWN, UI.OVERLAYS.CROP.TOOLTIPS.FULLY_GROWN, GlobalAssets.Instance.colorSet.cropGrown, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWING, UI.OVERLAYS.CROP.TOOLTIPS.GROWING, GlobalAssets.Instance.colorSet.cropGrowing, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWTH_HALTED, UI.OVERLAYS.CROP.TOOLTIPS.GROWTH_HALTED, GlobalAssets.Instance.colorSet.cropHalted, null, null, true)
			};
		}

		// Token: 0x060093C3 RID: 37827 RVA: 0x00330B40 File Offset: 0x0032ED40
		public override void Update()
		{
			this.updateCropInfo.Clear();
			this.freeHarvestableNotificationIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (HarvestDesignatable harvestDesignatable in this.layerTargets)
			{
				Vector2I vector2I3 = Grid.PosToXY(harvestDesignatable.transform.GetPosition());
				if (vector2I <= vector2I3 && vector2I3 <= vector2I2)
				{
					this.AddCropUI(harvestDesignatable);
				}
			}
			foreach (OverlayModes.Crop.UpdateCropInfo updateCropInfo in this.updateCropInfo)
			{
				updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>().Refresh(updateCropInfo.harvestable);
			}
			for (int i = this.freeHarvestableNotificationIdx; i < this.harvestableNotificationList.Count; i++)
			{
				if (this.harvestableNotificationList[i].activeSelf)
				{
					this.harvestableNotificationList[i].SetActive(false);
				}
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x060093C4 RID: 37828 RVA: 0x00330D30 File Offset: 0x0032EF30
		public override void Disable()
		{
			this.DisableHarvestableUINotifications();
			base.Disable();
		}

		// Token: 0x060093C5 RID: 37829 RVA: 0x00330D40 File Offset: 0x0032EF40
		private void DisableHarvestableUINotifications()
		{
			this.freeHarvestableNotificationIdx = 0;
			foreach (GameObject gameObject in this.harvestableNotificationList)
			{
				gameObject.SetActive(false);
			}
			this.updateCropInfo.Clear();
		}

		// Token: 0x060093C6 RID: 37830 RVA: 0x00330DA4 File Offset: 0x0032EFA4
		public GameObject GetFreeCropUI()
		{
			GameObject gameObject;
			if (this.freeHarvestableNotificationIdx < this.harvestableNotificationList.Count)
			{
				gameObject = this.harvestableNotificationList[this.freeHarvestableNotificationIdx];
				if (!gameObject.gameObject.activeSelf)
				{
					gameObject.gameObject.SetActive(true);
				}
				this.freeHarvestableNotificationIdx++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.harvestableNotificationPrefab.gameObject, this.uiRoot.transform.gameObject, false);
				this.harvestableNotificationList.Add(gameObject);
				this.freeHarvestableNotificationIdx++;
			}
			return gameObject;
		}

		// Token: 0x060093C7 RID: 37831 RVA: 0x00330E40 File Offset: 0x0032F040
		private void AddCropUI(HarvestDesignatable harvestable)
		{
			GameObject freeCropUI = this.GetFreeCropUI();
			OverlayModes.Crop.UpdateCropInfo item = new OverlayModes.Crop.UpdateCropInfo(harvestable, freeCropUI);
			Vector3 b = Grid.CellToPos(Grid.PosToCell(harvestable), 0.5f, -1.25f, 0f);
			freeCropUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b);
			this.updateCropInfo.Add(item);
		}

		// Token: 0x0400741A RID: 29722
		public static readonly HashedString ID = "Crop";

		// Token: 0x0400741B RID: 29723
		private Canvas uiRoot;

		// Token: 0x0400741C RID: 29724
		private List<OverlayModes.Crop.UpdateCropInfo> updateCropInfo = new List<OverlayModes.Crop.UpdateCropInfo>();

		// Token: 0x0400741D RID: 29725
		private int freeHarvestableNotificationIdx;

		// Token: 0x0400741E RID: 29726
		private List<GameObject> harvestableNotificationList = new List<GameObject>();

		// Token: 0x0400741F RID: 29727
		private GameObject harvestableNotificationPrefab;

		// Token: 0x04007420 RID: 29728
		private OverlayModes.ColorHighlightCondition[] highlightConditions;

		// Token: 0x02002210 RID: 8720
		private struct UpdateCropInfo
		{
			// Token: 0x0600ACAC RID: 44204 RVA: 0x00377E77 File Offset: 0x00376077
			public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI)
			{
				this.harvestable = harvestable;
				this.harvestableUI = harvestableUI;
			}

			// Token: 0x04009881 RID: 39041
			public HarvestDesignatable harvestable;

			// Token: 0x04009882 RID: 39042
			public GameObject harvestableUI;
		}
	}

	// Token: 0x02001919 RID: 6425
	public class Harvest : OverlayModes.BasePlantMode
	{
		// Token: 0x060093C9 RID: 37833 RVA: 0x00330EAC File Offset: 0x0032F0AC
		public override HashedString ViewMode()
		{
			return OverlayModes.Harvest.ID;
		}

		// Token: 0x060093CA RID: 37834 RVA: 0x00330EB3 File Offset: 0x0032F0B3
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x060093CB RID: 37835 RVA: 0x00330EBC File Offset: 0x0032F0BC
		public Harvest()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour harvestable) => new Color(0.65f, 0.65f, 0.65f, 0.65f), (KMonoBehaviour harvestable) => true);
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
		}

		// Token: 0x060093CC RID: 37836 RVA: 0x00330F28 File Offset: 0x0032F128
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x04007421 RID: 29729
		public static readonly HashedString ID = "HarvestWhenReady";

		// Token: 0x04007422 RID: 29730
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x0200191A RID: 6426
	public abstract class BasePlantMode : OverlayModes.Mode
	{
		// Token: 0x060093CE RID: 37838 RVA: 0x00331014 File Offset: 0x0032F214
		public BasePlantMode(ICollection<Tag> ids)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			this.targetIDs = ids;
		}

		// Token: 0x060093CF RID: 37839 RVA: 0x00331083 File Offset: 0x0032F283
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<HarvestDesignatable>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
		}

		// Token: 0x060093D0 RID: 37840 RVA: 0x003310C4 File Offset: 0x0032F2C4
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (!this.targetIDs.Contains(saveLoadTag))
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			this.partition.Add(component);
		}

		// Token: 0x060093D1 RID: 37841 RVA: 0x0033110C File Offset: 0x0032F30C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x060093D2 RID: 37842 RVA: 0x0033116C File Offset: 0x0032F36C
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			base.DisableHighlightTypeOverlay<HarvestDesignatable>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.partition.Clear();
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x04007423 RID: 29731
		protected UniformGrid<HarvestDesignatable> partition;

		// Token: 0x04007424 RID: 29732
		protected HashSet<HarvestDesignatable> layerTargets = new HashSet<HarvestDesignatable>();

		// Token: 0x04007425 RID: 29733
		protected ICollection<Tag> targetIDs;

		// Token: 0x04007426 RID: 29734
		protected int targetLayer;

		// Token: 0x04007427 RID: 29735
		private int cameraLayerMask;

		// Token: 0x04007428 RID: 29736
		private int selectionMask;
	}

	// Token: 0x0200191B RID: 6427
	public class Decor : OverlayModes.Mode
	{
		// Token: 0x060093D3 RID: 37843 RVA: 0x003311C3 File Offset: 0x0032F3C3
		public override HashedString ViewMode()
		{
			return OverlayModes.Decor.ID;
		}

		// Token: 0x060093D4 RID: 37844 RVA: 0x003311CA File Offset: 0x0032F3CA
		public override string GetSoundName()
		{
			return "Decor";
		}

		// Token: 0x060093D5 RID: 37845 RVA: 0x003311D4 File Offset: 0x0032F3D4
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.DECOR.HIGHDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.HIGHDECOR, GlobalAssets.Instance.colorSet.decorPositive, null, null, true),
				new LegendEntry(UI.OVERLAYS.DECOR.LOWDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.LOWDECOR, GlobalAssets.Instance.colorSet.decorNegative, null, null, true)
			};
		}

		// Token: 0x060093D6 RID: 37846 RVA: 0x00331254 File Offset: 0x0032F454
		public Decor()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour dp)
			{
				Color black = Color.black;
				Color b = Color.black;
				if (dp != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					float decorForCell = (dp as DecorProvider).GetDecorForCell(cell);
					if (decorForCell > 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
					}
					else if (decorForCell < 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
					}
					else if (dp.GetComponent<MonumentPart>() != null && dp.GetComponent<MonumentPart>().IsMonumentCompleted())
					{
						foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(dp.GetComponent<AttachableBuilding>()))
						{
							decorForCell = gameObject.GetComponent<DecorProvider>().GetDecorForCell(cell);
							if (decorForCell > 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
								break;
							}
							if (decorForCell < 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
								break;
							}
						}
					}
				}
				return Color.Lerp(black, b, 0.85f);
			}, (KMonoBehaviour dp) => SelectToolHoverTextCard.highlightedObjects.Contains(dp.gameObject));
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x060093D7 RID: 37847 RVA: 0x0033130C File Offset: 0x0032F50C
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<DecorProvider>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			foreach (Tag item in new Tag[]
			{
				new Tag("Tile"),
				new Tag("MeshTile"),
				new Tag("InsulationTile"),
				new Tag("GasPermeableMembrane"),
				new Tag("CarpetTile")
			})
			{
				this.targetIDs.Remove(item);
			}
			foreach (Tag item2 in OverlayScreen.GasVentIDs)
			{
				this.targetIDs.Remove(item2);
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				this.targetIDs.Remove(item3);
			}
			this.partition = OverlayModes.Mode.PopulatePartition<DecorProvider>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x060093D8 RID: 37848 RVA: 0x00331470 File Offset: 0x0032F670
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<DecorProvider>(this.layerTargets, vector2I, vector2I2, null);
			this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), this.workingTargets);
			for (int i = 0; i < this.workingTargets.Count; i++)
			{
				DecorProvider instance = this.workingTargets[i];
				base.AddTargetIfVisible<DecorProvider>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<DecorProvider>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
			this.workingTargets.Clear();
		}

		// Token: 0x060093D9 RID: 37849 RVA: 0x00331534 File Offset: 0x0032F734
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				DecorProvider component = item.GetComponent<DecorProvider>();
				if (component != null)
				{
					this.partition.Add(component);
				}
			}
		}

		// Token: 0x060093DA RID: 37850 RVA: 0x00331578 File Offset: 0x0032F778
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			DecorProvider component = item.GetComponent<DecorProvider>();
			if (component != null)
			{
				if (this.layerTargets.Contains(component))
				{
					this.layerTargets.Remove(component);
				}
				this.partition.Remove(component);
			}
		}

		// Token: 0x060093DB RID: 37851 RVA: 0x003315D4 File Offset: 0x0032F7D4
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<DecorProvider>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04007429 RID: 29737
		public static readonly HashedString ID = "Decor";

		// Token: 0x0400742A RID: 29738
		private UniformGrid<DecorProvider> partition;

		// Token: 0x0400742B RID: 29739
		private HashSet<DecorProvider> layerTargets = new HashSet<DecorProvider>();

		// Token: 0x0400742C RID: 29740
		private List<DecorProvider> workingTargets = new List<DecorProvider>();

		// Token: 0x0400742D RID: 29741
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x0400742E RID: 29742
		private int targetLayer;

		// Token: 0x0400742F RID: 29743
		private int cameraLayerMask;

		// Token: 0x04007430 RID: 29744
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x0200191C RID: 6428
	public class Disease : OverlayModes.Mode
	{
		// Token: 0x060093DD RID: 37853 RVA: 0x00331634 File Offset: 0x0032F834
		private static float CalculateHUE(Color32 colour)
		{
			byte b = Math.Max(colour.r, Math.Max(colour.g, colour.b));
			byte b2 = Math.Min(colour.r, Math.Min(colour.g, colour.b));
			float result = 0f;
			int num = (int)(b - b2);
			if (num == 0)
			{
				result = 0f;
			}
			else if (b == colour.r)
			{
				result = (float)(colour.g - colour.b) / (float)num % 6f;
			}
			else if (b == colour.g)
			{
				result = (float)(colour.b - colour.r) / (float)num + 2f;
			}
			else if (b == colour.b)
			{
				result = (float)(colour.r - colour.g) / (float)num + 4f;
			}
			return result;
		}

		// Token: 0x060093DE RID: 37854 RVA: 0x003316F8 File Offset: 0x0032F8F8
		public override HashedString ViewMode()
		{
			return OverlayModes.Disease.ID;
		}

		// Token: 0x060093DF RID: 37855 RVA: 0x003316FF File Offset: 0x0032F8FF
		public override string GetSoundName()
		{
			return "Disease";
		}

		// Token: 0x060093E0 RID: 37856 RVA: 0x00331708 File Offset: 0x0032F908
		public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab)
		{
			this.diseaseUIParent = diseaseUIParent;
			this.diseaseOverlayPrefab = diseaseOverlayPrefab;
			this.legendFilters = this.CreateDefaultFilters();
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x060093E1 RID: 37857 RVA: 0x00331790 File Offset: 0x0032F990
		public override void Enable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disease);
			CameraController.Instance.ToggleColouredOverlayView(true);
			Camera.main.cullingMask |= this.cameraLayerMask;
			base.RegisterSaveLoadListeners();
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(this.ViewMode());
				}
			}
		}

		// Token: 0x060093E2 RID: 37858 RVA: 0x00331828 File Offset: 0x0032FA28
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GASCONDUIT,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x060093E3 RID: 37859 RVA: 0x00331853 File Offset: 0x0032FA53
		public override void OnFiltersChanged()
		{
			Game.Instance.showGasConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, this.legendFilters);
			Game.Instance.showLiquidConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, this.legendFilters);
		}

		// Token: 0x060093E4 RID: 37860 RVA: 0x0033188C File Offset: 0x0032FA8C
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			if (item == null)
			{
				return;
			}
			KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			InfraredVisualizerComponents.ClearOverlayColour(component);
		}

		// Token: 0x060093E5 RID: 37861 RVA: 0x003318BA File Offset: 0x0032FABA
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
		}

		// Token: 0x060093E6 RID: 37862 RVA: 0x003318BC File Offset: 0x0032FABC
		public override void Disable()
		{
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(OverlayModes.None.ID);
				}
			}
			base.UnregisterSaveLoadListeners();
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			foreach (KMonoBehaviour kmonoBehaviour in this.layerTargets)
			{
				if (!(kmonoBehaviour == null))
				{
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
					Vector3 position = kmonoBehaviour.transform.GetPosition();
					position.z = defaultDepth;
					kmonoBehaviour.transform.SetPosition(position);
					KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
					component.enabled = false;
					component.enabled = true;
				}
			}
			CameraController.Instance.ToggleColouredOverlayView(false);
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			Game.Instance.showGasConduitDisease = false;
			Game.Instance.showLiquidConduitDisease = false;
			this.freeDiseaseUI = 0;
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.gameObject.SetActive(false);
			}
			this.updateDiseaseInfo.Clear();
			this.privateTargets.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x060093E7 RID: 37863 RVA: 0x00331A60 File Offset: 0x0032FC60
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<OverlayModes.Disease.DiseaseSortInfo> list2 = new List<OverlayModes.Disease.DiseaseSortInfo>();
			foreach (Klei.AI.Disease d in Db.Get().Diseases.resources)
			{
				list2.Add(new OverlayModes.Disease.DiseaseSortInfo(d));
			}
			list2.Sort((OverlayModes.Disease.DiseaseSortInfo a, OverlayModes.Disease.DiseaseSortInfo b) => a.sortkey.CompareTo(b.sortkey));
			foreach (OverlayModes.Disease.DiseaseSortInfo diseaseSortInfo in list2)
			{
				list.Add(new LegendEntry(diseaseSortInfo.disease.Name, diseaseSortInfo.disease.overlayLegendHovertext.ToString(), GlobalAssets.Instance.colorSet.GetColorByName(diseaseSortInfo.disease.overlayColourName), null, null, true));
			}
			return list;
		}

		// Token: 0x060093E8 RID: 37864 RVA: 0x00331B78 File Offset: 0x0032FD78
		public GameObject GetFreeDiseaseUI()
		{
			GameObject gameObject;
			if (this.freeDiseaseUI < this.diseaseUIList.Count)
			{
				gameObject = this.diseaseUIList[this.freeDiseaseUI];
				gameObject.gameObject.SetActive(true);
				this.freeDiseaseUI++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.diseaseOverlayPrefab, this.diseaseUIParent.transform.gameObject, false);
				this.diseaseUIList.Add(gameObject);
				this.freeDiseaseUI++;
			}
			return gameObject;
		}

		// Token: 0x060093E9 RID: 37865 RVA: 0x00331C00 File Offset: 0x0032FE00
		private void AddDiseaseUI(MinionIdentity target)
		{
			GameObject gameObject = this.GetFreeDiseaseUI();
			DiseaseOverlayWidget component = gameObject.GetComponent<DiseaseOverlayWidget>();
			AmountInstance amount_inst = target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel);
			OverlayModes.Disease.UpdateDiseaseInfo item = new OverlayModes.Disease.UpdateDiseaseInfo(amount_inst, component);
			KAnimControllerBase component2 = target.GetComponent<KAnimControllerBase>();
			Vector3 position = (component2 != null) ? component2.GetWorldPivot() : (target.transform.GetPosition() + Vector3.down);
			gameObject.GetComponent<RectTransform>().SetPosition(position);
			this.updateDiseaseInfo.Add(item);
		}

		// Token: 0x060093EA RID: 37866 RVA: 0x00331C8C File Offset: 0x0032FE8C
		public override void Update()
		{
			Vector2I u;
			Vector2I v;
			Grid.GetVisibleExtents(out u, out v);
			using (new KProfiler.Region("UpdateDiseaseCarriers", null))
			{
				this.queuedAdds.Clear();
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity == null))
					{
						Vector2I vector2I = Grid.PosToXY(minionIdentity.transform.GetPosition());
						if (u <= vector2I && vector2I <= v && !this.privateTargets.Contains(minionIdentity))
						{
							this.AddDiseaseUI(minionIdentity);
							this.queuedAdds.Add(minionIdentity);
						}
					}
				}
				foreach (KMonoBehaviour item in this.queuedAdds)
				{
					this.privateTargets.Add(item);
				}
				this.queuedAdds.Clear();
			}
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);
			}
			bool flag = false;
			if (Game.Instance.showLiquidConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.LiquidVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item2 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item2))
						{
							OverlayScreen.DiseaseIDs.Add(item2);
							flag = true;
						}
					}
					goto IL_1F1;
				}
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item3))
				{
					OverlayScreen.DiseaseIDs.Remove(item3);
					flag = true;
				}
			}
			IL_1F1:
			if (Game.Instance.showGasConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.GasVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item4 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item4))
						{
							OverlayScreen.DiseaseIDs.Add(item4);
							flag = true;
						}
					}
					goto IL_297;
				}
			}
			foreach (Tag item5 in OverlayScreen.GasVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item5))
				{
					OverlayScreen.DiseaseIDs.Remove(item5);
					flag = true;
				}
			}
			IL_297:
			if (flag)
			{
				this.SetLayerZ(-50f);
			}
		}

		// Token: 0x060093EB RID: 37867 RVA: 0x00331FA4 File Offset: 0x003301A4
		private void SetLayerZ(float offset_z)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.ClearOutsideViewObjects<KMonoBehaviour>(this.layerTargets, vector2I, vector2I2, OverlayScreen.DiseaseIDs, delegate(KMonoBehaviour go)
			{
				if (go != null)
				{
					float defaultDepth2 = OverlayModes.Mode.GetDefaultDepth(go);
					Vector3 position2 = go.transform.GetPosition();
					position2.z = defaultDepth2;
					go.transform.SetPosition(position2);
					KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
					component2.enabled = false;
					component2.enabled = true;
				}
			});
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			foreach (Tag key in OverlayScreen.DiseaseIDs)
			{
				List<SaveLoadRoot> list;
				if (lists.TryGetValue(key, out list))
				{
					foreach (KMonoBehaviour kmonoBehaviour in list)
					{
						if (!(kmonoBehaviour == null) && !this.layerTargets.Contains(kmonoBehaviour))
						{
							Vector3 position = kmonoBehaviour.transform.GetPosition();
							if (Grid.IsVisible(Grid.PosToCell(position)) && vector2I <= position && position <= vector2I2)
							{
								float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
								position.z = defaultDepth + offset_z;
								kmonoBehaviour.transform.SetPosition(position);
								KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
								component.enabled = false;
								component.enabled = true;
								this.layerTargets.Add(kmonoBehaviour);
							}
						}
					}
				}
			}
		}

		// Token: 0x04007431 RID: 29745
		public static readonly HashedString ID = "Disease";

		// Token: 0x04007432 RID: 29746
		private int cameraLayerMask;

		// Token: 0x04007433 RID: 29747
		private int freeDiseaseUI;

		// Token: 0x04007434 RID: 29748
		private List<GameObject> diseaseUIList = new List<GameObject>();

		// Token: 0x04007435 RID: 29749
		private List<OverlayModes.Disease.UpdateDiseaseInfo> updateDiseaseInfo = new List<OverlayModes.Disease.UpdateDiseaseInfo>();

		// Token: 0x04007436 RID: 29750
		private HashSet<KMonoBehaviour> layerTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04007437 RID: 29751
		private HashSet<KMonoBehaviour> privateTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04007438 RID: 29752
		private List<KMonoBehaviour> queuedAdds = new List<KMonoBehaviour>();

		// Token: 0x04007439 RID: 29753
		private Canvas diseaseUIParent;

		// Token: 0x0400743A RID: 29754
		private GameObject diseaseOverlayPrefab;

		// Token: 0x02002214 RID: 8724
		private struct DiseaseSortInfo
		{
			// Token: 0x0600ACBD RID: 44221 RVA: 0x003780C2 File Offset: 0x003762C2
			public DiseaseSortInfo(Klei.AI.Disease d)
			{
				this.disease = d;
				this.sortkey = OverlayModes.Disease.CalculateHUE(GlobalAssets.Instance.colorSet.GetColorByName(d.overlayColourName));
			}

			// Token: 0x04009890 RID: 39056
			public float sortkey;

			// Token: 0x04009891 RID: 39057
			public Klei.AI.Disease disease;
		}

		// Token: 0x02002215 RID: 8725
		private struct UpdateDiseaseInfo
		{
			// Token: 0x0600ACBE RID: 44222 RVA: 0x003780EB File Offset: 0x003762EB
			public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui)
			{
				this.ui = ui;
				this.valueSrc = amount_inst;
			}

			// Token: 0x04009892 RID: 39058
			public DiseaseOverlayWidget ui;

			// Token: 0x04009893 RID: 39059
			public AmountInstance valueSrc;
		}
	}

	// Token: 0x0200191D RID: 6429
	public class Logic : OverlayModes.Mode
	{
		// Token: 0x060093ED RID: 37869 RVA: 0x0033213D File Offset: 0x0033033D
		public override HashedString ViewMode()
		{
			return OverlayModes.Logic.ID;
		}

		// Token: 0x060093EE RID: 37870 RVA: 0x00332144 File Offset: 0x00330344
		public override string GetSoundName()
		{
			return "Logic";
		}

		// Token: 0x060093EF RID: 37871 RVA: 0x0033214C File Offset: 0x0033034C
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.LOGIC.INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.INPUT, Color.white, null, Assets.GetSprite("logicInput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.OUTPUT, Color.white, null, Assets.GetSprite("logicOutput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_INPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_in"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_OUTPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_out"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RESET_UPDATE, UI.OVERLAYS.LOGIC.TOOLTIPS.RESET_UPDATE, Color.white, null, Assets.GetSprite("logicResetUpdate"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CONTROL_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.CONTROL_INPUT, Color.white, null, Assets.GetSprite("control_input_frame_legend"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CIRCUIT_STATUS_HEADER, null, Color.white, null, null, false),
				new LegendEntry(UI.OVERLAYS.LOGIC.ONE, null, GlobalAssets.Instance.colorSet.logicOnText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.ZERO, null, GlobalAssets.Instance.colorSet.logicOffText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.DISCONNECTED, UI.OVERLAYS.LOGIC.TOOLTIPS.DISCONNECTED, GlobalAssets.Instance.colorSet.logicDisconnected, null, null, true)
			};
		}

		// Token: 0x060093F0 RID: 37872 RVA: 0x0033234C File Offset: 0x0033054C
		public Logic(LogicModeUI ui_asset)
		{
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.uiAsset = ui_asset;
		}

		// Token: 0x060093F1 RID: 37873 RVA: 0x00332430 File Offset: 0x00330630
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.gameObjPartition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayModes.Logic.HighlightItemIDs);
			this.ioPartition = this.CreateLogicUIPartition();
			GridCompositor.Instance.ToggleMinor(true);
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
		}

		// Token: 0x060093F2 RID: 37874 RVA: 0x003324FC File Offset: 0x003306FC
		public override void Disable()
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterLogicOn, STOP_MODE.ALLOWFADEOUT);
			foreach (SaveLoadRoot saveLoadRoot in this.gameObjTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = false;
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = true;
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.gameObjTargets);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.wireControllers);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.ribbonControllers);
			this.ResetRibbonSymbolTints<KBatchedAnimController>(this.ribbonControllers);
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
			{
				if (bridgeInfo.controller != null)
				{
					OverlayModes.Mode.ResetDisplayValues(bridgeInfo.controller);
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
			{
				if (bridgeInfo2.controller != null)
				{
					this.ResetRibbonTint(bridgeInfo2.controller);
				}
			}
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				uiinfo.Release();
			}
			this.uiInfo.Clear();
			this.uiNodes.Clear();
			this.ioPartition.Clear();
			this.ioTargets.Clear();
			this.gameObjPartition.Clear();
			this.gameObjTargets.Clear();
			this.wireControllers.Clear();
			this.ribbonControllers.Clear();
			this.bridgeControllers.Clear();
			this.ribbonBridgeControllers.Clear();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x060093F3 RID: 37875 RVA: 0x003327B8 File Offset: 0x003309B8
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayModes.Logic.HighlightItemIDs.Contains(saveLoadTag))
			{
				this.gameObjPartition.Add(item);
			}
		}

		// Token: 0x060093F4 RID: 37876 RVA: 0x003327EC File Offset: 0x003309EC
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.gameObjTargets.Contains(item))
			{
				this.gameObjTargets.Remove(item);
			}
			this.gameObjPartition.Remove(item);
		}

		// Token: 0x060093F5 RID: 37877 RVA: 0x00332838 File Offset: 0x00330A38
		private void OnUIElemAdded(ILogicUIElement elem)
		{
			this.ioPartition.Add(elem);
		}

		// Token: 0x060093F6 RID: 37878 RVA: 0x00332846 File Offset: 0x00330A46
		private void OnUIElemRemoved(ILogicUIElement elem)
		{
			this.ioPartition.Remove(elem);
			if (this.ioTargets.Contains(elem))
			{
				this.ioTargets.Remove(elem);
				this.FreeUI(elem);
			}
		}

		// Token: 0x060093F7 RID: 37879 RVA: 0x00332878 File Offset: 0x00330A78
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			Tag wire_id = TagManager.Create("LogicWire");
			Tag ribbon_id = TagManager.Create("LogicRibbon");
			Tag bridge_id = TagManager.Create("LogicWireBridge");
			Tag ribbon_bridge_id = TagManager.Create("LogicRibbonBridge");
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.gameObjTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				KPrefabID component7 = root.GetComponent<KPrefabID>();
				if (component7 != null)
				{
					Tag prefabTag = component7.PrefabTag;
					if (prefabTag == wire_id)
					{
						this.wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == ribbon_id)
					{
						this.ResetRibbonTint(root.GetComponent<KBatchedAnimController>());
						this.ribbonControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.bridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					if (prefabTag == ribbon_bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.ResetRibbonTint(controller);
						this.ribbonBridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
					Vector3 position = root.transform.GetPosition();
					position.z = defaultDepth;
					root.transform.SetPosition(position);
					root.GetComponent<KBatchedAnimController>().enabled = false;
					root.GetComponent<KBatchedAnimController>().enabled = true;
				}
			});
			OverlayModes.Mode.RemoveOffscreenTargets<ILogicUIElement>(this.ioTargets, this.workingIOTargets, vector2I, vector2I2, new Action<ILogicUIElement>(this.FreeUI), null);
			using (new KProfiler.Region("UpdateLogicOverlay", null))
			{
				Action<SaveLoadRoot> <>9__3;
				foreach (object obj in this.gameObjPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
					if (saveLoadRoot != null)
					{
						KPrefabID component = saveLoadRoot.GetComponent<KPrefabID>();
						if (component.PrefabTag == wire_id || component.PrefabTag == bridge_id || component.PrefabTag == ribbon_id || component.PrefabTag == ribbon_bridge_id)
						{
							SaveLoadRoot instance = saveLoadRoot;
							Vector2I vis_min = vector2I;
							Vector2I vis_max = vector2I2;
							ICollection<SaveLoadRoot> targets = this.gameObjTargets;
							int layer = this.conduitTargetLayer;
							Action<SaveLoadRoot> on_added;
							if ((on_added = <>9__3) == null)
							{
								on_added = (<>9__3 = delegate(SaveLoadRoot root)
								{
									if (root == null)
									{
										return;
									}
									KPrefabID component7 = root.GetComponent<KPrefabID>();
									if (OverlayModes.Logic.HighlightItemIDs.Contains(component7.PrefabTag))
									{
										if (component7.PrefabTag == wire_id)
										{
											this.wireControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == ribbon_id)
										{
											this.ribbonControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == bridge_id)
										{
											KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
											int networkCell2 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.bridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell2,
												controller = component8
											});
											return;
										}
										if (component7.PrefabTag == ribbon_bridge_id)
										{
											KBatchedAnimController component9 = root.GetComponent<KBatchedAnimController>();
											int networkCell3 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.ribbonBridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell3,
												controller = component9
											});
										}
									}
								});
							}
							base.AddTargetIfVisible<SaveLoadRoot>(instance, vis_min, vis_max, targets, layer, on_added, null);
						}
						else
						{
							base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.gameObjTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
							{
								Vector3 position = root.transform.GetPosition();
								float z = position.z;
								KPrefabID component7 = root.GetComponent<KPrefabID>();
								if (component7 != null)
								{
									if (component7.HasTag(GameTags.OverlayInFrontOfConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) - 0.2f;
									}
									else if (component7.HasTag(GameTags.OverlayBehindConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) + 0.2f;
									}
								}
								position.z = z;
								root.transform.SetPosition(position);
								KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
								component8.enabled = false;
								component8.enabled = true;
							}, null);
						}
					}
				}
				foreach (object obj2 in this.ioPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					ILogicUIElement logicUIElement = (ILogicUIElement)obj2;
					if (logicUIElement != null)
					{
						base.AddTargetIfVisible<ILogicUIElement>(logicUIElement, vector2I, vector2I2, this.ioTargets, this.objectTargetLayer, new Action<ILogicUIElement>(this.AddUI), (KMonoBehaviour kcmp) => kcmp != null && OverlayModes.Logic.HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag));
					}
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component2 = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component2 != null)
					{
						int networkCell = component2.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.logicCircuitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
				Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
				Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
				logicOff.a = (logicOn.a = 0);
				foreach (KBatchedAnimController kbatchedAnimController in this.wireControllers)
				{
					if (!(kbatchedAnimController == null))
					{
						Color32 color = logicOff;
						LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController.transform.GetPosition()));
						if (networkForCell != null)
						{
							color = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component3 = kbatchedAnimController.GetComponent<IBridgedNetworkItem>();
							if (component3 != null && component3.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
						}
						kbatchedAnimController.TintColour = color;
					}
				}
				foreach (KBatchedAnimController kbatchedAnimController2 in this.ribbonControllers)
				{
					if (!(kbatchedAnimController2 == null))
					{
						Color32 color2 = logicOff;
						Color32 color3 = logicOff;
						Color32 color4 = logicOff;
						Color32 color5 = logicOff;
						LogicCircuitNetwork networkForCell2 = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController2.transform.GetPosition()));
						if (networkForCell2 != null)
						{
							color2 = (networkForCell2.IsBitActive(0) ? logicOn : logicOff);
							color3 = (networkForCell2.IsBitActive(1) ? logicOn : logicOff);
							color4 = (networkForCell2.IsBitActive(2) ? logicOn : logicOff);
							color5 = (networkForCell2.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component4 = kbatchedAnimController2.GetComponent<IBridgedNetworkItem>();
							if (component4 != null && component4.IsConnectedToNetworks(this.connectedNetworks))
							{
								color2.r = (byte)((float)color2.r * num);
								color2.g = (byte)((float)color2.g * num);
								color2.b = (byte)((float)color2.b * num);
								color3.r = (byte)((float)color3.r * num);
								color3.g = (byte)((float)color3.g * num);
								color3.b = (byte)((float)color3.b * num);
								color4.r = (byte)((float)color4.r * num);
								color4.g = (byte)((float)color4.g * num);
								color4.b = (byte)((float)color4.b * num);
								color5.r = (byte)((float)color5.r * num);
								color5.g = (byte)((float)color5.g * num);
								color5.b = (byte)((float)color5.b * num);
							}
						}
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color2);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color3);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color4);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color5);
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
				{
					if (!(bridgeInfo.controller == null))
					{
						Color32 color6 = logicOff;
						LogicCircuitNetwork networkForCell3 = logicCircuitManager.GetNetworkForCell(bridgeInfo.cell);
						if (networkForCell3 != null)
						{
							color6 = (networkForCell3.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component5 = bridgeInfo.controller.GetComponent<IBridgedNetworkItem>();
							if (component5 != null && component5.IsConnectedToNetworks(this.connectedNetworks))
							{
								color6.r = (byte)((float)color6.r * num);
								color6.g = (byte)((float)color6.g * num);
								color6.b = (byte)((float)color6.b * num);
							}
						}
						bridgeInfo.controller.TintColour = color6;
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
				{
					if (!(bridgeInfo2.controller == null))
					{
						Color32 color7 = logicOff;
						Color32 color8 = logicOff;
						Color32 color9 = logicOff;
						Color32 color10 = logicOff;
						LogicCircuitNetwork networkForCell4 = logicCircuitManager.GetNetworkForCell(bridgeInfo2.cell);
						if (networkForCell4 != null)
						{
							color7 = (networkForCell4.IsBitActive(0) ? logicOn : logicOff);
							color8 = (networkForCell4.IsBitActive(1) ? logicOn : logicOff);
							color9 = (networkForCell4.IsBitActive(2) ? logicOn : logicOff);
							color10 = (networkForCell4.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component6 = bridgeInfo2.controller.GetComponent<IBridgedNetworkItem>();
							if (component6 != null && component6.IsConnectedToNetworks(this.connectedNetworks))
							{
								color7.r = (byte)((float)color7.r * num);
								color7.g = (byte)((float)color7.g * num);
								color7.b = (byte)((float)color7.b * num);
								color8.r = (byte)((float)color8.r * num);
								color8.g = (byte)((float)color8.g * num);
								color8.b = (byte)((float)color8.b * num);
								color9.r = (byte)((float)color9.r * num);
								color9.g = (byte)((float)color9.g * num);
								color9.b = (byte)((float)color9.b * num);
								color10.r = (byte)((float)color10.r * num);
								color10.g = (byte)((float)color10.g * num);
								color10.b = (byte)((float)color10.b * num);
							}
						}
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color7);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color8);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color9);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color10);
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x060093F8 RID: 37880 RVA: 0x003332D0 File Offset: 0x003314D0
		private void UpdateUI()
		{
			Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
			Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
			Color32 logicDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
			logicOff.a = (logicOn.a = byte.MaxValue);
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(uiinfo.cell);
				Color32 c = logicDisconnected;
				LogicControlInputUI component = uiinfo.instance.GetComponent<LogicControlInputUI>();
				if (component != null)
				{
					component.SetContent(networkForCell);
				}
				else if (uiinfo.bitDepth == 4)
				{
					LogicRibbonDisplayUI component2 = uiinfo.instance.GetComponent<LogicRibbonDisplayUI>();
					if (component2 != null)
					{
						component2.SetContent(networkForCell);
					}
				}
				else if (uiinfo.bitDepth == 1)
				{
					if (networkForCell != null)
					{
						c = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
					}
					if (uiinfo.image.color != c)
					{
						uiinfo.image.color = c;
					}
				}
			}
		}

		// Token: 0x060093F9 RID: 37881 RVA: 0x00333428 File Offset: 0x00331628
		private void AddUI(ILogicUIElement ui_elem)
		{
			if (this.uiNodes.ContainsKey(ui_elem))
			{
				return;
			}
			HandleVector<int>.Handle uiHandle = this.uiInfo.Allocate(new OverlayModes.Logic.UIInfo(ui_elem, this.uiAsset));
			this.uiNodes.Add(ui_elem, new OverlayModes.Logic.EventInfo
			{
				uiHandle = uiHandle
			});
		}

		// Token: 0x060093FA RID: 37882 RVA: 0x0033347C File Offset: 0x0033167C
		private void FreeUI(ILogicUIElement item)
		{
			if (item == null)
			{
				return;
			}
			OverlayModes.Logic.EventInfo eventInfo;
			if (this.uiNodes.TryGetValue(item, out eventInfo))
			{
				this.uiInfo.GetData(eventInfo.uiHandle).Release();
				this.uiInfo.Free(eventInfo.uiHandle);
				this.uiNodes.Remove(item);
			}
		}

		// Token: 0x060093FB RID: 37883 RVA: 0x003334D8 File Offset: 0x003316D8
		protected UniformGrid<ILogicUIElement> CreateLogicUIPartition()
		{
			UniformGrid<ILogicUIElement> uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (ILogicUIElement logicUIElement in Game.Instance.logicCircuitManager.GetVisElements())
			{
				if (logicUIElement != null)
				{
					uniformGrid.Add(logicUIElement);
				}
			}
			return uniformGrid;
		}

		// Token: 0x060093FC RID: 37884 RVA: 0x00333544 File Offset: 0x00331744
		private bool IsBitActive(int value, int bit)
		{
			return (value & 1 << bit) > 0;
		}

		// Token: 0x060093FD RID: 37885 RVA: 0x00333554 File Offset: 0x00331754
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x060093FE RID: 37886 RVA: 0x003335E4 File Offset: 0x003317E4
		private void ResetRibbonSymbolTints<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					this.ResetRibbonTint(component);
				}
			}
		}

		// Token: 0x060093FF RID: 37887 RVA: 0x00333648 File Offset: 0x00331848
		private void ResetRibbonTint(KBatchedAnimController kbac)
		{
			if (kbac != null)
			{
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, Color.white);
			}
		}

		// Token: 0x0400743B RID: 29755
		public static readonly HashedString ID = "Logic";

		// Token: 0x0400743C RID: 29756
		public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();

		// Token: 0x0400743D RID: 29757
		public static KAnimHashedString RIBBON_WIRE_1_SYMBOL_NAME = "wire1";

		// Token: 0x0400743E RID: 29758
		public static KAnimHashedString RIBBON_WIRE_2_SYMBOL_NAME = "wire2";

		// Token: 0x0400743F RID: 29759
		public static KAnimHashedString RIBBON_WIRE_3_SYMBOL_NAME = "wire3";

		// Token: 0x04007440 RID: 29760
		public static KAnimHashedString RIBBON_WIRE_4_SYMBOL_NAME = "wire4";

		// Token: 0x04007441 RID: 29761
		private int conduitTargetLayer;

		// Token: 0x04007442 RID: 29762
		private int objectTargetLayer;

		// Token: 0x04007443 RID: 29763
		private int cameraLayerMask;

		// Token: 0x04007444 RID: 29764
		private int selectionMask;

		// Token: 0x04007445 RID: 29765
		private UniformGrid<ILogicUIElement> ioPartition;

		// Token: 0x04007446 RID: 29766
		private HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04007447 RID: 29767
		private HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04007448 RID: 29768
		private HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x04007449 RID: 29769
		private HashSet<KBatchedAnimController> ribbonControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x0400744A RID: 29770
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x0400744B RID: 29771
		private List<int> visited = new List<int>();

		// Token: 0x0400744C RID: 29772
		private HashSet<OverlayModes.Logic.BridgeInfo> bridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x0400744D RID: 29773
		private HashSet<OverlayModes.Logic.BridgeInfo> ribbonBridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x0400744E RID: 29774
		private UniformGrid<SaveLoadRoot> gameObjPartition;

		// Token: 0x0400744F RID: 29775
		private HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04007450 RID: 29776
		private LogicModeUI uiAsset;

		// Token: 0x04007451 RID: 29777
		private Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo> uiNodes = new Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo>();

		// Token: 0x04007452 RID: 29778
		private KCompactedVector<OverlayModes.Logic.UIInfo> uiInfo = new KCompactedVector<OverlayModes.Logic.UIInfo>(0);

		// Token: 0x02002217 RID: 8727
		private struct BridgeInfo
		{
			// Token: 0x04009897 RID: 39063
			public int cell;

			// Token: 0x04009898 RID: 39064
			public KBatchedAnimController controller;
		}

		// Token: 0x02002218 RID: 8728
		private struct EventInfo
		{
			// Token: 0x04009899 RID: 39065
			public HandleVector<int>.Handle uiHandle;
		}

		// Token: 0x02002219 RID: 8729
		private struct UIInfo
		{
			// Token: 0x0600ACC3 RID: 44227 RVA: 0x00378178 File Offset: 0x00376378
			public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data)
			{
				this.cell = ui_elem.GetLogicUICell();
				GameObject original = null;
				Sprite sprite = null;
				this.bitDepth = 1;
				switch (ui_elem.GetLogicPortSpriteType())
				{
				case LogicPortSpriteType.Input:
					original = ui_data.prefab;
					sprite = ui_data.inputSprite;
					break;
				case LogicPortSpriteType.Output:
					original = ui_data.prefab;
					sprite = ui_data.outputSprite;
					break;
				case LogicPortSpriteType.ResetUpdate:
					original = ui_data.prefab;
					sprite = ui_data.resetSprite;
					break;
				case LogicPortSpriteType.ControlInput:
					original = ui_data.controlInputPrefab;
					break;
				case LogicPortSpriteType.RibbonInput:
					original = ui_data.ribbonInputPrefab;
					this.bitDepth = 4;
					break;
				case LogicPortSpriteType.RibbonOutput:
					original = ui_data.ribbonOutputPrefab;
					this.bitDepth = 4;
					break;
				}
				this.instance = global::Util.KInstantiate(original, Grid.CellToPosCCC(this.cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
				this.instance.SetActive(true);
				this.image = this.instance.GetComponent<Image>();
				if (this.image != null)
				{
					this.image.raycastTarget = false;
					this.image.sprite = sprite;
				}
			}

			// Token: 0x0600ACC4 RID: 44228 RVA: 0x00378288 File Offset: 0x00376488
			public void Release()
			{
				global::Util.KDestroyGameObject(this.instance);
			}

			// Token: 0x0400989A RID: 39066
			public GameObject instance;

			// Token: 0x0400989B RID: 39067
			public Image image;

			// Token: 0x0400989C RID: 39068
			public int cell;

			// Token: 0x0400989D RID: 39069
			public int bitDepth;
		}
	}

	// Token: 0x0200191E RID: 6430
	public enum BringToFrontLayerSetting
	{
		// Token: 0x04007454 RID: 29780
		None,
		// Token: 0x04007455 RID: 29781
		Constant,
		// Token: 0x04007456 RID: 29782
		Conditional
	}

	// Token: 0x0200191F RID: 6431
	public class ColorHighlightCondition
	{
		// Token: 0x06009401 RID: 37889 RVA: 0x00333702 File Offset: 0x00331902
		public ColorHighlightCondition(Func<KMonoBehaviour, Color> highlight_color, Func<KMonoBehaviour, bool> highlight_condition)
		{
			this.highlight_color = highlight_color;
			this.highlight_condition = highlight_condition;
		}

		// Token: 0x04007457 RID: 29783
		public Func<KMonoBehaviour, Color> highlight_color;

		// Token: 0x04007458 RID: 29784
		public Func<KMonoBehaviour, bool> highlight_condition;
	}

	// Token: 0x02001920 RID: 6432
	public class None : OverlayModes.Mode
	{
		// Token: 0x06009402 RID: 37890 RVA: 0x00333718 File Offset: 0x00331918
		public override HashedString ViewMode()
		{
			return OverlayModes.None.ID;
		}

		// Token: 0x06009403 RID: 37891 RVA: 0x0033371F File Offset: 0x0033191F
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x04007459 RID: 29785
		public static readonly HashedString ID = HashedString.Invalid;
	}

	// Token: 0x02001921 RID: 6433
	public class PathProber : OverlayModes.Mode
	{
		// Token: 0x06009406 RID: 37894 RVA: 0x0033373A File Offset: 0x0033193A
		public override HashedString ViewMode()
		{
			return OverlayModes.PathProber.ID;
		}

		// Token: 0x06009407 RID: 37895 RVA: 0x00333741 File Offset: 0x00331941
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x0400745A RID: 29786
		public static readonly HashedString ID = "PathProber";
	}

	// Token: 0x02001922 RID: 6434
	public class Oxygen : OverlayModes.Mode
	{
		// Token: 0x0600940A RID: 37898 RVA: 0x00333761 File Offset: 0x00331961
		public override HashedString ViewMode()
		{
			return OverlayModes.Oxygen.ID;
		}

		// Token: 0x0600940B RID: 37899 RVA: 0x00333768 File Offset: 0x00331968
		public override string GetSoundName()
		{
			return "Oxygen";
		}

		// Token: 0x0600940C RID: 37900 RVA: 0x00333770 File Offset: 0x00331970
		public override void Enable()
		{
			base.Enable();
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x0600940D RID: 37901 RVA: 0x003337AF File Offset: 0x003319AF
		public override void Disable()
		{
			base.Disable();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x0400745B RID: 29787
		public static readonly HashedString ID = "Oxygen";
	}

	// Token: 0x02001923 RID: 6435
	public class Light : OverlayModes.Mode
	{
		// Token: 0x06009410 RID: 37904 RVA: 0x003337DA File Offset: 0x003319DA
		public override HashedString ViewMode()
		{
			return OverlayModes.Light.ID;
		}

		// Token: 0x06009411 RID: 37905 RVA: 0x003337E1 File Offset: 0x003319E1
		public override string GetSoundName()
		{
			return "Lights";
		}

		// Token: 0x0400745C RID: 29788
		public static readonly HashedString ID = "Light";
	}

	// Token: 0x02001924 RID: 6436
	public class Priorities : OverlayModes.Mode
	{
		// Token: 0x06009414 RID: 37908 RVA: 0x00333801 File Offset: 0x00331A01
		public override HashedString ViewMode()
		{
			return OverlayModes.Priorities.ID;
		}

		// Token: 0x06009415 RID: 37909 RVA: 0x00333808 File Offset: 0x00331A08
		public override string GetSoundName()
		{
			return "Priorities";
		}

		// Token: 0x0400745D RID: 29789
		public static readonly HashedString ID = "Priorities";
	}

	// Token: 0x02001925 RID: 6437
	public class ThermalConductivity : OverlayModes.Mode
	{
		// Token: 0x06009418 RID: 37912 RVA: 0x00333828 File Offset: 0x00331A28
		public override HashedString ViewMode()
		{
			return OverlayModes.ThermalConductivity.ID;
		}

		// Token: 0x06009419 RID: 37913 RVA: 0x0033382F File Offset: 0x00331A2F
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x0400745E RID: 29790
		public static readonly HashedString ID = "ThermalConductivity";
	}

	// Token: 0x02001926 RID: 6438
	public class HeatFlow : OverlayModes.Mode
	{
		// Token: 0x0600941C RID: 37916 RVA: 0x0033384F File Offset: 0x00331A4F
		public override HashedString ViewMode()
		{
			return OverlayModes.HeatFlow.ID;
		}

		// Token: 0x0600941D RID: 37917 RVA: 0x00333856 File Offset: 0x00331A56
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x0400745F RID: 29791
		public static readonly HashedString ID = "HeatFlow";
	}

	// Token: 0x02001927 RID: 6439
	public class Rooms : OverlayModes.Mode
	{
		// Token: 0x06009420 RID: 37920 RVA: 0x00333876 File Offset: 0x00331A76
		public override HashedString ViewMode()
		{
			return OverlayModes.Rooms.ID;
		}

		// Token: 0x06009421 RID: 37921 RVA: 0x0033387D File Offset: 0x00331A7D
		public override string GetSoundName()
		{
			return "Rooms";
		}

		// Token: 0x06009422 RID: 37922 RVA: 0x00333884 File Offset: 0x00331A84
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<RoomType> list2 = new List<RoomType>(Db.Get().RoomTypes.resources);
			list2.Sort((RoomType a, RoomType b) => a.sortKey.CompareTo(b.sortKey));
			foreach (RoomType roomType in list2)
			{
				string text = roomType.GetCriteriaString();
				if (roomType.effects != null && roomType.effects.Length != 0)
				{
					text = text + "\n\n" + roomType.GetRoomEffectsString();
				}
				list.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect, text, GlobalAssets.Instance.colorSet.GetColorByName(roomType.category.colorName), null, null, true));
			}
			return list;
		}

		// Token: 0x04007460 RID: 29792
		public static readonly HashedString ID = "Rooms";
	}

	// Token: 0x02001928 RID: 6440
	public abstract class Mode
	{
		// Token: 0x06009425 RID: 37925 RVA: 0x00333991 File Offset: 0x00331B91
		public static void Clear()
		{
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x06009426 RID: 37926
		public abstract HashedString ViewMode();

		// Token: 0x06009427 RID: 37927 RVA: 0x0033399D File Offset: 0x00331B9D
		public virtual void Enable()
		{
		}

		// Token: 0x06009428 RID: 37928 RVA: 0x0033399F File Offset: 0x00331B9F
		public virtual void Update()
		{
		}

		// Token: 0x06009429 RID: 37929 RVA: 0x003339A1 File Offset: 0x00331BA1
		public virtual void Disable()
		{
		}

		// Token: 0x0600942A RID: 37930 RVA: 0x003339A3 File Offset: 0x00331BA3
		public virtual List<LegendEntry> GetCustomLegendData()
		{
			return null;
		}

		// Token: 0x0600942B RID: 37931 RVA: 0x003339A6 File Offset: 0x00331BA6
		public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return null;
		}

		// Token: 0x0600942C RID: 37932 RVA: 0x003339A9 File Offset: 0x00331BA9
		public virtual void OnFiltersChanged()
		{
		}

		// Token: 0x0600942D RID: 37933 RVA: 0x003339AB File Offset: 0x00331BAB
		public virtual void DisableOverlay()
		{
		}

		// Token: 0x0600942E RID: 37934
		public abstract string GetSoundName();

		// Token: 0x0600942F RID: 37935 RVA: 0x003339AD File Offset: 0x00331BAD
		protected bool InFilter(string layer, Dictionary<string, ToolParameterMenu.ToggleState> filter)
		{
			return (filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On) || (filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On);
		}

		// Token: 0x06009430 RID: 37936 RVA: 0x003339E0 File Offset: 0x00331BE0
		public void RegisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister += this.OnSaveLoadRootRegistered;
			saveManager.onUnregister += this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x06009431 RID: 37937 RVA: 0x00333A11 File Offset: 0x00331C11
		public void UnregisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister -= this.OnSaveLoadRootRegistered;
			saveManager.onUnregister -= this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x06009432 RID: 37938 RVA: 0x00333A42 File Offset: 0x00331C42
		protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot root)
		{
		}

		// Token: 0x06009433 RID: 37939 RVA: 0x00333A44 File Offset: 0x00331C44
		protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root)
		{
		}

		// Token: 0x06009434 RID: 37940 RVA: 0x00333A48 File Offset: 0x00331C48
		protected void ProcessExistingSaveLoadRoots()
		{
			foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in SaveLoader.Instance.saveManager.GetLists())
			{
				foreach (SaveLoadRoot root in keyValuePair.Value)
				{
					this.OnSaveLoadRootRegistered(root);
				}
			}
		}

		// Token: 0x06009435 RID: 37941 RVA: 0x00333AE0 File Offset: 0x00331CE0
		protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject
		{
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			UniformGrid<T> uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (Tag key in tags)
			{
				List<SaveLoadRoot> list = null;
				if (lists.TryGetValue(key, out list))
				{
					foreach (SaveLoadRoot saveLoadRoot in list)
					{
						T component = saveLoadRoot.GetComponent<T>();
						if (component != null)
						{
							uniformGrid.Add(component);
						}
					}
				}
			}
			return uniformGrid;
		}

		// Token: 0x06009436 RID: 37942 RVA: 0x00333BA4 File Offset: 0x00331DA4
		protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
				}
			}
		}

		// Token: 0x06009437 RID: 37943 RVA: 0x00333C10 File Offset: 0x00331E10
		protected static void ResetDisplayValues(KBatchedAnimController controller)
		{
			controller.SetLayer(0);
			controller.HighlightColour = Color.clear;
			controller.TintColour = Color.white;
			controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
		}

		// Token: 0x06009438 RID: 37944 RVA: 0x00333C4C File Offset: 0x00331E4C
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, Vector2I min, Vector2I max, Action<T> on_removed = null) where T : KMonoBehaviour
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, min, max, null, delegate(T cmp)
			{
				if (cmp != null)
				{
					KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
					if (on_removed != null)
					{
						on_removed(cmp);
					}
				}
			});
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x06009439 RID: 37945 RVA: 0x00333C88 File Offset: 0x00331E88
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, Vector2I vis_min, Vector2I vis_max, ICollection<Tag> item_ids, Action<T> on_remove) where T : KMonoBehaviour
		{
			OverlayModes.Mode.workingTargets.Clear();
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector2I vector2I = Grid.PosToXY(t.transform.GetPosition());
					if (!(vis_min <= vector2I) || !(vector2I <= vis_max) || t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
					{
						OverlayModes.Mode.workingTargets.Add(t);
					}
					else
					{
						KPrefabID component = t.GetComponent<KPrefabID>();
						if (item_ids != null && !item_ids.Contains(component.PrefabTag) && t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
						{
							OverlayModes.Mode.workingTargets.Add(t);
						}
					}
				}
			}
			foreach (KMonoBehaviour kmonoBehaviour in OverlayModes.Mode.workingTargets)
			{
				T t2 = (T)((object)kmonoBehaviour);
				if (!(t2 == null))
				{
					if (on_remove != null)
					{
						on_remove(t2);
					}
					targets.Remove(t2);
				}
			}
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600943A RID: 37946 RVA: 0x00333DFC File Offset: 0x00331FFC
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null, Func<T, bool> special_clear_condition = null) where T : IUniformGridObject
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, working_targets, vis_min, vis_max, delegate(T cmp)
			{
				if (cmp != null && on_removed != null)
				{
					on_removed(cmp);
				}
			});
			if (special_clear_condition != null)
			{
				working_targets.Clear();
				foreach (T t in targets)
				{
					if (special_clear_condition(t))
					{
						working_targets.Add(t);
					}
				}
				foreach (T t2 in working_targets)
				{
					if (t2 != null)
					{
						if (on_removed != null)
						{
							on_removed(t2);
						}
						targets.Remove(t2);
					}
				}
				working_targets.Clear();
			}
		}

		// Token: 0x0600943B RID: 37947 RVA: 0x00333ED8 File Offset: 0x003320D8
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null) where T : IUniformGridObject
		{
			working_targets.Clear();
			foreach (T t in targets)
			{
				if (t != null)
				{
					Vector2 vector = t.PosMin();
					Vector2 vector2 = t.PosMin();
					if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || (float)vis_max.x < vector.x || (float)vis_max.y < vector.y)
					{
						working_targets.Add(t);
					}
				}
			}
			foreach (T t2 in working_targets)
			{
				if (t2 != null)
				{
					if (on_removed != null)
					{
						on_removed(t2);
					}
					targets.Remove(t2);
				}
			}
			working_targets.Clear();
		}

		// Token: 0x0600943C RID: 37948 RVA: 0x00333FDC File Offset: 0x003321DC
		protected static float GetDefaultDepth(KMonoBehaviour cmp)
		{
			BuildingComplete component = cmp.GetComponent<BuildingComplete>();
			float layerZ;
			if (component != null)
			{
				layerZ = Grid.GetLayerZ(component.Def.SceneLayer);
			}
			else
			{
				layerZ = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			}
			return layerZ;
		}

		// Token: 0x0600943D RID: 37949 RVA: 0x00334018 File Offset: 0x00332218
		protected void UpdateHighlightTypeOverlay<T>(Vector2I min, Vector2I max, ICollection<T> targets, ICollection<Tag> item_ids, OverlayModes.ColorHighlightCondition[] highlights, OverlayModes.BringToFrontLayerSetting bringToFrontSetting, int layer) where T : KMonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector3 position = t.transform.GetPosition();
					int cell = Grid.PosToCell(position);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && min <= position && position <= max)
					{
						KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
						if (!(component == null))
						{
							int layer2 = 0;
							Color32 highlightColour = Color.clear;
							if (highlights != null)
							{
								foreach (OverlayModes.ColorHighlightCondition colorHighlightCondition in highlights)
								{
									if (colorHighlightCondition.highlight_condition(t))
									{
										highlightColour = colorHighlightCondition.highlight_color(t);
										layer2 = layer;
										break;
									}
								}
							}
							if (bringToFrontSetting != OverlayModes.BringToFrontLayerSetting.Constant)
							{
								if (bringToFrontSetting == OverlayModes.BringToFrontLayerSetting.Conditional)
								{
									component.SetLayer(layer2);
								}
							}
							else
							{
								component.SetLayer(layer);
							}
							component.HighlightColour = highlightColour;
						}
					}
				}
			}
		}

		// Token: 0x0600943E RID: 37950 RVA: 0x00334170 File Offset: 0x00332370
		protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour
		{
			Color32 highlightColour = Color.clear;
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.HighlightColour = highlightColour;
						component.SetLayer(0);
					}
				}
			}
			targets.Clear();
		}

		// Token: 0x0600943F RID: 37951 RVA: 0x003341F4 File Offset: 0x003323F4
		protected void AddTargetIfVisible<T>(T instance, Vector2I vis_min, Vector2I vis_max, ICollection<T> targets, int layer, Action<T> on_added = null, Func<KMonoBehaviour, bool> should_add = null) where T : IUniformGridObject
		{
			if (instance.Equals(null))
			{
				return;
			}
			Vector2 vector = instance.PosMin();
			Vector2 vector2 = instance.PosMax();
			if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || vector.x > (float)vis_max.x || vector.y > (float)vis_max.y)
			{
				return;
			}
			if (targets.Contains(instance))
			{
				return;
			}
			bool flag = false;
			int num = (int)vector.y;
			while ((float)num <= vector2.y)
			{
				int num2 = (int)vector.x;
				while ((float)num2 <= vector2.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					if ((Grid.IsValidCell(num3) && Grid.Visible[num3] > 20 && (int)Grid.WorldIdx[num3] == ClusterManager.Instance.activeWorldId) || !PropertyTextures.IsFogOfWarEnabled)
					{
						flag = true;
						break;
					}
					num2++;
				}
				num++;
			}
			if (flag)
			{
				bool flag2 = true;
				KMonoBehaviour kmonoBehaviour = instance as KMonoBehaviour;
				if (kmonoBehaviour != null && should_add != null)
				{
					flag2 = should_add(kmonoBehaviour);
				}
				if (flag2)
				{
					if (kmonoBehaviour != null)
					{
						KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
						if (component != null)
						{
							component.SetLayer(layer);
						}
					}
					targets.Add(instance);
					if (on_added != null)
					{
						on_added(instance);
					}
				}
			}
		}

		// Token: 0x04007461 RID: 29793
		public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;

		// Token: 0x04007462 RID: 29794
		private static List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
	}

	// Token: 0x02001929 RID: 6441
	public class ModeUtil
	{
		// Token: 0x06009442 RID: 37954 RVA: 0x00334368 File Offset: 0x00332568
		public static float GetHighlightScale()
		{
			return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
		}
	}

	// Token: 0x0200192A RID: 6442
	public class Power : OverlayModes.Mode
	{
		// Token: 0x06009444 RID: 37956 RVA: 0x00334396 File Offset: 0x00332596
		public override HashedString ViewMode()
		{
			return OverlayModes.Power.ID;
		}

		// Token: 0x06009445 RID: 37957 RVA: 0x0033439D File Offset: 0x0033259D
		public override string GetSoundName()
		{
			return "Power";
		}

		// Token: 0x06009446 RID: 37958 RVA: 0x003343A4 File Offset: 0x003325A4
		public Power(Canvas powerLabelParent, LocText powerLabelPrefab, BatteryUI batteryUIPrefab, Vector3 powerLabelOffset, Vector3 batteryUIOffset, Vector3 batteryUITransformerOffset, Vector3 batteryUISmallTransformerOffset)
		{
			this.powerLabelParent = powerLabelParent;
			this.powerLabelPrefab = powerLabelPrefab;
			this.batteryUIPrefab = batteryUIPrefab;
			this.powerLabelOffset = powerLabelOffset;
			this.batteryUIOffset = batteryUIOffset;
			this.batteryUITransformerOffset = batteryUITransformerOffset;
			this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x06009447 RID: 37959 RVA: 0x0033448C File Offset: 0x0033268C
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayScreen.WireIDs);
			GridCompositor.Instance.ToggleMinor(true);
		}

		// Token: 0x06009448 RID: 37960 RVA: 0x003344E4 File Offset: 0x003326E4
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			this.privateTargets.Clear();
			this.queuedAdds.Clear();
			this.DisablePowerLabels();
			this.DisableBatteryUIs();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x06009449 RID: 37961 RVA: 0x00334568 File Offset: 0x00332768
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayScreen.WireIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600944A RID: 37962 RVA: 0x0033459C File Offset: 0x0033279C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600944B RID: 37963 RVA: 0x003345E8 File Offset: 0x003327E8
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			using (new KProfiler.Region("UpdatePowerOverlay", null))
			{
				foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot instance = (SaveLoadRoot)obj;
					base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component != null)
					{
						int networkCell = component.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.electricalConduitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				CircuitManager circuitManager = Game.Instance.circuitManager;
				foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
				{
					if (!(saveLoadRoot == null))
					{
						IBridgedNetworkItem component2 = saveLoadRoot.GetComponent<IBridgedNetworkItem>();
						if (component2 != null)
						{
							KAnimControllerBase component3 = (component2 as KMonoBehaviour).GetComponent<KBatchedAnimController>();
							int networkCell2 = component2.GetNetworkCell();
							UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(networkCell2);
							ushort circuitID = (networkForCell != null) ? ((ushort)networkForCell.id) : ushort.MaxValue;
							float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
							float num2 = circuitManager.GetMaxSafeWattageForCircuit(circuitID);
							num2 += POWER.FLOAT_FUDGE_FACTOR;
							float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
							Color32 color;
							if (wattsUsedByCircuit <= 0f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitUnpowered;
							}
							else if (wattsUsedByCircuit > num2)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitOverloading;
							}
							else if (wattsNeededWhenActive > num2 && num2 > 0f && wattsUsedByCircuit / num2 >= 0.75f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitStraining;
							}
							else
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitSafe;
							}
							if (this.connectedNetworks.Count > 0 && component2.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
							component3.TintColour = color;
						}
					}
				}
			}
			this.queuedAdds.Clear();
			using (new KProfiler.Region("BatteryUI", null))
			{
				foreach (Battery battery in Components.Batteries.Items)
				{
					Vector2I vector2I3 = Grid.PosToXY(battery.transform.GetPosition());
					if (vector2I <= vector2I3 && vector2I3 <= vector2I2 && battery.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component4 = battery.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component4))
						{
							this.AddBatteryUI(battery);
							this.queuedAdds.Add(component4);
						}
					}
				}
				foreach (Generator generator in Components.Generators.Items)
				{
					Vector2I vector2I4 = Grid.PosToXY(generator.transform.GetPosition());
					if (vector2I <= vector2I4 && vector2I4 <= vector2I2 && generator.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component5 = generator.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component5))
						{
							this.privateTargets.Add(component5);
							if (generator.GetComponent<PowerTransformer>() == null)
							{
								this.AddPowerLabels(generator);
							}
						}
					}
				}
				foreach (EnergyConsumer energyConsumer in Components.EnergyConsumers.Items)
				{
					Vector2I vector2I5 = Grid.PosToXY(energyConsumer.transform.GetPosition());
					if (vector2I <= vector2I5 && vector2I5 <= vector2I2 && energyConsumer.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component6 = energyConsumer.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component6))
						{
							this.privateTargets.Add(component6);
							this.AddPowerLabels(energyConsumer);
						}
					}
				}
			}
			foreach (SaveLoadRoot item in this.queuedAdds)
			{
				this.privateTargets.Add(item);
			}
			this.queuedAdds.Clear();
			this.UpdatePowerLabels();
		}

		// Token: 0x0600944C RID: 37964 RVA: 0x00334C00 File Offset: 0x00332E00
		private LocText GetFreePowerLabel()
		{
			LocText locText;
			if (this.freePowerLabelIdx < this.powerLabels.Count)
			{
				locText = this.powerLabels[this.freePowerLabelIdx];
				this.freePowerLabelIdx++;
			}
			else
			{
				locText = global::Util.KInstantiateUI<LocText>(this.powerLabelPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.powerLabels.Add(locText);
				this.freePowerLabelIdx++;
			}
			return locText;
		}

		// Token: 0x0600944D RID: 37965 RVA: 0x00334C84 File Offset: 0x00332E84
		private void UpdatePowerLabels()
		{
			foreach (OverlayModes.Power.UpdatePowerInfo updatePowerInfo in this.updatePowerInfo)
			{
				KMonoBehaviour item = updatePowerInfo.item;
				LocText powerLabel = updatePowerInfo.powerLabel;
				LocText unitLabel = updatePowerInfo.unitLabel;
				Generator generator = updatePowerInfo.generator;
				IEnergyConsumer consumer = updatePowerInfo.consumer;
				if (updatePowerInfo.item == null || updatePowerInfo.item.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
				{
					powerLabel.gameObject.SetActive(false);
				}
				else
				{
					powerLabel.gameObject.SetActive(true);
					if (generator != null && consumer == null)
					{
						int num;
						if (generator.GetComponent<ManualGenerator>() == null)
						{
							generator.GetComponent<Operational>();
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						else
						{
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						powerLabel.text = ((num != 0) ? ("+" + num.ToString()) : num.ToString());
						BuildingEnabledButton component = item.GetComponent<BuildingEnabledButton>();
						Color color = (component != null && !component.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerGenerator;
						powerLabel.color = color;
						unitLabel.color = color;
						BuildingCellVisualizer component2 = generator.GetComponent<BuildingCellVisualizer>();
						if (component2 != null)
						{
							Image outputIcon = component2.GetOutputIcon();
							if (outputIcon != null)
							{
								outputIcon.color = color;
							}
						}
					}
					if (consumer != null)
					{
						BuildingEnabledButton component3 = item.GetComponent<BuildingEnabledButton>();
						Color color2 = (component3 != null && !component3.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerConsumer;
						int num2 = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
						string text = num2.ToString();
						powerLabel.text = ((num2 != 0) ? ("-" + text) : text);
						powerLabel.color = color2;
						unitLabel.color = color2;
						Image inputIcon = item.GetComponentInChildren<BuildingCellVisualizer>().GetInputIcon();
						if (inputIcon != null)
						{
							inputIcon.color = color2;
						}
					}
				}
			}
			foreach (OverlayModes.Power.UpdateBatteryInfo updateBatteryInfo in this.updateBatteryInfo)
			{
				updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
			}
		}

		// Token: 0x0600944E RID: 37966 RVA: 0x00334F60 File Offset: 0x00333160
		private void AddPowerLabels(KMonoBehaviour item)
		{
			if (item.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				IEnergyConsumer componentInChildren = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
				Generator componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
				if (componentInChildren != null || componentInChildren2 != null)
				{
					float num = -10f;
					if (componentInChildren2 != null)
					{
						LocText freePowerLabel = this.GetFreePowerLabel();
						freePowerLabel.gameObject.SetActive(true);
						freePowerLabel.gameObject.name = item.gameObject.name + "power label";
						LocText component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
						component.gameObject.SetActive(true);
						freePowerLabel.enabled = true;
						component.enabled = true;
						Vector3 a = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel.rectTransform.SetPosition(a + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						if (componentInChildren != null && componentInChildren.PowerCell == componentInChildren2.PowerCell)
						{
							num -= 15f;
						}
						this.SetToolTip(freePowerLabel, UI.OVERLAYS.POWER.WATTS_GENERATED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel, component, componentInChildren2, null));
					}
					if (componentInChildren != null && componentInChildren.GetType() != typeof(Battery))
					{
						LocText freePowerLabel2 = this.GetFreePowerLabel();
						LocText component2 = freePowerLabel2.transform.GetChild(0).GetComponent<LocText>();
						freePowerLabel2.gameObject.SetActive(true);
						component2.gameObject.SetActive(true);
						freePowerLabel2.gameObject.name = item.gameObject.name + "power label";
						freePowerLabel2.enabled = true;
						component2.enabled = true;
						Vector3 a2 = Grid.CellToPos(componentInChildren.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel2.rectTransform.SetPosition(a2 + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						this.SetToolTip(freePowerLabel2, UI.OVERLAYS.POWER.WATTS_CONSUMED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel2, component2, null, componentInChildren));
					}
				}
			}
		}

		// Token: 0x0600944F RID: 37967 RVA: 0x003351AC File Offset: 0x003333AC
		private void DisablePowerLabels()
		{
			this.freePowerLabelIdx = 0;
			foreach (LocText locText in this.powerLabels)
			{
				locText.gameObject.SetActive(false);
			}
			this.updatePowerInfo.Clear();
		}

		// Token: 0x06009450 RID: 37968 RVA: 0x00335214 File Offset: 0x00333414
		private void AddBatteryUI(Battery bat)
		{
			BatteryUI freeBatteryUI = this.GetFreeBatteryUI();
			freeBatteryUI.SetContent(bat);
			Vector3 b = Grid.CellToPos(bat.PowerCell, 0.5f, 0f, 0f);
			bool flag = bat.powerTransformer != null;
			float num = 1f;
			Rotatable component = bat.GetComponent<Rotatable>();
			if (component != null && component.GetVisualizerFlipX())
			{
				num = -1f;
			}
			Vector3 b2 = this.batteryUIOffset;
			if (flag)
			{
				b2 = ((bat.GetComponent<Building>().Def.WidthInCells == 2) ? this.batteryUISmallTransformerOffset : this.batteryUITransformerOffset);
			}
			b2.x *= num;
			freeBatteryUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b + b2);
			this.updateBatteryInfo.Add(new OverlayModes.Power.UpdateBatteryInfo(bat, freeBatteryUI));
		}

		// Token: 0x06009451 RID: 37969 RVA: 0x003352E4 File Offset: 0x003334E4
		private void SetToolTip(LocText label, string text)
		{
			ToolTip component = label.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = text;
			}
		}

		// Token: 0x06009452 RID: 37970 RVA: 0x00335308 File Offset: 0x00333508
		private void DisableBatteryUIs()
		{
			this.freeBatteryUIIdx = 0;
			foreach (BatteryUI batteryUI in this.batteryUIList)
			{
				batteryUI.gameObject.SetActive(false);
			}
			this.updateBatteryInfo.Clear();
		}

		// Token: 0x06009453 RID: 37971 RVA: 0x00335370 File Offset: 0x00333570
		private BatteryUI GetFreeBatteryUI()
		{
			BatteryUI batteryUI;
			if (this.freeBatteryUIIdx < this.batteryUIList.Count)
			{
				batteryUI = this.batteryUIList[this.freeBatteryUIIdx];
				batteryUI.gameObject.SetActive(true);
				this.freeBatteryUIIdx++;
			}
			else
			{
				batteryUI = global::Util.KInstantiateUI<BatteryUI>(this.batteryUIPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.batteryUIList.Add(batteryUI);
				this.freeBatteryUIIdx++;
			}
			return batteryUI;
		}

		// Token: 0x06009454 RID: 37972 RVA: 0x00335400 File Offset: 0x00333600
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x04007463 RID: 29795
		public static readonly HashedString ID = "Power";

		// Token: 0x04007464 RID: 29796
		private int targetLayer;

		// Token: 0x04007465 RID: 29797
		private int cameraLayerMask;

		// Token: 0x04007466 RID: 29798
		private int selectionMask;

		// Token: 0x04007467 RID: 29799
		private List<OverlayModes.Power.UpdatePowerInfo> updatePowerInfo = new List<OverlayModes.Power.UpdatePowerInfo>();

		// Token: 0x04007468 RID: 29800
		private List<OverlayModes.Power.UpdateBatteryInfo> updateBatteryInfo = new List<OverlayModes.Power.UpdateBatteryInfo>();

		// Token: 0x04007469 RID: 29801
		private Canvas powerLabelParent;

		// Token: 0x0400746A RID: 29802
		private LocText powerLabelPrefab;

		// Token: 0x0400746B RID: 29803
		private Vector3 powerLabelOffset;

		// Token: 0x0400746C RID: 29804
		private BatteryUI batteryUIPrefab;

		// Token: 0x0400746D RID: 29805
		private Vector3 batteryUIOffset;

		// Token: 0x0400746E RID: 29806
		private Vector3 batteryUITransformerOffset;

		// Token: 0x0400746F RID: 29807
		private Vector3 batteryUISmallTransformerOffset;

		// Token: 0x04007470 RID: 29808
		private int freePowerLabelIdx;

		// Token: 0x04007471 RID: 29809
		private int freeBatteryUIIdx;

		// Token: 0x04007472 RID: 29810
		private List<LocText> powerLabels = new List<LocText>();

		// Token: 0x04007473 RID: 29811
		private List<BatteryUI> batteryUIList = new List<BatteryUI>();

		// Token: 0x04007474 RID: 29812
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04007475 RID: 29813
		private List<SaveLoadRoot> queuedAdds = new List<SaveLoadRoot>();

		// Token: 0x04007476 RID: 29814
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04007477 RID: 29815
		private HashSet<SaveLoadRoot> privateTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04007478 RID: 29816
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04007479 RID: 29817
		private List<int> visited = new List<int>();

		// Token: 0x02002221 RID: 8737
		private struct UpdatePowerInfo
		{
			// Token: 0x0600ACD7 RID: 44247 RVA: 0x003786DC File Offset: 0x003768DC
			public UpdatePowerInfo(KMonoBehaviour item, LocText power_label, LocText unit_label, Generator g, IEnergyConsumer c)
			{
				this.item = item;
				this.powerLabel = power_label;
				this.unitLabel = unit_label;
				this.generator = g;
				this.consumer = c;
			}

			// Token: 0x040098AD RID: 39085
			public KMonoBehaviour item;

			// Token: 0x040098AE RID: 39086
			public LocText powerLabel;

			// Token: 0x040098AF RID: 39087
			public LocText unitLabel;

			// Token: 0x040098B0 RID: 39088
			public Generator generator;

			// Token: 0x040098B1 RID: 39089
			public IEnergyConsumer consumer;
		}

		// Token: 0x02002222 RID: 8738
		private struct UpdateBatteryInfo
		{
			// Token: 0x0600ACD8 RID: 44248 RVA: 0x00378703 File Offset: 0x00376903
			public UpdateBatteryInfo(Battery battery, BatteryUI ui)
			{
				this.battery = battery;
				this.ui = ui;
			}

			// Token: 0x040098B2 RID: 39090
			public Battery battery;

			// Token: 0x040098B3 RID: 39091
			public BatteryUI ui;
		}
	}

	// Token: 0x0200192B RID: 6443
	public class Radiation : OverlayModes.Mode
	{
		// Token: 0x06009456 RID: 37974 RVA: 0x0033549E File Offset: 0x0033369E
		public override HashedString ViewMode()
		{
			return OverlayModes.Radiation.ID;
		}

		// Token: 0x06009457 RID: 37975 RVA: 0x003354A5 File Offset: 0x003336A5
		public override string GetSoundName()
		{
			return "Radiation";
		}

		// Token: 0x06009458 RID: 37976 RVA: 0x003354AC File Offset: 0x003336AC
		public override void Enable()
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterRadiationOn);
		}

		// Token: 0x06009459 RID: 37977 RVA: 0x003354C3 File Offset: 0x003336C3
		public override void Disable()
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterRadiationOn, STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x0400747A RID: 29818
		public static readonly HashedString ID = "Radiation";
	}

	// Token: 0x0200192C RID: 6444
	public class SolidConveyor : OverlayModes.Mode
	{
		// Token: 0x0600945C RID: 37980 RVA: 0x003354F4 File Offset: 0x003336F4
		public override HashedString ViewMode()
		{
			return OverlayModes.SolidConveyor.ID;
		}

		// Token: 0x0600945D RID: 37981 RVA: 0x003354FB File Offset: 0x003336FB
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x0600945E RID: 37982 RVA: 0x00335504 File Offset: 0x00333704
		public SolidConveyor()
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x0600945F RID: 37983 RVA: 0x0033559C File Offset: 0x0033379C
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x06009460 RID: 37984 RVA: 0x003355F8 File Offset: 0x003337F8
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x06009461 RID: 37985 RVA: 0x0033562C File Offset: 0x0033382C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x06009462 RID: 37986 RVA: 0x00335678 File Offset: 0x00333878
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x06009463 RID: 37987 RVA: 0x003356E0 File Offset: 0x003338E0
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				SolidConduit component = gameObject.GetComponent<SolidConduit>();
				if (component != null)
				{
					int cell = Grid.PosToCell(component);
					UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem = Game.Instance.solidConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(cell, solidConduitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					Color32 color = this.tint_color;
					SolidConduit component2 = saveLoadRoot.GetComponent<SolidConduit>();
					if (component2 != null)
					{
						if (this.connectedNetworks.Count > 0 && this.IsConnectedToNetworks(component2, this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
						saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = color;
					}
				}
			}
		}

		// Token: 0x06009464 RID: 37988 RVA: 0x00335904 File Offset: 0x00333B04
		public bool IsConnectedToNetworks(SolidConduit conduit, ICollection<UtilityNetwork> networks)
		{
			UtilityNetwork network = conduit.GetNetwork();
			return networks.Contains(network);
		}

		// Token: 0x06009465 RID: 37989 RVA: 0x00335920 File Offset: 0x00333B20
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						GameObject gameObject = networkItem.GameObject;
						if (gameObject != null)
						{
							IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
							if (component != null)
							{
								component.AddNetworks(networks);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400747B RID: 29819
		public static readonly HashedString ID = "SolidConveyor";

		// Token: 0x0400747C RID: 29820
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x0400747D RID: 29821
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x0400747E RID: 29822
		private ICollection<Tag> targetIDs = OverlayScreen.SolidConveyorIDs;

		// Token: 0x0400747F RID: 29823
		private Color32 tint_color = new Color32(201, 201, 201, 0);

		// Token: 0x04007480 RID: 29824
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04007481 RID: 29825
		private List<int> visited = new List<int>();

		// Token: 0x04007482 RID: 29826
		private int targetLayer;

		// Token: 0x04007483 RID: 29827
		private int cameraLayerMask;

		// Token: 0x04007484 RID: 29828
		private int selectionMask;
	}

	// Token: 0x0200192D RID: 6445
	public class Sound : OverlayModes.Mode
	{
		// Token: 0x06009467 RID: 37991 RVA: 0x003359FA File Offset: 0x00333BFA
		public override HashedString ViewMode()
		{
			return OverlayModes.Sound.ID;
		}

		// Token: 0x06009468 RID: 37992 RVA: 0x00335A01 File Offset: 0x00333C01
		public override string GetSoundName()
		{
			return "Sound";
		}

		// Token: 0x06009469 RID: 37993 RVA: 0x00335A08 File Offset: 0x00333C08
		public Sound()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour np)
			{
				Color black = Color.black;
				Color black2 = Color.black;
				float t = 0.8f;
				if (np != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					if ((np as NoisePolluter).GetNoiseForCell(cell) < 36f)
					{
						t = 1f;
						black2 = new Color(0.4f, 0.4f, 0.4f);
					}
				}
				return Color.Lerp(black, black2, t);
			}, delegate(KMonoBehaviour np)
			{
				List<GameObject> highlightedObjects = SelectToolHoverTextCard.highlightedObjects;
				bool result = false;
				for (int i = 0; i < highlightedObjects.Count; i++)
				{
					if (highlightedObjects[i] != null && highlightedObjects[i] == np.gameObject)
					{
						result = true;
						break;
					}
				}
				return result;
			});
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
		}

		// Token: 0x0600946A RID: 37994 RVA: 0x00335AC8 File Offset: 0x00333CC8
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			this.partition = OverlayModes.Mode.PopulatePartition<NoisePolluter>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600946B RID: 37995 RVA: 0x00335B18 File Offset: 0x00333D18
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<NoisePolluter>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				NoisePolluter instance = (NoisePolluter)obj;
				base.AddTargetIfVisible<NoisePolluter>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<NoisePolluter>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600946C RID: 37996 RVA: 0x00335BE8 File Offset: 0x00333DE8
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				NoisePolluter component = item.GetComponent<NoisePolluter>();
				this.partition.Add(component);
			}
		}

		// Token: 0x0600946D RID: 37997 RVA: 0x00335C24 File Offset: 0x00333E24
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			NoisePolluter component = item.GetComponent<NoisePolluter>();
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x0600946E RID: 37998 RVA: 0x00335C78 File Offset: 0x00333E78
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<NoisePolluter>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04007485 RID: 29829
		public static readonly HashedString ID = "Sound";

		// Token: 0x04007486 RID: 29830
		private UniformGrid<NoisePolluter> partition;

		// Token: 0x04007487 RID: 29831
		private HashSet<NoisePolluter> layerTargets = new HashSet<NoisePolluter>();

		// Token: 0x04007488 RID: 29832
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04007489 RID: 29833
		private int targetLayer;

		// Token: 0x0400748A RID: 29834
		private int cameraLayerMask;

		// Token: 0x0400748B RID: 29835
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x0200192E RID: 6446
	public class Suit : OverlayModes.Mode
	{
		// Token: 0x06009470 RID: 38000 RVA: 0x00335CD6 File Offset: 0x00333ED6
		public override HashedString ViewMode()
		{
			return OverlayModes.Suit.ID;
		}

		// Token: 0x06009471 RID: 38001 RVA: 0x00335CDD File Offset: 0x00333EDD
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x06009472 RID: 38002 RVA: 0x00335CE4 File Offset: 0x00333EE4
		public Suit(Canvas ui_parent, GameObject overlay_prefab)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = OverlayScreen.SuitIDs;
			this.uiParent = ui_parent;
			this.overlayPrefab = overlay_prefab;
		}

		// Token: 0x06009473 RID: 38003 RVA: 0x00335D64 File Offset: 0x00333F64
		public override void Enable()
		{
			this.partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			base.ProcessExistingSaveLoadRoots();
			base.RegisterSaveLoadListeners();
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x06009474 RID: 38004 RVA: 0x00335DCC File Offset: 0x00333FCC
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			this.partition.Clear();
			this.partition = null;
			this.layerTargets.Clear();
			for (int i = 0; i < this.uiList.Count; i++)
			{
				this.uiList[i].SetActive(false);
			}
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x06009475 RID: 38005 RVA: 0x00335E64 File Offset: 0x00334064
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x06009476 RID: 38006 RVA: 0x00335E98 File Offset: 0x00334098
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x06009477 RID: 38007 RVA: 0x00335EE4 File Offset: 0x003340E4
		private GameObject GetFreeUI()
		{
			GameObject gameObject;
			if (this.freeUiIdx >= this.uiList.Count)
			{
				gameObject = global::Util.KInstantiateUI(this.overlayPrefab, this.uiParent.transform.gameObject, false);
				this.uiList.Add(gameObject);
			}
			else
			{
				List<GameObject> list = this.uiList;
				int num = this.freeUiIdx;
				this.freeUiIdx = num + 1;
				gameObject = list[num];
			}
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x06009478 RID: 38008 RVA: 0x00335F60 File Offset: 0x00334160
		public override void Update()
		{
			this.freeUiIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = Color.white;
					bool flag = false;
					if (saveLoadRoot.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
					{
						flag = true;
					}
					else
					{
						SuitLocker component = saveLoadRoot.GetComponent<SuitLocker>();
						if (component != null)
						{
							flag = (component.GetStoredOutfit() != null);
						}
					}
					if (flag)
					{
						this.GetFreeUI().GetComponent<RectTransform>().SetPosition(saveLoadRoot.transform.GetPosition());
					}
				}
			}
			for (int i = this.freeUiIdx; i < this.uiList.Count; i++)
			{
				if (this.uiList[i].activeSelf)
				{
					this.uiList[i].SetActive(false);
				}
			}
		}

		// Token: 0x0400748C RID: 29836
		public static readonly HashedString ID = "Suit";

		// Token: 0x0400748D RID: 29837
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x0400748E RID: 29838
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x0400748F RID: 29839
		private ICollection<Tag> targetIDs;

		// Token: 0x04007490 RID: 29840
		private List<GameObject> uiList = new List<GameObject>();

		// Token: 0x04007491 RID: 29841
		private int freeUiIdx;

		// Token: 0x04007492 RID: 29842
		private int targetLayer;

		// Token: 0x04007493 RID: 29843
		private int cameraLayerMask;

		// Token: 0x04007494 RID: 29844
		private int selectionMask;

		// Token: 0x04007495 RID: 29845
		private Canvas uiParent;

		// Token: 0x04007496 RID: 29846
		private GameObject overlayPrefab;
	}

	// Token: 0x0200192F RID: 6447
	public class Temperature : OverlayModes.Mode
	{
		// Token: 0x0600947A RID: 38010 RVA: 0x0033612D File Offset: 0x0033432D
		public override HashedString ViewMode()
		{
			return OverlayModes.Temperature.ID;
		}

		// Token: 0x0600947B RID: 38011 RVA: 0x00336134 File Offset: 0x00334334
		public override string GetSoundName()
		{
			return "Temperature";
		}

		// Token: 0x0600947C RID: 38012 RVA: 0x0033613C File Offset: 0x0033433C
		public Temperature()
		{
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x0600947D RID: 38013 RVA: 0x0033662C File Offset: 0x0033482C
		public override void Enable()
		{
			base.Enable();
			int num = SimDebugView.Instance.temperatureThresholds.Length - 1;
			for (int i = 0; i < this.temperatureLegend.Count; i++)
			{
				this.temperatureLegend[i].colour = GlobalAssets.Instance.colorSet.GetColorByName(SimDebugView.Instance.temperatureThresholds[num - i].colorName);
				this.temperatureLegend[i].desc_arg = GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - i].value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			}
		}

		// Token: 0x0600947E RID: 38014 RVA: 0x003366D1 File Offset: 0x003348D1
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.HEATFLOW,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.STATECHANGE,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600947F RID: 38015 RVA: 0x003366FC File Offset: 0x003348FC
		public override List<LegendEntry> GetCustomLegendData()
		{
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				return this.temperatureLegend;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				return this.expandedTemperatureLegend;
			case Game.TemperatureOverlayModes.HeatFlow:
				return this.heatFlowLegend;
			case Game.TemperatureOverlayModes.StateChange:
				return this.stateChangeLegend;
			default:
				return this.temperatureLegend;
			}
		}

		// Token: 0x06009480 RID: 38016 RVA: 0x00336750 File Offset: 0x00334950
		public override void OnFiltersChanged()
		{
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;
			}
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.HeatFlow:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.StateChange:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06009481 RID: 38017 RVA: 0x00336852 File Offset: 0x00334A52
		public override void Disable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			CameraController.Instance.ToggleColouredOverlayView(false);
			base.Disable();
		}

		// Token: 0x04007497 RID: 29847
		public static readonly HashedString ID = "Temperature";

		// Token: 0x04007498 RID: 29848
		public List<LegendEntry> temperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true)
		};

		// Token: 0x04007499 RID: 29849
		public List<LegendEntry> heatFlowLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0.9098039f, 0.25882354f, 0.14901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0.30980393f, 0.30980393f, 0.30980393f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0.2509804f, 0.6313726f, 0.90588236f), null, null, true)
		};

		// Token: 0x0400749A RID: 29850
		public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true)
		};

		// Token: 0x0400749B RID: 29851
		public List<LegendEntry> stateChangeLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.STATECHANGE.HIGHPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.STABLE, UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.LOWPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true)
		};
	}

	// Token: 0x02001930 RID: 6448
	public class TileMode : OverlayModes.Mode
	{
		// Token: 0x06009483 RID: 38019 RVA: 0x00336881 File Offset: 0x00334A81
		public override HashedString ViewMode()
		{
			return OverlayModes.TileMode.ID;
		}

		// Token: 0x06009484 RID: 38020 RVA: 0x00336888 File Offset: 0x00334A88
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x06009485 RID: 38021 RVA: 0x00336890 File Offset: 0x00334A90
		public TileMode()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour primary_element)
			{
				Color result = Color.black;
				if (primary_element != null)
				{
					result = (primary_element as PrimaryElement).Element.substance.uiColour;
				}
				return result;
			}, (KMonoBehaviour primary_element) => primary_element.gameObject.GetComponent<KBatchedAnimController>().IsVisible());
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x06009486 RID: 38022 RVA: 0x00336948 File Offset: 0x00334B48
		public override void Enable()
		{
			base.Enable();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<PrimaryElement>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			Camera.main.cullingMask |= this.cameraLayerMask;
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x06009487 RID: 38023 RVA: 0x003369B0 File Offset: 0x00334BB0
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<PrimaryElement>(this.layerTargets, vector2I, vector2I2, null);
			int height = vector2I2.y - vector2I.y;
			int width = vector2I2.x - vector2I.x;
			Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
			List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry in list)
			{
				PrimaryElement component = ((Pickupable)scenePartitionerEntry.obj).gameObject.GetComponent<PrimaryElement>();
				if (component != null)
				{
					this.TryAddObject(component, vector2I, vector2I2);
				}
			}
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in list)
			{
				BuildingComplete buildingComplete = (BuildingComplete)scenePartitionerEntry2.obj;
				PrimaryElement component2 = buildingComplete.gameObject.GetComponent<PrimaryElement>();
				if (component2 != null && buildingComplete.gameObject.layer == 0)
				{
					this.TryAddObject(component2, vector2I, vector2I2);
				}
			}
			base.UpdateHighlightTypeOverlay<PrimaryElement>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x06009488 RID: 38024 RVA: 0x00336B3C File Offset: 0x00334D3C
		private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max)
		{
			Element element = pe.Element;
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					base.AddTargetIfVisible<PrimaryElement>(pe, min, max, this.layerTargets, this.targetLayer, null, null);
					break;
				}
			}
		}

		// Token: 0x06009489 RID: 38025 RVA: 0x00336BB8 File Offset: 0x00334DB8
		public override void Disable()
		{
			base.Disable();
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x0600948A RID: 38026 RVA: 0x00336C04 File Offset: 0x00334E04
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.METAL,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.BUILDABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FILTER,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.ORGANICS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FARMABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GAS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUID,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.MISC,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600948B RID: 38027 RVA: 0x00336C9C File Offset: 0x00334E9C
		public override void OnFiltersChanged()
		{
			Game.Instance.tileOverlayFilters.Clear();
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.METAL, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
				Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Filter);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Organics);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.GAS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.MISC, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Other);
			}
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			this.layerTargets.Clear();
			Game.Instance.ForceOverlayUpdate(false);
		}

		// Token: 0x0400749C RID: 29852
		public static readonly HashedString ID = "TileMode";

		// Token: 0x0400749D RID: 29853
		private HashSet<PrimaryElement> layerTargets = new HashSet<PrimaryElement>();

		// Token: 0x0400749E RID: 29854
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x0400749F RID: 29855
		private int targetLayer;

		// Token: 0x040074A0 RID: 29856
		private int cameraLayerMask;

		// Token: 0x040074A1 RID: 29857
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}
}
