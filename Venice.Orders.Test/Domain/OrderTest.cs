using Venice.Orders.Domain.Entities;
using Venice.Orders.Domain.Exceptions;
using Xunit;

namespace Venice.Orders.Test.Domain
{
    public class OrderTest
    {
        [Fact]
        public void AddItem_WhenQuantityIsZero()
        {
            int quantity = 1;
            decimal invalidPrice = -10.50m;
            string product = "Guitarra Fender";

            var order = new Order(Guid.NewGuid());

            var exception = Assert.Throws<DomainException>(() => 
                order.AddItem(product, quantity, invalidPrice));

            Assert.Equal("Preço deve ser positivo.", exception.Message);
        }

        [Fact]
        public void AddItem_DuplicateProduct()
        {
            int quantity = 1;
            decimal price = 5550;
            string product = "Guitarra Gibson";

            var order = new Order(Guid.NewGuid());

            var exception = Assert.Throws<DomainException>(() =>
            {
                order.AddItem(product, quantity, price);
                order.AddItem(product, quantity, price);
            });

            Assert.Equal("Produto já adicionado ao pedido.", exception.Message);
        }
    }
}