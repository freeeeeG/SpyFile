using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x02000043 RID: 67
[SelectionBase]
public abstract class AProjectile : MonoBehaviour
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600014D RID: 333 RVA: 0x00005FAB File Offset: 0x000041AB
	public AProjectile.eState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00005FB4 File Offset: 0x000041B4
	public virtual void Spawn(AMonsterBase target, GameObject source = null)
	{
		if (target == null)
		{
			Debug.LogError("子彈的目標是空的", base.gameObject);
			this.Despawn();
		}
		this.targetMonster = target;
		this.spawnSource = source;
		this.SpawnProc();
		if (this.node_Model != null)
		{
			this.node_Model.SetActive(true);
		}
		this.state = AProjectile.eState.STARTED;
		this.spawnPosition = base.transform.position;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00006026 File Offset: 0x00004226
	public void RegisterOnHitCallback(Action<AMonsterBase, int, int> onHitTargetCallback)
	{
		this.OnHitTargetCallback = (Action<AMonsterBase, int, int>)Delegate.Combine(this.OnHitTargetCallback, onHitTargetCallback);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000603F File Offset: 0x0000423F
	public void SetIndex(int shootIndex, int bulletIndex)
	{
		this.shootIndex = shootIndex;
		this.bulletIndex = bulletIndex;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000604F File Offset: 0x0000424F
	public void OnHit(AMonsterBase monster)
	{
		if (this.OnHitTargetCallback != null)
		{
			this.OnHitTargetCallback(monster, this.shootIndex, this.bulletIndex);
		}
		this.OnHitProc(monster);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00006078 File Offset: 0x00004278
	public virtual void OnHitProc(AMonsterBase monster)
	{
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000607C File Offset: 0x0000427C
	public virtual void Despawn()
	{
		if (this.state == AProjectile.eState.FINISHED)
		{
			return;
		}
		this.OnHitTargetCallback = null;
		this.state = AProjectile.eState.FINISHED;
		this.DespawnAsync().Forget();
	}

	// Token: 0x06000154 RID: 340 RVA: 0x000060B0 File Offset: 0x000042B0
	private UniTaskVoid DespawnAsync()
	{
		AProjectile.<DespawnAsync>d__19 <DespawnAsync>d__;
		<DespawnAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DespawnAsync>d__.<>4__this = this;
		<DespawnAsync>d__.<>1__state = -1;
		<DespawnAsync>d__.<>t__builder.Start<AProjectile.<DespawnAsync>d__19>(ref <DespawnAsync>d__);
		return <DespawnAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000155 RID: 341
	protected abstract void SpawnProc();

	// Token: 0x06000156 RID: 342
	protected abstract void DespawnProc();

	// Token: 0x06000157 RID: 343
	protected abstract void DestroyProc();

	// Token: 0x06000158 RID: 344 RVA: 0x000060F3 File Offset: 0x000042F3
	public bool IsState(AProjectile.eState targetState)
	{
		return targetState == this.state;
	}

	// Token: 0x040000E1 RID: 225
	[SerializeField]
	protected AProjectile.eState state;

	// Token: 0x040000E2 RID: 226
	[SerializeField]
	protected GameObject node_Model;

	// Token: 0x040000E3 RID: 227
	[SerializeField]
	protected ParticleSystem particle_Explosion;

	// Token: 0x040000E4 RID: 228
	[SerializeField]
	protected float despawnDelay = 1f;

	// Token: 0x040000E5 RID: 229
	protected Vector3 spawnPosition = Vector3.zero;

	// Token: 0x040000E6 RID: 230
	private int shootIndex = -1;

	// Token: 0x040000E7 RID: 231
	private int bulletIndex = -1;

	// Token: 0x040000E8 RID: 232
	protected AMonsterBase targetMonster;

	// Token: 0x040000E9 RID: 233
	protected Action<AMonsterBase, int, int> OnHitTargetCallback;

	// Token: 0x040000EA RID: 234
	protected GameObject spawnSource;

	// Token: 0x020001DF RID: 479
	public enum eState
	{
		// Token: 0x040009C1 RID: 2497
		NONE,
		// Token: 0x040009C2 RID: 2498
		STARTED,
		// Token: 0x040009C3 RID: 2499
		FINISHED,
		// Token: 0x040009C4 RID: 2500
		DESTROYED
	}
}
