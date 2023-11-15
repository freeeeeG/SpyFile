using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x0200110B RID: 4363
	public sealed class BrothersAI : AIController
	{
		// Token: 0x060054DD RID: 21725 RVA: 0x000FDACD File Offset: 0x000FBCCD
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._attack
			};
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x000FDAF2 File Offset: 0x000FBCF2
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x000FDB1A File Offset: 0x000FBD1A
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			base.StartCoroutine(this.COutro());
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x000FDB29 File Offset: 0x000FBD29
		private IEnumerator CCombat()
		{
			yield return this._intro.CRun(this);
			if (this._idleOnStart)
			{
				yield return this._idle.CRun(this);
			}
			while (!base.dead)
			{
				if (this._readyForOutro)
				{
					yield return this._outro.CRun(this);
					UnityEngine.Object.Destroy(this.character.gameObject);
				}
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
					yield return this._attack.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x000FDB38 File Offset: 0x000FBD38
		private IEnumerator COutro()
		{
			yield return this.character.chronometer.master.WaitForSeconds(this._lifeTime);
			this._readyForOutro = true;
			yield break;
		}

		// Token: 0x04004411 RID: 17425
		[SerializeField]
		private float _lifeTime = 30f;

		// Token: 0x04004412 RID: 17426
		[SerializeField]
		private bool _idleOnStart;

		// Token: 0x04004413 RID: 17427
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004414 RID: 17428
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _intro;

		// Token: 0x04004415 RID: 17429
		[Subcomponent(typeof(ActionAttack))]
		[SerializeField]
		private ActionAttack _outro;

		// Token: 0x04004416 RID: 17430
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private Attack _attack;

		// Token: 0x04004417 RID: 17431
		[SerializeField]
		[Characters.AI.Behaviours.Behaviour.SubcomponentAttribute(true)]
		private Idle _idle;

		// Token: 0x04004418 RID: 17432
		private bool _readyForOutro;
	}
}
