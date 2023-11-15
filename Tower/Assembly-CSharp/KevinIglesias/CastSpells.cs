using System;
using System.Collections;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000C9 RID: 201
	public class CastSpells : MonoBehaviour
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000CFD9 File Offset: 0x0000B1D9
		public void ThrowFireball(CastHand hand, float delay)
		{
			base.StartCoroutine(this.SpawnFireball(hand, delay));
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000CFEA File Offset: 0x0000B1EA
		public void ThrowNova(float delay)
		{
			base.StartCoroutine(this.SpawnNova(delay));
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000CFFA File Offset: 0x0000B1FA
		public void ThrowHealing(CastHand hand, float delay)
		{
			base.StartCoroutine(this.SpawnHealing(hand, delay));
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000D00B File Offset: 0x0000B20B
		public void ThrowShockwave(CastHand hand, float delay)
		{
			base.StartCoroutine(this.SpawnShockwave(hand, delay));
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000D01C File Offset: 0x0000B21C
		public IEnumerator SpawnFireball(CastHand hand, float delay)
		{
			Transform handT;
			if (hand == CastHand.RightHand)
			{
				handT = this.rightHand;
			}
			else
			{
				handT = this.leftHand;
			}
			yield return new WaitForSeconds(delay);
			GameObject gameObject = Object.Instantiate<GameObject>(this.spellPrefab, handT.position, Quaternion.identity);
			gameObject.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
			gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z + this.spellOffset);
			base.StartCoroutine(this.AppearFireball(gameObject.transform));
			base.StartCoroutine(this.MoveFireball(gameObject.transform));
			if (hand == CastHand.RightHand)
			{
				if (this.castEffectR != null)
				{
					Object.Destroy(this.castEffectR);
				}
			}
			else if (this.castEffectL != null)
			{
				Object.Destroy(this.castEffectL);
			}
			yield break;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000D039 File Offset: 0x0000B239
		public IEnumerator SpawnHealing(CastHand hand, float delay)
		{
			Transform handT;
			if (hand == CastHand.RightHand)
			{
				handT = this.rightHand;
			}
			else
			{
				handT = this.leftHand;
			}
			yield return new WaitForSeconds(delay);
			GameObject gameObject = Object.Instantiate<GameObject>(this.spellPrefab, handT.position, Quaternion.identity);
			Transform t = gameObject.transform;
			t.rotation *= Quaternion.Euler(0f, 180f, 0f);
			t.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z);
			if (hand == CastHand.RightHand)
			{
				if (this.castEffectR != null)
				{
					Object.Destroy(this.castEffectR);
				}
			}
			else if (this.castEffectL != null)
			{
				Object.Destroy(this.castEffectL);
			}
			Vector3 startSize = t.localScale;
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 8f;
				t.localScale = Vector3.Lerp(startSize, Vector3.one, i);
				yield return 0;
			}
			yield return new WaitForSeconds(1f);
			Object.Destroy(t.gameObject);
			yield break;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000D058 File Offset: 0x0000B258
		public void SpawnEffect(CastHand hand)
		{
			if (hand == CastHand.RightHand)
			{
				this.castEffectR = Object.Instantiate<GameObject>(this.castEffectPrefab, this.rightHand);
				this.castEffectR.transform.localPosition = this.castEffectR.transform.localPosition + this.handOffset;
				return;
			}
			this.castEffectL = Object.Instantiate<GameObject>(this.castEffectPrefab, this.leftHand);
			this.castEffectL.transform.localPosition = this.castEffectL.transform.localPosition + this.handOffset;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000D0ED File Offset: 0x0000B2ED
		public IEnumerator SpawnNova(float delay)
		{
			yield return new WaitForSeconds(delay);
			GameObject newNova = Object.Instantiate<GameObject>(this.spellPrefab, new Vector3(base.transform.position.x, this.spellPrefab.transform.position.y, base.transform.position.z), Quaternion.identity);
			Transform t = newNova.transform;
			t.localScale = Vector3.zero;
			Vector3 startSize = t.localScale;
			if (this.castEffectR != null)
			{
				Object.Destroy(this.castEffectR);
			}
			if (this.castEffectL != null)
			{
				Object.Destroy(this.castEffectL);
			}
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 5f;
				t.localScale = Vector3.Lerp(startSize, Vector3.one * 4f, i);
				yield return 0;
			}
			Renderer r = newNova.GetComponent<Renderer>();
			Color initColor = r.material.GetColor("_Color");
			Color endColor = new Color(initColor.r, initColor.g, initColor.b, 0f);
			i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 5f;
				r.material.SetColor("_Color", Color.Lerp(initColor, endColor, i));
				yield return 0;
			}
			Object.Destroy(t.gameObject);
			yield break;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000D103 File Offset: 0x0000B303
		public IEnumerator SpawnShockwave(CastHand hand, float delay)
		{
			Transform handT;
			if (hand == CastHand.RightHand)
			{
				handT = this.rightHand;
			}
			else
			{
				handT = this.leftHand;
			}
			yield return new WaitForSeconds(delay);
			GameObject newShockwave = Object.Instantiate<GameObject>(this.spellPrefab, handT.position, Quaternion.identity);
			Transform t = newShockwave.transform;
			t.position = new Vector3(t.position.x, 0.001f, t.position.z);
			if (hand == CastHand.RightHand)
			{
				if (this.castEffectR != null)
				{
					Object.Destroy(this.castEffectR);
				}
			}
			else if (this.castEffectL != null)
			{
				Object.Destroy(this.castEffectL);
			}
			yield return new WaitForSeconds(3f);
			Renderer r = newShockwave.GetComponent<Renderer>();
			Color initColor = r.material.GetColor("_Color");
			Color endColor = new Color(initColor.r, initColor.g, initColor.b, 0f);
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 5f;
				r.material.SetColor("_Color", Color.Lerp(initColor, endColor, i));
				yield return 0;
			}
			Object.Destroy(t.gameObject);
			yield break;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000D120 File Offset: 0x0000B320
		private IEnumerator AppearFireball(Transform t)
		{
			Vector3 startSize = t.localScale;
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 8f;
				t.localScale = Vector3.Lerp(startSize, Vector3.one, i);
				yield return 0;
			}
			yield break;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000D12F File Offset: 0x0000B32F
		private IEnumerator MoveFireball(Transform t)
		{
			Vector3 initPosition = t.localPosition;
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 0.33f;
				t.localPosition = Vector3.Lerp(initPosition, new Vector3(initPosition.x, initPosition.y, 50f), i);
				yield return 0;
			}
			Object.Destroy(t.gameObject);
			yield break;
		}

		// Token: 0x04000276 RID: 630
		public Transform rightHand;

		// Token: 0x04000277 RID: 631
		public Transform leftHand;

		// Token: 0x04000278 RID: 632
		public Vector3 handOffset;

		// Token: 0x04000279 RID: 633
		public float spellOffset;

		// Token: 0x0400027A RID: 634
		public GameObject spellPrefab;

		// Token: 0x0400027B RID: 635
		public GameObject castEffectPrefab;

		// Token: 0x0400027C RID: 636
		[HideInInspector]
		public GameObject castEffectR;

		// Token: 0x0400027D RID: 637
		[HideInInspector]
		public GameObject castEffectL;
	}
}
