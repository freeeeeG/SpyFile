using System;
using Level;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F2B RID: 3883
	[Serializable]
	public class ContainInWave : IBDCharacterSetting
	{
		// Token: 0x06004BA6 RID: 19366 RVA: 0x000DEA37 File Offset: 0x000DCC37
		public void ApplyTo(Character character)
		{
			Map.Instance.waveContainer.Attach(character);
		}
	}
}
