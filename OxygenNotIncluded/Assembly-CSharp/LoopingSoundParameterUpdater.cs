using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public abstract class LoopingSoundParameterUpdater
{
	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06001C05 RID: 7173 RVA: 0x00095964 File Offset: 0x00093B64
	// (set) Token: 0x06001C06 RID: 7174 RVA: 0x0009596C File Offset: 0x00093B6C
	public HashedString parameter { get; private set; }

	// Token: 0x06001C07 RID: 7175 RVA: 0x00095975 File Offset: 0x00093B75
	public LoopingSoundParameterUpdater(HashedString parameter)
	{
		this.parameter = parameter;
	}

	// Token: 0x06001C08 RID: 7176
	public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x06001C09 RID: 7177
	public abstract void Update(float dt);

	// Token: 0x06001C0A RID: 7178
	public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x02001176 RID: 4470
	public struct Sound
	{
		// Token: 0x04005C61 RID: 23649
		public EventInstance ev;

		// Token: 0x04005C62 RID: 23650
		public HashedString path;

		// Token: 0x04005C63 RID: 23651
		public Transform transform;

		// Token: 0x04005C64 RID: 23652
		public SoundDescription description;

		// Token: 0x04005C65 RID: 23653
		public bool objectIsSelectedAndVisible;
	}
}
