using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.AI.Behaviours;
using Characters.Movements;
using Hardmode;
using Level;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x02001168 RID: 4456
	public class DarkAideAI : AIController
	{
		// Token: 0x06005724 RID: 22308 RVA: 0x00102C24 File Offset: 0x00100E24
		private new void Start()
		{
			base.Start();
			this._collisionState = this.character.movement.controller.collisionState;
			this.platformWidth = this._collisionState.lastStandingCollider.bounds.size.x;
			this._patterns = new List<DarkAideAI.Pattern>(this._goldenMeteorPercent + this._meteorAirPercent + this._meteorGroundPercent + this._homingPercent);
			for (int i = 0; i < this._goldenMeteorPercent; i++)
			{
				this._patterns.Add(DarkAideAI.Pattern.GoldenMeteor);
			}
			for (int j = 0; j < this._meteorAirPercent; j++)
			{
				this._patterns.Add(DarkAideAI.Pattern.MeteorAir);
			}
			for (int k = 0; k < this._meteorGroundPercent; k++)
			{
				this._patterns.Add(DarkAideAI.Pattern.MeteorGround);
			}
			for (int l = 0; l < this._homingPercent; l++)
			{
				this._patterns.Add(DarkAideAI.Pattern.Homing);
			}
			this.character.health.onDiedTryCatch += delegate()
			{
				this._body.rotation = Quaternion.identity;
			};
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x00102D2F File Offset: 0x00100F2F
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CProcess());
			base.StartCoroutine(this._checkWithinSight.CRun(this));
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x00102D57 File Offset: 0x00100F57
		protected override IEnumerator CProcess()
		{
			yield return Chronometer.global.WaitForSeconds(1f);
			this.character.movement.config.type = Movement.Config.Type.Walking;
			base.StartCoroutine(this.CStartPredelay());
			while (!base.dead)
			{
				yield return this.Combat();
			}
			yield break;
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x00102D68 File Offset: 0x00100F68
		public void ApplyHealth(Character healthOwner)
		{
			if (healthOwner.health.currentHealth > 0.0)
			{
				this.character.health.SetCurrentHealth(healthOwner.health.currentHealth);
				this.character.health.PercentHeal(0.4f);
				return;
			}
			this.character.health.SetCurrentHealth(healthOwner.health.maximumHealth);
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x00102DD8 File Offset: 0x00100FD8
		private IEnumerator RunPattern(DarkAideAI.Pattern pattern)
		{
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			switch (pattern)
			{
			case DarkAideAI.Pattern.GoldenMeteor:
				yield return this.CastGoldenMeteor();
				break;
			case DarkAideAI.Pattern.MeteorAir:
				yield return this.CastMeteorInAir();
				break;
			case DarkAideAI.Pattern.Rush:
				yield return this.CastRush();
				break;
			case DarkAideAI.Pattern.DimensionPierce:
				yield return this.CastDimensionPierce();
				break;
			case DarkAideAI.Pattern.Homing:
				yield return this.CastRangeAttackHoming();
				break;
			case DarkAideAI.Pattern.MeteorGround:
				yield return this.CastBackstep();
				yield return this.CastMeteorInGround2();
				break;
			case DarkAideAI.Pattern.Idle:
				yield return this.CastIdle();
				break;
			case DarkAideAI.Pattern.SkippableIdle:
				yield return this.CastSkippableIdle();
				break;
			}
			yield break;
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x00102DEE File Offset: 0x00100FEE
		private IEnumerator Combat()
		{
			if (this.CanUseDarkDimensionRush())
			{
				yield return this.RunPattern(DarkAideAI.Pattern.Rush);
				yield return this.RunPattern(DarkAideAI.Pattern.Idle);
			}
			else if (this.CanUseDarkDimensionPierce())
			{
				yield return this.RunPattern(DarkAideAI.Pattern.DimensionPierce);
				yield return this.RunPattern(DarkAideAI.Pattern.SkippableIdle);
			}
			else
			{
				DarkAideAI.Pattern pattern = this._patterns.Random<DarkAideAI.Pattern>();
				yield return this.RunPattern(pattern);
				if (pattern == DarkAideAI.Pattern.GoldenMeteor)
				{
					yield return this.RunPattern(DarkAideAI.Pattern.Idle);
				}
				else if (pattern == DarkAideAI.Pattern.MeteorAir)
				{
					yield return this.RunPattern(DarkAideAI.Pattern.Idle);
				}
				else if (pattern == DarkAideAI.Pattern.MeteorGround)
				{
					yield return this.RunPattern(DarkAideAI.Pattern.SkippableIdle);
				}
				else if (pattern == DarkAideAI.Pattern.Homing)
				{
					yield return this.RunPattern(DarkAideAI.Pattern.SkippableIdle);
				}
			}
			yield break;
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x00102DFD File Offset: 0x00100FFD
		private IEnumerator CastIdle()
		{
			yield return this._idle.CRun(this);
			yield break;
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x00102E0C File Offset: 0x0010100C
		private IEnumerator CastSkippableIdle()
		{
			yield return this._skippableIdle.CRun(this);
			yield break;
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x00102E1B File Offset: 0x0010101B
		public IEnumerator CastGoldenMeteor()
		{
			Bounds platform = this._collisionState.lastStandingCollider.bounds;
			this._countOfGoldenMeteor = UnityEngine.Random.Range(this._countOfGoldenMeteorRange.x, this._countOfGoldenMeteorRange.y);
			int num;
			for (int i = 0; i < this._countOfGoldenMeteor; i = num + 1)
			{
				Vector2 v = new Vector2(base.target.transform.position.x, platform.max.y + this._heightOfGoldenMeteor);
				this._teleportDestination.transform.position = v;
				yield return this._teleportForAir.CRun(this);
				this._goldenMeteorJump.TryStart();
				while (this._goldenMeteorJump.running)
				{
					yield return null;
				}
				this._goldenMeteorReady.TryStart();
				while (this._goldenMeteorReady.running)
				{
					yield return null;
				}
				this._goldenMeteorAttack.TryStart();
				Vector2 v2 = this.character.transform.position;
				Vector2 v3 = new Vector2(this.character.transform.position.x, platform.max.y);
				yield return this.MoveToDestination(v2, v3, this._goldenMeteorAttack, this._durationOfGoldenMeteor, false, true);
				this._goldenMeteorLanding.TryStart();
				while (this._goldenMeteorLanding.running)
				{
					yield return null;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x00102E2A File Offset: 0x0010102A
		public IEnumerator CastMeteorInAir()
		{
			Bounds platform = this._collisionState.lastStandingCollider.bounds;
			float duration = Singleton<HardmodeManager>.Instance.hardmode ? this._durationOfMeteorInAirHardmode : this._durationOfMeteorInAir;
			this._countOfMeteorInAir = UnityEngine.Random.Range(this._countOfMeteorInAirRange.x, this._countOfMeteorInAirRange.y);
			int num;
			for (int i = 0; i < this._countOfMeteorInAir; i = num + 1)
			{
				Vector2 position = this.GetMeteorAirStartPosition();
				while (this.character.stunedOrFreezed)
				{
					yield return null;
				}
				this._teleportDestination.transform.position = position;
				yield return this._teleportForAir.CRun(this);
				Vector2 source = this.character.transform.position;
				Vector2 dest = new Vector2(base.target.transform.position.x, platform.max.y);
				this.character.ForceToLookAt(dest.x);
				while (this.character.stunedOrFreezed)
				{
					yield return null;
				}
				this._meteorInAirJumpAndReady.TryStart();
				while (this._meteorInAirJumpAndReady.running)
				{
					yield return null;
				}
				while (this.character.stunedOrFreezed)
				{
					yield return null;
				}
				this._meteorInAirAttack.TryStart();
				yield return this.MoveToDestination(source, dest, this._meteorInAirAttack, duration, true, false);
				while (this._meteorInAirAttack.running)
				{
					yield return null;
				}
				while (this.character.stunedOrFreezed)
				{
					yield return null;
				}
				this._meteorInAirLandingAndStanding.TryStart();
				while (this._meteorInAirLandingAndStanding.running)
				{
					yield return null;
				}
				position = default(Vector2);
				source = default(Vector2);
				dest = default(Vector2);
				num = i;
			}
			yield break;
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x00102E3C File Offset: 0x0010103C
		private Vector2 GetMeteorAirStartPosition()
		{
			float distanceToPlayerOfMeteorInAir = this._distanceToPlayerOfMeteorInAir;
			Vector2 vector = this.Clamp();
			float angle = UnityEngine.Random.Range(vector.x, vector.y);
			Vector2 b = this.RotateVector(Vector2.right, angle) * distanceToPlayerOfMeteorInAir;
			Bounds bounds = base.target.movement.controller.collisionState.lastStandingCollider.bounds;
			return new Vector2(base.target.transform.position.x, bounds.max.y) + b;
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x00102ECC File Offset: 0x001010CC
		private Vector2 Clamp()
		{
			Vector2 angle = this._angleOfMeteorInAir;
			angle = this.MinClamp(angle);
			return this.MaxClamp(angle);
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x00102EF4 File Offset: 0x001010F4
		private Vector2 MinClamp(Vector2 angle)
		{
			float distanceToPlayerOfMeteorInAir = this._distanceToPlayerOfMeteorInAir;
			Vector2 b = this.RotateVector(Vector2.right, this._angleOfMeteorInAir.x) * distanceToPlayerOfMeteorInAir;
			if ((base.target.transform.position + b).x >= Map.Instance.bounds.max.x)
			{
				return new Vector2(90f, this._angleOfMeteorInAir.y);
			}
			return angle;
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x00102F78 File Offset: 0x00101178
		private Vector2 MaxClamp(Vector2 angle)
		{
			float distanceToPlayerOfMeteorInAir = this._distanceToPlayerOfMeteorInAir;
			Vector2 b = this.RotateVector(Vector2.right, this._angleOfMeteorInAir.y) * distanceToPlayerOfMeteorInAir;
			if ((base.target.transform.position + b).x <= Map.Instance.bounds.min.x)
			{
				return new Vector2(this._angleOfMeteorInAir.x, 90f);
			}
			return angle;
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x00102FFC File Offset: 0x001011FC
		private Vector2 RotateVector(Vector2 v, float angle)
		{
			float f = angle * 0.017453292f;
			float x = v.x * Mathf.Cos(f) - v.y * Mathf.Sin(f);
			float y = v.x * Mathf.Sin(f) + v.y * Mathf.Cos(f);
			return new Vector2(x, y);
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x0010304E File Offset: 0x0010124E
		public IEnumerator CastMeteorInGround2()
		{
			Bounds bounds = this._collisionState.lastStandingCollider.bounds;
			Vector2 source = this.character.transform.position;
			float num = (source.x > bounds.center.x) ? (bounds.min.x + 3f) : (bounds.max.x - 3f);
			Vector2 dest = new Vector2(num, this.character.transform.position.y);
			this.character.ForceToLookAt(num);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Ready.TryStart();
			while (this._meteorInGround2Ready.running)
			{
				yield return null;
			}
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Attack.TryStart();
			yield return this.MoveToDestination(source, dest, this._meteorInGround2Attack, this._durationOfMeteorInGround2, false, true);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			this._meteorInGround2Landing.TryStart();
			yield return this._thunderAttack.CRun(this);
			while (this.character.stunedOrFreezed)
			{
				yield return null;
			}
			while (this._meteorInGround2Landing.running)
			{
				yield return null;
			}
			this._meteorInGround2Standing.TryStart();
			while (this._meteorInGround2Standing.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x0010305D File Offset: 0x0010125D
		public IEnumerator CastRush()
		{
			yield return this._darkRush.CRun(this);
			yield break;
		}

		// Token: 0x06005735 RID: 22325 RVA: 0x0010306C File Offset: 0x0010126C
		public IEnumerator CastRangeAttackHoming()
		{
			yield return this._rangeAttack.CRun(this);
			yield break;
		}

		// Token: 0x06005736 RID: 22326 RVA: 0x0010307B File Offset: 0x0010127B
		public IEnumerator CastBackstep()
		{
			Bounds platformBounds = this._collisionState.lastStandingCollider.bounds;
			float destinationX = (platformBounds.center.x > this.character.transform.position.x) ? (platformBounds.max.x - 1f) : (platformBounds.min.x + 1f);
			Vector2 v = new Vector2(destinationX, platformBounds.max.y);
			this._teleportDestination.transform.position = v;
			this.character.ForceToLookAt((destinationX > platformBounds.center.x) ? platformBounds.max.x : platformBounds.min.x);
			yield return this._backStepTeleport.CRun(this);
			this.character.ForceToLookAt((destinationX > platformBounds.center.x) ? platformBounds.max.x : platformBounds.min.x);
			yield break;
		}

		// Token: 0x06005737 RID: 22327 RVA: 0x0010308A File Offset: 0x0010128A
		public IEnumerator CastDimensionPierce()
		{
			this._dimensionPierceCount = UnityEngine.Random.Range(this._dimensionPierceCountRange.x, this._dimensionPierceCountRange.y);
			int num;
			for (int i = 0; i < this._dimensionPierceCount; i = num + 1)
			{
				this._dimensionPierce.TryStart();
				while (this._dimensionPierce.running)
				{
					yield return null;
				}
				num = i;
			}
			this._dimensionPierceCoolTimeAction.TryStart();
			yield break;
		}

		// Token: 0x06005738 RID: 22328 RVA: 0x00103099 File Offset: 0x00101299
		private IEnumerator MoveToDestination(Vector3 source, Vector3 dest, Characters.Actions.Action action, float duration, bool rotate = false, bool interporate = true)
		{
			float elapsed = 0f;
			this.ClampDestinationY(ref dest);
			if (interporate)
			{
				float num = (source - dest).magnitude / this.platformWidth;
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
				if (this.character.stunedOrFreezed)
				{
					yield return null;
				}
				else
				{
					Vector2 v = Vector2.Lerp(source, dest, elapsed / duration);
					this.character.transform.position = v;
					elapsed += this.character.chronometer.master.deltaTime;
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

		// Token: 0x06005739 RID: 22329 RVA: 0x001030D5 File Offset: 0x001012D5
		private bool CanUseDarkDimensionPierce()
		{
			return this._dimensionPierceCoolTimeAction.canUse;
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x001030E2 File Offset: 0x001012E2
		private bool CanUseDarkDimensionRush()
		{
			return this._darkRush.CanUse() && this._darkRushPredelayEnd;
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x001030F9 File Offset: 0x001012F9
		private void ClampDestinationY(ref Vector3 dest)
		{
			if (dest.y <= this._minHeightTransform.transform.position.y)
			{
				dest.y = this._minHeightTransform.transform.position.y;
			}
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x00103133 File Offset: 0x00101333
		private IEnumerator CStartPredelay()
		{
			this._darkRushPredelayEnd = false;
			yield return this.character.chronometer.master.WaitForSeconds(this._darkRushPredelay);
			this._darkRushPredelayEnd = true;
			yield break;
		}

		// Token: 0x04004604 RID: 17924
		[SerializeField]
		private Transform _minHeightTransform;

		// Token: 0x04004605 RID: 17925
		[SerializeField]
		[Subcomponent(typeof(CheckWithinSight))]
		private CheckWithinSight _checkWithinSight;

		// Token: 0x04004606 RID: 17926
		[SerializeField]
		private Transform _body;

		// Token: 0x04004607 RID: 17927
		[SerializeField]
		private Transform _teleportDestination;

		// Token: 0x04004608 RID: 17928
		[SerializeField]
		[Space]
		[Header("GoldmaneMeteor")]
		private Characters.Actions.Action _goldenMeteorJump;

		// Token: 0x04004609 RID: 17929
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorReady;

		// Token: 0x0400460A RID: 17930
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorAttack;

		// Token: 0x0400460B RID: 17931
		[SerializeField]
		private Characters.Actions.Action _goldenMeteorLanding;

		// Token: 0x0400460C RID: 17932
		private int _countOfGoldenMeteor = 3;

		// Token: 0x0400460D RID: 17933
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2Int _countOfGoldenMeteorRange;

		// Token: 0x0400460E RID: 17934
		[SerializeField]
		private float _heightOfGoldenMeteor;

		// Token: 0x0400460F RID: 17935
		[SerializeField]
		private float _durationOfGoldenMeteor;

		// Token: 0x04004610 RID: 17936
		[Header("MeteorInTheAir")]
		[Space]
		[SerializeField]
		private Characters.Actions.Action _meteorInAirJumpAndReady;

		// Token: 0x04004611 RID: 17937
		[SerializeField]
		private Characters.Actions.Action _meteorInAirAttack;

		// Token: 0x04004612 RID: 17938
		[SerializeField]
		private Characters.Actions.Action _meteorInAirLandingAndStanding;

		// Token: 0x04004613 RID: 17939
		[MinMaxSlider(0f, 10f)]
		[SerializeField]
		private Vector2Int _countOfMeteorInAirRange;

		// Token: 0x04004614 RID: 17940
		private int _countOfMeteorInAir = 2;

		// Token: 0x04004615 RID: 17941
		[SerializeField]
		private float _durationOfMeteorInAir;

		// Token: 0x04004616 RID: 17942
		[SerializeField]
		private float _durationOfMeteorInAirHardmode;

		// Token: 0x04004617 RID: 17943
		[SerializeField]
		private float _distanceToPlayerOfMeteorInAir;

		// Token: 0x04004618 RID: 17944
		[MinMaxSlider(-180f, 180f)]
		[SerializeField]
		private Vector2 _angleOfMeteorInAir;

		// Token: 0x04004619 RID: 17945
		[SerializeField]
		[Subcomponent(typeof(Teleport))]
		private Teleport _teleportForAir;

		// Token: 0x0400461A RID: 17946
		[Header("MeteorInTheGround2")]
		[SerializeField]
		[Space]
		private Characters.Actions.Action _meteorInGround2Ready;

		// Token: 0x0400461B RID: 17947
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Attack;

		// Token: 0x0400461C RID: 17948
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Landing;

		// Token: 0x0400461D RID: 17949
		[SerializeField]
		private Characters.Actions.Action _meteorInGround2Standing;

		// Token: 0x0400461E RID: 17950
		[SerializeField]
		private float _durationOfMeteorInGround2;

		// Token: 0x0400461F RID: 17951
		[SerializeField]
		[Subcomponent(typeof(ThunderAttack))]
		private ThunderAttack _thunderAttack;

		// Token: 0x04004620 RID: 17952
		[SerializeField]
		[Subcomponent(typeof(RangeAttack))]
		[Header("RangeAttackHoming")]
		[Space]
		private RangeAttack _rangeAttack;

		// Token: 0x04004621 RID: 17953
		[Subcomponent(typeof(Teleport))]
		[SerializeField]
		[Space]
		[Header("BackStep")]
		private Teleport _backStepTeleport;

		// Token: 0x04004622 RID: 17954
		[Header("Rush")]
		[SerializeField]
		[Space]
		[Subcomponent(typeof(DarkRush))]
		private DarkRush _darkRush;

		// Token: 0x04004623 RID: 17955
		[SerializeField]
		private float _darkRushPredelay = 15f;

		// Token: 0x04004624 RID: 17956
		private bool _darkRushPredelayEnd;

		// Token: 0x04004625 RID: 17957
		[Header("Dimension Piece")]
		[SerializeField]
		private Characters.Actions.Action _dimensionPierce;

		// Token: 0x04004626 RID: 17958
		[SerializeField]
		private Characters.Actions.Action _dimensionPierceCoolTimeAction;

		// Token: 0x04004627 RID: 17959
		[SerializeField]
		private Transform _dimensionPiercePoint;

		// Token: 0x04004628 RID: 17960
		[Space]
		[SerializeField]
		[MinMaxSlider(0f, 10f)]
		private Vector2Int _dimensionPierceCountRange;

		// Token: 0x04004629 RID: 17961
		private int _dimensionPierceCount;

		// Token: 0x0400462A RID: 17962
		[SerializeField]
		[Subcomponent(typeof(Idle))]
		[Space]
		[Header("Idle")]
		private Idle _idle;

		// Token: 0x0400462B RID: 17963
		[SerializeField]
		[Subcomponent(typeof(SkipableIdle))]
		private SkipableIdle _skippableIdle;

		// Token: 0x0400462C RID: 17964
		[SerializeField]
		[Header("Tools")]
		private Collider2D _meleeAttackTrigger;

		// Token: 0x0400462D RID: 17965
		private float platformWidth;

		// Token: 0x0400462E RID: 17966
		private CharacterController2D.CollisionState _collisionState;

		// Token: 0x0400462F RID: 17967
		[Range(0f, 10f)]
		[SerializeField]
		[Header("Pattern Proportion")]
		private int _goldenMeteorPercent;

		// Token: 0x04004630 RID: 17968
		[SerializeField]
		[Range(0f, 10f)]
		private int _meteorAirPercent;

		// Token: 0x04004631 RID: 17969
		[SerializeField]
		[Range(0f, 10f)]
		private int _meteorGroundPercent;

		// Token: 0x04004632 RID: 17970
		[Range(0f, 10f)]
		[SerializeField]
		private int _homingPercent;

		// Token: 0x04004633 RID: 17971
		private List<DarkAideAI.Pattern> _patterns;

		// Token: 0x02001169 RID: 4457
		private enum Pattern
		{
			// Token: 0x04004635 RID: 17973
			GoldenMeteor,
			// Token: 0x04004636 RID: 17974
			MeteorAir,
			// Token: 0x04004637 RID: 17975
			Rush,
			// Token: 0x04004638 RID: 17976
			DimensionPierce,
			// Token: 0x04004639 RID: 17977
			Homing,
			// Token: 0x0400463A RID: 17978
			MeteorGround,
			// Token: 0x0400463B RID: 17979
			Idle,
			// Token: 0x0400463C RID: 17980
			SkippableIdle
		}
	}
}
