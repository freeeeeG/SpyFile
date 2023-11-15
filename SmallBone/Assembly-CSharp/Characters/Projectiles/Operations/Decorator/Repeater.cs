using System;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator
{
	// Token: 0x020007AF RID: 1967
	public class Repeater : Operation
	{
		// Token: 0x06002818 RID: 10264 RVA: 0x000794EC File Offset: 0x000776EC
		private void Awake()
		{
			if (this._times == 0)
			{
				this._times = int.MaxValue;
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00079504 File Offset: 0x00077704
		public override void Run(IProjectile projectile)
		{
			Repeater.<>c__DisplayClass5_0 CS$<>8__locals1 = new Repeater.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.projectile = projectile;
			CS$<>8__locals1.interval = this._interval;
			this._repeatCoroutineReference = this.StartCoroutineWithReference(CS$<>8__locals1.<Run>g__CRepeat|0());
		}

		// Token: 0x04002259 RID: 8793
		[SerializeField]
		private int _times;

		// Token: 0x0400225A RID: 8794
		[SerializeField]
		private float _interval;

		// Token: 0x0400225B RID: 8795
		[SerializeField]
		[Operation.SubcomponentAttribute]
		private Operation _toRepeat;

		// Token: 0x0400225C RID: 8796
		private CoroutineReference _repeatCoroutineReference;
	}
}
