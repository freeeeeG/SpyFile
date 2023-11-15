using System;
using System.Collections.Generic;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements
{
	// Token: 0x020007D7 RID: 2007
	[Serializable]
	public class TargetFinder
	{
		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x0007B709 File Offset: 0x00079909
		public Collider2D range
		{
			get
			{
				return this._range;
			}
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x0007B714 File Offset: 0x00079914
		internal void Initialize(IProjectile projectile)
		{
			this._projectile = projectile;
			switch (this._method)
			{
			case TargetFinder.Method.Closest:
				this._finder = new TargetFinder.FindDelegate(this.FindClosest);
				return;
			case TargetFinder.Method.First:
				this._finder = new TargetFinder.FindDelegate(this.FindFirst);
				return;
			case TargetFinder.Method.Random:
				this._finder = new TargetFinder.FindDelegate(this.FindRandom);
				return;
			case TargetFinder.Method.Player:
				this._finder = new TargetFinder.FindDelegate(this.FindPlayer);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x0007B794 File Offset: 0x00079994
		public Target Find()
		{
			TargetFinder._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(this._projectile.gameObject));
			this._range.enabled = true;
			TargetFinder._overlapper.OverlapCollider(this._range);
			this._range.enabled = false;
			return this._finder(TargetFinder._overlapper.results);
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x0007B804 File Offset: 0x00079A04
		private Target FindClosest(IReadOnlyList<Collider2D> result)
		{
			Target result2 = null;
			float num = float.MaxValue;
			for (int i = 0; i < result.Count; i++)
			{
				Target component = result[i].GetComponent<Target>();
				if (!(component == null))
				{
					Vector2 b = this._projectile.transform.position;
					float sqrMagnitude = (component.transform.position - b).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result2 = component;
					}
				}
			}
			return result2;
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0007B884 File Offset: 0x00079A84
		private Target FindFirst(IReadOnlyList<Collider2D> result)
		{
			return result.GetComponent<Collider2D, Target>();
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0007B88C File Offset: 0x00079A8C
		private Target FindRandom(IReadOnlyList<Collider2D> result)
		{
			List<Target> components = result.GetComponents(true);
			if (components.Count == 0)
			{
				return null;
			}
			return components.Random<Target>();
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0007B8B1 File Offset: 0x00079AB1
		private Target FindPlayer(IReadOnlyList<Collider2D> result)
		{
			return Singleton<Service>.Instance.levelManager.player.GetComponent<Target>();
		}

		// Token: 0x04002311 RID: 8977
		private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);

		// Token: 0x04002312 RID: 8978
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(2048, false, true, false, false);

		// Token: 0x04002313 RID: 8979
		[SerializeField]
		private TargetFinder.Method _method;

		// Token: 0x04002314 RID: 8980
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04002315 RID: 8981
		private IProjectile _projectile;

		// Token: 0x04002316 RID: 8982
		private TargetFinder.FindDelegate _finder;

		// Token: 0x020007D8 RID: 2008
		public enum Method
		{
			// Token: 0x04002318 RID: 8984
			Closest,
			// Token: 0x04002319 RID: 8985
			First,
			// Token: 0x0400231A RID: 8986
			Random,
			// Token: 0x0400231B RID: 8987
			Player
		}

		// Token: 0x020007D9 RID: 2009
		// (Invoke) Token: 0x060028A7 RID: 10407
		private delegate Target FindDelegate(IReadOnlyList<Collider2D> result);
	}
}
