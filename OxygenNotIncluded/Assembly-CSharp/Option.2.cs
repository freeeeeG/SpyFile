using System;

// Token: 0x0200038D RID: 909
public static class Option
{
	// Token: 0x060012D7 RID: 4823 RVA: 0x0006467A File Offset: 0x0006287A
	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x00064684 File Offset: 0x00062884
	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060012D9 RID: 4825 RVA: 0x000646B0 File Offset: 0x000628B0
	public static Option.Internal.Value_None None
	{
		get
		{
			return default(Option.Internal.Value_None);
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x000646C8 File Offset: 0x000628C8
	public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
	{
		if (options == null || options.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (!options[i].HasValue)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x02000FB4 RID: 4020
	public static class Internal
	{
		// Token: 0x02001FB6 RID: 8118
		public readonly struct Value_None
		{
		}

		// Token: 0x02001FB7 RID: 8119
		public readonly struct Value_HasValue
		{
			// Token: 0x0600A338 RID: 41784 RVA: 0x00367CB9 File Offset: 0x00365EB9
			public Value_HasValue(bool hasValue)
			{
				this.HasValue = hasValue;
			}

			// Token: 0x04008ED5 RID: 36565
			public readonly bool HasValue;
		}
	}
}
