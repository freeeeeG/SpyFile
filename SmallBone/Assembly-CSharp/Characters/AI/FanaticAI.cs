using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010BB RID: 4283
	public sealed class FanaticAI : AIController
	{
		// Token: 0x06005310 RID: 21264 RVA: 0x000F91EC File Offset: 0x000F73EC
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._checkWithinSight,
				this._wander,
				this._chase,
				this._attack,
				this._sacrifice
			};
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x000F9240 File Offset: 0x000F7440
		private void OnDestroy()
		{
			this._characterAnimation = null;
			this._idleClipAfterWander = null;
			this._walkClipAfterWander = null;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x000F9257 File Offset: 0x000F7457
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x000F927F File Offset: 0x000F747F
		protected override IEnumerator CProcess()
		{
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x06005314 RID: 21268 RVA: 0x000F928E File Offset: 0x000F748E
		private IEnumerator CCombat()
		{
			yield return this._wander.CRun(this);
			this._characterAnimation.SetIdle(this._idleClipAfterWander);
			this._characterAnimation.SetWalk(this._walkClipAfterWander);
			while (!base.dead)
			{
				if (this._sacrifice.result.Equals(Characters.AI.Behaviours.Behaviour.Result.Doing))
				{
					yield return null;
				}
				else if (base.FindClosestPlayerBody(this._attackTrigger) != null)
				{
					yield return this._attack.CRun(this);
				}
				else
				{
					yield return this._chase.CRun(this);
					if (this._chase.result == Characters.AI.Behaviours.Behaviour.Result.Success)
					{
						yield return this._attack.CRun(this);
					}
				}
			}
			yield break;
		}

		// Token: 0x040042AF RID: 17071
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		[Header("Behaviours")]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x040042B0 RID: 17072
		[Subcomponent(typeof(Wander))]
		[SerializeField]
		private Wander _wander;

		// Token: 0x040042B1 RID: 17073
		[SerializeField]
		[Subcomponent(typeof(Chase))]
		private Chase _chase;

		// Token: 0x040042B2 RID: 17074
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		private Attack _attack;

		// Token: 0x040042B3 RID: 17075
		[Subcomponent(typeof(Sacrifice))]
		[SerializeField]
		private Sacrifice _sacrifice;

		// Token: 0x040042B4 RID: 17076
		[Header("Tools")]
		[Space]
		[SerializeField]
		private Collider2D _attackTrigger;

		// Token: 0x040042B5 RID: 17077
		[SerializeField]
		private CharacterAnimation _characterAnimation;

		// Token: 0x040042B6 RID: 17078
		[SerializeField]
		private AnimationClip _idleClipAfterWander;

		// Token: 0x040042B7 RID: 17079
		[SerializeField]
		private AnimationClip _walkClipAfterWander;
	}
}
