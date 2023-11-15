using System;
using System.Collections.Generic;
using Characters.Operations.Attack;
using Characters.Operations.BehaviorDesigner;
using Characters.Operations.Customs;
using Characters.Operations.Customs.GlacialSkull;
using Characters.Operations.Customs.GrimReaper;
using Characters.Operations.Decorator;
using Characters.Operations.Decorator.Deprecated;
using Characters.Operations.Essences;
using Characters.Operations.Fx;
using Characters.Operations.Gauge;
using Characters.Operations.Health;
using Characters.Operations.Movement;
using Characters.Operations.ObjectTransform;
using Characters.Operations.Summon;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E45 RID: 3653
	public abstract class TargetedCharacterOperation : MonoBehaviour
	{
		// Token: 0x060048A9 RID: 18601 RVA: 0x000D3A74 File Offset: 0x000D1C74
		static TargetedCharacterOperation()
		{
			int length = typeof(CharacterOperation).Namespace.Length;
			TargetedCharacterOperation.names = new string[TargetedCharacterOperation.types.Length];
			for (int i = 0; i < TargetedCharacterOperation.names.Length; i++)
			{
				Type type = TargetedCharacterOperation.types[i];
				if (type == null)
				{
					string text = TargetedCharacterOperation.names[i - 1];
					int num = text.LastIndexOf('/');
					if (num == -1)
					{
						TargetedCharacterOperation.names[i] = string.Empty;
					}
					else
					{
						TargetedCharacterOperation.names[i] = text.Substring(0, num + 1);
					}
				}
				else
				{
					TargetedCharacterOperation.names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
				}
			}
		}

		// Token: 0x060048AA RID: 18602 RVA: 0x00002191 File Offset: 0x00000391
		public virtual void Initialize()
		{
		}

		// Token: 0x060048AB RID: 18603
		public abstract void Run(Character owner, Character target);

		// Token: 0x060048AC RID: 18604 RVA: 0x000D40FC File Offset: 0x000D22FC
		public virtual void Run(Character owner, IList<Character> targets)
		{
			for (int i = 0; i < targets.Count; i++)
			{
				this.Run(owner, targets[i]);
			}
		}

		// Token: 0x060048AD RID: 18605 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x040037C1 RID: 14273
		public static readonly Type[] types = new Type[]
		{
			typeof(InstantAttack),
			typeof(InstantAttack2),
			null,
			typeof(SweepAttack),
			typeof(SweepAttack2),
			null,
			typeof(CastAttack),
			null,
			typeof(FireProjectile),
			typeof(FireProjectileInBounds),
			typeof(FireBouncyProjectile),
			typeof(MultipleFireProjectile),
			typeof(BreakPlayerShield),
			typeof(CameraShake),
			typeof(CameraShakeCurve),
			typeof(CameraZoom),
			null,
			typeof(PlayMusic),
			typeof(PauseBackgroundMusic),
			typeof(StopMusic),
			typeof(PlaySound),
			null,
			typeof(MotionTrail),
			typeof(ScreenFlash),
			typeof(ShaderEffect),
			typeof(SpawnEffect),
			typeof(SpawnEffectOnScreen),
			null,
			typeof(Vignette),
			typeof(Vibration),
			typeof(SpawnLineText),
			typeof(SpawnEnemyLineText),
			typeof(Repeater),
			typeof(Repeater2),
			typeof(Chance),
			typeof(Characters.Operations.Decorator.Random),
			typeof(SeedRandom),
			typeof(HealthFilter),
			typeof(CharacterTypeFilter),
			typeof(Sequencer),
			typeof(InHardmode),
			typeof(RandomlyRunningOperation),
			typeof(SummonCharacter),
			typeof(SummonMinion),
			typeof(SummonOperationRunner),
			typeof(SummonMultipleOperationRunners),
			typeof(SummonOperationRunnerAtTarget),
			typeof(ApplyStatusFromEssenceOwner),
			typeof(Move),
			typeof(DualMove),
			null,
			typeof(Knockback),
			typeof(KnockbackTo),
			typeof(Smash),
			typeof(SmashTo),
			null,
			typeof(GrabTo),
			null,
			typeof(ChangeGravity),
			typeof(ModifyVerticalVelocity),
			typeof(OverrideMovementConfig),
			typeof(SetCharacterColliderLayerMask),
			typeof(Jump),
			typeof(JumpDown),
			typeof(Teleport),
			typeof(TeleportOverTime),
			typeof(MoveTransform),
			typeof(MoveTransformFromPosition),
			typeof(ResetGlobalTransformToLocal),
			typeof(RotateAngle),
			typeof(SetPositionTo),
			typeof(Heal),
			typeof(Invulnerable),
			typeof(Suicide),
			typeof(Kill),
			typeof(AddGaugeValue),
			typeof(SetGaugeValue),
			typeof(ClearGaugeValue),
			typeof(SetBDVariableToTarget),
			typeof(ModifyTimeScale),
			typeof(ApplyStatus),
			typeof(AddMarkStack),
			null,
			typeof(AttachCurseOfLight),
			typeof(AttachAbility),
			typeof(AttachAbilityWithinCollider),
			null,
			typeof(Polymorph),
			typeof(ReduceCooldownTime),
			typeof(SetRemainCooldownTime),
			null,
			typeof(ActivateGameObjectOperation),
			typeof(DeactivateGameObject),
			null,
			typeof(SpawnCharacter),
			typeof(SpawnRandomCharacter),
			typeof(DestoryCharacter),
			typeof(SetCharacterVisible),
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
			typeof(SpawnGoldAtTarget),
			typeof(ConsumeGold),
			null,
			typeof(InvokeUnityEvent),
			typeof(LerpCollider),
			null,
			typeof(SetAirJumpCount),
			null,
			typeof(StopAnotherOperation),
			typeof(PrintDebugLog),
			typeof(AttachSilence),
			typeof(SpawnThiefGoldAtTarget),
			typeof(SpawnPropAtTarget),
			typeof(SlimeMagic),
			typeof(GrimReaperSentence3),
			typeof(AddFreezeRemainHitStack)
		};

		// Token: 0x040037C2 RID: 14274
		public static readonly string[] names;

		// Token: 0x040037C3 RID: 14275
		[NonSerialized]
		public float runSpeed = 1f;

		// Token: 0x02000E46 RID: 3654
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x060048AF RID: 18607 RVA: 0x000D413B File Offset: 0x000D233B
			public SubcomponentAttribute() : base(TargetedCharacterOperation.types, TargetedCharacterOperation.names)
			{
			}
		}

		// Token: 0x02000E47 RID: 3655
		[Serializable]
		public class Subcomponents : SubcomponentArray<TargetedCharacterOperation>
		{
			// Token: 0x060048B0 RID: 18608 RVA: 0x000D4150 File Offset: 0x000D2350
			public void Initialize()
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Initialize();
				}
			}

			// Token: 0x060048B1 RID: 18609 RVA: 0x000D4180 File Offset: 0x000D2380
			public void Run(Character owner, Character target)
			{
				for (int i = 0; i < base.components.Length; i++)
				{
					base.components[i].Run(owner, target);
				}
			}
		}
	}
}
