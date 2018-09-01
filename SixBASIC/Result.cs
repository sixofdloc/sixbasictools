using System;
namespace SixBASIC
{
    public class Result
    {
		public bool Success { get; set; }
        public Object ReturnValue { get; set; }
        public string ErrorMessage { get; set; }

		public Result()
        {
        }

		public Result(bool success, Object returnValue, string errorMessage){
			Success = success;
			ReturnValue = returnValue;
			ErrorMessage = errorMessage;
		}
    }
}
