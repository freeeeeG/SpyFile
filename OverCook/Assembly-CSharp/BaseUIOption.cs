using System;
using UnityEngine;

// Token: 0x02000AE7 RID: 2791
public abstract class BaseUIOption<T> : MonoBehaviour, ISyncUIWithOption where T : IOption
{
	// Token: 0x0600388F RID: 14479 RVA: 0x0010AF28 File Offset: 0x00109328
	protected virtual void Awake()
	{
		if (Enum.IsDefined(typeof(OptionsData.OptionType), this.m_OptionType))
		{
			IOption option = GameUtils.GetMetaGameProgress().AccessOptionsData.GetOption(this.m_OptionType);
			if (option != null)
			{
				this.m_Option = (T)((object)option);
			}
			this.SyncUIWithOption();
		}
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x0010AF82 File Offset: 0x00109382
	public virtual void SetValue(int _Value)
	{
		if (this.m_Option != null)
		{
			this.m_Option.SetOption(_Value);
		}
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x0010AFA6 File Offset: 0x001093A6
	public virtual int GetValue()
	{
		if (this.m_Option != null)
		{
			return this.m_Option.GetOption();
		}
		return 0;
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x0010AFCB File Offset: 0x001093CB
	public virtual void SyncUIWithOption()
	{
	}

	// Token: 0x04002D42 RID: 11586
	protected T m_Option = default(T);

	// Token: 0x04002D43 RID: 11587
	[SerializeField]
	private OptionsData.OptionType m_OptionType;
}
