using System;
using System.Data;
using Microsoft.Reporting.WinForms;
using test.dataset;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Report.xaml 的交互逻辑
    /// </summary>
    public partial class Report
    {

        public Report(DataTable source, User user, String reportTitle)
        {
            InitializeComponent();

            PrintDataSet data = new PrintDataSet();
            DataTable table = Main.TableNameChnToEng(source.Copy());

            DataTableReader reader = new DataTableReader(table);

            ReportParameter title = new ReportParameter("ReportTitle", reportTitle);
            ReportParameter userName = new ReportParameter("UserName", user.UserName);
            ReportParameter userId = new ReportParameter("UserID", user.UserID);
            ReportParameter printTime = new ReportParameter("PrintTime", DateTime.Now.ToLongDateString());

            //DataTable table = Main.TableNameChnToEng(source.Clone());

            ReportViewer.LocalReport.DataSources.Clear();
            data.Magzine.Load(reader);

            ReportViewer.ProcessingMode = ProcessingMode.Local;
            ReportViewer.LocalReport.ReportPath = @"D:\Project\CS\Magazine\test\report\FullReport.rdlc";
            ReportDataSource dataSource = new ReportDataSource("DataSet1", data.Tables[0]);
            ReportViewer.LocalReport.DataSources.Add(dataSource);
            //ReportViewer.LocalReport.DataSources[0] = dataSource;
            ReportViewer.LocalReport.SetParameters(new[] { title, userName, userId, printTime });

            //ReportViewer.LocalReport.Refresh();
            ReportViewer.RefreshReport();

        }
    }
}
