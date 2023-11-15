using System;
using System.Collections;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E75 RID: 3701
	public class TeleportOverTime : CharacterOperation
	{
		// Token: 0x06004961 RID: 18785 RVA: 0x000D6728 File Offset: 0x000D4928
		public override void Run(Character owner)
		{
			if (this._curve.duration == 0f)
			{
				Debug.LogError("The duration of the curve is zero. Set it higher than 0 or use teleport operation.");
				return;
			}
			this.Stop();
			base.StartCoroutine(this.CTeleportOverTime(owner));
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x000D675B File Offset: 0x000D495B
		private Vector2 GetPosition(Collider2D collider, Transform transform)
		{
			if (collider != null)
			{
				return MMMaths.RandomPointWithinBounds(collider.bounds);
			}
			if (transform != null)
			{
				return transform.position;
			}
			Debug.LogError("Both of collider and transform are null on teleport over time.");
			return Vector2.zero;
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x000D6796 File Offset: 0x000D4996
		private void Teleport(Character target, Vector2 destination)
		{
			if (this._maxRetryDistance > 0f)
			{
				target.movement.controller.Teleport(destination, this._maxRetryDistance);
				return;
			}
			target.movement.controller.Teleport(destination);
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x000D67D0 File Offset: 0x000D49D0
		private IEnumerator CTeleportOverTime(Character target)
		{
			Transform transform = (this._startPoint == null) ? target.transform : this._startPoint;
			Vector2 startPosition = this.GetPosition(this._startRange, transform);
			Vector2 endPosition = this.GetPosition(this._endRange, this._endPoint);
			float time = 0f;
			float duration = this._curve.duration;
			target.movement.configs.Add(int.MaxValue, this._staticMovementConfig);
			while (time < duration)
			{
				time += target.chronometer.master.deltaTime;
				Vector2 destination = Vector2.LerpUnclamped(startPosition, endPosition, time / duration);
				this.Teleport(target, destination);
				yield return null;
			}
			this.Teleport(target, endPosition);
			target.movement.configs.Remove(this._staticMovementConfig);
			this._currentTarget = null;
			yield break;
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x000D67E6 File Offset: 0x000D49E6
		public override void Stop()
		{
			base.StopAllCoroutines();
			if (this._currentTarget != null)
			{
				this._currentTarget.movement.configs.Remove(this._staticMovementConfig);
				this._currentTarget = null;
			}
		}

		// Token: 0x04003897 RID: 14487
		private Movement.Config _staticMovementConfig = new Movement.Config(Movement.Config.Type.Static);

		// Token: 0x04003898 RID: 14488
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003899 RID: 14489
		[Information("0이상이면 텔레포트 실패 시 거리 1마다 재시도, 이동에 특별한 문제가 없는 한 0으로 유지.", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _maxRetryDistance;

		// Token: 0x0400389A RID: 14490
		[Header("Start")]
		[Information("둘 다 비워두면 캐릭터의 현재 위치 사용", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Collider2D _startRange;

		// Token: 0x0400389B RID: 14491
		[SerializeField]
		private Transform _startPoint;

		// Token: 0x0400389C RID: 14492
		[Header("End")]
		[SerializeField]
		private Collider2D _endRange;

		// Token: 0x0400389D RID: 14493
		[SerializeField]
		private Transform _endPoint;

		// Token: 0x0400389E RID: 14494
		private Character _currentTarget;
	}
}
