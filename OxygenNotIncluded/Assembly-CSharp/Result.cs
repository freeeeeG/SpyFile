using System;

// Token: 0x02000394 RID: 916
public readonly struct Result<TSuccess, TError>
{
	// Token: 0x06001310 RID: 4880 RVA: 0x00064D62 File Offset: 0x00062F62
	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	// Token: 0x06001311 RID: 4881 RVA: 0x00064D7C File Offset: 0x00062F7C
	public bool IsOk()
	{
		return this.successValue.IsSome();
	}

	// Token: 0x06001312 RID: 4882 RVA: 0x00064D89 File Offset: 0x00062F89
	public bool IsErr()
	{
		return this.errorValue.IsSome() || this.successValue.IsNone();
	}

	// Token: 0x06001313 RID: 4883 RVA: 0x00064DA5 File Offset: 0x00062FA5
	public TSuccess Unwrap()
	{
		if (this.successValue.IsSome())
		{
			return this.successValue.Unwrap();
		}
		if (this.errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x00064DE2 File Offset: 0x00062FE2
	public Option<TSuccess> Ok()
	{
		return this.successValue;
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x00064DEA File Offset: 0x00062FEA
	public Option<TError> Err()
	{
		return this.errorValue;
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x00064DF4 File Offset: 0x00062FF4
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x00064E18 File Offset: 0x00063018
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}

	// Token: 0x04000A48 RID: 2632
	private readonly Option<TSuccess> successValue;

	// Token: 0x04000A49 RID: 2633
	private readonly Option<TError> errorValue;
}
