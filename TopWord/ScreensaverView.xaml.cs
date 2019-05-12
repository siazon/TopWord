using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TopWord
{
    /// <summary>
    /// ScreensaverView.xaml 的交互逻辑
    /// </summary>
    public partial class ScreensaverView : Window
    {
        public ScreensaverView()
        {
            InitializeComponent();
            this.KeyDown += ScreensaverView_KeyDown;
        }

        private void ScreensaverView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Escape)
            {
                this.Hide();
            }
        }
    }
}
