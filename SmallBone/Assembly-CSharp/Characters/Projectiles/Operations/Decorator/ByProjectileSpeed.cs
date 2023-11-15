using System;
using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007A7 RID: 1959
	public sealed class ByProjectileSpeed : Operation
	{
		// Token: 0x06002802 RID: 10242 RVA: 0x00079114 File Offset: 0x00077314
		public override void Run(IProjectile projectile)
		{
			if (this._checkDuration == 0f)
			{
				if (this.CheckHorizontal(projectile) && this.CheckVertical(projectile))
				{
					this._operations.Run(projectile);
					return;
				}
			}
			else
			{
				this._coroutineReference.Stop();
				this._coroutineReference = this.StartCoroutineWithReference(this.CRun(projectile));
			}
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x0007916B File Offset: 0x0007736B
		private IEnumerator CRun(IProjectile projectile)
		{
			float remainTime = this._checkDuration;
			yield return null;
			Debug.Log("Run");
			while (remainTime > 0f)
			{
				if (this.CheckHorizontal(projectile) && this.CheckVertical(projectile))
				{
					this._operations.Run(projectile);
					yield break;
				}
				remainTime -= Chronometer.global.deltaTime;
				yield return null;
			}
			Debug.Log("End");
			yield break;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x00079184 File Offset: 0x00077384
		private bool CheckHorizontal(IProjectile projectile)
		{
			switch (this._horizontalComparer)
			{
			case ByProjectileSpeed.Comparer.NotUsed:
				return true;
			case ByProjectileSpeed.Comparer.GreaterThanOrEqual:
				if (projectile.firedDirection.x * projectile.speed >= this._horizontal)
				{
					return true;
				}
				break;
			case ByProjectileSpeed.Comparer.LessThan:
				if (projectile.firedDirection.x * projectile.speed <= this._horizontal)
				{
					return true;
				}
				break;
			}
			Debug.Log(string.Format("Horizontal : {0}", projectile.firedDirection.x * projectile.speed));
			return false;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x00079210 File Offset: 0x00077410
		private bool CheckVertical(IProjectile projectile)
		{
			switch (this._verticalComparer)
			{
			case ByProjectileSpeed.Comparer.NotUsed:
				return true;
			case ByProjectileSpeed.Comparer.GreaterThanOrEqual:
				if (projectile.firedDirection.y * projectile.speed >= this._vertical)
				{
					return true;
				}
				break;
			case ByProjectileSpeed.Comparer.LessThan:
				if (projectile.firedDirection.y * projectile.speed <= this._vertical)
				{
					return true;
				}
				break;
			}
			return false;
		}

		// Token: 0x04002241 RID: 8769
		[SerializeField]
		private float _checkDuration;

		// Token: 0x04002242 RID: 8770
		[SerializeField]
		[Header("조건")]
		private float _horizontal;

		// Token: 0x04002243 RID: 8771
		[SerializeField]
		private ByProjectileSpeed.Comparer _horizontalComparer;

		// Token: 0x04002244 RID: 8772
		[SerializeField]
		private float _vertical;

		// Token: 0x04002245 RID: 8773
		[SerializeField]
		private ByProjectileSpeed.Comparer _verticalComparer;

		// Token: 0x04002246 RID: 8774
		[SerializeField]
		[Operation.SubcomponentAttribute]
		private Operation.Subcomponents _operations;

		// Token: 0x04002247 RID: 8775
		private CoroutineReference _coroutineReference;

		// Token: 0x020007A8 RID: 1960
		private enum Comparer
		{
			// Token: 0x04002249 RID: 8777
			NotUsed,
			// Token: 0x0400224A RID: 8778
			GreaterThanOrEqual,
			// Token: 0x0400224B RID: 8779
			LessThan
		}
	}
}
