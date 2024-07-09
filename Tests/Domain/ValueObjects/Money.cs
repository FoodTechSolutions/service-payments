using Domain.ValueObjects;

namespace Tests.Domain.ValueObjects;

[TestFixture]
public class MoneyTest
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var amount = new Money(100, "BRL");

        // Assert
        Assert.That(amount.Amount, Is.EqualTo(100));
        Assert.That(amount.Currency, Is.EqualTo("BRL"));
    }
}
