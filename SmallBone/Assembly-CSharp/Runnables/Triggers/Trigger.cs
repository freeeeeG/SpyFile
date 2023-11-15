using System;
using Runnables.Triggers.Customs;
using UnityEditor;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x0200036B RID: 875
	public abstract class Trigger : MonoBehaviour
	{
		// Token: 0x06001027 RID: 4135
		protected abstract bool Check();

		// Token: 0x06001028 RID: 4136 RVA: 0x0003016F File Offset: 0x0002E36F
		public bool IsSatisfied()
		{
			return this.Check();
		}

		// Token: 0x04000D3E RID: 3390
		public static readonly Type[] types = new Type[]
		{
			typeof(Always),
			typeof(ActivatedSkulStory),
			typeof(ByPosition),
			typeof(CageOnDestroyed),
			typeof(CharacterDied),
			typeof(EnterZone),
			typeof(EqualsState),
			typeof(EqualWeaponName),
			typeof(HasCurrency),
			typeof(Inverter),
			typeof(MapRewardActivated),
			typeof(StoppedEnemyContainer),
			typeof(Sequence),
			typeof(Timer),
			typeof(WaveOnSpawn),
			typeof(WaveOnClear),
			typeof(PlayedCutScene),
			typeof(PlayedSkulStory),
			typeof(PlayedTutorial),
			typeof(CharacterActionRunning),
			typeof(HasBDVariable),
			typeof(ClearedLevelCompare),
			typeof(HardmodeLevelCompare),
			typeof(DarktechUnlocked),
			typeof(DarktechActivated),
			typeof(Mode),
			typeof(NormalEnding)
		};

		// Token: 0x0200036C RID: 876
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x0600102B RID: 4139 RVA: 0x00030302 File Offset: 0x0002E502
			public SubcomponentAttribute() : base(true, Trigger.types)
			{
			}
		}
	}
}
