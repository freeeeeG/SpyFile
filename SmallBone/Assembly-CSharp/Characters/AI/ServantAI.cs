using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.Movements;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001084 RID: 4228
	public sealed class ServantAI : AIController
	{
		// Token: 0x060051CD RID: 20941 RVA: 0x000F5A95 File Offset: 0x000F3C95
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._confusing
			};
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x000F0D27 File Offset: 0x000EEF27
		protected override void OnEnable()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x000F5AAE File Offset: 0x000F3CAE
		protected override IEnumerator CProcess()
		{
			this._confusingState = true;
			yield return base.CPlayStartOption();
			base.StartCoroutine(this.CConfusing());
			base.StartCoroutine(this.CCombat());
			base.StartCoroutine(this.CObserveHit());
			yield break;
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x000F5ABD File Offset: 0x000F3CBD
		private IEnumerator CConfusing()
		{
			while (!base.dead)
			{
				yield return null;
				if (this._confusingState && this.character.movement.isGrounded)
				{
					yield return this._confusing.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x000F5ACC File Offset: 0x000F3CCC
		private IEnumerator CCombat()
		{
			this._screaming.Play();
			while (!base.dead)
			{
				yield return null;
				if (this._crashable && !this.character.stunedOrFreezed)
				{
					Character character = base.FindClosestPlayerBody(this._clashCollider);
					if (!(character == null) && !character.cinematic.value)
					{
						if (this.character.transform.position.x > character.transform.position.x)
						{
							character.movement.push.ApplyKnockback(this.character, this._rightPushInfo);
							this.character.movement.push.ApplyKnockback(character, this._leftPushInfo);
							character.lookingDirection = Character.LookingDirection.Left;
						}
						else
						{
							character.movement.push.ApplyKnockback(this.character, this._leftPushInfo);
							this.character.movement.push.ApplyKnockback(character, this._rightPushInfo);
							character.lookingDirection = Character.LookingDirection.Right;
						}
						CharacterStatus status = character.status;
						if (status != null)
						{
							status.Apply(this.character, this._status);
						}
						this._confusing.StopPropagation();
						this._screaming.Stop();
						this._confusingState = false;
						this._clash.TryStart();
						while (this._clash.running)
						{
							yield return null;
						}
						this._confusingState = true;
						this._screaming.Play();
					}
				}
			}
			yield break;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x000F5ADB File Offset: 0x000F3CDB
		private IEnumerator CObserveHit()
		{
			while (!base.dead)
			{
				yield return null;
				if (this.character.hit.action.running)
				{
					this._crashable = false;
				}
				else
				{
					this._crashable = true;
				}
			}
			yield break;
		}

		// Token: 0x040041B0 RID: 16816
		[SerializeField]
		[Subcomponent(typeof(Confusing))]
		private Confusing _confusing;

		// Token: 0x040041B1 RID: 16817
		[SerializeField]
		private Characters.Actions.Action _clash;

		// Token: 0x040041B2 RID: 16818
		[SerializeField]
		private BoxCollider2D _clashCollider;

		// Token: 0x040041B3 RID: 16819
		[SerializeField]
		private CharacterStatus.ApplyInfo _status;

		// Token: 0x040041B4 RID: 16820
		[Range(1f, 4f)]
		[SerializeField]
		private int _grade = 1;

		// Token: 0x040041B5 RID: 16821
		[SerializeField]
		private PushInfo _rightPushInfo;

		// Token: 0x040041B6 RID: 16822
		[SerializeField]
		private PushInfo _leftPushInfo;

		// Token: 0x040041B7 RID: 16823
		[Subcomponent(typeof(RepeatPlaySound))]
		[SerializeField]
		private RepeatPlaySound _screaming;

		// Token: 0x040041B8 RID: 16824
		private bool _confusingState;

		// Token: 0x040041B9 RID: 16825
		private bool _crashable = true;
	}
}
