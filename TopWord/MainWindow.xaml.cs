using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TopWord.Dapper;
using TopWord.Model;

namespace TopWord
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "topword";
            this.Loaded += MainWindow_Loaded;
            Top();
        }
        public int Group = 10;
        public int time = 5;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            List<tb_word> list = new List<tb_word>();
            Task.Run(() =>
            {
                list = SQLiteUtil.Ins.Query<tb_word>("select * from tb_word");
                List<tb_word> _currWord = list.GetRange(0, Group);
                for (int j = 0; j < time; j++)
                {
                    for (int i = 0; i < _currWord.Count; i++)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            txtword.Text = list[i].word;
                            txtphonetic.Text = " [" + list[i].phonetic + "] ";
                            txtmeaning.Text = list[i].meaning;
                        });
                        Thread.Sleep(3000);
                    }
                }
              
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SYLog();
            //List<tb_word> list= SQLiteUtil.Ins.Query<tb_word>("SELECT * from tb_word");
        }
        public void SYLog()
        {
            using (StreamReader sr = new StreamReader("E://Debug-20190404.log", Encoding.Default))
            {
                List<tb_word> list = new List<tb_word>();
                while (true)
                {
                    string sline = sr.ReadLine();
                    if (sline == null)
                    {
                        break;
                    }
                    if (sline.IndexOf(" ") >= 0)
                    {
                        tb_word word = new tb_word();
                        word.keyid = Guid.NewGuid().ToString("N");
                        word.word = sline.Substring(0, sline.IndexOf(" "));
                        if (sline.IndexOf('[') > 0 || sline.IndexOf(']') > 0)
                        {
                            word.phonetic = sline.Substring(sline.IndexOf('[') + 1, sline.IndexOf(']') - sline.IndexOf('[') - 1);
                            word.meaning = sline.Substring(sline.IndexOf(']') + 1, sline.Length - sline.IndexOf(']') - 1).Trim();
                        }
                        else
                        {
                            word.meaning = sline.Substring(sline.IndexOf(' ') + 1, sline.Length - sline.IndexOf(' ') - 1).Trim();
                        }
                        list.Add(word);
                    }
                    //string tem = GetValue(sline, "->", "。");

                    continue;
                }

                int succse = SQLiteUtil.Ins.Insert("INSERT into tb_word (keyid,word,phonetic,meaning,sentence) VALUES (@keyid,@word,@phonetic,@meaning,@sentence);", list);
            }
        }
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);//找应用

        [DllImport("user32", EntryPoint = "SetWindowPos")]
        private static extern int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);//把应用置顶
        public void Top()
        {

            IntPtr CustomBar = (IntPtr)FindWindow(null, "TopWord");    //CustomBar是我的程序中需要置顶的窗体的名字
            if (CustomBar != null)
            {
                SetWindowPos(CustomBar, -1, 0, 0, 0, 0,0);
            }
            return;
            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("TopWord");

            if (pro.Length > 0)
            {
                if (string.IsNullOrWhiteSpace(pro[0].MainWindowTitle))
                    return;
                IntPtr ptr = (IntPtr)FindWindow(null, pro[0].MainWindowTitle);
                if (ptr != IntPtr.Zero)
                {
                    SetWindowPos(ptr, -1, 0, 0, (int)this.Width, (int)this.Height, 0);//应用置顶
                }
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
