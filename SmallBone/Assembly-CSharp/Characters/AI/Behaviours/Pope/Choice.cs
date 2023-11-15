using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Behaviours.Attacks;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Pope
{
	// Token: 0x0200133F RID: 4927
	public sealed class Choice : Behaviour
	{
		// Token: 0x0600612F RID: 24879 RVA: 0x0011CA29 File Offset: 0x0011AC29
		private void Awake()
		{
			this._container = UnityEngine.Object.FindObjectOfType<EnemyWaveContainer>();
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x0011CA36 File Offset: 0x0011AC36
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			yield return this._attack.CRun(controller);
			base.result = Behaviour.Result.Success;
			yield break;
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x0011CA4C File Offset: 0x0011AC4C
		public void RunSacrifice()
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			if (allEnemies == null)
			{
				return;
			}
			if (allEnemies.Count <= 0)
			{
				return;
			}
			this._targets = (from character in allEnemies
			select character.GetComponent<SacrificeCharacter>()).ToList<SacrificeCharacter>();
			foreach (SacrificeCharacter sacrificeCharacter in this._targets)
			{
				if (sacrificeCharacter != null)
				{
					sacrificeCharacter.Run(true);
				}
			}
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x0011CAF8 File Offset: 0x0011ACF8
		public bool CanUse()
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			if (allEnemies == null)
			{
				return false;
			}
			if (allEnemies.Count <= 0)
			{
				return false;
			}
			return allEnemies.Any((Character character) => character.GetComponent<SacrificeCharacter>() != null);
		}

		// Token: 0x04004E5C RID: 20060
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(ActionAttack))]
		private ActionAttack _attack;

		// Token: 0x04004E5D RID: 20061
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04004E5E RID: 20062
		private SacrificeCharacter _target;

		// Token: 0x04004E5F RID: 20063
		private List<SacrificeCharacter> _targets;

		// Token: 0x04004E60 RID: 20064
		private EnemyWaveContainer _container;
	}
}
