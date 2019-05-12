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
using System.Windows.Forms;
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
        private NotifyIcon notifyIcon;
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "topword";
            this.ShowInTaskbar = false;
            this.Loaded += MainWindow_Loaded;
            Top();
            ToolBarSetting();
        }
        public int Group = 10;
        public int time = 5;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //string title = "";
            //Dictionary<string, int> dic = new Dictionary<string, int>();
            //List<tb_word> lista = SQLiteUtil.Ins.Query<tb_word>("select * from tb_word  ORDER BY seq");
            //foreach (var item in lista)
            //{
            //    if (item.word[0].ToString().ToLower()=="a")
            //    {
            //        title = "";
            //    }
            //    if (item.word.IndexOf('[')>=0)
            //    title += " " + item.word.Substring(0,item.word.IndexOf('['));
            //    else if (item.word.IndexOf('/') >= 0)
            //        title += " " + item.word.Substring(0, item.word.IndexOf('/'));
            //    else if (item.word.IndexOf('-') >= 0)
            //        title += " " + item.word.Substring(0, item.word.IndexOf('-'));
            //    else
            //        title += " " + item.word;

            //}
            //string tttt = title;
            //return;
            this.Topmost = true;
            List<tb_word> list = new List<tb_word>();
            Task.Run(() =>
            {
                list = SQLiteUtil.Ins.Query<tb_word>("select * from tb_word where type=0 ORDER BY seq,length(word)");
                while (list.Count > 0)
                {
                    List<tb_word> _currWord = list.GetRange(0, Group);
                    list.RemoveRange(0, Group);
                    SQLiteUtil.Ins.Update("update tb_word set type=1 where keyid=@keyid", _currWord);
                    for (int j = 0; j < time; j++)
                    {
                        for (int i = 0; i < _currWord.Count; i++)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                txtword.Text = _currWord[i].word;
                                txtphonetic.Text = " [" + _currWord[i].phonetic + "] ";
                                txtmeaning.Text = "";
                            });
                            Thread.Sleep(2000);
                            this.Dispatcher.Invoke(() =>
                            {
                                txtmeaning.Text = _currWord[i].meaning;
                            });
                            Thread.Sleep(1000);
                        }
                    }
                }
            });
        }
        #region ToolBar

        private void ToolBarSetting()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.BalloonTipText = "系统运行中... ...";
            this.notifyIcon.ShowBalloonTip(2000);
            this.notifyIcon.Text = "系统运行中... ...";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = true;
            //打开菜单项
            System.Windows.Forms.MenuItem open = new System.Windows.Forms.MenuItem("Open");
            open.Click += new EventHandler(Show);
            //打开菜单项
            System.Windows.Forms.MenuItem hide = new System.Windows.Forms.MenuItem("Hide");
            hide.Click += new EventHandler(Hide);
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { open, hide, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) this.Show(o, e);
            });
            //this.ShowInTaskbar = false;
            //this.Visibility = System.Windows.Visibility.Hidden;

        }

        private void Show(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            this.ShowInTaskbar = true;
            this.Activate();
            this.Show();
        }

        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            return;
            MessageBoxResult re = System.Windows.MessageBox.Show("关闭设备将会停运，您确定关闭吗？", "提示", MessageBoxButton.OKCancel);
            if (re == MessageBoxResult.OK)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            //SYLog();
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
            SetBottom();
            return;
            IntPtr CustomBar = (IntPtr)FindWindow(null, "TopWord");    //CustomBar是我的程序中需要置顶的窗体的名字
            if (CustomBar != null)
            {
                SetWindowPos(CustomBar, -1, 0, 0, 0, 0, 0);
            }
          
        }
        private void SetBottom( )
        {
            IntPtr CustomBar = (IntPtr)FindWindow(null, "TopWord");    //CustomBar是我的程序中需要置顶的窗体的名字
            if (CustomBar != null)
            {
                SetWindowPos(CustomBar, 1, 0, 0, 0, 0, 0x01|0x02|0x10);
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
