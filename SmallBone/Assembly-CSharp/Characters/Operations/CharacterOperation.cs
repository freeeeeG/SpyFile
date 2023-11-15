using System;
using Characters.Operations.Animation;
using Characters.Operations.Attack;
using Characters.Operations.BehaviorDesigner;
using Characters.Operations.Customs;
using Characters.Operations.Customs.AquaSkull;
using Characters.Operations.Customs.BombSkul;
using Characters.Operations.Customs.DavyJones;
using Characters.Operations.Customs.EntSkul;
using Characters.Operations.Customs.GraveDigger;
using Characters.Operations.Customs.Minotaurus;
using Characters.Operations.Customs.Skeleton_Mage;
using Characters.Operations.Customs.Skeleton_Sword;
using Characters.Operations.Decorator;
using Characters.Operations.Decorator.Deprecated;
using Characters.Operations.Essences;
using Characters.Operations.Fx;
using Characters.Operations.Gauge;
using Characters.Operations.GrabBorad;
using Characters.Operations.Health;
using Characters.Operations.Items;
using Characters.Operations.Movement;
using Characters.Operations.ObjectTransform;
using Characters.Operations.Summon;
using UnityEditor;

namespace Characters.Operations
{
	// Token: 0x02000DB4 RID: 3508
	public abstract class CharacterOperation : TargetedCharacterOperation
	{
		// Token: 0x0600469F RID: 18079 RVA: 0x000CC3FC File Offset: 0x000CA5FC
		static CharacterOperation()
		{
			int length = typeof(CharacterOperation).Namespace.Length;
			CharacterOperation.names = new string[CharacterOperation.types.Length];
			for (int i = 0; i < CharacterOperation.names.Length; i++)
			{
				Type type = CharacterOperation.types[i];
				if (type == null)
				{
					string text = CharacterOperation.names[i - 1];
					int num = text.LastIndexOf('/');
					if (num == -1)
					{
						CharacterOperation.names[i] = string.Empty;
					}
					else
					{
						CharacterOperation.names[i] = text.Substring(0, num + 1);
					}
				}
				else
				{
					CharacterOperation.names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
				}
			}
		}

		// Token: 0x060046A0 RID: 18080
		public abstract void Run(Character owner);

		// Token: 0x060046A1 RID: 18081 RVA: 0x000CD136 File Offset: 0x000CB336
		public override void Run(Character owner, Character target)
		{
			this.Run(target);
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x00002191 File Offset: 0x00000391
		public virtual void Stop()
		{
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x000CD13F File Offset: 0x000CB33F
		protected virtual void OnDestroy()
		{
			this.Stop();
		}

		// Token: 0x0400357F RID: 13695
		public new static readonly Type[] types = new Type[]
		{
			typeof(InstantAttack),
			typeof(InstantAttack2),
			typeof(InstantAttackByCount),
			null,
			typeof(SweepAttack),
			typeof(SweepAttack2),
			null,
			typeof(RingAttack),
			typeof(TeleportAttack),
			null,
			typeof(CastAttack),
			null,
			typeof(GrabbedTargetAttack),
			null,
			typeof(PlayerAttack),
			typeof(BreakPlayerShield),
			null,
			typeof(FireProjectile),
			typeof(FireProjectileInBounds),
			typeof(FireBouncyProjectile),
			typeof(MultipleFireProjectile),
			typeof(GlobalAttack),
			typeof(WeaponMaster),
			typeof(CameraShake),
			typeof(CameraShakeCurve),
			typeof(CameraZoom),
			null,
			typeof(PlayMusic),
			typeof(PlayChapterMusic),
			typeof(PauseBackgroundMusic),
			typeof(StopMusic),
			typeof(PlaySound),
			typeof(SetInternalMusicVolume),
			null,
			typeof(MotionTrail),
			typeof(ScreenFlash),
			typeof(ShaderEffect),
			typeof(SpawnEffect),
			typeof(SpawnEffectOnScreen),
			typeof(SetOwnerColor),
			null,
			typeof(Vignette),
			typeof(Vibration),
			typeof(SpawnLineText),
			typeof(SpawnEnemyLineText),
			typeof(DropParts),
			typeof(ByAbility),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Repeater3),
			typeof(Chance),
			typeof(Characters.Operations.Decorator.Random),
			typeof(SeedRandom),
			typeof(WeightedRandom),
			typeof(ByLookingDirection),
			typeof(OneByOne),
			typeof(CharacterTypeFilter),
			typeof(HealthFilter),
			typeof(InHardmode),
			typeof(ByPositionX),
			typeof(ByCharacterSize),
			typeof(Sequencer),
			typeof(RandomlyRunningOperation),
			typeof(SummonCharacter),
			typeof(SummonMinion),
			typeof(SummonMinionToTarget),
			typeof(SummonMonster),
			typeof(SummonOperationRunner),
			typeof(SummonMultipleOperationRunners),
			typeof(SummonOperationRunnersOnGround),
			typeof(SummonOperationRunnersOnGroundOneDirection),
			typeof(SummonOperationRunnerAtChildren),
			typeof(SummonOperationRunnersAtTargetWithinRange),
			typeof(SummonOriginBasedOperationRunners),
			typeof(SummonLiquid),
			typeof(SummonBDCharacter),
			typeof(SummonBDRandomCharacter),
			typeof(SummonCharactersInRange),
			typeof(MinionGroupOperations),
			typeof(DespawnMinions),
			typeof(SummonDarkOrb),
			typeof(Move),
			typeof(MoveTo),
			typeof(DualMove),
			typeof(StopMove),
			null,
			typeof(ClearGrabBoard),
			typeof(StartToAddGrabTarget),
			typeof(StartMovingGrabbedTarget),
			null,
			typeof(ChangeGravity),
			typeof(ModifyVerticalVelocity),
			typeof(OverrideMovementConfig),
			typeof(SetCharacterColliderLayerMask),
			typeof(Jump),
			typeof(JumpDown),
			typeof(Teleport),
			typeof(TeleportToCharacter),
			typeof(TeleportOverTime),
			typeof(FlipObject),
			typeof(SetPositionTo),
			typeof(SetRotationTo),
			typeof(SetRotationToTarget),
			typeof(SetScaleToDistance),
			typeof(MoveTransform),
			typeof(MoveTransformFromPosition),
			typeof(ResetGlobalTransformToLocal),
			typeof(RotateAngle),
			typeof(RotateTransform),
			typeof(RotateToTarget),
			typeof(LerpTransform),
			typeof(ScaleByPlatformSize),
			typeof(SetParentToCharacterAttach),
			typeof(ActivateLaser),
			typeof(LerpLaser),
			typeof(SetRotationByDirection),
			typeof(Heal),
			typeof(RangeHeal),
			typeof(LoseHealth),
			typeof(Invulnerable),
			typeof(Lockout),
			typeof(Suicide),
			typeof(Cinematic),
			typeof(Evasion),
			typeof(AddGaugeValue),
			typeof(SetGaugeValue),
			typeof(ClearGaugeValue),
			typeof(Change),
			typeof(Remove),
			typeof(Discard),
			typeof(Equip),
			typeof(Drop),
			typeof(DropByRarity),
			typeof(SummonDwarfTurret),
			typeof(AttachKirizGlobal),
			typeof(Naias),
			typeof(ApplyStatusFromEssenceOwner),
			typeof(SetAnimationClip),
			typeof(Characters.Operations.GrabBorad.AddToGrabBoard),
			typeof(RunOperationOnGrabBoard),
			typeof(OverrideBDVariableInRange),
			typeof(FindEnemy),
			typeof(SetBehaviorTreeVariable),
			typeof(IncreaseSharedIntVariable),
			typeof(ModifyTimeScale),
			typeof(ApplyStatus),
			typeof(AddMarkStack),
			null,
			typeof(AttachAbility),
			typeof(AttachAbilityGlobal),
			typeof(AttachAbilityWithinCollider),
			typeof(AttachAbilityToMinions),
			null,
			typeof(Polymorph),
			typeof(StartWeaponPolymorph),
			typeof(EndWeaponPolymorph),
			null,
			typeof(SwapWeapon),
			typeof(ReduceCooldownTime),
			typeof(SetRemainCooldownTime),
			typeof(ModifyRemainCooldownTime),
			typeof(ModifyEssenceRemainCooldownTime),
			typeof(TriggerActionStart),
			typeof(DoAction),
			null,
			typeof(ActivateGameObjectOperation),
			typeof(DeactivateGameObject),
			null,
			typeof(SpawnProp),
			typeof(SpawnCharacter),
			typeof(SpawnRandomCharacter),
			typeof(DestoryCharacter),
			typeof(SetCharacterVisible),
			typeof(UpdateAnimation),
			null,
			typeof(LookAt),
			typeof(LookTarget),
			typeof(LookTargetOpposition),
			null,
			typeof(TakeAim),
			typeof(TakeAimTowardsTheFront),
			typeof(GiveBuff),
			null,
			typeof(SpawnGold),
			typeof(ConsumeGold),
			null,
			typeof(InvokeUnityEvent),
			typeof(LerpCollider),
			typeof(StopAnotherOperation),
			typeof(PrintDebugLog),
			typeof(Instantiate),
			null,
			typeof(Guard),
			null,
			typeof(TeleportToSkulHead),
			typeof(SpawnThiefGoldAtTarget),
			typeof(ArchlichPassive),
			typeof(AddYakshaStompStack),
			typeof(PrisonerPhaser),
			typeof(Samurai2IlseomInstantAttack),
			typeof(MultidimensionalPrism),
			typeof(DropManatechPart),
			null,
			typeof(AddRockstarPassiveStack),
			typeof(SummonRockstarAmp),
			typeof(FireHighTideProjectile),
			typeof(FireLowTideProjectile),
			typeof(FireWaterspoutProjectile),
			typeof(SetFloodSweepAttackDamageMultiplier),
			typeof(ReduceCooldownTimeByProjectileCount),
			typeof(Explode),
			typeof(AddDamageStack),
			typeof(RiskyUpgrade),
			typeof(SummonSmallBomb),
			typeof(EntSkulPassive),
			typeof(EntSkulThornyVine),
			typeof(SummonEntMinionAtEntSapling),
			typeof(SummonEntSapling),
			typeof(SummonMinionFromGraves),
			typeof(SummonGrave),
			typeof(SummonLandOfTheDead),
			typeof(SpawnCorpseForLandOfTheDead),
			typeof(StartRecordAttacks),
			typeof(Skeleton_SwordInstantAttack),
			typeof(Skeleton_SwordSweepAttack),
			typeof(AddManaChargingSpeedMultiplier),
			typeof(TryReduceMana),
			typeof(FillUpMana),
			typeof(PushCannonBall),
			typeof(PopCannonBall),
			typeof(FireCannonBall)
		};

		// Token: 0x04003580 RID: 13696
		public new static readonly string[] names;

		// Token: 0x02000DB5 RID: 3509
		public new class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060046A5 RID: 18085 RVA: 0x000CD14F File Offset: 0x000CB34F
			public SubcomponentAttribute() : base(true, CharacterOperation.types, CharacterOperation.names)
			{
			}
		}

		// Token: 0x02000DB6 RID: 3510
		[Serializable]
		public new class Subcomponents : SubcomponentArray<CharacterOperation>
		{
			// Token: 0x060046A6 RID: 18086 RVA: 0x000CD164 File Offset: 0x000CB364
			public void Initialize()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Initialize();
				}
			}

			// Token: 0x060046A7 RID: 18087 RVA: 0x000CD194 File Offset: 0x000CB394
			public void Run(Character owner)
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Run(owner);
				}
			}

			// Token: 0x060046A8 RID: 18088 RVA: 0x000CD1C2 File Offset: 0x000CB3C2
			public void Run(Character owner, Character target)
			{
				this.Run(target);
			}

			// Token: 0x060046A9 RID: 18089 RVA: 0x000CD1CC File Offset: 0x000CB3CC
			public void Stop()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Stop();
				}
			}
		}
	}
}
