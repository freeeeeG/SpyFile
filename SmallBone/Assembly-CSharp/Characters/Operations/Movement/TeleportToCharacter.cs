using System;
using System.Collections.Generic;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E77 RID: 3703
	public class TeleportToCharacter : CharacterOperation
	{
		// Token: 0x0600496D RID: 18797 RVA: 0x000D69AE File Offset: 0x000D4BAE
		static TeleportToCharacter()
		{
			TeleportToCharacter._overlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x000D69E1 File Offset: 0x000D4BE1
		private void Awake()
		{
			if (this._optimizeFindingRange)
			{
				this._findingRange.enabled = false;
			}
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x000D69F8 File Offset: 0x000D4BF8
		public override void Run(Character owner)
		{
			Character character;
			using (new UsingCollider(this._findingRange, this._optimizeFindingRange))
			{
				character = TeleportToCharacter.FindTargetCharacter(owner.collider, this._findingRange, this._layer.Evaluate(owner.gameObject), this._findingMethod, this._onlyGroundedTarget);
			}
			if (character == null)
			{
				return;
			}
			bool flag = false;
			if (this._flipXOffsetByTargetDirection)
			{
				if (character.movement.config.type == Movement.Config.Type.Walking)
				{
					flag = (character.lookingDirection == Character.LookingDirection.Left);
				}
				else
				{
					flag = MMMaths.RandomBool();
				}
			}
			Vector2 destination = character.transform.position;
			Character.LookingDirection lookingDirection;
			if (flag)
			{
				destination.x -= this._xOffset;
				lookingDirection = Character.LookingDirection.Right;
			}
			else
			{
				destination.x += this._xOffset;
				lookingDirection = Character.LookingDirection.Left;
			}
			if (!owner.movement.controller.TeleportUponGround(destination, 4f) && !owner.movement.controller.Teleport(destination))
			{
				return;
			}
			owner.ForceToLookAt(lookingDirection);
			owner.movement.verticalVelocity = 0f;
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x000D6B20 File Offset: 0x000D4D20
		private static Character FindTargetCharacter(Collider2D origin, Collider2D range, LayerMask layerMask, TeleportToCharacter.FindingMethod findingMethod, bool onlyGroundedTarget)
		{
			TeleportToCharacter._overlapper.contactFilter.SetLayerMask(layerMask);
			List<Target> components = TeleportToCharacter._overlapper.OverlapCollider(range).GetComponents<Target>(true);
			if (components.Count == 0)
			{
				return null;
			}
			if (components.Count == 1)
			{
				return components[0].character;
			}
			TeleportToCharacter._characters.Clear();
			foreach (Target target in components)
			{
				if (!(target.character == null) && (!onlyGroundedTarget || target.character.movement.isGrounded))
				{
					TeleportToCharacter._characters.Add(target.character);
				}
			}
			if (TeleportToCharacter._characters.Count == 0)
			{
				return null;
			}
			if (TeleportToCharacter._characters.Count == 1)
			{
				return TeleportToCharacter._characters[0];
			}
			if (findingMethod == TeleportToCharacter.FindingMethod.Random)
			{
				return TeleportToCharacter._characters.Random<Character>();
			}
			if (findingMethod == TeleportToCharacter.FindingMethod.Closest)
			{
				float num = float.MaxValue;
				Character result = null;
				foreach (Character character in TeleportToCharacter._characters)
				{
					float distance = Physics2D.Distance(origin, character.collider).distance;
					if (num > distance)
					{
						result = character;
						num = distance;
					}
				}
				return result;
			}
			return null;
		}

		// Token: 0x040038A7 RID: 14503
		private const int _maxTargets = 32;

		// Token: 0x040038A8 RID: 14504
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(32);

		// Token: 0x040038A9 RID: 14505
		private static readonly List<Character> _characters = new List<Character>(32);

		// Token: 0x040038AA RID: 14506
		[Header("Find")]
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040038AB RID: 14507
		[SerializeField]
		private Collider2D _findingRange;

		// Token: 0x040038AC RID: 14508
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizeFindingRange = true;

		// Token: 0x040038AD RID: 14509
		[SerializeField]
		private TeleportToCharacter.FindingMethod _findingMethod;

		// Token: 0x040038AE RID: 14510
		[SerializeField]
		private bool _onlyGroundedTarget;

		// Token: 0x040038AF RID: 14511
		[SerializeField]
		[Header("Position")]
		private float _xOffset = 1f;

		// Token: 0x040038B0 RID: 14512
		[SerializeField]
		private bool _flipXOffsetByTargetDirection;

		// Token: 0x02000E78 RID: 3704
		public enum FindingMethod
		{
			// Token: 0x040038B2 RID: 14514
			Random,
			// Token: 0x040038B3 RID: 14515
			Closest
		}
	}
}
