using System.Text.RegularExpressions;

namespace OAuth2Provider
{
    public class Regexs
    {
        //sterling bug 6384
        //changed to only allow one "." and to match Visitor.EmailRegEx
        public static Regex EmailRegex = new Regex(@"^([-0-9a-zA-Z](\.?[-\w\+]*[-0-9a-zA-Z_])*@([0-9a-zA-Z]*[-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        public static Regex DigitRegex = new Regex(@"\d+");
        public static Regex GuidRegex = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
        public static Regex HTTPUrl = new Regex(@"(http|https):\/\/(?<subdomain>[\w\-_]+)(?<domain>(\.[\w\-_]+)+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", RegexOptions.IgnoreCase);
    }
}
