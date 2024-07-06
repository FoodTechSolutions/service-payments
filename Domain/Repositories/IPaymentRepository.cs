namespace Domain.Repositories;

public interface IPaymentRepository
{
    void Update(Payment payment);
    void Add(Payment payment);
    Payment GetById(Guid id);
}
