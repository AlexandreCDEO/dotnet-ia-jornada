namespace desafio_03_ef_sqlite.Domain;

public record ProductRequest(string Name, decimal Price, int QuantityInStock);