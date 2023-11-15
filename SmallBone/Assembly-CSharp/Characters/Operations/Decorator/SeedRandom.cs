using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000ECE RID: 3790
	public class SeedRandom : CharacterOperation
	{
		// Token: 0x06004A71 RID: 19057 RVA: 0x000D9378 File Offset: 0x000D7578
		public override void Initialize()
		{
			this._toRandom.Initialize();
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			this._random = new System.Random((int)(GameData.Save.instance.randomSeed + 1020464638 + currentChapter.type * (Chapter.Type)256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex));
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x000D93E0 File Offset: 0x000D75E0
		public override void Run(Character owner)
		{
			CharacterOperation[] components = this._toRandom.components;
			if (components.Length == 0)
			{
				return;
			}
			components.PseudoShuffle(this._random);
			if (components[0] == null)
			{
				return;
			}
			components[0].Run(owner);
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x000D941F File Offset: 0x000D761F
		public override void Stop()
		{
			this._toRandom.Stop();
		}

		// Token: 0x0400399A RID: 14746
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _toRandom;

		// Token: 0x0400399B RID: 14747
		private const int _randomSeed = 1020464638;

		// Token: 0x0400399C RID: 14748
		private System.Random _random;
	}
}
