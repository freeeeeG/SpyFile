using System;
using System.Collections;
using Characters.Actions;
using Level;
using Level.Traps;
using UnityEngine;

namespace Characters.AI.Behaviours
{
	// Token: 0x020012D8 RID: 4824
	public sealed class Sacrifice : Behaviour
	{
		// Token: 0x06005F6B RID: 24427 RVA: 0x001178C8 File Offset: 0x00115AC8
		private void Awake()
		{
			this.Preload();
			base.result = Behaviour.Result.Done;
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x001178D7 File Offset: 0x00115AD7
		private void Preload()
		{
			this._instantiatedTentacle = UnityEngine.Object.Instantiate<TentacleAI>(this._tentaclePrefab, base.transform);
			this._instantiatedTentacle.Hide();
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x001178FB File Offset: 0x00115AFB
		public override IEnumerator CRun(AIController controller)
		{
			Character character = controller.character;
			controller.StopAllBehaviour();
			base.result = Behaviour.Result.Doing;
			while (!controller.dead)
			{
				if (controller.stuned)
				{
					yield return null;
				}
				else
				{
					if (this._action.TryStart())
					{
						while (this._action.running && base.result.Equals(Behaviour.Result.Doing))
						{
							yield return null;
						}
					}
					if (!controller.stuned)
					{
						break;
					}
					yield return null;
				}
			}
			base.result = Behaviour.Result.Done;
			this.SummonTentacle(character);
			yield break;
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x00117914 File Offset: 0x00115B14
		private void SummonTentacle(Character owner)
		{
			this._instantiatedTentacle.Appear(owner.transform, this._corpseImage, owner.lookingDirection == Character.LookingDirection.Left);
			this._instantiatedTentacle.character.lookingDirection = owner.lookingDirection;
			Map.Instance.waveContainer.summonWave.Attach(this._instantiatedTentacle.character);
			CharacterDieEffect component = owner.GetComponent<CharacterDieEffect>();
			if (component != null)
			{
				component.Detach();
			}
			owner.health.Kill();
			owner.gameObject.SetActive(false);
			owner.collider.enabled = false;
		}

		// Token: 0x04004CAE RID: 19630
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04004CAF RID: 19631
		[SerializeField]
		private TentacleAI _tentaclePrefab;

		// Token: 0x04004CB0 RID: 19632
		[SerializeField]
		private Sprite _corpseImage;

		// Token: 0x04004CB1 RID: 19633
		private TentacleAI _instantiatedTentacle;
	}
}
