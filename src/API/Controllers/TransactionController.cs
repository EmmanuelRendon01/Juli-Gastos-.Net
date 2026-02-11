using System.Security.Claims;
using JuliGastos.Application.Interfaces.Handlers.Transaction;
using JuliGastos.Application.Interfaces.Repositories;
using JuliGastos.Application.UseCases.Transaction.Expense;
using JuliGastos.Application.UseCases.Transaction.Income;
using Microsoft.AspNetCore.Mvc;
using JuliGastos.Domain.Exceptions;
using JuliGastos.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace JuliGastos.API.Controllers;

[Authorize]
public class TransactionController : BaseApiController
{
    private readonly IExpenseHandler _expenseHandler;
    private readonly IIncomeHandler _incomeHandler;

    public TransactionController(IExpenseHandler expenseHandler, IIncomeHandler incomeHandler)
    {
        _expenseHandler = expenseHandler;
        _incomeHandler = incomeHandler;
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseResponse>> ExpenseAsync([FromBody] ExpenseCommand command)
    {
        try
        {
             var userId = GetUserIdFromToken();
             var response = await _expenseHandler.Handle(command, userId);
             return StatusCode(201, response); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<IncomeResponse>> IncomeAsync([FromBody] IncomeCommand command)
    {
        try
        {
            var userId = GetUserIdFromToken();
            var response = await _incomeHandler.Handle(command, userId);
            return StatusCode(201, response); 
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