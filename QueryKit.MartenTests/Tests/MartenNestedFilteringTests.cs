using Bogus;
using FluentAssertions;
using QueryKit.MartenTests.Documents;
using System.Collections.Generic;
using System.Linq;
using Marten.Linq;
using QueryKit.Exceptions;
using Marten;

namespace QueryKit.MartenTests.Tests;

public class MartenNestedFilteringTests : TestBase
{
    [Fact]
    public async Task can_filter_by_guid_array_count_equals_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            AdditionalIds = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            AdditionalIds = [Guid.NewGuid(), Guid.NewGuid()]
        };

        Session.Store(docWithEmptyArray, docWithItems);
        await Session.SaveChangesAsync();

        var input = """AdditionalIds #== 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithEmptyArray.Id);
    }

    [Fact]
    public async Task can_filter_by_guid_array_count_greater_than_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            AdditionalIds = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            AdditionalIds = [Guid.NewGuid(), Guid.NewGuid()]
        };

        Session.Store(docWithEmptyArray, docWithItems);
        await Session.SaveChangesAsync();

        var input = """AdditionalIds #> 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithItems.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid_array_count_equals_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
        var docWithNull = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = null
        };

        Session.Store(docWithEmptyArray, docWithItems, docWithNull);
        await Session.SaveChangesAsync();

        var input = """NullableAdditionalIds #== 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithEmptyArray.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_guid_array_count_greater_than_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
        var docWithNull = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableAdditionalIds = null
        };

        Session.Store(docWithEmptyArray, docWithItems, docWithNull);
        await Session.SaveChangesAsync();

        var input = """NullableAdditionalIds #> 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithItems.Id);
    }

    [Fact]
    public async Task can_filter_by_string_array_count_equals_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = ["tag1", "tag2", "tag3"]
        };

        Session.Store(docWithEmptyArray, docWithItems);
        await Session.SaveChangesAsync();

        var input = """Tags #== 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithEmptyArray.Id);
    }

    [Fact]
    public async Task can_filter_by_string_array_count_greater_than_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = ["tag1", "tag2", "tag3"]
        };

        Session.Store(docWithEmptyArray, docWithItems);
        await Session.SaveChangesAsync();

        var input = """Tags #> 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithItems.Id);
    }

    [Fact]
    public async Task can_filter_by_string_array_count_equals_specific_value()
    {
        // Arrange
        var faker = new Faker();
        var docWithTwoTags = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = ["tag1", "tag2"]
        };
        var docWithThreeTags = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = ["tag1", "tag2", "tag3"]
        };
        var docWithOneTag = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Tags = ["tag1"]
        };

        Session.Store(docWithTwoTags, docWithThreeTags, docWithOneTag);
        await Session.SaveChangesAsync();

        var input = """Tags #== 2""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithTwoTags.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_string_array_count_equals_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = ["tag1", "tag2"]
        };
        var docWithNull = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = null
        };

        Session.Store(docWithEmptyArray, docWithItems, docWithNull);
        await Session.SaveChangesAsync();

        var input = """NullableTags #== 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithEmptyArray.Id);
    }

    [Fact]
    public async Task can_filter_by_nullable_string_array_count_greater_than_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyArray = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = ["tag1", "tag2"]
        };
        var docWithNull = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            NullableTags = null
        };

        Session.Store(docWithEmptyArray, docWithItems, docWithNull);
        await Session.SaveChangesAsync();

        var input = """NullableTags #> 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithItems.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_list_count_equals_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyList = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = [
                new NestedItem { Name = "Item1", Value = 10 },
                new NestedItem { Name = "Item2", Value = 20 }
            ]
        };

        Session.Store(docWithEmptyList, docWithItems);
        await Session.SaveChangesAsync();

        var input = """Items #== 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithEmptyList.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_list_count_greater_than_zero()
    {
        // Arrange
        var faker = new Faker();
        var docWithEmptyList = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = []
        };
        var docWithItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = [
                new NestedItem { Name = "Item1", Value = 10 },
                new NestedItem { Name = "Item2", Value = 20 }
            ]
        };

        Session.Store(docWithEmptyList, docWithItems);
        await Session.SaveChangesAsync();

        var input = """Items #> 0""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithItems.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_list_count_equals_specific_value()
    {
        // Arrange
        var faker = new Faker();
        var docWithTwoItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = [
                new NestedItem { Name = "Item1", Value = 10 },
                new NestedItem { Name = "Item2", Value = 20 }
            ]
        };
        var docWithThreeItems = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = [
                new NestedItem { Name = "Item1", Value = 10 },
                new NestedItem { Name = "Item2", Value = 20 },
                new NestedItem { Name = "Item3", Value = 30 }
            ]
        };
        var docWithOneItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            Items = [
                new NestedItem { Name = "Item1", Value = 10 }
            ]
        };

        Session.Store(docWithTwoItems, docWithThreeItems, docWithOneItem);
        await Session.SaveChangesAsync();

        var input = """Items #== 2""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithTwoItems.Id);
    }

    [Fact]
    public async Task can_filter_with_complex_conditions_using_count_operators()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has tags and items",
            Tags = ["tag1", "tag2"],
            Items = [
                new NestedItem { Name = "Item1", Value = 10 }
            ],
            AdditionalIds = [Guid.NewGuid()]
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "No tags but has items",
            Tags = [],
            Items = [
                new NestedItem { Name = "Item1", Value = 10 },
                new NestedItem { Name = "Item2", Value = 20 }
            ],
            AdditionalIds = []
        };
        var doc3 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Has tags but no items",
            Tags = ["tag1"],
            Items = [],
            AdditionalIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
        var doc4 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Empty everything",
            Tags = [],
            Items = [],
            AdditionalIds = []
        };

        Session.Store(doc1, doc2, doc3, doc4);
        await Session.SaveChangesAsync();

        var input = """(Tags #> 0 && Items #> 0) || (AdditionalIds #> 1)""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(2);
        results.Should().Contain(x => x.Id == doc1.Id); // Has tags and items
        results.Should().Contain(x => x.Id == doc3.Id); // Has AdditionalIds count > 1
        results.Should().NotContain(x => x.Id == doc2.Id);
        results.Should().NotContain(x => x.Id == doc4.Id);
    }
}

