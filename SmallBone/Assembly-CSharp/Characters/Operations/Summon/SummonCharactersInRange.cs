using System;
using Characters.Operations.Summon.SummonInRange.Policy;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F38 RID: 3896
	public class SummonCharactersInRange : CharacterOperation
	{
		// Token: 0x06004BCB RID: 19403 RVA: 0x000DEE34 File Offset: 0x000DD034
		private void Awake()
		{
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.SetLayerMask(256);
			this._rayCaster = new RayCaster
			{
				contactFilter = contactFilter,
				origin = base.transform.position,
				distance = this._summonRange / 2f
			};
		}

		// Token: 0x06004BCC RID: 19404 RVA: 0x000DEE94 File Offset: 0x000DD094
		public override void Run(Character owner)
		{
			if (this._characters.Length == 0)
			{
				return;
			}
			this._rayCaster.direction = Vector2.left;
			RaycastHit2D hit = this._rayCaster.SingleCast();
			float num = 0f;
			if (hit)
			{
				num += hit.distance;
			}
			else
			{
				num += this._summonRange / 2f;
			}
			this._rayCaster.direction = Vector2.right;
			hit = this._rayCaster.SingleCast();
			float num2 = 0f;
			if (hit)
			{
				num2 += hit.distance;
			}
			else
			{
				num2 += this._summonRange / 2f;
			}
			Vector2 originPosition = owner.transform.position;
			originPosition.x += (num2 - num) / 2f;
			for (int i = 0; i < this._characters.Length; i++)
			{
				Vector2 position = this._positionPolicy.GetPosition(originPosition, num + num2, this._characters.Length, i);
				Character character = UnityEngine.Object.Instantiate<Character>(this._characters[i], position, Quaternion.identity, Map.Instance.transform);
				IBDCharacterSetting[] settings = this._settings;
				for (int j = 0; j < settings.Length; j++)
				{
					settings[j].ApplyTo(character);
				}
			}
		}

		// Token: 0x04003AFE RID: 15102
		[SerializeField]
		private Character[] _characters;

		// Token: 0x04003AFF RID: 15103
		[SerializeField]
		private float _summonRange = 8f;

		// Token: 0x04003B00 RID: 15104
		[SerializeReference]
		[SubclassSelector]
		[Header("소환위치 세팅")]
		private ISummonPosition _positionPolicy;

		// Token: 0x04003B01 RID: 15105
		[SerializeReference]
		[SubclassSelector]
		[Header("소환체 세팅")]
		private IBDCharacterSetting[] _settings;

		// Token: 0x04003B02 RID: 15106
		private RayCaster _rayCaster;
	}
}
