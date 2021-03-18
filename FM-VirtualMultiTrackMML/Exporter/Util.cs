


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FM_VirtualMultiTrackMML
{
	public class Util {
		public class Sequence {
			List<Delivery.Part> maPart;
			int moPart = 0;
			int moPacket = -1;
			
			
			
			public Sequence(List<Delivery.Part> aPart)
			{
				maPart = aPart;
			}
			
			
			
			public bool IsTerm
			{
				get { return (moPart >= maPart.Count); }
			}
			
			
			
			public Delivery.Part Part
			{
				get { return (moPart < maPart.Count)? maPart[moPart]: null; }
			}
			
			
			
			public bool Pop(out Packet @Packet)
			{
				Packet = null;
				if (moPart < maPart.Count){
					if ((moPacket+1) < maPart[moPart].Packet.Count){
						Packet = maPart[moPart].Packet[++moPacket];
						return true;
					} else {
						++moPart;
						moPacket = -1;
						return Pop(out Packet);
					}
				}
				return false;
			}
			
			
			
			public bool Peek(out Packet @Packet)
			{
				Packet = null;
				if (!IsTerm){
					Packet = maPart[moPart].Packet[moPacket];
					return true;
				}
				return false;
			}
		}
		
		
		
		public class Volume {
			int mv = 0;
			
			
			
			public int Value
			{
				set { mv = value; }
				get { return mv; }
			}
			
			
			
			public int Clamp
			{
				get {
					var v = mv;
					v = (v >=  0)? v:  0;
					v = (v <= 15)? v: 15;
					return v;
				}
			}
		}
		
		
		
		public class Register {
			public class Param {
				int mValue = 0;
				bool mbModified = true;
				
				
				
				public int Value
				{
					set { mbModified = (mbModified || mValue != value); mValue = value; }
					get { mbModified = false; return mValue; }
				}
				
				
				
				public bool IsModified
				{
					get { return mbModified; }
				}
			}
			
			public Param FB = new Param();
			public Param LR = new Param();
			public Param AMS = new Param();
			public Param PMS = new Param();
			
			
			
			public class Operator {
				public Param AR = new Param();
				public Param DR = new Param();
				public Param SR = new Param();
				public Param RR = new Param();
				public Param SL = new Param();
				public Param TL = new Param();
				public Param KS = new Param();
				public Param SE = new Param();
				public Param AM = new Param();
			}
			
			public Operator[] aOP = new Operator[2]{
				new Operator(),
				new Operator(),
			};
		}
		
		
		
		public class Chord {
			int mTranspose;
			int mDetune;
			int mDT_Low;
			int mMT_Low;
			int mDT_High;
			int mMT_High;
			
			
			
			public Chord(int Transpose, int Detune, int DT_Low, int MT_Low, int DT_High, int MT_High)
			{
				mTranspose = Transpose;
				mDetune = Detune;
				mDT_Low = DT_Low;
				mMT_Low = MT_Low;
				mDT_High = DT_High;
				mMT_High = MT_High;
			}
			
			
			
			public int Transpose
			{
				get { return mTranspose; }
			}
			
			
			
			public int Detune
			{
				get { return mDetune; }
			}
			
			
			
			public int DT_Low
			{
				get { return mDT_Low; }
			}
			
			
			
			public int MT_Low
			{
				get { return mMT_Low; }
			}
			
			
			
			public int DT_High
			{
				get { return mDT_High; }
			}
			
			
			
			public int MT_High
			{
				get { return mMT_High; }
			}
		}
		
		
		
		public static readonly Chord[] s_aChord = new Chord[]{
			new Chord(0,0,	0,8,	0,8),	// 0	c	c
			new Chord(-10,9,3,14,	7,15),	// 1	c+	<b
			new Chord(0,0,	0,8,	5,9),	// 2	d	<a+
			new Chord(-4,4,	3,10,	7,12),	// 3	d+	<a
			new Chord(0,2,	7,8,	3,10),	// 4	e	<g+
			new Chord(5,0,	6,6,	0,8),	// 5	f	<g
			new Chord(-4,13,7,10,	3,14),	// 6	f+	<f+
			new Chord(0,0,	0,8,	5,12),	// 7	g	<f
			new Chord(8,5,	3,5,	7,8),	// 8	g+	<e
			new Chord(5,3,	7,6,	3,10),	// 9	a	<d+
			new Chord(8,2,	3,5,	7,9),	// 10	a+	<d
			new Chord(0,2,	7,8,	3,15),	// 11	b	<c+
			new Chord(12,0,	0,4,	0,8),	// 12	>c	<c
			new Chord(2,9,	3,7,	7,15),	// 13	>c+	<<b
			new Chord(12,0,	0,4,	5,9),	// 14	>d	<<a+
			new Chord(8,4,	3,5,	7,12),	// 15	>d+	<<a
			new Chord(12,2,	7,4,	3,10),	// 16	>e	<<g+
			new Chord(17,0,	6,3,	0,8),	// 17	>f	<<g
			new Chord(8,13,	7,5,	3,14),	// 18	>f+	<<f+
			new Chord(12,0,	0,4,	5,12),	// 19	>g	<<f
		};
	}
}
