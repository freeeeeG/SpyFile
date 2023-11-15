using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED2 RID: 3794
	public class ToBDLowHealthEnemy : Policy
	{
		// Token: 0x06004A82 RID: 19074 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x000D9518 File Offset: 0x000D7718
		public override Vector2 GetPosition()
		{
			Character character = this.GetLowHealthCharacter();
			if (character == null)
			{
				character = this._ownerValue;
			}
			return character.transform.position;
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x000D954C File Offset: 0x000D774C
		private Character GetLowHealthCharacter()
		{
			SharedCharacter sharedCharacter = this._tree.GetVariable(this._ownerValueName) as SharedCharacter;
			this._ownerValue = sharedCharacter.Value;
			List<Character> list = this.FindEnemiesInRange(this._findRange);
			double num = 1.0;
			Character result = null;
			foreach (Character character in list)
			{
				if (character.liveAndActive && (this._includeSelf || !(character == this._ownerValue)) && character.health.percent < num)
				{
					num = character.health.percent;
					result = character;
				}
			}
			return result;
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x000D9610 File Offset: 0x000D7810
		public List<Character> FindEnemiesInRange(Collider2D collider)
		{
			collider.enabled = true;
			NonAllocOverlapper nonAllocOverlapper = new NonAllocOverlapper(31);
			nonAllocOverlapper.contactFilter.SetLayerMask(1024);
			List<Character> components = nonAllocOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
			if (this._optimizedCollider)
			{
				collider.enabled = false;
			}
			return components;
		}

		// Token: 0x040039A1 RID: 14753
		[SerializeField]
		private BehaviorTree _tree;

		// Token: 0x040039A2 RID: 14754
		[SerializeField]
		private string _ownerValueName = "Owner";

		// Token: 0x040039A3 RID: 14755
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x040039A4 RID: 14756
		[SerializeField]
		private bool _includeSelf = true;

		// Token: 0x040039A5 RID: 14757
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x040039A6 RID: 14758
		private Character _ownerValue;
	}
}
