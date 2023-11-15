using System;
using Level;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F2A RID: 3882
	[Serializable]
	public class AttachSummonEnemyWave : IBDCharacterSetting
	{
		// Token: 0x06004BA4 RID: 19364 RVA: 0x000DEA25 File Offset: 0x000DCC25
		public void ApplyTo(Character character)
		{
			Map.Instance.waveContainer.AttachToSummonEnemyWave(character);
		}
	}
}
