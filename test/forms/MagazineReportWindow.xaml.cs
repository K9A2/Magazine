using System.Data;
using System.Windows;
using CrystalDecisions.CrystalReports.Engine;
using test.entities;
using test.report;

namespace test.forms
{
    /// <summary>
    /// MagazineReportWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MagazineReportWindow : Window
    {

        DataTable dt = new DataTable();
        User user = new User();
        string title = "";

        public MagazineReportWindow(DataTable dt, User user,string title)
        {
            InitializeComponent();

            this.dt = dt;
            this.user = user;
            this.title = title;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FullReport report = new FullReport();
            TextObject userName = (TextObject)report.ReportDefinition.ReportObjects["UserName"];
            TextObject userID = (TextObject)report.ReportDefinition.ReportObjects["UserID"];
            TextObject title = (TextObject)report.ReportDefinition.ReportObjects["title"];
            userName.Text = user.UserName;
            userID.Text = user.UserID;
            title.Text = "关键字“"+this.title+"”的搜索结果";
            report.SetDataSource(dt);
            Crv.ViewerCore.ReportSource = report;

            /*
            PrintDataSet pds = new PrintDataSet();
            pds.Tables.Add(dt);
            CrystalDecisions.CrystalReports.Engine.ReportDocument reportDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            reportDoc.Load(@"D:\magazine\test\report\FullReport.rpt");
            reportDoc.SetDataSource(pds);
            this.crv.ViewerCore.ReportSource = reportDoc;
            */

            /*
            ReportDataSource reportDataSource = new ReportDataSource();

            reportDataSource.Name = "MagazineDataSet";
            reportDataSource.Value = dt;

            bookReportViewer.LocalReport.ReportPath = @"D:\magazine\test\report\FullReport.rdlc";
            bookReportViewer.LocalReport.DataSources.Add(reportDataSource);

            bookReportViewer.RefreshReport();
            */
        }
    }
}
