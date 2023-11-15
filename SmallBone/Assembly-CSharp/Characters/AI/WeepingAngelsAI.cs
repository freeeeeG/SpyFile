using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010DF RID: 4319
	public sealed class WeepingAngelsAI : AIController
	{
		// Token: 0x060053E8 RID: 21480 RVA: 0x000FB758 File Offset: 0x000F9958
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._hideAndSeek,
				this._attack
			};
			if (Singleton<Service>.Instance.levelManager.currentChapter.stageIndex == 0)
			{
				this._stage1.Apply(this._characterAnimation, this._renderer);
				this._activate = this._stage1.activate;
				return;
			}
			this._stage2.Apply(this._characterAnimation, this._renderer);
			this._activate = this._stage2.activate;
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x000FB7FB File Offset: 0x000F99FB
		private void OnDestroy()
		{
			this._stage1.Dispose();
			this._stage2.Dispose();
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x000FB813 File Offset: 0x000F9A13
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x000FB83B File Offset: 0x000F9A3B
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x000FB84A File Offset: 0x000F9A4A
		private IEnumerator CCombat()
		{
			while (base.target == null)
			{
				yield return null;
			}
			yield return this._activate.CRun(this);
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else if (base.stuned)
				{
					yield return null;
				}
				else
				{
					yield return this._hideAndSeek.CRun(this);
					if (this._hideAndSeek.result == Characters.AI.Behaviours.Behaviour.Result.Success)
					{
						yield return this._attack.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x04004365 RID: 17253
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004366 RID: 17254
		[SerializeField]
		[Subcomponent(typeof(HideAndSeek))]
		private HideAndSeek _hideAndSeek;

		// Token: 0x04004367 RID: 17255
		[SerializeField]
		[Attack.SubcomponentAttribute(true)]
		private Attack _attack;

		// Token: 0x04004368 RID: 17256
		[SerializeField]
		private WeepingAngelsAI.Setting _stage1;

		// Token: 0x04004369 RID: 17257
		[SerializeField]
		private WeepingAngelsAI.Setting _stage2;

		// Token: 0x0400436A RID: 17258
		[Header("Tools")]
		[Space]
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x0400436B RID: 17259
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x0400436C RID: 17260
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x0400436D RID: 17261
		private RunAction _activate;

		// Token: 0x020010E0 RID: 4320
		[Serializable]
		private class Setting
		{
			// Token: 0x170010A7 RID: 4263
			// (get) Token: 0x060053EE RID: 21486 RVA: 0x000FB859 File Offset: 0x000F9A59
			internal RunAction activate
			{
				get
				{
					return this._activate;
				}
			}

			// Token: 0x060053EF RID: 21487 RVA: 0x000FB861 File Offset: 0x000F9A61
			public void Apply(CharacterAnimation animation, SpriteRenderer renderer)
			{
				renderer.sprite = this._firstFrame;
				animation.SetIdle(this._idle);
			}

			// Token: 0x060053F0 RID: 21488 RVA: 0x000FB87B File Offset: 0x000F9A7B
			public void Dispose()
			{
				this._firstFrame = null;
				this._idle = null;
				this._activate = null;
			}

			// Token: 0x0400436E RID: 17262
			[SerializeField]
			private Sprite _firstFrame;

			// Token: 0x0400436F RID: 17263
			[SerializeField]
			private AnimationClip _idle;

			// Token: 0x04004370 RID: 17264
			[SerializeField]
			private RunAction _activate;
		}
	}
}
