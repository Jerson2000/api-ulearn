

using Microsoft.AspNetCore.Mvc;
using ULearn.Api.Extensions;

namespace ULearn.Api.Controllers;


public abstract class BaseController : ControllerBase
{
    protected Guid UserId => User.GetUserIdSafe() ?? Guid.Empty;
}