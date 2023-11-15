using System;
using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using Level;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001263 RID: 4707
	public class KilivanFinish : MonoBehaviour
	{
		// Token: 0x17001280 RID: 4736
		// (get) Token: 0x06005D50 RID: 23888 RVA: 0x001129FD File Offset: 0x00110BFD
		// (set) Token: 0x06005D51 RID: 23889 RVA: 0x00112A05 File Offset: 0x00110C05
		public bool running { get; set; }

		// Token: 0x06005D52 RID: 23890 RVA: 0x00112A0E File Offset: 0x00110C0E
		private void Awake()
		{
			this._fireOperations.Initialize();
			this._hitOperations.Initialize();
			this._sweepAttack.Initialize();
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x00112A31 File Offset: 0x00110C31
		public IEnumerator CFire()
		{
			this.running = true;
			this._direction = ((this._owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
			yield return this.CMove(Singleton<Service>.Instance.levelManager.player.transform.position);
			yield break;
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x00112A40 File Offset: 0x00110C40
		public Vector2 Fire(Vector2 direction)
		{
			this._direction = direction;
			this._projectile.transform.position = this._firePosition.position;
			float num = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			if (this._owner.lookingDirection == Character.LookingDirection.Left)
			{
				num += 180f;
			}
			this._projectile.transform.rotation = Quaternion.Euler(0f, 0f, num);
			this._sweepAttack.Run(this._owner);
			this.Show();
			RaycastHit2D raycastHit2D;
			Singleton<Service>.Instance.levelManager.player.movement.TryBelowRayCast(262144, out raycastHit2D, 100f);
			return raycastHit2D.point;
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x00112B08 File Offset: 0x00110D08
		public bool UpdateMove(float deltaTime, Vector2 destination)
		{
			float num = this._speed * deltaTime;
			if (!this.DetectCollision(destination, num))
			{
				this._projectile.Translate(this._direction * num, Space.World);
				return true;
			}
			this._sweepAttack.Stop();
			this.Hide();
			this.running = false;
			return false;
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x00112B60 File Offset: 0x00110D60
		private IEnumerator CMove(Vector2 destination)
		{
			this._projectile.transform.position = this._firePosition.position;
			this._sweepAttack.Run(this._owner);
			this.Show();
			float num = this._speed * Chronometer.global.deltaTime;
			while (!this.DetectCollision(destination, num))
			{
				yield return null;
				num = this._speed * Chronometer.global.deltaTime;
				this._projectile.Translate(this._direction * num, Space.World);
			}
			this._sweepAttack.Stop();
			this.Hide();
			this.running = false;
			yield break;
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x00112B78 File Offset: 0x00110D78
		private bool DetectCollision(Vector2 destination, float speed)
		{
			float x = this._projectile.transform.position.x;
			if ((this._direction.x >= 0f && x > destination.x) || (this._direction.x <= 0f && x < destination.x))
			{
				this.OnCollision(destination);
				return true;
			}
			RaycastHit2D hit = Physics2D.Raycast(this._projectile.transform.position, this._direction, speed, this._collision);
			if (hit)
			{
				this.OnCollision(hit.point);
				return true;
			}
			return false;
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x00112C20 File Offset: 0x00110E20
		private void OnCollision(Vector2 hitPoint)
		{
			this._projectile.transform.position = hitPoint;
			this._hitOperations.gameObject.SetActive(true);
			this._hitOperations.Run(this._owner);
			float x = hitPoint.x;
			this.Evaluate(ref x);
			hitPoint = new Vector2(x, hitPoint.y);
			this._owner.movement.controller.TeleportUponGround(hitPoint, 3f);
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x00112CA0 File Offset: 0x00110EA0
		private void Evaluate(ref float x)
		{
			float num = Map.Instance.bounds.max.x - this._owner.collider.bounds.size.x;
			float num2 = Map.Instance.bounds.min.x + this._owner.collider.bounds.size.x;
			if (x > num)
			{
				x = num;
			}
			if (x < num2)
			{
				x = num2;
			}
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x00112D29 File Offset: 0x00110F29
		private void Show()
		{
			this._projectile.gameObject.SetActive(true);
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x00112D3C File Offset: 0x00110F3C
		private void Hide()
		{
			this._projectile.gameObject.SetActive(false);
		}

		// Token: 0x04004AE7 RID: 19175
		[SerializeField]
		private Character _owner;

		// Token: 0x04004AE8 RID: 19176
		[SerializeField]
		private float _speed;

		// Token: 0x04004AE9 RID: 19177
		[SerializeField]
		private LayerMask _collision;

		// Token: 0x04004AEA RID: 19178
		[SerializeField]
		private Transform _firePosition;

		// Token: 0x04004AEB RID: 19179
		[SerializeField]
		private Transform _projectile;

		// Token: 0x04004AEC RID: 19180
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _fireOperations;

		// Token: 0x04004AED RID: 19181
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _hitOperations;

		// Token: 0x04004AEE RID: 19182
		[Subcomponent(typeof(SweepAttack))]
		[SerializeField]
		private SweepAttack _sweepAttack;

		// Token: 0x04004AEF RID: 19183
		private Vector2 _direction;
	}
}
