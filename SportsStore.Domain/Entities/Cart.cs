using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
    public class Cart
    {
        private List<CartLine> lineColletion = new List<CartLine>();

        public void AddItem (Product product,int quantity)
        {
            CartLine line = lineColletion
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();

            if (line == null)
            {
                lineColletion.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Product product)
        {
            lineColletion.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }
        public decimal ComputeTotalValue()
        {
            return lineColletion.Sum(e => e.Product.Price * e.Quantity);
        }
        
        public void Clear()
        {
            lineColletion.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineColletion; }
        }
    }
}
