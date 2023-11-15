using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class Cannon_AOEAttack : ABaseCannon
{
	// Token: 0x06000704 RID: 1796 RVA: 0x00019502 File Offset: 0x00017702
	private void Start()
	{
		this.shootTimer = 0f;
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00019510 File Offset: 0x00017710
	private void Update()
	{
		if (this.shootTimer > 0f)
		{
			this.shootTimer -= Time.deltaTime;
			return;
		}
		if (this.currentTarget == null || !this.currentTarget.IsAttackable() || !this.currentTarget.IsInRange(base.transform.position, this.settingData.GetAttackRange(1f)))
		{
			this.currentTarget = Singleton<MonsterManager>.Instance.GetTargetByTowerPriority(this.targetPriority, base.transform.position, this.settingData.GetAttackRange(1f));
		}
		if (this.currentTarget != null && this.currentTarget.IsAttackable())
		{
			this.shootTimer = this.settingData.GetShootInterval(1f);
			base.Shoot();
			return;
		}
		this.shootTimer = 0.1f;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x000195F8 File Offset: 0x000177F8
	protected override void ShootProc()
	{
		this.ShootProcAsync().Forget();
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x00019614 File Offset: 0x00017814
	private UniTaskVoid ShootProcAsync()
	{
		Cannon_AOEAttack.<ShootProcAsync>d__4 <ShootProcAsync>d__;
		<ShootProcAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<ShootProcAsync>d__.<>4__this = this;
		<ShootProcAsync>d__.<>1__state = -1;
		<ShootProcAsync>d__.<>t__builder.Start<Cannon_AOEAttack.<ShootProcAsync>d__4>(ref <ShootProcAsync>d__);
		return <ShootProcAsync>d__.<>t__builder.Task;
	}

	// Token: 0x040005B9 RID: 1465
	[SerializeField]
	private float attackDamageDelayTime = 0.5f;
}
