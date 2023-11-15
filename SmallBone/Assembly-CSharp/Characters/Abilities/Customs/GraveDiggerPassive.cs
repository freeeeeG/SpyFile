using System;
using System.Collections.Generic;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Minions;
using Characters.Operations;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D56 RID: 3414
	[Serializable]
	public class GraveDiggerPassive : Ability
	{
		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x060044DD RID: 17629 RVA: 0x000C7E00 File Offset: 0x000C6000
		public GraveDiggerCorpse corpse
		{
			get
			{
				return this._corpse;
			}
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x000C7E08 File Offset: 0x000C6008
		public override void Initialize()
		{
			base.Initialize();
			this._onCorpseDied.Initialize();
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x000C7E1B File Offset: 0x000C601B
		public override IAbilityInstance CreateInstance(Character owner)
		{
			if (this._instance == null)
			{
				this._instance = new GraveDiggerPassive.Instance(owner, this);
			}
			this._owner = owner;
			return this._instance;
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x000C7E3F File Offset: 0x000C603F
		public void HandleCorpseDie(Vector3 position)
		{
			if (this._instance == null)
			{
				return;
			}
			this.SpawnGrave(position);
			if (this._reduceSkillCooldown <= 0f)
			{
				return;
			}
			this._instance.ReduceSkillCooldown();
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x000C7E6C File Offset: 0x000C606C
		public void SpawnCorpse(Vector3 position)
		{
			GraveDiggerPassive.<>c__DisplayClass24_0 CS$<>8__locals1 = new GraveDiggerPassive.<>c__DisplayClass24_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.summonMinion = this._corpse.minion.Summon(position);
			CS$<>8__locals1.summonMinion.GetComponent<GraveDiggerCorpse>().SetPassive(this, this._owner);
			CS$<>8__locals1.summonMinion.OnDespawn -= CS$<>8__locals1.<SpawnCorpse>g__OnCorpseDespawn|0;
			CS$<>8__locals1.summonMinion.OnDespawn += CS$<>8__locals1.<SpawnCorpse>g__OnCorpseDespawn|0;
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x000C7EE2 File Offset: 0x000C60E2
		public void SpawnGrave(Vector3 position)
		{
			this._graveDiggerGrave.Spawn(position, this._container);
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x000C7EF6 File Offset: 0x000C60F6
		public void OnDestroy()
		{
			if (this._corpsePosition != null)
			{
				UnityEngine.Object.Destroy(this._corpsePosition);
			}
		}

		// Token: 0x04003475 RID: 13429
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x04003476 RID: 13430
		[SerializeField]
		[Header("Grave Setting")]
		private GraveDiggerGrave _graveDiggerGrave;

		// Token: 0x04003477 RID: 13431
		[SerializeField]
		[Header("Corpse Setting")]
		private GraveDiggerCorpse _corpse;

		// Token: 0x04003478 RID: 13432
		[SerializeField]
		private MinionSetting _corpseSetting;

		// Token: 0x04003479 RID: 13433
		[SerializeField]
		private CustomFloat _corpseSpawnInterval;

		// Token: 0x0400347A RID: 13434
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x0400347B RID: 13435
		[SerializeField]
		private float _spawnRange = 2f;

		// Token: 0x0400347C RID: 13436
		[Header("Corpse Setting")]
		[SerializeField]
		private PoolObject _landOfDeadCorpseSpawner;

		// Token: 0x0400347D RID: 13437
		[SerializeField]
		[Header("망령을 처치 시 스킬 쿨타임 감소")]
		private float _reduceSkillCooldown;

		// Token: 0x0400347E RID: 13438
		[SerializeField]
		[Header("맵 진입 시 생성되는 망령 개수")]
		private CustomFloat _corpseCountOnMap = new CustomFloat(3f, 5f);

		// Token: 0x0400347F RID: 13439
		[SerializeField]
		[Tooltip("적 처치 시 망령 생성확률")]
		[Range(0f, 100f)]
		[Header("Spawn Corpse By Kill")]
		private int _chanceToSpawnCorpseByKill;

		// Token: 0x04003480 RID: 13440
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false
		});

		// Token: 0x04003481 RID: 13441
		[Space]
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x04003482 RID: 13442
		[SerializeField]
		private GraveDiggerGraveContainer _container;

		// Token: 0x04003483 RID: 13443
		[SerializeField]
		private Transform _corpsePosition;

		// Token: 0x04003484 RID: 13444
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onCorpseDied;

		// Token: 0x04003485 RID: 13445
		private GraveDiggerPassive.Instance _instance;

		// Token: 0x04003486 RID: 13446
		private Character _owner;

		// Token: 0x02000D57 RID: 3415
		public class Instance : AbilityInstance<GraveDiggerPassive>
		{
			// Token: 0x060044E5 RID: 17637 RVA: 0x000C7F6E File Offset: 0x000C616E
			public Instance(Character owner, GraveDiggerPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x060044E6 RID: 17638 RVA: 0x000C7F78 File Offset: 0x000C6178
			protected override void OnAttach()
			{
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += this.SpawnCorpseOnMap;
				if (this.ability._chanceToSpawnCorpseByKill > 0)
				{
					Character owner = this.owner;
					owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
				}
			}

			// Token: 0x060044E7 RID: 17639 RVA: 0x000C7FD8 File Offset: 0x000C61D8
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= this.SpawnCorpseOnMap;
				if (this.ability._chanceToSpawnCorpseByKill > 0)
				{
					Character owner = this.owner;
					owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
				}
				this.ability._container.Clear();
				this.ability.corpse.minion.poolObject.DespawnAllSiblings();
				if (this.ability._landOfDeadCorpseSpawner != null)
				{
					this.ability._landOfDeadCorpseSpawner.DespawnAllSiblings();
				}
			}

			// Token: 0x060044E8 RID: 17640 RVA: 0x000C8084 File Offset: 0x000C6284
			public override void UpdateTime(float deltaTime)
			{
				if (!this.ability._constraints.Pass())
				{
					return;
				}
				if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent != this.ability._weapon)
				{
					return;
				}
				base.UpdateTime(deltaTime);
				this.HandlePeriodicCorpseSpawn(deltaTime);
			}

			// Token: 0x060044E9 RID: 17641 RVA: 0x000C80E8 File Offset: 0x000C62E8
			private void SpawnCorpseOnMap(Map old, Map @new)
			{
				if (Map.Instance.type != Map.Type.Normal)
				{
					return;
				}
				List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
				allEnemies.PseudoShuffle<Character>();
				float num = math.min((float)allEnemies.Count, this.ability._corpseCountOnMap.value);
				int num2 = 0;
				while ((float)num2 < num)
				{
					Vector2 v;
					if (this.FindGroundSpawnPosition(allEnemies[num2], out v))
					{
						this.ability.SpawnCorpse(v);
					}
					num2++;
				}
			}

			// Token: 0x060044EA RID: 17642 RVA: 0x000C8164 File Offset: 0x000C6364
			private void OnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!MMMaths.PercentChance(this.ability._chanceToSpawnCorpseByKill))
				{
					return;
				}
				if (!this.ability._characterTypeFilter[character.type])
				{
					return;
				}
				if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent != this.ability._weapon)
				{
					return;
				}
				Vector2 v;
				if (!this.FindGroundSpawnPosition(character, out v))
				{
					return;
				}
				this.ability.SpawnCorpse(v);
			}

			// Token: 0x060044EB RID: 17643 RVA: 0x000C8204 File Offset: 0x000C6404
			private void HandlePeriodicCorpseSpawn(float deltaTime)
			{
				this._remainCorpseSpawnTime -= deltaTime;
				if (this._remainCorpseSpawnTime > 0f)
				{
					return;
				}
				this._remainCorpseSpawnTime += this.ability._corpseSpawnInterval.value;
				Vector2 v;
				if (!this.FindGroundSpawnPosition(this.owner, out v))
				{
					this._remainCorpseSpawnTime += 0.5f;
				}
				this.ability.SpawnCorpse(v);
			}

			// Token: 0x060044EC RID: 17644 RVA: 0x000C8280 File Offset: 0x000C6480
			private Collider2D FindGround(Vector2 position)
			{
				Vector2 vector = position + Vector2.up;
				if (Physics2D.OverlapPoint(vector, Layers.groundMask) != null)
				{
					return null;
				}
				RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.down, this.ability._groundFindingDistance, Layers.groundMask);
				if (!hit)
				{
					return null;
				}
				return hit.collider;
			}

			// Token: 0x060044ED RID: 17645 RVA: 0x000C82E8 File Offset: 0x000C64E8
			private bool FindGroundSpawnPosition(Character character, out Vector2 position)
			{
				position = character.transform.position;
				BoxCollider2D boxCollider2D = (BoxCollider2D)this.ability.corpse.collider;
				Vector2 b = new Vector2(UnityEngine.Random.Range(-this.ability._spawnRange, this.ability._spawnRange), 0f);
				Collider2D collider2D = this.FindGround(position + b);
				if (collider2D == null)
				{
					if (character.movement == null)
					{
						collider2D = this.FindGround(position);
					}
					else
					{
						collider2D = character.movement.controller.collisionState.lastStandingCollider;
					}
				}
				if (collider2D == null)
				{
					return false;
				}
				Bounds bounds = collider2D.bounds;
				float minInclusive = math.max(bounds.min.x + boxCollider2D.size.x, position.x - this.ability._spawnRange);
				float maxInclusive = math.min(bounds.max.x - boxCollider2D.size.x, position.x + this.ability._spawnRange);
				position.x = UnityEngine.Random.Range(minInclusive, maxInclusive);
				position.y = bounds.max.y;
				return true;
			}

			// Token: 0x060044EE RID: 17646 RVA: 0x000C8430 File Offset: 0x000C6630
			public void ReduceSkillCooldown()
			{
				foreach (SkillInfo skillInfo in this.owner.playerComponents.inventory.weapon.current.currentSkills)
				{
					Characters.Actions.Action action = skillInfo.action;
					if (action.cooldown.time != null)
					{
						action.cooldown.time.ReduceCooldown(this.ability._reduceSkillCooldown);
					}
				}
			}

			// Token: 0x04003487 RID: 13447
			private float _remainCorpseSpawnTime;
		}
	}
}
