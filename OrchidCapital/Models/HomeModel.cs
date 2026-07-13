namespace OrchidCapital.Models
{
    public class HomeModel
    {

    }
    public class AllModuleListRequest
    {
        public List<ImageConfigurationModel> ImagesList { get; set; }
        public List<LoanTypeModel> ProductTypesList { get; set; }
        public List<BankServiceListModel> ProductsList { get; set; }
         public List<TeamModel> TeamsList { get; set; }
    }

}