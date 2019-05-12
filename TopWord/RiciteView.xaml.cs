using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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



            IntPtr hWnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
            SetParent(hWnd, hWndProgMan);

            var handle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr hprog = FindWindowEx(
                FindWindowEx(
                    FindWindow("Progman", "Program Manager"),
                    IntPtr.Zero, "SHELLDLL_DefView", ""
                ),
                IntPtr.Zero, "SysListView32", "FolderView"
            );
            SetWindowLong(handle, 0, hprog);

            return;
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            SetParent(tWnd, pWnd);

     
        }



        //[DllImport("user32.dll", SetLastError = true)]
        //static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        //const int GWL_HWNDPARENT = -8;
        //[DllImport("user32.dll")]
        //static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);




        //[DllImport("user32.dll", EntryPoint = "SetParent")]
        //public static extern int SetParent(int hWndChild, int hWndNewParent);
        //[DllImport("user32.dll", EntryPoint = "FindWindow")]
        //public static extern int FindWindow(string lpClassName, string lpWindowName);
   

    }
}
