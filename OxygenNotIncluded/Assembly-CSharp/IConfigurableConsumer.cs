using System;

// Token: 0x02000C0E RID: 3086
public interface IConfigurableConsumer
{
	// Token: 0x060061C0 RID: 25024
	IConfigurableConsumerOption[] GetSettingOptions();

	// Token: 0x060061C1 RID: 25025
	IConfigurableConsumerOption GetSelectedOption();

	// Token: 0x060061C2 RID: 25026
	void SetSelectedOption(IConfigurableConsumerOption option);
}
