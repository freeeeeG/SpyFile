using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200091D RID: 2333
[AddComponentMenu("KMonoBehaviour/scripts/Radiator")]
public class Radiator : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600439A RID: 17306 RVA: 0x0017AC90 File Offset: 0x00178E90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.emitter = new RadiationGridEmitter(Grid.PosToCell(base.gameObject), this.intensity);
		this.emitter.projectionCount = this.projectionCount;
		this.emitter.direction = this.direction;
		this.emitter.angle = this.angle;
		if (base.GetComponent<Operational>() == null)
		{
			this.emitter.enabled = true;
		}
		else
		{
			base.Subscribe(824508782, new Action<object>(this.OnOperationalChanged));
		}
		RadiationGridManager.emitters.Add(this.emitter);
	}

	// Token: 0x0600439B RID: 17307 RVA: 0x0017AD36 File Offset: 0x00178F36
	protected override void OnCleanUp()
	{
		RadiationGridManager.emitters.Remove(this.emitter);
		base.OnCleanUp();
	}

	// Token: 0x0600439C RID: 17308 RVA: 0x0017AD50 File Offset: 0x00178F50
	private void OnOperationalChanged(object data)
	{
		bool isActive = base.GetComponent<Operational>().IsActive;
		this.emitter.enabled = isActive;
	}

	// Token: 0x0600439D RID: 17309 RVA: 0x0017AD75 File Offset: 0x00178F75
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.EMITS_LIGHT, this.intensity), UI.GAMEOBJECTEFFECTS.TOOLTIPS.EMITS_LIGHT, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x0600439E RID: 17310 RVA: 0x0017ADAD File Offset: 0x00178FAD
	private void Update()
	{
		this.emitter.originCell = Grid.PosToCell(base.gameObject);
	}

	// Token: 0x04002CCA RID: 11466
	public RadiationGridEmitter emitter;

	// Token: 0x04002CCB RID: 11467
	public int intensity;

	// Token: 0x04002CCC RID: 11468
	public int projectionCount;

	// Token: 0x04002CCD RID: 11469
	public int direction;

	// Token: 0x04002CCE RID: 11470
	public int angle = 360;
}
