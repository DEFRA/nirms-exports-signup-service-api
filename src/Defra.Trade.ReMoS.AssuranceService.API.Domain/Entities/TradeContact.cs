using System.Text.Json.Serialization;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.Entities;

public class TradeContact
{
    public Guid Id { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid TradePartyId { get; set; }
    public string? PersonName { get; set; }
    public string? TelephoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Position { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsAuthorisedSignatory { get; set; }
    [JsonIgnore]
    public DateTime SubmittedDate { get; set; }
    [JsonIgnore]
    public DateTime LastModifiedDate { get; set; }
    [JsonIgnore]
    public Guid ModifiedBy { get; set; }
}
