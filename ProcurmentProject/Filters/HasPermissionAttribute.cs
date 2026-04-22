
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace ProcurmentProject.Filters
{
    public class HasPermissionAttribute : TypeFilterAttribute
    {
        public HasPermissionAttribute(string module, string action) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { module, action };
        }
    }
}
