using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Abilities;
using Characters.Operations;
using FX;
using Hardmode;
using Level.Waves;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level.Objects
{
	// Token: 0x02000571 RID: 1393
	public class DivineShield : MonoBehaviour
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x00054E60 File Offset: 0x00053060
		private void Start()
		{
			switch (Singleton<HardmodeManager>.Instance.GetEnemyStep())
			{
			case HardmodeManager.EnemyStep.Normal:
				this._targetPin = this._normalTargetPin;
				break;
			case HardmodeManager.EnemyStep.A:
				this._targetPin = this._atargetPin;
				break;
			case HardmodeManager.EnemyStep.B:
				this._targetPin = this._btargetPin;
				break;
			case HardmodeManager.EnemyStep.C:
				this._targetPin = this._ctargetPin;
				break;
			}
			if (this._targetPin == null)
			{
				return;
			}
			base.StartCoroutine(this.CTryAttach());
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x00054EE2 File Offset: 0x000530E2
		private IEnumerator CTryAttach()
		{
			yield return this.CWaitForSpawend();
			ICollection<Character> characters = this._targetPin.characters;
			if (characters.Count >= 2)
			{
				Debug.LogError("대상이 2명 이상입니다");
				yield break;
			}
			this._target = characters.Random<Character>();
			this.Initialize();
			this.AttachShield();
			yield break;
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x00054EF1 File Offset: 0x000530F1
		private IEnumerator CWaitForSpawend()
		{
			while (!this._targetPin.spawned)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x00054F00 File Offset: 0x00053100
		private void Initialize()
		{
			this._divineShieldEffect.Activate(this._target);
			this._smallShield.Initialize();
			this._hitOperation.Initialize();
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x00054F29 File Offset: 0x00053129
		private void AttachShield()
		{
			base.StartCoroutine(this.CAttach());
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x00054F38 File Offset: 0x00053138
		private IEnumerator CAttach()
		{
			while (!this._target.gameObject.activeSelf)
			{
				yield return null;
			}
			this._target.ability.Add(this._smallShield.ability);
			switch (this._target.sizeForEffect)
			{
			case Character.SizeForEffect.Small:
			case Character.SizeForEffect.None:
				this._spawnedLoopEffect = this._smallEffect.Spawn(this._target.transform.position, this._target, 0f, 1f);
				break;
			case Character.SizeForEffect.Medium:
				this._spawnedLoopEffect = this._medium.Spawn(this._target.transform.position, this._target, 0f, 1f);
				break;
			case Character.SizeForEffect.Large:
				this._spawnedLoopEffect = this._bigEffect.Spawn(this._target.transform.position, this._target, 0f, 1f);
				break;
			case Character.SizeForEffect.ExtraLarge:
				this._spawnedLoopEffect = this._extraBigEffect.Spawn(this._target.transform.position, this._target, 0f, 1f);
				break;
			default:
				this._spawnedLoopEffect = this._smallEffect.Spawn(this._target.transform.position, this._target, 0f, 1f);
				break;
			}
			this._target.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.OnTakeDamage));
			this._prop.onDestroy += this.DetachShield;
			this._target.onDie += this.InstantDestroy;
			yield break;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x00054F48 File Offset: 0x00053148
		private void DetachShield()
		{
			if (this._target == null || !this._target.liveAndActive)
			{
				return;
			}
			if (this._spawnedLoopEffect != null)
			{
				this._spawnedLoopEffect.Stop();
				this._spawnedLoopEffect = null;
			}
			this._target.ability.Remove(this._smallShield.ability);
			this._target.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x00054FCF File Offset: 0x000531CF
		private bool OnTakeDamage(ref Damage damage)
		{
			this._hitOperation.gameObject.SetActive(true);
			this._hitOperation.Run(this._target);
			return true;
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x00054FF4 File Offset: 0x000531F4
		private void InstantDestroy()
		{
			if (this._prop.destroyed)
			{
				return;
			}
			Character player = Singleton<Service>.Instance.levelManager.player;
			Damage damage = new Damage(player, 10000.0, Vector2.zero, Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Basic, 1.0, 0f, 0.0, 1.0, 1.0, true, false, 0.0, 0.0, 0, null, 1.0);
			this._prop.Hit(player, ref damage);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x00055093 File Offset: 0x00053293
		private void OnDestroy()
		{
			if (this._spawnedLoopEffect != null)
			{
				this._spawnedLoopEffect.Stop();
				this._spawnedLoopEffect = null;
			}
		}

		// Token: 0x04001783 RID: 6019
		[SerializeField]
		private Pin _normalTargetPin;

		// Token: 0x04001784 RID: 6020
		[SerializeField]
		private Pin _atargetPin;

		// Token: 0x04001785 RID: 6021
		[SerializeField]
		private Pin _btargetPin;

		// Token: 0x04001786 RID: 6022
		[SerializeField]
		private Pin _ctargetPin;

		// Token: 0x04001787 RID: 6023
		[SerializeField]
		private Prop _prop;

		// Token: 0x04001788 RID: 6024
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _smallShield;

		// Token: 0x04001789 RID: 6025
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _hitOperation;

		// Token: 0x0400178A RID: 6026
		[SerializeField]
		private DivineShieldEffect _divineShieldEffect;

		// Token: 0x0400178B RID: 6027
		[SerializeField]
		private EffectInfo _smallEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400178C RID: 6028
		[SerializeField]
		private EffectInfo _medium = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400178D RID: 6029
		[SerializeField]
		private EffectInfo _bigEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400178E RID: 6030
		[SerializeField]
		private EffectInfo _extraBigEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x0400178F RID: 6031
		private Pin _targetPin;

		// Token: 0x04001790 RID: 6032
		private Character _target;

		// Token: 0x04001791 RID: 6033
		private EffectPoolInstance _spawnedLoopEffect;
	}
}
