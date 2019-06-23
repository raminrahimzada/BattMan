using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BattMan.Properties;

namespace BattMan
{
    public partial class FormMain : Form
    {
        private static FormMain _instance;
        private bool _allowExit;

        public static FormMain Instance => _instance ?? (_instance = new FormMain());

        public FormMain()
        {
            InitializeComponent();
            Load += FormMain_Load;
            Hide();
            Visible = false;
            _allowExit = false;
        }


        private void _HideBalloon()
        {
            Taskbar.Show();
            _allowExit = true;
            notifyIconMain.Visible = false;
            Hide();
        }
        public void HideBalloon()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(_HideBalloon));
            }
            else
            {
                _HideBalloon();
            }
        }

        private void _ShowBalloon( string description, bool highBattery)
        {
            _allowExit = false;
            notifyIconMain.Visible = true;
            notifyIconMain.Icon = Icon;
            notifyIconMain.Icon = highBattery ? Resources.battery_full_icon : Resources.battery_empty_icon; ;
            notifyIconMain.BalloonTipIcon = ToolTipIcon.Warning;
            notifyIconMain.BalloonTipTitle = description;
            notifyIconMain.BalloonTipText = description;
            notifyIconMain.ShowBalloonTip(3000);
            WindowState = FormWindowState.Maximized;
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            warningControlMain.Title = description;
            warningControlMain.LocateOnCenterOf(this);
            warningControlMain.HighBattery = highBattery;
            Show();
            Taskbar.Hide();
        }

        public void ShowBalloon( string description,bool highBattery)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    _ShowBalloon(description, highBattery);
                }));
            }
            else
            {
                _ShowBalloon(description, highBattery);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Opacity = 0.9;
            Resize += FormMain_Resize;
            TopMost = true;
            ShowInTaskbar = false;

            WindowState = FormWindowState.Maximized;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!_allowExit)
            {
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Maximized;
            }
        }
    }
}
