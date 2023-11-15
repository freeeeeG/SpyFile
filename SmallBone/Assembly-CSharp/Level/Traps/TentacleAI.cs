using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using Characters.Actions;
using Characters.AI;
using Characters.AI.Behaviours;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x02000647 RID: 1607
	public class TentacleAI : AIController
	{
		// Token: 0x0600204A RID: 8266 RVA: 0x00061DA8 File Offset: 0x0005FFA8
		private void Awake()
		{
			this._attackAction.Initialize(this.character);
			this._appearance.Initialize(this.character);
			this._abilityAttacher.Initialize(this.character);
			this._abilityAttacher.StartAttach();
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x00061DE8 File Offset: 0x0005FFE8
		private new void OnEnable()
		{
			base.OnEnable();
			this._elapsedTime = 0f;
			base.transform.parent = Map.Instance.transform;
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00061E3C File Offset: 0x0006003C
		public void Appear(Transform point, Sprite corpse, bool flip)
		{
			this._corpseRenderer.sprite = corpse;
			if (flip)
			{
				this._corpseRenderer.flipX = true;
			}
			this.character.health.onDied += delegate()
			{
				this._corpseRenderer.transform.SetParent(Map.Instance.transform);
				this._corpseRenderer.sortingOrder = this.character.sortingGroup.sortingOrder;
			};
			base.transform.position = point.position;
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000075E7 File Offset: 0x000057E7
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x00061EA0 File Offset: 0x000600A0
		private void FindPlayer()
		{
			this._attackTrigger.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(512);
			this._overlapper.OverlapCollider(this._attackTrigger);
			this._attackTrigger.enabled = false;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x00061EF1 File Offset: 0x000600F1
		private void OnDestroy()
		{
			this._abilityAttacher.StopAttach();
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00061EFE File Offset: 0x000600FE
		protected override IEnumerator CProcess()
		{
			yield return null;
			this._appearance.TryStart();
			while (this._appearance.running)
			{
				yield return null;
			}
			while (!base.dead)
			{
				if (base.target == null)
				{
					yield return null;
				}
				else
				{
					do
					{
						yield return null;
						this.FindPlayer();
					}
					while (this._overlapper.results.Count == 0);
					this._attackAction.TryStart();
					while (this._attackAction.running)
					{
						yield return null;
					}
					yield return this._idle.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x04001B62 RID: 7010
		[SerializeField]
		private SpriteRenderer _corpseRenderer;

		// Token: 0x04001B63 RID: 7011
		[SerializeField]
		private CharacterAnimation _animation;

		// Token: 0x04001B64 RID: 7012
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04001B65 RID: 7013
		[Space]
		[Header("Appearance")]
		[SerializeField]
		private Characters.Actions.Action _appearance;

		// Token: 0x04001B66 RID: 7014
		[SerializeField]
		[Header("Attack")]
		[Space]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04001B67 RID: 7015
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x04001B68 RID: 7016
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idle;

		// Token: 0x04001B69 RID: 7017
		[Space]
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher _abilityAttacher;

		// Token: 0x04001B6A RID: 7018
		private float _elapsedTime;

		// Token: 0x04001B6B RID: 7019
		private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);
	}
}
