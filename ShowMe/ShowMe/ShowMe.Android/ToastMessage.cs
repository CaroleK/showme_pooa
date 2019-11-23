using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShowMe.Droid;
using ShowMe.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]

namespace ShowMe.Droid
{
    public class ToastMessage : IMessage
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}