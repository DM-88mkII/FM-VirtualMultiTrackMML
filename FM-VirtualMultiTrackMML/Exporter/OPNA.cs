


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FM_VirtualMultiTrackMML
{
	public class OPNA
	{
		public static readonly byte s_Reg_KeyOn = 0x28;
		public static readonly byte[] s_aValue_Ch = new byte[]{
			0x00,	// Ch.0
			0x01,	// Ch.1
			0x02,	// Ch.2
			0x04,	// Ch.3
			0x05,	// Ch.4
			0x06,	// Ch.5
		};
		public static readonly byte[] s_aValue_KeyOn = new byte[]{
			0x30,	// Op1,Op2
			0xc0,	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_DT_MT = new byte[][]{
			new byte[]{0x30, 0x38},	// Op1,Op2
			new byte[]{0x34, 0x3c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_TL = new byte[][]{
			new byte[]{0x40, 0x48},	// Op1,Op2
			new byte[]{0x44, 0x4c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_KS_AR = new byte[][]{
			new byte[]{0x50, 0x58},	// Op1,Op2
			new byte[]{0x54, 0x5c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_AM_DR = new byte[][]{
			new byte[]{0x60, 0x68},	// Op1,Op2
			new byte[]{0x64, 0x6c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_SR = new byte[][]{
			new byte[]{0x70, 0x78},	// Op1,Op2
			new byte[]{0x74, 0x7c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_SL_RR = new byte[][]{
			new byte[]{0x80, 0x88},	// Op1,Op2
			new byte[]{0x84, 0x8c},	// Op3,Op4
		};
		
		public static readonly byte[][] s_aaReg_SSGEG = new byte[][]{
			new byte[]{0x90, 0x98},	// Op1,Op2
			new byte[]{0x94, 0x9c},	// Op3,Op4
		};
		
		public static readonly byte s_Reg_FB_AL = 0xb0;
		public static readonly byte s_Value_AL = 0x04;
		
		public static readonly byte s_Reg_LR_AMS_PMS = 0xb4;
		
		public static readonly byte[] s_aaReg_Block_FNumber = new byte[]{
			0xa0,	// Low
			0xa4,	// High
		};
		public static ushort[] s_aValue_Block_FNumber = new ushort[]{
		//	c       c+      d       d+      e       f       f+      g       g+      a       a+      b
			0x026a, 0x028f, 0x02b6, 0x02df, 0x030b, 0x0339, 0x036a, 0x039e, 0x03d5, 0x0410, 0x044e, 0x048f,	// o1
			0x0a6a, 0x0a8f, 0x0ab6, 0x0adf, 0x0b0b, 0x0b39, 0x0b6a, 0x0b9e, 0x0bd5, 0x0c10, 0x0c4e, 0x0c8f,	// o2
			0x126a, 0x128f, 0x12b6, 0x12df, 0x130b, 0x1339, 0x136a, 0x139e, 0x13d5, 0x1410, 0x144e, 0x148f,	// o3
			0x1a6a, 0x1a8f, 0x1ab6, 0x1adf, 0x1b0b, 0x1b39, 0x1b6a, 0x1b9e, 0x1bd5, 0x1c10, 0x1c4e, 0x1c8f,	// o4
			0x226a, 0x228f, 0x22b6, 0x22df, 0x230b, 0x2339, 0x236a, 0x239e, 0x23d5, 0x2410, 0x244e, 0x248f,	// o5
			0x2a6a, 0x2a8f, 0x2ab6, 0x2adf, 0x2b0b, 0x2b39, 0x2b6a, 0x2b9e, 0x2bd5, 0x2c10, 0x2c4e, 0x2c8f,	// o6
			0x326a, 0x328f, 0x32b6, 0x32df, 0x330b, 0x3339, 0x336a, 0x339e, 0x33d5, 0x3410, 0x344e, 0x348f,	// o7
			0x3a6a, 0x3a8f, 0x3ab6, 0x3adf, 0x3b0b, 0x3b39, 0x3b6a, 0x3b9e, 0x3bd5, 0x3c10, 0x3c4e, 0x3c8f,	// o8
		};
	}
}
