using System.ComponentModel.DataAnnotations;

namespace desafio_02_crud_memoria;

public record CreateProductCommand(
    [property: Required(ErrorMessage = "O nome do produto é obrigatório")]
    [property: StringLength(int.MaxValue, MinimumLength = 1)]
    string Name,

    [property: Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    decimal Price,

    int QuantityInStock
);
