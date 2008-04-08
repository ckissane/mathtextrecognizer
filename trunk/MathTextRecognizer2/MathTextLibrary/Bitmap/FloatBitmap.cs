// FloatImage.cs created with MonoDevelop
// User: luis at 15:56 07/04/2008

using System;
using System.Collections.Generic;

using Gdk;

namespace MathTextLibrary.Bitmap
{
	
	/// <summary>
	/// Esta clase implementa una imagen en formato de 
	/// </summary>
	public class FloatBitmap
	{
		float [,] image;
		
		/// <summary>
		/// Crea una nueva instancia de la clase <c>FloatBitmap</c>.
		/// </summary>
		/// <param name="witdh">
		/// La anchura del bitmap.
		/// </param>
		/// <param name="height">
		/// La altura del bitmap.
		/// </param>
		public FloatBitmap(int width, int height)
		{
			image = new float[width,height];
			
			for(int i= 0; i<width; i++)
			{
				for(int j=0; j<height; j++)
				{
					image[i,j] = MathTextBitmap.White;
				}
			}
		}
		
		/// <summary>
		/// Constructor copia de <c>FloatBitmap</c>.
		/// </summary>
		/// <param name="source">
		/// La instancia que se copiará.
		/// </param>
		public FloatBitmap(FloatBitmap source)
			: this(source.Width, source.Height)
		{
			for(int i=0; i<source.Width; i++)
			{
				for(int j=0;j<source.Height; j++)
				{
					this.image[i,j] = source.image[i,j];
				}
				
			}
		}
		
		/// <value>
		/// Contiene el tamaño horizontal de la imagen.
		/// </value>
		public int Width
		{
			get{
				return image.GetLength(0);
			}
		}
		
		
		/// <value>
		/// Contiene el tamaño vertical de la imagen.
		/// </value>
		public int Height
		{
			get{
				return image.GetLength(1);
			}
		}
		
		/// <summary>
		/// Permite acceder a las componentes de la imagen.
		/// </summary>
		public float this[int i, int j]
		{
			get
			{
				return image[i,j];
			}
			set
			{
				image[i,j] = value;
			}
		}
		
		/// <summary>
		/// Convierte la imagen pasada como array de float a un <c>Pixbuf</c>
		/// en escala de grises.
		/// </summary>		
		/// <returns>
		/// <c>Pixbuf</c> en escala de grises equivalente a <c>image</c>
		/// </returns>
		public Pixbuf CreatePixbuf()
		{
			int width = image.GetLength(0);
			int height = image.GetLength(1);
			
			// Creamos el Pixbuf, y lo rellenamos de blanco, así nos
			// aseguramos de que el array de píxeles esta creado.
			Pixbuf p = new Pixbuf(Colorspace.Rgb, false,8, width, height);
			p.Fill(0xFFFFFF);
			
			// Comprobamos el Rowstride del Pixbuf, para compensarlo.
			int rowstrideCompensation = p.Rowstride - 3 * width; 
			
			int k = 0;
			byte color;
			unsafe
			{
				byte* data = (byte*) p.Pixels;
				for(int j = 0; j < height; j++)
				{
					for(int i = 0; i < width; i++)					
					{
						color=(byte)(255*image[i,j]);
						
						/// Establecemos las tres componentes
						data[k] = color;
						data[k + 1] = color;
						data[k + 2] = color;						
						
						k += 3;
					}		
					
					k += rowstrideCompensation;		
				}	
			}
			return p;
		}
		
		/// <summary>
		/// Convierte el <c>Pixbuf</c> pasado como parámetro a un array de float.
		/// </summary>
		/// <param name="b">
		/// Imagen bitmap a convertir.
		/// </param>
		/// <returns>
		/// Array de float bidimensional conteniendo la misma
		/// informacion de pixeles que el bitmap original, pero en escala de
		/// grises, y de forma que la coordenada Y de la imagen se almacena
		/// en la primera componente del array.
		/// </returns>
		public static FloatBitmap CreateFromPixbuf(Pixbuf b)
		{
			
			FloatBitmap imageRes =new FloatBitmap(b.Width, b.Height);
			
			int pixelStep = b.NChannels;
			
			// Tenemos que compensar los pixeles que se añaden para tener
			// un rowstride optimo.			
			int rowstrideCompensation = b.Rowstride - pixelStep * b.Width;
			
			unsafe
			{
				byte* data = (byte*) b.Pixels;
				int k = 0;		
				float color;
				for(int j = 0; j < b.Height; j++)
				{
					for(int i = 0; i < b.Width; i++)					
					{
						// Usamos la formula para la luminosidad NTSC
						color = 
							data[k]*0.299f + data[k + 1]*0.587f + data[k + 2] * 0.114f;
						
						imageRes[i,j]= color/255.0f;
								
						k += pixelStep;		
					}
					
					k+= rowstrideCompensation;
				}
			
			}
			
			if(imageRes.Width != b.Width
				|| imageRes.Height != b.Height)
			{
				throw new Exception("Error al crear la matriz a partir del Pixbuf");
			}
		
			return imageRes;
		}
		
		/// <summary>
		/// Devuelve una sub imagene.
		/// </summary>
		/// <param name="x">Minima posicion horizontal de la subimagen.</param>
		/// <param name="y">Minima posicion vertical de la subimagen.</param>
		/// <param name="width">Anchura de la subimagen medida desde <c>x</c></param>
		/// <param name="height">Altura de la subimagen medida desde <c>y</c></param>
		/// <returns>Array de float que representa la subimagen deseada.</returns>
		public FloatBitmap SubImage(int x,int y,int width,int height)
		{
			FloatBitmap resImage=new FloatBitmap(width,height);
			
			for(int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					resImage[i,j] = this[x + i, y + j];
				}			
			}
			
			return resImage;
		}
		
		/// <summary>
		/// Gira 90 grados una imagen de forma no destructiva.
		/// </summary>
		/// <returns>
		/// La copia de la original, girada.
		/// </returns>
		public FloatBitmap Rotate90()
		{
			FloatBitmap res =  new FloatBitmap(this.Height, this.Width);
			
			for(int i = 0; i< this.Width; i++)
			{
				for(int j=0; j<this.Height; j++)
				{
					res[j, Width-i-1] = this[i, j]; 
				}
			}
			
			return res;
		}
		
		/// <summary>
		/// Escribe la imagen como una tabla de caracteres.
		/// </summary>
		/// <returns>
		/// La cadena de caracteres que representa a la instancia.
		/// </returns>
		public override string ToString ()
		{
			string res = "";
			for (int i = 0; i<this.Width; i++)
			{
				for(int j=0; j<this.Height; j++)
				{
					res+= this[i,j]+", ";
				}
				res+="\n";
			}
			
			return res;
		}
		
		/// <summary>
		/// Rellena una zona delimilitada de la imagen por un color, usando
		/// ese color.
		/// </summary>
		/// <param name="x">
		/// La coordenada x del punto inicial del relleno.
		/// </param>
		/// <param name="y">
		/// La coordenada y del punto inicial del relleno.
		/// </param>
		/// <param name="color">
		/// El color que delimita el relleno, y del que se rellena la imagen.
		/// </param>
		public void Fill(int x, int y, float color)
		{

			if(x>=0 
			   && x<Width 
			   && y>=0 
			   && y<Height 
			   && image[x,y]!=color)
			{
			   image[x,y] = color;
			   
			   Fill(x+1, y, color);
			   Fill(x-1, y, color);
			   Fill(x, y+1, color);
			   Fill(x, y-1, color);
			}
		}
	}
	
	
}
