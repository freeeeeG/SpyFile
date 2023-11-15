using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;

// Token: 0x0200038C RID: 908
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	// Token: 0x060012BA RID: 4794 RVA: 0x00064398 File Offset: 0x00062598
	public Option(T value)
	{
		this.value = value;
		this.hasValue = true;
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060012BB RID: 4795 RVA: 0x000643A8 File Offset: 0x000625A8
	public bool HasValue
	{
		get
		{
			return this.hasValue;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060012BC RID: 4796 RVA: 0x000643B0 File Offset: 0x000625B0
	public T Value
	{
		get
		{
			return this.Unwrap();
		}
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x000643B8 File Offset: 0x000625B8
	public T Unwrap()
	{
		if (!this.hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return this.value;
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x000643EC File Offset: 0x000625EC
	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return fallback_value;
		}
		return this.value;
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x00064427 File Offset: 0x00062627
	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return get_fallback_value_fn();
		}
		return this.value;
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x00064468 File Offset: 0x00062668
	public T UnwrapOrDefault()
	{
		if (!this.hasValue)
		{
			return default(T);
		}
		return this.value;
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0006448D File Offset: 0x0006268D
	public T Expect(string msg_on_fail)
	{
		if (!this.hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return this.value;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x000644A4 File Offset: 0x000626A4
	public bool IsSome()
	{
		return this.hasValue;
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x000644AC File Offset: 0x000626AC
	public bool IsNone()
	{
		return !this.hasValue;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x000644B7 File Offset: 0x000626B7
	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return Option.Maybe<U>(fn(this.value));
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x000644DD File Offset: 0x000626DD
	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return fn(this.value);
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x000644FE File Offset: 0x000626FE
	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe<T>(value);
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x00064506 File Offset: 0x00062706
	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x00064510 File Offset: 0x00062710
	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x00064526 File Offset: 0x00062726
	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x00064533 File Offset: 0x00062733
	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x00064549 File Offset: 0x00062749
	public bool Equals(Option<T> other)
	{
		return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0006457C File Offset: 0x0006277C
	public override bool Equals(object obj)
	{
		if (obj is Option<T>)
		{
			Option<T> other = (Option<T>)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x000645A1 File Offset: 0x000627A1
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x000645AB File Offset: 0x000627AB
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x000645B8 File Offset: 0x000627B8
	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x000645F6 File Offset: 0x000627F6
	public override string ToString()
	{
		if (!this.hasValue)
		{
			return "None";
		}
		return string.Format("{0}", this.value);
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0006461B File Offset: 0x0006281B
	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00064625 File Offset: 0x00062825
	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x00064631 File Offset: 0x00062831
	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0006463B File Offset: 0x0006283B
	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00064647 File Offset: 0x00062847
	public bool Equals(T other)
	{
		return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060012D6 RID: 4822 RVA: 0x00064664 File Offset: 0x00062864
	public static Option<T> None
	{
		get
		{
			return default(Option<T>);
		}
	}

	// Token: 0x04000A3D RID: 2621
	[Serialize]
	private readonly bool hasValue;

	// Token: 0x04000A3E RID: 2622
	[Serialize]
	private readonly T value;
}
