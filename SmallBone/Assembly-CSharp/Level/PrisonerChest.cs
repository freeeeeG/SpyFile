using System;
using System.Collections;
using System.Linq;
using Characters;
using Characters.Abilities.Weapons;
using Characters.Gear.Weapons;
using Data;
using FX;
using GameResources;
using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000508 RID: 1288
	public class PrisonerChest : InteractiveObject
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0004F930 File Offset: 0x0004DB30
		public Weapon weapon
		{
			get
			{
				return this._weapon;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0004F938 File Offset: 0x0004DB38
		public PrisonerSkillInfosByGrade skills
		{
			get
			{
				return this._skills;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0004F940 File Offset: 0x0004DB40
		public SkillInfo skillInfo
		{
			get
			{
				return this._skillInfo;
			}
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0004F948 File Offset: 0x0004DB48
		protected override void Awake()
		{
			base.Awake();
			this._spawnEffect.Spawn(base.transform.position, 0f, 1f);
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0004F971 File Offset: 0x0004DB71
		public void SetSkillInfo(Weapon weapon, PrisonerSkillInfosByGrade skills, SkillInfo skillInfo)
		{
			this._weapon = weapon;
			this._skills = skills;
			this._skillInfo = skillInfo;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0004F988 File Offset: 0x0004DB88
		public int GetGradeBonus()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			double num = new System.Random((int)(GameData.Save.instance.randomSeed + 1177618293 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex)).NextDouble() * (double)this._weights.Sum();
			for (int i = 0; i < this._weights.Length; i++)
			{
				num -= (double)this._weights[i];
				if (num <= 0.0)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0004FA1E File Offset: 0x0004DC1E
		private void Start()
		{
			this._lineTextCoroutineReference = this.StartCoroutineWithReference(this.CStartLineText());
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0004FA32 File Offset: 0x0004DC32
		public override void OnActivate()
		{
			base.OnActivate();
			this._animator.Play(InteractiveObject._activateHash);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0004FA4A File Offset: 0x0004DC4A
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			this._animator.Play(InteractiveObject._deactivateHash);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0004FA64 File Offset: 0x0004DC64
		public override void OpenPopupBy(Character character)
		{
			if (!this._activationTrigger.IsSatisfied())
			{
				return;
			}
			if (!this._encounterSoundPlayed)
			{
				this._encounterSoundPlayed = true;
				PersistentSingleton<SoundManager>.Instance.PlaySound(this._encounterSound, base.transform.position);
			}
			base.OpenPopupBy(character);
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0004FAB1 File Offset: 0x0004DCB1
		public override void InteractWithByPressing(Character character)
		{
			if (!this._activationTrigger.IsSatisfied())
			{
				return;
			}
			this._lineTextCoroutineReference.Stop();
			character.status.RemoveStun();
			this._activateCutscene.Run();
			base.Deactivate();
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0004FAE8 File Offset: 0x0004DCE8
		private IEnumerator CStartLineText()
		{
			if (this._lineText == null || this._lineTextPosition == null)
			{
				yield break;
			}
			string[] texts = Localization.GetLocalizedStringArray("theKing/cursedChest/line/idle");
			if (texts.Length == 0)
			{
				yield break;
			}
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds((float)UnityEngine.Random.Range(5, 10));
				string text = texts.Random<string>();
				this._lineText.transform.position = this._lineTextPosition.position;
				this._lineText.Display(text, 2f);
			}
			yield break;
		}

		// Token: 0x04001620 RID: 5664
		private const int _randomSeed = 1177618293;

		// Token: 0x04001621 RID: 5665
		private const string _cursedChestTextKey = "theKing/cursedChest/line/idle";

		// Token: 0x04001622 RID: 5666
		[Space]
		[SerializeField]
		private SoundInfo _encounterSound;

		// Token: 0x04001623 RID: 5667
		private bool _encounterSoundPlayed;

		// Token: 0x04001624 RID: 5668
		[Header("LineText")]
		[SerializeField]
		private LineText _lineText;

		// Token: 0x04001625 RID: 5669
		[SerializeField]
		private Transform _lineTextPosition;

		// Token: 0x04001626 RID: 5670
		[GetComponent]
		[SerializeField]
		private Animator _animator;

		// Token: 0x04001627 RID: 5671
		[SerializeField]
		private PrisonerSkillScroll _skillScroll;

		// Token: 0x04001628 RID: 5672
		[SerializeField]
		[Space]
		private DroppedCell _cellPrefab;

		// Token: 0x04001629 RID: 5673
		[SerializeField]
		private CustomFloat _cellCount;

		// Token: 0x0400162A RID: 5674
		[SerializeField]
		[Header("Grade Weights (단위 : 0.5,  순서÷2가 등급보너스임)")]
		[Range(0f, 100f)]
		private int[] _weights;

		// Token: 0x0400162B RID: 5675
		[SerializeField]
		[Space]
		private EffectInfo _spawnEffect;

		// Token: 0x0400162C RID: 5676
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _activationTrigger;

		// Token: 0x0400162D RID: 5677
		[SerializeField]
		private Runnable _activateCutscene;

		// Token: 0x0400162E RID: 5678
		private Weapon _weapon;

		// Token: 0x0400162F RID: 5679
		private PrisonerSkillInfosByGrade _skills;

		// Token: 0x04001630 RID: 5680
		private SkillInfo _skillInfo;

		// Token: 0x04001631 RID: 5681
		private CoroutineReference _lineTextCoroutineReference;
	}
}
