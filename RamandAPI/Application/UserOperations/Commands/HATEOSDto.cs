namespace Application.UserOperations.Commands
{
    public class HATEOSDto
    {
        public string hrref { get; set; }
        public string Method { get; set; }
        public HATEOSDto()
        {

        }
        public HATEOSDto(string hrref, string method)
        {
            hrref = hrref;
            Method = method;
        }
    }
}
