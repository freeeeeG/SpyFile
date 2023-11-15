using System;
using Characters;
using Characters.Abilities;
using Characters.Operations.Attack;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200065B RID: 1627
	public class DarkOrbWayPoint : MonoBehaviour
	{
		// Token: 0x060020B1 RID: 8369 RVA: 0x00062A84 File Offset: 0x00060C84
		private void Start()
		{
			if (this._waypointParent.childCount <= 0)
			{
				Debug.LogError("Waypoint Transform이 자식 오브젝트 가지고 있지 않습니다.");
				return;
			}
			this._abilityAttacher.Initialize(this._character);
			this._abilityAttacher.StartAttach();
			this._sweepAttack.Initialize();
			this._sweepAttack.Run(this._character);
			this._sweepAttackToEnemy.Initialize();
			this._sweepAttackToEnemy.Run(this._character);
			this._character.transform.position = this._waypointParent.GetChild(0).position;
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00062B1F File Offset: 0x00060D1F
		private void Update()
		{
			this.Move();
			this.UpdateWayPoint();
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00062B30 File Offset: 0x00060D30
		private void Move()
		{
			Vector3 vector = this._waypointParent.GetChild(this._currentCount).transform.position - this._character.transform.position;
			this._distance = vector.sqrMagnitude;
			this._character.movement.MoveHorizontal(vector.normalized);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00062B98 File Offset: 0x00060D98
		private void UpdateWayPoint()
		{
			if (this._distance > this.minimumDistanceFromDestination)
			{
				return;
			}
			DarkOrbWayPoint.LoopType rollbackType = this._rollbackType;
			if (rollbackType == DarkOrbWayPoint.LoopType.Loop)
			{
				int num = this._currentCount + 1;
				this._currentCount = num;
				this._currentCount = num % this._waypointParent.childCount;
				return;
			}
			if (rollbackType != DarkOrbWayPoint.LoopType.PingPong)
			{
				return;
			}
			if (this._currentCount == this._waypointParent.childCount - 1)
			{
				this._wayPointDelta = -1;
			}
			else if (this._currentCount == 0)
			{
				this._wayPointDelta = 1;
			}
			this._currentCount += this._wayPointDelta;
		}

		// Token: 0x04001BB4 RID: 7092
		[SerializeField]
		private DarkOrbWayPoint.LoopType _rollbackType;

		// Token: 0x04001BB5 RID: 7093
		[SerializeField]
		private Character _character;

		// Token: 0x04001BB6 RID: 7094
		[SerializeField]
		private SweepAttack _sweepAttack;

		// Token: 0x04001BB7 RID: 7095
		[SerializeField]
		private SweepAttack _sweepAttackToEnemy;

		// Token: 0x04001BB8 RID: 7096
		[SerializeField]
		private Transform _waypointParent;

		// Token: 0x04001BB9 RID: 7097
		[SerializeField]
		private float minimumDistanceFromDestination = 0.1f;

		// Token: 0x04001BBA RID: 7098
		[SerializeField]
		[AbilityAttacher.SubcomponentAttribute]
		private AbilityAttacher _abilityAttacher;

		// Token: 0x04001BBB RID: 7099
		private float _distance;

		// Token: 0x04001BBC RID: 7100
		private int _currentCount;

		// Token: 0x04001BBD RID: 7101
		private int _wayPointDelta = 1;

		// Token: 0x0200065C RID: 1628
		private enum LoopType
		{
			// Token: 0x04001BBF RID: 7103
			Loop,
			// Token: 0x04001BC0 RID: 7104
			PingPong
		}
	}
}
