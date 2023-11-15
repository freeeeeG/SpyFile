using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Pathfinding;
using UnityEngine;

// Token: 0x020000B1 RID: 177
[SelectionBase]
public abstract class AMonsterBase : MonoBehaviour, IHaveHP
{
	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
	public MonsterSettingData MonsterData
	{
		get
		{
			return this.monsterData;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x0000EADC File Offset: 0x0000CCDC
	public Vector3 HeadWorldPosition
	{
		get
		{
			return base.transform.position + base.transform.rotation * this.headPosition;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000EB04 File Offset: 0x0000CD04
	public AMonsterBase.eState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060003B8 RID: 952 RVA: 0x0000EB0C File Offset: 0x0000CD0C
	public float Progress
	{
		get
		{
			return this.timeSinceSpawn * this.speed;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000EB1B File Offset: 0x0000CD1B
	public float RemainingDistance
	{
		get
		{
			return this.aiPath.remainingDistance;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060003BA RID: 954 RVA: 0x0000EB28 File Offset: 0x0000CD28
	public bool IsImpendingDeath
	{
		get
		{
			return this.isImpendingDeath;
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0000EB30 File Offset: 0x0000CD30
	private void OnEnable()
	{
		EventMgr.Register<int>(eGameEvents.OnFloodPathUpdate, new Action<int>(this.OnFloodPathUpdated));
		if (this.matPropBlock == null)
		{
			this.matPropBlock = new MaterialPropertyBlock();
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0000EB5D File Offset: 0x0000CD5D
	private void OnDisable()
	{
		EventMgr.Remove<int>(eGameEvents.OnFloodPathUpdate, new Action<int>(this.OnFloodPathUpdated));
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0000EB77 File Offset: 0x0000CD77
	private void OnFloodPathUpdated(int spawnIndex)
	{
		if (this.monsterSpawner == null)
		{
			return;
		}
		if (spawnIndex != this.monsterSpawner.SpawnNodeIndex)
		{
			return;
		}
		this.RecalculatePath();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
	public virtual void Spawn(MonsterSpawner spawner, bool isCorrupted)
	{
		float currentDifficulty = Singleton<StageDataReader>.Instance.GetCurrentDifficulty();
		if (this.list_SpeedModifier == null)
		{
			this.list_SpeedModifier = new List<AMonsterBase.MonsterSpeedModifier>();
		}
		this.list_SpeedModifier.Clear();
		if (this.list_MonsterDamageDebuff == null)
		{
			this.list_MonsterDamageDebuff = new List<MonsterDamageDebuff>();
		}
		this.list_MonsterDamageDebuff.Clear();
		this.monsterSpawner = spawner;
		this.hp = this.monsterData.GetMaxHP(currentDifficulty);
		this.speed = this.monsterData.GetMoveSpeed(1f);
		if (isCorrupted)
		{
			this.hp = (int)((float)this.hp * 1.2f);
			this.speed *= 1.05f;
		}
		this.animator.speed = 1f;
		this.isImpendingDeath = false;
		this.timeSinceSpawn = 0f;
		this.aiPath.maxSpeed = this.speed;
		this.aiPath.canSearch = false;
		this.aiPath.rotationSpeed = 360f * Mathf.Max(1f, this.speed / 5f);
		this.aiPath.constrainInsideGraph = true;
		this.doDropChestOnDeath = false;
		if (isCorrupted)
		{
			this.renderer.material = this.material_Corrupted;
		}
		else
		{
			this.renderer.material = this.material_Normal;
		}
		this.SpawnProc();
		this.collider.enabled = true;
		if (this.node_Model != null)
		{
			this.node_Model.SetActive(true);
		}
		this.RecalculatePath();
		if (this.matPropBlock != null)
		{
			this.matPropBlock.SetFloat("_HitFlashEffect", 0f);
			this.matPropBlock.SetFloat("_Dissolve", 0f);
			this.renderer.SetPropertyBlock(this.matPropBlock);
		}
		EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterSpawn, this);
		this.state = AMonsterBase.eState.ALIVE;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0000ED76 File Offset: 0x0000CF76
	public void RecalculatePath()
	{
		this.isCalculatingPath = true;
		this.seeker.CancelCurrentPathRequest(true);
		this.UpdateAiPathMoveable();
		base.StartCoroutine(this.CR_WaitRecalculatePath());
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0000ED9E File Offset: 0x0000CF9E
	private IEnumerator CR_WaitRecalculatePath()
	{
		while (!this.monsterSpawner.IsFloodPathReady())
		{
			yield return null;
		}
		FloodPathTracer floodPathTracer = this.monsterSpawner.GetFloodPathTracer(base.transform.position, null);
		this.seeker.StartPath(floodPathTracer, new OnPathDelegate(this.OnPathReady));
		yield break;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0000EDAD File Offset: 0x0000CFAD
	private void OnPathReady(Path path)
	{
		this.isCalculatingPath = false;
		path.callback = (OnPathDelegate)Delegate.Remove(path.callback, new OnPathDelegate(this.OnPathReady));
		this.UpdateAiPathMoveable();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0000EDDE File Offset: 0x0000CFDE
	public void ToggleMoveable(bool canMove)
	{
		this.canMove = canMove;
		this.UpdateAiPathMoveable();
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0000EDED File Offset: 0x0000CFED
	private void UpdateAiPathMoveable()
	{
		this.aiPath.canMove = (!this.isCalculatingPath && this.canMove);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0000EE0C File Offset: 0x0000D00C
	public virtual void Despawn()
	{
		if (this.state == AMonsterBase.eState.DESPAWNED || this.state == AMonsterBase.eState.REMOVED)
		{
			return;
		}
		this.state = AMonsterBase.eState.DESPAWNED;
		EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterDespawn, this);
		Action<AMonsterBase> onMonsterDespawn = this.OnMonsterDespawn;
		if (onMonsterDespawn != null)
		{
			onMonsterDespawn(this);
		}
		this.OnMonsterDespawn = null;
		this.DespawnAsync().Forget();
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0000EE68 File Offset: 0x0000D068
	private UniTaskVoid DespawnAsync()
	{
		AMonsterBase.<DespawnAsync>d__54 <DespawnAsync>d__;
		<DespawnAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DespawnAsync>d__.<>4__this = this;
		<DespawnAsync>d__.<>1__state = -1;
		<DespawnAsync>d__.<>t__builder.Start<AMonsterBase.<DespawnAsync>d__54>(ref <DespawnAsync>d__);
		return <DespawnAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0000EEAB File Offset: 0x0000D0AB
	public void PreregisterAttack(int damage)
	{
		if (damage >= this.hp)
		{
			this.isImpendingDeath = true;
			EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterImpendingDeath, this);
		}
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0000EECC File Offset: 0x0000D0CC
	public virtual void Hit(int damage, eDamageType damageType, Vector3 hitDirection = default(Vector3))
	{
		if (this.state != AMonsterBase.eState.ALIVE)
		{
			return;
		}
		damage = DamageCalculator.CalculateDamage(damage, damageType, this);
		bool arg = damage < 0;
		if (damage < 0)
		{
			damage = Mathf.Abs(damage);
		}
		if (damage > 0)
		{
			EventMgr.SendEvent<Vector3, int, bool, eDamageType>(eGameEvents.UI_ShowDamageNumber, this.HeadWorldPosition, damage, arg, damageType);
			this.hp -= damage;
			Action onMonsterHPChange = this.OnMonsterHPChange;
			if (onMonsterHPChange != null)
			{
				onMonsterHPChange();
			}
			if (this.IsDead())
			{
				this.TriggerHitFlashEffect();
				this.state = AMonsterBase.eState.KILLED;
				this.aiPath.canMove = false;
				this.aiPath.maxSpeed = 0f;
				this.collider.enabled = false;
				base.StartCoroutine(this.DeathProc(damage, true));
				base.StartCoroutine(this.CR_DeathDissolveEffect(this.deadAnimationLength));
				if (this.monsterData.Size != eMonsterSize.BOSS)
				{
					this.Despawn();
				}
				EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterKilled, this);
				Action<AMonsterBase> onMonsterKilled = this.OnMonsterKilled;
				if (onMonsterKilled != null)
				{
					onMonsterKilled(this);
				}
				this.OnMonsterKilled = null;
				if (this.sfxKey_OnDeath != "")
				{
					SoundManager.PlaySound("Monster", this.sfxKey_OnDeath, -1f, -1f, -1f);
				}
				EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, this.monsterData.GetReward(1f));
				return;
			}
			this.TriggerHitFlashEffect();
			this.HitProc(damage);
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0000F034 File Offset: 0x0000D234
	private IEnumerator CR_DeathDissolveEffect(float delay)
	{
		if (delay > 0f)
		{
			yield return new WaitForSeconds(delay);
		}
		float time = 0f;
		float duration = 0.5f;
		while (time <= duration)
		{
			float value = time / duration;
			this.matPropBlock.SetFloat("_Dissolve", value);
			this.renderer.SetPropertyBlock(this.matPropBlock);
			yield return null;
			time += Time.deltaTime;
		}
		yield break;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0000F04C File Offset: 0x0000D24C
	private void TriggerHitFlashEffect()
	{
		if (this.cr_HitFlashEffect != null)
		{
			base.StopCoroutine(this.cr_HitFlashEffect);
			if (this.matPropBlock != null)
			{
				this.matPropBlock.SetFloat("_HitFlashEffect", 0f);
				this.renderer.SetPropertyBlock(this.matPropBlock);
			}
		}
		this.cr_HitFlashEffect = base.StartCoroutine(this.CR_HitFlashEffect());
	}

	// Token: 0x060003CA RID: 970 RVA: 0x0000F0AD File Offset: 0x0000D2AD
	private IEnumerator CR_HitFlashEffect()
	{
		float time = 0f;
		float duration = 0.05f;
		while (time <= duration)
		{
			float num = time / duration;
			this.matPropBlock.SetFloat("_HitFlashEffect", num * 0.2f);
			this.renderer.SetPropertyBlock(this.matPropBlock);
			yield return null;
			time += Time.deltaTime;
		}
		time = 0f;
		duration = 0.1f;
		while (time <= duration)
		{
			float num = time / duration;
			this.matPropBlock.SetFloat("_HitFlashEffect", (1f - num) * 0.2f);
			this.renderer.SetPropertyBlock(this.matPropBlock);
			yield return null;
			time += Time.deltaTime;
		}
		this.matPropBlock.SetFloat("_HitFlashEffect", 0f);
		this.renderer.SetPropertyBlock(this.matPropBlock);
		this.cr_HitFlashEffect = null;
		yield break;
	}

	// Token: 0x060003CB RID: 971 RVA: 0x0000F0BC File Offset: 0x0000D2BC
	protected virtual void Update()
	{
		if (this.state == AMonsterBase.eState.ALIVE)
		{
			this.timeSinceSpawn += Time.deltaTime;
			if (this.aiPath.reachedEndOfPath)
			{
				this.ReachEndOfPathProc();
			}
			if (this.list_SpeedModifier.Count > 0)
			{
				for (int i = this.list_SpeedModifier.Count - 1; i >= 0; i--)
				{
					AMonsterBase.MonsterSpeedModifier monsterSpeedModifier = this.list_SpeedModifier[i];
					monsterSpeedModifier.duration -= Time.deltaTime;
					if (monsterSpeedModifier.duration <= 0f)
					{
						this.list_SpeedModifier.RemoveAt(i);
						this.UpdateAIPathSpeed();
					}
				}
			}
			if (this.list_MonsterDamageDebuff.Count > 0)
			{
				for (int j = this.list_MonsterDamageDebuff.Count - 1; j >= 0; j--)
				{
					MonsterDamageDebuff monsterDamageDebuff = this.list_MonsterDamageDebuff[j];
					monsterDamageDebuff.Update(Time.deltaTime);
					if (monsterDamageDebuff.IsFinished)
					{
						this.list_MonsterDamageDebuff.RemoveAt(j);
						Action onMonsterDamageDebuffChange = this.OnMonsterDamageDebuffChange;
						if (onMonsterDamageDebuffChange != null)
						{
							onMonsterDamageDebuffChange();
						}
					}
				}
			}
			if (base.transform.position.y < -1f)
			{
				base.transform.position = base.transform.position.WithY(0.1f);
			}
		}
		this.UpdateProc(Time.deltaTime);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0000F200 File Offset: 0x0000D400
	protected virtual void ReachEndOfPathProc()
	{
		EventMgr.SendEvent<int>(eGameEvents.RequestAddHP, -1);
		EventMgr.SendEvent<AMonsterBase>(eGameEvents.MonsterDealDamageToPlayer, this);
		this.aiPath.canMove = false;
		this.collider.enabled = false;
		base.StartCoroutine(this.CR_DeathDissolveEffect(0f));
		base.StartCoroutine(this.DeathProc(0, false));
		this.Despawn();
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0000F268 File Offset: 0x0000D468
	private void UpdateAIPathSpeed()
	{
		this.aiPath.maxSpeed = this.speed * this.GetSpeedModifier();
		this.animator.speed = Mathf.Clamp01(this.GetSpeedModifier());
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0000F298 File Offset: 0x0000D498
	public void ApplySpeedModifier(float modifier, float time, bool isFromPlayer)
	{
		this.list_SpeedModifier.Add(new AMonsterBase.MonsterSpeedModifier(modifier, time, isFromPlayer));
		this.UpdateAIPathSpeed();
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0000F2B4 File Offset: 0x0000D4B4
	public float GetSpeedModifier()
	{
		if (this.list_SpeedModifier.Count == 0)
		{
			return 1f;
		}
		float num = 1f;
		float num2 = 1f;
		foreach (AMonsterBase.MonsterSpeedModifier monsterSpeedModifier in this.list_SpeedModifier)
		{
			if (monsterSpeedModifier.isFromPlayer)
			{
				if (monsterSpeedModifier.value < num)
				{
					num = monsterSpeedModifier.value;
				}
			}
			else if (monsterSpeedModifier.value > num2)
			{
				num2 = monsterSpeedModifier.value;
			}
		}
		return 1f * num * num2;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0000F354 File Offset: 0x0000D554
	public void ApplyDamageDebuff(float duration, float tickInterval, int damagePerTick, eDamageType damageType, int sourceID)
	{
		if (this.list_MonsterDamageDebuff.Exists((MonsterDamageDebuff debuff) => debuff.IsSameSource(sourceID, damageType)))
		{
			this.list_MonsterDamageDebuff.Find((MonsterDamageDebuff debuff) => debuff.IsSameSource(sourceID, damageType)).RenewDebuff(duration, tickInterval, damagePerTick);
			return;
		}
		this.list_MonsterDamageDebuff.Add(new MonsterDamageDebuff(this, duration, tickInterval, damagePerTick, damageType, sourceID));
		Action onMonsterDamageDebuffChange = this.OnMonsterDamageDebuffChange;
		if (onMonsterDamageDebuffChange == null)
		{
			return;
		}
		onMonsterDamageDebuffChange();
	}

	// Token: 0x060003D1 RID: 977
	protected abstract void HitProc(int damage);

	// Token: 0x060003D2 RID: 978
	protected abstract IEnumerator DeathProc(int damage, bool playAnimation = true);

	// Token: 0x060003D3 RID: 979
	protected abstract void SpawnProc();

	// Token: 0x060003D4 RID: 980
	protected abstract void DespawnProc();

	// Token: 0x060003D5 RID: 981
	protected abstract void UpdateProc(float deltaTime);

	// Token: 0x060003D6 RID: 982 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
	public bool IsState(AMonsterBase.eState targetState)
	{
		return targetState == this.state;
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0000F3EB File Offset: 0x0000D5EB
	public int GetHP()
	{
		return this.hp;
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0000F3F4 File Offset: 0x0000D5F4
	public int GetMaxHP(bool withDifficulty)
	{
		if (withDifficulty)
		{
			float currentDifficulty = Singleton<StageDataReader>.Instance.GetCurrentDifficulty();
			return this.monsterData.GetMaxHP(currentDifficulty);
		}
		return this.monsterData.GetMaxHP(1f);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0000F42C File Offset: 0x0000D62C
	public float GetHPPercentage()
	{
		float currentDifficulty = Singleton<StageDataReader>.Instance.GetCurrentDifficulty();
		if (this.monsterData.GetMonsterSize() == eMonsterSize.BOSS)
		{
			return (float)this.hp / (float)this.monsterData.GetMaxHP(1f);
		}
		return (float)this.hp / (float)this.monsterData.GetMaxHP(currentDifficulty);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0000F481 File Offset: 0x0000D681
	public bool IsDead()
	{
		return this.hp <= 0;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0000F48F File Offset: 0x0000D68F
	public bool IsAttackable()
	{
		return this.state == AMonsterBase.eState.ALIVE;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0000F49A File Offset: 0x0000D69A
	public bool IsInRange(Vector3 center, float range)
	{
		return Vector3.SqrMagnitude(base.transform.position - center) < range * range;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0000F4B8 File Offset: 0x0000D6B8
	public bool IsPoisoned()
	{
		if (this.list_MonsterDamageDebuff == null || this.list_MonsterDamageDebuff.Count == 0)
		{
			return false;
		}
		return this.list_MonsterDamageDebuff.Exists((MonsterDamageDebuff debuff) => debuff.GetDamageType() == eDamageType.POISON);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0000F508 File Offset: 0x0000D708
	public bool IsBurning()
	{
		if (this.list_MonsterDamageDebuff == null || this.list_MonsterDamageDebuff.Count == 0)
		{
			return false;
		}
		return this.list_MonsterDamageDebuff.Exists((MonsterDamageDebuff debuff) => debuff.GetDamageType() == eDamageType.FIRE);
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0000F558 File Offset: 0x0000D758
	public bool IsFrozen()
	{
		if (this.list_MonsterDamageDebuff == null || this.list_MonsterDamageDebuff.Count == 0)
		{
			return false;
		}
		return this.list_MonsterDamageDebuff.Exists((MonsterDamageDebuff debuff) => debuff.GetDamageType() == eDamageType.ICE);
	}

	// Token: 0x040003CC RID: 972
	[Header("怪物資料")]
	[SerializeField]
	protected MonsterSettingData monsterData;

	// Token: 0x040003CD RID: 973
	[Header("Animator")]
	[SerializeField]
	protected Animator animator;

	// Token: 0x040003CE RID: 974
	[Header("Collider")]
	[SerializeField]
	protected Collider collider;

	// Token: 0x040003CF RID: 975
	[Header("Renderer")]
	[SerializeField]
	protected Renderer renderer;

	// Token: 0x040003D0 RID: 976
	[Header("Node_模型")]
	[SerializeField]
	protected GameObject node_Model;

	// Token: 0x040003D1 RID: 977
	[Header("一般材質")]
	[SerializeField]
	protected Material material_Normal;

	// Token: 0x040003D2 RID: 978
	[Header("腐化材質")]
	[SerializeField]
	protected Material material_Corrupted;

	// Token: 0x040003D3 RID: 979
	[SerializeField]
	[Header("AI Seeker")]
	protected Seeker seeker;

	// Token: 0x040003D4 RID: 980
	[SerializeField]
	protected AIPath aiPath;

	// Token: 0x040003D5 RID: 981
	[SerializeField]
	protected Vector3 headPosition;

	// Token: 0x040003D6 RID: 982
	[SerializeField]
	[Header("死亡動畫的時間")]
	protected float deadAnimationLength = 1f;

	// Token: 0x040003D7 RID: 983
	[SerializeField]
	[Header("死掉後回收Prefab的延遲時間")]
	protected float despawnDelay = 1f;

	// Token: 0x040003D8 RID: 984
	[SerializeField]
	private string sfxKey_OnDeath = "";

	// Token: 0x040003D9 RID: 985
	[SerializeField]
	protected AMonsterBase.eState state;

	// Token: 0x040003DA RID: 986
	[SerializeField]
	protected int hp;

	// Token: 0x040003DB RID: 987
	[SerializeField]
	protected float speed;

	// Token: 0x040003DC RID: 988
	[SerializeField]
	protected float timeSinceSpawn;

	// Token: 0x040003DD RID: 989
	[SerializeField]
	public MonsterSpawner monsterSpawner;

	// Token: 0x040003DE RID: 990
	[SerializeField]
	protected List<AMonsterBase.MonsterSpeedModifier> list_SpeedModifier;

	// Token: 0x040003DF RID: 991
	[SerializeField]
	protected List<MonsterDamageDebuff> list_MonsterDamageDebuff;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	protected bool doDropChestOnDeath;

	// Token: 0x040003E1 RID: 993
	[SerializeField]
	protected bool isImpendingDeath;

	// Token: 0x040003E2 RID: 994
	private bool isCalculatingPath;

	// Token: 0x040003E3 RID: 995
	private bool canMove = true;

	// Token: 0x040003E4 RID: 996
	public Action<AMonsterBase> OnMonsterKilled;

	// Token: 0x040003E5 RID: 997
	public Action<AMonsterBase> OnMonsterDespawn;

	// Token: 0x040003E6 RID: 998
	public Action OnMonsterDamageDebuffChange;

	// Token: 0x040003E7 RID: 999
	public Action OnMonsterHPChange;

	// Token: 0x040003E8 RID: 1000
	[SerializeField]
	private MaterialPropertyBlock matPropBlock;

	// Token: 0x040003E9 RID: 1001
	private Coroutine cr_HitFlashEffect;

	// Token: 0x02000217 RID: 535
	public class MonsterSpeedModifier
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x000328E3 File Offset: 0x00030AE3
		public MonsterSpeedModifier(float value, float duration, bool isFromPlayer)
		{
			this.value = value;
			this.duration = duration;
			this.isFromPlayer = isFromPlayer;
		}

		// Token: 0x04000A7F RID: 2687
		public bool isFromPlayer = true;

		// Token: 0x04000A80 RID: 2688
		public float value;

		// Token: 0x04000A81 RID: 2689
		public float duration;
	}

	// Token: 0x02000218 RID: 536
	public enum eState
	{
		// Token: 0x04000A83 RID: 2691
		NONE,
		// Token: 0x04000A84 RID: 2692
		ALIVE,
		// Token: 0x04000A85 RID: 2693
		KILLED,
		// Token: 0x04000A86 RID: 2694
		DESPAWNED,
		// Token: 0x04000A87 RID: 2695
		REMOVED
	}
}
