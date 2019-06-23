using System.Windows.Forms;
using BattMan.Properties;

namespace BattMan
{
    public partial class WarningControl : UserControl
    {
        public WarningControl()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => labelDescription.Text;
            set => labelDescription.Text = value;
        }

        public bool HighBattery
        {
            get => labelDescription.Image == Resources.battery_full;
            set
            {
                if (value)
                {
                    labelDescription.Image = Resources.battery_full;
                }
                else
                {
                    labelDescription.Image = Resources.battery_empty;
                }
            }
        }

        public void LocateOnCenterOf(Control parent)
        {
            Left = (parent.ClientSize.Width - Width) / 2;
            Top = (parent.ClientSize.Height - Height) / 2;
        }
    }
}
