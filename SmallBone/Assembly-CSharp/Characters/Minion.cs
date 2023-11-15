using System;
using System.Collections;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Minions;
using Characters.Movements;
using Characters.Player;
using Scenes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
	// Token: 0x02000705 RID: 1797
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Character))]
	public class Minion : MonoBehaviour
	{
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x0006D63F File Offset: 0x0006B83F
		public Minion.State state
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x0006D647 File Offset: 0x0006B847
		private bool isActivated
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x0006D654 File Offset: 0x0006B854
		public int maxCount
		{
			get
			{
				return this._defaultSetting.maxCount;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x0006D661 File Offset: 0x0006B861
		// (set) Token: 0x06002455 RID: 9301 RVA: 0x0006D669 File Offset: 0x0006B869
		public float lifeTime { get; private set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x0006D672 File Offset: 0x0006B872
		public Character character
		{
			get
			{
				return this._character;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x0006D67A File Offset: 0x0006B87A
		// (set) Token: 0x06002458 RID: 9304 RVA: 0x0006D682 File Offset: 0x0006B882
		public MinionLeader leader { get; private set; }

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06002459 RID: 9305 RVA: 0x0006D68C File Offset: 0x0006B88C
		// (remove) Token: 0x0600245A RID: 9306 RVA: 0x0006D6C4 File Offset: 0x0006B8C4
		public event Minion.OnSummonDelegate onSummon;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x0600245B RID: 9307 RVA: 0x0006D6FC File Offset: 0x0006B8FC
		// (remove) Token: 0x0600245C RID: 9308 RVA: 0x0006D734 File Offset: 0x0006B934
		public event Minion.OnUnsummonDelegate onUnsummon;

		// Token: 0x0600245D RID: 9309 RVA: 0x0006D769 File Offset: 0x0006B969
		private void Awake()
		{
			if (this._onExpired != null)
			{
				this._onExpired.Initialize(this._character);
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0006D78A File Offset: 0x0006B98A
		private void Update()
		{
			if (!this.isActivated)
			{
				return;
			}
			this.TryToExpire();
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0006D79C File Offset: 0x0006B99C
		public Minion Summon(MinionLeader minionOwner, Vector3 position, MinionGroup group, MinionSetting overrideSetting)
		{
			Minion component = this._poolObject.Spawn(false).GetComponent<Minion>();
			if (overrideSetting != null)
			{
				component._defaultSetting = overrideSetting;
			}
			component.InitializeState(minionOwner, group);
			Movement movement = component.character.movement;
			if (movement != null && (movement.config.type == Movement.Config.Type.Walking || movement.config.type == Movement.Config.Type.AcceleratingWalking))
			{
				component.character.movement.verticalVelocity = 0f;
				component.character.animationController.ForceUpdate();
			}
			component.transform.position = position;
			return component;
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0006D838 File Offset: 0x0006BA38
		private void InitializeState(MinionLeader leader, MinionGroup group)
		{
			this._state = Minion.State.Summoned;
			this._initialized = true;
			this.leader = leader;
			this.JoinGroup(group);
			this._elapsedLifeTime = 0f;
			this._syncWithOwner.Synchronize(this._character, leader.player);
			this.AttachEvents();
			if (!this._defaultSetting.despawnOnMapChanged)
			{
				Scene<GameBase>.instance.poolObjectContainer.Push(this._poolObject);
			}
			base.gameObject.SetActive(true);
			Minion.OnSummonDelegate onSummonDelegate = this.onSummon;
			if (onSummonDelegate == null)
			{
				return;
			}
			onSummonDelegate(leader.player, this._character);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0006D8D4 File Offset: 0x0006BAD4
		private void TryToExpire()
		{
			this._elapsedLifeTime += Chronometer.global.deltaTime;
			if (this._elapsedLifeTime > this._defaultSetting.lifeTime && this._state != Minion.State.Unsummoning)
			{
				this._state = Minion.State.Expired;
			}
			if (this._state == Minion.State.Expired && this._state != Minion.State.Unsummoning)
			{
				this.Despawn();
			}
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0006D933 File Offset: 0x0006BB33
		public void Despawn()
		{
			this.LeaveGroup();
			if (this._onExpired == null)
			{
				this.RevertState();
				return;
			}
			base.StartCoroutine(this.CDespawn());
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0006D95D File Offset: 0x0006BB5D
		public void DespawnImmediately()
		{
			this.LeaveGroup();
			this.RevertState();
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0006D96B File Offset: 0x0006BB6B
		public void OnDisable()
		{
			if (this._initialized && this._state != Minion.State.Unsummoned)
			{
				this.LeaveGroup();
				this.RevertState();
			}
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0006D989 File Offset: 0x0006BB89
		private IEnumerator CDespawn()
		{
			this._state = Minion.State.Unsummoning;
			this._onExpired.TryStart();
			while (this._onExpired.running)
			{
				yield return null;
			}
			this.RevertState();
			yield break;
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0006D998 File Offset: 0x0006BB98
		private void RevertState()
		{
			this._state = Minion.State.Unsummoned;
			Minion.OnUnsummonDelegate onUnsummonDelegate = this.onUnsummon;
			if (onUnsummonDelegate != null)
			{
				onUnsummonDelegate(this.leader.player, this._character);
			}
			this.DettachEvents();
			this._poolObject.Despawn();
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0006D9D4 File Offset: 0x0006BBD4
		private void JoinGroup(MinionGroup group)
		{
			this._group = group;
			this._group.Join(this);
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0006D9E9 File Offset: 0x0006BBE9
		private void LeaveGroup()
		{
			if (this._group != null)
			{
				this._group.Leave(this);
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0006DA00 File Offset: 0x0006BC00
		private void AttachEvents()
		{
			if (this._defaultSetting.triggerOnKilled)
			{
				Character character = this._character;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			}
			if (this._defaultSetting.triggerOnGiveDamage)
			{
				this._character.onGiveDamage.Add(int.MinValue, new GiveDamageDelegate(this.OnGiveDamage));
			}
			if (this._defaultSetting.triggerOnGaveDamage)
			{
				Character character2 = this._character;
				character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}
			if (this._defaultSetting.triggerOnGaveStatus)
			{
				Character character3 = this._character;
				character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}
			foreach (Stat.OnUpdatedDelegate item in this.leader.player.stat.onUpdated)
			{
				this._character.stat.onUpdated.Add(0, item);
			}
			if (this._defaultSetting.despawnOnSwap)
			{
				this.leader.player.onStartAction += this.OnSwapAction;
			}
			if (this._defaultSetting.despawnOnEssenceChanged)
			{
				this.leader.player.playerComponents.inventory.quintessence.onChanged += this.Despawn;
			}
			if (this._defaultSetting.despawnOnWeaponDropped)
			{
				this.leader.player.playerComponents.inventory.weapon.onChanged += this.OnWeaponChanged;
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0006DBD0 File Offset: 0x0006BDD0
		private void DettachEvents()
		{
			if (this.leader == null)
			{
				return;
			}
			if (this._defaultSetting.triggerOnKilled)
			{
				Character character = this._character;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			}
			if (this._defaultSetting.triggerOnGiveDamage)
			{
				this._character.onGiveDamage.Remove(new GiveDamageDelegate(this.OnGiveDamage));
			}
			if (this._defaultSetting.triggerOnGaveDamage)
			{
				Character character2 = this._character;
				character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
			}
			if (this._defaultSetting.triggerOnGaveStatus)
			{
				Character character3 = this._character;
				character3.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character3.onGaveStatus, new Character.OnGaveStatusDelegate(this.OnGaveStatus));
			}
			this._character.stat.onUpdated.Clear();
			if (this._defaultSetting.despawnOnSwap)
			{
				this.leader.player.onStartAction -= this.OnSwapAction;
			}
			if (this._defaultSetting.despawnOnEssenceChanged)
			{
				this.leader.player.playerComponents.inventory.quintessence.onChanged -= this.Despawn;
			}
			if (this._defaultSetting.despawnOnWeaponDropped)
			{
				this.leader.player.playerComponents.inventory.weapon.onChanged -= this.OnWeaponChanged;
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0006DD5A File Offset: 0x0006BF5A
		private void OnKilled(ITarget target, ref Damage damage)
		{
			Character.OnKilledDelegate onKilled = this.leader.player.onKilled;
			if (onKilled == null)
			{
				return;
			}
			onKilled(target, ref damage);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0006DD78 File Offset: 0x0006BF78
		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			return this.leader.player.onGiveDamage.Invoke(target, ref damage);
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x0006DD91 File Offset: 0x0006BF91
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			GaveDamageDelegate onGaveDamage = this.leader.player.onGaveDamage;
			if (onGaveDamage == null)
			{
				return;
			}
			onGaveDamage(target, originalDamage, gaveDamage, damageDealt);
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x0006DDB2 File Offset: 0x0006BFB2
		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			Character.OnGaveStatusDelegate onGaveStatus = this.leader.player.onGaveStatus;
			if (onGaveStatus == null)
			{
				return;
			}
			onGaveStatus(target, applyInfo, result);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x0006DDD1 File Offset: 0x0006BFD1
		private void OnSwapAction(Characters.Actions.Action action)
		{
			if (action.type != Characters.Actions.Action.Type.Swap)
			{
				return;
			}
			this.Despawn();
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x0006DDE3 File Offset: 0x0006BFE3
		private void OnWeaponChanged(Weapon old, Weapon @new)
		{
			this.Despawn();
		}

		// Token: 0x04001EF5 RID: 7925
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x04001EF6 RID: 7926
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001EF7 RID: 7927
		[SerializeField]
		private MinionSetting _defaultSetting;

		// Token: 0x04001EF8 RID: 7928
		[SerializeField]
		private CharacterSynchronization _syncWithOwner;

		// Token: 0x04001EF9 RID: 7929
		[FormerlySerializedAs("_onDespawn")]
		[SerializeField]
		private Characters.Actions.Action _onExpired;

		// Token: 0x04001EFA RID: 7930
		private Minion.State _state;

		// Token: 0x04001EFB RID: 7931
		private MinionGroup _group;

		// Token: 0x04001EFC RID: 7932
		private float _elapsedLifeTime;

		// Token: 0x04001F01 RID: 7937
		private bool _initialized;

		// Token: 0x02000706 RID: 1798
		public enum State
		{
			// Token: 0x04001F03 RID: 7939
			Unsummoned,
			// Token: 0x04001F04 RID: 7940
			Summoned,
			// Token: 0x04001F05 RID: 7941
			Expired,
			// Token: 0x04001F06 RID: 7942
			Unsummoning
		}

		// Token: 0x02000707 RID: 1799
		// (Invoke) Token: 0x06002473 RID: 9331
		public delegate void OnSummonDelegate(Character owner, Character summoned);

		// Token: 0x02000708 RID: 1800
		// (Invoke) Token: 0x06002477 RID: 9335
		public delegate void OnUnsummonDelegate(Character owner, Character summoned);
	}
}
