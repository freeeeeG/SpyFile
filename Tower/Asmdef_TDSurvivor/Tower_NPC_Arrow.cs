using System;
using DG.Tweening;
using UnityEngine;

// Token: 0x0200011D RID: 285
[SelectionBase]
public class Tower_NPC_Arrow : MonoBehaviour
{
	// Token: 0x06000750 RID: 1872 RVA: 0x0001B687 File Offset: 0x00019887
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0001B6A1 File Offset: 0x000198A1
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0001B6BB File Offset: 0x000198BB
	private void OnBattleStart()
	{
		if (!this.doTalk || !this.isAttackable)
		{
			return;
		}
		this.ShowDialog(2f, string.Format("DIALOG_INGAME_ENEMY_INCOMING_{0}", Random.Range(1, 3)));
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0001B6F0 File Offset: 0x000198F0
	private void ShowDialog(float delay, string locKey)
	{
		UI_NpcDialog_Popup ui_NpcDialog_Popup = APopupWindow.CreateWindow<UI_NpcDialog_Popup>(APopupWindow.ePopupWindowLayer.MID, null, false);
		string @string = LocalizationManager.Instance.GetString("NPCDialog", locKey, Array.Empty<object>());
		ui_NpcDialog_Popup.SetupContent(@string, 2f, delay, this.shootPosition.position, new Vector3(-50f, 50f, 0f));
		ui_NpcDialog_Popup.ShowWindow();
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0001B74C File Offset: 0x0001994C
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0001B75C File Offset: 0x0001995C
	private void Update()
	{
		if (!this.isAttackable)
		{
			return;
		}
		if (this.shootTimer > 0f)
		{
			this.shootTimer -= Time.deltaTime;
			return;
		}
		this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(eTowerTargetPriority.NEAREST, base.transform.position, this.attackRange);
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.shootTimer = this.shootInterval;
			this.Shoot();
			return;
		}
		this.shootTimer = 0.05f;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0001B7F0 File Offset: 0x000199F0
	protected void Shoot()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.bulletPrefab, this.shootPosition.position, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		component.Setup(this.damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		this.currentTarget.PreregisterAttack(this.damage);
		SoundManager.PlaySound("Cannon", "NPC_Shoot_Arrow", -1f, -1f, -1f);
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0001B872 File Offset: 0x00019A72
	public void ToggleIsAttackable(bool isAttackable)
	{
		this.isAttackable = isAttackable;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0001B87B File Offset: 0x00019A7B
	public void ToggleBurnEffect(bool isOn)
	{
		this.ShowDialog(0f, string.Format("DIALOG_INGAME_KILLED_BY_BOSS_01_{0}", Random.Range(1, 4)));
		if (isOn)
		{
			this.particle_Burn.Play();
			return;
		}
		this.particle_Burn.Stop();
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0001B8B8 File Offset: 0x00019AB8
	public void AttackedEffect(float duration, float strengthMultiplier, float delay)
	{
		base.transform.DOShakePosition(duration, strengthMultiplier * 0.1f, 10, 90f, false, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
		base.transform.DOShakeRotation(duration, strengthMultiplier * 5f, 10, 90f, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0001B90C File Offset: 0x00019B0C
	public void PlayDestroyTowerAnim()
	{
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x040005ED RID: 1517
	[SerializeField]
	private Animator animator;

	// Token: 0x040005EE RID: 1518
	[SerializeField]
	private int damage = 3;

	// Token: 0x040005EF RID: 1519
	[SerializeField]
	private float attackRange = 10f;

	// Token: 0x040005F0 RID: 1520
	[SerializeField]
	private float shootInterval = 1f;

	// Token: 0x040005F1 RID: 1521
	[SerializeField]
	private GameObject bulletPrefab;

	// Token: 0x040005F2 RID: 1522
	[SerializeField]
	private Transform shootPosition;

	// Token: 0x040005F3 RID: 1523
	[SerializeField]
	private ParticleSystem particle_Burn;

	// Token: 0x040005F4 RID: 1524
	[SerializeField]
	private bool doTalk;

	// Token: 0x040005F5 RID: 1525
	[SerializeField]
	private bool isAttackable = true;

	// Token: 0x040005F6 RID: 1526
	private Vector3 headModelForward;

	// Token: 0x040005F7 RID: 1527
	private float shootTimer;

	// Token: 0x040005F8 RID: 1528
	private AMonsterBase currentTarget;
}
