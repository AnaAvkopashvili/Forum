namespace Forum.Application.Accounts.Requests
{
    public class ManageUserRolesRequestModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
