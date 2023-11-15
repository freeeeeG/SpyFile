using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Objects.DecorationCharacter
{
	// Token: 0x02000578 RID: 1400
	public class DecorationCharacterAnimationController : MonoBehaviour
	{
		// Token: 0x06001B82 RID: 7042 RVA: 0x00055688 File Offset: 0x00053888
		private void Update()
		{
			for (int i = 0; i < this._animations.Count; i++)
			{
				CharacterAnimation characterAnimation = this._animations[i];
				characterAnimation.speed = this._chronometer.animation.timeScale / Time.timeScale;
				characterAnimation.parameter.walk.Value = this.parameter.walk;
				characterAnimation.parameter.grounded.Value = this.parameter.grounded;
				characterAnimation.parameter.movementSpeed.Value = this.parameter.movementSpeed;
				characterAnimation.parameter.ySpeed.Value = this.parameter.ySpeed;
				characterAnimation.transform.localScale = (this.parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x00055777 File Offset: 0x00053977
		public void Initialize(CharacterChronometer chronometer)
		{
			base.GetComponentsInChildren<CharacterAnimation>(true, this._animations);
			this._animations.ForEach(delegate(CharacterAnimation animation)
			{
				animation.Initialize();
			});
			this._chronometer = chronometer;
		}

		// Token: 0x040017A8 RID: 6056
		public readonly CharacterAnimationController.Parameter parameter = new CharacterAnimationController.Parameter();

		// Token: 0x040017A9 RID: 6057
		private List<CharacterAnimation> _animations = new List<CharacterAnimation>();

		// Token: 0x040017AA RID: 6058
		private CharacterChronometer _chronometer;
	}
}
