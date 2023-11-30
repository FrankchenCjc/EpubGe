using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace EpubGe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SLipT(object sender, RoutedEventArgs e)
        {
            LogOut.Text = DateTime.Now.ToString() + ":开始作业";
            string RNove = string.Empty;
            try 
            {
                LogOut.Text = DateTime.Now.ToString() + ":正在读取原小说";

                 RNove = File.ReadAllText(OriginTxt.Text);
            }
            catch (Exception ex) 
            {
                LogOut.Text = DateTime.Now.ToString() + ":小说原文件读取错误"+ex.Message;
            }
            try
            {
                if (MarkPrx.Text == "") throw (new Exception("空的表达式"));
                ChaList.Items.Clear();
                int i = 0;
                foreach (Match Ma in Regex.Matches(RNove, MarkPrx.Text).Cast<Match>())
                {
                    i++;
                    ChaList.Items.Add("第" + i.ToString() + "章:  " + (Ma.Groups.Count == 2 ? Ma.Groups[1]:Ma.Value));
                }
                ToFile.IsEnabled = true;
                LogOut.Text = DateTime.Now.ToString() + ":章节分隔完成";
            }
            catch (Exception ex)
            {
                LogOut.Text = DateTime.Now.ToString() + ":正则故障" + ex.Message;
            }
        }

        private void ToFile_Click(object sender, RoutedEventArgs e)
        {
            LogOut.Text = DateTime.Now.ToString() + ":开始作业";
            string RNove = string.Empty;
            try
            {
                LogOut.Text = DateTime.Now.ToString() + ":正在读取原小说";

                RNove = File.ReadAllText(OriginTxt.Text);
            }
            catch (Exception ex)
            {
                LogOut.Text = DateTime.Now.ToString() + ":小说原文件读取错误" + ex.Message;
            }
            List<Match> Ma = Regex.Matches(RNove, MarkPrx.Text).ToList();
            int ind = 0, i = 0;
            string lt = "0", indexc = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset=\"utf-8\">\n<title>Index</title>\n</head>\n<body>\n<center><h1>Index</h1></center>\n";
            foreach (Match m in Ma)
            {
                LogOut.Text = DateTime.Now.ToString() + ":处理章节:  " + lt;
                string tc = RNove.Substring(ind, m.Index-ind);
                ind = m.Index + m.Length;
                string rc = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset=\"utf-8\">\n<title>"+lt+"</title>\n</head>\n<body>\n<h1>" + lt + "</h1>\n<p>"+
                    tc.Replace("\n", "<br/>\n")+ "</p>\n</body>\n</html>";
                try
                {
                    LogOut.Text = DateTime.Now.ToString() + ":写入文件:  " + OutputPath.Text + i.ToString() + ".html";
                    File.WriteAllText(OutputPath.Text +i.ToString()+".html", rc);
                }
                catch (Exception ex)
                {
                    LogOut.Text = DateTime.Now.ToString() + ":写入文件失败: "+ ex.Message;
                }
                indexc += "<p><a href=./" + i.ToString() + ".html>" + lt + "</a></p>\n";
                lt = m.Groups.Count == 2 ? m.Groups[1].Value : m.Value;
                i++;
            }
            indexc += "</body>\n</html>";
            LogOut.Text = DateTime.Now.ToString() + ":写入目录:  " + OutputPath.Text + "index.html";
            File.WriteAllText(OutputPath.Text + "index.html", indexc);
        }
    }
}
