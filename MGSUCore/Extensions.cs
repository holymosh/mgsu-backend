using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MGSUCore
{
    public static class Extensions
    {
        public static ModelStateDictionary ClearError(this ModelStateDictionary m, string fieldName)
        {
            if (m.ContainsKey(fieldName))
            {
                m[fieldName].Errors.Clear();
                m[fieldName].ValidationState = ModelValidationState.Valid;
            }
            return m;
        }
    }
}