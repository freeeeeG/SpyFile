using System;
using System.Collections;
using Characters.Abilities;
using Hardmode;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011E6 RID: 4582
	public sealed class DarkCrystal : MonoBehaviour
	{
		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x060059ED RID: 23021 RVA: 0x0010B587 File Offset: 0x00109787
		private Character owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x0010B58F File Offset: 0x0010978F
		private void Start()
		{
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				UnityEngine.Object.Destroy(this);
			}
			this._doubleBarrierAbility.Initialize();
			this._singleBarrierAbility.Initialize();
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x0010B5B9 File Offset: 0x001097B9
		public void StartProcess()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x0010B5C8 File Offset: 0x001097C8
		public void AttachDoubleAbility()
		{
			this._owner.ability.Add(this._doubleBarrierAbility);
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x0010B5E1 File Offset: 0x001097E1
		private IEnumerator CProcess()
		{
			if (Singleton<Service>.Instance.levelManager.player == null)
			{
				yield break;
			}
			while (Scene<GameBase>.instance.uiManager.letterBox.visible)
			{
				yield return null;
			}
			if (this._host)
			{
				yield return this.StartAttachDoubleAbility();
			}
			while (!this._other.owner.health.dead)
			{
				yield return null;
			}
			yield return this.StartAttachSingleAbility();
			yield break;
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x0010B5F0 File Offset: 0x001097F0
		private IEnumerator StartAttachDoubleAbility()
		{
			float elapsed = this._doubleAbilityInterval;
			while (!this._other.owner.health.dead)
			{
				elapsed += Chronometer.global.deltaTime;
				if (elapsed >= this._doubleAbilityInterval)
				{
					elapsed -= this._doubleAbilityInterval;
					if (MMMaths.RandomBool())
					{
						this.AttachDoubleAbility();
					}
					else
					{
						this._other.AttachDoubleAbility();
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x0010B5FF File Offset: 0x001097FF
		private IEnumerator StartAttachSingleAbility()
		{
			float elapsed = 0f;
			while (!this.owner.health.dead)
			{
				elapsed += Chronometer.global.deltaTime;
				if (elapsed >= this._singleAbilityInterval)
				{
					elapsed -= this._singleAbilityInterval;
					this._owner.ability.Add(this._singleBarrierAbility);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x040048A6 RID: 18598
		[Header("하드모드")]
		[SerializeField]
		private Character _owner;

		// Token: 0x040048A7 RID: 18599
		[SerializeField]
		private Animator _animator;

		// Token: 0x040048A8 RID: 18600
		[SerializeField]
		private DarkCrystal _other;

		// Token: 0x040048A9 RID: 18601
		[SerializeField]
		private bool _host;

		// Token: 0x040048AA RID: 18602
		[SerializeField]
		private float _doubleAbilityInterval;

		// Token: 0x040048AB RID: 18603
		[SerializeField]
		private float _singleAbilityInterval;

		// Token: 0x040048AC RID: 18604
		[SerializeField]
		private DarkCrystal.DarkCrystalBarrier _doubleBarrierAbility;

		// Token: 0x040048AD RID: 18605
		[SerializeField]
		private DarkCrystal.DarkCrystalBarrier _singleBarrierAbility;

		// Token: 0x020011E7 RID: 4583
		[Serializable]
		private class DarkCrystalBarrier : Ability
		{
			// Token: 0x060059F6 RID: 23030 RVA: 0x0010B60E File Offset: 0x0010980E
			public override IAbilityInstance CreateInstance(Character owner)
			{
				return new DarkCrystal.DarkCrystalBarrier.Instance(owner, this);
			}

			// Token: 0x040048AE RID: 18606
			private static readonly int _startHash = Animator.StringToHash("Start");

			// Token: 0x040048AF RID: 18607
			[SerializeField]
			private SpriteRenderer _frontRenderer;

			// Token: 0x040048B0 RID: 18608
			[SerializeField]
			private SpriteRenderer _behindRenderer;

			// Token: 0x040048B1 RID: 18609
			[SerializeField]
			private Animator _front;

			// Token: 0x040048B2 RID: 18610
			[SerializeField]
			private Animator _behind;

			// Token: 0x020011E8 RID: 4584
			public class Instance : AbilityInstance<DarkCrystal.DarkCrystalBarrier>
			{
				// Token: 0x060059F9 RID: 23033 RVA: 0x0010B628 File Offset: 0x00109828
				public Instance(Character owner, DarkCrystal.DarkCrystalBarrier ability) : base(owner, ability)
				{
				}

				// Token: 0x060059FA RID: 23034 RVA: 0x0010B634 File Offset: 0x00109834
				protected override void OnAttach()
				{
					this.owner.invulnerable.Attach(this);
					this.ability._front.gameObject.SetActive(true);
					this.ability._behind.gameObject.SetActive(true);
					Color color = this.ability._frontRenderer.color;
					color.a = 1f;
					this.ability._frontRenderer.color = color;
					color = this.ability._behindRenderer.color;
					color.a = 1f;
					this.ability._behindRenderer.color = color;
					this.ability._front.Play(DarkCrystal.DarkCrystalBarrier._startHash);
					this.ability._behind.Play(DarkCrystal.DarkCrystalBarrier._startHash);
				}

				// Token: 0x060059FB RID: 23035 RVA: 0x0010B704 File Offset: 0x00109904
				protected override void OnDetach()
				{
					this.owner.invulnerable.Detach(this);
					this.owner.StartCoroutine(this.CFadeOut());
				}

				// Token: 0x060059FC RID: 23036 RVA: 0x0010B72A File Offset: 0x0010992A
				private IEnumerator CFadeOut()
				{
					float elapsed = 0f;
					int duration = 1;
					while (elapsed < (float)duration)
					{
						yield return null;
						elapsed += this.owner.chronometer.master.deltaTime;
						Color color = this.ability._frontRenderer.color;
						color.a = Mathf.Lerp(1f, 0f, elapsed / (float)duration);
						this.ability._frontRenderer.color = color;
						color = this.ability._behindRenderer.color;
						color.a = Mathf.Lerp(1f, 0f, elapsed / (float)duration);
						this.ability._behindRenderer.color = color;
					}
					this.ability._front.gameObject.SetActive(false);
					this.ability._behind.gameObject.SetActive(false);
					yield break;
				}
			}
		}
	}
}
