namespace ApartmentManagementSystem.Core.DTOs.ApartmentDto;

public class ApartmentGetAllResponseDto
{
    public int ApartmentId { get; set; }
    public string Block { get; set; }
    public bool Status { get; set; } // Bu alanı "Available" veya "Occupied" gibi string olarak da dönebilirsiniz.
    public string Type { get; set; } // Örneğin: "2+1"
    public int Floor { get; set; }
    public int Number { get; set; } // Daire numarası

    // İsteğe bağlı olarak kullanıcı bilgileri
    public Guid? UserId { get; set; }

    // Eğer ilgili faturaların bilgisi de listelenmek isteniyorsa:
    public List<InvoiceInfo> Invoices { get; set; }

    // Bu sınıfı fatura bilgilerini taşımak için kullanabilirsiniz
    public class InvoiceInfo
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }

}