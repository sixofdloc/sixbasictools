using System;
namespace sixbasiclib
{
    public class Result<T>
    {
		public bool Success { get; set; }
        public T? ReturnValue { get; set; } = default;
        public string ErrorMessage { get; set; } = string.Empty;

		public Result()
        {
        }

		public Result(bool success, T returnValue, string errorMessage){
			Success = success;
			ReturnValue = returnValue;
			ErrorMessage = errorMessage;
		}
    }
}
