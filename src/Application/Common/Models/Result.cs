using System.Collections.Generic;

namespace Application.Common.Models
{
    public interface IResult
    {
        IEnumerable<string> Errors { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out TData> : IResult
    {
        TData Data { get; }
    }

    public class Result : IResult
    {
        public Result() { }

        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public static Result Success()
        {
            return new Result { Succeeded = true };
        }

        public static Result Fail(IEnumerable<string> errors)
        {
            return new Result { Succeeded = false, Errors = errors };
        }
    }

    public class Result<TData> : Result, IResult<TData>
        where TData : class
    {
        public Result() { }

        public TData Data { get; set; }

        public static Result<TData> Success(TData data)
        {
            return new Result<TData> { Succeeded = true, Data = data };
        }

        public static Result<TData> Fail(IEnumerable<string> errors, TData data = null)
        {
            return new Result<TData> { Succeeded = false, Errors = errors, Data = data };
        }
    }
}
