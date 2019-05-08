using MahApps.Metro.Controls;
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
using TopWord.Dapper;
using TopWord.Model;

namespace TopWord
{
    /// <summary>
    /// RiciteView.xaml 的交互逻辑
    /// </summary>
    public partial class RiciteView : MetroWindow
    {
        List<tb_word> list = new List<tb_word>();
        int i = 0;
        public RiciteView()
        {
            InitializeComponent();
            list = SQLiteUtil.Ins.Query<tb_word>("select * from tb_word where type=0 limit 100");
            txtword.Text = list[0].word;
            txtphonetic.Text = list[0].phonetic;
            txtmeaning.Text = list[0].meaning;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            i++;
            txtword.Text = list[i].word;
            txtphonetic.Text = "["+list[i].phonetic+"]";
            txtmeaning.Text = list[i].meaning;
          
        }
    }
}
