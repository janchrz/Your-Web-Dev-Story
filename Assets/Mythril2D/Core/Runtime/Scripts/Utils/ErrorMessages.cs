namespace Gyvr.Mythril2D
{
    public static class ErrorMessages
    {
        public static string MissingComponentReference<Type>()
        {
            return string.Format("Missing component reference of type [{0}].", typeof(Type).Name);
        }

        public static string InspectorMissingComponentReference<Type>()
        {
            return string.Format("Missing component reference of type [{0}]. Make sure the reference has been provided in the inspector.", typeof(Type).Name);
        }
    }
}