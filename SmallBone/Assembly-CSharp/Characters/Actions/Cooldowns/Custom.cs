using System;

namespace Characters.Actions.Cooldowns
{
	// Token: 0x02000966 RID: 2406
	public class Custom : Cooldown
	{
		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x060033E1 RID: 13281 RVA: 0x00099DDF File Offset: 0x00097FDF
		public override float remainPercent
		{
			get
			{
				return (float)(this._canUse ? 0 : 1);
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060033E2 RID: 13282 RVA: 0x00099DEE File Offset: 0x00097FEE
		public override bool canUse
		{
			get
			{
				return this._canUse;
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x00099DF6 File Offset: 0x00097FF6
		internal override bool Consume()
		{
			if (this._canUse)
			{
				this._canUse = false;
				return true;
			}
			return false;
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x00099E0A File Offset: 0x0009800A
		internal void SetCanUse()
		{
			this._canUse = true;
		}

		// Token: 0x04002A09 RID: 10761
		private bool _canUse;
	}
}
