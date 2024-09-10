using System.ComponentModel.DataAnnotations;

namespace Domain.Boundaries.Invoices.DeleteUser;

public record DeleteUserRequest(
    [Required] Guid UserId
);
