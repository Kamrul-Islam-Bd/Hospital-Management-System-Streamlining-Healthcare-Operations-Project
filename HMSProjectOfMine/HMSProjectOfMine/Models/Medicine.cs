namespace HMSProjectOfMine.Models
{
    public class Medicine
    {
        public int MedicineId { get; set; }
        public string MedicineType { get; set; }
        public string MedicineName { get; set; }
        public string GenericName { get; set; }
        public string Company { get; set; }

        public virtual ICollection<MedicinePurchase>? MedicinePurchases { get; set; } = new List<MedicinePurchase>();
        public virtual ICollection<MedicineSale>? MedicineSales { get; set; } = new List<MedicineSale>();
    }





    public class MedicinePurchase
    {
        public int MedicinePurchaseId { get; set; }
        public int MedicineId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalPrice { get; set; }
        public string Supplier { get; set; }

        public virtual Medicine? Medicine { get; set; }
        public virtual ICollection<MedicineLoss>? MedicineLosses { get; set; } = new List<MedicineLoss>();
    }





    public class MedicineSale
    {
        public int MedicineSaleId { get; set; }
        public int MedicineId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SalePrice { get; set; }
        public int QuantitySold { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; }

        public virtual Medicine? Medicine { get; set; }
        public virtual ICollection<MedicineProfit>? MedicineProfits { get; set; } = new List<MedicineProfit>();
    }





    public class MedicineProfit
    {
        public int MedicineProfitId { get; set; }
        public int MedicineSaleId { get; set; }
        public DateTime ProfitDate { get; set; }
        public decimal ProfitAmount { get; set; }
        public int QuantityProfit { get; set; }
        public decimal TotalProfit { get; set; }

        public virtual MedicineSale? MedicineSale { get; set; }
    }





    public class MedicineLoss
    {
        public int MedicineLossId { get; set; }
        public int MedicinePurchaseId { get; set; }
        public DateTime LossDate { get; set; }
        public int QuantityLoss { get; set; }
        public decimal LossAmount { get; set; }
        public decimal TotalLoss { get; set; }
        public string LossReason { get; set; }

        public virtual MedicinePurchase? MedicinePurchase { get; set; }
    }

}
