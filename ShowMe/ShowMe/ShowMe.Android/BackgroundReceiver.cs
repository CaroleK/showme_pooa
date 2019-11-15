using Android.Content;
using Android.OS;
using ShowMe.Services;

namespace ShowMe.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        // This function is called everytime the alarm manager repeats
        public override async void OnReceive(Context context, Intent intent)
        {
            // Fetch the user id from the intent
            string userId = intent.GetStringExtra("userId");

            // Wake up the device
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");

            wakeLock.Acquire();

            // Schedule notifications
            await NotificationScheduler.ScheduleNotification(userId); 

            wakeLock.Release();
        }
    }
}