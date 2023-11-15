using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DBF RID: 3519
	public class Worship : CharacterOperation
	{
		// Token: 0x060046BE RID: 18110 RVA: 0x000CD598 File Offset: 0x000CB798
		private void Awake()
		{
			for (int i = 0; i < this._operations.Length; i++)
			{
				this._operations[i].Initialize();
			}
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x000CD5C8 File Offset: 0x000CB7C8
		public override void Run(Character owner)
		{
			if (this._current >= this._operations.Length || this._current == 0)
			{
				this._current = 0;
				this._operations.Shuffle<OperationInfos>();
			}
			int num = this._current;
			int num2 = 0;
			while (num < this._operations.Length && num2 < this._firedAtOnce)
			{
				this._commonOperations.Run(owner);
				this._operations[num].gameObject.SetActive(true);
				this._operations[num].Run(owner);
				num++;
				num2++;
				this._current = num;
			}
		}

		// Token: 0x04003597 RID: 13719
		[SerializeField]
		private int _firedAtOnce;

		// Token: 0x04003598 RID: 13720
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _commonOperations;

		// Token: 0x04003599 RID: 13721
		[SerializeField]
		private OperationInfos[] _operations = new OperationInfos[4];

		// Token: 0x0400359A RID: 13722
		private int _current;
	}
}
