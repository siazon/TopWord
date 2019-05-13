using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TopWord.Dapper;
using TopWord.Model;

namespace TopWord
{
    /// <summary>
    /// ScreensaverView.xaml 的交互逻辑
    /// </summary>
    public partial class ScreensaverView : Window
    {
        List<tb_word> list = new List<tb_word>();
        public ScreensaverView()
        {
            InitializeComponent();
            this.KeyDown += ScreensaverView_KeyDown;
            list = SQLiteUtil.Ins.Query<tb_word>("select * from tb_word where type=0 limit 2000");
            Task.Run(() =>
            {
                for (int i = 0; i < list.Count; i++)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        txtword.Text = list[i].word;
                        txtphonetic.Text = "["+list[i].phonetic+"]";
                        txtmeaning.Text = list[i].meaning;
                    });

                    Thread.Sleep(3000);
                }

            });

        }

        private void ScreensaverView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Hide();
            }
        }
    }
}
