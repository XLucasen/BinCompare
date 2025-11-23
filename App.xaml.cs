using System.Configuration;
using System.Data;
using System.Windows;
using BinCompare.ViewModels;

namespace BinCompare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 应用启动事件处理
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 获取命令行参数
            string[] args = e.Args;

            // 创建主窗口
            MainWindow mainWindow = new MainWindow();
            
            // 如果有命令行参数，在窗口加载完成后处理文件
            if (args.Length > 0)
            {
                // 使用 ContentRendered 事件，确保窗口和 ViewModel 都已完全初始化
                mainWindow.ContentRendered += (s, ev) =>
                {
                    ProcessCommandLineArgs(mainWindow, args);
                };
            }
            
            mainWindow.Show();
        }

        /// <summary>
        /// 处理命令行参数中的文件
        /// </summary>
        private void ProcessCommandLineArgs(MainWindow mainWindow, string[] args)
        {
            try
            {
                if (mainWindow.DataContext is MainWindowViewModel viewModel)
                {
                    if (args.Length == 1)
                    {
                        // 单个文件：加载到文件A
                        viewModel.LoadFileDirectly(args[0], true);
                    }
                    else if (args.Length >= 2)
                    {
                        // 两个或更多文件：第一个加载到文件A，第二个加载到文件B
                        viewModel.LoadFileDirectly(args[0], true);
                        viewModel.LoadFileDirectly(args[1], false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理命令行参数时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
