
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Pdf;
using Android.Renderscripts;
using Android.Graphics.Drawables;

namespace PDFToImage.Droid
{
	public class PDFRendererFragment : Fragment
	{
		private static String STATE_CURRENT_PAGE_INDEX = "current_page_index";
		private ParcelFileDescriptor mFileDescriptor;

		private ImageView mImageView;
		private Button mButtonPrevious;
		private Button mButtonNext;

		public PDFRendererFragment()
		{
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		//public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
		//Bundle savedInstanceState) {
		//return inflater.Inflate(Resource.Layout.PDFRendererFragment, container, false);
		//}
	}
}

