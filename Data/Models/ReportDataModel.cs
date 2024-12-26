namespace Sportify_Back.Models

{
    public class ReportDataModel
    {
        public string Title { get; set; }
        public IEnumerable<MonthlyClassReport> Reports { get; set; }
    }
}
