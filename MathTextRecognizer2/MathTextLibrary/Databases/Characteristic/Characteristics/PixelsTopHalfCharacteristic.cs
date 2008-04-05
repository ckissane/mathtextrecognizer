using System;

using MathTextLibrary.Bitmap;
using MathTextLibrary.Databases.Characteristic.Characteristics.Helpers;

namespace MathTextLibrary.Databases.Characteristic.Characteristics
{
	/// <summary>
	/// Esta caracteristica determina si el numero de pixeles negros en la mitad
	/// superior de la imagen es mayor al numero de pixeles negros en la mitad
	/// inferior.
	/// </summary>
	/// <seealso cref="MathTextLibrary.Characteristics.Helpers.CountPixelsHelper"/>
	public class PixelsTopHalfCharacteristic:IBinaryCharacteristic
	{
		public PixelsTopHalfCharacteristic()
		{
			priority=40;
		}

		public override bool Apply(MathTextBitmap image)
		{
			int npixelsTop=CountPixelsHelper.NumBlackPixelsInHalf(image, Half.Top);
			int npixelsBottom=CountPixelsHelper.NumBlackPixelsInHalf(image, Half.Bottom);
			
			return npixelsTop > npixelsBottom;
		}
	}
}