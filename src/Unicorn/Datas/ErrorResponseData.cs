namespace Unicorn.Datas
{
    public class ErrorResponseData : IErrorResponseData
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public void SetCode(int code)
        {
            Code = code;
        }

        public void SetMessage(string message)
        {
            Message = message;
        }
    }
}
