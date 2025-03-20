﻿namespace OrderAPI.Domain.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
