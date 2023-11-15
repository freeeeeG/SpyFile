using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class Obj_SceneDecoration : MonoBehaviour
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060004AF RID: 1199 RVA: 0x00012EFB File Offset: 0x000110FB
	private float rangeWithScale
	{
		get
		{
			return this.range * base.transform.localScale.x;
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00012F14 File Offset: 0x00011114
	private void OnEnable()
	{
		EventMgr.Register<Obj_TetrisBlock>(eGameEvents.OnTetrisPlaced, new Action<Obj_TetrisBlock>(this.OnTetrisPlaced));
		if (this.isFlammable)
		{
			EventMgr.Register<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, new Action<Vector3, float>(this.OnPhysicsInteraction_Flame));
		}
		if (this.isExplodeable)
		{
			EventMgr.Register<Vector3, float>(eGameEvents.PhysicsInteraction_Explosion, new Action<Vector3, float>(this.OnPhysicsInteraction_Explosion));
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00012F80 File Offset: 0x00011180
	private void OnDisable()
	{
		EventMgr.Remove<Obj_TetrisBlock>(eGameEvents.OnTetrisPlaced, new Action<Obj_TetrisBlock>(this.OnTetrisPlaced));
		if (this.isFlammable)
		{
			EventMgr.Remove<Vector3, float>(eGameEvents.PhysicsInteraction_Flame, new Action<Vector3, float>(this.OnPhysicsInteraction_Flame));
		}
		if (this.isExplodeable)
		{
			EventMgr.Remove<Vector3, float>(eGameEvents.PhysicsInteraction_Explosion, new Action<Vector3, float>(this.OnPhysicsInteraction_Explosion));
		}
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x00012FEC File Offset: 0x000111EC
	private void OnTetrisPlaced(Obj_TetrisBlock block)
	{
		if (this.isDestroyed)
		{
			return;
		}
		if (Vector3.SqrMagnitude(block.transform.position - base.transform.position) < this.rangeWithScale * this.rangeWithScale)
		{
			base.StartCoroutine(this.DestroyProc());
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x00013040 File Offset: 0x00011240
	private void OnPhysicsInteraction_Flame(Vector3 pos, float effectRange)
	{
		if (this.isDestroyed)
		{
			return;
		}
		if (this.isBurning)
		{
			return;
		}
		if (Vector3.SqrMagnitude(pos - base.transform.position) < (effectRange + this.rangeWithScale) * (effectRange + this.rangeWithScale))
		{
			this.AttackedEffect(2f, 1.5f, 0f);
			base.StartCoroutine(this.CR_BurnEffect());
			this.isBurning = true;
		}
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000130B4 File Offset: 0x000112B4
	public void AttackedEffect(float duration, float strengthMultiplier, float delay)
	{
		base.transform.DOShakePosition(duration, strengthMultiplier * 0.1f, 10, 90f, false, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
		base.transform.DOShakeRotation(duration, strengthMultiplier * 5f, 10, 90f, true, ShakeRandomnessMode.Harmonic).SetDelay(delay);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00013108 File Offset: 0x00011308
	private IEnumerator CR_BurnEffect()
	{
		this.particle_Flame.Play();
		this.renderer.material = this.mat_Burning;
		this.burnTimeBeforeDestroy = Random.Range(this.burnTimeBeforeDestroy_Min, this.burnTimeBeforeDestroy_Max);
		while (this.burnTimer < this.burnTimeBeforeDestroy)
		{
			this.burnTimer += Time.deltaTime;
			yield return null;
		}
		this.particle_Flame.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		yield return new WaitForSeconds(1f);
		base.StartCoroutine(this.DestroyProc());
		yield break;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00013118 File Offset: 0x00011318
	private void OnPhysicsInteraction_Explosion(Vector3 pos, float effectRange)
	{
		if (this.isDestroyed)
		{
			return;
		}
		if (Vector3.SqrMagnitude(pos - base.transform.position) < (effectRange + this.rangeWithScale) * (effectRange + this.rangeWithScale))
		{
			base.StartCoroutine(this.DestroyProc());
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00013164 File Offset: 0x00011364
	private IEnumerator DestroyProc()
	{
		this.isDestroyed = true;
		this.node_Content.SetActive(false);
		this.particle_Destroy.Play();
		yield return new WaitForSeconds(this.destroyDelay);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000488 RID: 1160
	[SerializeField]
	private GameObject node_Content;

	// Token: 0x04000489 RID: 1161
	[SerializeField]
	private Renderer renderer;

	// Token: 0x0400048A RID: 1162
	[SerializeField]
	private bool isDestroyable = true;

	// Token: 0x0400048B RID: 1163
	[SerializeField]
	private bool isFlammable;

	// Token: 0x0400048C RID: 1164
	[SerializeField]
	private bool isExplodeable;

	// Token: 0x0400048D RID: 1165
	[SerializeField]
	private ParticleSystem particle_Destroy;

	// Token: 0x0400048E RID: 1166
	[SerializeField]
	private ParticleSystem particle_Flame;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	private float burnTimeBeforeDestroy_Min = 30f;

	// Token: 0x04000490 RID: 1168
	[SerializeField]
	private float burnTimeBeforeDestroy_Max = 60f;

	// Token: 0x04000491 RID: 1169
	[SerializeField]
	private Material mat_Burning;

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	private ParticleSystem particle_Explosion;

	// Token: 0x04000493 RID: 1171
	[SerializeField]
	private float range;

	// Token: 0x04000494 RID: 1172
	[SerializeField]
	private float destroyDelay = 3f;

	// Token: 0x04000495 RID: 1173
	private bool isDestroyed;

	// Token: 0x04000496 RID: 1174
	private bool isBurning;

	// Token: 0x04000497 RID: 1175
	private float burnTimer;

	// Token: 0x04000498 RID: 1176
	private float burnTimeBeforeDestroy;
}
