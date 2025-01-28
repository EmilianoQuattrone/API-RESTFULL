using System.Net;

namespace Models.Response
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
            ErrorMesages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSucces { get; set; }

        public List<string> ErrorMesages { get; set; }

        public object Result { get; set; }
    }
}