using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000ECF RID: 3791
	public class Sequencer : CharacterOperation
	{
		// Token: 0x06004A75 RID: 19061 RVA: 0x000D942C File Offset: 0x000D762C
		public override void Initialize()
		{
			this._operations.Initialize();
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x000D943C File Offset: 0x000D763C
		public override void Run(Character owner)
		{
			CharacterOperation[] components = this._operations.components;
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Run(owner);
			}
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x000D946C File Offset: 0x000D766C
		public override void Run(Character owner, Character target)
		{
			CharacterOperation[] components = this._operations.components;
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Run(owner);
			}
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x000D949C File Offset: 0x000D769C
		public override void Stop()
		{
			this._operations.Stop();
		}

		// Token: 0x0400399D RID: 14749
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operations;
	}
}
