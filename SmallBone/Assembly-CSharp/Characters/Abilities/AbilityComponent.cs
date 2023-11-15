using System;
using Characters.Abilities.CharacterStat;
using Characters.Abilities.Customs;
using Characters.Abilities.Darks;
using Characters.Abilities.Debuffs;
using Characters.Abilities.Decorators;
using Characters.Abilities.Enemies;
using Characters.Abilities.Essences;
using Characters.Abilities.Items;
using Characters.Abilities.Upgrades;
using Characters.Abilities.Weapons;
using Characters.Abilities.Weapons.DavyJones;
using Characters.Abilities.Weapons.EntSkul;
using Characters.Abilities.Weapons.Fighter;
using Characters.Abilities.Weapons.Ghoul;
using Characters.Abilities.Weapons.GrimReaper;
using Characters.Abilities.Weapons.Minotaurus;
using Characters.Abilities.Weapons.Skeleton_Sword;
using Characters.Abilities.Weapons.Wizard;
using Characters.Abilities.Weapons.Yaksha;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000998 RID: 2456
	public abstract class AbilityComponent : MonoBehaviour
	{
		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060034D0 RID: 13520
		public abstract IAbility ability { get; }

		// Token: 0x060034D1 RID: 13521
		public abstract void Initialize();

		// Token: 0x060034D2 RID: 13522
		public abstract IAbilityInstance CreateInstance(Character owner);

		// Token: 0x02000999 RID: 2457
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060034D4 RID: 13524 RVA: 0x0009B8BC File Offset: 0x00099ABC
			static SubcomponentAttribute()
			{
				int length = typeof(AbilityComponent).Namespace.Length;
				AbilityComponent.SubcomponentAttribute.names = new string[AbilityComponent.SubcomponentAttribute.types.Length];
				for (int i = 0; i < AbilityComponent.SubcomponentAttribute.names.Length; i++)
				{
					Type type = AbilityComponent.SubcomponentAttribute.types[i];
					if (type == null)
					{
						string text = AbilityComponent.SubcomponentAttribute.names[i - 1];
						int num = text.LastIndexOf('/');
						if (num == -1)
						{
							AbilityComponent.SubcomponentAttribute.names[i] = string.Empty;
						}
						else
						{
							AbilityComponent.SubcomponentAttribute.names[i] = text.Substring(0, num + 1);
						}
					}
					else
					{
						AbilityComponent.SubcomponentAttribute.names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
					}
				}
			}

			// Token: 0x060034D5 RID: 13525 RVA: 0x0009C802 File Offset: 0x0009AA02
			public SubcomponentAttribute() : base(true, AbilityComponent.SubcomponentAttribute.types, AbilityComponent.SubcomponentAttribute.names)
			{
			}

			// Token: 0x04002A87 RID: 10887
			public new static readonly Type[] types = new Type[]
			{
				typeof(NothingComponent),
				null,
				typeof(StatBonusComponent),
				typeof(StackableStatBonusComponent),
				typeof(StackableStatBonusByTimeComponent),
				typeof(StackableStatBonusByLostHealthComponent),
				typeof(TimeGradientStatBonusComponent),
				null,
				typeof(StatBonusByAirTimeComponent),
				typeof(StatBonusByMovingComponent),
				typeof(StatBonusByShieldComponent),
				typeof(StatBonusByIncomeComponent),
				typeof(StatBonusByOutcomeComponent),
				typeof(StatBonusByBalanceComponent),
				typeof(StatBonusBySkillsInCooldownComponent),
				typeof(StatBonusByOtherStatComponent),
				typeof(StatBonusByGaveDamageComponent),
				typeof(StatBonusByKillComponent),
				typeof(StatBonusAndTimeBonusByTriggerComponent),
				null,
				typeof(StatByApplyingStatusCountsWithinRangeComponent),
				typeof(StatByCountsWithinRangeComponent),
				typeof(StatBonusByTargetStatusComponent),
				null,
				typeof(StatBonusByMaxHealthComponent),
				typeof(StatPerCurrentHealthComponent),
				typeof(StatPerLostHealthComponent),
				typeof(StatBonusPerGearRarityComponent),
				typeof(StatBonusPerGearTagComponent),
				typeof(StatBonusByInscriptionCountComponent),
				typeof(StatBonusPerInscriptionStackComponent),
				typeof(StatBonusPerHealComponent),
				null,
				typeof(StatBonusCooldownableComponent),
				null,
				typeof(StatDebuffOnStatusComponent),
				typeof(OverrideFinalStatValuesComponent),
				typeof(UniqueStatBonusComponent),
				null,
				typeof(MassacreStatBonusComponent),
				null,
				typeof(AttachAbilityOnGaveDamageComponent),
				typeof(CurseOfLightComponent),
				typeof(GiveCurseOfLightComponent),
				null,
				typeof(OmenMisfortuneComponent),
				typeof(OmenSunAndMoonComponent),
				typeof(OmenExecutionComponent),
				typeof(OmenManaCycleComponent),
				typeof(OmenArmsComponent),
				typeof(FenrirFangComponent),
				typeof(GoludaluSummonBookComponent),
				typeof(GiantsCavalryComponent),
				typeof(SymbolOfToughnessComponent),
				typeof(BigBloodComponent),
				typeof(AccelerationSwordComponent),
				typeof(HealthScreeningsMachineComponent),
				typeof(BloodEatingSwordComponent),
				typeof(MiracleGrailComponent),
				typeof(GreatHerosArmorComponent),
				typeof(ManeOfBeastKingComponent),
				typeof(VictoryBatonComponent),
				typeof(StoneMaskComponent),
				typeof(CrownOfThornsComponent),
				typeof(StigmaLegComponent),
				typeof(WindMailComponent),
				typeof(CowardlyCloakComponent),
				typeof(FrostGiantsLeatherComponent),
				typeof(SylphidsWingComponent),
				typeof(MasterPieceBerserkersGloveComponent),
				typeof(ChosenHerosCircletComponent),
				typeof(ChosenWarriorsArmorComponent),
				typeof(ChosenThiefDaggersComponent),
				typeof(ChosenClericsBibleComponent),
				typeof(CupOfFateComponent),
				typeof(OldSpellBookCoverComponent),
				typeof(DarkPriestsRobesShieldComponent),
				typeof(GraceOfLeoniaComponent),
				typeof(FightersBeltComponent),
				null,
				typeof(DwarfComponent),
				typeof(KirizComponent),
				typeof(EvileEyeComponent),
				typeof(CharmComponent),
				typeof(FaceBugComponent),
				typeof(SpectorOwnerComponent),
				typeof(PrisonerPassiveComponent),
				typeof(PrisonerChestPassiveComponent),
				typeof(PrisonerLancePassiveComponent),
				typeof(YakshaHomePassiveComponent),
				typeof(GhoulPassive2Component),
				typeof(GrimReaperPassiveComponent),
				typeof(GrimReaperHarvestPassiveComponent),
				typeof(MinotaurusPassiveComponent),
				typeof(ChallengerMarkPassiveComponent),
				typeof(StatBonusByEntGassCountComponent),
				typeof(Skeleton_SwordPassiveComponent),
				typeof(Skeleton_SwordPassiveTetanusComponent),
				typeof(Skeleton_SwordTetanusDamageComponent),
				typeof(WizardPassiveComponent),
				typeof(WizardManaChargingSpeedBonusComponent),
				typeof(WizardTranscendenceComponent),
				typeof(DavyJonesPassiveComponent),
				typeof(LazinessOfGeniusComponent),
				typeof(PlagueComponent),
				typeof(KettleOfSwampWitchComponent),
				typeof(SpotlightRewardComponent),
				typeof(GreatMountainForceComponent),
				typeof(GluttonComponent),
				typeof(BoneShieldComponent),
				typeof(StatusRegrantComponent),
				typeof(TimeChargingStatBonusComponent),
				typeof(OneHitSkillDamageComponent),
				typeof(OneHitSkillDamageMarkingComponent),
				typeof(ManyHitMarkComponent),
				typeof(TimeBombGiverComponent),
				typeof(TimeBombComponent),
				typeof(WhaleboneAmuletComponent),
				typeof(ThornsArmorComponent),
				typeof(MightGuyComponent),
				typeof(AngrySightComponent),
				typeof(DamageAttributeChangeComponent),
				typeof(HealthArmorComponnet),
				typeof(RecklessPostureComponent),
				typeof(ObsessiveCompulsiveComponent),
				typeof(KingSlayerComponent),
				typeof(LivingArmorComponent),
				typeof(GhostStoriesComponent),
				typeof(BallistaComponent),
				typeof(RechargeComponent),
				typeof(QuarantineComponent),
				typeof(ProtectionComponent),
				null,
				typeof(DotDamageComponent),
				typeof(StackableModifyTakingDamageComponent),
				typeof(DamageMissionComponent),
				typeof(ReverseHorizontalInputComponent),
				typeof(BlockCriticalComponent),
				null,
				typeof(RandomAbilitiesComponent),
				typeof(AirAndGroundAttackDamageComponent),
				typeof(ExtraDamageToBackComponent),
				typeof(ModifyDamageByStackResolverComponent),
				typeof(ModifyTakingDamageComponent),
				typeof(ModifyDamageComponent),
				typeof(ModifyDamageStackableComponent),
				typeof(ModifyDamageByDistanceComponent),
				typeof(ModifyDamageByTargetLayerComponent),
				typeof(ModifyDamageByTargetSizeComponent),
				typeof(ModifyTrapDamageComponent),
				typeof(ModifyDamageByDashDistanceComponent),
				typeof(ModifyDamageOnStatusComponent),
				typeof(ModifyActionSpeedComponent),
				typeof(ModifyDamageByOwnerAndTargetHealthComponent),
				typeof(ModifyStatusDurationComponent),
				typeof(ModifyDamageByStatComponent),
				typeof(ChangeTakingDamageToOneComponent),
				typeof(IgnoreShieldOverDamageComponent),
				typeof(IgnoreTakingDamageByDirectionComponent),
				typeof(IgnoreTrapDamageComponent),
				typeof(CriticalToMaximumHealthComponent),
				null,
				typeof(AddAirJumpCountComponent),
				typeof(AttachAbilityToTargetOnGaveDamageComponent),
				typeof(AddGaugeValueOnGaveDamageComponent),
				typeof(AddMaxGaugeValueComponent),
				typeof(StackableAdditionalHitComponent),
				typeof(AdditionalHitComponent),
				typeof(AdditionalHitOnStatusTriggerComponent),
				typeof(AdditionalHitByTargetStatusComponent),
				typeof(AdditionalHitToStatusTakerComponent),
				typeof(ApplyStatusOnGaveDamageComponent),
				typeof(ApplyStatusOnGaveEmberDamageComponent),
				typeof(ApplyStatusOnApplyStatusComponent),
				typeof(AttachAbilityWithinColliderComponent),
				typeof(AttachAbilityToMinionWithinColliderComponent),
				typeof(AttachAbilityToStatusTargetComponent),
				typeof(DetachAbilityByTriggerComponent),
				null,
				typeof(ReduceCooldownByTriggerComponent),
				typeof(ReduceSwapCooldownByTriggerComponent),
				typeof(ReduceCooldownAnotherSkillComponent),
				typeof(IgnoreSkillCooldownComponent),
				null,
				typeof(ChangeActionComponent),
				typeof(CurrencyBonusComponent),
				typeof(EnhanceComboActionComponent),
				typeof(GetInvulnerableComponent),
				typeof(GetLockoutComponent),
				typeof(GetEvasionComponent),
				typeof(GetEvasionShieldComponent),
				typeof(GetSilenceComponent),
				typeof(GetGuardComponent),
				typeof(GiveStatusOnGaveDamageComponent),
				typeof(ModifyTimeScaleComponent),
				typeof(OperationByTriggerComponent),
				typeof(OperationByTriggersComponent),
				typeof(OperationOnGuardMotionComponent),
				typeof(RunTargetOperationOnGaveDamageComponent),
				typeof(RunTargetOperationOnGiveDamageComponent),
				typeof(OverrideMovementConfigComponent),
				typeof(PeriodicHealComponent),
				typeof(ShieldComponent),
				typeof(StackableShieldComponent),
				typeof(ShieldByCountWithinRangeComponent),
				typeof(WeaponPolymorphComponent),
				typeof(ReviveComponent),
				typeof(HitBombComponent),
				null,
				typeof(AttachToOneTargetOnGaveDamageComponent),
				null,
				typeof(AlchemistGaugeBoostComponent),
				typeof(AlchemistGaugeDeactivateComponent),
				typeof(AlchemistPassiveComponent),
				null,
				typeof(RiderPassiveComponent),
				typeof(RiderSkeletonRiderComponent),
				null,
				typeof(ThiefPassiveComponent),
				typeof(SpawnThiefGoldOnTookDamageComponent),
				typeof(SpawnThiefGoldOnGaveDamageComponent),
				null,
				typeof(MummyPassiveComponent),
				typeof(MummyGunDropPassiveComponent),
				null,
				typeof(BombSkulPassiveComponent),
				typeof(ArchlichSoulLootingPassiveComponent),
				typeof(AwakenDarkPaladinPassiveComponent),
				typeof(Berserker2PassiveComponent),
				typeof(GhoulPassiveComponent),
				typeof(LivingArmorPassiveComponent),
				typeof(RecruitPassiveComponent),
				typeof(RockstarPassiveComponent),
				typeof(SamuraiPassive2Component),
				null,
				typeof(BoneOfBraveComponent),
				typeof(BoneOfManaComponent),
				typeof(BoneOfSpeedComponent),
				typeof(CriticalChanceByDistanceComponent),
				typeof(MagesManaBraceletComponent),
				typeof(MedalOfCarleonComponent),
				typeof(NonConsumptionComponent),
				null,
				typeof(Skeleton_ShieldExplosionPassiveComponent),
				typeof(Skeleton_Shield4GuardComponent),
				null,
				typeof(ElderEntsGratitudeComponent),
				typeof(OffensiveWheelComponent),
				typeof(GoldenManeRapierComponent),
				typeof(ForbiddenSwordComponent),
				typeof(ChimerasFangComponent),
				typeof(UnknownSeedComponent),
				typeof(DoomsdayComponent),
				typeof(LeoniasGraceComponent),
				typeof(CretanBullComponent),
				typeof(AttentivenessComponent),
				typeof(SpecterComponent),
				typeof(GraveDiggerPassiveComponent),
				typeof(EssenceRecruitPassiveComponent),
				typeof(ProjectileCountComponent),
				typeof(EmptyPotionComponent),
				typeof(MagicWandComponent),
				typeof(HotTagComponent)
			};

			// Token: 0x04002A88 RID: 10888
			public new static readonly string[] names;
		}

		// Token: 0x0200099A RID: 2458
		[Serializable]
		public class Subcomponents : SubcomponentArray<AbilityComponent>
		{
			// Token: 0x060034D6 RID: 13526 RVA: 0x0009C818 File Offset: 0x0009AA18
			public void Initialize()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Initialize();
				}
			}
		}
	}
}
