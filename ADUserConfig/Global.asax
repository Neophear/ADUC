<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        RegisterRoutes(RouteTable.Routes);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
     {
        string url = HttpContext.Current.Request.Url.AbsolutePath;
        //if (string.IsNullOrEmpty(url) ||
        //    System.IO.Directory.Exists(Server.MapPath(url)))
        //    return;

        //if (url.EndsWith("/"))
        //{
        //    Response.Clear();
        //    Response.Status = "301 Moved Permanently";
        //    Response.AddHeader("Location", url.Substring(0, url.Length - 1));
        //    Response.End();
        //}

        if (!url.ToLower().EndsWith("default.aspx") && url.ToLower().EndsWith(".aspx"))
            Response.Redirect(url.Remove(url.Length - 5));
    }

    void RegisterRoutes(RouteCollection routes)
    {
        string[] pages = { "public/error", "public/Login", "BULK", "Feedback", "UserSettings", "ChangeLog", "admin/BULKs", "admin/BULKdelete", "admin/BULKOUmove", "admin/EditData", "admin/EditUsers", "admin/EditChangeLog", "admin/Feedback", "admin/Log" };

        foreach (string page in pages)
            routes.MapPageRoute(page,
                page,
                String.Format("~/{0}.aspx", page), true);

        routes.MapPageRoute("default",
            "default/{page}",
            "~/default.aspx", true,
            new RouteValueDictionary {
                { "page", "" } },
            new RouteValueDictionary {
                { "page", @"\d*" } });
        
        routes.MapPageRoute("EditADUser",
            "EditADUser/{username}",
            "~/EditADUser.aspx", true,
            new RouteValueDictionary {
                { "username", "" } },
            new RouteValueDictionary {
                { "username", ".*" } });

        routes.MapPageRoute("CreateADUser",
            "CreateADUser/{username}",
            "~/CreateADUser.aspx", true,
            new RouteValueDictionary {
                { "username", "" } },
            new RouteValueDictionary {
                { "username", ".*" } });

        routes.MapPageRoute("BULKPrint",
            "BULKPrint/{form}/{id}",
            "~/Print.aspx", true,
            new RouteValueDictionary {
                { "form", "list" },
                { "id", "" } },
            new RouteValueDictionary {
                { "form", @"(?:list|a5)?" },
                { "id", @"\d*" } });

        routes.MapPageRoute("Print",
            "Print/{username}/{password}",
            "~/Print.aspx", true,
            new RouteValueDictionary {
                { "form", "single" },
                { "username", "" },
                { "password", "" } },
            new RouteValueDictionary {
                { "form", "single" },
                { "username", @".*" },
                { "password", @".*" } });

        routes.MapPageRoute("admin/News",
            "admin/News/{id}",
            "~/admin/News.aspx", true,
            new RouteValueDictionary {
                { "id", "" } },
            new RouteValueDictionary {
                { "id", @"\d*" } });
    }
</script>
