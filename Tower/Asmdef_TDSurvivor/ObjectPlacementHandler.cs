using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class ObjectPlacementHandler : MonoBehaviour
{
	// Token: 0x0600045D RID: 1117 RVA: 0x00010FAC File Offset: 0x0000F1AC
	private void Awake()
	{
		EventMgr.Register<GameObject, Action>(eGameEvents.RequestStartPlacement, new Action<GameObject, Action>(this.OnStartPlacement));
		EventMgr.Register(eGameEvents.ConfirmPlacement, new Action(this.OnConfirmPlacement));
		EventMgr.Register(eGameEvents.CancelPlacement, new Action(this.OnCancelPlacement));
		EventMgr.Register<bool>(eGameEvents.RequestRotatePlacement, new Action<bool>(this.OnRequestRotatePlacement));
		EventMgr.Register<IDynamicPlacementTarget>(eGameEvents.RegisterDynamicPlacementObject, new Action<IDynamicPlacementTarget>(this.OnRegisterDynamicPlacementObject));
		EventMgr.Register<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, new Action<IDynamicPlacementTarget>(this.OnUnregisterDynamicPlacementObject));
		this.ignoreRaycastLayerMask = ~LayerMask.GetMask(new string[]
		{
			"Ground",
			"Monsters",
			"MouseRaycastOnly"
		});
		this.tetrisLayerMask = LayerMask.GetMask(new string[]
		{
			"Tetris"
		});
		this.obstacleLayerMask = LayerMask.GetMask(new string[]
		{
			"PathObstacle"
		});
		this.groundLayerMask = LayerMask.GetMask(new string[]
		{
			"Ground"
		});
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000110C0 File Offset: 0x0000F2C0
	private void OnDestroy()
	{
		EventMgr.Remove<GameObject, Action>(eGameEvents.RequestStartPlacement, new Action<GameObject, Action>(this.OnStartPlacement));
		EventMgr.Remove(eGameEvents.ConfirmPlacement, new Action(this.OnConfirmPlacement));
		EventMgr.Remove(eGameEvents.CancelPlacement, new Action(this.OnCancelPlacement));
		EventMgr.Remove<bool>(eGameEvents.RequestRotatePlacement, new Action<bool>(this.OnRequestRotatePlacement));
		EventMgr.Remove<IDynamicPlacementTarget>(eGameEvents.RegisterDynamicPlacementObject, new Action<IDynamicPlacementTarget>(this.OnRegisterDynamicPlacementObject));
		EventMgr.Remove<IDynamicPlacementTarget>(eGameEvents.UnregisterDynamicPlacementObject, new Action<IDynamicPlacementTarget>(this.OnUnregisterDynamicPlacementObject));
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x00011160 File Offset: 0x0000F360
	private void Update()
	{
		if (this.placementDemoObject != null)
		{
			if (this.isCurrentPlacementTower)
			{
				this.placementDemoObject.GetComponent<ABaseTower>().Collider.enabled = false;
			}
			if (this.dynamicPlacementTarget != null && this.isCurrentPlacementTower)
			{
				this.placementDemoObject.transform.position = this.dynamicPlacementTarget.GetPlacementTransform().position;
			}
			else
			{
				this.SetDemoObjectToMousePos();
			}
			this.CheckPlacementBlocking();
			if (Input.GetKeyDown(KeyCode.Q))
			{
				this.OnRequestRotatePlacement(false);
			}
			if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
			{
				this.OnRequestRotatePlacement(true);
			}
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00011204 File Offset: 0x0000F404
	private void CheckPlacementBlocking()
	{
		Physics.SyncTransforms();
		if (this.forceCheckBlock || this.lastMouseCellPosition != this.GetMouseCellPosition())
		{
			this.lastMouseCellPosition = this.GetMouseCellPosition();
			switch (this.placementDemoObject.GetComponent<IPlaceable>().GetPlaceableType())
			{
			default:
				Debug.LogError("沒有設定目前Placeable的類型");
				break;
			case ePlaceableType.TOWER:
				this.UpdateCheckTowerPlaceable();
				break;
			case ePlaceableType.TETRIS:
				this.UpdateCheckTetrisPlaceable();
				break;
			}
			this.forceCheckBlock = false;
		}
		this.lastMouseCellPosition = this.GetMouseCellPosition();
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00011290 File Offset: 0x0000F490
	private bool UpdateCheckTowerPlaceable()
	{
		List<Collider> placementColliders = this.placementDemoObject.GetComponent<IPlaceable>().GetPlacementColliders();
		this.isCurrentPlacementBlocked = false;
		if (placementColliders.Count == 1 && this.dynamicPlacementTarget != null)
		{
			this.isCurrentPlacementHaveObject = true;
		}
		else
		{
			this.isCurrentPlacementHaveObject = this.CheckIsUpperPlacementAvaliableAtPositon(this.placementDemoObject.GetComponent<IPlaceable>(), this.tetrisLayerMask, this.obstacleLayerMask);
		}
		if (this.isCurrentPlacementBlocked)
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.BLOCKED_PATH);
		}
		else if (!this.isCurrentPlacementHaveObject)
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.NO_SUITABLE_BASE);
		}
		else
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.AVALIABLE);
		}
		this.isCurrentPlacementAvaliable = (!this.isCurrentPlacementBlocked && this.isCurrentPlacementHaveObject);
		return this.isCurrentPlacementAvaliable;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00011348 File Offset: 0x0000F548
	private bool UpdateCheckTetrisPlaceable()
	{
		IPlaceable component = this.placementDemoObject.GetComponent<IPlaceable>();
		List<Collider> placementColliders = component.GetPlacementColliders();
		this.isCurrentPlacementBlocked = Singleton<MapManager>.Instance.CheckPathBlockedByObject(placementColliders, true);
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleControlTipError_PathBlocked, this.isCurrentPlacementBlocked);
		this.isCurrentPlacementHaveObject = this.CheckIsPlacementOccupiedAtPositon(component, this.ignoreRaycastLayerMask);
		bool flag = this.CheckGroundExistAtPositon(component, this.groundLayerMask);
		if (this.isCurrentPlacementBlocked)
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.BLOCKED_PATH);
		}
		else if (this.isCurrentPlacementHaveObject)
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.OVERLAP_OBJECT);
		}
		else if (!flag)
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.NO_GROUND);
		}
		else
		{
			this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.AVALIABLE);
		}
		this.isCurrentPlacementAvaliable = (!this.isCurrentPlacementBlocked && !this.isCurrentPlacementHaveObject);
		return this.isCurrentPlacementAvaliable;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001141C File Offset: 0x0000F61C
	private bool CheckGroundExistAtPositon(IPlaceable placeable, int raycastLayerMask)
	{
		bool result = true;
		List<Collider> collisionColliders = placeable.GetCollisionColliders();
		List<Collider> placementColliders = placeable.GetPlacementColliders();
		foreach (Collider collider in collisionColliders)
		{
			collider.enabled = false;
		}
		foreach (Collider collider2 in placementColliders)
		{
			collider2.enabled = false;
		}
		using (List<Collider>.Enumerator enumerator = placementColliders.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!Physics.Raycast(enumerator.Current.gameObject.transform.position + Vector3.up * 10f, Vector3.down, 20f, raycastLayerMask))
				{
					result = false;
					break;
				}
			}
		}
		foreach (Collider collider3 in collisionColliders)
		{
			collider3.enabled = true;
		}
		foreach (Collider collider4 in placementColliders)
		{
			collider4.enabled = true;
		}
		return result;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00011598 File Offset: 0x0000F798
	private bool CheckIsPlacementOccupiedAtPositon(IPlaceable placeable, int raycastLayerMask)
	{
		bool result = false;
		List<Collider> collisionColliders = placeable.GetCollisionColliders();
		List<Collider> placementColliders = placeable.GetPlacementColliders();
		foreach (Collider collider in collisionColliders)
		{
			collider.enabled = false;
		}
		foreach (Collider collider2 in placementColliders)
		{
			collider2.enabled = false;
		}
		using (List<Collider>.Enumerator enumerator = placementColliders.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Physics.Raycast(enumerator.Current.gameObject.transform.position + Vector3.up * 10f, Vector3.down, 100f, raycastLayerMask))
				{
					result = true;
					break;
				}
			}
		}
		foreach (Collider collider3 in collisionColliders)
		{
			collider3.enabled = true;
		}
		foreach (Collider collider4 in placementColliders)
		{
			collider4.enabled = true;
		}
		return result;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00011714 File Offset: 0x0000F914
	private bool CheckIsUpperPlacementAvaliableAtPositon(IPlaceable placeable, int tetrisLayerMask, int obstacleLayerMask)
	{
		bool result = true;
		List<Collider> collisionColliders = placeable.GetCollisionColliders();
		List<Collider> placementColliders = placeable.GetPlacementColliders();
		foreach (Collider collider in collisionColliders)
		{
			collider.enabled = false;
		}
		foreach (Collider collider2 in placementColliders)
		{
			collider2.enabled = false;
		}
		foreach (Collider collider3 in placementColliders)
		{
			Obj_TetrisBlock tetrisAtPosition = Singleton<GridSystem>.Instance.GetTetrisAtPosition(collider3.gameObject.transform.position);
			if (tetrisAtPosition != null && !tetrisAtPosition.IsTowerAttachable())
			{
				result = false;
				break;
			}
			if (!Physics.Raycast(collider3.gameObject.transform.position + Vector3.up * 10f, Vector3.down, 100f, tetrisLayerMask))
			{
				result = false;
				break;
			}
			if (Physics.Raycast(collider3.gameObject.transform.position + Vector3.up * 10f, Vector3.down, 100f, obstacleLayerMask))
			{
				result = false;
				break;
			}
		}
		foreach (Collider collider4 in collisionColliders)
		{
			collider4.enabled = true;
		}
		foreach (Collider collider5 in placementColliders)
		{
			collider5.enabled = true;
		}
		return result;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001190C File Offset: 0x0000FB0C
	private void SetDemoObjectToMousePos()
	{
		this.placementDemoObject.transform.position = this.GetMouseGridPosition().WithY(0f) + this.placementDemoObjectScpt.GetPlacementOffset();
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00011940 File Offset: 0x0000FB40
	public void OnStartPlacement(GameObject prefab, Action callback)
	{
		this.curPlacementPrefab = prefab;
		this.placementSuccessCallback = callback;
		if (this.placementDemoObject != null)
		{
			Object.Destroy(this.placementDemoObject);
		}
		this.placementDemoObject = Object.Instantiate<GameObject>(prefab, Vector3.up * 1000f, Quaternion.identity);
		this.placementDemoObjectScpt = this.placementDemoObject.GetComponent<IPlaceable>();
		if (this.placementDemoObjectScpt == null)
		{
			Debug.LogError("目前的放置物件沒有IPlaceable!!!");
		}
		this.placementDemoObjectScpt.SwitchToPlacementMode();
		this.curPlacementPrefab.SetActive(false);
		this.isCurrentPlacementTower = (this.placementDemoObject.GetComponent<ABaseTower>() != null);
		if (this.isCurrentPlacementTower)
		{
			EventMgr.SendEvent<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, true, UI_ControlTip.eControlTipType.PLACE_TOWER);
		}
		else
		{
			EventMgr.SendEvent<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, true, UI_ControlTip.eControlTipType.PLACE_BLOCK);
		}
		Component[] componentsInChildren = this.placementDemoObject.GetComponentsInChildren<Component>();
		float ringRange = 0f;
		foreach (Component component in componentsInChildren)
		{
			ABaseTower abaseTower = component as ABaseTower;
			if (abaseTower != null)
			{
				abaseTower.Collider.enabled = false;
				abaseTower.enabled = false;
				ringRange = abaseTower.SettingData.GetAttackRange(1f);
			}
			Obj_TetrisBlock obj_TetrisBlock = component as Obj_TetrisBlock;
			if (obj_TetrisBlock != null)
			{
				obj_TetrisBlock.enabled = false;
			}
			Animator animator = component as Animator;
			if (animator != null)
			{
				animator.enabled = false;
			}
		}
		Obj_PlacementEffect obj_PlacementEffect = Object.Instantiate<Obj_PlacementEffect>(this.placementEffectPrefab, this.placementDemoObject.transform.position, Quaternion.identity);
		obj_PlacementEffect.transform.SetParent(this.placementDemoObject.transform);
		obj_PlacementEffect.transform.localPosition = Vector3.up * 0.1f;
		this.placementEffectObject = obj_PlacementEffect.GetComponent<Obj_PlacementEffect>();
		this.placementEffectObject.Initialize(this.placementDemoObject.transform);
		this.placementEffectObject.SetRingRange(ringRange);
		this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.AVALIABLE);
		this.SetDemoObjectToMousePos();
		this.lastMouseCellPosition = this.GetMouseCellPosition();
		this.targetPlacementRotation = this.placementDemoObject.transform.rotation.eulerAngles;
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00011B4C File Offset: 0x0000FD4C
	private void OnConfirmPlacement()
	{
		GameObject gameObject = null;
		bool flag = false;
		if (this.rotateTween != null && this.rotateTween.IsActive())
		{
			this.rotateTween.Complete();
			this.forceCheckBlock = true;
			this.CheckPlacementBlocking();
		}
		if (this.isCurrentPlacementAvaliable)
		{
			Vector3 position = this.placementDemoObject.transform.position.WithY(0f) + this.placementDemoObjectScpt.GetPlacementOffset();
			this.curPlacementPrefab.SetActive(true);
			this.curPlacementPrefab.transform.position = position;
			this.curPlacementPrefab.transform.rotation = Quaternion.Euler(this.targetPlacementRotation);
			Physics.SyncTransforms();
			EventMgr.SendEvent<GameObject>(eGameEvents.OnGridObjectChanged, this.curPlacementPrefab);
			ABaseTower abaseTower;
			Obj_TetrisBlock obj_TetrisBlock;
			if (this.curPlacementPrefab.TryGetComponent<ABaseTower>(out abaseTower))
			{
				int cost = abaseTower.GetCost(1f);
				EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, -1 * cost);
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					cost = abaseTower.GetCost(1f);
					if (MainGame.Instance.IngameData.IsCoinEnough(cost))
					{
						gameObject = Object.Instantiate<GameObject>(this.curPlacementPrefab);
						flag = true;
					}
				}
				abaseTower.Spawn();
				EventMgr.SendEvent<ABaseTower>(eGameEvents.OnTowerPlaced, abaseTower);
				if (this.dynamicPlacementTarget != null)
				{
					abaseTower.transform.parent = this.dynamicPlacementTarget.GetPlacementTransform();
					abaseTower.transform.localPosition = Vector3.zero;
					abaseTower.transform.localRotation = Quaternion.identity;
					this.dynamicPlacementTarget.PlaceTower(abaseTower);
				}
			}
			else if (this.curPlacementPrefab.TryGetComponent<Obj_TetrisBlock>(out obj_TetrisBlock))
			{
				obj_TetrisBlock.Spawn();
				EventMgr.SendEvent<Obj_TetrisBlock>(eGameEvents.OnTetrisPlaced, obj_TetrisBlock);
			}
			this.curPlacementPrefab.GetComponent<IPlaceable>().OnPlacementProc();
		}
		else
		{
			ABaseTower abaseTower2;
			if (this.curPlacementPrefab.TryGetComponent<ABaseTower>(out abaseTower2) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				int cost2 = abaseTower2.GetCost(1f);
				if (MainGame.Instance.IngameData.IsCoinEnough(cost2))
				{
					gameObject = Object.Instantiate<GameObject>(this.curPlacementPrefab);
					flag = true;
				}
			}
			Object.Destroy(this.curPlacementPrefab);
		}
		this.placementEffectObject.SetStatus(Obj_PlacementEffect.eStatus.AVALIABLE);
		Object.Destroy(this.placementEffectObject);
		Object.Destroy(this.placementDemoObject);
		this.placementDemoObject = null;
		this.placementDemoObjectScpt = null;
		this.curPlacementPrefab = null;
		EventMgr.SendEvent<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, false, UI_ControlTip.eControlTipType.NONE);
		if (this.isCurrentPlacementAvaliable && this.placementSuccessCallback != null)
		{
			this.placementSuccessCallback();
			this.placementSuccessCallback = null;
		}
		if (flag && gameObject != null)
		{
			Debug.Log("再蓋一次");
			EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
			this.forceCheckBlock = true;
			this.OnStartPlacement(gameObject, null);
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00011E1B File Offset: 0x0001001B
	private void OnCancelPlacement()
	{
		Debug.Log("OnCancelPlacement");
		Object.Destroy(this.placementDemoObject);
		this.placementDemoObject = null;
		this.placementDemoObjectScpt = null;
		this.curPlacementPrefab = null;
		EventMgr.SendEvent<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, false, UI_ControlTip.eControlTipType.NONE);
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00011E5C File Offset: 0x0001005C
	private void OnRequestRotatePlacement(bool isClockwise = true)
	{
		if (this.placementDemoObject == null)
		{
			return;
		}
		if (isClockwise)
		{
			this.targetPlacementRotation += Vector3.up * 90f;
		}
		else
		{
			this.targetPlacementRotation += Vector3.up * -90f;
		}
		if (this.coroutine_Rotate != null)
		{
			base.StopCoroutine(this.coroutine_Rotate);
		}
		this.coroutine_Rotate = base.StartCoroutine(this.CR_RotatePlacement(this.targetPlacementRotation));
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00011EE9 File Offset: 0x000100E9
	private IEnumerator CR_RotatePlacement(Vector3 targetRotation)
	{
		float duration = 0.3f;
		this.isRotating = true;
		if (this.rotateTween != null && this.rotateTween.IsActive())
		{
			this.rotateTween.Complete();
		}
		SoundManager.PlaySound("Block", "BlockRotate", -1f, -1f, -1f);
		this.rotateTween = this.placementDemoObject.transform.DORotate(targetRotation, duration, RotateMode.Fast).SetEase(Ease.OutElastic, 0.5f, 0f);
		yield return new WaitForSeconds(0.1f);
		this.forceCheckBlock = true;
		yield return new WaitForSeconds(duration - 0.1f);
		this.forceCheckBlock = true;
		this.isRotating = false;
		yield break;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00011EFF File Offset: 0x000100FF
	private void OnRegisterDynamicPlacementObject(IDynamicPlacementTarget target)
	{
		this.dynamicPlacementTarget = target;
		this.forceCheckBlock = true;
		Debug.Log("註冊動態物件");
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00011F19 File Offset: 0x00010119
	private void OnUnregisterDynamicPlacementObject(IDynamicPlacementTarget target)
	{
		if (this.dynamicPlacementTarget == target)
		{
			this.dynamicPlacementTarget = null;
			this.forceCheckBlock = true;
			Debug.Log("解除註冊動態物件");
		}
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00011F3C File Offset: 0x0001013C
	public Vector3 GetMouseGridPosition()
	{
		return Singleton<GridSystem>.Instance.GetGridPos(this.GetMouseAtWorldPosition());
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00011F4E File Offset: 0x0001014E
	public Vector3Int GetMouseCellPosition()
	{
		return Singleton<GridSystem>.Instance.GetGridCell(this.GetMouseAtWorldPosition());
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00011F60 File Offset: 0x00010160
	public Vector3 GetMouseAtWorldPosition()
	{
		Plane plane = new Plane(Vector3.up, this.placementDemoObjectScpt.GetPlacementOffset());
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		float distance;
		if (plane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}
		Debug.LogError("NoMousePos");
		return Vector3.zero;
	}

	// Token: 0x0400043F RID: 1087
	[SerializeField]
	private Obj_PlacementEffect placementEffectPrefab;

	// Token: 0x04000440 RID: 1088
	private GameObject curPlacementPrefab;

	// Token: 0x04000441 RID: 1089
	private GameObject placementDemoObject;

	// Token: 0x04000442 RID: 1090
	private IPlaceable placementDemoObjectScpt;

	// Token: 0x04000443 RID: 1091
	private Obj_PlacementEffect placementEffectObject;

	// Token: 0x04000444 RID: 1092
	private Plane plane_Height_0 = new Plane(Vector3.up, 0f);

	// Token: 0x04000445 RID: 1093
	private Plane plane_Height_1 = new Plane(Vector3.up, 0f);

	// Token: 0x04000446 RID: 1094
	private Vector3Int lastMouseCellPosition;

	// Token: 0x04000447 RID: 1095
	private bool isCurrentPlacementBlocked;

	// Token: 0x04000448 RID: 1096
	private bool isCurrentPlacementHaveObject;

	// Token: 0x04000449 RID: 1097
	private bool isCurrentPlacementAvaliable;

	// Token: 0x0400044A RID: 1098
	private int ignoreRaycastLayerMask;

	// Token: 0x0400044B RID: 1099
	private int tetrisLayerMask;

	// Token: 0x0400044C RID: 1100
	private int obstacleLayerMask;

	// Token: 0x0400044D RID: 1101
	private int groundLayerMask;

	// Token: 0x0400044E RID: 1102
	private bool forceCheckBlock;

	// Token: 0x0400044F RID: 1103
	private Action placementSuccessCallback;

	// Token: 0x04000450 RID: 1104
	private bool isCurrentPlacementTower;

	// Token: 0x04000451 RID: 1105
	private IDynamicPlacementTarget dynamicPlacementTarget;

	// Token: 0x04000452 RID: 1106
	private bool isRotating;

	// Token: 0x04000453 RID: 1107
	private Vector3 targetPlacementRotation = Vector3.zero;

	// Token: 0x04000454 RID: 1108
	private Coroutine coroutine_Rotate;

	// Token: 0x04000455 RID: 1109
	private Tween rotateTween;
}
