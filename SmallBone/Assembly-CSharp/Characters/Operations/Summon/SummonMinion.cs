using System;
using Characters.Minions;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F3C RID: 3900
	public class SummonMinion : CharacterOperation
	{
		// Token: 0x06004BE0 RID: 19424 RVA: 0x000DF2A0 File Offset: 0x000DD4A0
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			if (this._spawnPositions.Length == 0)
			{
				this.Spawn(owner, owner.transform.position);
				return;
			}
			foreach (Transform transform in this._spawnPositions)
			{
				this.Spawn(owner, transform.position);
			}
		}

		// Token: 0x06004BE1 RID: 19425 RVA: 0x000DF2F8 File Offset: 0x000DD4F8
		private void Spawn(Character owner, Vector3 position)
		{
			if (this._snapToGround)
			{
				RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, this._groundFindingDistance, Layers.groundMask);
				if (hit)
				{
					position = hit.point;
				}
			}
			Minion minion = owner.playerComponents.minionLeader.Summon(this._minion, position, this._overrideSetting);
			if (this._lookOwnerDirection)
			{
				minion.character.ForceToLookAt(owner.lookingDirection);
			}
			if (this._pushInfo != null)
			{
				minion.character.movement.push.ApplyKnockback(minion.character, this._pushInfo);
			}
		}

		// Token: 0x04003B0E RID: 15118
		[SerializeField]
		private Minion _minion;

		// Token: 0x04003B0F RID: 15119
		[SerializeField]
		private bool _lookOwnerDirection;

		// Token: 0x04003B10 RID: 15120
		[SerializeField]
		[Information("비워둘 경우 Default 설정 값을 적용", InformationAttribute.InformationType.Info, false)]
		private MinionSetting _overrideSetting;

		// Token: 0x04003B11 RID: 15121
		[SerializeField]
		[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
		private Transform[] _spawnPositions;

		// Token: 0x04003B12 RID: 15122
		[SerializeField]
		private PushInfo _pushInfo;

		// Token: 0x04003B13 RID: 15123
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x04003B14 RID: 15124
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;
	}
}
