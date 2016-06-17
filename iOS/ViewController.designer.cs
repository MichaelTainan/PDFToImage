// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PDFToImage.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIImageView imageView { get; set; }

		[Outlet]
		UIKit.UIToolbar pdfToolbar { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (pdfToolbar != null) {
				pdfToolbar.Dispose ();
				pdfToolbar = null;
			}

			if (imageView != null) {
				imageView.Dispose ();
				imageView = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}
