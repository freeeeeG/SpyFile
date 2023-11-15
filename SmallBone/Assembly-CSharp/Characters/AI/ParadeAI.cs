using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Movements;
using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001129 RID: 4393
	public sealed class ParadeAI : AIController
	{
		// Token: 0x06005566 RID: 21862 RVA: 0x000FECA4 File Offset: 0x000FCEA4
		private void Awake()
		{
			base.behaviours = new List<Characters.AI.Behaviours.Behaviour>
			{
				this._moveToTop,
				this._moveToTargetGround
			};
			this._onSpawn.Initialize();
			this._onAttack.Initialize();
			this._onGround.Initialize();
			this._onJump.Initialize();
			this._originGravity = this.character.movement.config.gravity;
			this._moveAmount = UnityEngine.Random.Range(this._moveAmountRange.x, this._moveAmountRange.y);
			this.LookToCenter();
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x000FED44 File Offset: 0x000FCF44
		private void LookToCenter()
		{
			this.character.lookingDirection = ((this.character.transform.position.x > Map.Instance.bounds.center.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x000FCBBB File Offset: 0x000FADBB
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x06005569 RID: 21865 RVA: 0x000FED8E File Offset: 0x000FCF8E
		protected override IEnumerator CProcess()
		{
			this._onSpawn.gameObject.SetActive(true);
			this._onSpawn.Run(this.character);
			yield return base.CPlayStartOption();
			yield return this.CCombat();
			yield break;
		}

		// Token: 0x0600556A RID: 21866 RVA: 0x000FEDA0 File Offset: 0x000FCFA0
		private void SetJumpableState()
		{
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.config.gravity = 0f;
			this.character.movement.controller.terrainMask = 0;
			this.character.movement.controller.oneWayPlatformMask = 0;
		}

		// Token: 0x0600556B RID: 21867 RVA: 0x000FEE14 File Offset: 0x000FD014
		private void SetAttackableState()
		{
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.config.gravity = this._originGravity;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			this.character.movement.controller.oneWayPlatformMask = 131072;
		}

		// Token: 0x0600556C RID: 21868 RVA: 0x000FEE8B File Offset: 0x000FD08B
		private IEnumerator CCombat()
		{
			this.character.movement.onGrounded += delegate()
			{
				this.SetAttackableState();
				this._onGround.gameObject.SetActive(true);
				this._onGround.Run(this.character);
			};
			this.SetJumpableState();
			this.SetDestination(0.5f);
			yield return this.CAttack();
			this.SetAttackableState();
			yield return this._idle.CRun(this);
			while (!base.dead)
			{
				this.SetAttackableState();
				yield return null;
				if (this.CheckExitTimingAndSetDestination())
				{
					yield return this.CDisappear();
				}
				else
				{
					yield return this.CAttack();
					this.SetAttackableState();
					yield return this._idle.CRun(this);
				}
			}
			yield break;
		}

		// Token: 0x0600556D RID: 21869 RVA: 0x000FEE9C File Offset: 0x000FD09C
		private bool CheckExitTimingAndSetDestination()
		{
			int num = (this.character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			float num2 = this.character.transform.position.x + (float)num * this._moveAmount;
			Bounds bounds = Map.Instance.bounds;
			float y = this.character.transform.position.y + this._attackHeight;
			base.destination = new Vector2(num2, y);
			return num2 >= bounds.max.x || num2 <= bounds.min.x;
		}

		// Token: 0x0600556E RID: 21870 RVA: 0x000FEF34 File Offset: 0x000FD134
		private void SetDestination(float moveAmount)
		{
			int num = (this.character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			float x = this.character.transform.position.x + (float)num * moveAmount;
			float y = this.character.transform.position.y + this._attackHeight;
			base.destination = new Vector2(x, y);
		}

		// Token: 0x0600556F RID: 21871 RVA: 0x000FEF98 File Offset: 0x000FD198
		private IEnumerator CAttack()
		{
			this.SetJumpableState();
			this._onJump.gameObject.SetActive(true);
			this._onJump.Run(this.character);
			yield return this._moveToTop.CRun(this);
			yield return this._attackReady.CRun(this);
			RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector2.down, this._attackHeight * 2f, Layers.terrainMask);
			if (!hit)
			{
				Debug.LogError("Parade's y position was wrong");
				yield break;
			}
			float x = base.transform.position.x;
			float y = hit.point.y;
			base.destination = new Vector2(x, y);
			this._onAttack.gameObject.SetActive(true);
			this._onAttack.Run(this.character);
			yield break;
		}

		// Token: 0x06005570 RID: 21872 RVA: 0x000FEFA7 File Offset: 0x000FD1A7
		private IEnumerator CAppear()
		{
			int num = (this.character.lookingDirection == Character.LookingDirection.Right) ? 1 : -1;
			float x = this.character.transform.position.x + (float)num;
			base.destination = new Vector2(x, this.character.transform.position.y);
			yield return this._appearance.CRun(this);
			yield return this._idleAfterAppearance.CRun(this);
			yield break;
		}

		// Token: 0x06005571 RID: 21873 RVA: 0x000FEFB6 File Offset: 0x000FD1B6
		private IEnumerator CDisappear()
		{
			this.character.health.Kill();
			yield break;
			IL_30:
			this._onJump.gameObject.SetActive(true);
			this._onJump.Run(this.character);
			yield return null;
			float num;
			num += this.character.chronometer.master.deltaTime;
			Vector3 v;
			float num2;
			this.character.transform.position = Vector2.Lerp(v, base.destination, num / num2);
			if (num < num2)
			{
				goto IL_30;
			}
			UnityEngine.Object.Destroy(this.character.gameObject);
			yield break;
		}

		// Token: 0x04004468 RID: 17512
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		[Header("Appearance")]
		[Space]
		[Header("Behaviours")]
		private MoveToDestinationWithFly _appearance;

		// Token: 0x04004469 RID: 17513
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _idleAfterAppearance;

		// Token: 0x0400446A RID: 17514
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		[Header("Move and Attack")]
		[SerializeField]
		[Space]
		private MoveToDestinationWithFly _moveToTop;

		// Token: 0x0400446B RID: 17515
		[Subcomponent(typeof(Idle))]
		[SerializeField]
		private Idle _attackReady;

		// Token: 0x0400446C RID: 17516
		[Subcomponent(typeof(MoveToDestinationWithFly))]
		[SerializeField]
		private MoveToDestinationWithFly _moveToTargetGround;

		// Token: 0x0400446D RID: 17517
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		private Idle _idle;

		// Token: 0x0400446E RID: 17518
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSpawn;

		// Token: 0x0400446F RID: 17519
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onGround;

		// Token: 0x04004470 RID: 17520
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onAttack;

		// Token: 0x04004471 RID: 17521
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onJump;

		// Token: 0x04004472 RID: 17522
		[Space]
		[SerializeField]
		[Header("Tools")]
		private float _attackHeight;

		// Token: 0x04004473 RID: 17523
		[SerializeField]
		[MinMaxSlider(1f, 20f)]
		private Vector2 _moveAmountRange;

		// Token: 0x04004474 RID: 17524
		private float _moveAmount;

		// Token: 0x04004475 RID: 17525
		private float _originGravity;
	}
}
