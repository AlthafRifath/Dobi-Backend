using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("The value of a failed result cannot be accessed.");
                }

                return _value!;
            }
        }

        public static implicit operator Result<TValue>(TValue? value)
        {
            return value is not null
                ? Success(value)
                : Failure<TValue>(Error.NullValue);
        }
    }
}
