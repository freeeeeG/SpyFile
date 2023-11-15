using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Hardmode;
using Level.Adventurer;
using Singletons;
using UnityEngine;

namespace Characters.AI.Adventurer
{
	// Token: 0x020013E9 RID: 5097
	public class Commander : MonoBehaviour
	{
		// Token: 0x0600645B RID: 25691 RVA: 0x001233B0 File Offset: 0x001215B0
		private void Awake()
		{
			this._partyRandomizer.Initialize();
			this._adventurers = this._partyRandomizer.SpawnCharacters();
			if (this._adventurers.Count > 1)
			{
				this._subAdventurers = new List<Character>();
			}
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._supportingCharacter = this._partyRandomizer.SpawnSupportingCharacter();
			}
		}

		// Token: 0x0600645C RID: 25692 RVA: 0x00123410 File Offset: 0x00121610
		public void StartIntro()
		{
			if (this._inCombat)
			{
				return;
			}
			this._inCombat = true;
			this.InitializeAdventurers();
			base.StartCoroutine(this.CCheckRoleChange());
			if (this._partyRandomizer.CanPlayHardmodeCutScene())
			{
				GameObject variableValue = null;
				foreach (Character character in this._adventurers)
				{
					character.gameObject.SetActive(false);
					character.GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedBool>(this._IntroSkipKey, true);
					variableValue = character.gameObject;
				}
				this._supportingCharacter.GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedGameObject>(this._cutSceneWarrior, variableValue);
			}
		}

		// Token: 0x0600645D RID: 25693 RVA: 0x001234D0 File Offset: 0x001216D0
		private void InitializeAdventurers()
		{
			using (List<Character>.Enumerator enumerator = this._adventurers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Character adventurer = enumerator.Current;
					adventurer.GetComponent<EnemyCharacterBehaviorOption>().SetTargetToPlayer();
					adventurer.health.onDied += delegate()
					{
						this._adventurers.Remove(adventurer);
						if (this._adventurers.Count > 0)
						{
							this.ChangeAdventurerRole();
						}
					};
					adventurer.GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedBool>(this._IsPartyKey, this._adventurers.Count > 1);
				}
			}
			if (Singleton<HardmodeManager>.Instance.hardmode && this._supportingCharacter != null)
			{
				this._supportingCharacter.GetComponent<EnemyCharacterBehaviorOption>().SetTargetToPlayer();
				this._supportingCharacter.GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedTransform>(this._supportingCharacterEscapePointKey, this._supportingCharacterEscapePoint);
			}
		}

		// Token: 0x0600645E RID: 25694 RVA: 0x001235CC File Offset: 0x001217CC
		private IEnumerator CCheckRoleChange()
		{
			while (this._adventurers.Count > 0)
			{
				this.ChangeAdventurerRole();
				yield return new WaitForSeconds(this._roleChangeTime);
			}
			yield break;
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x001235DC File Offset: 0x001217DC
		private void ChangeAdventurerRole()
		{
			Character y = this._adventurers.Random<Character>();
			if (this._subAdventurers != null)
			{
				this._subAdventurers.Clear();
			}
			foreach (Character character in this._adventurers)
			{
				BehaviorDesignerCommunicator component = character.GetComponent<BehaviorDesignerCommunicator>();
				if (character == y)
				{
					component.SetVariable<SharedBool>(this._IsMainKey, true);
				}
				else
				{
					this._subAdventurers.Add(character);
					component.SetVariable<SharedBool>(this._IsMainKey, false);
				}
			}
		}

		// Token: 0x040050F1 RID: 20721
		private readonly string _IsMainKey = "IsMain";

		// Token: 0x040050F2 RID: 20722
		private readonly string _IsPartyKey = "IsParty";

		// Token: 0x040050F3 RID: 20723
		private readonly string _supportingCharacterEscapePointKey = "EscapeDestination";

		// Token: 0x040050F4 RID: 20724
		private readonly string _IntroSkipKey = "IntroSkip";

		// Token: 0x040050F5 RID: 20725
		private readonly string _cutSceneWarrior = "CutSceneWarrior";

		// Token: 0x040050F6 RID: 20726
		[SerializeField]
		private PartyRandomizer _partyRandomizer;

		// Token: 0x040050F7 RID: 20727
		[SerializeField]
		private float _roleChangeTime = 10f;

		// Token: 0x040050F8 RID: 20728
		private bool _inCombat;

		// Token: 0x040050F9 RID: 20729
		private List<Character> _adventurers;

		// Token: 0x040050FA RID: 20730
		private List<Character> _subAdventurers;

		// Token: 0x040050FB RID: 20731
		[SerializeField]
		[Header("Hardmode")]
		private Transform _supportingCharacterEscapePoint;

		// Token: 0x040050FC RID: 20732
		private Character _supportingCharacter;
	}
}
