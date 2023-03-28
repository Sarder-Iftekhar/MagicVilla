using System.Net;

namespace MagicVilla_web.Models
{
    public class APIResponse 
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Data { get; set; }
    }
}
