using System.Web.Mvc;

namespace VilaStella.WebAdminClient.App_Start
{
    public class ViewEnginesConfig
    {
        public static void RegisterViewEngines(ViewEngineCollection engine)
        {
            engine.Clear();
            engine.Add(new RazorViewEngine());
        }
    }
}