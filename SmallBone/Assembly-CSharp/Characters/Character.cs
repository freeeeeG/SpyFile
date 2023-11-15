using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Marks;
using Characters.Movements;
using Characters.Player;
using FX;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters
{
	// Token: 0x020006A4 RID: 1700
	[RequireComponent(typeof(CharacterAnimationController))]
	[DisallowMultipleComponent]
	public class Character : MonoBehaviour
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060021ED RID: 8685 RVA: 0x00065D70 File Offset: 0x00063F70
		// (remove) Token: 0x060021EE RID: 8686 RVA: 0x00065DA8 File Offset: 0x00063FA8
		public event System.Action onDie;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060021EF RID: 8687 RVA: 0x00065DE0 File Offset: 0x00063FE0
		// (remove) Token: 0x060021F0 RID: 8688 RVA: 0x00065E18 File Offset: 0x00064018
		public event EvadeDamageDelegate onEvade;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060021F1 RID: 8689 RVA: 0x00065E50 File Offset: 0x00064050
		// (remove) Token: 0x060021F2 RID: 8690 RVA: 0x00065E88 File Offset: 0x00064088
		public event Action<Characters.Actions.Action> onStartAction;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060021F3 RID: 8691 RVA: 0x00065EC0 File Offset: 0x000640C0
		// (remove) Token: 0x060021F4 RID: 8692 RVA: 0x00065EF8 File Offset: 0x000640F8
		public event Action<Characters.Actions.Action> onEndAction;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060021F5 RID: 8693 RVA: 0x00065F30 File Offset: 0x00064130
		// (remove) Token: 0x060021F6 RID: 8694 RVA: 0x00065F68 File Offset: 0x00064168
		public event Action<Characters.Actions.Action> onCancelAction;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060021F7 RID: 8695 RVA: 0x00065FA0 File Offset: 0x000641A0
		// (remove) Token: 0x060021F8 RID: 8696 RVA: 0x00065FD8 File Offset: 0x000641D8
		public event Character.OnStartMotionDelegate onStartMotion;

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060021F9 RID: 8697 RVA: 0x0006600D File Offset: 0x0006420D
		public Key key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060021FA RID: 8698 RVA: 0x00066015 File Offset: 0x00064215
		public CharacterHealth health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060021FB RID: 8699 RVA: 0x0006601D File Offset: 0x0006421D
		public CharacterHit hit
		{
			get
			{
				return this._hit;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060021FC RID: 8700 RVA: 0x00066025 File Offset: 0x00064225
		public BoxCollider2D collider
		{
			get
			{
				return this._collider;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060021FD RID: 8701 RVA: 0x0006602D File Offset: 0x0006422D
		public Movement movement
		{
			get
			{
				return this._movement;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x00066035 File Offset: 0x00064235
		public CharacterAnimationController animationController
		{
			get
			{
				return this._animationController;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x0006603D File Offset: 0x0006423D
		public CharacterStatus status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x00066045 File Offset: 0x00064245
		public bool stunedOrFreezed
		{
			get
			{
				return this._status != null && (this._status.stuned || this._status.freezed || this._status.unmovable);
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x0006607E File Offset: 0x0006427E
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x00066086 File Offset: 0x00064286
		public Character.Type type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x0006608F File Offset: 0x0006428F
		public Character.SizeForEffect sizeForEffect
		{
			get
			{
				return this._sizeForEffect;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06002204 RID: 8708 RVA: 0x00066097 File Offset: 0x00064297
		public SortingGroup sortingGroup
		{
			get
			{
				return this._sortingGroup;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x0006609F File Offset: 0x0006429F
		public Transform @base
		{
			get
			{
				return this._base;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002206 RID: 8710 RVA: 0x000660A7 File Offset: 0x000642A7
		// (set) Token: 0x06002207 RID: 8711 RVA: 0x000660B0 File Offset: 0x000642B0
		public Character.LookingDirection lookingDirection
		{
			get
			{
				return this._lookingDirection;
			}
			set
			{
				this.desiringLookingDirection = value;
				if (this.blockLook.value)
				{
					return;
				}
				this._lookingDirection = value;
				if (this._lookingDirection == Character.LookingDirection.Right)
				{
					this._animationController.parameter.flipX = false;
					this.attachWithFlip.transform.localScale = Vector3.one;
					return;
				}
				this._animationController.parameter.flipX = true;
				this.attachWithFlip.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x0006613D File Offset: 0x0006433D
		// (set) Token: 0x06002209 RID: 8713 RVA: 0x00066145 File Offset: 0x00064345
		public PlayerComponents playerComponents { get; private set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600220A RID: 8714 RVA: 0x0006614E File Offset: 0x0006434E
		// (set) Token: 0x0600220B RID: 8715 RVA: 0x00066156 File Offset: 0x00064356
		public Character.LookingDirection desiringLookingDirection { get; private set; }

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x0006615F File Offset: 0x0006435F
		// (set) Token: 0x0600220D RID: 8717 RVA: 0x00066167 File Offset: 0x00064367
		public List<Characters.Actions.Action> actions { get; private set; } = new List<Characters.Actions.Action>();

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x00066170 File Offset: 0x00064370
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x00066178 File Offset: 0x00064378
		public ISpriteEffectStack spriteEffectStack { get; private set; }

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x00066181 File Offset: 0x00064381
		// (set) Token: 0x06002211 RID: 8721 RVA: 0x00066189 File Offset: 0x00064389
		public Mark mark { get; private set; }

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x00066192 File Offset: 0x00064392
		// (set) Token: 0x06002213 RID: 8723 RVA: 0x0006619A File Offset: 0x0006439A
		public CharacterAbilityManager ability { get; private set; }

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x000661A3 File Offset: 0x000643A3
		// (set) Token: 0x06002215 RID: 8725 RVA: 0x000661AB File Offset: 0x000643AB
		public Silence silence { get; private set; }

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x000661B4 File Offset: 0x000643B4
		// (set) Token: 0x06002217 RID: 8727 RVA: 0x000661BC File Offset: 0x000643BC
		public Characters.Actions.Motion motion { get; private set; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000661C5 File Offset: 0x000643C5
		public Characters.Actions.Motion runningMotion
		{
			get
			{
				if (this.motion == null || !this.motion.running)
				{
					return null;
				}
				return this.motion;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000661EA File Offset: 0x000643EA
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x000661F2 File Offset: 0x000643F2
		public GameObject attach
		{
			get
			{
				return this._attach;
			}
			set
			{
				this._attach = value;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000661FB File Offset: 0x000643FB
		// (set) Token: 0x0600221C RID: 8732 RVA: 0x00066203 File Offset: 0x00064403
		public GameObject attachWithFlip { get; private set; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x0006620C File Offset: 0x0006440C
		public bool liveAndActive
		{
			get
			{
				if (this.health != null)
				{
					return !this.health.dead && base.gameObject.activeSelf;
				}
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x00066244 File Offset: 0x00064444
		private Character()
		{
			this.stat = new Stat(this);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x00066308 File Offset: 0x00064508
		protected virtual void Awake()
		{
			if (this.hit == null)
			{
				this.cinematic.Attach(this);
			}
			if (this._attach == null)
			{
				this._attach = new GameObject("_attach");
				this._attach.transform.SetParent(base.transform, false);
			}
			if (this.attachWithFlip == null)
			{
				this.attachWithFlip = new GameObject("attachWithFlip");
				this.attachWithFlip.transform.SetParent(this.attach.transform, false);
			}
			this.spriteEffectStack = base.GetComponent<ISpriteEffectStack>();
			if (this.health != null)
			{
				this.mark = Mark.AddComponent(this);
			}
			this.ability = CharacterAbilityManager.AddComponent(this);
			this.stat.AttachValues(this._baseStat);
			this.stat.Update();
			this.InitializeActions();
			this._animationController.Initialize();
			this._animationController.onExpire += this.OnAnimationExpire;
			if (this._health != null)
			{
				this._health.owner = this;
				this._health.SetMaximumHealth(this.stat.Get(Stat.Category.Final, Stat.Kind.Health));
				this._health.ResetToMaximumHealth();
				this._health.onDie += this.OnDie;
				this._health.onTakeDamage.Add(0, delegate(ref Damage damage)
				{
					return this.stat.ApplyDefense(ref damage);
				});
				this._health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.<Awake>g__CancelDamage|127_1));
			}
			if (this.type == Character.Type.Player)
			{
				this.playerComponents = new PlayerComponents(this);
				this.playerComponents.Initialize();
			}
			this.silence = new Silence(this);
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000664DD File Offset: 0x000646DD
		private void OnDestroy()
		{
			if (this.type == Character.Type.Player)
			{
				this.playerComponents.Dispose();
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000664F4 File Offset: 0x000646F4
		protected virtual void Update()
		{
			float deltaTime = this.chronometer.master.deltaTime;
			this.stat.TakeTime(deltaTime);
			PlayerComponents playerComponents = this.playerComponents;
			if (playerComponents != null)
			{
				playerComponents.Update(deltaTime);
			}
			if (this._health == null)
			{
				this.stat.UpdateIfNecessary();
			}
			else if (this.stat.UpdateIfNecessary())
			{
				double num = math.ceil(this.stat.Get(Stat.Category.Final, Stat.Kind.Health));
				double current = math.ceil(this._health.percent * num);
				this._health.SetHealth(current, num);
			}
			double final = this.stat.GetFinal(Stat.Kind.CharacterSize);
			Vector3 localScale = base.transform.localScale;
			base.transform.localScale = Vector3.one * Mathf.Lerp(localScale.x, (float)final, Time.deltaTime * 10f);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000665E0 File Offset: 0x000647E0
		protected void OnDie()
		{
			System.Action action = this.onDie;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000665F2 File Offset: 0x000647F2
		public void Attack(ITarget target, ref Damage damage)
		{
			if (target.character != null)
			{
				this.TryAttackCharacter(target, ref damage);
				return;
			}
			if (target.damageable != null)
			{
				this.AttackDamageable(target, ref damage);
			}
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x00066622 File Offset: 0x00064822
		public void Attack(Character character, ref Damage damage)
		{
			this.TryAttackCharacter(new TargetStruct(character), ref damage);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x00066637 File Offset: 0x00064837
		public void Attack(DestructibleObject damageable, ref Damage damage)
		{
			this.AttackDamageable(new TargetStruct(damageable), ref damage);
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x0006664C File Offset: 0x0006484C
		public bool TryAttackCharacter(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (character.health.dead)
			{
				return false;
			}
			Damage damage2 = damage;
			if (this.onGiveDamage.Invoke(target, ref damage))
			{
				return false;
			}
			character.hit.Stop(damage.stoppingPower);
			double damageDealt;
			if (character.health.TakeDamage(ref damage, out damageDealt))
			{
				return false;
			}
			if (character.type == Character.Type.Player)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(damage);
			}
			else
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnTakingDamage(damage);
			}
			GaveDamageDelegate gaveDamageDelegate = this.onGaveDamage;
			if (gaveDamageDelegate != null)
			{
				gaveDamageDelegate(target, damage2, damage, damageDealt);
			}
			if (target.character.health.dead)
			{
				Character.OnKilledDelegate onKilledDelegate = this.onKilled;
				if (onKilledDelegate != null)
				{
					onKilledDelegate(target, ref damage);
				}
			}
			return true;
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00066714 File Offset: 0x00064914
		public void AttackDamageable(ITarget target, ref Damage damage)
		{
			DestructibleObject damageable = target.damageable;
			Damage damage2 = damage;
			GiveDamageEvent giveDamageEvent = this.onGiveDamage;
			if (giveDamageEvent != null && !giveDamageEvent.Invoke(target, ref damage))
			{
				return;
			}
			damageable.Hit(this, ref damage);
			if (damage.amount > 0.0)
			{
				Singleton<Service>.Instance.floatingTextSpawner.SpawnTakingDamage(damage);
			}
			GaveDamageDelegate gaveDamageDelegate = this.onGaveDamage;
			if (gaveDamageDelegate != null)
			{
				gaveDamageDelegate(target, damage2, damage, 0.0);
			}
			if (target.damageable.destroyed)
			{
				Character.OnKilledDelegate onKilledDelegate = this.onKilled;
				if (onKilledDelegate == null)
				{
					return;
				}
				onKilledDelegate(target, ref damage);
			}
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000667AF File Offset: 0x000649AF
		public void TryKillTarget(ITarget target, ref Damage damage)
		{
			if (target.character == null)
			{
				return;
			}
			target.character.health.TryKill();
			Character.OnKilledDelegate onKilledDelegate = this.onKilled;
			if (onKilledDelegate == null)
			{
				return;
			}
			onKilledDelegate(target, ref damage);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000667E4 File Offset: 0x000649E4
		public bool GiveStatus(Character target, CharacterStatus.ApplyInfo status)
		{
			if (target.status == null)
			{
				Character.OnGaveStatusDelegate onGaveStatusDelegate = this.onGaveStatus;
				if (onGaveStatusDelegate != null)
				{
					onGaveStatusDelegate(target, status, false);
				}
				return false;
			}
			bool result = target.status.Apply(this, status);
			Character.OnGaveStatusDelegate onGaveStatusDelegate2 = this.onGaveStatus;
			if (onGaveStatusDelegate2 != null)
			{
				onGaveStatusDelegate2(target, status, result);
			}
			return result;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x00066838 File Offset: 0x00064A38
		private IEnumerator CWaitForEndOfAction(Characters.Actions.Action action)
		{
			yield return this.motion.action.CWaitForEndOfRunning();
			Action<Characters.Actions.Action> action2 = this.onEndAction;
			if (action2 != null)
			{
				action2(action);
			}
			yield break;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x0006684E File Offset: 0x00064A4E
		public void DoAction(Characters.Actions.Motion motion, float speedMultiplier)
		{
			this.DoAction(motion, speedMultiplier, true);
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x0006685C File Offset: 0x00064A5C
		public void DoAction(Characters.Actions.Motion motion, float speedMultiplier, bool triggerOnStartAction)
		{
			Character.<>c__DisplayClass140_0 CS$<>8__locals1 = new Character.<>c__DisplayClass140_0();
			CS$<>8__locals1.motion = motion;
			CS$<>8__locals1.<>4__this = this;
			Characters.Actions.Motion motion2 = this.motion;
			if (motion2 != null && motion2.running)
			{
				this.CancelAction();
			}
			this.DoMotion(CS$<>8__locals1.motion, speedMultiplier);
			if (CS$<>8__locals1.motion.action != null)
			{
				this._cWaitForEndOfAction.Stop();
				if (base.isActiveAndEnabled)
				{
					this._cWaitForEndOfAction = this.StartCoroutineWithReference(this.CWaitForEndOfAction(CS$<>8__locals1.motion.action));
				}
				else
				{
					Debug.LogWarning("Coroutine couldn't be started because the character is not active or disabled, so use onEnd event");
					CS$<>8__locals1.motion.action.onEnd += CS$<>8__locals1.<DoAction>g__onActionEnd|0;
				}
				if (triggerOnStartAction)
				{
					Action<Characters.Actions.Action> action = this.onStartAction;
					if (action == null)
					{
						return;
					}
					action(CS$<>8__locals1.motion.action);
				}
			}
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00066931 File Offset: 0x00064B31
		public void TriggerOnStartActionManually(Characters.Actions.Action action)
		{
			Action<Characters.Actions.Action> action2 = this.onStartAction;
			if (action2 == null)
			{
				return;
			}
			action2(action);
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x00066944 File Offset: 0x00064B44
		public void DoActionNonBlock(Characters.Actions.Motion motion)
		{
			Action<Characters.Actions.Action> action = this.onStartAction;
			if (action != null)
			{
				action(motion.action);
			}
			motion.StartBehaviour(1f);
			motion.EndBehaviour();
			Action<Characters.Actions.Action> action2 = this.onEndAction;
			if (action2 == null)
			{
				return;
			}
			action2(motion.action);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x00066984 File Offset: 0x00064B84
		public void DoMotion(Characters.Actions.Motion motion, float speedMultiplier = 1f)
		{
			Characters.Actions.Motion motion2 = this.motion;
			if (motion2 != null && motion2.running)
			{
				if (motion2.action != motion.action)
				{
					this.CancelAction();
				}
				else
				{
					this._animationController.StopAll();
					Movement movement = this.movement;
					if (movement != null)
					{
						movement.blocked.Detach(this);
					}
					this.blockLook.Detach(this);
					motion2.CancelBehaviour();
				}
			}
			this.motion = motion;
			float num = motion.speed * speedMultiplier;
			if (motion.stay)
			{
				this._animationController.Play(motion.animationInfo, num);
			}
			else
			{
				this._animationController.Play(motion.animationInfo, motion.length / num, num);
			}
			this.blockLook.Detach(this);
			this.lookingDirection = this.desiringLookingDirection;
			motion.StartBehaviour(num);
			Character.OnStartMotionDelegate onStartMotionDelegate = this.onStartMotion;
			if (onStartMotionDelegate != null)
			{
				onStartMotionDelegate(motion, num);
			}
			if (this.movement != null)
			{
				this.movement.blocked.Detach(this);
				if (motion.blockMovement && motion.length > 0f)
				{
					this.movement.blocked.Attach(this);
				}
			}
			if (motion.blockLook)
			{
				this.blockLook.Attach(this);
			}
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00066AD0 File Offset: 0x00064CD0
		public void InitializeActions()
		{
			base.GetComponentsInChildren<Characters.Actions.Action>(true, this.actions);
			this.actions.Sort((Characters.Actions.Action a, Characters.Actions.Action b) => a.priority.CompareTo(b.priority));
			foreach (Characters.Actions.Action action in this.actions)
			{
				action.Initialize(this);
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x00066B58 File Offset: 0x00064D58
		public void CancelAction()
		{
			if (this.motion == null)
			{
				return;
			}
			this._animationController.StopAll();
			this._cWaitForEndOfAction.Stop();
			if (this.onCancelAction != null && this.motion.action != null)
			{
				this.onCancelAction(this.motion.action);
			}
			Movement movement = this.movement;
			if (movement != null)
			{
				movement.blocked.Detach(this);
			}
			this.blockLook.Detach(this);
			this.motion.CancelBehaviour();
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x00066BEC File Offset: 0x00064DEC
		public Character.LookingDirection DesireToLookAt(float targetX)
		{
			if (base.transform.position.x <= targetX)
			{
				return this.desiringLookingDirection = Character.LookingDirection.Right;
			}
			return this.desiringLookingDirection = Character.LookingDirection.Left;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x00066C24 File Offset: 0x00064E24
		public Character.LookingDirection ForceToLookAt(float targetX)
		{
			if (base.transform.position.x <= targetX)
			{
				return this.lookingDirection = Character.LookingDirection.Right;
			}
			return this.lookingDirection = Character.LookingDirection.Left;
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x00066C59 File Offset: 0x00064E59
		public void ForceToLookAt(Character.LookingDirection lookingDirection)
		{
			this.desiringLookingDirection = lookingDirection;
			this._lookingDirection = lookingDirection;
			this._animationController.parameter.flipX = (this._lookingDirection > Character.LookingDirection.Right);
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x00066C82 File Offset: 0x00064E82
		private void OnAnimationExpire()
		{
			Movement movement = this.movement;
			if (movement != null)
			{
				movement.blocked.Detach(this);
			}
			this.blockLook.Detach(this);
			Characters.Actions.Motion motion = this.motion;
			if (motion == null)
			{
				return;
			}
			motion.EndBehaviour();
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x00066CC8 File Offset: 0x00064EC8
		[CompilerGenerated]
		private bool <Awake>g__CancelDamage|127_1(ref Damage damage)
		{
			if (this.cinematic.value)
			{
				return true;
			}
			if ((int)damage.priority > Damage.evasionPriority)
			{
				return false;
			}
			if (this.evasion.value)
			{
				EvadeDamageDelegate evadeDamageDelegate = this.onEvade;
				if (evadeDamageDelegate != null)
				{
					evadeDamageDelegate(ref damage);
				}
				return true;
			}
			if ((int)damage.priority > Damage.invulnerablePriority)
			{
				return false;
			}
			if (this.invulnerable.value)
			{
				damage.@null = true;
			}
			return false;
		}

		// Token: 0x04001CE5 RID: 7397
		private const float sizeLerpSpeed = 10f;

		// Token: 0x04001CE6 RID: 7398
		public readonly GiveDamageEvent onGiveDamage = new GiveDamageEvent();

		// Token: 0x04001CE7 RID: 7399
		public GaveDamageDelegate onGaveDamage;

		// Token: 0x04001CE8 RID: 7400
		public Character.OnGaveStatusDelegate onGaveStatus;

		// Token: 0x04001CEB RID: 7403
		public Character.OnKilledDelegate onKilled;

		// Token: 0x04001CF0 RID: 7408
		public Action<Characters.Actions.Action> onStartCharging;

		// Token: 0x04001CF1 RID: 7409
		public Action<Characters.Actions.Action> onStopCharging;

		// Token: 0x04001CF2 RID: 7410
		public Action<Characters.Actions.Action> onCancelCharging;

		// Token: 0x04001CF3 RID: 7411
		public readonly TrueOnlyLogicalSumList cinematic = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CF4 RID: 7412
		public readonly TrueOnlyLogicalSumList invulnerable = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CF5 RID: 7413
		public readonly TrueOnlyLogicalSumList evasion = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CF6 RID: 7414
		public readonly TrueOnlyLogicalSumList blockLook = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CF7 RID: 7415
		public readonly TrueOnlyLogicalSumList stealth = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CF8 RID: 7416
		public readonly Stat stat;

		// Token: 0x04001CF9 RID: 7417
		public readonly CharacterChronometer chronometer = new CharacterChronometer();

		// Token: 0x04001CFA RID: 7418
		[SerializeField]
		private Key _key;

		// Token: 0x04001CFB RID: 7419
		[GetComponent]
		[SerializeField]
		protected CharacterHealth _health;

		// Token: 0x04001CFC RID: 7420
		[SerializeField]
		[GetComponent]
		protected CharacterHit _hit;

		// Token: 0x04001CFD RID: 7421
		[SerializeField]
		protected BoxCollider2D _collider;

		// Token: 0x04001CFE RID: 7422
		[GetComponent]
		[SerializeField]
		protected Movement _movement;

		// Token: 0x04001CFF RID: 7423
		[SerializeField]
		[GetComponent]
		private CharacterAnimationController _animationController;

		// Token: 0x04001D00 RID: 7424
		[GetComponent]
		[SerializeField]
		private CharacterStatus _status;

		// Token: 0x04001D01 RID: 7425
		[SerializeField]
		private Character.Type _type;

		// Token: 0x04001D02 RID: 7426
		[SerializeField]
		private Character.SizeForEffect _sizeForEffect;

		// Token: 0x04001D03 RID: 7427
		[SerializeField]
		private SortingGroup _sortingGroup;

		// Token: 0x04001D04 RID: 7428
		[SerializeField]
		protected Stat.Values _baseStat = new Stat.Values(new Stat.Value[]
		{
			new Stat.Value(Stat.Category.Constant, Stat.Kind.Health, 0.0),
			new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, 0.0)
		});

		// Token: 0x04001D05 RID: 7429
		[SerializeField]
		protected Transform _base;

		// Token: 0x04001D06 RID: 7430
		private Character.LookingDirection _lookingDirection;

		// Token: 0x04001D07 RID: 7431
		[SerializeField]
		protected Weapon _weapon;

		// Token: 0x04001D08 RID: 7432
		[SerializeField]
		private GameObject _attach;

		// Token: 0x04001D09 RID: 7433
		private CoroutineReference _cWaitForEndOfAction;

		// Token: 0x020006A5 RID: 1701
		public enum Type
		{
			// Token: 0x04001D14 RID: 7444
			TrashMob,
			// Token: 0x04001D15 RID: 7445
			Named,
			// Token: 0x04001D16 RID: 7446
			Adventurer,
			// Token: 0x04001D17 RID: 7447
			Boss,
			// Token: 0x04001D18 RID: 7448
			Summoned,
			// Token: 0x04001D19 RID: 7449
			Trap,
			// Token: 0x04001D1A RID: 7450
			Player,
			// Token: 0x04001D1B RID: 7451
			Dummy,
			// Token: 0x04001D1C RID: 7452
			PlayerMinion
		}

		// Token: 0x020006A6 RID: 1702
		public enum LookingDirection
		{
			// Token: 0x04001D1E RID: 7454
			Right,
			// Token: 0x04001D1F RID: 7455
			Left
		}

		// Token: 0x020006A7 RID: 1703
		public enum SizeForEffect
		{
			// Token: 0x04001D21 RID: 7457
			Small,
			// Token: 0x04001D22 RID: 7458
			Medium,
			// Token: 0x04001D23 RID: 7459
			Large,
			// Token: 0x04001D24 RID: 7460
			ExtraLarge,
			// Token: 0x04001D25 RID: 7461
			None
		}

		// Token: 0x020006A8 RID: 1704
		// (Invoke) Token: 0x06002239 RID: 8761
		public delegate void OnGaveStatusDelegate(Character target, CharacterStatus.ApplyInfo applyInfo, bool result);

		// Token: 0x020006A9 RID: 1705
		// (Invoke) Token: 0x0600223D RID: 8765
		public delegate void OnKilledDelegate(ITarget target, ref Damage damage);

		// Token: 0x020006AA RID: 1706
		// (Invoke) Token: 0x06002241 RID: 8769
		public delegate void OnStartMotionDelegate(Characters.Actions.Motion motion, float runSpeed);
	}
}
