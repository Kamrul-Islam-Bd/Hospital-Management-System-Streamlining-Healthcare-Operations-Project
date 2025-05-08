namespace HMSProjectOfMine.DTOs;

public class MedicineDTO
{
    public int MedicineId { get; set; }
    public string MedicineType { get; set; }
    public string MedicineName { get; set; }
    public string GenericName { get; set; }
    public string Company { get; set; }
}

public class CreateMedicineDTO
{
    public string MedicineType { get; set; }
    public string MedicineName { get; set; }
    public string GenericName { get; set; }
    public string Company { get; set; }
}

public class UpdateMedicineDTO
{
    public int MedicineId { get; set; }
    public string MedicineType { get; set; }
    public string MedicineName { get; set; }
    public string GenericName { get; set; }
    public string Company { get; set; }
}





public class MedicinePurchaseDTO
{
    public int MedicinePurchaseId { get; set; }
    public int MedicineId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public int QuantityPurchased { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalPrice { get; set; }
    public string Supplier { get; set; }
    public MedicineDTO? Medicine { get; set; } // Updated to nullable
}

public class CreateMedicinePurchaseDTO
{
    public int MedicineId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public int QuantityPurchased { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalPrice { get; set; }
    public string Supplier { get; set; }
}

public class UpdateMedicinePurchaseDTO
{
    public int MedicinePurchaseId { get; set; }
    public int MedicineId { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public int QuantityPurchased { get; set; }
    public decimal VAT { get; set; }
    public decimal TotalPrice { get; set; }
    public string Supplier { get; set; }
}





public class MedicineSaleDTO
{
    public int MedicineSaleId { get; set; }
    public int MedicineId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
    public int QuantitySold { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomerName { get; set; }
    public MedicineDTO? Medicine { get; set; }  // Updated to nullable
}

public class CreateMedicineSaleDTO
{
    public int MedicineId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
    public int QuantitySold { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomerName { get; set; }
}

public class UpdateMedicineSaleDTO
{
    public int MedicineSaleId { get; set; }
    public int MedicineId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }
    public int QuantitySold { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomerName { get; set; }
}





public class MedicineProfitDTO
{
    public int MedicineProfitId { get; set; }
    public int MedicineSaleId { get; set; }
    public DateTime ProfitDate { get; set; }
    public decimal ProfitAmount { get; set; }
    public int QuantityProfit { get; set; }
    public decimal TotalProfit { get; set; }
    public MedicineSaleDTO? MedicineSale { get; set; }
}

public class CreateMedicineProfitDTO
{
    public int MedicineSaleId { get; set; }
    public DateTime ProfitDate { get; set; }
    public decimal ProfitAmount { get; set; }
    public int QuantityProfit { get; set; }
    public decimal TotalProfit { get; set; }
}

public class UpdateMedicineProfitDTO
{
    public int MedicineProfitId { get; set; }
    public int MedicineSaleId { get; set; }
    public DateTime ProfitDate { get; set; }
    public decimal ProfitAmount { get; set; }
    public int QuantityProfit { get; set; }
    public decimal TotalProfit { get; set; }
}





public class MedicineLossDTO
{
    public int MedicineLossId { get; set; }
    public int MedicinePurchaseId { get; set; }
    public DateTime LossDate { get; set; }
    public int QuantityLoss { get; set; }
    public decimal LossAmount { get; set; }
    public decimal TotalLoss { get; set; }
    public string LossReason { get; set; }
    public MedicinePurchaseDTO? MedicinePurchase { get; set; }
}

public class CreateMedicineLossDTO
{
    public int MedicinePurchaseId { get; set; }
    public DateTime LossDate { get; set; }
    public int QuantityLoss { get; set; }
    public decimal LossAmount { get; set; }
    public decimal TotalLoss { get; set; }
    public string LossReason { get; set; }
}

public class UpdateMedicineLossDTO
{
    public int MedicineLossId { get; set; }
    public int MedicinePurchaseId { get; set; }
    public DateTime LossDate { get; set; }
    public int QuantityLoss { get; set; }
    public decimal LossAmount { get; set; }
    public decimal TotalLoss { get; set; }
    public string LossReason { get; set; }
}

