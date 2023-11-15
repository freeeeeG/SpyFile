using System;

// Token: 0x02000395 RID: 917
public static class Result
{
	// Token: 0x06001318 RID: 4888 RVA: 0x00064E39 File Offset: 0x00063039
	public static Result.Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Result.Internal.Value_Ok<T>(value);
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x00064E41 File Offset: 0x00063041
	public static Result.Internal.Value_Err<T> Err<T>(T value)
	{
		return new Result.Internal.Value_Err<T>(value);
	}

	// Token: 0x02000FBD RID: 4029
	public static class Internal
	{
		// Token: 0x02001FB8 RID: 8120
		public readonly struct Value_Ok<T>
		{
			// Token: 0x0600A339 RID: 41785 RVA: 0x00367CC2 File Offset: 0x00365EC2
			public Value_Ok(T value)
			{
				this.value = value;
			}

			// Token: 0x04008ED6 RID: 36566
			public readonly T value;
		}

		// Token: 0x02001FB9 RID: 8121
		public readonly struct Value_Err<T>
		{
			// Token: 0x0600A33A RID: 41786 RVA: 0x00367CCB File Offset: 0x00365ECB
			public Value_Err(T value)
			{
				this.value = value;
			}

			// Token: 0x04008ED7 RID: 36567
			public readonly T value;
		}
	}
}
