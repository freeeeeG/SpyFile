using System;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E9B RID: 3739
	[Serializable]
	public class Enabled : ICondition
	{
		// Token: 0x060049D2 RID: 18898 RVA: 0x000864E2 File Offset: 0x000846E2
		public bool Satisfied(Character character)
		{
			return character.gameObject.activeSelf;
		}
	}
}
