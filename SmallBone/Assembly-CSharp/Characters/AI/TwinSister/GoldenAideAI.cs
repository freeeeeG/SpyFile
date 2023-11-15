using System;
using System.Collections;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Movements;
using Characters.Operations;
using Data;
using Hardmode;
using Level;
using PhysicsUtils;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200117C RID: 4476
	public class GoldenAideAI : AIController
	{
		// Token: 0x060057A6 RID: 22438 RVA: 0x00104818 File Offset: 0x00102A18
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x00104834 File Offset: 0x00102A34
		private new void Start()
		{
			base.Start();
			this._collisionState = this.character.movement.controller.collisionState;
			this._platformWidth = this._collisionState.lastStandingCollider.bounds.size.x;
			GoldenAideAI._nonAllocOverlapper = new NonAllocOverlapper(15);
			GoldenAideAI._nonAllocOverlapper.contactFilter.SetLayerMask(Layers.groundMask);
			this._risingPieceAttackOperations.Initialize();
			this._risingPieceStartAttackOperations.Initialize();
			this.character.health.onDiedTryCatch += delegate()
			{
				this.character.status.RemoveAllStatus();
				this._body.rotation = Quaternion.identity;
			};
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				return;
			}
			this._durationOfMeteorInGround2 = this._durationOfMeteorInGround2OnHardmode;
			this._durationOfGoldmaneMeteor = this._durationOfGoldmaneMeteorOnHardmode;
			this._durationOfMeteorInAir = this._durationOfMeteorInAirOnHardmode;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x00104908 File Offset: 0x00102B08
		protected override IEnumerator CProcess()
		{
			yield return this.CIntro();
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			while (!base.dead)
			{
				yield return null;
				base.target == null;
			}
			yield break;
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x00104917 File Offset: 0x00102B17
		public IEnumerator CIntro()
		{
			this.character.transform.position = new Vector2(this._landingPoint.position.x, this._landingPoint.position.y + this._introFallHeight);
			this._introLanding.TryStart();
			while (this._introLanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x00104926 File Offset: 0x00102B26
		public IEnumerator CastAwakening()
		{
			this.character.status.RemoveAllStatus();
			yield return Chronometer.global.WaitForSeconds(1.5f);
			yield return this.CastAwakeningAppear();
			this._awakening.TryStart();
			while (this._awakening.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x00104935 File Offset: 0x00102B35
		private IEnumerator CastAwakeningAppear()
		{
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			float x = (this._awakeningPosition.position.x < bounds.center.x) ? (bounds.min.x - 4f) : (bounds.max.x + 4f);
			Vector2 v = new Vector2(x, bounds.max.y + 5f);
			Vector2 v2 = new Vector2(this._awakeningPosition.position.x, bounds.max.y);
			this.character.transform.position = v;
			this.character.lookingDirection = ((this._awakeningPosition.position.x < bounds.center.x) ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			this._twinAppear.TryStart();
			yield return this.MoveToDestination(v, v2, this._twinAppear, this._durationOfawakeningAppear, false, false);
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			yield break;
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x00104944 File Offset: 0x00102B44
		public IEnumerator CastTwinMeteorGround(bool left)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this._leftOfTwinMeteor = left;
			yield return this.CastTwinAppear();
			if (this.character.health.dead)
			{
				yield break;
			}
			yield return this.CastMeteorInGround2();
			if (this.character.health.dead)
			{
				yield break;
			}
			yield return this.EscapeForTwin(true);
			yield break;
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x0010495A File Offset: 0x00102B5A
		public IEnumerator CastTwinMeteorChain(bool left, bool ground)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this._leftOfTwinMeteor = left;
			if (ground)
			{
				yield return this.CastTwinAppear();
				yield return this.CastMeteorInGround2();
			}
			else
			{
				yield return this.CastGoldenMeteor();
			}
			yield return this.EscapeForTwin(true);
			yield break;
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x00104977 File Offset: 0x00102B77
		public IEnumerator CastTwinMeteorPierce(bool left)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this._leftOfTwinMeteor = left;
			this._rangeAttackHoming.lookTarget = Map.Instance.bounds.center;
			yield return this.CastTwinAppear();
			yield return this.CastRangeAttackHoming(true);
			yield return this.EscapeForTwin(false);
			yield break;
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x0010498D File Offset: 0x00102B8D
		public IEnumerator CastTwinMeteor(bool left)
		{
			if (base.dead)
			{
				yield break;
			}
			this._leftOfTwinMeteor = left;
			this.character.lookingDirection = (this._leftOfTwinMeteor ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			Bounds platform = this.character.movement.controller.collisionState.lastStandingCollider.bounds;
			float num = UnityEngine.Random.Range(this._minHeightOfTwinMeteor, this._maxHeightOfTwinMeteor);
			this.character.transform.position = new Vector2(this._leftOfTwinMeteor ? (platform.min.x - 4f) : (platform.max.x + 4f), platform.max.y + num);
			Vector2 source = this.character.transform.position;
			Vector2 dest = new Vector2(base.target.transform.position.x, platform.max.y);
			this._twinMeteorDestination.position = dest;
			this._twinMeteorPreparing.TryStart();
			while (this._twinMeteorPreparing.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._twinMeteor.TryStart();
			yield return this.MoveToDestination(source, dest, this._twinMeteor, this._durationOfTwinMeteor, true, false);
			if (this.character.lookingDirection == Character.LookingDirection.Right)
			{
				this.character.transform.position = new Vector2(Mathf.Max(platform.min.x, dest.x - 1.5f), platform.max.y);
			}
			else
			{
				this.character.transform.position = new Vector2(Mathf.Min(platform.max.x, dest.x + 1.5f), platform.max.y);
			}
			this._twinMeteorEnding.TryStart();
			while (this._twinMeteorEnding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			yield break;
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x001049A3 File Offset: 0x00102BA3
		public IEnumerator CastPredictTwinMeteor(bool left)
		{
			this._leftOfTwinMeteor = left;
			this.character.lookingDirection = (this._leftOfTwinMeteor ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			Bounds platform = this._collisionState.lastStandingCollider.bounds;
			float num = UnityEngine.Random.Range(this._minHeightOfTwinMeteor, this._maxHeightOfTwinMeteor);
			this.character.transform.position = new Vector2(this._leftOfTwinMeteor ? (platform.min.x - 4f) : (platform.max.x + 4f), platform.max.y + num);
			Vector2 vector = MMMaths.RandomBool() ? this._rangeOfPredictTwinMeteorLeft : this._rangeOfPredictTwinMeteorRight;
			float num2 = UnityEngine.Random.Range(vector.x, vector.y);
			float x = Mathf.Clamp(base.target.transform.position.x + num2, platform.min.x, platform.max.x);
			Vector2 source = this.character.transform.position;
			Vector2 dest = new Vector2(x, platform.max.y);
			this._twinMeteorDestination.position = dest;
			this._twinMeteorPreparing.TryStart();
			while (this._twinMeteorPreparing.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._twinMeteor.TryStart();
			yield return this.MoveToDestination(source, dest, this._twinMeteor, this._durationOfTwinMeteor, true, false);
			if (this.character.lookingDirection == Character.LookingDirection.Right)
			{
				this.character.transform.position = new Vector2(Mathf.Max(platform.min.x, dest.x - 2f), platform.max.y);
			}
			else
			{
				this.character.transform.position = new Vector2(Mathf.Min(platform.max.x, dest.x + 2f), platform.max.y);
			}
			this._twinMeteorEnding.TryStart();
			while (this._twinMeteorEnding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			yield break;
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x001049B9 File Offset: 0x00102BB9
		public IEnumerator EscapeForTwinMeteor()
		{
			if (base.dead)
			{
				yield break;
			}
			this._leftOfTwinMeteor = !this._leftOfTwinMeteor;
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			float num = UnityEngine.Random.Range(this._minHeightOfTwinMeteor, this._maxHeightOfTwinMeteor);
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 source = this.character.transform.position;
			Vector2 dest = new Vector2(this._leftOfTwinMeteor ? (bounds.min.x - this._endWidth) : (bounds.max.x + this._endWidth), bounds.max.y + num);
			this.character.ForceToLookAt(this._leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			yield return null;
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._twinMeteorEscape.TryStart();
			yield return this.MoveToDestination(source, dest, this._twinMeteorEscape, this._durationOfTwinMeteorEscaping, false, false);
			yield break;
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x001049C8 File Offset: 0x00102BC8
		private IEnumerator EscapeForTwin(bool flipDest = true)
		{
			if (base.dead)
			{
				yield break;
			}
			if (flipDest)
			{
				this._leftOfTwinMeteor = !this._leftOfTwinMeteor;
			}
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			float endHeight = this._endHeight;
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 source = this.character.transform.position;
			Vector2 dest = new Vector2(this._leftOfTwinMeteor ? (bounds.min.x - this._endWidth) : (bounds.max.x + this._endWidth), bounds.max.y + endHeight);
			this.character.lookingDirection = (this._leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._twinMeteorEscape.TryStart();
			yield return this.MoveToDestination(source, dest, this._twinMeteorEscape, this._durationOfTwinEscape, false, true);
			yield break;
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x001049E0 File Offset: 0x00102BE0
		public void PrepareTwinMeteorOfBehind()
		{
			this._leftOfTwinMeteor = !this._leftOfTwinMeteor;
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			float num = UnityEngine.Random.Range(this._minHeightOfTwinMeteor, this._maxHeightOfTwinMeteor);
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 v = new Vector2(this._leftOfTwinMeteor ? (bounds.min.x - 4f) : (bounds.max.x + 4f), bounds.max.y + num);
			this.character.transform.position = v;
			this.character.lookingDirection = (this._leftOfTwinMeteor ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x00104AC0 File Offset: 0x00102CC0
		public IEnumerator CastGoldenMeteor()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			Bounds platform = this._collisionState.lastStandingCollider.bounds;
			float num = base.target.transform.position.x;
			if (num + 0.5f >= platform.max.x)
			{
				num -= 0.5f;
			}
			else if (num - 0.5f <= platform.min.x)
			{
				num += 0.5f;
			}
			this.character.transform.position = new Vector2(num, platform.max.y + this._heightOfGoldmaneMeteor);
			this._goldenMeteorJump.TryStart();
			while (this._goldenMeteorJump.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._goldenMeteorReady.TryStart();
			while (this._goldenMeteorReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._goldenMeteorAttack.TryStart();
			Vector2 v = this.character.transform.position;
			Vector2 v2 = new Vector2(this.character.transform.position.x, platform.max.y);
			yield return this.MoveToDestination(v, v2, this._goldenMeteorAttack, this._durationOfGoldmaneMeteor, false, true);
			if (this.character.health.dead)
			{
				yield break;
			}
			this._goldenMeteorLanding.TryStart();
			while (this._goldenMeteorLanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x00104ACF File Offset: 0x00102CCF
		public IEnumerator CastMeteorInAir()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this.character.ForceToLookAt(base.target.transform.position.x);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInAirJump.TryStart();
			while (this._meteorInAirJump.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 source = this.character.transform.position;
			Vector2 dest = new Vector2(base.target.transform.position.x, bounds.max.y);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInAirReady.TryStart();
			while (this._meteorInAirReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.movement.verticalVelocity > 0f)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this.character.ForceToLookAt(dest.x);
			this._meteorInAirAttack.TryStart();
			yield return this.MoveToDestination(source, dest, this._meteorInAirAttack, this._durationOfMeteorInAir, true, true);
			if (this.character.health.dead)
			{
				yield break;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInAirLanding.TryStart();
			while (this._meteorInAirLanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInAirStanding.TryStart();
			while (this._meteorInAirStanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x00104ADE File Offset: 0x00102CDE
		public IEnumerator CastMeteorInGround()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGroundReady.TryStart();
			while (this._meteorInGroundReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			if (GameData.HardmodeProgress.hardmode)
			{
				this._meteorInGroundAttackOnHardmode.TryStart();
				while (this._meteorInGroundAttackOnHardmode.running)
				{
					if (this.character.health.dead)
					{
						yield break;
					}
					yield return null;
				}
			}
			else
			{
				this._meteorInGroundAttack.TryStart();
				while (this._meteorInGroundAttack.running)
				{
					if (this.character.health.dead)
					{
						yield break;
					}
					yield return null;
				}
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGroundLanding.TryStart();
			while (this._meteorInGroundLanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGroundStanding.TryStart();
			while (this._meteorInGroundStanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x00104AED File Offset: 0x00102CED
		public IEnumerator CastMeteorInGround2()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 source = this.character.transform.position;
			float x = (source.x > bounds.center.x) ? (bounds.min.x + 2f) : (bounds.max.x - 2f);
			Vector2 dest = new Vector2(x, this.character.transform.position.y);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Ready.TryStart();
			while (this._meteorInGround2Ready.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Attack.TryStart();
			yield return this.MoveToDestination(source, dest, this._meteorInGround2Attack, this._durationOfMeteorInGround2, false, true);
			if (this.character.health.dead)
			{
				yield break;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Landing.TryStart();
			while (this._meteorInGround2Landing.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Standing.TryStart();
			while (this._meteorInGround2Standing.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x00104AFC File Offset: 0x00102CFC
		public IEnumerator CastRush()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this.character.ForceToLookAt(base.target.transform.position.x);
			yield return this._dashOfRush.CRun(this);
			this._rushReady.TryStart();
			while (this._rushReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._rushA.TryStart();
			while (this._rushA.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this.character.ForceToLookAt(base.target.transform.position.x);
			yield return this._dashOfRush.CRun(this);
			this._rushReady.TryStart();
			while (this._rushReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._rushB.TryStart();
			while (this._rushB.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this.character.ForceToLookAt(base.target.transform.position.x);
			yield return this._dashOfRush.CRun(this);
			this._rushReady.TryStart();
			while (this._rushReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._rushC.TryStart();
			while (this._rushC.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._rushFinish.TryStart();
				while (this._rushFinish.running)
				{
					if (this.character.health.dead)
					{
						yield break;
					}
					yield return null;
				}
			}
			this._rushStanding.TryStart();
			while (this._rushStanding.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x00104B0B File Offset: 0x00102D0B
		public IEnumerator CastDimensionPierce()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this._dimensionPierce.TryStart();
			while (this._dimensionPierce.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x00104B1A File Offset: 0x00102D1A
		public IEnumerator CastRisingPierce()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			if (!this._canUseRisingPierce)
			{
				yield break;
			}
			base.StartCoroutine(this.CCoolDownRisingPierce());
			this._risingPierceReady.TryStart();
			while (this._risingPierceReady.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			this._risingPierceAttackAndEnd.TryStart();
			while (this._risingPierceAttackAndEnd.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x00104B29 File Offset: 0x00102D29
		public IEnumerator CastIdle()
		{
			yield return this._idle.CRun(this);
			yield break;
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x00104B38 File Offset: 0x00102D38
		public IEnumerator CastSkippableIdle()
		{
			yield return this._skippableIdle.CRun(this);
			yield break;
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x00104B47 File Offset: 0x00102D47
		private IEnumerator CastPowerWave()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			Bounds platformBounds = this.character.movement.controller.collisionState.lastStandingCollider.bounds;
			float cachedPositionX = this.character.transform.position.x;
			float sizeX = this._risingPeieceLeftRange.bounds.size.x;
			float extentsX = this._risingPeieceLeftRange.bounds.extents.x;
			this._risingPieceStartAttackOperations.gameObject.SetActive(true);
			this._risingPeieceLeftRange.transform.position = new Vector3(cachedPositionX, platformBounds.max.y);
			Physics2D.SyncTransforms();
			this._risingPieceStartAttackOperations.Run(this.character);
			yield return this.character.chronometer.animation.WaitForSeconds(this._risingPeieceTerm);
			int num;
			for (int i = 1; i < this._risingPeieceCount; i = num + 1)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				this._risingPeieceLeftRange.transform.position = new Vector3(cachedPositionX + sizeX * (float)(-(float)i) + (float)(-(float)i) * this._risingPeieceDistance - extentsX, platformBounds.max.y);
				this._risingPeieceRightRange.transform.position = new Vector3(cachedPositionX + sizeX * (float)i + (float)i * this._risingPeieceDistance + extentsX, platformBounds.max.y);
				Physics2D.SyncTransforms();
				this._risingPieceAttackOperations.gameObject.SetActive(true);
				this._risingPieceAttackOperations.Run(this.character);
				yield return this.character.chronometer.animation.WaitForSeconds(this._risingPeieceTerm);
				num = i;
			}
			yield break;
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x00104B56 File Offset: 0x00102D56
		public IEnumerator CastBackstep()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			float num = (base.target.transform.position.x < this.character.transform.position.x) ? bounds.max.x : bounds.min.x;
			Character.LookingDirection lookingDirection = (base.target.transform.position.x < this.character.transform.position.x) ? Character.LookingDirection.Left : Character.LookingDirection.Right;
			if (Mathf.Abs(this.character.transform.position.x - num) <= 2f)
			{
				if (lookingDirection == Character.LookingDirection.Right)
				{
					lookingDirection = Character.LookingDirection.Left;
				}
				else
				{
					lookingDirection = Character.LookingDirection.Right;
				}
			}
			this.character.ForceToLookAt(lookingDirection);
			this._backStep.TryStart();
			while (this._backStep.running)
			{
				if (this.character.health.dead)
				{
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x00104B65 File Offset: 0x00102D65
		public IEnumerator CastRangeAttackHoming(bool centerTarget)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			if (centerTarget)
			{
				Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
				this._rangeAttackHoming.lookTarget = bounds.center;
			}
			else
			{
				this._rangeAttackHoming.lookTarget = base.target.transform.position;
			}
			yield return this._rangeAttackHoming.CRun(this);
			yield break;
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x00104B7B File Offset: 0x00102D7B
		public bool IsMeleeCombat()
		{
			return base.FindClosestPlayerBody(this._meleeAttackTrigger) != null;
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x00104B8F File Offset: 0x00102D8F
		private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Characters.Actions.Action action, float duration, bool rotate = false, bool interporate = true)
		{
			float elapsed = 0f;
			this.ClampDestinationY(ref dest);
			if (interporate)
			{
				float num = (source - dest).magnitude / this._platformWidth;
				duration *= num;
			}
			Character.LookingDirection direction = this.character.lookingDirection;
			if (rotate)
			{
				Vector3 vector = dest - source;
				float num2 = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				if (this.character.lookingDirection == Character.LookingDirection.Left)
				{
					num2 += 180f;
				}
				this._body.rotation = Quaternion.AngleAxis(num2, Vector3.forward);
			}
			while (action.running)
			{
				yield return null;
				if (this.character.chronometer.master.deltaTime > 0f && !this.character.stunedOrFreezed)
				{
					Vector2 v = Vector2.Lerp(source, dest, elapsed / duration);
					this.character.transform.position = v;
					elapsed += this.character.chronometer.animation.deltaTime;
					if (this.character.health.dead)
					{
						this.character.movement.config.type = Movement.Config.Type.Walking;
						this.character.movement.controller.terrainMask = Layers.terrainMask;
						this._body.rotation = Quaternion.identity;
						yield break;
					}
					if (elapsed > duration)
					{
						this.character.CancelAction();
						break;
					}
					if ((source - dest).magnitude < 0.1f)
					{
						this.character.CancelAction();
						break;
					}
				}
			}
			for (;;)
			{
				if (this.character.chronometer.master.deltaTime <= 0f)
				{
					yield return null;
				}
				else
				{
					if (!this.character.stunedOrFreezed)
					{
						break;
					}
					yield return null;
				}
			}
			this.character.transform.position = dest;
			this.character.lookingDirection = direction;
			if (rotate)
			{
				this._body.rotation = Quaternion.identity;
			}
			yield break;
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x00104BCB File Offset: 0x00102DCB
		public IEnumerator CastDash(float stopDistance = 0f)
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			float num = this.character.transform.position.x - base.target.transform.position.x;
			this.character.ForceToLookAt(base.target.transform.position.x);
			this._dash.TryStart();
			float num2 = (num > 0f) ? stopDistance : (-stopDistance);
			Vector2 v = this.character.transform.position;
			Vector2 v2 = new Vector2(base.target.transform.position.x + num2, this.character.transform.position.y);
			yield return this.MoveToDestination(v, v2, this._dash, this._durationOfRush, false, true);
			this.character.CancelAction();
			yield break;
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x00104BE1 File Offset: 0x00102DE1
		private IEnumerator CastTwinAppear()
		{
			if (this.character.health.dead)
			{
				yield break;
			}
			this.character.movement.config.type = Movement.Config.Type.Flying;
			this.character.movement.controller.terrainMask = 0;
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 v = new Vector2(this._leftOfTwinMeteor ? (bounds.min.x - 4f) : (bounds.max.x + 4f), bounds.max.y + this._startHeight);
			Vector2 v2 = new Vector2(this._leftOfTwinMeteor ? (bounds.min.x + this._endDistanceWithPlatformEdge) : (bounds.max.x - this._endDistanceWithPlatformEdge), bounds.max.y);
			this.character.transform.position = v;
			this.character.lookingDirection = (this._leftOfTwinMeteor ? Character.LookingDirection.Right : Character.LookingDirection.Left);
			this._twinAppear.TryStart();
			yield return this.MoveToDestination(v, v2, this._twinAppear, this._durationOfTwinAppear, false, false);
			this.character.movement.config.type = Movement.Config.Type.Walking;
			this.character.movement.controller.terrainMask = Layers.terrainMask;
			yield break;
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x00104BF0 File Offset: 0x00102DF0
		public void Hide()
		{
			this.character.@base.gameObject.SetActive(false);
			this.character.attach.SetActive(false);
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x00104C19 File Offset: 0x00102E19
		public void Show()
		{
			this.character.@base.gameObject.SetActive(true);
			this.character.attach.SetActive(true);
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x00104C42 File Offset: 0x00102E42
		public void Dettachinvincibility()
		{
			this.character.cinematic.Detach(this);
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x00104C56 File Offset: 0x00102E56
		public void Attachinvincibility()
		{
			this.character.cinematic.Attach(this);
		}

		// Token: 0x060057C8 RID: 22472 RVA: 0x00104C69 File Offset: 0x00102E69
		public bool CanUseDimensionPierce()
		{
			return this._dimensionPierce.canUse;
		}

		// Token: 0x060057C9 RID: 22473 RVA: 0x00104C76 File Offset: 0x00102E76
		public bool CanUseRisingPierce()
		{
			return this._risingPierceAttackAndEnd.canUse && this._preDelayOfRisingPierceEnd && this._canUseRisingPierce;
		}

		// Token: 0x060057CA RID: 22474 RVA: 0x00104C95 File Offset: 0x00102E95
		private void ClampDestinationY(ref Vector3 dest)
		{
			if (dest.y <= this._minHeightTransform.transform.position.y)
			{
				dest.y = this._minHeightTransform.transform.position.y;
			}
		}

		// Token: 0x060057CB RID: 22475 RVA: 0x00104CCF File Offset: 0x00102ECF
		public IEnumerator CStartSinglePhasePreDelay()
		{
			this._preDelayOfRisingPierceEnd = false;
			yield return this.character.chronometer.animation.WaitForSeconds(this._preDelayOfRisingPierce);
			this._preDelayOfRisingPierceEnd = true;
			yield break;
		}

		// Token: 0x060057CC RID: 22476 RVA: 0x00104CDE File Offset: 0x00102EDE
		private IEnumerator CCoolDownRisingPierce()
		{
			this._canUseRisingPierce = false;
			yield return this.character.chronometer.master.WaitForSeconds(this._delayOfRisingPierce);
			this._canUseRisingPierce = true;
			yield break;
		}

		// Token: 0x0400468E RID: 18062
		[Subcomponent(typeof(CheckWithinSight))]
		[SerializeField]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x0400468F RID: 18063
		[SerializeField]
		private Characters.Actions.Action _dash;

		// Token: 0x04004690 RID: 18064
		[SerializeField]
		private Transform _body;

		// Token: 0x04004691 RID: 18065
		[Header("Intro")]
		[SerializeField]
		private Characters.Actions.Action _introFall;

		// Token: 0x04004692 RID: 18066
		[SerializeField]
		private Characters.Actions.Action _introLanding;

		// Token: 0x04004693 RID: 18067
		[SerializeField]
		private float _introFallHeight;

		// Token: 0x04004694 RID: 18068
		[Space]
		[SerializeField]
		private Transform _landingPoint;

		// Token: 0x04004695 RID: 18069
		[SerializeField]
		[Space]
		[Header("Awaken")]
		private ChainAction _awakening;

		// Token: 0x04004696 RID: 18070
		[SerializeField]
		private Transform _awakeningPosition;

		// Token: 0x04004697 RID: 18071
		[SerializeField]
		private float _durationOfawakeningAppear = 0.8f;

		// Token: 0x04004698 RID: 18072
		[SerializeField]
		[Header("TwinAppear")]
		private Characters.Actions.Action _twinAppear;

		// Token: 0x04004699 RID: 18073
		[SerializeField]
		private float _startHeight = 5f;

		// Token: 0x0400469A RID: 18074
		[SerializeField]
		private float _endDistanceWithPlatformEdge = 2f;

		// Token: 0x0400469B RID: 18075
		[SerializeField]
		[Space]
		private float _durationOfTwinAppear = 0.6f;

		// Token: 0x0400469C RID: 18076
		[Header("TwinEscape")]
		[SerializeField]
		private float _endHeight = 3f;

		// Token: 0x0400469D RID: 18077
		[SerializeField]
		private float _endWidth = 5f;

		// Token: 0x0400469E RID: 18078
		[SerializeField]
		[Space]
		private float _durationOfTwinEscape = 0.8f;

		// Token: 0x0400469F RID: 18079
		[SerializeField]
		[Header("TwinMeteor")]
		private Characters.Actions.Action _twinMeteorEscape;

		// Token: 0x040046A0 RID: 18080
		[SerializeField]
		private Characters.Actions.Action _twinMeteorPreparing;

		// Token: 0x040046A1 RID: 18081
		[SerializeField]
		private Characters.Actions.Action _twinMeteor;

		// Token: 0x040046A2 RID: 18082
		[SerializeField]
		private Characters.Actions.Action _twinMeteorEnding;

		// Token: 0x040046A3 RID: 18083
		[SerializeField]
		[MinMaxSlider(-10f, 0f)]
		private Vector2 _rangeOfPredictTwinMeteorLeft;

		// Token: 0x040046A4 RID: 18084
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2 _rangeOfPredictTwinMeteorRight;

		// Token: 0x040046A5 RID: 18085
		[SerializeField]
		private float _minHeightOfTwinMeteor;

		// Token: 0x040046A6 RID: 18086
		[SerializeField]
		private float _maxHeightOfTwinMeteor;

		// Token: 0x040046A7 RID: 18087
		[SerializeField]
		private bool _leftOfTwinMeteor;

		// Token: 0x040046A8 RID: 18088
		[SerializeField]
		private float _durationOfTwinMeteorEscaping;

		// Token: 0x040046A9 RID: 18089
		[SerializeField]
		private float _durationOfTwinMeteorPreparing;

		// Token: 0x040046AA RID: 18090
		[SerializeField]
		private float _durationOfTwinMeteor;

		// Token: 0x040046AB RID: 18091
		[SerializeField]
		[Space]
		private Transform _twinMeteorDestination;

		// Token: 0x040046AC RID: 18092
		[SerializeField]
		[Header("GoldenMeteor")]
		private Characters.Actions.Action _goldenMeteorJump;

		// Token: 0x040046AD RID: 18093
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorReady;

		// Token: 0x040046AE RID: 18094
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorAttack;

		// Token: 0x040046AF RID: 18095
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorLanding;

		// Token: 0x040046B0 RID: 18096
		[SerializeField]
		private float _heightOfGoldmaneMeteor = 6f;

		// Token: 0x040046B1 RID: 18097
		[Space]
		[SerializeField]
		private float _durationOfGoldmaneMeteor;

		// Token: 0x040046B2 RID: 18098
		[SerializeField]
		private float _durationOfGoldmaneMeteorOnHardmode;

		// Token: 0x040046B3 RID: 18099
		[Header("MeteorInTheAir")]
		[SerializeField]
		[Space]
		private Characters.Actions.Action _meteorInAirJump;

		// Token: 0x040046B4 RID: 18100
		[SerializeField]
		private Characters.Actions.Action _meteorInAirReady;

		// Token: 0x040046B5 RID: 18101
		[SerializeField]
		private Characters.Actions.Action _meteorInAirAttack;

		// Token: 0x040046B6 RID: 18102
		[SerializeField]
		private Characters.Actions.Action _meteorInAirLanding;

		// Token: 0x040046B7 RID: 18103
		[SerializeField]
		private Characters.Actions.Action _meteorInAirStanding;

		// Token: 0x040046B8 RID: 18104
		[SerializeField]
		private float _durationOfMeteorInAir;

		// Token: 0x040046B9 RID: 18105
		[SerializeField]
		private float _durationOfMeteorInAirOnHardmode;

		// Token: 0x040046BA RID: 18106
		[Header("MeteorInTheGround")]
		[SerializeField]
		private Characters.Actions.Action _meteorInGroundReady;

		// Token: 0x040046BB RID: 18107
		[SerializeField]
		private Characters.Actions.Action _meteorInGroundAttack;

		// Token: 0x040046BC RID: 18108
		[SerializeField]
		private Characters.Actions.Action _meteorInGroundAttackOnHardmode;

		// Token: 0x040046BD RID: 18109
		[SerializeField]
		private Characters.Actions.Action _meteorInGroundLanding;

		// Token: 0x040046BE RID: 18110
		[SerializeField]
		private Characters.Actions.Action _meteorInGroundStanding;

		// Token: 0x040046BF RID: 18111
		[Header("MeteorInTheGround2")]
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Ready;

		// Token: 0x040046C0 RID: 18112
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Attack;

		// Token: 0x040046C1 RID: 18113
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Landing;

		// Token: 0x040046C2 RID: 18114
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Standing;

		// Token: 0x040046C3 RID: 18115
		[Space]
		[SerializeField]
		private float _durationOfMeteorInGround2;

		// Token: 0x040046C4 RID: 18116
		[SerializeField]
		private float _durationOfMeteorInGround2OnHardmode;

		// Token: 0x040046C5 RID: 18117
		[Header("RangeAttackHoming")]
		[Attack.SubcomponentAttribute(true)]
		[SerializeField]
		[Space]
		private MultiCircularProjectileAttack _rangeAttackHoming;

		// Token: 0x040046C6 RID: 18118
		[Header("BackStep")]
		[SerializeField]
		private Characters.Actions.Action _backStep;

		// Token: 0x040046C7 RID: 18119
		[SerializeField]
		[Space]
		private Transform _backStepDestination;

		// Token: 0x040046C8 RID: 18120
		[Header("Rush")]
		[SerializeField]
		private Characters.Actions.Action _rushReady;

		// Token: 0x040046C9 RID: 18121
		[SerializeField]
		private Characters.Actions.Action _rushA;

		// Token: 0x040046CA RID: 18122
		[SerializeField]
		private Characters.Actions.Action _rushB;

		// Token: 0x040046CB RID: 18123
		[SerializeField]
		private Characters.Actions.Action _rushC;

		// Token: 0x040046CC RID: 18124
		[SerializeField]
		private Characters.Actions.Action _rushFinish;

		// Token: 0x040046CD RID: 18125
		[SerializeField]
		private Characters.Actions.Action _rushStanding;

		// Token: 0x040046CE RID: 18126
		[SerializeField]
		[Subcomponent(typeof(Dash))]
		private Dash _dashOfRush;

		// Token: 0x040046CF RID: 18127
		[Space]
		[SerializeField]
		private float _durationOfRush;

		// Token: 0x040046D0 RID: 18128
		[SerializeField]
		[Header("Dimension Piece")]
		private Characters.Actions.Action _dimensionPierce;

		// Token: 0x040046D1 RID: 18129
		[SerializeField]
		private Transform _dimensionPiercePoint;

		// Token: 0x040046D2 RID: 18130
		[SerializeField]
		[Space]
		private int _dimensionPierceCount;

		// Token: 0x040046D3 RID: 18131
		[Subcomponent(typeof(Idle))]
		[Header("Idle")]
		[SerializeField]
		private Idle _idle;

		// Token: 0x040046D4 RID: 18132
		[SerializeField]
		[Subcomponent(typeof(SkipableIdle))]
		private SkipableIdle _skippableIdle;

		// Token: 0x040046D5 RID: 18133
		[Header("Rising Pierce")]
		[SerializeField]
		private float _preDelayOfRisingPierce = 10f;

		// Token: 0x040046D6 RID: 18134
		[FormerlySerializedAs("_risingPieceMotion")]
		[SerializeField]
		private Characters.Actions.Action _risingPierceReady;

		// Token: 0x040046D7 RID: 18135
		[SerializeField]
		private Characters.Actions.Action _risingPierceAttackAndEnd;

		// Token: 0x040046D8 RID: 18136
		[SerializeField]
		private OperationInfos _risingPieceStartAttackOperations;

		// Token: 0x040046D9 RID: 18137
		[SerializeField]
		private OperationInfos _risingPieceAttackOperations;

		// Token: 0x040046DA RID: 18138
		[SerializeField]
		private Collider2D _risingPeieceLeftRange;

		// Token: 0x040046DB RID: 18139
		[SerializeField]
		private Collider2D _risingPeieceRightRange;

		// Token: 0x040046DC RID: 18140
		[SerializeField]
		private float _risingPeieceTerm;

		// Token: 0x040046DD RID: 18141
		[SerializeField]
		private int _risingPeieceCount;

		// Token: 0x040046DE RID: 18142
		[SerializeField]
		[Space]
		private float _risingPeieceDistance;

		// Token: 0x040046DF RID: 18143
		private float _delayOfRisingPierce = 20f;

		// Token: 0x040046E0 RID: 18144
		private bool _canUseRisingPierce = true;

		// Token: 0x040046E1 RID: 18145
		private bool _preDelayOfRisingPierceEnd;

		// Token: 0x040046E2 RID: 18146
		[SerializeField]
		[Header("Tools")]
		private Collider2D _meleeAttackTrigger;

		// Token: 0x040046E3 RID: 18147
		[SerializeField]
		private Transform _minHeightTransform;

		// Token: 0x040046E4 RID: 18148
		private float _platformWidth;

		// Token: 0x040046E5 RID: 18149
		private CharacterController2D.CollisionState _collisionState;

		// Token: 0x040046E6 RID: 18150
		private static NonAllocOverlapper _nonAllocOverlapper;

		// Token: 0x040046E7 RID: 18151
		private const float _maxDistanceOfWall = 4f;
	}
}
