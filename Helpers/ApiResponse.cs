using Microsoft.AspNetCore.Mvc;

namespace APAERMENT_LAST_API.Helpers
{
    public static class ApiResponse
    {
        // 200 OK
        public static IActionResult Success(object data, string message = "Success")
        {
            return new OkObjectResult(new
            {
                Success = true,
                StatusCode = 200,
                Message = message,
                Data = data
            });
        }
    }
}
