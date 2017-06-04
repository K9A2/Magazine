using System;
using System.Data;
using Microsoft.Reporting.WinForms;
using test.dataset;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Report.xaml 的交互逻辑
    /// </summary>
    public partial class Report
    {
        public Report(DataTable source, User user, string reportTitle)
        {
            InitializeComponent();

            var data = new PrintDataSet();
            var table = Main.TableNameChnToEng(source.Copy());

            var reader = new DataTableReader(table);

            var title = new ReportParameter("ReportTitle", reportTitle);
            var userName = new ReportParameter("UserName", user.UserName);
            var userId = new ReportParameter("UserID", user.UserId);
            var printTime = new ReportParameter("PrintTime", DateTime.Now.ToLongDateString());

            //DataTable table = Main.TableNameChnToEng(source.Clone());

            ReportViewer.LocalReport.DataSources.Clear();
            data.Magzine.Load(reader);

            ReportViewer.ProcessingMode = ProcessingMode.Local;
            ReportViewer.LocalReport.ReportPath = @"D:\Project\CS\Magazine\test\report\FullReport.rdlc";
            var dataSource = new ReportDataSource("DataSet1", data.Tables[0]);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            //ReportViewer.LocalReport.DataSources[0] = dataSource;
            ReportViewer.LocalReport.SetParameters(new[] {title, userName, userId, printTime});

            //ReportViewer.LocalReport.Refresh();
            ReportViewer.RefreshReport();
        }
    }
}