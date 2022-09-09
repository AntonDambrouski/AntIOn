using Workout.Core.Models;

namespace Workout.Api.Wrappers;

public class Response<TData> 
{
    public TData? Data { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Error>? Errors { get; set; }
    public string Message { get; set; }

	public Response()
	{ }

	public Response(IEnumerable<Error> errors, string? message = null)
	{
		Errors = errors;
		Message ??= string.Empty;
		Success = false;
		Data = default;
	}

	public Response(TData data)
	{
		Data = data;
		Success = true;
		Message = string.Empty;
		Errors = null;
	}
}
