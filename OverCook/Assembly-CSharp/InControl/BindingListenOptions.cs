using System;

namespace InControl
{
	// Token: 0x02000294 RID: 660
	public class BindingListenOptions
	{
		// Token: 0x04000930 RID: 2352
		public bool IncludeControllers = true;

		// Token: 0x04000931 RID: 2353
		public bool IncludeUnknownControllers;

		// Token: 0x04000932 RID: 2354
		public bool IncludeNonStandardControls = true;

		// Token: 0x04000933 RID: 2355
		public bool IncludeMouseButtons;

		// Token: 0x04000934 RID: 2356
		public bool IncludeKeys = true;

		// Token: 0x04000935 RID: 2357
		public bool IncludeModifiersAsFirstClassKeys;

		// Token: 0x04000936 RID: 2358
		public uint MaxAllowedBindings;

		// Token: 0x04000937 RID: 2359
		public uint MaxAllowedBindingsPerType;

		// Token: 0x04000938 RID: 2360
		public bool AllowDuplicateBindingsPerSet;

		// Token: 0x04000939 RID: 2361
		public bool UnsetDuplicateBindingsOnSet;

		// Token: 0x0400093A RID: 2362
		public BindingSource ReplaceBinding;

		// Token: 0x0400093B RID: 2363
		public Func<PlayerAction, BindingSource, bool> OnBindingFound;

		// Token: 0x0400093C RID: 2364
		public Action<PlayerAction, BindingSource> OnBindingAdded;

		// Token: 0x0400093D RID: 2365
		public Action<PlayerAction, BindingSource, BindingSourceRejectionType> OnBindingRejected;
	}
}
