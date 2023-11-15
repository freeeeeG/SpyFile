using System;
using Characters;
using Characters.Actions;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001463 RID: 5219
	[TaskDescription("커스텀 : 검은 초대용사 Rush 공격")]
	public sealed class DarkFirstHeroRush : BehaviorDesigner.Runtime.Tasks.Action
	{
		// Token: 0x060065EF RID: 26095 RVA: 0x0012671E File Offset: 0x0012491E
		public override void OnAwake()
		{
			this._caster = new NonAllocCaster(1);
			this._config = new Movement.Config(Movement.Config.Type.Flying);
			this._config.ignoreGravity = true;
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x00126744 File Offset: 0x00124944
		public override void OnStart()
		{
			Component value = this._target.Value;
			Character value2 = this._character.Value;
			this._elapsed = 0f;
			value2.movement.configs.Add(int.MaxValue, this._config);
			Vector2 vector = value.transform.position - value2.transform.position;
			this._caster.contactFilter.SetLayerMask(Layers.terrainMask);
			this._caster.RayCast(value2.transform.position + Vector2.up * 1.5f, vector, 30f);
			ReadonlyBoundedList<RaycastHit2D> results = this._caster.results;
			if (results.Count == 0)
			{
				return;
			}
			this._source = value2.transform.position;
			this._destination = results[0].point;
			this._action.TryStart();
			this._laser.Activate(value2, (value2.lookingDirection == Character.LookingDirection.Left) ? new Vector2(-vector.x, vector.y) : vector);
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x00126874 File Offset: 0x00124A74
		public override TaskStatus OnUpdate()
		{
			this._elapsed += Chronometer.global.deltaTime;
			if (this._elapsed < this._signTime)
			{
				return TaskStatus.Running;
			}
			this._laser.Deactivate();
			Character value = this._character.Value;
			if (this._elapsed < this._lerpTime + this._signTime)
			{
				Vector2 a = Vector2.Lerp(this._source, this._destination, this._elapsed / this._lerpTime);
				value.movement.force = a - value.transform.position;
			}
			else
			{
				value.movement.force = this._destination - value.transform.position;
			}
			if (!this._action.running)
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		// Token: 0x060065F2 RID: 26098 RVA: 0x0012694C File Offset: 0x00124B4C
		public override void OnEnd()
		{
			base.OnEnd();
			this._character.Value.movement.configs.Remove(this._config);
		}

		// Token: 0x040051DD RID: 20957
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x040051DE RID: 20958
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040051DF RID: 20959
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x040051E0 RID: 20960
		[SerializeField]
		private float _lerpTime;

		// Token: 0x040051E1 RID: 20961
		[SerializeField]
		private float _signTime;

		// Token: 0x040051E2 RID: 20962
		[SerializeField]
		private LineRenderer _lineRenderer;

		// Token: 0x040051E3 RID: 20963
		[SerializeField]
		private Laser _laser;

		// Token: 0x040051E4 RID: 20964
		private Vector2 _source;

		// Token: 0x040051E5 RID: 20965
		private Vector2 _destination;

		// Token: 0x040051E6 RID: 20966
		private NonAllocCaster _caster;

		// Token: 0x040051E7 RID: 20967
		private Movement.Config _config;

		// Token: 0x040051E8 RID: 20968
		private float _elapsed;
	}
}
