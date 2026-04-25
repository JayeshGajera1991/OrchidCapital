namespace Orchid.DataModels
{
    public class GetMasterListRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int IsActive { get; set; }
        public string SPName { get; set; }
    }
    public class DeleteMasterDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public string SPName { get; set; }
    }
    public class ChangeStatusInMasterDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string SPName { get; set; }
    }
    public class UpdatePageDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string PageName { get; set; }
    }
    public class UpdateConfigSettingsDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class UpdateImageConfigDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
    }
    public class UpdateTeamDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Qualification { get; set; }
        public string Headline { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
    }
    public class UpdateRoleDetailsRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public int Id { get; set; }
        public int IsActive { get; set; }
        public string Role { get; set; }
    }
    public class UpdateUserPageRightsMappingRequest
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string CreatedBy { get; set; }
        public string XmlData { get; set; }
    }
}