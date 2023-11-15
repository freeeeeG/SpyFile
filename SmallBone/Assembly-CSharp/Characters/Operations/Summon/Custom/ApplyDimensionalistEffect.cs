using System;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Operations.Summon.Custom
{
	// Token: 0x02000F64 RID: 3940
	[Serializable]
	public class ApplyDimensionalistEffect : IBDCharacterSetting
	{
		// Token: 0x06004C87 RID: 19591 RVA: 0x000E2F60 File Offset: 0x000E1160
		public void ApplyTo(Character character)
		{
			this._despawnEffect.Spawn(character.transform.position, 0f, 1f);
			if (this._loopEffect != null)
			{
				this._loopEffect.Spawn(character.transform.position, character, 0f, 1f).transform.parent = character.transform;
			}
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			SpriteRenderer mainRenderer = character.spriteEffectStack.mainRenderer;
			mainRenderer.material = MaterialResource.darkEnemy;
			mainRenderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_UseInverseColor"), 0f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_FirstGrayScale"), 1f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_UseAdderColor"), 1f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_DarkAreaAdderLevel"), -0.05f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_DarkAreaPower"), 0.08f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_RedLerpMax"), 1f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_BlueLerpMax"), 1f);
			materialPropertyBlock.SetFloat(Shader.PropertyToID("_RedLerpMax2"), 0f);
			materialPropertyBlock.SetColor(Shader.PropertyToID("_ColorBurn"), new Color(0.932489f, 0.8932005f, 0.9811321f));
			materialPropertyBlock.SetColor(Shader.PropertyToID("_AdderColor"), new Color(0.6739612f, 0.5707546f, 1f));
			materialPropertyBlock.SetColor(Shader.PropertyToID("_DarkAreaColor"), new Color(1f, 1f, 1f));
			mainRenderer.SetPropertyBlock(materialPropertyBlock);
			CharacterDieEffect component = character.GetComponent<CharacterDieEffect>();
			component.effect = this._despawnEffect;
			component.particleInfo = null;
		}

		// Token: 0x04003C2E RID: 15406
		[SerializeField]
		private EffectInfo _loopEffect;

		// Token: 0x04003C2F RID: 15407
		[SerializeField]
		private EffectInfo _despawnEffect;
	}
}
