using System;
using CoreGraphics;
		
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
			_pdf = CGPDFDocument.FromFile("/Users/Michael/Documents/Projects/Mono/PDFToImage/iOS/goeckner-pres.pdf");
		    
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Perform any additional setup after loading the view, typically from a nib.
			this.View.BackgroundColor = UIColor.Gray;

			imageView.Image = GetThumbForPage();
			scrollView.ContentSize = imageView.Image.Size;

			var thisImageView = new UIImageView();
			thisImageView.Frame = new CoreGraphics.CGRect(740, 85, 225, 35);
			scrollView.InsertSubview(thisImageView, 1);

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

		public UIImage GetThumbForPage() {
			CGPDFPage pdfPage = _pdf.GetPage(PageNumber);
			CGRect pageRect = pdfPage.GetBoxRect(CGPDFBox.Media);
			//calculate fram and width scale
			nfloat scale = this.View.Frame.Width / pageRect.Width;
			pageRect.Size = new CGSize(pageRect.Width*scale, pageRect.Height *scale);

			//UIGraphics.BeginImageContext(aSize) is for creating graphics contexts at the UIKit level outside of UIView's drawRect: method.
			UIGraphics.BeginImageContext(pageRect.Size);
			CGContext context = UIGraphics.GetCurrentContext();

			context.SetFillColor((nfloat)1.0, (nfloat)1.0, (nfloat)1.0, (nfloat)1.0);
			context.FillRect(pageRect);
			//context.ConvertSizeToDeviceSpace(pageRect.Size);
			context.SaveState();

			Debug.WriteLine("getTranslatCTM=" + context.GetCTM());
			//context.ScaleCTM(50,50);
			context.TranslateCTM(0, pageRect.Size.Height);
			Debug.WriteLine("getTranslatCTM=" + context.GetCTM());
			//context.TranslateCTM(-0.5f*pageRect.Size.Width, -0.5f*pageRect.Size.Height);

			//context.RotateCTM(-(float)Math.PI / 2);
			Debug.WriteLine("getTranslatCTM=" + context.GetCTM());



			/*CGContextTranslateCTM(context, 0.5f * size.width, 0.5f * size.height);
			CGContextRotateCTM(context, radians(90));

            [image drawInRect:(CGRect){ { -size.width * 0.5f, -size.height * 0.5f }, size} ] ;*/

			context.ScaleCTM(1, -1);

			context.ConcatCTM(CGAffineTransform.MakeScale(scale, scale));


			context.DrawPDFPage(pdfPage);


			context.RestoreState();

			UIImage thm = UIGraphics.GetImageFromCurrentImageContext();

			//UIImage.FromImage(thm.CGImage, thm.CurrentScale, UIImageOrientation.Left);
			//RotateImage(thm, true);
			UIGraphics.EndImageContext();

			return thm;
			
		}

		public void RotateImage(UIImage imageToRotate, bool isCCW)
		{
			var imageRotation = isCCW ? UIImageOrientation.Right : UIImageOrientation.Left;
			imageToRotate = UIImage.FromImage(imageToRotate.CGImage, imageToRotate.CurrentScale, imageRotation);
	
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
