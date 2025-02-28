using System;
using System.Reflection;

namespace USDA.ARS.GRIN.GRINGlobal.API.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}