using System;

namespace Common
{
    public static class Require
    {
        public static void NotNull(object @object, string name)
        {
            if (@object == null)
            {
                throw new ArgumentNullException($"Parameter {name} can not be null");
            }
        }

        public static void NotEmpty(object @object, string name)
        {
            if (@object.Equals(string.Empty))
            {
                throw new ArgumentNullException($"String {name} can not be empty");
            }
        }

		public static void Positive(object @object, string name)
		{
            if (Math.Sign((decimal)@object) <= 0)
			{
				throw new ArgumentNullException($"Parameter {name} can not be non-positive");
			}
		}
    }
}