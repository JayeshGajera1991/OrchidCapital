namespace OrchidCapital.Models
{
    public class DashboardModels
    {

    }
    public class MenuItemVm
    {
        public int? Id { get; set; }
        public string PageName { get; set; }
        public string Icon { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsAdd { get; set; }
        public bool IsView { get; set; }
        public string Section { get; set; } // Overview, Customer Management etc.
        public string IsSelected { get; set; }
    }
    public class GetToolsCountsListModel
    {
        public string MetricName { get; set; }
        public int TotalCount { get; set; }
        public string TotalPer { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
    }
    public class DashboardCommonModel
    {
        public List<GetToolsCountsListModel> ToolsCountsList { get; set; }
    }
}