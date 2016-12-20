namespace Common.DotNetBean
{
    public class SessionUser
    {
        public object UserId
        {
            get;
            set;
        }

        public object UserAccount
        {
            get;
            set;
        }

        public object UserPwd
        {
            get;
            set;
        }

        public object UserName
        {
            get;
            set;
        }

        public object Organization_IDs
        {
            get;
            set;
        }

        public object OrganizationID
        {
            get;
            set;
        }
        public object Organization_Fax
        {
            get;
            set;
        }

        public SessionUser(object userId, object userAccount, object userPwd, object userName, object Organization_IDs, object Organization_Fax)
        {
            this.UserId = userId;
            this.UserAccount = userAccount;
            this.UserName = userName;
            this.UserPwd = userPwd;
            this.Organization_IDs = Organization_IDs;
            this.OrganizationID = OrganizationID;
            this.Organization_Fax = Organization_Fax;
        }

        public SessionUser()
        {
        }
    }
}