using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Win32;

namespace BattMan
{
    public static class Program
    {
        private static readonly Mutex AppMutex = new Mutex(true, "{1F6F0ACF-B9A1-55fd-18CF-72F04E6BDE8E}");
        public static void CheckBattery()
        {
            var setting = Setting.Instance;
            PowerStatus pw = SystemInformation.PowerStatus;
            var batteryStatus = pw.BatteryChargeStatus;
            var batteryLevel = (int) (100.0 * pw.BatteryLifePercent);
            var isBatteryCharging = (batteryStatus & BatteryChargeStatus.Charging) != 0;
            if (isBatteryCharging && batteryLevel >= setting.MaxBatteryLevel)
            {
                FormMain.Instance.ShowBalloon(
                    setting.MaxBatteryAlertMessage,true);
            }
            else if (!isBatteryCharging && batteryLevel <= setting.MinBatteryLevel)
            {
                FormMain.Instance.ShowBalloon(
                    setting.MinBatteryAlertMessage,false);
            }
            else
            {
                FormMain.Instance.HideBalloon();
            }
        }

        private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode != PowerModes.StatusChange) return;
            CheckBattery();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Run()
        {
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Taskbar.Show();
            Debug.WriteLine(Setting.Instance);
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CheckBattery();
            GlobalConfiguration.Configuration.UseMemoryStorage();
            //FormMain.Instance.ShowBalloon(Setting.Instance.MinBatteryAlertTitle, Setting.Instance.MinBatteryAlertMessage,false);
            using (new BackgroundJobServer())
            {
                RecurringJob.AddOrUpdate("Main", () => CheckBattery(), Cron.Minutely);
                Application.Run();
            }
        }
        public static void Main( )
        {
            if (AppMutex.WaitOne(TimeSpan.Zero, true))
            {
                Run();
                AppMutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Battman has already Started!");
            }
        }
        private static void OnProcessExit(object sender, EventArgs e)
        {
            Taskbar.Show();
        }
    }
}
