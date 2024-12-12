using practiceAPP.UserModels;
namespace parcticeAPP.ResponseModels
{
    public class LoginResponse
    {
        public string token { get; set; }
        public ExchangeUser user { get; set; }
    }

}
