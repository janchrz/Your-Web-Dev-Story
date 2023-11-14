namespace Gyvr.Mythril2D
{
    public static class StringFormatter
    {
        public static string Format(string input, params object[] args)
        {
            return Format(string.Format(input, args));
        }

        public static string GetTermDefinitionValue(TermDefinition definition, string member)
        {
            switch (member)
            {
                default:
                case "full":
                case "fullName": return definition.fullName;

                case "short":
                case "shortName": return definition.shortName;

                case "desc":
                case "description": return definition.description;
            }
        }

        public static string Format(string input)
        {
            GameConfig config = GameManager.Config;

            while (true)
            {
                int start = input.IndexOf('<');
                int end = input.IndexOf('>');

                if (start == -1 || end == -1) break;

                string pattern = input.Substring(start, end - start + 1);
                string content = pattern.Substring(1, pattern.Length - 2);
                string termID = content;
                string[] contentSplit = content.Split('.');
                string termMember = string.Empty;

                if (contentSplit.Length == 2)
                {
                    termID = contentSplit[0];
                    termMember = contentSplit[1];
                }

                TermDefinition definition = config.GetTermDefinition(termID);

                input = input.Replace(pattern, GetTermDefinitionValue(definition, termMember));
            }

            return input;
        }
    }
}
