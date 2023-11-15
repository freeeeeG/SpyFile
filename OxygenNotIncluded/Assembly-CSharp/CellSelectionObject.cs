using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
[AddComponentMenu("KMonoBehaviour/scripts/CellSelectionObject")]
public class CellSelectionObject : KMonoBehaviour
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000F8CFF File Offset: 0x000F6EFF
	public int SelectedCell
	{
		get
		{
			return this.selectedCell;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06002F35 RID: 12085 RVA: 0x000F8D07 File Offset: 0x000F6F07
	public float FlowRate
	{
		get
		{
			return Grid.AccumulatedFlow[this.selectedCell] / 3f;
		}
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x000F8D20 File Offset: 0x000F6F20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.mCollider = base.GetComponent<KBoxCollider2D>();
		this.mCollider.size = new Vector2(1.1f, 1.1f);
		this.mSelectable = base.GetComponent<KSelectable>();
		this.SelectedDisplaySprite.transform.localScale = Vector3.one * 0.390625f;
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		base.Subscribe(Game.Instance.gameObject, 493375141, new Action<object>(this.ForceRefreshUserMenu));
		this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, () => Grid.Element[this.mouseCell].IsGas);
		this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, () => Grid.Element[this.mouseCell].IsLiquid);
		if (CellSelectionObject.selectionObjectA == null)
		{
			CellSelectionObject.selectionObjectA = this;
			return;
		}
		if (CellSelectionObject.selectionObjectB == null)
		{
			CellSelectionObject.selectionObjectB = this;
			return;
		}
		global::Debug.LogError("CellSelectionObjects not properly cleaned up.");
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x000F8E42 File Offset: 0x000F7042
	protected override void OnCleanUp()
	{
		CellSelectionObject.selectionObjectA = null;
		CellSelectionObject.selectionObjectB = null;
		base.OnCleanUp();
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x000F8E56 File Offset: 0x000F7056
	public static bool IsSelectionObject(GameObject testObject)
	{
		return testObject == CellSelectionObject.selectionObjectA.gameObject || testObject == CellSelectionObject.selectionObjectB.gameObject;
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x000F8E7C File Offset: 0x000F707C
	private void OnApplicationFocus(bool focusStatus)
	{
		this.isAppFocused = focusStatus;
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x000F8E88 File Offset: 0x000F7088
	private void Update()
	{
		if (!this.isAppFocused || SelectTool.Instance == null)
		{
			return;
		}
		if (Game.Instance == null || !Game.Instance.GameStarted())
		{
			return;
		}
		this.SelectedDisplaySprite.SetActive(PlayerController.Instance.IsUsingDefaultTool() && !DebugHandler.HideUI);
		if (SelectTool.Instance.selected != this.mSelectable)
		{
			this.mouseCell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(this.mouseCell) && Grid.IsVisible(this.mouseCell))
			{
				bool flag = true;
				foreach (KeyValuePair<HashedString, Func<bool>> keyValuePair in this.overlayFilterMap)
				{
					if (keyValuePair.Value == null)
					{
						global::Debug.LogWarning("Filter value is null");
					}
					else if (OverlayScreen.Instance == null)
					{
						global::Debug.LogWarning("Overlay screen Instance is null");
					}
					else if (OverlayScreen.Instance.GetMode() == keyValuePair.Key)
					{
						flag = false;
						if (base.gameObject.layer != LayerMask.NameToLayer("MaskedOverlay"))
						{
							base.gameObject.layer = LayerMask.NameToLayer("MaskedOverlay");
						}
						if (!keyValuePair.Value())
						{
							this.SelectedDisplaySprite.SetActive(false);
							return;
						}
						break;
					}
				}
				if (flag && base.gameObject.layer != LayerMask.NameToLayer("Default"))
				{
					base.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				Vector3 position = Grid.CellToPos(this.mouseCell, 0f, 0f, 0f) + this.offset;
				position.z = this.zDepth;
				base.transform.SetPosition(position);
				this.mSelectable.SetName(Grid.Element[this.mouseCell].name);
			}
			if (SelectTool.Instance.hover != this.mSelectable)
			{
				this.SelectedDisplaySprite.SetActive(false);
			}
		}
		this.updateTimer += Time.deltaTime;
		if (this.updateTimer >= 0.5f)
		{
			this.updateTimer = 0f;
			if (SelectTool.Instance.selected == this.mSelectable)
			{
				this.UpdateValues();
			}
		}
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x000F9110 File Offset: 0x000F7310
	public void UpdateValues()
	{
		if (!Grid.IsValidCell(this.selectedCell))
		{
			return;
		}
		this.Mass = Grid.Mass[this.selectedCell];
		this.element = Grid.Element[this.selectedCell];
		this.ElementName = this.element.name;
		this.state = this.element.state;
		this.tags = this.element.GetMaterialCategoryTag();
		this.temperature = Grid.Temperature[this.selectedCell];
		this.diseaseIdx = Grid.DiseaseIdx[this.selectedCell];
		this.diseaseCount = Grid.DiseaseCount[this.selectedCell];
		this.mSelectable.SetName(Grid.Element[this.selectedCell].name);
		DetailsScreen.Instance.Trigger(-1514841199, null);
		this.UpdateStatusItem();
		int num = Grid.CellAbove(this.selectedCell);
		bool flag = this.element.IsLiquid && Grid.IsValidCell(num) && (Grid.Element[num].IsGas || Grid.Element[num].IsVacuum);
		if (this.element.sublimateId != (SimHashes)0 && (this.element.IsSolid || flag))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, this);
			bool flag2;
			bool flag3;
			GameUtil.IsEmissionBlocked(this.selectedCell, out flag2, out flag3);
			if (flag2)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
			}
			else if (flag3)
			{
				this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, this);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
			else
			{
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
				this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			}
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationEmitting, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationBlocked, false);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.SublimationOverpressure, false);
		}
		if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(this.selectedCell))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.BuriedItem, this);
		}
		else
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.BuriedItem, true);
		}
		bool on = CellSelectionObject.IsExposedToSpace(this.selectedCell);
		this.mSelectable.ToggleStatusItem(Db.Get().MiscStatusItems.Space, on, null);
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x000F9412 File Offset: 0x000F7612
	public static bool IsExposedToSpace(int cell)
	{
		return Game.Instance.world.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space && Grid.Objects[cell, 2] == null;
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x000F9440 File Offset: 0x000F7640
	private void UpdateStatusItem()
	{
		if (this.element.id == SimHashes.Vacuum || this.element.id == SimHashes.Void)
		{
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalCategory, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalMass, true);
			this.mSelectable.RemoveStatusItem(Db.Get().MiscStatusItems.ElementalDisease, true);
			return;
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalCategory))
		{
			Func<Element> data = () => this.element;
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalCategory, data);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalTemperature))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalTemperature, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalMass))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalMass, this);
		}
		if (!this.mSelectable.HasStatusItem(Db.Get().MiscStatusItems.ElementalDisease))
		{
			this.mSelectable.AddStatusItem(Db.Get().MiscStatusItems.ElementalDisease, this);
		}
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x000F95D0 File Offset: 0x000F77D0
	public void OnObjectSelected(object o)
	{
		this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Hover;
		this.UpdateStatusItem();
		if (SelectTool.Instance.selected == this.mSelectable)
		{
			this.selectedCell = Grid.PosToCell(base.gameObject);
			this.UpdateValues();
			Vector3 position = Grid.CellToPos(this.selectedCell, 0f, 0f, 0f) + this.offset;
			position.z = this.zDepthSelected;
			base.transform.SetPosition(position);
			this.SelectedDisplaySprite.GetComponent<SpriteRenderer>().sprite = this.Sprite_Selected;
		}
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x000F967D File Offset: 0x000F787D
	public string MassString()
	{
		return string.Format("{0:0.00}", this.Mass);
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x000F9694 File Offset: 0x000F7894
	private void ForceRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x04001BFE RID: 7166
	private static CellSelectionObject selectionObjectA;

	// Token: 0x04001BFF RID: 7167
	private static CellSelectionObject selectionObjectB;

	// Token: 0x04001C00 RID: 7168
	[HideInInspector]
	public CellSelectionObject alternateSelectionObject;

	// Token: 0x04001C01 RID: 7169
	private float zDepth = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection) - 0.5f;

	// Token: 0x04001C02 RID: 7170
	private float zDepthSelected = Grid.GetLayerZ(Grid.SceneLayer.WorldSelection);

	// Token: 0x04001C03 RID: 7171
	private KBoxCollider2D mCollider;

	// Token: 0x04001C04 RID: 7172
	private KSelectable mSelectable;

	// Token: 0x04001C05 RID: 7173
	private Vector3 offset = new Vector3(0.5f, 0.5f, 0f);

	// Token: 0x04001C06 RID: 7174
	public GameObject SelectedDisplaySprite;

	// Token: 0x04001C07 RID: 7175
	public Sprite Sprite_Selected;

	// Token: 0x04001C08 RID: 7176
	public Sprite Sprite_Hover;

	// Token: 0x04001C09 RID: 7177
	public int mouseCell;

	// Token: 0x04001C0A RID: 7178
	private int selectedCell;

	// Token: 0x04001C0B RID: 7179
	public string ElementName;

	// Token: 0x04001C0C RID: 7180
	public Element element;

	// Token: 0x04001C0D RID: 7181
	public Element.State state;

	// Token: 0x04001C0E RID: 7182
	public float Mass;

	// Token: 0x04001C0F RID: 7183
	public float temperature;

	// Token: 0x04001C10 RID: 7184
	public Tag tags;

	// Token: 0x04001C11 RID: 7185
	public byte diseaseIdx;

	// Token: 0x04001C12 RID: 7186
	public int diseaseCount;

	// Token: 0x04001C13 RID: 7187
	private float updateTimer;

	// Token: 0x04001C14 RID: 7188
	private Dictionary<HashedString, Func<bool>> overlayFilterMap = new Dictionary<HashedString, Func<bool>>();

	// Token: 0x04001C15 RID: 7189
	private bool isAppFocused = true;
}
