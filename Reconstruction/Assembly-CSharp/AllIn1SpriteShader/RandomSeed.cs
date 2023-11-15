using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CD RID: 717
	public class RandomSeed : MonoBehaviour
	{
		// Token: 0x06001169 RID: 4457 RVA: 0x000320F4 File Offset: 0x000302F4
		private void Start()
		{
			Renderer component = base.GetComponent<Renderer>();
			if (component != null)
			{
				if (component.material != null)
				{
					component.material.SetFloat("_RandomSeed", Random.Range(0f, 1000f));
					return;
				}
				Debug.LogError("Missing Renderer or Material: " + base.gameObject.name);
				return;
			}
			else
			{
				Image component2 = base.GetComponent<Image>();
				if (!(component2 != null))
				{
					Debug.LogError("Missing Renderer or UI Image on: " + base.gameObject.name);
					return;
				}
				if (component2.material != null)
				{
					component2.material.SetFloat("_RandomSeed", Random.Range(0f, 1000f));
					return;
				}
				Debug.LogError("Missing Material on UI Image: " + base.gameObject.name);
				return;
			}
		}
	}
}
