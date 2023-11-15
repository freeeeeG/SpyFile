using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class Obj_TetrisBlock : MonoBehaviour, IPlaceable
{
	// Token: 0x060004B9 RID: 1209 RVA: 0x000131A4 File Offset: 0x000113A4
	private void Awake()
	{
		this.list_BlockRenderers = new List<Renderer>();
		foreach (Collider collider in this.list_Colliders)
		{
			Renderer component = collider.gameObject.GetComponent<Renderer>();
			this.list_BlockRenderers.Add(component);
		}
		if (this.material_Runtime == null)
		{
			this.material_Runtime = new Material(this.list_BlockRenderers[0].material);
		}
		foreach (Renderer renderer in this.list_BlockRenderers)
		{
			renderer.material = this.material_Runtime;
		}
		this.isFrozen = false;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00013288 File Offset: 0x00011488
	protected void OnEnable()
	{
		this.animator.SetBool("isOn", true);
		Action<Obj_TetrisBlock> onPlacement = this.OnPlacement;
		if (onPlacement != null)
		{
			onPlacement(this);
		}
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x000132C5 File Offset: 0x000114C5
	protected void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		Action<Obj_TetrisBlock> onRemove = this.OnRemove;
		if (onRemove == null)
		{
			return;
		}
		onRemove(this);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x000132F0 File Offset: 0x000114F0
	private void Update()
	{
		if (this.isMouseDown)
		{
			this.mouseDownTimer += Time.deltaTime;
		}
		if (this.mouseDownTimer > 1.2f)
		{
			this.isMouseDown = false;
			this.mouseDownTimer = 0f;
			this.Recall();
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001333C File Offset: 0x0001153C
	public void Spawn()
	{
		SoundManager.PlaySound("Block", "BlockShow", -1f, -1f, -1f);
		SoundManager.PlaySound("Block", "BlockLandOnGround", -1f, -1f, 0.33f);
		Singleton<GridSystem>.Instance.RegisterTetris(this);
		foreach (Collider collider in this.list_Colliders)
		{
			if (Singleton<GridSystem>.Instance.IsHavePowerGridAtPosition(collider.transform.position))
			{
				Singleton<GridSystem>.Instance.GetPowerGridObjectAtPosition(collider.transform.position).OnTetrisPlaced(this);
			}
		}
		this.isFirstRoundAfterPlacement = true;
		if (Singleton<GameStateController>.Instance.IsInBattle)
		{
			this.ChangeMaterialToStone();
		}
		base.StartCoroutine(this.CR_PlacementEffect());
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0001342C File Offset: 0x0001162C
	private IEnumerator CR_PlacementEffect()
	{
		if (this.particle_PlacementEffect != null)
		{
			yield return new WaitForSeconds(0.33f);
			this.particle_PlacementEffect.Play();
		}
		yield break;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0001343B File Offset: 0x0001163B
	protected void OnBattleStart()
	{
		if (this.IsFrozen())
		{
			return;
		}
		if (!this.isFirstRoundAfterPlacement)
		{
			return;
		}
		this.ChangeMaterialToStone();
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00013455 File Offset: 0x00011655
	protected void ChangeMaterialToStone()
	{
		this.isFirstRoundAfterPlacement = false;
		base.StartCoroutine(this.CR_ChangeMaterialToStone());
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0001346B File Offset: 0x0001166B
	private IEnumerator CR_ChangeMaterialToStone()
	{
		float time = 0f;
		float duration = 1f;
		while (time <= duration)
		{
			float t = time / duration * 0.45f;
			this.ModifyMaterial("_Stone", Easing.GetEasingFunction(Easing.Type.EaseOutCubic, t));
			yield return null;
			time += Time.deltaTime;
		}
		yield break;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0001347C File Offset: 0x0001167C
	public void OnChildMouseEnter()
	{
		if (!this.IsRecallAble())
		{
			return;
		}
		EventMgr.SendEvent<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, this.list_BlockRenderers, OutlineController.eOutlineType.BASIC);
		this.isOutlineOn = true;
		string @string = LocalizationManager.Instance.GetString("UI", "TOOLTIP_LONGPRESS_TO_RECALL_TETRIS", Array.Empty<object>());
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, "", @string);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._3D, base.transform, Vector3.up * 2f);
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00013517 File Offset: 0x00011717
	public void OnChildMouseExit()
	{
		if (this.isOutlineOn)
		{
			EventMgr.SendEvent<List<Renderer>>(eGameEvents.RequestRemoveOutlineByList, this.list_BlockRenderers);
			this.isOutlineOn = false;
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00013550 File Offset: 0x00011750
	public void OnChildMouseDown()
	{
		if (!this.IsRecallAble())
		{
			return;
		}
		this.isMouseDown = true;
		this.mouseDownTimer = 0f;
		this.animator.SetBool("isClicked", true);
		this.blockClickSndIndex = SoundManager.PlaySound("Block", "BlockClick", -1f, -1f, -1f);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000135AD File Offset: 0x000117AD
	public void OnChildMouseUp()
	{
		if (!this.IsRecallAble())
		{
			return;
		}
		this.isMouseDown = false;
		this.animator.SetBool("isClicked", false);
		SoundManager.StopSound(this.blockClickSndIndex);
		this.mouseDownTimer = 0f;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x000135E8 File Offset: 0x000117E8
	public void Remove()
	{
		Singleton<GridSystem>.Instance.UnregisterTetris(this);
		foreach (Collider collider in this.list_Colliders)
		{
			collider.enabled = false;
		}
		EventMgr.SendEvent<GameObject>(eGameEvents.OnGridObjectChanged, base.gameObject);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00013664 File Offset: 0x00011864
	public bool IsRecallAble()
	{
		return this.isFirstRoundAfterPlacement && !Singleton<GameStateController>.Instance.IsInBattle && Singleton<GameStateController>.Instance.IsCurrentState(eGameState.NORMAL_MODE) && this.list_TowerOnBlock.Count == 0 && !this.isFrozen;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001369F File Offset: 0x0001189F
	protected void Recall()
	{
		base.StartCoroutine(this.CR_Recall());
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000136AE File Offset: 0x000118AE
	private IEnumerator CR_Recall()
	{
		Singleton<GridSystem>.Instance.UnregisterTetris(this);
		foreach (Collider collider in this.list_Colliders)
		{
			collider.enabled = false;
		}
		EventMgr.SendEvent<GameObject>(eGameEvents.OnGridObjectChanged, base.gameObject);
		this.animator.SetTrigger("Recall");
		SoundManager.PlaySound("Block", "BlockRecall", -1f, -1f, -1f);
		yield return new WaitForSeconds(0.33f);
		EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToHand, this.itemType);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x000136BD File Offset: 0x000118BD
	public void RegisterTowerOnTop(ABaseTower tower)
	{
		this.list_TowerOnBlock.Add(tower);
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x000136CB File Offset: 0x000118CB
	public void UnregisterTowerOnTop(ABaseTower tower)
	{
		this.list_TowerOnBlock.Remove(tower);
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000136DA File Offset: 0x000118DA
	public bool IsFrozen()
	{
		return this.isFrozen;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000136E2 File Offset: 0x000118E2
	public void FreezeBlock()
	{
		if (this.isFrozen)
		{
			return;
		}
		this.isFrozen = true;
		base.StartCoroutine(this.CR_FreezeBlock());
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00013701 File Offset: 0x00011901
	private IEnumerator CR_FreezeBlock()
	{
		float time = 0f;
		float duration = 1f;
		SoundManager.PlaySound("Block", "BlockFreeze", -1f, -1f, -1f);
		while (time <= duration)
		{
			float t = time / duration;
			this.ModifyMaterial("_Freeze", Easing.GetEasingFunction(Easing.Type.EaseOutCubic, t));
			yield return null;
			time += Time.deltaTime;
		}
		yield break;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00013710 File Offset: 0x00011910
	private void ModifyMaterial(string key, float level)
	{
		foreach (Renderer renderer in this.list_BlockRenderers)
		{
			renderer.material.SetFloat(key, level);
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00013768 File Offset: 0x00011968
	public bool IsTowerAttachable()
	{
		return !this.isFrozen;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00013773 File Offset: 0x00011973
	public List<Collider> GetCollisionColliders()
	{
		return this.list_Colliders;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0001377B File Offset: 0x0001197B
	public List<Collider> GetPlacementColliders()
	{
		return this.list_Colliders;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00013783 File Offset: 0x00011983
	public ePlaceableType GetPlaceableType()
	{
		return ePlaceableType.TETRIS;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00013786 File Offset: 0x00011986
	public Vector3 GetPlacementOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0001378D File Offset: 0x0001198D
	public void SwitchToPlacementMode()
	{
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001378F File Offset: 0x0001198F
	public void OnPlacementProc()
	{
		Singleton<CameraManager>.Instance.ShakeCamera(0.1f, 0.005f, 0.33f);
	}

	// Token: 0x04000499 RID: 1177
	[SerializeField]
	[Header("物品類型")]
	protected eItemType itemType;

	// Token: 0x0400049A RID: 1178
	[SerializeField]
	protected Animator animator;

	// Token: 0x0400049B RID: 1179
	[SerializeField]
	protected List<Collider> list_Colliders;

	// Token: 0x0400049C RID: 1180
	[SerializeField]
	[Header("放在上面的砲塔")]
	protected List<ABaseTower> list_TowerOnBlock;

	// Token: 0x0400049D RID: 1181
	[SerializeField]
	private ParticleSystem particle_PlacementEffect;

	// Token: 0x0400049E RID: 1182
	public Action<Obj_TetrisBlock> OnPlacement;

	// Token: 0x0400049F RID: 1183
	public Action<Obj_TetrisBlock> OnRemove;

	// Token: 0x040004A0 RID: 1184
	protected List<Renderer> list_BlockRenderers;

	// Token: 0x040004A1 RID: 1185
	protected bool isFirstRoundAfterPlacement;

	// Token: 0x040004A2 RID: 1186
	protected bool isOutlineOn;

	// Token: 0x040004A3 RID: 1187
	protected bool isFrozen;

	// Token: 0x040004A4 RID: 1188
	protected Material material_Runtime;

	// Token: 0x040004A5 RID: 1189
	private bool isMouseDown;

	// Token: 0x040004A6 RID: 1190
	private float mouseDownTimer;

	// Token: 0x040004A7 RID: 1191
	private int blockClickSndIndex = -1;
}
