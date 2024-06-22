using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents
{
    internal class ModelCalismasi
    {
    }
    public class Cart
    {
        public int CartId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CustomerId { get; set; } //CutomerID olmayabilir
        //public Customer Customer {get; set;} //Sonradan eklenecek
        public ICollection<CartItem> CartItems { get; set; }
    }

    //CartItem oluşturmak için cart olmak zorunda
    //Cart sorgulamak zorundayız.
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        //public Product Products{get: set:}
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
    //Categoryde değişecek bi şey yok ıd ve isim sadece, 
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        //public ICollection<Product> Products { get; set; }
    }
    //Customer içinde yapacak bi şey yok, ekleme yapılabilir Adress ama burda adresleri çok olabilir oyüzden sipraiş oluştururken girsin şimdilik.
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public List<Adress> Adress {  get; set; } //Sonradan adress domaini oluşturacağız.
        //public ICollection<Order> Orders { get; set; }
    }
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; } //TotalAmount u ORderItemlerin fiyatlatını e adetlerini işlem yapıp o şekilde yazdıracağız, normalde el ile yazıyorduk
        //TotalAmount = OrderITems.Totalprice ların hepsini toplayacağız
        public string OrderStatus { get; set; }
        //public string BillingAdress { get; set; } //Fatura adresi teslimat adresi ile aynı olacak diye ekleme yapmıyoruz.
        public string ShippingAdress { get; set; }
        //public string PaymentMethod { get; set; } //PaymentMethod Şimdlik kapatıyoruz.
        public int CustomerId { get; set; }
        //public Customer Customer { get; set; } //Bunuda açmamız gerekcek. Sipariş oluştururken customerID yoksa kullanıcı bilgilerini alıp burda Customer oluşturmamız gerekecek.
        public ICollection<OrderItem> OrderItems { get; set; }
    }
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        //public Order Order { get; set; } //Bunu sileceğiz
        public int ProductId { get; set; }
        //public Product Product { get; set; } //Burda Productı göstermemiz lazım Id gelse bile 
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; } // Quantity* Product.Price
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        //public Category Category { get; set; } //Categoryi göstermek gerekebilir.
    }
}
