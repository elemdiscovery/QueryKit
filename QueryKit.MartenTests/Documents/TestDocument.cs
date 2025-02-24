namespace QueryKit.MartenTests.Documents;

public class TestDocument
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public Guid? RelatedId { get; set; }
}