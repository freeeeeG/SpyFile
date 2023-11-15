using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lean.Touch;
using UnityEngine;

// Token: 0x0200010C RID: 268
[SelectionBase]
public abstract class ABaseTower : MonoBehaviour, IPlaceable
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00018A51 File Offset: 0x00016C51
	public TowerSettingData SettingData
	{
		get
		{
			return this.settingData;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00018A59 File Offset: 0x00016C59
	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00018A61 File Offset: 0x00016C61
	public Renderer Renderer_Tower
	{
		get
		{
			return this.renderer_Tower;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00018A69 File Offset: 0x00016C69
	public List<Renderer> List_TowerRenderers
	{
		get
		{
			return this.list_TowerRenderers;
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00018A71 File Offset: 0x00016C71
	public Collider Collider
	{
		get
		{
			return this.collider;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00018A79 File Offset: 0x00016C79
	public Vector3 ShootWorldPosition
	{
		get
		{
			return this.node_ShootPosition.position;
		}
	}

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060006D8 RID: 1752 RVA: 0x00018A86 File Offset: 0x00016C86
	public bool DoRotate
	{
		get
		{
			return this.doRotate;
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00018A8E File Offset: 0x00016C8E
	public eTowerTargetPriority TargetPriority
	{
		get
		{
			return this.targetPriority;
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x00018A96 File Offset: 0x00016C96
	public bool IsInitialized
	{
		get
		{
			return this.isInitialized;
		}
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00018AA0 File Offset: 0x00016CA0
	protected void Awake()
	{
		this.settingData = Object.Instantiate<TowerSettingData>(this.settingData);
		if (this.settingData.GetBulletPrefab() == null)
		{
			Debug.LogError("沒有指定砲台" + base.gameObject.name + "的子彈Prefab!!", base.gameObject);
		}
		if (base.gameObject.GetComponent<TowerBuffModule>() == null)
		{
			base.gameObject.AddComponent<TowerBuffModule>();
		}
		foreach (Collider collider in this.list_PlacementColliders)
		{
			collider.enabled = false;
		}
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00018B5C File Offset: 0x00016D5C
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Register(eGameEvents.OnRoundEnd, new Action(this.OnRoundEnd));
		EventMgr.Register<ABaseTower>(eGameEvents.OnTowerPlaced, new Action<ABaseTower>(this.OnTowerPlaced));
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00018BB4 File Offset: 0x00016DB4
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Remove(eGameEvents.OnRoundEnd, new Action(this.OnRoundEnd));
		EventMgr.Remove<ABaseTower>(eGameEvents.OnTowerPlaced, new Action<ABaseTower>(this.OnTowerPlaced));
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00018C0C File Offset: 0x00016E0C
	private void LateUpdate()
	{
		if (this.isInitialized && this.doRotate && !Singleton<GameStateController>.Instance.IsInBattle)
		{
			this.idleAnimationTimer -= Time.deltaTime;
			if (this.idleAnimationTimer <= 0f)
			{
				this.idleAnimationTimer = Random.Range(3f, 8f);
				this.node_CannonHeadModel.DOLocalRotate(this.startRotation + Vector3.up * Random.Range(-30f, 30f), Random.Range(0.5f, 1.5f), RotateMode.Fast).SetEase(Ease.OutBack);
			}
		}
		if (this.isInitialized && this.tooltipDelayOnPlacement > 0f)
		{
			this.tooltipDelayOnPlacement -= Time.deltaTime;
		}
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00018CDD File Offset: 0x00016EDD
	private void OnBattleStart()
	{
		this.deployedRoundCount++;
		this.OnBattleStartProc();
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00018CF3 File Offset: 0x00016EF3
	private void OnRoundStart(int arg1, int arg2)
	{
		this.OnRoundStartProc();
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x00018CFB File Offset: 0x00016EFB
	private void OnRoundEnd()
	{
		this.OnRoundEndProc();
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x00018D04 File Offset: 0x00016F04
	private void OnTowerPlaced(ABaseTower tower)
	{
		if (Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.settingData.TowerSizeType != eTowerSizeType._1x1)
		{
			return;
		}
		Vector3 position = tower.transform.position;
		bool flag = tower.SettingData.TowerSizeType >= eTowerSizeType._2x2;
		float num = 7f;
		float num2 = Vector3.Distance(base.transform.position, position);
		float num3 = num2 / num;
		if (flag && num2 < num)
		{
			base.transform.DOJump(base.transform.position, 0.66f * (1f - num3), 1, 0.5f * (1f - num3), false).SetDelay(0.66f + 0.33f * num3);
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x00018DB4 File Offset: 0x00016FB4
	public void Spawn()
	{
		this.CannonSpawnProc();
		this.isInitialized = true;
		this.shootIndex = 0;
		this.bulletIndex = 0;
		this.shootTimer = 1f;
		this.ApplyTowerTalentBuff();
		this.deployedRoundCount = (Singleton<GameStateController>.Instance.IsInBattle ? 1 : 0);
		SoundManager.PlaySound("Cannon", "BuildTower", -1f, -1f, 0.33f);
		this.animator.SetTrigger("Build");
		if (this.doRotate)
		{
			this.node_CannonHeadModel.forward = (base.transform.position - Singleton<MapManager>.Instance.GetClosestPlayerOrigin(base.transform.position).GetPosition()).WithY(0f);
			this.startRotation = this.node_CannonHeadModel.localRotation.eulerAngles;
		}
		Obj_TetrisBlock tetrisAtPosition = Singleton<GridSystem>.Instance.GetTetrisAtPosition(base.transform.position);
		if (tetrisAtPosition != null)
		{
			this.connectedTetris = tetrisAtPosition;
			tetrisAtPosition.RegisterTowerOnTop(this);
		}
		foreach (Collider collider in this.list_PlacementColliders)
		{
			if (Singleton<GridSystem>.Instance.IsHavePowerGridAtPosition(collider.transform.position))
			{
				Singleton<GridSystem>.Instance.GetPowerGridObjectAtPosition(collider.transform.position).OnTowerPlaced(this);
			}
		}
		this.tooltipDelayOnPlacement = 0.5f;
		this.idleAnimationTimer = Random.Range(3f, 8f) / 2f;
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x00018F58 File Offset: 0x00017158
	private void ApplyTowerTalentBuff()
	{
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.FLAME_TOWER_ENHANCE) && this.settingData.DamageType == eDamageType.FIRE)
		{
			TowerStats towerStats = new TowerStats();
			StatModifier modifier = new StatModifier(eModifierType.MULTIPLY, 1.2f);
			towerStats.StatType = eStatType.DAMAGE;
			towerStats.AddModifier(modifier);
			this.settingData.AddBuffMultiplier(towerStats);
		}
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.ELECTRIC_TOWER_DAMAGE_INCREASE) && this.settingData.DamageType == eDamageType.ELECTRIC)
		{
			TowerStats towerStats2 = new TowerStats();
			StatModifier modifier2 = new StatModifier(eModifierType.MULTIPLY, 1.2f);
			towerStats2.StatType = eStatType.SHOOT_RATE;
			towerStats2.AddModifier(modifier2);
			this.settingData.AddBuffMultiplier(towerStats2);
		}
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00019004 File Offset: 0x00017204
	public void Despawn()
	{
		this.isInitialized = false;
		this.CannonDespawnProc();
		Action<ABaseTower> onTowerDespawn = this.OnTowerDespawn;
		if (onTowerDespawn != null)
		{
			onTowerDespawn(this);
		}
		if (this.connectedTetris != null)
		{
			this.connectedTetris.UnregisterTowerOnTop(this);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00019055 File Offset: 0x00017255
	public void Move()
	{
		this.CannonMoveProc();
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00019060 File Offset: 0x00017260
	public void Shoot()
	{
		if (this.particle_ShootEffect != null)
		{
			this.particle_ShootEffect.Play();
		}
		this.shootIndex++;
		this.ShootProc();
		Action<ABaseTower, AMonsterBase> onTowerShoot = this.OnTowerShoot;
		if (onTowerShoot == null)
		{
			return;
		}
		onTowerShoot(this, this.currentTarget);
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x000190B1 File Offset: 0x000172B1
	public void ResetShootCooldown()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x000190BE File Offset: 0x000172BE
	public int GetCost(float multiplier = 1f)
	{
		return this.settingData.GetBuildCost(multiplier);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x000190CC File Offset: 0x000172CC
	public virtual int GetSellValue()
	{
		return this.settingData.GetSellValue(this.deployedRoundCount);
	}

	// Token: 0x060006EB RID: 1771
	protected abstract void ShootProc();

	// Token: 0x060006EC RID: 1772 RVA: 0x000190E0 File Offset: 0x000172E0
	protected virtual void CannonSpawnProc()
	{
		ABaseTower.<CannonSpawnProc>d__59 <CannonSpawnProc>d__;
		<CannonSpawnProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonSpawnProc>d__.<>1__state = -1;
		<CannonSpawnProc>d__.<>t__builder.Start<ABaseTower.<CannonSpawnProc>d__59>(ref <CannonSpawnProc>d__);
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00019110 File Offset: 0x00017310
	protected virtual void CannonUpgradeProc()
	{
		ABaseTower.<CannonUpgradeProc>d__60 <CannonUpgradeProc>d__;
		<CannonUpgradeProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonUpgradeProc>d__.<>1__state = -1;
		<CannonUpgradeProc>d__.<>t__builder.Start<ABaseTower.<CannonUpgradeProc>d__60>(ref <CannonUpgradeProc>d__);
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00019140 File Offset: 0x00017340
	protected virtual void CannonMoveProc()
	{
		ABaseTower.<CannonMoveProc>d__61 <CannonMoveProc>d__;
		<CannonMoveProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonMoveProc>d__.<>1__state = -1;
		<CannonMoveProc>d__.<>t__builder.Start<ABaseTower.<CannonMoveProc>d__61>(ref <CannonMoveProc>d__);
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00019170 File Offset: 0x00017370
	protected virtual void CannonDespawnProc()
	{
		ABaseTower.<CannonDespawnProc>d__62 <CannonDespawnProc>d__;
		<CannonDespawnProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CannonDespawnProc>d__.<>1__state = -1;
		<CannonDespawnProc>d__.<>t__builder.Start<ABaseTower.<CannonDespawnProc>d__62>(ref <CannonDespawnProc>d__);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x000191A0 File Offset: 0x000173A0
	protected virtual void OnBattleStartProc()
	{
		ABaseTower.<OnBattleStartProc>d__63 <OnBattleStartProc>d__;
		<OnBattleStartProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnBattleStartProc>d__.<>1__state = -1;
		<OnBattleStartProc>d__.<>t__builder.Start<ABaseTower.<OnBattleStartProc>d__63>(ref <OnBattleStartProc>d__);
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x000191D0 File Offset: 0x000173D0
	protected virtual void OnRoundStartProc()
	{
		ABaseTower.<OnRoundStartProc>d__64 <OnRoundStartProc>d__;
		<OnRoundStartProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnRoundStartProc>d__.<>1__state = -1;
		<OnRoundStartProc>d__.<>t__builder.Start<ABaseTower.<OnRoundStartProc>d__64>(ref <OnRoundStartProc>d__);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00019200 File Offset: 0x00017400
	protected virtual void OnRoundEndProc()
	{
		ABaseTower.<OnRoundEndProc>d__65 <OnRoundEndProc>d__;
		<OnRoundEndProc>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnRoundEndProc>d__.<>1__state = -1;
		<OnRoundEndProc>d__.<>t__builder.Start<ABaseTower.<OnRoundEndProc>d__65>(ref <OnRoundEndProc>d__);
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0001922F File Offset: 0x0001742F
	public virtual List<Collider> GetCollisionColliders()
	{
		return new List<Collider>
		{
			this.collider
		};
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00019242 File Offset: 0x00017442
	public virtual List<Collider> GetPlacementColliders()
	{
		return this.list_PlacementColliders;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0001924A File Offset: 0x0001744A
	public ePlaceableType GetPlaceableType()
	{
		return ePlaceableType.TOWER;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0001924D File Offset: 0x0001744D
	public Vector3 GetPlacementOffset()
	{
		return Vector3.up;
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x00019254 File Offset: 0x00017454
	public void SwitchToPlacementMode()
	{
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x00019256 File Offset: 0x00017456
	public void OnPlacementProc()
	{
		Singleton<CameraManager>.Instance.ShakeCamera(0.1f, 0.005f, 0.5f);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00019271 File Offset: 0x00017471
	public void SwitchToNextTargetPriority()
	{
		this.targetPriority = this.targetPriority.GetNext();
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00019284 File Offset: 0x00017484
	public void OverrideDamageType(eDamageType newType)
	{
		this.settingData.OverrideDamageType(newType);
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00019292 File Offset: 0x00017492
	protected void OnCreateBullet(AProjectile bullet)
	{
		this.bulletIndex++;
		bullet.RegisterOnHitCallback(new Action<AMonsterBase, int, int>(this.BulletHitCallback));
		bullet.SetIndex(this.shootIndex, this.bulletIndex);
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x000192C6 File Offset: 0x000174C6
	protected void BulletHitCallback(AMonsterBase monster, int shootIndex, int bulletIndex)
	{
		Action<ABaseTower, AMonsterBase, int, int> onTowerHit = this.OnTowerHit;
		if (onTowerHit == null)
		{
			return;
		}
		onTowerHit(this, monster, shootIndex, bulletIndex);
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x000192DC File Offset: 0x000174DC
	public void ToggleOverchargeAnim(bool isOn)
	{
		this.animator.SetBool("isOvercharged", isOn);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x000192EF File Offset: 0x000174EF
	public void PlayAnim_ApplyBuff()
	{
		this.animator.SetTrigger("ApplyBuff");
		this.shootTimer = 1f;
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0001930C File Offset: 0x0001750C
	public void ToggleCollider(bool isOn)
	{
		Debug.Log(string.Format("ToggleCollider: {0}", isOn), base.gameObject);
		this.collider.enabled = isOn;
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x00019338 File Offset: 0x00017538
	private void OnMouseDown()
	{
		if (!this.isInitialized)
		{
			return;
		}
		if (LeanTouch.PointOverGui(Input.mousePosition))
		{
			return;
		}
		if (Singleton<GameStateController>.Instance.IsCurrentState(eGameState.NORMAL_MODE))
		{
			EventMgr.SendEvent<ABaseTower>(eGameEvents.OnClickTowerOnField, this);
			EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0001938C File Offset: 0x0001758C
	private void OnMouseEnter()
	{
		if (!this.isInitialized)
		{
			return;
		}
		if (LeanTouch.PointOverGui(Input.mousePosition))
		{
			return;
		}
		if (!Singleton<GameStateController>.Instance.IsCurrentState(eGameState.EDIT_MODE))
		{
			if (this.tooltipDelayOnPlacement <= 0f)
			{
				string locNameString = this.settingData.GetLocNameString(false);
				string arg = this.settingData.GetLocStatsString() + "\n" + this.settingData.GetLocFlavorTextString();
				EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
				EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, arg);
				EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._3D, base.transform, Vector3.zero);
			}
			EventMgr.SendEvent<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestAddOutlineByList, this.list_TowerRenderers, OutlineController.eOutlineType.BASIC);
			EventMgr.SendEvent<ABaseTower, float>(eGameEvents.SetupRangeIndicator, this, this.settingData.GetAttackRange(1f));
			EventMgr.SendEvent<bool>(eGameEvents.ToggleRangeIndicator, true);
		}
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x00019484 File Offset: 0x00017684
	private void OnMouseExit()
	{
		if (!this.isInitialized)
		{
			return;
		}
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
		if (!Singleton<GameStateController>.Instance.IsCurrentState(eGameState.EDIT_MODE))
		{
			EventMgr.SendEvent<List<Renderer>, OutlineController.eOutlineType>(eGameEvents.RequestRemoveOutlineByListAndType, this.list_TowerRenderers, OutlineController.eOutlineType.BASIC);
			EventMgr.SendEvent<bool>(eGameEvents.ToggleRangeIndicator, false);
		}
	}

	// Token: 0x040005A1 RID: 1441
	[SerializeField]
	[Header("設定資料")]
	protected TowerSettingData settingData;

	// Token: 0x040005A2 RID: 1442
	[SerializeField]
	protected Animator animator;

	// Token: 0x040005A3 RID: 1443
	[Header("砲台Renderer")]
	[SerializeField]
	protected Renderer renderer_Tower;

	// Token: 0x040005A4 RID: 1444
	[Header("砲台Renderer")]
	[SerializeField]
	protected List<Renderer> list_TowerRenderers;

	// Token: 0x040005A5 RID: 1445
	[SerializeField]
	protected ParticleSystem particle_ShootEffect;

	// Token: 0x040005A6 RID: 1446
	[SerializeField]
	[Header("常駐的碰撞collider")]
	protected Collider collider;

	// Token: 0x040005A7 RID: 1447
	[SerializeField]
	[Header("判斷放置時使用的collider, 不可以跟碰撞共用")]
	protected List<Collider> list_PlacementColliders;

	// Token: 0x040005A8 RID: 1448
	[SerializeField]
	[Header("砲台的旋轉節點")]
	protected Transform node_CannonHeadModel;

	// Token: 0x040005A9 RID: 1449
	[SerializeField]
	[Header("發射點node")]
	protected Transform node_ShootPosition;

	// Token: 0x040005AA RID: 1450
	[SerializeField]
	[Header("是否會旋轉面向目標")]
	protected bool doRotate = true;

	// Token: 0x040005AB RID: 1451
	[SerializeField]
	protected eTowerTargetPriority targetPriority;

	// Token: 0x040005AC RID: 1452
	protected float shootTimer;

	// Token: 0x040005AD RID: 1453
	[SerializeField]
	protected AMonsterBase currentTarget;

	// Token: 0x040005AE RID: 1454
	protected Obj_TetrisBlock connectedTetris;

	// Token: 0x040005AF RID: 1455
	protected bool isInitialized;

	// Token: 0x040005B0 RID: 1456
	protected int deployedRoundCount;

	// Token: 0x040005B1 RID: 1457
	protected int shootIndex;

	// Token: 0x040005B2 RID: 1458
	protected int bulletIndex;

	// Token: 0x040005B3 RID: 1459
	private float idleAnimationTimer;

	// Token: 0x040005B4 RID: 1460
	private Vector3 startRotation = Vector3.zero;

	// Token: 0x040005B5 RID: 1461
	private float tooltipDelayOnPlacement = 0.5f;

	// Token: 0x040005B6 RID: 1462
	public Action<ABaseTower, AMonsterBase> OnTowerShoot;

	// Token: 0x040005B7 RID: 1463
	public Action<ABaseTower> OnTowerDespawn;

	// Token: 0x040005B8 RID: 1464
	public Action<ABaseTower, AMonsterBase, int, int> OnTowerHit;
}
