using System;
using System.Collections.Generic;
using FX;
using GameResources;
using UnityEngine;

namespace Characters.Marks
{
	// Token: 0x02000817 RID: 2071
	public class Mark : MonoBehaviour
	{
		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06002A99 RID: 10905 RVA: 0x00083425 File Offset: 0x00081625
		// (set) Token: 0x06002A9A RID: 10906 RVA: 0x0008342D File Offset: 0x0008162D
		public Character owner { get; private set; }

		// Token: 0x06002A9B RID: 10907 RVA: 0x00083438 File Offset: 0x00081638
		public static Mark AddComponent(Character owner)
		{
			Mark mark = owner.gameObject.AddComponent<Mark>();
			mark.owner = owner;
			mark.owner.health.onDied += mark.ClearAllStack;
			return mark;
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x00083478 File Offset: 0x00081678
		public void AddStack(MarkInfo mark, float count = 1f)
		{
			if (this.owner.type == Character.Type.Trap)
			{
				return;
			}
			int num = this._markInfo.IndexOf(mark);
			int num2 = (int)count;
			float num3 = count - (float)num2;
			if (num == -1)
			{
				this._markInfo.Add(mark);
				this._stacks.Add(0f);
				Mark.stackImageBase.attachInfo = mark.attachInfo;
				EffectPoolInstance item = Mark.stackImageBase.Spawn(base.transform.position, this.owner, 0f, 1f);
				this._stackImages.Add(item);
				num = this._markInfo.Count - 1;
			}
			float num4 = this._stacks[num];
			if ((int)num4 == (int)(num4 + num3))
			{
				List<float> stacks = this._stacks;
				int index = num;
				stacks[index] += num3;
			}
			else
			{
				List<float> stacks = this._stacks;
				int index = num;
				stacks[index] += num3 - 1f;
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				if ((float)mark.maxStack <= this._stacks[num])
				{
					this._stacks[num] = (float)mark.maxStack;
					break;
				}
				List<float> stacks2 = this._stacks;
				int index = num;
				float num5 = stacks2[index] + 1f;
				stacks2[index] = num5;
				num4 = num5;
				MarkInfo.OnStackDelegate onStack = mark.onStack;
				if (onStack != null)
				{
					onStack(this, num4);
				}
			}
			num = this._markInfo.IndexOf(mark);
			if (num >= 0)
			{
				this.UpdateStackImage(this._stackImages[num], mark, this._stacks[num]);
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x0008361C File Offset: 0x0008181C
		public float GetStack(MarkInfo mark)
		{
			int num = this._markInfo.IndexOf(mark);
			if (num == -1)
			{
				return 0f;
			}
			return this._stacks[num];
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x0008364C File Offset: 0x0008184C
		public float TakeAllStack(MarkInfo mark)
		{
			int num = this._markInfo.IndexOf(mark);
			if (num == -1)
			{
				return 0f;
			}
			float result = this._stacks[num];
			this.ClearStack(num);
			return result;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x00083684 File Offset: 0x00081884
		public float TakeStack(MarkInfo mark, float count)
		{
			int num = this._markInfo.IndexOf(mark);
			if (num == -1)
			{
				return 0f;
			}
			float result;
			if (this._stacks[num] > count)
			{
				result = count;
				List<float> stacks = this._stacks;
				int index = num;
				stacks[index] -= count;
			}
			else
			{
				result = this._stacks[num];
				this._stacks[num] = 0f;
			}
			this.UpdateStackImage(this._stackImages[num], mark, this._stacks[num]);
			return result;
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x00083714 File Offset: 0x00081914
		public void ClearAllStack()
		{
			this._markInfo.Clear();
			this._stacks.Clear();
			foreach (EffectPoolInstance effectPoolInstance in this._stackImages)
			{
				effectPoolInstance.Stop();
			}
			this._stackImages.Clear();
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x00083788 File Offset: 0x00081988
		public void ClearStack(MarkInfo mark)
		{
			this.ClearStack(this._markInfo.IndexOf(mark));
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0008379C File Offset: 0x0008199C
		private void ClearStack(int index)
		{
			this._markInfo.RemoveAt(index);
			this._stacks.RemoveAt(index);
			this._stackImages[index].Stop();
			this._stackImages.RemoveAt(index);
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000837D4 File Offset: 0x000819D4
		private void UpdateStackImage(EffectPoolInstance effect, MarkInfo mark, float stacks)
		{
			if (stacks < 1f)
			{
				return;
			}
			int num = Mathf.Clamp((int)stacks - 1, 0, mark.stackImages.Length - 1);
			effect.renderer.sprite = mark.stackImages[num];
		}

		// Token: 0x0400244D RID: 9293
		private static readonly EffectInfo stackImageBase = new EffectInfo(CommonResource.instance.emptyEffect)
		{
			loop = true,
			flipXByOwnerDirection = false
		};

		// Token: 0x0400244E RID: 9294
		private readonly List<MarkInfo> _markInfo = new List<MarkInfo>();

		// Token: 0x0400244F RID: 9295
		private readonly List<float> _stacks = new List<float>();

		// Token: 0x04002450 RID: 9296
		private readonly List<EffectPoolInstance> _stackImages = new List<EffectPoolInstance>();
	}
}
