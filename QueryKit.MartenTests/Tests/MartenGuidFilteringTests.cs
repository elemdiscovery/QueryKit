using Bogus;
using FluentAssertions;
using QueryKit.MartenTests.Documents;
using System.Collections.Generic;
using System.Linq;
using Marten.Linq;

namespace QueryKit.MartenTests.Tests;

public class MartenGuidFilteringTests : TestBase
{
    [Fact]
    public async Task can_filter_by_guid()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""Id == "{doc1.Id}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc1.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid()
    {
        // Arrange
        var faker = new Faker();
        var relatedId = Guid.NewGuid();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = relatedId
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = null
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""RelatedId == "{relatedId}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc1.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid_is_null()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = Guid.NewGuid()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            RelatedId = null
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = """RelatedId == null""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc2.Id);
    }

    [Fact]
    public async Task can_filter_by_guid_contains()
    {
        // Arrange
        var faker = new Faker();
        var guidToFind = Guid.NewGuid();
        var doc1 = new TestDocument
        {
            Id = guidToFind,
            Title = faker.Lorem.Sentence()
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence()
        };

        Session.Store(doc1, doc2);
        await Session.SaveChangesAsync();

        var input = $"""Id @= "{guidToFind}" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(guidToFind);
    }
}