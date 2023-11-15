using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Movements;
using Characters.Operations;
using Characters.Player;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.Yaksha
{
	// Token: 0x02000C20 RID: 3104
	public sealed class YakshaHome : MonoBehaviour
	{
		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x000B8E56 File Offset: 0x000B7056
		private bool isYaksha
		{
			get
			{
				return this._inventory.polymorphOrCurrent.name.Equals(this._yaksha.name);
			}
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x000B8E78 File Offset: 0x000B7078
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(128);
			this._enemies = new List<Character>(128);
			this._onActivate.Initialize();
			this._onEnter.Initialize();
			this._onExit.Initialize();
			this._owner = this._yaksha.owner;
			this._inventory = this._owner.playerComponents.inventory.weapon;
			this._abilityComponents.Initialize();
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Disappear;
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x000B8F18 File Offset: 0x000B7118
		public void Update()
		{
			if (this._yaksha == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!this._activated)
			{
				return;
			}
			if (!this._ranActivateAction)
			{
				CoroutineProxy.instance.StartCoroutine(this._onActivate.CRun(this._owner));
				this._ranActivateAction = true;
			}
			this._remainTime -= Chronometer.global.deltaTime;
			if (this._remainTime < 0f)
			{
				this.Disappear();
				return;
			}
			this.TryToAddEnemy();
			this.UpdateOwnerState();
			this.UpdateInnerEnemeyForce();
			this.CheckOutgoingEnemy();
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x000B8FB8 File Offset: 0x000B71B8
		public void Appear()
		{
			this._ranActivateAction = false;
			this.ClearEnmies();
			this.ResetValue();
			this._inventory.onSwap -= this.Disappear;
			this._inventory.onSwap += this.Disappear;
			if (this._yaksha != null)
			{
				this._yaksha.onDropped -= this.Disappear;
				this._yaksha.onDropped += this.Disappear;
			}
			this.TryToAddEnemy();
			this.CheckOutgoingEnemy();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x000B905A File Offset: 0x000B725A
		private void ResetValue()
		{
			this._remainTime = this._duration;
			base.transform.position = this._owner.transform.position;
			this._activated = true;
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x000B908C File Offset: 0x000B728C
		public void Disappear()
		{
			try
			{
				this._owner.StartCoroutine(this.CExit());
				this.ClearEnmies();
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("Yaksha Home Error \n{0}", arg));
			}
			this._activated = false;
			this._playerisStaying = false;
			this._ranActivateAction = false;
			if (this._inventory != null)
			{
				this._inventory.onSwap -= this.Disappear;
			}
			if (this._yaksha != null)
			{
				this._yaksha.onDropped -= this.Disappear;
			}
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x000B9150 File Offset: 0x000B7350
		private void UpdateOwnerState()
		{
			if (this.InEnterArea(this._owner.transform.position))
			{
				if (!this._playerisStaying)
				{
					this.EnterPlayer();
					return;
				}
			}
			else if (this._playerisStaying && (this._owner.runningMotion == null || (this._owner.runningMotion.action.type != Characters.Actions.Action.Type.Skill && this._owner.runningMotion.action.type != Characters.Actions.Action.Type.Dash && this._owner.runningMotion.action.type != Characters.Actions.Action.Type.JumpAttack)))
			{
				this.ExitPlayer();
			}
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x000B91F4 File Offset: 0x000B73F4
		private void TryToAddEnemy()
		{
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._owner.gameObject));
			List<Target> components = this._overlapper.OverlapCircle(this._enterBoundary.transform.position, this._enterBoundary.radius).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				return;
			}
			for (int i = 0; i < components.Count; i++)
			{
				Character character = components[i].character;
				if (!(character == null) && !this._enemies.Contains(character))
				{
					this._enemies.Add(character);
					for (int j = 0; j < this._abilityComponents.components.Length; j++)
					{
						character.ability.Add(this._abilityComponents.components[j].ability);
					}
				}
			}
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x000B92DC File Offset: 0x000B74DC
		private void ClearEnmies()
		{
			foreach (Character character in this._enemies)
			{
				foreach (AbilityComponent abilityComponent in this._abilityComponents.components)
				{
					character.ability.Remove(abilityComponent.ability);
				}
			}
			this._enemies.Clear();
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x000B9368 File Offset: 0x000B7568
		private void UpdateInnerEnemeyForce()
		{
			foreach (Character enemy in this._enemies)
			{
				this.TryFastenEnemy(enemy);
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x000B93BC File Offset: 0x000B75BC
		private void CheckOutgoingEnemy()
		{
			for (int i = this._enemies.Count - 1; i >= 0; i--)
			{
				if (this.OutExitArea(this._enemies[i].transform.position))
				{
					foreach (AbilityComponent abilityComponent in this._abilityComponents.components)
					{
						this._enemies[i].ability.Remove(abilityComponent.ability);
					}
					this._enemies.RemoveAt(i);
				}
			}
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x000B944B File Offset: 0x000B764B
		private void EnterPlayer()
		{
			this._playerisStaying = true;
			if (!this.isYaksha)
			{
				return;
			}
			this._onEnter.StopAll();
			CoroutineProxy.instance.StartCoroutine(this._onEnter.CRun(this._owner));
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x000B9484 File Offset: 0x000B7684
		private void ExitPlayer()
		{
			this._playerisStaying = false;
			this._onEnter.StopAll();
			CoroutineProxy.instance.StartCoroutine(this._onExit.CRun(this._owner));
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x000B94B4 File Offset: 0x000B76B4
		private IEnumerator CExit()
		{
			while (this._owner.runningMotion != null && (this._owner.runningMotion.action.type == Characters.Actions.Action.Type.Skill || this._owner.runningMotion.action.type == Characters.Actions.Action.Type.Dash || this._owner.runningMotion.action.type == Characters.Actions.Action.Type.JumpAttack))
			{
				yield return null;
			}
			this.ExitPlayer();
			yield break;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x000B94C4 File Offset: 0x000B76C4
		private void TryFastenEnemy(Character enemy)
		{
			if (Chronometer.global.timeScale == 0f)
			{
				return;
			}
			if (this.InEnterArea(enemy.transform.position))
			{
				return;
			}
			enemy.movement.push.ApplyKnockback(this._owner, this._pushInfo);
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x000B9519 File Offset: 0x000B7719
		private bool InEnterArea(Vector2 position)
		{
			return Vector2.SqrMagnitude(this._enterBoundary.transform.position - position) < this._enterBoundary.radius * this._enterBoundary.radius;
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x000B9557 File Offset: 0x000B7757
		private bool OutExitArea(Vector2 position)
		{
			return Vector2.SqrMagnitude(this._enterBoundary.transform.position - position) > this._exitBoundary.radius * this._exitBoundary.radius;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x000B9595 File Offset: 0x000B7795
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Disappear;
		}

		// Token: 0x04003110 RID: 12560
		[SerializeField]
		private Weapon _yaksha;

		// Token: 0x04003111 RID: 12561
		[SerializeField]
		[Header("사용자 설정")]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onActivate;

		// Token: 0x04003112 RID: 12562
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onEnter;

		// Token: 0x04003113 RID: 12563
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onExit;

		// Token: 0x04003114 RID: 12564
		[SerializeField]
		[Header("영역 설정")]
		private float _duration;

		// Token: 0x04003115 RID: 12565
		[SerializeField]
		private CircleCollider2D _enterBoundary;

		// Token: 0x04003116 RID: 12566
		[SerializeField]
		private CircleCollider2D _exitBoundary;

		// Token: 0x04003117 RID: 12567
		[Header("적 설정")]
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);

		// Token: 0x04003118 RID: 12568
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04003119 RID: 12569
		[SerializeField]
		private Curve _fastenCurve;

		// Token: 0x0400311A RID: 12570
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x0400311B RID: 12571
		private List<Character> _enemies;

		// Token: 0x0400311C RID: 12572
		private NonAllocOverlapper _overlapper;

		// Token: 0x0400311D RID: 12573
		private float _remainTime;

		// Token: 0x0400311E RID: 12574
		private bool _playerisStaying;

		// Token: 0x0400311F RID: 12575
		private bool _activated;

		// Token: 0x04003120 RID: 12576
		private bool _ranActivateAction;

		// Token: 0x04003121 RID: 12577
		private Character _owner;

		// Token: 0x04003122 RID: 12578
		private WeaponInventory _inventory;
	}
}
