using System;
using UnityEngine;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200001F RID: 31
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class AnimatorParamAttribute : DrawerAttribute
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000332A File Offset: 0x0000152A
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003332 File Offset: 0x00001532
		public string AnimatorName { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000051 RID: 81 RVA: 0x0000333B File Offset: 0x0000153B
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003343 File Offset: 0x00001543
		public AnimatorControllerParameterType? AnimatorParamType { get; private set; }

		// Token: 0x06000053 RID: 83 RVA: 0x0000334C File Offset: 0x0000154C
		public AnimatorParamAttribute(string animatorName)
		{
			this.AnimatorName = animatorName;
			this.AnimatorParamType = null;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003375 File Offset: 0x00001575
		public AnimatorParamAttribute(string animatorName, AnimatorControllerParameterType animatorParamType)
		{
			this.AnimatorName = animatorName;
			this.AnimatorParamType = new AnimatorControllerParameterType?(animatorParamType);
		}
	}
}
