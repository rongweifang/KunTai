using System.Web;

namespace Common.DotNetBean
{
    public class RequestSession
    {
        private static string SESSION_USER = "SESSION_USER";
        private static string SESSION_PERSON = "SESSION_PERSON";

        public static void AddSessionUser(SessionUser user)
        {
            HttpContext rq = HttpContext.Current;
            rq.Session[RequestSession.SESSION_USER] = user;
        }

        public static SessionUser GetSessionUser()
        {
            HttpContext rq = HttpContext.Current;
            return (SessionUser)rq.Session[RequestSession.SESSION_USER];
        }
    }
}