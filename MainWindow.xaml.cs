using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BatchRename
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 获取的文件路径
        /// </summary>
        private static string filePath = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 选择路径
        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            filePath = OpenFolderDialog();
            this.txtAddress.Text = filePath;
        }
        #endregion

        #region 开始转换
        private void BtnBegin_Click(object sender, RoutedEventArgs e)
        {
            string newFilesPath = filePath + "\\转换后文件\\";
            if (!FileHelper.IsExistDirectory(newFilesPath))
            {
                FileHelper.CreateDirectory(newFilesPath);
            }
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                List<string> list = FileHelper.GetFileNames(filePath).ToList();
                int count = 1;
                foreach (var item in list)
                {
                    //获取文件路径分组
                    string[] files = item.Split('\\');
                    //获取实际文件名
                    string fileName = files[files.Length - 1];

                    string newFileName = count.ToString() + "." + fileName;
                    try
                    {
                        File.Copy(item, newFilesPath + newFileName);
                        count++;
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                MessageBox.Show("转换成功！");
                System.Diagnostics.Process.Start(newFilesPath);
            }
            else
            {
                MessageBox.Show("请选择要改名的文件夹路径");
            }
        }
        #endregion

        #region  重置
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtAddress.Text = "";
            filePath = "";
        }
        #endregion

        #region 支持文件拖拽获取路径
        /// <summary>
        /// 支持文件拖拽获取路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMain_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                filePath = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
            }
        }
        #endregion

        #region 打开文件对话框
        /// <summary>
        /// 打开文件对话框
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>返回文件路径</returns>
        public static string OpenFolderDialog()
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult dr = folder.ShowDialog();//打开文件夹会话

            if (dr.ToString() == "OK")
                return folder.SelectedPath;//获取文件夹路径
            else
                return "";
        }
        #endregion
    }
}
