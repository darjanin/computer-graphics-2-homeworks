using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CG2.Mathematics
{
	public static class MathEx
	{
		public static Double DegToRad(Double angleDeg)
		{
			return Math.PI / 180 * angleDeg;
		}

		public static Double RadToDeg(Double angleRad)
		{
			return 180 / Math.PI * angleRad;
		}

	}
}
