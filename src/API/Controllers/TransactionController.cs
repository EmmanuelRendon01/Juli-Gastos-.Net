using System.Security.Claims;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.UseCases.Transaction.Expense;
using Microsoft.AspNetCore.Mvc;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace JuliGastos.API.Controllers;

[Authorize]
public class TransactionController : BaseApiController
{
    private readonly ExpenseHandler _expenseHandler;

    public TransactionController(ExpenseHandler expenseHandler)
    {
        _expenseHandler = expenseHandler;
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> ExpenseAsync([FromBody] ExpenseCommand command)
    {
        try
        {
             var userId = GetUserIdFromToken();
             var response = await _expenseHandler.Handle(command, userId);
             return Created("201", response); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    private long GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return long.Parse(userIdClaim);
    }
}