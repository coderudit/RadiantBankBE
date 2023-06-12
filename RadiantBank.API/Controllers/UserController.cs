using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RadiantBank.Application.UserFeature.Commands;
using RadiantBank.Application.UserFeature.Queries;
using RadiantBank.Domain.Models;

namespace RadiantBank.API.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ApiControllerBase
{
    #region "private variables"

    private readonly ILogger<UserController> _logger;

    #endregion


    #region Constructor

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Endpoints

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserCommand createUserCommand)
    {
        _logger.LogInformation(">>>CreateUserAsync Start");
        string userId = string.Empty;
        try
        {
            userId = await Mediator.Send(createUserCommand);
        }
        catch (ValidationException ex)
        {
            // var errors = ex.Errors.SelectMany(x => x.Value).ToList();
            // return new BadRequestObjectResult(Result.Failure(errors));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception while creating an user:{ex.Message} " +
                             $"at {DateTime.UtcNow.ToString("u", CultureInfo.GetCultureInfo("en-US"))}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create an user.");
        }
        finally
        {
            _logger.LogInformation("<<<CreateUserAsync End");
        }
        
        return Created(nameof(GetUserByIdAsync), new { userId });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
    {
        _logger.LogInformation(">>>GetAllUsersAsync Start");
        try
        {
            var result = await Mediator.Send(new GetUsersQuery());
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception while creating an user:{ex.Message} " +
                             $"at {DateTime.UtcNow.ToString("u", CultureInfo.GetCultureInfo("en-US"))}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create an user.");
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        _logger.LogInformation(">>>GetUserByIdQuery Start");
        try
        {
            var result = await Mediator.Send(new GetUserByIdQuery{UserId = userId});
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception while creating an user:{ex.Message} " +
                             $"at {DateTime.UtcNow.ToString("u", CultureInfo.GetCultureInfo("en-US"))}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create an user.");
        }
    }

    #endregion
}