using System.Collections.Generic;
using System.Text;
namespace VilaStella.Web.Common.Helpers
{
    public static class UrlGenerator
    {
        public static string MakeUrl(string action, string controller)
        {
            return MakeUrlFromParameters(action, controller, null, null);
        }

        public static string MakeUrl(string action, string controller, string area)
        {
            return MakeUrlFromParameters(action, controller, area, null);
        }

        public static string MakeUrl(string action, string controller, string area, string firstParam)
        {
            return MakeUrlFromParameters(action, controller, area, new List<string> { firstParam });
        }

        public static string MakeUrl(string action, string controller, string area, string firstParam, string secondParam)
        {
            return MakeUrlFromParameters(action, controller, area, new List<string> { firstParam, secondParam });
        }

        public static string MakeUrl(string action, string controller, string area, List<string> parameters)
        {
            return MakeUrlFromParameters(action, controller, area, parameters);
        }

        private static string MakeUrlFromParameters(string action, string controller, string area, List<string> parameters)
        {
            StringBuilder url = new StringBuilder();

            if (area != null)
            {
                url.Append(area + '/');
            }

            url.Append(controller + '/' + action);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    url.Append('/' + parameter);
                }
            }

            return url.ToString();
        }
    }
}
