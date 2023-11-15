using System;

namespace Database
{
	// Token: 0x02000D2F RID: 3375
	public abstract class ColonyAchievementRequirement
	{
		// Token: 0x06006A45 RID: 27205
		public abstract bool Success();

		// Token: 0x06006A46 RID: 27206 RVA: 0x002982C8 File Offset: 0x002964C8
		public virtual bool Fail()
		{
			return false;
		}

		// Token: 0x06006A47 RID: 27207 RVA: 0x002982CB File Offset: 0x002964CB
		public virtual string GetProgress(bool complete)
		{
			return "";
		}
	}
}
