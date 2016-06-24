using System;
using CoreGraphics;
using System.Collections.Generic;		
using System.IO;

using UIKit;
using Debug = System.Diagnostics.Debug;

namespace PDFToImage.iOS
{
	public partial class ViewController : UIViewController
	{
		private CGPDFDocument _pdf;
		private int _pageNumber;

		public int PageNumber
		{
			get { return this._pageNumber; }
			set
			{
				if (value >= 1 && value <= _pdf.Pages)
				{
					_pageNumber = value;
					this.View.SetNeedsDisplay();
				}
			}
		}

		public ViewController (IntPtr handle) : base (handle)
		{
			_pageNumber = 1;
			String filename = Path.Combine(NibBundle.ResourcePath, "InterViewer.pdf");
			_pdf = CGPDFDocument.FromFile(filename);
		    
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.View.BackgroundColor = UIColor.Gray;

			imageView.Image = GetThumbForPage();
			scrollView.ContentSize = imageView.Image.Size;


			pdfToolbar.Items[0].Clicked +=	delegate {
				this.PageNumber--;
				imageView.Image = GetThumbForPage();
				scrollView.ScrollRectToVisible(new CGRect(0,0,1,1),false);
			};

			pdfToolbar.Items[2].Clicked += delegate {
				this.PageNumber++;
				imageView.Image = GetThumbForPage();
				scrollView.ScrollRectToVisible(new CGRect(0, 0, 1, 1), false);
			};

		}

		public UIImage GetThumbForPage()
		{

			CGPDFPage pdfPg = _pdf.GetPage(PageNumber);
			nfloat scale;
			CGRect pageRect = pdfPg.GetBoxRect(CGPDFBox.Media);
			if (pageRect.Height > pageRect.Width)
			{
				/*nfloat swithWidth = 0.0f;
				swithWidth = pageRect.Width;
				pageRect.Width = pageRect.Height;
				pageRect.Height = swithWidth;*/
				scale = this.View.Frame.Width / pageRect.Width;
			}
			else {
				scale = this.View.Frame.Height / pageRect.Height;
			}

			pageRect.Size = new CGSize(pageRect.Width * scale, pageRect.Height * scale);

			UIGraphics.BeginImageContext(pageRect.Size);
			CGContext context = UIGraphics.GetCurrentContext();

			context.SetFillColor((nfloat)1.0, (nfloat)1.0, (nfloat)1.0, (nfloat)1.0);
			context.FillRect(pageRect);

			context.SaveState();

			context.TranslateCTM(0, pageRect.Size.Height);
			context.ScaleCTM(1, -1);
			/*if (pageRect.Width > pageRect.Height)
			{
				context.ConcatCTM(pdfPg.GetDrawingTransform(CGPDFBox.Crop, pageRect, 0, true));
				context.TranslateCTM(-100f, -100f);
			}*/

			context.ConcatCTM(CGAffineTransform.MakeScale(scale, scale));


			context.DrawPDFPage(pdfPg);
			context.RestoreState();

			UIImage thm = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return thm;
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			imageView.Image = GetThumbForPage();
			scrollView.ContentSize = imageView.Image.Size;
			//imageView.Image.DrawAsPatternInRect(new CGRect(0, 0, 1, 1));
		}

		public override void DidReceiveMemoryWarning ()
		{		
			base.DidReceiveMemoryWarning ();		
			// Release any cached data, images, etc that aren't in use.		
		}

	}
}
