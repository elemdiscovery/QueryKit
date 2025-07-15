namespace QueryKit.MartenTests.Documents;

public class TestDocument
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid? RelatedId { get; set; }
    public Guid[] AdditionalIds { get; set; } = [];
    public Guid[]? NullableAdditionalIds { get; set; }
    public decimal Rating { get; set; }
    public int Age { get; set; }
    public BirthMonthEnum BirthMonth { get; set; }
    public DateTimeOffset? SpecificDate { get; set; }
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
}

public enum BirthMonthEnum
{
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
}