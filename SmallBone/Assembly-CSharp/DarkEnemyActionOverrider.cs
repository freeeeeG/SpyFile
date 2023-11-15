using System;
using BehaviorDesigner.Runtime;
using Characters.Actions;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class DarkEnemyActionOverrider : MonoBehaviour
{
	// Token: 0x06000057 RID: 87 RVA: 0x000038C4 File Offset: 0x00001AC4
	private void Start()
	{
		foreach (DarkEnemyActionOverrider.DarkEnemyAction darkEnemyAction in this._darkEnemyActions)
		{
			this._communicator.SetVariable<SharedCharacterAction>(darkEnemyAction.variableName, darkEnemyAction.action);
		}
	}

	// Token: 0x0400005B RID: 91
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	// Token: 0x0400005C RID: 92
	[SerializeField]
	private DarkEnemyActionOverrider.DarkEnemyAction[] _darkEnemyActions;

	// Token: 0x0200001A RID: 26
	[Serializable]
	private class DarkEnemyAction
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003901 File Offset: 0x00001B01
		public string variableName
		{
			get
			{
				return this._variableName;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003909 File Offset: 0x00001B09
		public Characters.Actions.Action action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private string _variableName;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private Characters.Actions.Action _action;
	}
}
