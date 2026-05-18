using System.ComponentModel.DataAnnotations;

namespace desafio_02_crud_memoria;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null)
        {
            return Results.BadRequest(new { error = "Dados ausentes ou inválidos." });
        }

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(argument);

        bool isValid = Validator.TryValidateObject(argument, validationContext, validationResults, validateAllProperties: true);

        if (!isValid)
        {
            var errors = validationResults
                .GroupBy(e => e.MemberNames.FirstOrDefault() ?? string.Empty)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage ?? "Erro de validação").ToArray()
                );

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}