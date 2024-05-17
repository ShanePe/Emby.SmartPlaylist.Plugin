using System;

namespace SmartPlaylist.Contracts
{
    [Serializable]
    public class ResponseDto<T>
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public T Response { get; set; }

        public static ResponseDto<T> CreateSuccess(T response)
        {
            return new ResponseDto<T>()
            {
                Success = true,
                Response = response
            };
        }

        public static ResponseDto<T> CreateError(string error)
        {
            return new ResponseDto<T>()
            {
                Success = false,
                Error = error
            };
        }
    }
}