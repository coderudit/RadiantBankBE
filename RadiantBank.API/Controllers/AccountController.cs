using System.Globalization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RadiantBank.Application.UserFeature.Commands;
using RadiantBank.Application.UserFeature.Queries;

namespace RadiantBank.API.Controllers;

[Route("api/v1/users/{userId}/accounts")]
[ApiController]
public class AccountController : ApiControllerBase
{
    #region "private variables"

    private readonly ILogger<AccountController> _logger;

    #endregion
    
    #region Constructor

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    #endregion
    
    [HttpPost]
    public async Task<IActionResult> CreateAccountAsync(CreateAccountCommand createAccountCommand)
    {
        _logger.LogInformation(">>>CreateAccountAsync Start");
        string accountId = string.Empty;
        try
        {
            accountId = await Mediator.Send(createAccountCommand);
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

        return Created(nameof(GetAccountAsync), new { id = accountId });
    }
    
    [HttpGet("{accountNumber}")]
    public async Task<IActionResult> GetAccountAsync(string userId, string accountNumber)
    {
        var account = await Mediator.Send(new GetAccountByIdQuery{UserId = userId, AccountNumber = accountNumber});
        return Ok(account);
    }
    
    [HttpDelete("{accountNumber}")]
    public async Task<IActionResult> DeleteAccountAsync(string accountNumber)
    {
        await Mediator.Send(new DeleteAccountCommand{AccountNumber = accountNumber});
        return NoContent();
    }
    
    [HttpPost("{accountNumber}/deposit")]
    public async Task<IActionResult> DepositFundsAsync([FromBody]DepositFundsCommand command)
    {
        var result = await Mediator.Send(command);
        return new JsonResult(result);
    }
    
    [HttpPost("{accountNumber}/withdraw")]
    public async Task<IActionResult> WithdrawFundsAsync([FromBody]WithdrawFundsCommand command)
    {
        var result = await Mediator.Send(command);
        return new JsonResult(result);
    }
}