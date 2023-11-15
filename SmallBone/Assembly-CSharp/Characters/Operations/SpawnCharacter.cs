using System;
using BehaviorDesigner.Runtime;
using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E35 RID: 3637
	public class SpawnCharacter : CharacterOperation
	{
		// Token: 0x06004881 RID: 18561 RVA: 0x000D2C80 File Offset: 0x000D0E80
		public override void Run(Character owner)
		{
			Character character;
			if (this._position != null)
			{
				character = UnityEngine.Object.Instantiate<Character>(this._character, this._position.position, Quaternion.identity, Map.Instance.transform);
			}
			else
			{
				character = UnityEngine.Object.Instantiate<Character>(this._character);
			}
			if (this._containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
			if (this._setPlayerAsTarget)
			{
				AIController componentInChildren = character.GetComponentInChildren<AIController>();
				if (componentInChildren != null)
				{
					componentInChildren.target = Singleton<Service>.Instance.levelManager.player;
					componentInChildren.character.ForceToLookAt(componentInChildren.target.transform.position.x);
				}
				else
				{
					BehaviorDesignerCommunicator component = character.GetComponent<BehaviorDesignerCommunicator>();
					if (component != null)
					{
						component.GetVariable<SharedCharacter>("Target").Value = Singleton<Service>.Instance.levelManager.player;
					}
				}
			}
			if (!this._masterSlaveLink)
			{
				return;
			}
			Master componentInChildren2 = owner.GetComponentInChildren<Master>();
			Slave componentInChildren3 = character.GetComponentInChildren<Slave>();
			componentInChildren2.AddSlave(componentInChildren3);
			componentInChildren3.Initialize(componentInChildren2);
		}

		// Token: 0x04003791 RID: 14225
		[SerializeField]
		private Character _character;

		// Token: 0x04003792 RID: 14226
		[SerializeField]
		private Transform _position;

		// Token: 0x04003793 RID: 14227
		[SerializeField]
		private bool _masterSlaveLink;

		// Token: 0x04003794 RID: 14228
		[SerializeField]
		private bool _setPlayerAsTarget;

		// Token: 0x04003795 RID: 14229
		[SerializeField]
		private bool _containInWave;
	}
}
