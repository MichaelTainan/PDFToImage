﻿using Android.App;
using Android.Widget;
using Android.OS;
using SignaturePad;
using Android.Views;
using Android.Graphics;
using System;
using Android.Graphics.Pdf;
using Com.Artifex.Mupdfdemo;
using System.IO;
using Android.Content;
using Android.Content.PM;
using Android.Util;
using Debug = System.Diagnostics.Debug;

namespace PDFToImage.Droid
{
	[Activity (Label = "PDFToImage", MainLauncher = true, Icon = "@mipmap/icon", ScreenOrientation = ScreenOrientation.Landscape)]
	public class MainActivity : Activity
	{
		//MuPDFCore pdf;
		PDFDocument pdf;
		Bitmap bitmap;
		int _pageNumber;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			//Xamarin.Insights.Initialize(global::PDFToImage.Droid.XamarinInsights.ApiKey, this);
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			ImageView iv = FindViewById<ImageView>(Resource.Id.imageView1);
			Button botton1 = FindViewById<Button>(Resource.Id.btn1);
			Button botton2 = FindViewById<Button>(Resource.Id.btn2);

			var dir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var pdfFilepath = System.IO.Path.Combine(dir, "0200B9.pdf");
			//pdf = new MuPDFCore(this, pdfFilepath);

			if (!File.Exists(pdfFilepath))
			{
				using (var source = Assets.Open(@"0200B9.pdf"))
				using (var dest = OpenFileOutput("0200B9.pdf", FileCreationMode.WorldReadable | FileCreationMode.WorldWriteable))
				{
					source.CopyTo(dest);
				}
			}

			//pdf = new MuPDFCore(this, pdfFilepath);
			pdf = new PDFDocument(this, pdfFilepath); 
			//var cookie = new MuPDFCore.Cookie(pdf.Doc);
			var count = pdf.Count;

			//	for (int i = 1; i < count; i++) {
			//var size = pdf.GetPageSize(0);
			//var size = pdf.GetPageSize (i);

			//int pageWidth = (int)size.X;
			//int pageHeight = (int)size.Y;

			//int ScreenWidth = pageWidth;
			//int ScreenHeight = pageHeight;

			//bitmap = Bitmap.CreateBitmap(ScreenWidth, ScreenHeight, Bitmap.Config.Argb8888);
			//pdf.DrawPage(bitmap, 0, pageWidth, pageHeight, 0, 0, ScreenWidth, ScreenHeight, cookie);
			//pdf.DrawPage (bitmap, i, pageWidth, pageHeight, 0, 0, ScreenWidth, ScreenHeight, cookie);
			//	}

			iv.SetImageBitmap(pdf.Images[0]);

			botton1.Click += (object sender, EventArgs e) => {
				this.PageNumber--;
				iv.SetImageBitmap(pdf.Images[PageNumber]);

			};

			botton2.Click += delegate {
				this.PageNumber++;
				iv.SetImageBitmap(pdf.Images[PageNumber]);

			};

		}

		public int PageNumber
		{
			get { return this._pageNumber; }
			set
			{
				if (value >= 0 && value <= pdf.Count-1)
				{
					_pageNumber = value;
				}
			}
		}

		/*public Bitmap GetThumbForPage()
		{
			
			var cookie = new MuPDFCore.Cookie(pdf);
			var count = pdf.CountPages();

			var size = pdf.GetPageSize(0);
			//var size = pdf.GetPageSize (i);

			int pageWidth = (int)size.X;
			int pageHeight = (int)size.Y;

			int ScreenWidth = pageWidth;
			int ScreenHeight = pageHeight;

			bitmap = Bitmap.CreateBitmap(ScreenWidth, ScreenHeight, Bitmap.Config.Argb8888);
			pdf.DrawPage(bitmap, PageNumber, pageWidth, pageHeight, 0, 0, ScreenWidth, ScreenHeight, cookie);
			return bitmap;
		}*/
	}
}


