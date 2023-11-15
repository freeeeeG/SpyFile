using System;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F33 RID: 3891
	[Serializable]
	public class SetPlayerAsTarget : IBDCharacterSetting
	{
		// Token: 0x06004BC0 RID: 19392 RVA: 0x000DEC3C File Offset: 0x000DCE3C
		public void ApplyTo(Character character)
		{
			EnemyCharacterBehaviorOption component = character.GetComponent<EnemyCharacterBehaviorOption>();
			if (component)
			{
				component.SetTargetToPlayer();
			}
		}
	}
}
