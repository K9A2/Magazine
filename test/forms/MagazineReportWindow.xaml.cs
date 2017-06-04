using System.Data;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using test.entities;
using test.report;

namespace test.forms
{
    /// <summary>
    ///     MagazineReportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MagazineReportWindow
    {
        private readonly DataTable _dataTable;
        private readonly string _title;
        private readonly User _user;

        public MagazineReportWindow(DataTable dataTable, User user, string title)
        {
            InitializeComponent();

            _dataTable = dataTable;
            _user = user;
            _title = title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var report = new FullReport();
            var userName = (TextObject) report.ReportDefinition.ReportObjects["UserName"];
            var userId = (TextObject) report.ReportDefinition.ReportObjects["UserID"];
            var title = (TextObject) report.ReportDefinition.ReportObjects["title"];
            userName.Text = _user.UserName;
            userId.Text = _user.UserId;
            title.Text = "关键字“" + _title + "”的搜索结果";
            report.SetDataSource(_dataTable);
            Crv.ViewerCore.ReportSource = report;
        }
    }
}