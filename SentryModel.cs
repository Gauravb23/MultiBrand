using System;

public class SentryModel
{
    public static void Configure()
    {
        var environment = ConfigurationManager.AppSettings["Environment"];

        //escape the method if we are in a development environment
        if (environment.Equals("development", StringComparison.CurrentCultureIgnoreCase))
            return;

        Assembly web = Assembly.GetExecutingAssembly();
        AssemblyName webName = web.GetName();
        string myVersion = webName.Version.ToString();
        string dsn_data = ConfigurationManager.ConnectionStrings["Sentry"].ConnectionString;

        using (SentrySdk.Init(o =>
        {
            o.Dsn = new Dsn(dsn_data);
            o.MaxBreadcrumbs = 50;
            o.Debug = true;
            o.Environment = environment;
            o.Release = myVersion;
            o.AttachStacktrace = true;
        })) 
        {
            // app code here
        }
    }
}
