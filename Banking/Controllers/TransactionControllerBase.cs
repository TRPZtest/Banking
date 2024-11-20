using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    public abstract class TransactionControllerBase : ControllerBase
    {
        protected async Task<IActionResult> ExecuteTransaction(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return UnprocessableEntity(new { message = ex.Message });
            }
        }
    }
}
