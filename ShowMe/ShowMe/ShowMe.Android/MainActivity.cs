using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Auth;
using Rg.Plugins.Popup.Services;
using Android.Content;
using Plugin.LocalNotifications;
using Xamarin.Forms;
using ShowMe.Views;

namespace ShowMe.Droid
{
    [Activity(Label = "ShowMe", Icon = "@drawable/eye_icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            CustomTabsConfiguration.CustomTabsClosingMessage = null;
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            LoadApplication(new App());

            // Change notification icon
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.eye_icon_round;

            // Once user is logged in
            MessagingCenter.Subscribe<LoginPage>(this, "UserLoggedIn", (obj) =>
            {
                // Handle the periodic background task for notifications: 
                // Every half-day, the application wakes up to check what are the upcoming shows and schedules notifications
                var alarmIntent = new Intent(this, typeof(BackgroundReceiver));
                alarmIntent.PutExtra("userId", App.User.Id);
                var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

                var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
                alarmManager.SetInexactRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 30 * 1000, AlarmManager.IntervalHalfDay, pending);
            });
            
        }

       

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override async void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                //Exit pop up page on back button device pressed
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                //Exit application
                Finish();
            }
        }
    }
}