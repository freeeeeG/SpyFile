using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000077 RID: 119
	[AddComponentMenu("Pathfinding/Modifiers/Advanced Smooth")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_advanced_smooth.php")]
	[Serializable]
	public class AdvancedSmooth : MonoModifier
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0002492B File Offset: 0x00022B2B
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00024930 File Offset: 0x00022B30
		public override void Apply(Path p)
		{
			Vector3[] array = p.vectorPath.ToArray();
			if (array == null || array.Length <= 2)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(array[0]);
			AdvancedSmooth.TurnConstructor.turningRadius = this.turningRadius;
			for (int i = 1; i < array.Length - 1; i++)
			{
				List<AdvancedSmooth.Turn> turnList = new List<AdvancedSmooth.Turn>();
				AdvancedSmooth.TurnConstructor.Setup(i, array);
				this.turnConstruct1.Prepare(i, array);
				this.turnConstruct2.Prepare(i, array);
				AdvancedSmooth.TurnConstructor.PostPrepare();
				if (i == 1)
				{
					this.turnConstruct1.PointToTangent(turnList);
					this.turnConstruct2.PointToTangent(turnList);
				}
				else
				{
					this.turnConstruct1.TangentToTangent(turnList);
					this.turnConstruct2.TangentToTangent(turnList);
				}
				this.EvaluatePaths(turnList, list);
				if (i == array.Length - 2)
				{
					this.turnConstruct1.TangentToPoint(turnList);
					this.turnConstruct2.TangentToPoint(turnList);
				}
				this.EvaluatePaths(turnList, list);
			}
			list.Add(array[array.Length - 1]);
			p.vectorPath = list;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00024A34 File Offset: 0x00022C34
		private void EvaluatePaths(List<AdvancedSmooth.Turn> turnList, List<Vector3> output)
		{
			turnList.Sort();
			for (int i = 0; i < turnList.Count; i++)
			{
				if (i == 0)
				{
					turnList[i].GetPath(output);
				}
			}
			turnList.Clear();
			if (AdvancedSmooth.TurnConstructor.changedPreviousTangent)
			{
				this.turnConstruct1.OnTangentUpdate();
				this.turnConstruct2.OnTangentUpdate();
			}
		}

		// Token: 0x04000381 RID: 897
		public float turningRadius = 1f;

		// Token: 0x04000382 RID: 898
		public AdvancedSmooth.MaxTurn turnConstruct1 = new AdvancedSmooth.MaxTurn();

		// Token: 0x04000383 RID: 899
		public AdvancedSmooth.ConstantTurn turnConstruct2 = new AdvancedSmooth.ConstantTurn();

		// Token: 0x02000147 RID: 327
		[Serializable]
		public class MaxTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x06000B27 RID: 2855 RVA: 0x00046984 File Offset: 0x00044B84
			public override void OnTangentUpdate()
			{
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x06000B28 RID: 2856 RVA: 0x00046A04 File Offset: 0x00044C04
			public override void Prepare(int i, Vector3[] vectorPath)
			{
				this.preRightCircleCenter = this.rightCircleCenter;
				this.preLeftCircleCenter = this.leftCircleCenter;
				this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius;
				this.preVaRight = this.vaRight;
				this.preVaLeft = this.vaLeft;
				this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.141592653589793;
			}

			// Token: 0x06000B29 RID: 2857 RVA: 0x00046AB4 File Offset: 0x00044CB4
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				this.alfaRightRight = base.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
				this.alfaLeftLeft = base.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
				this.alfaRightLeft = base.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
				this.alfaLeftRight = base.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
				double num = (double)(this.leftCircleCenter - this.preRightCircleCenter).magnitude;
				double num2 = (double)(this.rightCircleCenter - this.preLeftCircleCenter).magnitude;
				bool flag = false;
				bool flag2 = false;
				if (num < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag = true;
				}
				if (num2 < (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num2 = (double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag2 = true;
				}
				this.deltaRightLeft = (flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num)));
				this.deltaLeftRight = (flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num2)));
				this.betaRightRight = base.ClockwiseAngle(this.preVaRight, this.alfaRightRight - 1.5707963267948966);
				this.betaRightLeft = base.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
				this.betaLeftRight = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
				this.betaLeftLeft = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966);
				this.betaRightRight += base.ClockwiseAngle(this.alfaRightRight - 1.5707963267948966, this.vaRight);
				this.betaRightLeft += base.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
				this.betaLeftRight += base.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
				this.betaLeftLeft += base.CounterClockwiseAngle(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft);
				this.betaRightRight = base.GetLengthFromAngle(this.betaRightRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaRightLeft = base.GetLengthFromAngle(this.betaRightLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftRight = base.GetLengthFromAngle(this.betaLeftRight, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftLeft = base.GetLengthFromAngle(this.betaLeftLeft, (double)AdvancedSmooth.TurnConstructor.turningRadius);
				Vector3 a = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 a2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				Vector3 a3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 a4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				Vector3 b = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 b2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				Vector3 b3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793) * AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				Vector3 b4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				this.betaRightRight += (double)(a - b).magnitude;
				this.betaRightLeft += (double)(a2 - b2).magnitude;
				this.betaLeftRight += (double)(a3 - b3).magnitude;
				this.betaLeftLeft += (double)(a4 - b4).magnitude;
				if (flag)
				{
					this.betaRightLeft += 10000000.0;
				}
				if (flag2)
				{
					this.betaLeftRight += 10000000.0;
				}
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightRight, this, 2));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaRightLeft, this, 3));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftRight, this, 4));
				turnList.Add(new AdvancedSmooth.Turn((float)this.betaLeftLeft, this, 5));
			}

			// Token: 0x06000B2A RID: 2858 RVA: 0x00047004 File Offset: 0x00045204
			public override void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				double num = flag ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter);
				double num2 = flag ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude)));
				this.gammaRight = num + num2;
				double num3 = flag ? 0.0 : base.ClockwiseAngle(this.gammaRight, this.vaRight);
				double num4 = flag2 ? 0.0 : base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter);
				double num5 = flag2 ? 0.0 : (1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude)));
				this.gammaLeft = num4 - num5;
				double num6 = flag2 ? 0.0 : base.CounterClockwiseAngle(this.gammaLeft, this.vaLeft);
				if (!flag)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 0));
				}
				if (!flag2)
				{
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 1));
				}
			}

			// Token: 0x06000B2B RID: 2859 RVA: 0x000471A4 File Offset: 0x000453A4
			public override void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter).magnitude;
				float magnitude2 = (AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter).magnitude;
				if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				if (!flag)
				{
					double num = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter);
					double num2 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude));
					this.gammaRight = num - num2;
					double num3 = base.ClockwiseAngle(this.vaRight, this.gammaRight);
					turnList.Add(new AdvancedSmooth.Turn((float)num3, this, 6));
				}
				if (!flag2)
				{
					double num4 = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter);
					double num5 = 1.5707963267948966 - Math.Asin((double)(AdvancedSmooth.TurnConstructor.turningRadius / magnitude2));
					this.gammaLeft = num4 + num5;
					double num6 = base.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
					turnList.Add(new AdvancedSmooth.Turn((float)num6, this, 7));
				}
			}

			// Token: 0x06000B2C RID: 2860 RVA: 0x000472C4 File Offset: 0x000454C4
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				switch (turn.id)
				{
				case 0:
					base.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 1:
					base.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 2:
					base.AddCircleSegment(this.preVaRight, this.alfaRightRight - 1.5707963267948966, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightRight - 1.5707963267948966, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 3:
					base.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightLeft - this.deltaRightLeft + 3.141592653589793, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 4:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftRight + this.deltaLeftRight + 3.141592653589793, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 5:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 6:
					base.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				case 7:
					base.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
					return;
				default:
					return;
				}
			}

			// Token: 0x04000773 RID: 1907
			private Vector3 preRightCircleCenter = Vector3.zero;

			// Token: 0x04000774 RID: 1908
			private Vector3 preLeftCircleCenter = Vector3.zero;

			// Token: 0x04000775 RID: 1909
			private Vector3 rightCircleCenter;

			// Token: 0x04000776 RID: 1910
			private Vector3 leftCircleCenter;

			// Token: 0x04000777 RID: 1911
			private double vaRight;

			// Token: 0x04000778 RID: 1912
			private double vaLeft;

			// Token: 0x04000779 RID: 1913
			private double preVaLeft;

			// Token: 0x0400077A RID: 1914
			private double preVaRight;

			// Token: 0x0400077B RID: 1915
			private double gammaLeft;

			// Token: 0x0400077C RID: 1916
			private double gammaRight;

			// Token: 0x0400077D RID: 1917
			private double betaRightRight;

			// Token: 0x0400077E RID: 1918
			private double betaRightLeft;

			// Token: 0x0400077F RID: 1919
			private double betaLeftRight;

			// Token: 0x04000780 RID: 1920
			private double betaLeftLeft;

			// Token: 0x04000781 RID: 1921
			private double deltaRightLeft;

			// Token: 0x04000782 RID: 1922
			private double deltaLeftRight;

			// Token: 0x04000783 RID: 1923
			private double alfaRightRight;

			// Token: 0x04000784 RID: 1924
			private double alfaLeftLeft;

			// Token: 0x04000785 RID: 1925
			private double alfaRightLeft;

			// Token: 0x04000786 RID: 1926
			private double alfaLeftRight;
		}

		// Token: 0x02000148 RID: 328
		[Serializable]
		public class ConstantTurn : AdvancedSmooth.TurnConstructor
		{
			// Token: 0x06000B2E RID: 2862 RVA: 0x000474F0 File Offset: 0x000456F0
			public override void Prepare(int i, Vector3[] vectorPath)
			{
			}

			// Token: 0x06000B2F RID: 2863 RVA: 0x000474F4 File Offset: 0x000456F4
			public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
				Vector3 dir = Vector3.Cross(AdvancedSmooth.TurnConstructor.t1, Vector3.up);
				Vector3 vector = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.prev;
				Vector3 start = vector * 0.5f + AdvancedSmooth.TurnConstructor.prev;
				vector = Vector3.Cross(vector, Vector3.up);
				bool flag;
				this.circleCenter = VectorMath.LineDirIntersectionPointXZ(AdvancedSmooth.TurnConstructor.prev, dir, start, vector, out flag);
				if (!flag)
				{
					return;
				}
				this.gamma1 = base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
				this.gamma2 = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.circleCenter);
				this.clockwise = !VectorMath.RightOrColinearXZ(this.circleCenter, AdvancedSmooth.TurnConstructor.prev, AdvancedSmooth.TurnConstructor.prev + AdvancedSmooth.TurnConstructor.t1);
				double num = this.clockwise ? base.ClockwiseAngle(this.gamma1, this.gamma2) : base.CounterClockwiseAngle(this.gamma1, this.gamma2);
				num = base.GetLengthFromAngle(num, (double)(this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				turnList.Add(new AdvancedSmooth.Turn((float)num, this, 0));
			}

			// Token: 0x06000B30 RID: 2864 RVA: 0x00047620 File Offset: 0x00045820
			public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
			{
				base.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, (this.circleCenter - AdvancedSmooth.TurnConstructor.current).magnitude);
				AdvancedSmooth.TurnConstructor.normal = (AdvancedSmooth.TurnConstructor.current - this.circleCenter).normalized;
				AdvancedSmooth.TurnConstructor.t2 = Vector3.Cross(AdvancedSmooth.TurnConstructor.normal, Vector3.up).normalized;
				AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				if (!this.clockwise)
				{
					AdvancedSmooth.TurnConstructor.t2 = -AdvancedSmooth.TurnConstructor.t2;
					AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
				}
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
			}

			// Token: 0x04000787 RID: 1927
			private Vector3 circleCenter;

			// Token: 0x04000788 RID: 1928
			private double gamma1;

			// Token: 0x04000789 RID: 1929
			private double gamma2;

			// Token: 0x0400078A RID: 1930
			private bool clockwise;
		}

		// Token: 0x02000149 RID: 329
		public abstract class TurnConstructor
		{
			// Token: 0x06000B32 RID: 2866
			public abstract void Prepare(int i, Vector3[] vectorPath);

			// Token: 0x06000B33 RID: 2867 RVA: 0x000476E0 File Offset: 0x000458E0
			public virtual void OnTangentUpdate()
			{
			}

			// Token: 0x06000B34 RID: 2868 RVA: 0x000476E2 File Offset: 0x000458E2
			public virtual void PointToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x06000B35 RID: 2869 RVA: 0x000476E4 File Offset: 0x000458E4
			public virtual void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x06000B36 RID: 2870 RVA: 0x000476E6 File Offset: 0x000458E6
			public virtual void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
			{
			}

			// Token: 0x06000B37 RID: 2871
			public abstract void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output);

			// Token: 0x06000B38 RID: 2872 RVA: 0x000476E8 File Offset: 0x000458E8
			public static void Setup(int i, Vector3[] vectorPath)
			{
				AdvancedSmooth.TurnConstructor.current = vectorPath[i];
				AdvancedSmooth.TurnConstructor.prev = vectorPath[i - 1];
				AdvancedSmooth.TurnConstructor.next = vectorPath[i + 1];
				AdvancedSmooth.TurnConstructor.prev.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.next.y = AdvancedSmooth.TurnConstructor.current.y;
				AdvancedSmooth.TurnConstructor.t1 = AdvancedSmooth.TurnConstructor.t2;
				AdvancedSmooth.TurnConstructor.t2 = (AdvancedSmooth.TurnConstructor.next - AdvancedSmooth.TurnConstructor.current).normalized - (AdvancedSmooth.TurnConstructor.prev - AdvancedSmooth.TurnConstructor.current).normalized;
				AdvancedSmooth.TurnConstructor.t2 = AdvancedSmooth.TurnConstructor.t2.normalized;
				AdvancedSmooth.TurnConstructor.prevNormal = AdvancedSmooth.TurnConstructor.normal;
				AdvancedSmooth.TurnConstructor.normal = Vector3.Cross(AdvancedSmooth.TurnConstructor.t2, Vector3.up);
				AdvancedSmooth.TurnConstructor.normal = AdvancedSmooth.TurnConstructor.normal.normalized;
			}

			// Token: 0x06000B39 RID: 2873 RVA: 0x000477C3 File Offset: 0x000459C3
			public static void PostPrepare()
			{
				AdvancedSmooth.TurnConstructor.changedPreviousTangent = false;
			}

			// Token: 0x06000B3A RID: 2874 RVA: 0x000477CC File Offset: 0x000459CC
			public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
			{
				double num = 0.06283185307179587;
				if (clockwise)
				{
					while (endAngle > startAngle + 6.283185307179586)
					{
						endAngle -= 6.283185307179586;
					}
					while (endAngle < startAngle)
					{
						endAngle += 6.283185307179586;
					}
				}
				else
				{
					while (endAngle < startAngle - 6.283185307179586)
					{
						endAngle += 6.283185307179586;
					}
					while (endAngle > startAngle)
					{
						endAngle -= 6.283185307179586;
					}
				}
				if (clockwise)
				{
					for (double num2 = startAngle; num2 < endAngle; num2 += num)
					{
						output.Add(this.AngleToVector(num2) * radius + center);
					}
				}
				else
				{
					for (double num3 = startAngle; num3 > endAngle; num3 -= num)
					{
						output.Add(this.AngleToVector(num3) * radius + center);
					}
				}
				output.Add(this.AngleToVector(endAngle) * radius + center);
			}

			// Token: 0x06000B3B RID: 2875 RVA: 0x000478B8 File Offset: 0x00045AB8
			public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
			{
				double num = 0.06283185307179587;
				while (endAngle < startAngle)
				{
					endAngle += 6.283185307179586;
				}
				Vector3 start = this.AngleToVector(startAngle) * (float)radius + center;
				for (double num2 = startAngle + num; num2 < endAngle; num2 += num)
				{
					Debug.DrawLine(start, this.AngleToVector(num2) * (float)radius + center);
				}
				Debug.DrawLine(start, this.AngleToVector(endAngle) * (float)radius + center);
			}

			// Token: 0x06000B3C RID: 2876 RVA: 0x0004793C File Offset: 0x00045B3C
			public void DebugCircle(Vector3 center, double radius, Color color)
			{
				double num = 0.06283185307179587;
				Vector3 start = this.AngleToVector(-num) * (float)radius + center;
				for (double num2 = 0.0; num2 < 6.283185307179586; num2 += num)
				{
					Vector3 vector = this.AngleToVector(num2) * (float)radius + center;
					Debug.DrawLine(start, vector, color);
					start = vector;
				}
			}

			// Token: 0x06000B3D RID: 2877 RVA: 0x000479A4 File Offset: 0x00045BA4
			public double GetLengthFromAngle(double angle, double radius)
			{
				return radius * angle;
			}

			// Token: 0x06000B3E RID: 2878 RVA: 0x000479A9 File Offset: 0x00045BA9
			public double ClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(to - from);
			}

			// Token: 0x06000B3F RID: 2879 RVA: 0x000479B4 File Offset: 0x00045BB4
			public double CounterClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(from - to);
			}

			// Token: 0x06000B40 RID: 2880 RVA: 0x000479BF File Offset: 0x00045BBF
			public Vector3 AngleToVector(double a)
			{
				return new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
			}

			// Token: 0x06000B41 RID: 2881 RVA: 0x000479D9 File Offset: 0x00045BD9
			public double ToDegrees(double rad)
			{
				return rad * 57.295780181884766;
			}

			// Token: 0x06000B42 RID: 2882 RVA: 0x000479E6 File Offset: 0x00045BE6
			public double ClampAngle(double a)
			{
				while (a < 0.0)
				{
					a += 6.283185307179586;
				}
				while (a > 6.283185307179586)
				{
					a -= 6.283185307179586;
				}
				return a;
			}

			// Token: 0x06000B43 RID: 2883 RVA: 0x00047A1F File Offset: 0x00045C1F
			public double Atan2(Vector3 v)
			{
				return Math.Atan2((double)v.z, (double)v.x);
			}

			// Token: 0x0400078B RID: 1931
			public float constantBias;

			// Token: 0x0400078C RID: 1932
			public float factorBias = 1f;

			// Token: 0x0400078D RID: 1933
			public static float turningRadius = 1f;

			// Token: 0x0400078E RID: 1934
			public const double ThreeSixtyRadians = 6.283185307179586;

			// Token: 0x0400078F RID: 1935
			public static Vector3 prev;

			// Token: 0x04000790 RID: 1936
			public static Vector3 current;

			// Token: 0x04000791 RID: 1937
			public static Vector3 next;

			// Token: 0x04000792 RID: 1938
			public static Vector3 t1;

			// Token: 0x04000793 RID: 1939
			public static Vector3 t2;

			// Token: 0x04000794 RID: 1940
			public static Vector3 normal;

			// Token: 0x04000795 RID: 1941
			public static Vector3 prevNormal;

			// Token: 0x04000796 RID: 1942
			public static bool changedPreviousTangent = false;
		}

		// Token: 0x0200014A RID: 330
		public struct Turn : IComparable<AdvancedSmooth.Turn>
		{
			// Token: 0x1700018C RID: 396
			// (get) Token: 0x06000B46 RID: 2886 RVA: 0x00047A59 File Offset: 0x00045C59
			public float score
			{
				get
				{
					return this.length * this.constructor.factorBias + this.constructor.constantBias;
				}
			}

			// Token: 0x06000B47 RID: 2887 RVA: 0x00047A79 File Offset: 0x00045C79
			public Turn(float length, AdvancedSmooth.TurnConstructor constructor, int id = 0)
			{
				this.length = length;
				this.id = id;
				this.constructor = constructor;
			}

			// Token: 0x06000B48 RID: 2888 RVA: 0x00047A90 File Offset: 0x00045C90
			public void GetPath(List<Vector3> output)
			{
				this.constructor.GetPath(this, output);
			}

			// Token: 0x06000B49 RID: 2889 RVA: 0x00047AA4 File Offset: 0x00045CA4
			public int CompareTo(AdvancedSmooth.Turn t)
			{
				if (t.score > this.score)
				{
					return -1;
				}
				if (t.score >= this.score)
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06000B4A RID: 2890 RVA: 0x00047AC9 File Offset: 0x00045CC9
			public static bool operator <(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score < rhs.score;
			}

			// Token: 0x06000B4B RID: 2891 RVA: 0x00047ADB File Offset: 0x00045CDB
			public static bool operator >(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
			{
				return lhs.score > rhs.score;
			}

			// Token: 0x04000797 RID: 1943
			public float length;

			// Token: 0x04000798 RID: 1944
			public int id;

			// Token: 0x04000799 RID: 1945
			public AdvancedSmooth.TurnConstructor constructor;
		}
	}
}
