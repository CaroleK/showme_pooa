using Android.Content;
using Android.OS;
using ShowMe.Services;

namespace ShowMe.Droid
{
    [BroadcastReceiver]
    public class BackgroundReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string name = intent.GetStringArrayListExtra("name")[0];
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            wakeLock.Acquire();

            NotificationScheduler.ScheduleNotification(name); 

            wakeLock.Release();
        }
    }
}