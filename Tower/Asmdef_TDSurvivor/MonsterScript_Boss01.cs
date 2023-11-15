using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class MonsterScript_Boss01 : ABossStageScript
{
	// Token: 0x0600040F RID: 1039 RVA: 0x0001009C File Offset: 0x0000E29C
	private void OnEnable()
	{
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x000100B6 File Offset: 0x0000E2B6
	private void OnDisable()
	{
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000100D0 File Offset: 0x0000E2D0
	protected override void Awake()
	{
		base.Awake();
		this.monster_boss = base.GetComponent<Monster_Boss_01>();
		this.list_Towers = new List<Tower_NPC_Arrow>();
		foreach (Tower_NPC_Arrow item in Object.FindObjectsOfType<Tower_NPC_Arrow>())
		{
			this.list_Towers.Add(item);
		}
		this.list_Towers.Sort((Tower_NPC_Arrow a, Tower_NPC_Arrow b) => Vector3.Distance(base.transform.position, a.transform.position).CompareTo(Vector3.Distance(base.transform.position, b.transform.position)));
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00010135 File Offset: 0x0000E335
	private void OnRoundStart(int round, int totalRound)
	{
		if (round < totalRound)
		{
			this.currentBossActionCoroutine = base.StartCoroutine(this.CR_Round_AttackTower(round));
			return;
		}
		this.currentBossActionCoroutine = base.StartCoroutine(this.CR_Round_AttackPlayer());
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x00010161 File Offset: 0x0000E361
	public override IEnumerator CR_Intro()
	{
		UI_CinematicBorder ui_CinematicBorder = APopupWindow.CreateWindow<UI_CinematicBorder>(APopupWindow.ePopupWindowLayer.MID, Singleton<UIManager>.Instance.PopupUIAnchor_MidLevel, false);
		ui_CinematicBorder.ShowWindow();
		Debug.Log("等三秒");
		yield return new WaitForSeconds(3f);
		EventMgr.SendEvent<Vector3, bool>(eGameEvents.MoveCameraToOffset, new Vector3(-19f, 0f, 0f), false);
		Debug.Log("移動攝影機");
		ui_CinematicBorder.CloseWindow();
		bool isTutorialFinished = false;
		EventMgr.SendEvent<eTutorialType, float, Action>(eGameEvents.RequestTutorial, eTutorialType.BOSS_STAGE, 0.5f, delegate()
		{
			isTutorialFinished = true;
		});
		while (!isTutorialFinished)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x00010169 File Offset: 0x0000E369
	private IEnumerator CR_Round_AttackPlayer()
	{
		Obj_FireSource flameSource = Object.FindFirstObjectByType<Obj_FireSource>();
		this.state = MonsterScript_Boss01.eState.IDLE;
		this.TriggerAnimation("Summon");
		while (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			yield return null;
		}
		foreach (Obj_TetrisBlock obj_TetrisBlock in Object.FindObjectsOfType<Obj_TetrisBlock>())
		{
			if (Vector3.Distance(base.transform.position, obj_TetrisBlock.transform.position) < 2f)
			{
				obj_TetrisBlock.Remove();
			}
		}
		this.state = MonsterScript_Boss01.eState.MOVE;
		this.TriggerAnimation("Move");
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Laugh", -1f, -1f, -1f);
		this.monster_boss.RecalculatePath();
		this.monster_boss.ToggleMoveable(true);
		while (Vector3.Distance(base.transform.position, flameSource.transform.position) > 1.5f)
		{
			yield return null;
		}
		this.monster_boss.ToggleMoveable(false);
		this.TriggerAnimation("Summon");
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Laugh", -1f, -1f, -1f);
		yield return new WaitForSeconds(2f);
		if (!this.monster_boss.IsDead())
		{
			SoundManager.PlaySound("Monster", "Monster_Boss_01_Spell", -1f, -1f, -1f);
			this.TriggerAnimation("Attack_Ranged");
			yield return new WaitForSeconds(0.5f);
			EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 7.5f, 3f);
			EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 10f, 3f);
			Singleton<CameraManager>.Instance.ShakeCamera(0.15f, 0.0025f, 0f);
			EventMgr.SendEvent<int>(eGameEvents.RequestAddHP, -1 * MainGame.Instance.IngameData.HP);
			yield return new WaitForSeconds(1f);
			this.TriggerAnimation("Summon");
			SoundManager.PlaySound("Monster", "Monster_Boss_01_Laugh", -1f, -1f, -1f);
		}
		this.currentBossActionCoroutine = null;
		yield break;
	}

	// Token: 0x06000415 RID: 1045 RVA: 0x00010178 File Offset: 0x0000E378
	private IEnumerator CR_Round_AttackTower(int round)
	{
		Tower_NPC_Arrow targetTower = this.list_Towers[round - 1];
		this.state = MonsterScript_Boss01.eState.IDLE;
		this.TriggerAnimation("Summon");
		while (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			yield return null;
		}
		this.state = MonsterScript_Boss01.eState.MOVE;
		this.TriggerAnimation("Move");
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Laugh", -1f, -1f, -1f);
		while (Vector3.Distance(base.transform.position, targetTower.transform.position) > this.attackRange)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetTower.transform.position, this.moveSpeed * Time.deltaTime);
			base.transform.forward = (targetTower.transform.position - base.transform.position).normalized;
			yield return null;
		}
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Spell", -1f, -1f, -1f);
		this.TriggerAnimation("Attack_Ranged");
		yield return new WaitForSeconds(0.5f);
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 7.5f, 3f);
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 10f, 3f);
		Singleton<CameraManager>.Instance.ShakeCamera(0.15f, 0.0025f, 0f);
		targetTower.ToggleBurnEffect(true);
		targetTower.ToggleIsAttackable(false);
		targetTower.AttackedEffect(1f, 1f, 0f);
		yield return new WaitForSeconds(1f);
		this.TriggerAnimation("Summon");
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Laugh", -1f, -1f, -1f);
		yield return new WaitForSeconds(1.5f);
		this.state = MonsterScript_Boss01.eState.ATTACK;
		float attackTimer = 0f;
		float attackInterval = Random.Range(0.8f, 1.5f);
		Random.Range(0, 2);
		while (Singleton<GameStateController>.Instance.IsInBattle)
		{
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackInterval)
			{
				attackTimer = 0f;
				attackInterval = Random.Range(0.8f, 1.5f);
				if (Random.Range(0, 2) == 0)
				{
					this.TriggerAnimation("Attack_Normal_A");
					SoundManager.PlaySound("Monster", "Monster_Boss_01_Attack", -1f, -1f, 0.3f);
					Singleton<CameraManager>.Instance.ShakeCamera(0.1f, 0.0025f, 0f);
				}
				else
				{
					this.TriggerAnimation("Attack_Normal_B");
					SoundManager.PlaySound("Monster", "Monster_Boss_01_Attack", -1f, -1f, 0.3f);
					Singleton<CameraManager>.Instance.ShakeCamera(0.1f, 0.0025f, 0f);
				}
				targetTower.AttackedEffect(0.5f, 0.5f, 0.25f);
			}
			yield return null;
		}
		SoundManager.PlaySound("Monster", "Monster_Boss_01_Spell", -1f, -1f, -1f);
		this.TriggerAnimation("Attack_Ranged");
		yield return new WaitForSeconds(0.5f);
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 7.5f, 3f);
		EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position + base.transform.forward * 10f, 3f);
		Singleton<CameraManager>.Instance.ShakeCamera(0.15f, 0.0025f, 0f);
		targetTower.AttackedEffect(1f, 1f, 0f);
		targetTower.PlayDestroyTowerAnim();
		yield return new WaitForSeconds(0.5f);
		this.currentBossActionCoroutine = null;
		yield break;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x00010190 File Offset: 0x0000E390
	private void Update()
	{
		this.burningSignalSendTimer += Time.deltaTime;
		if (this.burningSignalSendTimer >= this.burningSignalSendInterval)
		{
			this.burningSignalSendTimer = 0f;
			EventMgr.SendEvent<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, base.transform.position, 6.66f);
		}
		if (this.monster_boss.IsDead())
		{
			this.monster_boss.ToggleMoveable(false);
			if (this.currentBossActionCoroutine != null)
			{
				base.StopCoroutine(this.currentBossActionCoroutine);
				this.currentBossActionCoroutine = null;
			}
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0001021C File Offset: 0x0000E41C
	private void TriggerAnimation(string key)
	{
		if (this.monster_boss.IsDead())
		{
			return;
		}
		this.animator.SetTrigger(key);
	}

	// Token: 0x04000416 RID: 1046
	[SerializeField]
	private Animator animator;

	// Token: 0x04000417 RID: 1047
	[SerializeField]
	private List<Tower_NPC_Arrow> list_Towers;

	// Token: 0x04000418 RID: 1048
	[SerializeField]
	private float moveSpeed = 1f;

	// Token: 0x04000419 RID: 1049
	[SerializeField]
	private float attackRange = 8f;

	// Token: 0x0400041A RID: 1050
	[SerializeField]
	private MonsterScript_Boss01.eState state;

	// Token: 0x0400041B RID: 1051
	private Monster_Boss_01 monster_boss;

	// Token: 0x0400041C RID: 1052
	private Coroutine currentBossActionCoroutine;

	// Token: 0x0400041D RID: 1053
	private float burningSignalSendTimer;

	// Token: 0x0400041E RID: 1054
	private float burningSignalSendInterval = 1f;

	// Token: 0x02000220 RID: 544
	public enum eState
	{
		// Token: 0x04000AA4 RID: 2724
		IDLE,
		// Token: 0x04000AA5 RID: 2725
		MOVE,
		// Token: 0x04000AA6 RID: 2726
		ATTACK
	}
}
