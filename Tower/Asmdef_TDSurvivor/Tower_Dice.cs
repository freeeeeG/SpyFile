using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class Tower_Dice : ABaseTower
{
	// Token: 0x06000722 RID: 1826 RVA: 0x0001A00D File Offset: 0x0001820D
	private void Start()
	{
		this.shootTimer = 0f;
		this.RollDice();
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0001A020 File Offset: 0x00018220
	private void Update()
	{
		if (!Singleton<GameStateController>.Instance.IsInBattle)
		{
			return;
		}
		if (this.shootTimer <= 0f)
		{
			this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
			if (this.currentTarget != null && this.currentTarget.IsAttackable())
			{
				this.shootTimer = this.settingData.GetShootInterval(1f);
				base.Shoot();
			}
			else
			{
				this.shootTimer = 0.05f;
			}
		}
		else
		{
			this.shootTimer -= Time.deltaTime;
		}
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.headModelForward = this.currentTarget.HeadWorldPosition - this.node_CannonHeadModel.position;
			this.headModelForward.y = 0f;
			this.node_CannonHeadModel.forward = this.headModelForward;
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			this.RollDice();
		}
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0001A13C File Offset: 0x0001833C
	private void RollDice()
	{
		int num = Random.Range(1, 7);
		this.curDamage = num;
		SoundManager.PlaySound("Cannon", "Cannon_1017_RollDice", -1f, -1f, 0.4f);
		Vector3 targetRotation = this.list_DiceFaceRotations[num - 1];
		TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenerCore = this.node_Dice.DOLocalRotate((Vector3.one + Random.insideUnitSphere) * 720f, 0.5f, RotateMode.LocalAxisAdd);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(delegate()
		{
			this.node_Dice.DOLocalRotate(targetRotation, 0.5f, RotateMode.FastBeyond360);
		}));
		Sequence sequence = this.node_Dice.DOLocalJump(this.node_Dice.transform.localPosition, 3f, 1, 0.5f, false);
		TweenCallback <>9__2;
		sequence.onComplete = (TweenCallback)Delegate.Combine(sequence.onComplete, new TweenCallback(delegate()
		{
			Sequence sequence2 = this.node_Dice.DOLocalJump(this.node_Dice.transform.localPosition, 1f, 1, 0.3f, false);
			Delegate onComplete = sequence2.onComplete;
			TweenCallback b;
			if ((b = <>9__2) == null)
			{
				b = (<>9__2 = delegate()
				{
					this.node_Dice.DOLocalJump(this.node_Dice.transform.localPosition, 0.33f, 1, 0.2f, false);
				});
			}
			sequence2.onComplete = (TweenCallback)Delegate.Combine(onComplete, b);
		}));
		if (num == 6)
		{
			base.StartCoroutine(this.CR_ConfettiEffect());
		}
		StatModifier modifier = new StatModifier(eModifierType.MULTIPLY, (float)this.curDamage);
		if (this.currentDiceValueModifier != null)
		{
			this.settingData.GetTowerStats(eStatType.DAMAGE).RemoveModifier(this.currentDiceValueModifier);
		}
		this.settingData.GetTowerStats(eStatType.DAMAGE).AddModifier(modifier);
		this.currentDiceValueModifier = modifier;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0001A284 File Offset: 0x00018484
	private IEnumerator CR_ConfettiEffect()
	{
		yield return new WaitForSeconds(1f);
		this.particle_Confetti.Play();
		SoundManager.PlaySound("Cannon", "Cannon_1017_Confetti", -1f, -1f, -1f);
		yield break;
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0001A294 File Offset: 0x00018494
	protected override void ShootProc()
	{
		Bullet_SingleTarget component = Singleton<PrefabManager>.Instance.InstantiatePrefab(this.settingData.GetBulletPrefab(), base.ShootWorldPosition, base.transform.rotation, null).GetComponent<Bullet_SingleTarget>();
		int damage = this.settingData.GetDamage(1f);
		component.Setup(damage, eDamageType.NONE);
		component.Spawn(this.currentTarget, null);
		base.OnCreateBullet(component);
		this.animator.SetTrigger("Shoot");
		SoundManager.PlaySound("Cannon", "Cannon_Shoot_1000", -1f, -1f, -1f);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0001A32C File Offset: 0x0001852C
	public override int GetSellValue()
	{
		switch (this.curDamage)
		{
		case 1:
			return 0;
		case 2:
			return 2;
		case 3:
			return 4;
		case 4:
			return 6;
		case 5:
			return 10;
		case 6:
			return 15;
		default:
			return 0;
		}
	}

	// Token: 0x040005C7 RID: 1479
	[SerializeField]
	private Transform node_Dice;

	// Token: 0x040005C8 RID: 1480
	[SerializeField]
	private ParticleSystem particle_Confetti;

	// Token: 0x040005C9 RID: 1481
	[SerializeField]
	private List<Vector3> list_DiceFaceRotations;

	// Token: 0x040005CA RID: 1482
	private int curDamage = 1;

	// Token: 0x040005CB RID: 1483
	private Vector3 headModelForward;

	// Token: 0x040005CC RID: 1484
	private StatModifier currentDiceValueModifier;
}
