//
// Bindings for SKImageInfo
//
// Author:
//   Matthew Leibowitz
//
// Copyright 2017 Xamarin Inc
//

using System;
using System.Runtime.InteropServices;

namespace SkiaSharp
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct SKImageInfoNative
	{
		public IntPtr fColorSpace;
		public int fWidth;
		public int fHeight;
		public SKColorType fColorType;
		public SKAlphaType fAlphaType;

		public static void UpdateNative (ref SKImageInfo managed, ref SKImageInfoNative native)
		{
			native.fColorSpace = managed.ColorSpace == null ? IntPtr.Zero : managed.ColorSpace.Handle;
			native.fWidth = managed.Width;
			native.fHeight = managed.Height;
			native.fColorType = managed.ColorType;
			native.fAlphaType = managed.AlphaType;
		}

		public static SKImageInfoNative FromManaged (ref SKImageInfo managed)
		{
			return new SKImageInfoNative
			{
				fColorSpace = managed.ColorSpace == null ? IntPtr.Zero : managed.ColorSpace.Handle,
				fWidth = managed.Width,
				fHeight = managed.Height,
				fColorType = managed.ColorType,
				fAlphaType = managed.AlphaType,
			};
		}

		public static SKImageInfo ToManaged (ref SKImageInfoNative native)
		{
			return new SKImageInfo
			{
				ColorSpace = SKObject.GetObject<SKColorSpace> (native.fColorSpace),
				Width = native.fWidth,
				Height = native.fHeight,
				ColorType = native.fColorType,
				AlphaType = native.fAlphaType,
			};
		}
	}

	public struct SKImageInfo
	{
		public static readonly SKImageInfo Empty;
		public static readonly SKColorType PlatformColorType;
		public static readonly int PlatformColorAlphaShift;
		public static readonly int PlatformColorRedShift;
		public static readonly int PlatformColorGreenShift;
		public static readonly int PlatformColorBlueShift;

		private int width;
		private int height;
		private SKColorType colorType;
		private SKAlphaType alphaType;
		private SKColorSpace colorSpace;

		static SKImageInfo ()
		{
			PlatformColorType = SkiaApi.sk_colortype_get_default_8888 ();
			SkiaApi.sk_color_get_bit_shift (out PlatformColorAlphaShift, out PlatformColorRedShift, out PlatformColorGreenShift, out PlatformColorBlueShift);
		}

		public int Width { get { return width; } set { width = value; } }

		public int Height { get { return height; } set { height = value; } }

		public SKColorType ColorType { get { return colorType; } set { colorType = value; } }

		public SKAlphaType AlphaType { get { return alphaType; } set { alphaType = value; } }

		public SKColorSpace ColorSpace { get { return colorSpace; } set { colorSpace = value; } }

		public SKImageInfo (int width, int height)
		{
			this.width = width;
			this.height = height;
			this.colorType = PlatformColorType;
			this.alphaType = SKAlphaType.Premul;
			this.colorSpace = null;
		}

		public SKImageInfo (int width, int height, SKColorType colorType)
		{
			this.width = width;
			this.height = height;
			this.colorType = colorType;
			this.alphaType = SKAlphaType.Premul;
			this.colorSpace = null;
		}

		public SKImageInfo (int width, int height, SKColorType colorType, SKAlphaType alphaType)
		{
			this.width = width;
			this.height = height;
			this.colorType = colorType;
			this.alphaType = alphaType;
			this.colorSpace = null;
		}

		public SKImageInfo (int width, int height, SKColorType colorType, SKAlphaType alphaType, SKColorSpace colorspace)
		{
			this.width = width;
			this.height = height;
			this.colorType = colorType;
			this.alphaType = alphaType;
			this.colorSpace = colorspace;
		}

		public int BytesPerPixel {
			get {
				switch (ColorType) {
				case SKColorType.Unknown:
					return 0;
				case SKColorType.Alpha8:
				case SKColorType.Index8:
				case SKColorType.Gray8:
					return 1;
				case SKColorType.Rgb565:
				case SKColorType.Argb4444:
					return 2;
				case SKColorType.Bgra8888:
				case SKColorType.Rgba8888:
					return 4;
				case SKColorType.RgbaF16:
					return 8;
				}
				throw new ArgumentOutOfRangeException (nameof (ColorType));
			}
		}

		public int BitsPerPixel {
			get {
				switch (ColorType) {
				case SKColorType.Unknown:
					return 0;
				case SKColorType.Alpha8:
				case SKColorType.Index8:
				case SKColorType.Gray8:
					return 8;
				case SKColorType.Rgb565:
				case SKColorType.Argb4444:
					return 16;
				case SKColorType.Bgra8888:
				case SKColorType.Rgba8888:
					return 32;
				case SKColorType.RgbaF16:
					return 64;
				}
				throw new ArgumentOutOfRangeException (nameof (ColorType));
			}
		}

		public int BytesSize {
			get { return Width * Height * BytesPerPixel; }
		}

		public long BytesSize64 {
			get { return (long)Width * (long)Height * (long)BytesPerPixel; }
		}

		public int RowBytes {
			get { return Width * BytesPerPixel; }
		}

		public long RowBytes64 {
			get { return (long)Width * (long)BytesPerPixel; }
		}

		public bool IsEmpty {
			get { return Width <= 0 || Height <= 0; }
		}

		public bool IsOpaque {
			get { return AlphaType == SKAlphaType.Opaque; }
		}

		public SKSizeI Size {
			get { return new SKSizeI (Width, Height); }
		}

		public SKRectI Rect {
			get { return SKRectI.Create (Width, Height); }
		}
	}
}
