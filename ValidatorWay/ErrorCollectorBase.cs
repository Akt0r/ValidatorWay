using System;

namespace ValidatorWay;

public abstract class ErrorCollectorBase
{
    protected readonly List<Error> _errors = [];
}
