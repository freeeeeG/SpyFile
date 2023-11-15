using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Behaviours;
using FX;
using GameResources;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200102E RID: 4142
	public abstract class AIController : MonoBehaviour
	{
		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06004FC5 RID: 20421 RVA: 0x000F0BA6 File Offset: 0x000EEDA6
		// (set) Token: 0x06004FC6 RID: 20422 RVA: 0x000F0BAE File Offset: 0x000EEDAE
		public Character target { get; set; }

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x000F0BB7 File Offset: 0x000EEDB7
		// (set) Token: 0x06004FC8 RID: 20424 RVA: 0x000F0BBF File Offset: 0x000EEDBF
		public Character lastAttacker { get; set; }

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x000F0BC8 File Offset: 0x000EEDC8
		// (set) Token: 0x06004FCA RID: 20426 RVA: 0x000F0BD0 File Offset: 0x000EEDD0
		public List<Characters.AI.Behaviours.Behaviour> behaviours { private get; set; }

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x000F0BD9 File Offset: 0x000EEDD9
		// (set) Token: 0x06004FCC RID: 20428 RVA: 0x000F0BE1 File Offset: 0x000EEDE1
		public AIController.StartOption startOption
		{
			get
			{
				return this._startOption;
			}
			set
			{
				this._startOption = value;
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x000F0BEA File Offset: 0x000EEDEA
		// (set) Token: 0x06004FCE RID: 20430 RVA: 0x000F0BF2 File Offset: 0x000EEDF2
		public Vector2 destination { get; set; }

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06004FCF RID: 20431 RVA: 0x000F0BFB File Offset: 0x000EEDFB
		public bool dead
		{
			get
			{
				return this.character.health.dead;
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x000F0C0D File Offset: 0x000EEE0D
		public bool stuned
		{
			get
			{
				return this.character.status.stuned || this.character.status.unmovable;
			}
		}

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06004FD1 RID: 20433 RVA: 0x000F0C34 File Offset: 0x000EEE34
		// (remove) Token: 0x06004FD2 RID: 20434 RVA: 0x000F0C6C File Offset: 0x000EEE6C
		public event Action onFind;

		// Token: 0x06004FD3 RID: 20435 RVA: 0x000F0CA4 File Offset: 0x000EEEA4
		static AIController()
		{
			AIController._playerOverlapper.contactFilter.SetLayerMask(512);
			AIController._groundOverlapper = new NonAllocOverlapper(15);
			AIController._groundOverlapper.contactFilter.SetLayerMask(Layers.groundMask);
			AIController._enemyOverlapper = new NonAllocOverlapper(31);
			AIController._enemyOverlapper.contactFilter.SetLayerMask(1024);
			AIController._reusableCaster = new NonAllocCaster(15);
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x000F0D27 File Offset: 0x000EEF27
		public void RunProcess()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06004FD5 RID: 20437
		protected abstract IEnumerator CProcess();

		// Token: 0x06004FD6 RID: 20438 RVA: 0x000F0D36 File Offset: 0x000EEF36
		public void FoundEnemy()
		{
			Action action = this.onFind;
			if (action != null)
			{
				action();
			}
			this.NotifyHitEvent();
			if (this._hideFindEffect)
			{
				return;
			}
			this.SpawnFindTargetEffect();
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x000F0D60 File Offset: 0x000EEF60
		private void SpawnFindTargetEffect()
		{
			Vector2 v;
			if (this._findEffectTransform == null)
			{
				Bounds bounds = this.character.collider.bounds;
				v = new Vector3(bounds.center.x, bounds.max.y);
			}
			else
			{
				v = this._findEffectTransform.position;
			}
			AIController.Assets.effect.Spawn(v, 0f, 1f);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x000F0DE0 File Offset: 0x000EEFE0
		public Character FindClosestPlayerBody(Collider2D collider)
		{
			collider.enabled = true;
			List<Target> components = AIController._playerOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				collider.enabled = false;
				return null;
			}
			if (components.Count == 1)
			{
				collider.enabled = false;
				return components[0].character;
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < components.Count; i++)
			{
				float distance = Physics2D.Distance(components[i].character.collider, this.character.collider).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
			collider.enabled = false;
			return components[index].character;
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x000F0E98 File Offset: 0x000EF098
		public Character FindClosestPlayerBody(Collider2D range, Vector3 origin, LayerMask blockLayerMask)
		{
			range.enabled = true;
			List<Target> components = AIController._playerOverlapper.OverlapCollider(range).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				range.enabled = false;
				return null;
			}
			float num = float.MaxValue;
			int num2 = -1;
			AIController._reusableCaster.contactFilter.SetLayerMask(blockLayerMask);
			for (int i = 0; i < components.Count; i++)
			{
				Character character = components[i].character;
				float distance = Physics2D.Distance(character.collider, this.character.collider).distance;
				if (AIController._reusableCaster.RayCast(origin, character.collider.bounds.center - origin, distance * 1.5f).results.Count <= 0 && num > distance)
				{
					num2 = i;
					num = distance;
				}
			}
			range.enabled = false;
			if (num2 == -1)
			{
				return null;
			}
			return components[num2].character;
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x000F0F96 File Offset: 0x000EF196
		public List<Character> FindEnemiesInRange(Collider2D collider)
		{
			collider.enabled = true;
			List<Character> components = AIController._enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
			collider.enabled = false;
			return components;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x000F0FB8 File Offset: 0x000EF1B8
		public Collider2D FindClosestGround(Collider2D collider)
		{
			collider.enabled = true;
			ReadonlyBoundedList<Collider2D> results = AIController._groundOverlapper.OverlapCollider(collider).results;
			if (results.Count == 0)
			{
				collider.enabled = false;
				return null;
			}
			if (results.Count == 1)
			{
				collider.enabled = false;
				return results[0];
			}
			float num = float.MaxValue;
			int index = 0;
			for (int i = 1; i < results.Count; i++)
			{
				float distance = Physics2D.Distance(results[i], this.character.collider).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
			collider.enabled = false;
			return results[index];
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x000F105C File Offset: 0x000EF25C
		public List<Character> FindRandomEnemies(Collider2D collider, Character except, int amount)
		{
			collider.enabled = true;
			List<Character> components = AIController._enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
			foreach (Character character in components)
			{
				if (character == except)
				{
					components.Remove(character);
					break;
				}
			}
			if (components.Count <= 0)
			{
				collider.enabled = false;
				return null;
			}
			int[] array = Enumerable.Range(0, components.Count).ToArray<int>();
			array.PseudoShuffle<int>();
			IEnumerable<int> enumerable = array.Take(amount);
			List<Character> list = new List<Character>(components.Count);
			foreach (int index in enumerable)
			{
				list.Add(components[index]);
			}
			collider.enabled = false;
			return list;
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x000F1158 File Offset: 0x000EF358
		protected virtual void OnEnable()
		{
			if (this.character.health == null)
			{
				return;
			}
			this.character.health.onTookDamage += new TookDamageDelegate(this.onTookDamage);
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x000F118A File Offset: 0x000EF38A
		protected virtual void OnDisable()
		{
			if (this.character.health == null)
			{
				return;
			}
			this.character.health.onTookDamage -= new TookDamageDelegate(this.onTookDamage);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x000F11BC File Offset: 0x000EF3BC
		protected void Start()
		{
			base.StartCoroutine(this.CCheckStun());
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x000F11CC File Offset: 0x000EF3CC
		private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (damageDealt <= 0.0 || originalDamage.attacker.character == null || originalDamage.attacker.character.collider == null || originalDamage.attacker.character.health == null)
			{
				return;
			}
			if (originalDamage.attacker.character.gameObject.layer != 9)
			{
				return;
			}
			if (this.target == null)
			{
				this.FoundEnemy();
			}
			this.target = originalDamage.attacker.character;
			this.lastAttacker = originalDamage.attacker.character;
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x000F127C File Offset: 0x000EF47C
		private void NotifyHitEvent()
		{
			if (this._notifyCollider == null)
			{
				return;
			}
			List<Character> list = this.FindEnemiesInRange(this._notifyCollider);
			Collider2D lastStandingCollider = this.character.movement.controller.collisionState.lastStandingCollider;
			foreach (Character character in list)
			{
				Collider2D lastStandingCollider2 = character.movement.controller.collisionState.lastStandingCollider;
				if (!(lastStandingCollider != lastStandingCollider2))
				{
					AIController componentInChildren = character.GetComponentInChildren<AIController>();
					if (!(componentInChildren == null))
					{
						componentInChildren.target = this.target;
					}
				}
			}
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x000F1338 File Offset: 0x000EF538
		protected IEnumerator CPlayStartOption()
		{
			AIController.StartOption startOption = this._startOption;
			if (startOption != AIController.StartOption.IdleUntilFindTarget)
			{
				if (startOption == AIController.StartOption.SetPlayerAsTarget)
				{
					while (Singleton<Service>.Instance.levelManager.player == null)
					{
						yield return null;
					}
					this.target = Singleton<Service>.Instance.levelManager.player;
				}
			}
			else
			{
				while (this.target == null)
				{
					yield return null;
				}
				this.FoundEnemy();
			}
			yield break;
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x000F1348 File Offset: 0x000EF548
		public void StopAllCoroutinesWithBehaviour()
		{
			base.StopAllCoroutines();
			this.character.CancelAction();
			if (this.behaviours == null)
			{
				return;
			}
			foreach (Characters.AI.Behaviours.Behaviour behaviour in this.behaviours)
			{
				behaviour.StopPropagation();
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x000F13B4 File Offset: 0x000EF5B4
		public void StopAllBehaviour()
		{
			if (this.behaviours == null)
			{
				return;
			}
			foreach (Characters.AI.Behaviours.Behaviour behaviour in this.behaviours)
			{
				behaviour.StopPropagation();
			}
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x000F1410 File Offset: 0x000EF610
		protected IEnumerator CCheckStun()
		{
			if (this.character.status == null)
			{
				yield break;
			}
			while (!this.dead)
			{
				yield return null;
				if (this.stuned)
				{
					this.StopAllBehaviour();
				}
			}
			yield break;
		}

		// Token: 0x04004041 RID: 16449
		private static readonly NonAllocOverlapper _playerOverlapper = new NonAllocOverlapper(15);

		// Token: 0x04004042 RID: 16450
		private static readonly NonAllocOverlapper _groundOverlapper;

		// Token: 0x04004043 RID: 16451
		private static readonly NonAllocOverlapper _enemyOverlapper;

		// Token: 0x04004044 RID: 16452
		private static readonly NonAllocCaster _reusableCaster;

		// Token: 0x04004045 RID: 16453
		public Character character;

		// Token: 0x04004046 RID: 16454
		public Collider2D stopTrigger;

		// Token: 0x04004049 RID: 16457
		[SerializeField]
		private Collider2D _notifyCollider;

		// Token: 0x0400404A RID: 16458
		[SerializeField]
		private Transform _findEffectTransform;

		// Token: 0x0400404B RID: 16459
		[SerializeField]
		private bool _hideFindEffect;

		// Token: 0x0400404C RID: 16460
		[SerializeField]
		private AIController.StartOption _startOption;

		// Token: 0x0200102F RID: 4143
		private class Assets
		{
			// Token: 0x04004050 RID: 16464
			internal static readonly EffectInfo effect = new EffectInfo(CommonResource.instance.enemyInSightEffect);
		}

		// Token: 0x02001030 RID: 4144
		public enum StartOption
		{
			// Token: 0x04004052 RID: 16466
			None,
			// Token: 0x04004053 RID: 16467
			IdleUntilFindTarget,
			// Token: 0x04004054 RID: 16468
			SetPlayerAsTarget
		}
	}
}
