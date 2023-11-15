using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C6 RID: 198
public class Obj_GrabEffect : MonoBehaviour
{
	// Token: 0x06000490 RID: 1168 RVA: 0x0001269C File Offset: 0x0001089C
	public void GrabMonsters(List<AMonsterBase> list_Monsters, float maxDist)
	{
		base.StartCoroutine(this.CR_GrabMonsters(list_Monsters, maxDist));
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x000126AD File Offset: 0x000108AD
	private IEnumerator CR_GrabMonsters(List<AMonsterBase> list_Monsters, float maxDist)
	{
		this.particle_GrabEffect.transform.position = base.transform.position.WithY(0.1f);
		this.particle_GrabEffect.Play();
		int num;
		for (int i = 0; i < list_Monsters.Count; i = num + 1)
		{
			float height = this.grabHeight * (Vector3.Distance(list_Monsters[i].transform.position, base.transform.position) / maxDist);
			base.StartCoroutine(this.CR_GrabMonster(list_Monsters[i], this.list_LineRenderers[i], height));
			yield return new WaitForSeconds(0.1f);
			num = i;
		}
		yield return new WaitForSeconds(3f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x000126CA File Offset: 0x000108CA
	private IEnumerator CR_GrabMonster(AMonsterBase target, LineRenderer line, float height)
	{
		float time = 0f;
		float duration = this.grabDuration;
		Vector3 startPos = target.transform.position;
		Vector3 position = Vector3.zero;
		Vector3 zero = Vector3.zero;
		SoundManager.PlaySound("VFX", "DeathGrip", -1f, -1f, -1f);
		line.enabled = true;
		line.positionCount = this.lineSegmentCount;
		this.UpdateLine(line, target.HeadWorldPosition, base.transform.position);
		while (time <= duration)
		{
			float num = time / duration;
			position = Vector3.Lerp(startPos, base.transform.position, num);
			position.y = Mathf.Sin(num * 3.1415927f) * height;
			target.transform.position = position;
			this.UpdateLine(line, target.HeadWorldPosition, base.transform.position);
			yield return null;
			time += Time.deltaTime;
		}
		line.enabled = false;
		target.transform.position = target.transform.position.WithY(0f);
		target.RecalculatePath();
		yield break;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x000126F0 File Offset: 0x000108F0
	private void UpdateLine(LineRenderer line, Vector3 start, Vector3 end)
	{
		for (int i = 0; i < this.lineSegmentCount; i++)
		{
			float num = (float)i / (float)this.lineSegmentCount;
			this.linePos = Vector3.Lerp(start, end, num);
			this.linePos.y = Mathf.Sin(num * 3.1415927f) * this.grabHeight;
			line.SetPosition(i, this.linePos);
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00012754 File Offset: 0x00010954
	private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float d = t * t;
		return num * num * p0 + 2f * num * t * p1 + d * p2;
	}

	// Token: 0x0400046E RID: 1134
	[SerializeField]
	private List<LineRenderer> list_LineRenderers;

	// Token: 0x0400046F RID: 1135
	[SerializeField]
	private ParticleSystem particle_GrabEffect;

	// Token: 0x04000470 RID: 1136
	[SerializeField]
	private float grabDuration = 0.5f;

	// Token: 0x04000471 RID: 1137
	[SerializeField]
	private float grabHeight = 3f;

	// Token: 0x04000472 RID: 1138
	[SerializeField]
	private int lineSegmentCount = 10;

	// Token: 0x04000473 RID: 1139
	private Vector3 linePos;
}
