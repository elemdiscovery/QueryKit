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

    [Fact]
    public async Task can_filter_by_nested_object_property_name()
    {
        // Arrange
        var faker = new Faker();
        var docWithNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "TestItem", Value = 10 }
        };
        var docWithDifferentName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "OtherItem", Value = 20 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithNestedItem, docWithDifferentName, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Name == "TestItem" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithNestedItem.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_property_value()
    {
        // Arrange
        var faker = new Faker();
        var docWithValue10 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "Item1", Value = 10 }
        };
        var docWithValue20 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "Item2", Value = 20 }
        };
        var docWithValue30 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "Item3", Value = 30 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithValue10, docWithValue20, docWithValue30, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Value > 15""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(2);
        results.Should().Contain(x => x.Id == docWithValue20.Id);
        results.Should().Contain(x => x.Id == docWithValue30.Id);
        results.Should().NotContain(x => x.Id == docWithValue10.Id);
        results.Should().NotContain(x => x.Id == docWithNullNestedItem.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_property_when_null()
    {
        // Arrange
        var faker = new Faker();
        var docWithNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "TestItem", Value = 10 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithNestedItem, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem == null""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithNullNestedItem.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_property_name_with_contains()
    {
        // Arrange
        var faker = new Faker();
        var docWithMatchingName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "TestItem123", Value = 10 }
        };
        var docWithNonMatchingName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "OtherItem", Value = 20 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithMatchingName, docWithNonMatchingName, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Name @= "Test" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(docWithMatchingName.Id);
    }

    [Fact]
    public async Task can_filter_with_complex_nested_object_conditions()
    {
        // Arrange
        var faker = new Faker();
        var doc1 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Doc1",
            SingleNestItem = new NestedItem { Name = "Item1", Value = 10 },
            Age = 25
        };
        var doc2 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Doc2",
            SingleNestItem = new NestedItem { Name = "Item2", Value = 20 },
            Age = 30
        };
        var doc3 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Doc3",
            SingleNestItem = new NestedItem { Name = "Item1", Value = 30 },
            Age = 25
        };
        var doc4 = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = "Doc4",
            SingleNestItem = null,
            Age = 25
        };

        Session.Store(doc1, doc2, doc3, doc4);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Name == "Item1" && SingleNestItem.Value > 15""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1);
        results[0].Id.Should().Be(doc3.Id);
        results.Should().NotContain(x => x.Id == doc1.Id); // Name matches but Value is 10, not > 15
        results.Should().NotContain(x => x.Id == doc2.Id); // Value > 15 but Name doesn't match
        results.Should().NotContain(x => x.Id == doc4.Id); // SingleNestItem is null
    }

    [Fact]
    public async Task can_filter_by_nested_object_property_name_not_null()
    {
        // Arrange
        var faker = new Faker();
        var docWithName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "TestItem", Value = 10 }
        };
        var docWithNullName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = null, Value = 20 }
        };
        var docWithEmptyName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "", Value = 30 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithName, docWithNullName, docWithEmptyName, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Name != null""";

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(2); // docWithName and docWithEmptyName (empty string is not null)
        results.Should().Contain(x => x.Id == docWithName.Id);
        results.Should().Contain(x => x.Id == docWithEmptyName.Id);
        results.Should().NotContain(x => x.Id == docWithNullName.Id);
        results.Should().NotContain(x => x.Id == docWithNullNestedItem.Id);
    }

    [Fact]
    public async Task can_filter_by_nested_object_property_name_not_empty()
    {
        // Arrange
        var faker = new Faker();
        var docWithName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "TestItem", Value = 10 }
        };
        var docWithNullName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = null, Value = 20 }
        };
        var docWithEmptyName = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = new NestedItem { Name = "", Value = 30 }
        };
        var docWithNullNestedItem = new TestDocument
        {
            Id = Guid.NewGuid(),
            Title = faker.Lorem.Sentence(),
            SingleNestItem = null
        };

        Session.Store(docWithName, docWithNullName, docWithEmptyName, docWithNullNestedItem);
        await Session.SaveChangesAsync();

        var input = """SingleNestItem.Name != "" """;

        // Act
        var queryable = Session.Query<TestDocument>();
        var appliedQueryable = queryable.ApplyQueryKitFilter(input);
        var results = appliedQueryable.ToList();

        // Assert
        results.Count.Should().Be(1); // Only docWithName has a non-empty name
        results.Should().Contain(x => x.Id == docWithName.Id);
        results.Should().NotContain(x => x.Id == docWithNullName.Id);
        results.Should().NotContain(x => x.Id == docWithEmptyName.Id);
        results.Should().NotContain(x => x.Id == docWithNullNestedItem.Id);
    }
}

